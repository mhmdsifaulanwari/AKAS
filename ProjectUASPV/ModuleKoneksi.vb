Imports MySql.Data.MySqlClient

Module ModuleKoneksi

    Public conn As MySqlConnection
    Public cmd As MySqlCommand
    Public dr As MySqlDataReader
    Public da As MySqlDataAdapter
    Public ds As DataSet
    Public rd As MySqlDataReader
    Public sql As String


    Public Sub bukaKoneksi()
        Try
            conn = New MySqlConnection("server=localhost;userid=root;password=admin123;database=db_keuangansekolah")

            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

        Catch ex As Exception
            MsgBox("Koneksi gagal: " & ex.Message)
        End Try
    End Sub

End Module