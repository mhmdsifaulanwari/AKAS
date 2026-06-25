Imports MySql.Data.MySqlClient

Public Class Login
    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TxtUsername.Select()
        TxtPassword.UseSystemPasswordChar = True
        ' Jangan set AcceptButton di sini dulu
        Me.AcceptButton = Nothing
    End Sub

    ' === Tekan ENTER di Username → pindah ke Password ===
    Private Sub TxtUsername_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtUsername.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            TxtPassword.Focus()
        End If
    End Sub

    ' === Setelah masuk ke Password, baru Enter aktifkan tombol Login ===
    Private Sub TxtPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Button1.PerformClick()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Call bukaKoneksi()

        ' Pakai parameter biar aman
        cmd = New MySqlCommand("SELECT * FROM login WHERE username=@user AND password=@pass", conn)
        cmd.Parameters.AddWithValue("@user", TxtUsername.Text)
        cmd.Parameters.AddWithValue("@pass", TxtPassword.Text)
        dr = cmd.ExecuteReader()

        If dr.HasRows Then
            MessageBox.Show("Selamat datang, " & TxtUsername.Text & "!", "Login Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Dim frmMenuDasbor As New MenuDashboard()
            Dashboard.Show()
            Dashboard.OpenForm(frmMenuDasbor)
            Me.Hide()
        Else
            MessageBox.Show("Username atau password salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TxtUsername.Clear()
            TxtPassword.Clear()
            TxtUsername.Focus()
        End If

        dr.Close()
        conn.Close()
    End Sub

    Private Sub CheckPassword_CheckedChanged(sender As Object, e As EventArgs) Handles CheckPassword.CheckedChanged
        TxtPassword.UseSystemPasswordChar = Not CheckPassword.Checked
    End Sub
End Class
