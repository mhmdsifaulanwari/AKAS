Imports System.Drawing.Printing
Imports MySql.Data.MySqlClient

Public Class LaporanTabungan
    Private WithEvents PrintDocument1 As New Printing.PrintDocument
    Private PrintPreviewDialog1 As New PrintPreviewDialog

    Private currentRow As Integer = 0
    Private pageNumber As Integer = 1
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
            .Columns.Add("nominal", "Nominal")
            .Columns.Add("staf", "Staf")
            .Columns.Add("tanggal", "Tanggal")

            ' === Atur kolom utama ===
            .Columns("nis").Width = 200
            .Columns("nama").Width = 250
            .Columns("kelas").Width = 150
            .Columns("pembayaran").Width = 138
            .Columns("nominal").Width = 150
            .Columns("staf").Width = 250
            .Columns("tanggal").Width = 110

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
            Dim sql As String = "SELECT nis, nama, kelas, pembayaran, staf, nominal, tanggal, keterangan 
                                 FROM pembayaran 
                                 WHERE pembayaran = 'Tabungan'
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
                data(4) = row("nominal").ToString()
                data(5) = GetNamaStaff(row("staf").ToString())
                data(6) = CDate(row("tanggal")).ToString("dd/MM/yyyy")

                DataGridView1.Rows.Add(data)
            Next
        Catch ex As Exception
            MsgBox("Gagal memuat tabungan: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub LaporanTabungan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call bukaKoneksi()
        Call AturKolomLaporan()
        Call TampilLaporan()
    End Sub

    Private Sub TxtCari_TextChanged(sender As Object, e As EventArgs) Handles TxtCari.TextChanged
        Try
            Call bukaKoneksi()
            Dim cari As String = TxtCari.Text.Trim()

            Dim sql As String
            If cari = "" Then
                sql = "SELECT nis, nama, kelas, pembayaran, nominal, staf, tanggal, keterangan 
                       FROM pembayaran 
                       WHERE pembayaran = 'Tabungan'
                       ORDER BY kelas ASC, nama ASC"
            Else
                sql = "SELECT nis, nama, kelas, pembayaran, nominal, staf, tanggal, keterangan 
                       FROM pembayaran 
                       WHERE pembayaran = 'Tabungan' AND (nis LIKE @cari OR nama LIKE @cari)
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
                data(4) = row("nominal").ToString()
                data(5) = GetNamaStaff(row("staf").ToString())
                data(6) = CDate(row("tanggal")).ToString("dd/MM/yyyy")

                DataGridView1.Rows.Add(data)
            Next
        Catch ex As Exception
            MsgBox("Gagal mencari data: " & ex.Message, vbCritical)
        End Try
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
    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        e.PageSettings.Landscape = True
        Dim leftMargin As Integer = 40
        Dim topMargin As Integer = 40
        Dim yPos As Integer = topMargin
        Dim rowHeight As Integer = 25
        Dim fontJudul As New Font("Arial", 14, FontStyle.Bold)
        Dim fontHeader As New Font("Arial", 9, FontStyle.Bold)
        Dim fontIsi As New Font("Arial", 9)
        Dim brush As Brush = Brushes.Black

        ' === Judul Laporan ===
        e.Graphics.DrawString("Laporan Tabungan Siswa", fontJudul, brush, leftMargin, yPos)
        yPos += 35

        ' === Header Kolom ===
        ' (Tanpa kolom "Pembayaran")
        Dim headers() As String = {"NIS", "Nama", "Kelas", "Nominal", "Staf", "Tanggal"}
        Dim colWidths() As Integer = {100, 200, 80, 100, 200, 100}

        Dim xPos As Integer = leftMargin
        For i As Integer = 0 To headers.Length - 1
            e.Graphics.DrawString(headers(i), fontHeader, brush, xPos, yPos)
            xPos += colWidths(i)
        Next
        yPos += rowHeight
        e.Graphics.DrawLine(Pens.Black, leftMargin, yPos, leftMargin + colWidths.Sum(), yPos)

        ' === Isi Data ===
        Dim maxRowsPerPage As Integer = CInt((e.MarginBounds.Height - 100) / rowHeight)
        Dim rowsPrinted As Integer = 0

        While currentRow < DataGridView1.Rows.Count AndAlso rowsPrinted < maxRowsPerPage
            Dim row As DataGridViewRow = DataGridView1.Rows(currentRow)
            xPos = leftMargin

            ' Ambil data tanpa kolom "pembayaran"
            Dim data() As String = {
                row.Cells("nis").Value?.ToString(),
                row.Cells("nama").Value?.ToString(),
                row.Cells("kelas").Value?.ToString(),
                row.Cells("nominal").Value?.ToString(),
                row.Cells("staf").Value?.ToString(),
                row.Cells("tanggal").Value?.ToString()
            }

            For i As Integer = 0 To data.Length - 1
                e.Graphics.DrawString(data(i), fontIsi, brush, xPos, yPos)
                xPos += colWidths(i)
            Next

            yPos += rowHeight
            currentRow += 1
            rowsPrinted += 1
        End While

        ' === Nomor Halaman ===
        e.Graphics.DrawString("Halaman: " & pageNumber, fontIsi, brush, e.MarginBounds.Right - 100, e.MarginBounds.Bottom + 30)

        ' === Cek Halaman Berikut ===
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