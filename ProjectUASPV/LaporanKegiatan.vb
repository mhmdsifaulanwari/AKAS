Imports System.Globalization
Imports MySql.Data.MySqlClient

Public Class LaporanKegiatan
    Private WithEvents PrintDocument1 As New Printing.PrintDocument
    Private PrintPreviewDialog1 As New PrintPreviewDialog

    Private currentRow As Integer = 0
    Private pageNumber As Integer = 1
    Private Sub LaporanKegiatan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        bukaKoneksi()
        AturKolomLaporan()
        TampilLaporan()
    End Sub
    Sub AturKolomLaporan()
        With DataGridView1
            .Columns.Clear()
            .Columns.Add("pemasukan", "Pemasukan")
            .Columns.Add("tanggal", "Tanggal")
            .Columns.Add("staf", "Nama Staf")
            .Columns.Add("nominal", "Nominal")

            .Columns("pemasukan").Width = 311
            .Columns("tanggal").Width = 311
            .Columns("staf").Width = 311
            .Columns("nominal").Width = 311

            ' === Nonaktifkan edit manual ===
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .MultiSelect = False
            .RowHeadersVisible = False


            .Columns("nominal").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Sub

    '=== Menampilkan data pembayaran lainnya ===
    Sub TampilLaporan()
        Try
            bukaKoneksi()
            Dim sql As String =
                "SELECT pemasukan, tanggal, staf, nominal FROM pembayaran_lainnya ORDER BY tanggal DESC"
            cmd = New MySqlCommand(sql, conn)
            rd = cmd.ExecuteReader()

            DataGridView1.Rows.Clear()
            Dim totalNominal As Decimal = 0

            While rd.Read()
                Dim pemasukan As String = rd("pemasukan").ToString()
                Dim tanggal As Date = CDate(rd("tanggal"))
                Dim staf As String = rd("staf").ToString()
                Dim nominal As Decimal = CDec(rd("nominal"))

                totalNominal += nominal

                DataGridView1.Rows.Add(pemasukan, tanggal.ToString("dd/MM/yyyy"),
                                       staf, nominal.ToString("C0", New CultureInfo("id-ID")))
            End While
            rd.Close()
            conn.Close()

            '=== Tambahkan baris total ===
            Dim rowIndex As Integer = DataGridView1.Rows.Add()
            With DataGridView1.Rows(rowIndex)
                .Cells("pemasukan").Value = "TOTAL"

                .DefaultCellStyle.BackColor = Color.White
                .DefaultCellStyle.Font = New Font(DataGridView1.Font, FontStyle.Bold)
                .Cells("nominal").Value = totalNominal.ToString("C0", New CultureInfo("id-ID"))
            End With

            ' === Ambil total lainnya dari dashboard ===
            Dim totalDashboard As Decimal = 0

            Try
                bukaKoneksi()
                Dim sqlDash As String = "SELECT total_lainnya FROM dashboard LIMIT 1"
                Using cmdDash As New MySqlCommand(sqlDash, conn)
                    Dim hasil = cmdDash.ExecuteScalar()
                    If hasil IsNot DBNull.Value Then
                        totalDashboard = Convert.ToDecimal(hasil)
                    End If
                End Using
                conn.Close()
            Catch ex As Exception
                MsgBox("Gagal mengambil total dashboard: " & ex.Message, MsgBoxStyle.Critical)
            End Try

            ' === Tambahkan baris TOTAL DASHBOARD ===
            Dim dashIndex As Integer = DataGridView1.Rows.Add()
            With DataGridView1.Rows(dashIndex)
                .Cells("pemasukan").Value = "TOTAL (Setelah Pengeluaran)"
                .Cells("nominal").Value = totalDashboard.ToString("C0", New CultureInfo("id-ID"))

                .DefaultCellStyle.BackColor = Color.White
                .DefaultCellStyle.Font = New Font(DataGridView1.Font, FontStyle.Bold)
            End With


        Catch ex As Exception
            MsgBox("Gagal menampilkan laporan: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
    Private Sub TxtCari_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub BtnCetak_Click(sender As Object, e As EventArgs) Handles BtnCetak.Click
        Try
            currentRow = 0
            pageNumber = 1
            PrintPreviewDialog1.Document = PrintDocument1
            PrintPreviewDialog1.WindowState = FormWindowState.Maximized
            PrintPreviewDialog1.PrintPreviewControl.Zoom = 1.0
            PrintPreviewDialog1.ShowDialog()
        Catch ex As Exception
            MsgBox("Gagal menampilkan preview cetak: " & ex.Message, vbCritical)
        End Try
    End Sub

    ' === Proses Cetak ===
    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        e.PageSettings.Landscape = False ' Potrait cocok untuk laporan ini

        Dim leftMargin As Integer = 40
        Dim topMargin As Integer = 40
        Dim yPos As Integer = topMargin
        Dim rowHeight As Integer = 25
        Dim fontJudul As New Font("Arial", 14, FontStyle.Bold)
        Dim fontHeader As New Font("Arial", 9, FontStyle.Bold)
        Dim fontIsi As New Font("Arial", 9)
        Dim brush As Brush = Brushes.Black

        ' === Judul ===
        e.Graphics.DrawString("Laporan Pemasukan Lainnya", fontJudul, brush, leftMargin, yPos)
        yPos += 35

        ' === Header Kolom ===
        Dim headers() As String = {"Pemasukan", "Tanggal", "Nama Staf", "Nominal"}
        Dim colWidths() As Integer = {250, 120, 180, 120}

        Dim xPos As Integer = leftMargin
        For i As Integer = 0 To headers.Length - 1
            e.Graphics.DrawString(headers(i), fontHeader, brush, xPos, yPos)
            xPos += colWidths(i)
        Next
        yPos += rowHeight
        e.Graphics.DrawLine(Pens.Black, leftMargin, yPos, leftMargin + colWidths.Sum(), yPos)

        ' === Data ===
        Dim maxRowsPerPage As Integer = CInt((e.MarginBounds.Height - 100) / rowHeight)
        Dim rowsPrinted As Integer = 0

        While currentRow < DataGridView1.Rows.Count AndAlso rowsPrinted < maxRowsPerPage
            Dim row As DataGridViewRow = DataGridView1.Rows(currentRow)
            xPos = leftMargin

            Dim data() As String = {
                row.Cells("pemasukan").Value?.ToString(),
                row.Cells("tanggal").Value?.ToString(),
                row.Cells("staf").Value?.ToString(),
                row.Cells("nominal").Value?.ToString()
            }

            For i As Integer = 0 To data.Length - 1
                e.Graphics.DrawString(data(i), fontIsi, brush, xPos, yPos)
                xPos += colWidths(i)
            Next

            yPos += rowHeight
            currentRow += 1
            rowsPrinted += 1
        End While

        ' === Nomor halaman ===
        e.Graphics.DrawString("Halaman: " & pageNumber, fontIsi, brush, e.MarginBounds.Right - 100, e.MarginBounds.Bottom + 30)

        ' === Cek halaman berikutnya ===
        If currentRow < DataGridView1.Rows.Count Then
            e.HasMorePages = True
            pageNumber += 1
        Else
            e.HasMorePages = False
            currentRow = 0
            pageNumber = 1
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class