Imports System.Globalization
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MySql.Data.MySqlClient

Public Class Pengeluaran

    Dim isEdit As Boolean = False
    Dim kNominal As Double = 0
    Dim kPengeluaran As String = ""
    Dim kStaf As String = ""
    Dim kTanggal As String = ""
    Dim kKeterangan As String = ""



    '=== Ambil nama dasar dari teks ComboBox (misal "SPP (1000000)" → "SPP") ===
    Function GetJenisDasar(textCombo As String) As String
        If textCombo.Contains("(") Then
            Return textCombo.Substring(0, textCombo.IndexOf("(")).Trim()
        End If
        Return textCombo.Trim()
    End Function

    Function GetJenisPengeluaran() As String
        If ComboBoxPengeluaran.Text = "SPP" Then
            Return "spp"
        Else
            Return "lainnya"
        End If
    End Function

    '=== Load Form ===
    Private Sub Pengeluaran_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        bukaKoneksi()
        IsiComboBoxPengeluaran()
        IsiComboBoxPJ()
        TampilDataPengeluaran()
    End Sub

    Sub IsiComboBoxPengeluaran()

        Try
            ComboBoxPengeluaran.Items.Clear()
            ComboBoxPengeluaran.Items.Add("SPP")
            ComboBoxPengeluaran.Items.Add("Lainnya")
            ComboBoxPengeluaran.SelectedIndex = 0
        Catch ex As Exception
            MsgBox("Gagal memuat jenis pengeluaran: " & ex.Message, vbCritical)
        End Try
    End Sub

    '=== Isi ComboBox PJ (Penanggung Jawab) ===
    Sub IsiComboBoxPJ()
        Try
            ComboBoxStaf.Items.Clear()

            If conn.State = ConnectionState.Open Then conn.Close()
            conn.Open()

            Dim sql As String = "SELECT nama FROM data_staff"
            Using cmd As New MySqlCommand(sql, conn)
                Using rd As MySqlDataReader = cmd.ExecuteReader()
                    While rd.Read()
                        ComboBoxStaf.Items.Add(rd("nama").ToString())
                    End While
                End Using
            End Using

            conn.Close()
        Catch ex As Exception
            MsgBox("Gagal memuat data staff: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    '=== Kurangi Total di Database Dashboard ===
    Sub KurangiTotalDashboardDB(jenis As String, nominal As Double)
        If Application.OpenForms().OfType(Of MenuDashboard).Any() Then
            Application.OpenForms().OfType(Of MenuDashboard).First().HitungTotalDashboard()
        End If

        Try
            bukaKoneksi()

            Dim sql As String
            If jenis = "spp" Then
                sql = "UPDATE dashboard SET total_spp = total_spp - @nominal"
            Else
                sql = "UPDATE dashboard SET total_lainnya = total_lainnya - @nominal"
            End If

            Using cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@nominal", nominal)
                cmd.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            MsgBox("Gagal update dashboard: " & ex.Message)
        End Try
    End Sub

    Function GetNipStaff(ByVal nama As String) As String
        Dim result As String = ""
        Try
            Call bukaKoneksi()
            Dim sql As String = "SELECT nip FROM data_staff WHERE nama=@nama LIMIT 1"
            Using cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@nama", nama)
                Using rd As MySqlDataReader = cmd.ExecuteReader()
                    If rd.Read() Then
                        result = rd("nip").ToString()
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Gagal mengambil NIP staff: " & ex.Message)
        End Try
        Return result
    End Function

    Function GetNamaStaff(ByVal nip As String) As String
        Dim result As String = ""
        Try
            Call bukaKoneksi()
            Dim sql As String = "SELECT nama FROM data_staff WHERE nip=@nip LIMIT 1"
            Using cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@nip", nip)
                Using rd As MySqlDataReader = cmd.ExecuteReader()
                    If rd.Read() Then
                        result = rd("nama").ToString()
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Gagal mengambil nama staff: " & ex.Message)
        End Try
        Return result
    End Function

    '=== Tambah / Update Data ===
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If TxtNominal.Text = "" Or ComboBoxPengeluaran.Text = "" Or ComboBoxStaf.Text = "" Then
                MsgBox("Semua data harus diisi!", vbExclamation)
                Exit Sub
            End If

            Dim nominal As Double = Val(TxtNominal.Text.Replace(".", ""))
            Dim jenis As String = GetJenisPengeluaran()
            Dim tanggalDB As String = DateTimePickerTanggal.Value.ToString("yyyy-MM-dd")
            Dim nipStaf As String = GetNipStaff(ComboBoxStaf.Text)
            Dim ket As String = RichTxtKeterangan.Text

            bukaKoneksi()

            '================ INSERT ================
            If isEdit = False Then
                Dim sql As String =
                    "INSERT INTO pengeluaran (nominal, pengeluaran, staf, tanggal, keterangan) 
                     VALUES (@nominal, @pengeluaran, @staf, @tanggal, @keterangan)"

                Using cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@nominal", nominal)
                    cmd.Parameters.AddWithValue("@pengeluaran", jenis)
                    cmd.Parameters.AddWithValue("@staf", nipStaf)
                    cmd.Parameters.AddWithValue("@tanggal", tanggalDB)
                    cmd.Parameters.AddWithValue("@keterangan", ket)
                    cmd.ExecuteNonQuery()
                End Using

                KurangiTotalDashboardDB(jenis, nominal)
                MsgBox("Pengeluaran berhasil ditambahkan!", vbInformation)

            Else
                '================ UPDATE ================
                Dim sql As String =
                    "UPDATE pengeluaran SET 
                        nominal=@nominalBaru,
                        pengeluaran=@pengeluaranBaru,
                        staf=@stafBaru,
                        tanggal=@tanggalBaru,
                        keterangan=@keteranganBaru
                     WHERE nominal=@nominalLama AND pengeluaran=@pengeluaranLama 
                     AND staf=@stafLama AND tanggal=@tanggalLama AND keterangan=@keteranganLama LIMIT 1"

                Using cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@nominalBaru", nominal)
                    cmd.Parameters.AddWithValue("@pengeluaranBaru", jenis)
                    cmd.Parameters.AddWithValue("@stafBaru", nipStaf)
                    cmd.Parameters.AddWithValue("@tanggalBaru", tanggalDB)
                    cmd.Parameters.AddWithValue("@keteranganBaru", ket)

                    cmd.Parameters.AddWithValue("@nominalLama", kNominal)
                    cmd.Parameters.AddWithValue("@pengeluaranLama", kPengeluaran)
                    cmd.Parameters.AddWithValue("@stafLama", kStaf)
                    cmd.Parameters.AddWithValue("@tanggalLama", kTanggal)
                    cmd.Parameters.AddWithValue("@keteranganLama", kKeterangan)
                    cmd.ExecuteNonQuery()
                End Using

                KurangiTotalDashboardDB(jenis, nominal)
                MsgBox("Data pengeluaran berhasil diperbarui!", vbInformation)

            End If

            BersihkanForm()
            TampilDataPengeluaran()

        Catch ex As Exception
            MsgBox("Terjadi kesalahan: " & ex.Message, vbCritical)
        End Try
    End Sub

    '=== Update Total Dashboard ===


    '=== Tampilkan Data ke Grid ===
    Sub TampilDataPengeluaran()
        Try
            bukaKoneksi()
            Dim sql As String = "SELECT nominal, pengeluaran, staf, tanggal, keterangan FROM pengeluaran ORDER BY tanggal DESC"
            Dim da As New MySqlDataAdapter(sql, conn)
            Dim dt As New DataTable
            da.Fill(dt)

            ' Ubah NIP → Nama
            For Each row As DataRow In dt.Rows
                row("staf") = GetNamaStaff(row("staf").ToString())
            Next

            DataGridView1.DataSource = dt

            ' Tambah tombol edit
            If Not DataGridView1.Columns.Contains("Edit") Then
                Dim btn As New DataGridViewButtonColumn()
                btn.Name = "Edit"
                btn.Text = "Edit"
                btn.UseColumnTextForButtonValue = True
                DataGridView1.Columns.Add(btn)
            End If

            If Not DataGridView1.Columns.Contains("Hapus") Then
                Dim btn As New DataGridViewButtonColumn()
                btn.Name = "Hapus"
                btn.Text = "Hapus"
                btn.UseColumnTextForButtonValue = True
                DataGridView1.Columns.Add(btn)
            End If

            ' === Atur lebar kolom ===
            With DataGridView1
                If .Columns.Contains("nominal") Then .Columns("nominal").Width = 150
                If .Columns.Contains("pengeluaran") Then .Columns("pengeluaran").Width = 150
                If .Columns.Contains("staf") Then .Columns("staf").Width = 250
                If .Columns.Contains("tanggal") Then .Columns("tanggal").Width = 150
                If .Columns.Contains("keterangan") Then .Columns("keterangan").Width = 300

                If .Columns.Contains("Edit") Then .Columns("Edit").Width = 100
                If .Columns.Contains("Hapus") Then .Columns("Hapus").Width = 100
            End With

        Catch ex As Exception
            MsgBox("Gagal menampilkan pengeluaran: " & ex.Message)
        End Try
    End Sub

    Sub BersihkanForm()
        TxtNominal.Clear()
        ComboBoxPengeluaran.SelectedIndex = 0
        ComboBoxStaf.SelectedIndex = -1
        RichTxtKeterangan.Clear()
        isEdit = False
    End Sub

    '=== Klik tombol Edit / Hapus ===
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Try
            If e.RowIndex < 0 Then Exit Sub

            Dim row = DataGridView1.Rows(e.RowIndex)
            Dim col = DataGridView1.Columns(e.ColumnIndex).Name

            '===========================
            '          EDIT
            '===========================
            If col = "Edit" Then
                kNominal = Val(row.Cells("nominal").Value)
                kPengeluaran = row.Cells("pengeluaran").Value.ToString().ToLower()
                kStaf = GetNipStaff(row.Cells("staf").Value.ToString())
                kTanggal = Convert.ToDateTime(row.Cells("tanggal").Value).ToString("yyyy-MM-dd")
                kKeterangan = row.Cells("keterangan").Value.ToString()

                TxtNominal.Text = kNominal
                ComboBoxPengeluaran.Text = If(kPengeluaran = "spp", "SPP", "Lainnya")
                ComboBoxStaf.Text = row.Cells("staf").Value.ToString()
                DateTimePickerTanggal.Value = Convert.ToDateTime(kTanggal)
                RichTxtKeterangan.Text = kKeterangan

                isEdit = True
                MsgBox("Mode Edit diaktifkan!", vbInformation)

                '===========================
                '          HAPUS
                '===========================
            ElseIf col = "Hapus" Then

                If MsgBox("Yakin ingin menghapus data ini?", vbYesNo + vbQuestion) = vbYes Then

                    ' Normalisasi agar sesuai database
                    Dim pengeluaranDB As String = row.Cells("pengeluaran").Value.ToString().ToLower()
                    If pengeluaranDB = "spp" Then
                        pengeluaranDB = "spp"
                    Else
                        pengeluaranDB = "lainnya"
                    End If

                    bukaKoneksi()
                    Dim sql As String =
                    "DELETE FROM pengeluaran 
                     WHERE nominal=@n AND pengeluaran=@p AND staf=@s
                     AND tanggal=@t AND keterangan=@k LIMIT 1"

                    Using cmd As New MySqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@n", Val(row.Cells("nominal").Value))
                        cmd.Parameters.AddWithValue("@p", pengeluaranDB)
                        cmd.Parameters.AddWithValue("@s", GetNipStaff(row.Cells("staf").Value.ToString()))
                        cmd.Parameters.AddWithValue("@t", Convert.ToDateTime(row.Cells("tanggal").Value).ToString("yyyy-MM-dd"))
                        cmd.Parameters.AddWithValue("@k", row.Cells("keterangan").Value.ToString())
                        cmd.ExecuteNonQuery()
                    End Using

                    MsgBox("Data berhasil dihapus!", vbInformation)

                    ' Reload tabel
                    TampilDataPengeluaran()

                    ' Refresh dashboard
                    If Application.OpenForms().OfType(Of MenuDashboard).Any() Then
                        Application.OpenForms().OfType(Of MenuDashboard).First().HitungTotalDashboard()
                    End If

                End If

            End If

        Catch ex As Exception
            MsgBox("Kesalahan saat menghapus data: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub TxtNominal_TextChanged(sender As Object, e As EventArgs) Handles TxtNominal.TextChanged
        Try
            RemoveHandler TxtNominal.TextChanged, AddressOf TxtNominal_TextChanged
            Dim teks As String = TxtNominal.Text.Replace(".", "").Trim()
            If teks <> "" AndAlso IsNumeric(teks) Then
                TxtNominal.Text = FormatNumber(CDec(teks), 0, , , TriState.True)
            End If
            TxtNominal.SelectionStart = TxtNominal.Text.Length
        Catch
        Finally
            AddHandler TxtNominal.TextChanged, AddressOf TxtNominal_TextChanged
        End Try
    End Sub

End Class
