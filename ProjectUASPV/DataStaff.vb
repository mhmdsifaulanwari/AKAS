Imports MySql.Data.MySqlClient

Public Class DataStaff
    Dim da As MySqlDataAdapter
    Dim ds As DataSet
    Dim sql As String
    Dim dt As DataTable
    Dim cmd As MySqlCommand
    '=== Tambahan untuk mode edit ===
    Dim isEdit As Boolean = False
    Dim nipEdit As String = ""


    '=== Tampilkan data ke DataGridView ===
    Sub TampilData()
        Try
            bukaKoneksi()
            sql = "SELECT nip AS 'NIP', nama AS 'Nama', gender AS 'Gender', jabatan AS 'Jabatan', no_hp AS 'No.HP', alamat AS 'Alamat' FROM data_staff"
            da = New MySqlDataAdapter(sql, conn)
            ds = New DataSet()
            da.Fill(ds, "data_staff")
            DataGridView1.DataSource = ds.Tables("data_staff")

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

            ' Tambahkan tombol Edit & Hapus jika belum ada
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
            MsgBox("Gagal menampilkan data: " & ex.Message)
        End Try
    End Sub

    Private Sub DataStaff_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Isi gender
        TxtGender.Items.Clear()
        TxtGender.Items.Add("Laki-laki")
        TxtGender.Items.Add("Perempuan")

        TampilData()
    End Sub


    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Try
            If e.RowIndex >= 0 Then
                Dim kolom As String = DataGridView1.Columns(e.ColumnIndex).Name
                Dim nip As String = DataGridView1.Rows(e.RowIndex).Cells("NIP").Value.ToString()

                ' Tombol Edit ditekan
                If kolom = "Edit" Then
                    TxtNIP.Text = DataGridView1.Rows(e.RowIndex).Cells("NIP").Value.ToString()
                    TxtNama.Text = DataGridView1.Rows(e.RowIndex).Cells("Nama").Value.ToString()
                    TxtGender.Text = DataGridView1.Rows(e.RowIndex).Cells("Gender").Value.ToString()
                    TxtJabatan.Text = DataGridView1.Rows(e.RowIndex).Cells("Jabatan").Value.ToString()
                    TxtNoHp.Text = DataGridView1.Rows(e.RowIndex).Cells("No.HP").Value.ToString()
                    RichTxtAlamat.Text = DataGridView1.Rows(e.RowIndex).Cells("Alamat").Value.ToString()

                    '=== Mode Edit Aktif ===
                    isEdit = True
                    nipEdit = nip
                    MsgBox("Mode Edit Aktif! Silakan ubah data lalu tekan tombol Simpan.", vbInformation)
                End If

                ' Tombol Hapus ditekan
                If kolom = "Hapus" Then
                    Dim konfirmasi = MsgBox("Yakin ingin menghapus data NIP: " & nip & " ?", vbYesNo + vbQuestion, "Konfirmasi Hapus")
                    If konfirmasi = vbYes Then
                        Call bukaKoneksi()
                        sql = "DELETE FROM data_staff WHERE nip='" & nip & "'"
                        cmd = New MySqlCommand(sql, conn)
                        cmd.ExecuteNonQuery()
                        MsgBox("Data berhasil dihapus!", vbInformation)
                        TampilData()
                    End If
                End If
            End If

        Catch ex As Exception
            MsgBox("Terjadi kesalahan: " & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Sub Bersih()
        TxtNIP.Clear()
        TxtNama.Clear()
        TxtGender.SelectedIndex = -1
        TxtNoHp.Clear()
        TxtJabatan.Clear()
        RichTxtAlamat.Clear()
    End Sub

    Private Sub TxtCari_TextChanged(sender As Object, e As EventArgs) Handles TxtCari.TextChanged
        Try
            bukaKoneksi()
            sql = "
            SELECT 
                nip AS 'NIP', 
                nama AS 'Nama', 
                gender AS 'Gender', 
                jabatan AS 'Jabatan', 
                no_hp AS 'No.HP', 
                alamat AS 'Alamat' 
            FROM data_staff 
            WHERE 
                nip LIKE '%" & TxtCari.Text & "%' 
                OR nama LIKE '%" & TxtCari.Text & "%'"

            da = New MySqlDataAdapter(sql, conn)
            ds = New DataSet()
            da.Fill(ds, "data_staff")
            DataGridView1.DataSource = ds.Tables("data_staff")

        Catch ex As Exception
            MsgBox("Gagal mencari data: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnSimpan_Click(sender As Object, e As EventArgs) Handles BtnSimpan.Click
        ' validasi input
        If TxtNIP.Text = "" Or TxtNama.Text = "" Or TxtGender.Text = "" Or TxtNoHp.Text = "" Or TxtJabatan.Text = "" Or RichTxtAlamat.Text = "" Then
            MsgBox("Semua data wajib diisi!", vbExclamation)
            Exit Sub
        End If

        Try
            ' pastikan koneksi terbuka (fungsi bukaKoneksi() harus membuka conn)
            bukaKoneksi()

            If isEdit Then
                ' UPDATE record (nipEdit berisi NIP lama sebelum diubah)
                sql = "UPDATE data_staff SET nip=@nip, nama=@nama, gender=@gender, jabatan=@jabatan, no_hp=@no_hp, alamat=@alamat WHERE nip=@nipEdit"
                cmd = New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@nip", TxtNIP.Text)
                cmd.Parameters.AddWithValue("@nama", TxtNama.Text)
                cmd.Parameters.AddWithValue("@gender", TxtGender.Text)
                cmd.Parameters.AddWithValue("@jabatan", TxtJabatan.Text)
                cmd.Parameters.AddWithValue("@no_hp", TxtNoHp.Text)
                cmd.Parameters.AddWithValue("@alamat", RichTxtAlamat.Text)
                cmd.Parameters.AddWithValue("@nipEdit", nipEdit)
                cmd.ExecuteNonQuery()

                MsgBox("Data berhasil diperbarui!", vbInformation)

                ' reset mode edit
                isEdit = False
                nipEdit = ""
            Else
                ' INSERT record baru
                sql = "INSERT INTO data_staff (nip, nama, gender, jabatan, no_hp, alamat) VALUES (@nip, @nama, @gender, @jabatan, @no_hp, @alamat)"
                cmd = New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@nip", TxtNIP.Text)
                cmd.Parameters.AddWithValue("@nama", TxtNama.Text)
                cmd.Parameters.AddWithValue("@gender", TxtGender.Text)
                cmd.Parameters.AddWithValue("@jabatan", TxtJabatan.Text)
                cmd.Parameters.AddWithValue("@no_hp", TxtNoHp.Text)
                cmd.Parameters.AddWithValue("@alamat", RichTxtAlamat.Text)
                cmd.ExecuteNonQuery()

                MsgBox("Data berhasil disimpan!", vbInformation)
            End If

            ' Bersihkan input dan refresh DataGridView
            Bersih()
            TampilData()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            MsgBox("Kesalahan database: " & ex.Message, vbCritical)
        Catch ex As Exception
            MsgBox("Terjadi kesalahan: " & ex.Message, vbCritical)
        Finally
            ' pastikan koneksi tertutup jika bukaKoneksi tidak menutupnya sendiri
            Try
                If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            Catch
            End Try
        End Try
    End Sub
End Class