Imports System.Windows.Forms.VisualStyles
Imports MySql.Data.MySqlClient

Public Class Pembayaran_lainnya
    Dim isEdit As Boolean = False
    Dim kNis As String = ""
    Dim kTanggal As String = ""
    Dim kPembayaran As String = ""
    Dim isEditingNominal As Boolean = False
    Dim kIdPembayaran As String = ""

    Dim kKeterangan As String = ""
    Dim kNama As String = ""
    Dim kKelas As String = ""
    Dim kNominal As String = ""

    Dim kStaf As String = ""


    Function GetNamaStaff(ByVal nip As String) As String
        Dim result As String = ""
        Try
            Call bukaKoneksi()
            ' Ubah "staf" menjadi nama kolom NIP di tabel staff, misal "nip"
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


    '================= AMBIL NIP STAFF DARI NAMA ==================
    Function GetNipStaff(ByVal nama As String) As String
        Dim result As String = ""
        Try
            Call bukaKoneksi()
            ' Ambil NIP dari nama staff
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

    Sub LoadNamaStaff()
        Try
            Call bukaKoneksi()
            Dim sql As String = "SELECT nama FROM data_staff ORDER BY nama ASC"
            Using cmd As New MySqlCommand(sql, conn)
                Using rd As MySqlDataReader = cmd.ExecuteReader()
                    ComboBoxStaff.Items.Clear()
                    While rd.Read()
                        ComboBoxStaff.Items.Add(rd("nama").ToString())
                    End While
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Gagal memuat data staf: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub


    Private Sub Pembayaran_lainnya_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call bukaKoneksi()
        Call LoadNamaStaff()
        Call LoadKelas()
        Call TampilDataPembayaran()
    End Sub

    Sub LoadKelas(Optional selectedKelas As String = "")
        Try
            Call bukaKoneksi()
            Dim sql As String = "SELECT DISTINCT kelas FROM data_siswa ORDER BY kelas"
            Using cmd As New MySqlCommand(sql, conn)
                Using rd As MySqlDataReader = cmd.ExecuteReader()
                    ComboBoxKelas.Items.Clear()
                    While rd.Read()
                        ComboBoxKelas.Items.Add(rd("kelas").ToString())
                    End While
                End Using
            End Using

            If selectedKelas <> "" Then
                ComboBoxKelas.Text = selectedKelas
            Else
                ComboBoxKelas.SelectedIndex = -1
            End If
        Catch ex As Exception
            MsgBox("Gagal memuat daftar kelas: " & ex.Message)
        End Try
    End Sub

    '=========================================================
    ''' >>> UPDATE UNTUK CHECKBOX BULAN <<<
    '=========================================================
    Function GetBulanDipilih() As String
        Dim bulan As New List(Of String)
        If CheckBoxJan.Checked Then bulan.Add("Januari")
        If CheckBoxFeb.Checked Then bulan.Add("Februari")
        If CheckBoxMar.Checked Then bulan.Add("Maret")
        If CheckBoxApr.Checked Then bulan.Add("April")
        If CheckBoxMei.Checked Then bulan.Add("Mei")
        If CheckBoxJun.Checked Then bulan.Add("Juni")
        If CheckBoxJul.Checked Then bulan.Add("Juli")
        If CheckBoxAgs.Checked Then bulan.Add("Agustus")
        If CheckBoxSep.Checked Then bulan.Add("September")
        If CheckBoxOkt.Checked Then bulan.Add("Oktober")
        If CheckBoxNov.Checked Then bulan.Add("November")
        If CheckBoxDes.Checked Then bulan.Add("Desember")
        Return String.Join(", ", bulan)
    End Function

    Sub SetCheckboxBulan(ByVal keterangan As String)
        ClearCheckboxBulan()
        Dim bulanList As String() = keterangan.Split(","c)
        For Each b In bulanList
            Dim bln = b.Trim()
            Select Case bln
                Case "Januari" : CheckBoxJan.Checked = True
                Case "Februari" : CheckBoxFeb.Checked = True
                Case "Maret" : CheckBoxMar.Checked = True
                Case "April" : CheckBoxApr.Checked = True
                Case "Mei" : CheckBoxMei.Checked = True
                Case "Juni" : CheckBoxJun.Checked = True
                Case "Juli" : CheckBoxJul.Checked = True
                Case "Agustus" : CheckBoxAgs.Checked = True
                Case "September" : CheckBoxSep.Checked = True
                Case "Oktober" : CheckBoxOkt.Checked = True
                Case "November" : CheckBoxNov.Checked = True
                Case "Desember" : CheckBoxDes.Checked = True
            End Select
        Next
    End Sub

    Sub ClearCheckboxBulan()
        For Each cb As CheckBox In GroupBoxBulan.Controls.OfType(Of CheckBox)()
            cb.Checked = False
        Next
    End Sub
    '=========================================================

    'Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    '    ' Validasi input
    '    If TxtNis.Text = "" Or TxtNama.Text = "" Or TxtNominal.Text = "" Or
    '       ComboBoxPembayaran.Text = "" Or ComboBoxStaff.Text = "" Then
    '        MsgBox("Semua data wajib diisi!", vbExclamation)
    '        Exit Sub
    '    End If

    '    Try
    '        Call bukaKoneksi()
    '        Dim nip_staff As String = GetNipStaff(ComboBoxStaff.Text)
    '        Dim nominalDB As String = TxtNominal.Text.Replace(".", "").Trim()
    '        Dim tanggalDB As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
    '        Dim keteranganDB As String = GetBulanDipilih()

    '        If isEdit = False Then
    '            Dim sqlInsert As String = "INSERT INTO pembayaran (nis, nama, kelas, nominal, pembayaran, staf, tanggal, keterangan) " &
    '                                      "VALUES (@nis, @nama, @kelas, @nominal, @pembayaran, @staf, @tanggal, @keterangan)"
    '            Using cmd As New MySqlCommand(sqlInsert, conn)
    '                cmd.Parameters.AddWithValue("@nis", TxtNis.Text)
    '                cmd.Parameters.AddWithValue("@nama", TxtNama.Text)
    '                cmd.Parameters.AddWithValue("@kelas", ComboBoxKelas.Text)
    '                cmd.Parameters.AddWithValue("@nominal", nominalDB)
    '                cmd.Parameters.AddWithValue("@pembayaran", ComboBoxPembayaran.Text)
    '                cmd.Parameters.AddWithValue("@staf", nip_staff)
    '                cmd.Parameters.AddWithValue("@tanggal", tanggalDB)
    '                cmd.Parameters.AddWithValue("@keterangan", keteranganDB)
    '                cmd.ExecuteNonQuery()
    '            End Using
    '            MsgBox("Pembayaran baru berhasil ditambahkan!", vbInformation)
    '        Else
    '            Dim sqlUpdate As String = "UPDATE pembayaran SET " &
    '                "nominal=@nominalBaru, pembayaran=@pembayaranBaru, staf=@stafBaru, tanggal=@tanggalBaru, keterangan=@keteranganBaru " &
    '                "WHERE nis=@nisLama AND nama=@namaLama AND kelas=@kelasLama AND nominal=@nominalLama " &
    '                "AND pembayaran=@pembayaranLama AND tanggal=@tanggalLama AND keterangan=@keteranganLama LIMIT 1"

    '            Using cmd As New MySqlCommand(sqlUpdate, conn)
    '                cmd.Parameters.AddWithValue("@nominalBaru", nominalDB)
    '                cmd.Parameters.AddWithValue("@pembayaranBaru", ComboBoxPembayaran.Text)
    '                cmd.Parameters.AddWithValue("@stafBaru", nip_staff)
    '                cmd.Parameters.AddWithValue("@tanggalBaru", tanggalDB)
    '                cmd.Parameters.AddWithValue("@keteranganBaru", keteranganDB)

    '                cmd.Parameters.AddWithValue("@nisLama", kNis)
    '                cmd.Parameters.AddWithValue("@namaLama", kNama)
    '                cmd.Parameters.AddWithValue("@kelasLama", kKelas)
    '                cmd.Parameters.AddWithValue("@nominalLama", kNominal)
    '                cmd.Parameters.AddWithValue("@pembayaranLama", kPembayaran)
    '                cmd.Parameters.AddWithValue("@tanggalLama", kTanggal)
    '                cmd.Parameters.AddWithValue("@keteranganLama", kKeterangan)

    '                Dim result As Integer = cmd.ExecuteNonQuery()
    '                If result > 0 Then
    '                    MsgBox("Data pembayaran berhasil diperbarui!", vbInformation)
    '                Else
    '                    MsgBox("Tidak ada data yang diperbarui.", vbExclamation)
    '                End If
    '            End Using
    '        End If

    '        Bersih()
    '        TampilDataPembayaran()
    '        LoadKelas()
    '    Catch ex As Exception
    '        MsgBox("Gagal menyimpan data: " & ex.Message, vbCritical)
    '    End Try
    'End Sub



    Sub LoadStaff()
        Try
            Call bukaKoneksi()
            Dim sql As String = "SELECT nama FROM data_staff ORDER BY nama ASC"
            Using cmd As New MySqlCommand(sql, conn)
                Using rd As MySqlDataReader = cmd.ExecuteReader()
                    ComboBoxStaff.Items.Clear()
                    While rd.Read()
                        ComboBoxStaff.Items.Add(rd("nama").ToString())
                    End While
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Gagal memuat staff: " & ex.Message)
        End Try
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If TxtNis.Text = "" Or TxtNama.Text = "" Or TxtNominal.Text = "" Or
       ComboBoxPembayaran.Text = "" Or ComboBoxStaff.Text = "" Then
            MsgBox("Semua data wajib diisi!", vbExclamation)
            Exit Sub
        End If

        Try
            Call bukaKoneksi()
            Dim nip_staff As String = GetNipStaff(ComboBoxStaff.Text)
            Dim nominalDB As String = TxtNominal.Text.Replace(".", "").Trim()
            Dim tanggalDB As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
            Dim keteranganDB As String = GetBulanDipilih() ' <-- Tambahkan ini agar bulan tersimpan

            If isEdit = False Then
                ' ==== INSERT DATA BARU ====
                Dim sqlInsert As String =
                "INSERT INTO pembayaran (nis, nama, kelas, nominal, pembayaran, staf, tanggal, keterangan) " &
                "VALUES (@nis, @nama, @kelas, @nominal, @pembayaran, @staf, @tanggal, @keterangan)"
                Using cmd As New MySqlCommand(sqlInsert, conn)
                    cmd.Parameters.AddWithValue("@nis", TxtNis.Text)
                    cmd.Parameters.AddWithValue("@nama", TxtNama.Text)
                    cmd.Parameters.AddWithValue("@kelas", ComboBoxKelas.Text)
                    cmd.Parameters.AddWithValue("@nominal", nominalDB)
                    cmd.Parameters.AddWithValue("@pembayaran", ComboBoxPembayaran.Text)
                    cmd.Parameters.AddWithValue("@staf", nip_staff)
                    cmd.Parameters.AddWithValue("@tanggal", tanggalDB)
                    cmd.Parameters.AddWithValue("@keterangan", keteranganDB) ' <-- Tambahkan di sini
                    cmd.ExecuteNonQuery()
                End Using
                MsgBox("Pembayaran baru berhasil ditambahkan!", vbInformation)
            Else
                ' ==== UPDATE DATA BARIS TERPILIH ====
                Dim sqlUpdate As String =
                "UPDATE pembayaran SET " &
                "nominal=@nominalBaru, pembayaran=@pembayaranBaru, staf=@stafBaru, tanggal=@tanggalBaru, keterangan=@keteranganBaru " &
                "WHERE nis=@nisLama AND nama=@namaLama AND kelas=@kelasLama AND nominal=@nominalLama " &
                "AND pembayaran=@pembayaranLama AND tanggal=@tanggalLama AND keterangan=@keteranganLama LIMIT 1"

                Using cmd As New MySqlCommand(sqlUpdate, conn)
                    cmd.Parameters.AddWithValue("@nominalBaru", nominalDB)
                    cmd.Parameters.AddWithValue("@pembayaranBaru", ComboBoxPembayaran.Text)
                    cmd.Parameters.AddWithValue("@stafBaru", nip_staff)
                    cmd.Parameters.AddWithValue("@tanggalBaru", tanggalDB)
                    cmd.Parameters.AddWithValue("@keteranganBaru", keteranganDB) ' <-- Tambahkan di sini juga

                    cmd.Parameters.AddWithValue("@nisLama", kNis)
                    cmd.Parameters.AddWithValue("@namaLama", kNama)
                    cmd.Parameters.AddWithValue("@kelasLama", kKelas)
                    cmd.Parameters.AddWithValue("@nominalLama", kNominal)
                    cmd.Parameters.AddWithValue("@pembayaranLama", kPembayaran)
                    cmd.Parameters.AddWithValue("@tanggalLama", kTanggal)
                    cmd.Parameters.AddWithValue("@keteranganLama", kKeterangan)

                    Dim result As Integer = cmd.ExecuteNonQuery()
                    If result > 0 Then
                        MsgBox("Data pembayaran berhasil diperbarui!", vbInformation)
                    Else
                        MsgBox("Tidak ada data yang diperbarui. Pastikan data lama masih sama.", vbExclamation)
                    End If
                End Using
            End If

            Bersih()
            TampilDataPembayaran()
            LoadKelas()
            TxtCari.Clear()       ' Menghapus isi textbox cari
            TampilDataPembayaran() ' Menampilkan semua data kembali


        Catch ex As Exception
            MsgBox("Gagal menyimpan data: " & ex.Message, vbCritical)
        Finally
            ClearCheckboxBulan()
        End Try
    End Sub

    Sub TampilDataPembayaran()
        Try
            Call bukaKoneksi()
            Dim sql As String = "SELECT nis, nama, kelas, nominal, pembayaran, staf, tanggal, keterangan FROM pembayaran"
            da = New MySqlDataAdapter(sql, conn)
            ds = New DataSet
            da.Fill(ds)

            ' Ubah NIP ke nama staf
            For Each row As DataRow In ds.Tables(0).Rows
                row("staf") = GetNamaStaff(row("staf").ToString())

            Next


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

            ' Format kolom
            With DataGridView1
                .Columns("nis").Width = 100
                .Columns("nama").Width = 200
                .Columns("kelas").Width = 70
                .Columns("nominal").Width = 140
                .Columns("pembayaran").Width = 80
                .Columns("staf").Width = 200
                .Columns("tanggal").Width = 100
                .Columns("keterangan").Width = 200

                .Columns("Edit").Width = 50
                .Columns("Hapus").Width = 50
            End With

            ' Nominal tanpa titik
            For Each row As DataGridViewRow In DataGridView1.Rows
                If Not row.IsNewRow Then
                    Dim val = row.Cells("nominal").Value
                    If val IsNot Nothing AndAlso IsNumeric(val) Then
                        row.Cells("nominal").Value = CDec(val).ToString("0")
                    End If
                End If
            Next

        Catch ex As Exception
            MsgBox("Gagal menampilkan pembayaran: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub TxtCari_TextChanged(sender As Object, e As EventArgs) Handles TxtCari.TextChanged
        Call CariSiswa()
    End Sub

    Sub CariSiswa()
        If TxtCari.Text.Trim() = "" Then
            TxtNis.Clear()
            TxtNama.Clear()
            ComboBoxKelas.Items.Clear()
            ComboBoxKelas.Text = ""
            TxtNominal.Clear()
            ComboBoxPembayaran.Text = ""
            ComboBoxStaff.Text = ""

            ' Reload semua kelas
            LoadKelas()
            Exit Sub
        End If

        Try
            Call bukaKoneksi()
            Dim sql As String = "SELECT * FROM data_siswa WHERE nis LIKE @cari OR nama LIKE @cari LIMIT 1"
            Using cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@cari", "%" & TxtCari.Text & "%")
                Using rd As MySqlDataReader = cmd.ExecuteReader()
                    If rd.Read() Then
                        TxtNis.Text = rd("nis").ToString()
                        TxtNama.Text = rd("nama").ToString()

                        ' Load semua kelas dari DB lalu pilih sesuai data siswa
                        LoadKelas(rd("kelas").ToString())

                    Else
                        TxtNis.Clear()
                        TxtNama.Clear()
                        ComboBoxKelas.Items.Clear()
                        ComboBoxKelas.Text = ""
                        TxtNominal.Clear()
                        ComboBoxPembayaran.Text = ""
                        ComboBoxStaff.Text = ""
                        LoadKelas()
                    End If
                End Using
            End Using

        Catch ex As Exception
            MsgBox("Error pencarian siswa: " & ex.Message)
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Try
            If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub
            Dim col = DataGridView1.Columns(e.ColumnIndex)
            Dim row = DataGridView1.Rows(e.RowIndex)

            If TypeOf col Is DataGridViewButtonColumn Then
                Select Case col.Name
                    Case "Edit"
                        ' Ambil semua nilai lama dari DataGridView
                        kNis = row.Cells("nis").Value.ToString()
                        kNama = row.Cells("nama").Value.ToString()
                        kKelas = row.Cells("kelas").Value.ToString()
                        kNominal = row.Cells("nominal").Value.ToString().Replace(".", "").Trim()
                        kPembayaran = row.Cells("pembayaran").Value.ToString()

                        ' Ambil NIP dari nama staf di DataGridView
                        kStaf = GetNipStaff(row.Cells("staf").Value.ToString())

                        kTanggal = Convert.ToDateTime(row.Cells("tanggal").Value).ToString("yyyy-MM-dd")
                        kKeterangan = row.Cells("keterangan").Value.ToString()

                        ' Isi form
                        TxtNis.Text = kNis
                        TxtNama.Text = kNama
                        ComboBoxKelas.Text = kKelas
                        TxtNominal.Text = kNominal
                        ComboBoxPembayaran.Text = kPembayaran
                        ComboBoxStaff.Text = row.Cells("staf").Value.ToString() ' tetap tampilkan nama
                        DateTimePicker1.Value = Convert.ToDateTime(kTanggal)
                        'RichTxtKeterangan.Text = row.Cells("keterangan").Value.ToString()

                        kNominal = row.Cells("nominal").Value.ToString().Replace(".", "").Trim()
                        kPembayaran = row.Cells("pembayaran").Value.ToString()
                        kTanggal = Convert.ToDateTime(row.Cells("tanggal").Value).ToString("yyyy-MM-dd")


                        isEdit = True
                        MsgBox("Mode Edit diaktifkan!", vbInformation)


                    Case "Hapus"
                        If MsgBox("Yakin ingin menghapus data pembayaran ini?", vbYesNo + vbQuestion) = vbYes Then
                            Dim sqlDelete As String =
                                "DELETE FROM pembayaran 
                                 WHERE nis=@nis AND nama=@nama AND kelas=@kelas 
                                   AND nominal=@nominal AND pembayaran=@pembayaran 
                                   AND staf=@staf AND tanggal=@tanggal AND keterangan=@keterangan LIMIT 1"
                            Using cmdDel As New MySqlCommand(sqlDelete, conn)
                                cmdDel.Parameters.AddWithValue("@nis", row.Cells("nis").Value.ToString())
                                cmdDel.Parameters.AddWithValue("@nama", row.Cells("nama").Value.ToString())
                                cmdDel.Parameters.AddWithValue("@kelas", row.Cells("kelas").Value.ToString())
                                cmdDel.Parameters.AddWithValue("@nominal", row.Cells("nominal").Value.ToString().Replace(".", "").Trim())
                                cmdDel.Parameters.AddWithValue("@pembayaran", row.Cells("pembayaran").Value.ToString())
                                cmdDel.Parameters.AddWithValue("@staf", GetNipStaff(row.Cells("staf").Value.ToString()))
                                cmdDel.Parameters.AddWithValue("@tanggal", Convert.ToDateTime(row.Cells("tanggal").Value).ToString("yyyy-MM-dd"))
                                cmdDel.Parameters.AddWithValue("@keterangan", row.Cells("keterangan").Value.ToString())
                                Dim affected As Integer = cmdDel.ExecuteNonQuery()
                                If affected > 0 Then
                                    MsgBox("Data pembayaran berhasil dihapus!", vbInformation)
                                Else
                                    MsgBox("Data tidak ditemukan atau gagal dihapus.", vbExclamation)
                                End If
                            End Using
                            TampilDataPembayaran()
                            Bersih()
                        End If
                End Select
            End If
        Catch ex As Exception
            MsgBox("Terjadi kesalahan: " & ex.Message, vbCritical)
        End Try
    End Sub
    Sub Bersih()
        TxtNis.Clear()
        TxtNama.Clear()
        ComboBoxKelas.Text = ""
        TxtNominal.Clear()
        ComboBoxPembayaran.Text = ""
        ComboBoxStaff.Text = ""
        ClearCheckboxBulan()

        isEdit = False
    End Sub

    '=== Format angka otomatis menjadi Rupiah dengan titik ===
    Private Sub TxtNominal_TextChanged(sender As Object, e As EventArgs) Handles TxtNominal.TextChanged
        Try
            RemoveHandler TxtNominal.TextChanged, AddressOf TxtNominal_TextChanged
            isEditingNominal = True

            Dim teks As String = TxtNominal.Text.Replace(".", "").Replace(",", "").Trim()
            If teks <> "" AndAlso IsNumeric(teks) Then
                TxtNominal.Text = FormatNumber(CDec(teks), 0, , , TriState.True)
            End If

            TxtNominal.SelectionStart = TxtNominal.Text.Length
            isEditingNominal = False
            AddHandler TxtNominal.TextChanged, AddressOf TxtNominal_TextChanged
        Catch
            isEditingNominal = False
            AddHandler TxtNominal.TextChanged, AddressOf TxtNominal_TextChanged
        End Try
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub ComboBoxPembayaran_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxPembayaran.SelectedIndexChanged

    End Sub
End Class