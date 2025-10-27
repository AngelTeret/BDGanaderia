Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Namespace Reports
    Public Class ReportsRepository
        
        ''' <summary>
        ''' Ejecuta un stored procedure de reporte y devuelve los resultados en un DataTable
        ''' </summary>
        Public Shared Function ExecuteReport(storedProcedureName As String, parameters As Dictionary(Of String, Object)) As DataTable
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("BDGanaderiaConnectionString").ConnectionString
            Dim dt As New DataTable()
            
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Using cmd As New SqlCommand(storedProcedureName, connection)
                    cmd.CommandType = CommandType.StoredProcedure
                    
                    ' Agregar parámetros
                    If parameters IsNot Nothing Then
                        For Each param In parameters
                            If param.Value IsNot Nothing Then
                                cmd.Parameters.AddWithValue(param.Key, param.Value)
                            End If
                        Next
                    End If
                    
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
            
            Return dt
        End Function
        
        ''' <summary>
        ''' Obtiene datos para llenar un dropdown select (combos de filtros)
        ''' </summary>
        Public Shared Function GetDropdownData(storedProcedureName As String, textField As String, valueField As String) As List(Of KeyValuePair(Of String, String))
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("BDGanaderiaConnectionString").ConnectionString
            Dim list As New List(Of KeyValuePair(Of String, String))
            
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Using cmd As New SqlCommand(storedProcedureName, connection)
                    cmd.CommandType = CommandType.StoredProcedure
                    
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim value As String = reader(valueField).ToString()
                            Dim text As String = reader(textField).ToString()
                            list.Add(New KeyValuePair(Of String, String)(value, text))
                        End While
                    End Using
                End Using
            End Using
            
            Return list
        End Function
    End Class
End Namespace

