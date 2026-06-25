Imports System.Globalization
Imports MySql.Data.MySqlClient

Public Class MenuDashboard
    Public Shared totalSPP_Tabungan As Double
    Public Shared totalLainnya As Double

    Private Sub MenuDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.ControlBox = False
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        Call bukaKoneksi()
        Call HitungTotalDashboard() ' <-- panggil saat load
    End Sub

    ' Fungsi hitung dan tampilkan total SPP, Tabungan, Lainnya
    Public Sub HitungTotalDashboard()
        Try
            bukaKoneksi()

            ' ======== HITUNG TOTAL SPP (DARI TABEL PEMBAYARAN) ========
            Dim sqlSPP As String = "SELECT IFNULL(SUM(nominal),0) FROM pembayaran WHERE pembayaran='SPP'"
            Dim totalSPP As Double = Convert.ToDouble(New MySqlCommand(sqlSPP, conn).ExecuteScalar())

            ' ======== HITUNG TOTAL TABUNGAN (JIKA DIPERLUKAN) ========
            Dim sqlTabungan As String = "SELECT IFNULL(SUM(nominal),0) FROM pembayaran WHERE pembayaran='Tabungan'"
            Dim totalTabungan As Double = Convert.ToDouble(New MySqlCommand(sqlTabungan, conn).ExecuteScalar())

            ' ======== HITUNG PEMASUKAN LAINNYA ========
            Dim sqlLainnya As String = "SELECT IFNULL(SUM(nominal),0) FROM pembayaran_lainnya"
            Dim totalLainnya As Double = Convert.ToDouble(New MySqlCommand(sqlLainnya, conn).ExecuteScalar())

            ' ======== HITUNG TOTAL PENGELUARAN SPP ========
            Dim sqlPengelSPP As String = "SELECT IFNULL(SUM(nominal),0) FROM pengeluaran WHERE pengeluaran='spp'"
            Dim totalKeluarSPP As Double = Convert.ToDouble(New MySqlCommand(sqlPengelSPP, conn).ExecuteScalar())

            ' ======== HITUNG TOTAL PENGELUARAN LAINNYA ========
            Dim sqlPengelLain As String = "SELECT IFNULL(SUM(nominal),0) FROM pengeluaran WHERE pengeluaran='lainnya'"
            Dim totalKeluarLainnya As Double = Convert.ToDouble(New MySqlCommand(sqlPengelLain, conn).ExecuteScalar())

            ' ======== TOTAL AKHIR SETELAH PENGELUARAN ========
            Dim finalSPP As Double = (totalSPP + totalTabungan) - totalKeluarSPP
            Dim finalLainnya As Double = totalLainnya - totalKeluarLainnya

            ' ======== TAMPILKAN ==========
            LabelSPP.Text = "Rp" & finalSPP.ToString("N0", CultureInfo.GetCultureInfo("id-ID"))
            LabelTabungan.Text = "Rp" & totalTabungan.ToString("N0", CultureInfo.GetCultureInfo("id-ID"))
            LabelLainnya.Text = "Rp" & finalLainnya.ToString("N0", CultureInfo.GetCultureInfo("id-ID"))

            ' ======== SIMPAN KE TABEL DASHBOARD ========
            Dim sqlUp As String =
                "UPDATE dashboard SET total_spp=@spp, total_tabungan=@tab, total_lainnya=@lain"

            Using cmd As New MySqlCommand(sqlUp, conn)
                cmd.Parameters.AddWithValue("@spp", finalSPP)
                cmd.Parameters.AddWithValue("@tab", totalTabungan)
                cmd.Parameters.AddWithValue("@lain", finalLainnya)
                cmd.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            MsgBox("Gagal menghitung dashboard: " & ex.Message)
        End Try
    End Sub

End Class
