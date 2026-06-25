Imports MySql.Data.MySqlClient
Imports System.Drawing.Printing

Public Class LaporanSpp
    Private WithEvents PrintDocument1 As New Printing.PrintDocument
    Private PrintPreviewDialog1 As New PrintPreviewDialog

    Private currentRow As Integer = 0
    Private pageNumber As Integer = 1
    Private Sub LaporanSpp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call bukaKoneksi()
        Call AturKolomLaporan()
        Call TampilLaporan()

    End Sub
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
            result = nip ' fallback: tampilkan NIP jika nama tidak ditemukan
        End Try
        Return result
    End Function

    Sub AturKolomLaporan()
        With DataGridView1
            .Columns.Clear()
            .Columns.Add("nis", "NIS")
            .Columns.Add("nama", "Nama")
            .Columns.Add("kelas", "Kelas")
            .Columns.Add("pembayaran", "Pembayaran")
            .Columns.Add("staf", "Staf")
            .Columns.Add("tanggal", "Tanggal")
            .Columns.Add("jan", "Jan")
            .Columns.Add("feb", "Feb")
            .Columns.Add("mar", "Mar")
            .Columns.Add("apr", "Apr")
            .Columns.Add("mei", "Mei")
            .Columns.Add("jun", "Jun")
            .Columns.Add("jul", "Jul")
            .Columns.Add("agu", "Agu")
            .Columns.Add("sep", "Sep")
            .Columns.Add("okt", "Okt")
            .Columns.Add("nov", "Nov")
            .Columns.Add("des", "Des")

            ' === Atur kolom utama ===
            .Columns("nis").Width = 100
            .Columns("nama").Width = 200
            .Columns("kelas").Width = 120
            .Columns("pembayaran").Width = 100
            .Columns("staf").Width = 200
            .Columns("tanggal").Width = 100

            ' === Atur kolom bulan satu per satu ===
            .Columns("jan").Width = 30
            .Columns("feb").Width = 30
            .Columns("mar").Width = 30
            .Columns("apr").Width = 30
            .Columns("mei").Width = 30
            .Columns("jun").Width = 30
            .Columns("jul").Width = 30
            .Columns("agu").Width = 30
            .Columns("sep").Width = 30
            .Columns("okt").Width = 30
            .Columns("nov").Width = 30
            .Columns("des").Width = 30

            ' === Tengah-kan isi kolom bulan ===
            Dim bulan() As String = {"jan", "feb", "mar", "apr", "mei", "jun", "jul", "agu", "sep", "okt", "nov", "des"}
            For Each b As String In bulan
                .Columns(b).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            Next

            ' === Nonaktifkan edit manual ===
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .MultiSelect = False
            .RowHeadersVisible = False
        End With
    End Sub

    Sub TampilLaporan()
        Try
            Call bukaKoneksi()
            Dim sql As String = "SELECT nis, nama, kelas, pembayaran, staf, tanggal, keterangan 
                                 FROM pembayaran 
                                 WHERE pembayaran = 'SPP'
                                 ORDER BY kelas ASC, nama ASC"

            Dim da As New MySqlDataAdapter(sql, conn)
            Dim dt As New DataTable
            da.Fill(dt)

            DataGridView1.Rows.Clear()

            For Each row As DataRow In dt.Rows
                Dim data(17) As String
                data(0) = row("nis").ToString()
                data(1) = row("nama").ToString()
                data(2) = row("kelas").ToString()
                data(3) = row("pembayaran").ToString()
                data(4) = GetNamaStaff(row("staf").ToString())
                data(5) = CDate(row("tanggal")).ToString("dd/MM/yyyy")

                Dim ket As String = row("keterangan").ToString().ToLower()
                data(6) = If(ket.Contains("januari"), "✔", "")
                data(7) = If(ket.Contains("februari"), "✔", "")
                data(8) = If(ket.Contains("maret"), "✔", "")
                data(9) = If(ket.Contains("april"), "✔", "")
                data(10) = If(ket.Contains("mei"), "✔", "")
                data(11) = If(ket.Contains("juni"), "✔", "")
                data(12) = If(ket.Contains("juli"), "✔", "")
                data(13) = If(ket.Contains("agustus"), "✔", "")
                data(14) = If(ket.Contains("september"), "✔", "")
                data(15) = If(ket.Contains("oktober"), "✔", "")
                data(16) = If(ket.Contains("november"), "✔", "")
                data(17) = If(ket.Contains("desember"), "✔", "")

                DataGridView1.Rows.Add(data)
            Next
            ' === Atur lebar kolom laporan SPP ===
            With DataGridView1
                ' Kolom utama
                .Columns("nis").Width = 80
                .Columns("nama").Width = 180
                .Columns("kelas").Width = 140
                .Columns("pembayaran").Width = 90
                .Columns("staf").Width = 180
                .Columns("tanggal").Width = 90

                ' Kolom bulan
                .Columns("jan").Width = 40
                .Columns("feb").Width = 40
                .Columns("mar").Width = 40
                .Columns("apr").Width = 40
                .Columns("mei").Width = 40
                .Columns("jun").Width = 40
                .Columns("jul").Width = 40
                .Columns("agu").Width = 40
                .Columns("sep").Width = 40
                .Columns("okt").Width = 40
                .Columns("nov").Width = 40
                .Columns("des").Width = 40

                ' Tengahkan semua centang SPP
                Dim bulan() As String = {"jan", "feb", "mar", "apr", "mei", "jun", "jul", "agu", "sep", "okt", "nov", "des"}
                For Each b In bulan
                    .Columns(b).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                Next

                ' Style baris lebih rapi
                .RowTemplate.Height = 28
            End With

        Catch ex As Exception
            MsgBox("Gagal memuat laporan: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub TxtCari_TextChanged(sender As Object, e As EventArgs) Handles TxtCari.TextChanged
        Try
            Call bukaKoneksi()
            Dim cari As String = TxtCari.Text.Trim()

            Dim sql As String
            If cari = "" Then
                sql = "SELECT nis, nama, kelas, pembayaran, staf, tanggal, keterangan 
                       FROM pembayaran 
                       WHERE pembayaran = 'SPP'
                       ORDER BY kelas ASC, nama ASC"
            Else
                sql = "SELECT nis, nama, kelas, pembayaran, staf, tanggal, keterangan 
                       FROM pembayaran 
                       WHERE pembayaran = 'SPP' AND (nis LIKE @cari OR nama LIKE @cari)
                       ORDER BY kelas ASC, nama ASC"
            End If

            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@cari", "%" & cari & "%")

            Dim da As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)

            DataGridView1.Rows.Clear()

            For Each row As DataRow In dt.Rows
                Dim data(17) As String
                data(0) = row("nis").ToString()
                data(1) = row("nama").ToString()
                data(2) = row("kelas").ToString()
                data(3) = row("pembayaran").ToString()
                data(4) = GetNamaStaff(row("staf").ToString())
                data(5) = CDate(row("tanggal")).ToString("dd/MM/yyyy")

                Dim ket As String = row("keterangan").ToString().ToLower()
                data(6) = If(ket.Contains("januari"), "✔", "")
                data(7) = If(ket.Contains("februari"), "✔", "")
                data(8) = If(ket.Contains("maret"), "✔", "")
                data(9) = If(ket.Contains("april"), "✔", "")
                data(10) = If(ket.Contains("mei"), "✔", "")
                data(11) = If(ket.Contains("juni"), "✔", "")
                data(12) = If(ket.Contains("juli"), "✔", "")
                data(13) = If(ket.Contains("agustus"), "✔", "")
                data(14) = If(ket.Contains("september"), "✔", "")
                data(15) = If(ket.Contains("oktober"), "✔", "")
                data(16) = If(ket.Contains("november"), "✔", "")
                data(17) = If(ket.Contains("desember"), "✔", "")

                DataGridView1.Rows.Add(data)
            Next
        Catch ex As Exception
            MsgBox("Gagal mencari data: " & ex.Message, vbCritical)
        End Try
    End Sub



    Private Sub CetakLangsung()
        Try
            PrintDocument1.Print()
        Catch ex As Exception
            MsgBox("Gagal mencetak: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub BtnCetak_Click(sender As Object, e As EventArgs) Handles BtnCetak.Click
        ' Siapkan print preview sebelum mencetak
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
    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        ' === Pengaturan halaman ===
        e.PageSettings.Landscape = True
        Dim leftMargin As Integer = 40
        Dim topMargin As Integer = 40
        Dim yPos As Integer = topMargin
        Dim rowHeight As Integer = 22
        Dim fontJudul As New Font("Arial", 14, FontStyle.Bold)
        Dim fontHeader As New Font("Arial", 9, FontStyle.Bold)
        Dim fontIsi As New Font("Arial", 9)
        Dim brush As Brush = Brushes.Black

        ' === Judul ===
        e.Graphics.DrawString("Laporan Pembayaran SPP", fontJudul, brush, leftMargin, yPos)
        yPos += 35

        ' === Header kolom (tambahkan Staf dan Tanggal) ===
        Dim headers() As String = {
            "NIS", "Nama", "Kelas", "Staf", "Tanggal",
            "Jan", "Feb", "Mar", "Apr", "Mei", "Jun",
            "Jul", "Agu", "Sep", "Okt", "Nov", "Des"
        }

        ' === Lebar kolom disesuaikan agar muat di halaman A4 landscape ===
        Dim colWidths() As Integer = {
            75,   ' NIS
            140,  ' Nama
            80,   ' Kelas
            120,  ' Staf
            80,   ' Tanggal
            24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24
        }

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

            ' Ambil data termasuk staf dan tanggal
            Dim data() As String = {
                row.Cells("nis").Value?.ToString(),
                row.Cells("nama").Value?.ToString(),
                row.Cells("kelas").Value?.ToString(),
                row.Cells("staf").Value?.ToString(),
                row.Cells("tanggal").Value?.ToString(),
                row.Cells("jan").Value?.ToString(),
                row.Cells("feb").Value?.ToString(),
                row.Cells("mar").Value?.ToString(),
                row.Cells("apr").Value?.ToString(),
                row.Cells("mei").Value?.ToString(),
                row.Cells("jun").Value?.ToString(),
                row.Cells("jul").Value?.ToString(),
                row.Cells("agu").Value?.ToString(),
                row.Cells("sep").Value?.ToString(),
                row.Cells("okt").Value?.ToString(),
                row.Cells("nov").Value?.ToString(),
                row.Cells("des").Value?.ToString()
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
End Class
