Imports System.Windows.Forms.VisualStyles
Imports MySql.Data.MySqlClient

Public Class PembayaranLainnya
    Dim isEdit As Boolean = False
    Dim kPemasukan As String = ""
    Dim kTanggal As Date
    Dim kNominal As Double = 0
    Dim kStaf As String = ""
    Dim isEditingNominal As Boolean = False




    '================= MUAT DATA PEMASUKAN UNIK ==================
    Sub LoadDataPemasukan()
        Try
            If conn.State = ConnectionState.Open Then conn.Close()
            bukaKoneksi()

            Dim sql As String = "SELECT DISTINCT pemasukan FROM pembayaran_lainnya ORDER BY pemasukan ASC"
            cmd = New MySqlCommand(sql, conn)
            rd = cmd.ExecuteReader()

            ComboBoxPemasukan.Items.Clear()
            While rd.Read()
                ComboBoxPemasukan.Items.Add(rd("pemasukan").ToString())
            End While
            rd.Close()
            conn.Close()
        Catch ex As Exception
            MsgBox("Gagal memuat data pemasukan: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Sub LoadNamaStaff()
        Try
            bukaKoneksi()
            Dim sql As String = "SELECT nama FROM data_staff ORDER BY nama ASC"
            cmd = New MySqlCommand(sql, conn)
            rd = cmd.ExecuteReader()

            ComboBoxStaf.Items.Clear()
            While rd.Read()
                ComboBoxStaf.Items.Add(rd("nama").ToString())
            End While
            rd.Close()
        Catch ex As Exception
            MsgBox("Gagal memuat data staf: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    '================= AMBIL NIP DARI NAMA STAFF ==================
    Function GetNipStaff(ByVal nama As String) As String
        Dim result As String = ""
        Try
            bukaKoneksi()
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

    Private Sub PembayaranLainnya_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        bukaKoneksi()
        TampilData()
        LoadNamaStaff()
        LoadDataPemasukan()
    End Sub

    Sub TampilData()
        Try
            bukaKoneksi()
            Dim sql As String = "SELECT pemasukan, tanggal, nominal, staf FROM pembayaran_lainnya ORDER BY tanggal DESC"
            da = New MySqlDataAdapter(sql, conn)
            ds = New DataSet
            da.Fill(ds)
            DataGridView1.DataSource = ds.Tables(0)

            ' Tambah tombol Edit & Hapus
            If Not DataGridView1.Columns.Contains("Edit") Then
                Dim btnEdit As New DataGridViewButtonColumn()
                btnEdit.Name = "Edit"
                btnEdit.HeaderText = "Edit"
                btnEdit.Text = "Edit"
                btnEdit.UseColumnTextForButtonValue = True
                btnEdit.FlatStyle = FlatStyle.Flat
                btnEdit.DefaultCellStyle.BackColor = Color.LightGreen
                DataGridView1.Columns.Add(btnEdit)
            End If
            If Not DataGridView1.Columns.Contains("Hapus") Then
                Dim btnHapus As New DataGridViewButtonColumn()
                btnHapus.Name = "Hapus"
                btnHapus.HeaderText = "Hapus"
                btnHapus.Text = "Hapus"
                btnHapus.UseColumnTextForButtonValue = True
                btnHapus.FlatStyle = FlatStyle.Flat
                btnHapus.DefaultCellStyle.BackColor = Color.LightCoral
                DataGridView1.Columns.Add(btnHapus)
            End If

            ' Tambah tombol Edit & Hapus
            If Not DataGridView1.Columns.Contains("Edit") Then
                Dim btnEdit As New DataGridViewButtonColumn()
                btnEdit.Name = "Edit"
                btnEdit.HeaderText = "Edit"
                btnEdit.Text = "Edit"
                btnEdit.UseColumnTextForButtonValue = True
                DataGridView1.Columns.Add(btnEdit)
            End If

            If Not DataGridView1.Columns.Contains("Hapus") Then
                Dim btnHapus As New DataGridViewButtonColumn()
                btnHapus.Name = "Hapus"
                btnHapus.HeaderText = "Hapus"
                btnHapus.Text = "Hapus"
                btnHapus.UseColumnTextForButtonValue = True
                DataGridView1.Columns.Add(btnHapus)
            End If
            ' Format kolom nominal sebagai Rupiah
            If DataGridView1.Columns.Contains("nominal") Then
                DataGridView1.Columns("nominal").DefaultCellStyle.Format = "C0"
                DataGridView1.Columns("nominal").DefaultCellStyle.FormatProvider = Globalization.CultureInfo.GetCultureInfo("id-ID")
            End If


            For Each row As DataGridViewRow In DataGridView1.Rows
                If Not row.IsNewRow Then
                    row.Cells("pemasukan").Tag = row.Cells("pemasukan").Value.ToString() &
                                     "|" & Format(CDate(row.Cells("tanggal").Value), "yyyy-MM-dd") &
                                     "|" & row.Cells("staf").Value.ToString()
                End If
            Next



            With DataGridView1
                .Columns("pemasukan").Width = 300
                .Columns("tanggal").Width = 100
                .Columns("nominal").Width = 290
                .Columns("staf").Width = 300

                .Columns("Edit").Width = 100
                .Columns("Hapus").Width = 100
            End With

        Catch ex As Exception
            MsgBox("Gagal menampilkan data: " & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            bukaKoneksi()

            If isEdit Then
                ' Pastikan Tag ada (key unik disimpan saat klik Edit)
                If ComboBoxPemasukan.Tag Is Nothing Then
                    MsgBox("Key unik tidak ditemukan. Pilih baris yang benar untuk diedit.", vbExclamation)
                    If conn.State = ConnectionState.Open Then conn.Close()
                    Exit Sub
                End If

                ' Ambil key lama dari Tag (format: pemasukan|yyyy-MM-dd|staf)
                Dim keyParts() As String = ComboBoxPemasukan.Tag.ToString().Split("|"c)
                If keyParts.Length < 3 Then
                    MsgBox("Key unik tidak valid. Pembaruan dibatalkan.", vbExclamation)
                    If conn.State = ConnectionState.Open Then conn.Close()
                    Exit Sub
                End If

                Dim lamaPemasukan As String = keyParts(0)
                Dim lamaTanggal As String = keyParts(1)
                Dim lamaStaf As String = keyParts(2)

                Dim sqlUpdate As String =
            "UPDATE pembayaran_lainnya " &
            "SET pemasukan=@baruPemasukan, tanggal=@baruTanggal, nominal=@baruNominal, staf=@baruStaf " &
            "WHERE pemasukan=@lamaPemasukan AND DATE(tanggal)=@lamaTanggal AND staf=@lamaStaf LIMIT 1"

                Using cmd As New MySqlCommand(sqlUpdate, conn)
                    cmd.Parameters.AddWithValue("@baruPemasukan", ComboBoxPemasukan.Text)
                    cmd.Parameters.AddWithValue("@baruTanggal", Format(DateTanggal.Value, "yyyy-MM-dd"))
                    ' Konversi dari "Rp1.000.000" -> 1000000 sebelum simpan
                    cmd.Parameters.AddWithValue("@baruNominal", Val(Replace(Replace(Replace(TxtNominal.Text, "Rp", ""), ".", ""), ",", "")))
                    cmd.Parameters.AddWithValue("@baruStaf", ComboBoxStaf.Text)

                    cmd.Parameters.AddWithValue("@lamaPemasukan", lamaPemasukan)
                    cmd.Parameters.AddWithValue("@lamaTanggal", lamaTanggal)
                    cmd.Parameters.AddWithValue("@lamaStaf", lamaStaf)

                    Dim rows As Integer = cmd.ExecuteNonQuery()
                    If rows > 0 Then
                        MsgBox("Data berhasil diperbarui!", vbInformation)

                    Else
                        MsgBox("Data gagal diperbarui. Pastikan baris sumber masih ada.", vbExclamation)
                    End If
                End Using

            Else
                ' INSERT DATA BARU
                Dim sqlInsert As String =
            "INSERT INTO pembayaran_lainnya (pemasukan, tanggal, nominal, staf) " &
            "VALUES (@pemasukan, @tanggal, @nominal, @staf)"

                Using cmd As New MySqlCommand(sqlInsert, conn)
                    cmd.Parameters.AddWithValue("@pemasukan", ComboBoxPemasukan.Text)
                    cmd.Parameters.AddWithValue("@tanggal", Format(DateTanggal.Value, "yyyy-MM-dd"))
                    ' HANYA satu parameter @nominal di sini (tidak duplikat)
                    cmd.Parameters.AddWithValue("@nominal", Val(Replace(Replace(Replace(TxtNominal.Text, "Rp", ""), ".", ""), ",", "")))
                    cmd.Parameters.AddWithValue("@staf", ComboBoxStaf.Text)
                    cmd.ExecuteNonQuery()

                    Call TambahTotalDashboardDB(CDbl(TxtNominal.Text))

                End Using

                MsgBox("Data baru berhasil disimpan!", vbInformation)

            End If

            ' Tutup koneksi jika masih terbuka
            If conn.State = ConnectionState.Open Then conn.Close()

            ' Refresh data & combobox
            TampilData()
            LoadNamaStaff()
            LoadDataPemasukan()

            ' Reset form
            Bersih()
            isEdit = False
            ComboBoxPemasukan.Tag = Nothing

        Catch ex As Exception
            If conn.State = ConnectionState.Open Then conn.Close()
            MsgBox("Terjadi kesalahan: " & ex.Message, vbCritical)
        End Try
    End Sub

    Public Sub TambahTotalDashboardDB(ByVal jumlah As Double)
        Try
            Dim conn As New MySqlConnection("server=localhost;user id=root;password=;database=db_keuangansekolah")
            conn.Open()

            ' Tambah total_lainnya dengan jumlah pemasukan
            Dim query As String = "UPDATE dashboard SET total_lainnya = total_lainnya + @jumlah"
            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@jumlah", jumlah)
            cmd.ExecuteNonQuery()

            conn.Close()
        Catch ex As Exception
            MessageBox.Show("Gagal menambah total dashboard: " & ex.Message)
        End Try
    End Sub


    Sub Bersih()
        ComboBoxPemasukan.Text = ""
        TxtNominal.Text = ""
        ComboBoxStaf.Text = ""
        DateTanggal.Value = Date.Now
        isEdit = False
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TxtCari.TextChanged
        Try
            bukaKoneksi()

            ' Query ambil data berdasarkan kata kunci pencarian
            Dim sql As String =
            "SELECT pemasukan, tanggal, nominal, staf 
             FROM pembayaran_lainnya 
             WHERE pemasukan LIKE @cari 
             OR staf LIKE @cari 
             OR tanggal LIKE @cari
             ORDER BY tanggal DESC"

            cmd = New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@cari", "%" & TxtCari.Text & "%")

            da = New MySqlDataAdapter(cmd)
            ds = New DataSet
            da.Fill(ds, "pembayaran_lainnya")

            DataGridView1.DataSource = ds.Tables("pembayaran_lainnya")

            ' Tambahkan kembali tombol Edit & Hapus agar tetap muncul
            If Not DataGridView1.Columns.Contains("Edit") Then
                Dim btnEdit As New DataGridViewButtonColumn()
                btnEdit.Name = "Edit"
                btnEdit.HeaderText = "Edit"
                btnEdit.Text = "Edit"
                btnEdit.UseColumnTextForButtonValue = True
                DataGridView1.Columns.Add(btnEdit)
            End If

            If Not DataGridView1.Columns.Contains("Hapus") Then
                Dim btnHapus As New DataGridViewButtonColumn()
                btnHapus.Name = "Hapus"
                btnHapus.HeaderText = "Hapus"
                btnHapus.Text = "Hapus"
                btnHapus.UseColumnTextForButtonValue = True
                DataGridView1.Columns.Add(btnHapus)
            End If

        Catch ex As Exception
            MsgBox("Terjadi kesalahan saat mencari data: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    '================= TOMBOL EDIT & HAPUS ==================
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Try
            If e.RowIndex < 0 Then Exit Sub
            Dim kolom As String = DataGridView1.Columns(e.ColumnIndex).Name
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

            Dim pemasukan As String = row.Cells("pemasukan").Value.ToString()
            Dim tanggal As Date = CDate(row.Cells("tanggal").Value)
            Dim nominal As Double = CDbl(row.Cells("nominal").Value)
            Dim staf As String = row.Cells("staf").Value.ToString()

            ' ==== EDIT ====
            If kolom = "Edit" Then
                ComboBoxPemasukan.Text = pemasukan
                DateTanggal.Value = tanggal
                TxtNominal.Text = nominal.ToString()
                ComboBoxStaf.Text = staf

                ' Simpan key unik di Tag (berguna saat Update)
                Dim keyUnik As String = pemasukan & "|" & Format(tanggal, "yyyy-MM-dd") & "|" & staf
                ComboBoxPemasukan.Tag = keyUnik

                kPemasukan = pemasukan
                kTanggal = tanggal
                kStaf = staf

                isEdit = True
                MsgBox("Mode Edit diaktifkan.", vbInformation)
                Exit Sub
            End If

            ' ==== HAPUS ====
            If kolom = "Hapus" Then
                If MsgBox("Yakin ingin menghapus data ini?", vbYesNo + vbQuestion) = vbYes Then
                    bukaKoneksi()
                    Dim sqlDel As String =
                    "DELETE FROM pembayaran_lainnya " &
                    "WHERE pemasukan=@pemasukan AND DATE(tanggal)=@tanggal AND staf=@staf LIMIT 1"

                    Using cmd As New MySqlCommand(sqlDel, conn)
                        cmd.Parameters.AddWithValue("@pemasukan", pemasukan)
                        cmd.Parameters.AddWithValue("@tanggal", Format(tanggal, "yyyy-MM-dd"))
                        cmd.Parameters.AddWithValue("@staf", staf)
                        Dim rows As Integer = cmd.ExecuteNonQuery()
                        If rows > 0 Then
                            MsgBox("Data berhasil dihapus!", vbInformation)
                        Else
                            MsgBox("Data tidak ditemukan / gagal dihapus.", vbExclamation)
                        End If
                    End Using

                    If conn.State = ConnectionState.Open Then conn.Close()

                    ' Refresh
                    TampilData()
                    LoadDataPemasukan()
                End If
            End If

        Catch ex As Exception
            If conn.State = ConnectionState.Open Then conn.Close()
            MsgBox("Terjadi kesalahan: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub TxtNominal_TextChanged(sender As Object, e As EventArgs) Handles TxtNominal.TextChanged
        Try
            RemoveHandler TxtNominal.TextChanged, AddressOf TxtNominal_TextChanged
            If isEditingNominal Then
                AddHandler TxtNominal.TextChanged, AddressOf TxtNominal_TextChanged
                Exit Sub
            End If

            Dim teks As String = TxtNominal.Text.Replace(".", "").Replace(",", "").Trim()
            If teks <> "" AndAlso IsNumeric(teks) Then
                TxtNominal.Text = FormatNumber(CDec(teks), 0, , , TriState.True)
            End If

            TxtNominal.SelectionStart = TxtNominal.Text.Length
            AddHandler TxtNominal.TextChanged, AddressOf TxtNominal_TextChanged
        Catch
        End Try
    End Sub
End Class