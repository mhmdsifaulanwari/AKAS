<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Dashboard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.DashboardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DataSiswaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DataStaffToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PembayaranToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SPPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PembayaranLainnyaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PengeluaranToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LaporanToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LaporanSPPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LaporanTabunganToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LaporanLainnyaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogoutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.Color.DodgerBlue
        Me.MenuStrip1.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuStrip1.GripMargin = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DashboardToolStripMenuItem, Me.DataToolStripMenuItem, Me.PembayaranToolStripMenuItem, Me.PengeluaranToolStripMenuItem, Me.LaporanToolStripMenuItem, Me.LogoutToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1593, 37)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'DashboardToolStripMenuItem
        '
        Me.DashboardToolStripMenuItem.ForeColor = System.Drawing.Color.White
        Me.DashboardToolStripMenuItem.Name = "DashboardToolStripMenuItem"
        Me.DashboardToolStripMenuItem.Size = New System.Drawing.Size(154, 33)
        Me.DashboardToolStripMenuItem.Text = "Dashboard"
        '
        'DataToolStripMenuItem
        '
        Me.DataToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DataSiswaToolStripMenuItem, Me.DataStaffToolStripMenuItem})
        Me.DataToolStripMenuItem.ForeColor = System.Drawing.Color.White
        Me.DataToolStripMenuItem.Name = "DataToolStripMenuItem"
        Me.DataToolStripMenuItem.Size = New System.Drawing.Size(80, 33)
        Me.DataToolStripMenuItem.Text = "Data"
        '
        'DataSiswaToolStripMenuItem
        '
        Me.DataSiswaToolStripMenuItem.Name = "DataSiswaToolStripMenuItem"
        Me.DataSiswaToolStripMenuItem.Size = New System.Drawing.Size(242, 38)
        Me.DataSiswaToolStripMenuItem.Text = "Data Siswa"
        '
        'DataStaffToolStripMenuItem
        '
        Me.DataStaffToolStripMenuItem.Name = "DataStaffToolStripMenuItem"
        Me.DataStaffToolStripMenuItem.Size = New System.Drawing.Size(242, 38)
        Me.DataStaffToolStripMenuItem.Text = "Data Staff"
        '
        'PembayaranToolStripMenuItem
        '
        Me.PembayaranToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SPPToolStripMenuItem, Me.PembayaranLainnyaToolStripMenuItem})
        Me.PembayaranToolStripMenuItem.ForeColor = System.Drawing.Color.White
        Me.PembayaranToolStripMenuItem.Name = "PembayaranToolStripMenuItem"
        Me.PembayaranToolStripMenuItem.Size = New System.Drawing.Size(161, 33)
        Me.PembayaranToolStripMenuItem.Text = "Pemasukan"
        '
        'SPPToolStripMenuItem
        '
        Me.SPPToolStripMenuItem.Name = "SPPToolStripMenuItem"
        Me.SPPToolStripMenuItem.Size = New System.Drawing.Size(306, 38)
        Me.SPPToolStripMenuItem.Text = "SPP + Tabungan"
        '
        'PembayaranLainnyaToolStripMenuItem
        '
        Me.PembayaranLainnyaToolStripMenuItem.Name = "PembayaranLainnyaToolStripMenuItem"
        Me.PembayaranLainnyaToolStripMenuItem.Size = New System.Drawing.Size(306, 38)
        Me.PembayaranLainnyaToolStripMenuItem.Text = "Pemasukan Lain"
        '
        'PengeluaranToolStripMenuItem
        '
        Me.PengeluaranToolStripMenuItem.ForeColor = System.Drawing.Color.White
        Me.PengeluaranToolStripMenuItem.Name = "PengeluaranToolStripMenuItem"
        Me.PengeluaranToolStripMenuItem.Size = New System.Drawing.Size(173, 33)
        Me.PengeluaranToolStripMenuItem.Text = "Pengeluaran"
        '
        'LaporanToolStripMenuItem
        '
        Me.LaporanToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LaporanSPPToolStripMenuItem, Me.LaporanTabunganToolStripMenuItem, Me.LaporanLainnyaToolStripMenuItem})
        Me.LaporanToolStripMenuItem.ForeColor = System.Drawing.Color.White
        Me.LaporanToolStripMenuItem.Name = "LaporanToolStripMenuItem"
        Me.LaporanToolStripMenuItem.Size = New System.Drawing.Size(124, 33)
        Me.LaporanToolStripMenuItem.Text = "Laporan"
        '
        'LaporanSPPToolStripMenuItem
        '
        Me.LaporanSPPToolStripMenuItem.Name = "LaporanSPPToolStripMenuItem"
        Me.LaporanSPPToolStripMenuItem.Size = New System.Drawing.Size(332, 38)
        Me.LaporanSPPToolStripMenuItem.Text = "Laporan SPP"
        '
        'LaporanTabunganToolStripMenuItem
        '
        Me.LaporanTabunganToolStripMenuItem.Name = "LaporanTabunganToolStripMenuItem"
        Me.LaporanTabunganToolStripMenuItem.Size = New System.Drawing.Size(332, 38)
        Me.LaporanTabunganToolStripMenuItem.Text = "Laporan Tabungan"
        '
        'LaporanLainnyaToolStripMenuItem
        '
        Me.LaporanLainnyaToolStripMenuItem.Name = "LaporanLainnyaToolStripMenuItem"
        Me.LaporanLainnyaToolStripMenuItem.Size = New System.Drawing.Size(332, 38)
        Me.LaporanLainnyaToolStripMenuItem.Text = "Laporan Lainnya"
        '
        'LogoutToolStripMenuItem
        '
        Me.LogoutToolStripMenuItem.ForeColor = System.Drawing.Color.White
        Me.LogoutToolStripMenuItem.Name = "LogoutToolStripMenuItem"
        Me.LogoutToolStripMenuItem.Size = New System.Drawing.Size(112, 33)
        Me.LogoutToolStripMenuItem.Text = "Logout"
        '
        'Dashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1593, 894)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.IsMdiContainer = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "Dashboard"
        Me.Text = "Form1"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents DashboardToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DataSiswaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DataStaffToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PembayaranToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SPPToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PengeluaranToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LaporanToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LaporanSPPToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LaporanTabunganToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LaporanLainnyaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LogoutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PembayaranLainnyaToolStripMenuItem As ToolStripMenuItem
End Class
