Public Class Dashboard
    Public Sub OpenForm(ByVal F As Form)

        For Each frm As Form In Me.MdiChildren
            frm.Close()
        Next

        F.MdiParent = Me
        F.FormBorderStyle = FormBorderStyle.None
        F.StartPosition = FormStartPosition.Manual

        Dim mdiClientArea As Rectangle = Nothing
        For Each ctl As Control In Me.Controls
            If TypeOf ctl Is MdiClient Then
                mdiClientArea = ctl.ClientRectangle
                Exit For
            End If
        Next

        F.Location = New Point(0, mdiClientArea.Top)
        F.Size = mdiClientArea.Size

        F.Show()

    End Sub

    Private Sub DataSiswaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataSiswaToolStripMenuItem.Click
        OpenForm(New DataSiswa)
    End Sub

    Private Sub DataStaffToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataStaffToolStripMenuItem.Click
        OpenForm(New DataStaff)
    End Sub

    Private Sub DashboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DashboardToolStripMenuItem.Click
        OpenForm(New MenuDashboard)
    End Sub

    Private Sub LogoutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogoutToolStripMenuItem.Click

        Dim result As DialogResult = MessageBox.Show("Apakah Anda yakin ingin logout?", "Konfirmasi Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub

    Private Sub SPPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SPPToolStripMenuItem.Click
        OpenForm(New Pembayaran_lainnya)
    End Sub

    Private Sub LaporanSPPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LaporanSPPToolStripMenuItem.Click
        OpenForm(New LaporanSpp)
    End Sub

    Private Sub LaporanTabunganToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LaporanTabunganToolStripMenuItem.Click
        OpenForm(New LaporanTabungan)
    End Sub

    Private Sub LaporanLainnyaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LaporanLainnyaToolStripMenuItem.Click
        OpenForm(New LaporanKegiatan)
    End Sub

    

    Private Sub PengeluaranToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PengeluaranToolStripMenuItem.Click
        OpenForm(New Pengeluaran)
    End Sub

    Private Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub SetelanToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub PembayaranLainnyaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PembayaranLainnyaToolStripMenuItem.Click
        OpenForm(New PembayaranLainnya)
    End Sub
End Class