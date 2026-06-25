Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MySql.Data.MySqlClient

Public Class DataSiswa
    Dim da As MySqlDataAdapter
    Dim ds As DataSet
    Dim sql As String
    Dim dt As DataTable
    Dim cmd As MySqlCommand
    Dim isEdit As Boolean = False
    Dim nisEdit As String = ""

    '=== Tampilkan data ke DataGridView ===
    Sub TampilData()
        Try
            bukaKoneksi()
            sql = "SELECT nis AS 'NIS', nama AS 'Nama', gender AS 'Gender', kelas AS 'Kelas', no_hp AS 'No.HP', alamat AS 'Alamat' FROM data_siswa"
            da = New MySqlDataAdapter(sql, conn)
            ds = New DataSet()
            da.Fill(ds, "data_siswa")
            DataGridView1.DataSource = ds.Tables("data_siswa")

            ' Tambah tombol Edit dan Hapus (tidak berubah)
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

            ' Tambahkan tombol Edit & Hapus hanya sekali
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
            MsgBox("Gagal menampilkan data: " & ex.Message, vbCritical)
        End Try
    End Sub

    '=== Isi ComboBox Kelas dari database ===
    Sub IsiComboBoxKelas()
        Try
            bukaKoneksi()
            ComboBoxKelas.Items.Clear()

            ' Ambil data kelas unik dari tabel data_siswa
            sql = "SELECT DISTINCT kelas FROM data_siswa ORDER BY kelas ASC"
            cmd = New MySqlCommand(sql, conn)
            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            While rd.Read()
                ComboBoxKelas.Items.Add(rd("kelas").ToString())
            End While
            rd.Close()

            ' Izinkan pengguna mengetik manual (tidak DropDownList)
            ComboBoxKelas.DropDownStyle = ComboBoxStyle.DropDown

        Catch ex As Exception
            MsgBox("Gagal memuat data kelas: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub DataSiswa_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TampilData()
        IsiComboBoxKelas()
        ComboBoxGender.DropDownStyle = ComboBoxStyle.DropDownList ' Supaya gender tidak bisa diketik manual
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Try
            If e.RowIndex >= 0 Then
                Dim kolom As String = DataGridView1.Columns(e.ColumnIndex).Name
                Dim nis As String = DataGridView1.Rows(e.RowIndex).Cells("NIS").Value.ToString()

                'Tombol Edit
                If kolom = "Edit" Then
                    TxtNIS.Text = DataGridView1.Rows(e.RowIndex).Cells("NIS").Value.ToString()
                    TxtNama.Text = DataGridView1.Rows(e.RowIndex).Cells("Nama").Value.ToString()
                    ComboBoxGender.Text = DataGridView1.Rows(e.RowIndex).Cells("Gender").Value.ToString()
                    ComboBoxKelas.Text = DataGridView1.Rows(e.RowIndex).Cells("Kelas").Value.ToString()
                    TxtNoHp.Text = DataGridView1.Rows(e.RowIndex).Cells("No.HP").Value.ToString()
                    RichTxtAlamat.Text = DataGridView1.Rows(e.RowIndex).Cells("Alamat").Value.ToString()

                    isEdit = True
                    nisEdit = nis
                    MsgBox("Mode Edit Aktif!.", vbInformation)

                End If

                'Tombol Hapus
                If kolom = "Hapus" Then
                    Dim konfirmasi = MsgBox("Yakin ingin menghapus data NIS: " & nis & " ?", vbYesNo + vbQuestion, "Konfirmasi Hapus")
                    If konfirmasi = vbYes Then
                        bukaKoneksi()
                        sql = "DELETE FROM data_siswa WHERE nis=@nis"
                        cmd = New MySqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@nis", nis)
                        cmd.ExecuteNonQuery()
                        MsgBox("Data berhasil dihapus!", vbInformation)
                        TampilData()
                        IsiComboBoxKelas()
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox("Terjadi kesalahan: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub BtnSimpan_Click(sender As Object, e As EventArgs) Handles BtnSimpan.Click
        If TxtNIS.Text = "" Or TxtNama.Text = "" Or ComboBoxGender.Text = "" Or ComboBoxKelas.Text = "" Or TxtNoHp.Text = "" Or RichTxtAlamat.Text = "" Then
            MsgBox("Semua data wajib diisi!", vbExclamation)
            Exit Sub
        End If

        Try
            bukaKoneksi()

            If isEdit Then
                'UPDATE Data
                sql = "UPDATE data_siswa SET nis=@nis, nama=@nama, gender=@gender, kelas=@kelas, no_hp=@no_hp, alamat=@alamat WHERE nis=@nisEdit"
                cmd = New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@nis", TxtNIS.Text)
                cmd.Parameters.AddWithValue("@nama", TxtNama.Text)
                cmd.Parameters.AddWithValue("@gender", ComboBoxGender.Text)
                cmd.Parameters.AddWithValue("@kelas", ComboBoxKelas.Text)
                cmd.Parameters.AddWithValue("@no_hp", TxtNoHp.Text)
                cmd.Parameters.AddWithValue("@alamat", RichTxtAlamat.Text)
                cmd.Parameters.AddWithValue("@nisEdit", nisEdit)
                cmd.ExecuteNonQuery()

                MsgBox("Data berhasil diperbarui!", vbInformation)
                isEdit = False
                nisEdit = ""
            Else
                'INSERT Data Baru
                sql = "INSERT INTO data_siswa (nis, nama, gender, kelas, no_hp, alamat) VALUES (@nis, @nama, @gender, @kelas, @no_hp, @alamat)"
                cmd = New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@nis", TxtNIS.Text)
                cmd.Parameters.AddWithValue("@nama", TxtNama.Text)
                cmd.Parameters.AddWithValue("@gender", ComboBoxGender.Text)
                cmd.Parameters.AddWithValue("@kelas", ComboBoxKelas.Text)
                cmd.Parameters.AddWithValue("@no_hp", TxtNoHp.Text)
                cmd.Parameters.AddWithValue("@alamat", RichTxtAlamat.Text)
                cmd.ExecuteNonQuery()

                MsgBox("Data berhasil disimpan!", vbInformation)
            End If


            Bersih()
            IsiComboBoxKelas()
            TampilData()

        Catch ex As Exception
            MsgBox("Gagal menyimpan data: " & ex.Message, vbCritical)
        End Try
    End Sub



    Sub Bersih()
        TxtNIS.Clear()
        TxtNama.Clear()
        ComboBoxGender.SelectedIndex = -1
        ComboBoxKelas.Text = ""
        TxtNoHp.Clear()
        RichTxtAlamat.Clear()
        isEdit = False
        nisEdit = ""
    End Sub

    Private Sub TxtCari_TextChanged(sender As Object, e As EventArgs) Handles TxtCari.TextChanged
        Try
            bukaKoneksi()
            sql = "
            SELECT 
                nis AS 'NIS', 
                nama AS 'Nama', 
                gender AS 'Gender', 
                kelas AS 'Kelas', 
                no_hp AS 'No.HP', 
                alamat AS 'Alamat' 
            FROM data_siswa 
            WHERE 
                nis LIKE '%" & TxtCari.Text & "%' 
                OR nama LIKE '%" & TxtCari.Text & "%'"

            da = New MySqlDataAdapter(sql, conn)
            ds = New DataSet()
            da.Fill(ds, "data_siswa")
            DataGridView1.DataSource = ds.Tables("data_siswa")

            '=== Jika hasil pencarian kosong ===
            If ds.Tables("data_siswa").Rows.Count = 0 Then
                ComboBoxKelas.Text = ""
            End If

        Catch ex As Exception
            MsgBox("Gagal mencari data: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub ComboBoxKelas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxKelas.SelectedIndexChanged

    End Sub
End Class