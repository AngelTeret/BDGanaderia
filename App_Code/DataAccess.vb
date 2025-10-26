Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class DataAccess
    Private Shared ReadOnly ConnectionString As String = ConfigurationManager.ConnectionStrings("BDGanaderiaConnectionString").ConnectionString

    ''' <summary>
    ''' Ejecuta una consulta SELECT y retorna un DataTable
    ''' </summary>
    Public Shared Function ExecuteSelectQuery(query As String, Optional parameters As SqlParameter() = Nothing) As DataTable
        Dim dataTable As New DataTable()
        
        Using connection As New SqlConnection(ConnectionString)
            Using command As New SqlCommand(query, connection)
                If parameters IsNot Nothing Then
                    command.Parameters.AddRange(parameters)
                End If
                
                connection.Open()
                Using adapter As New SqlDataAdapter(command)
                    adapter.Fill(dataTable)
                End Using
            End Using
        End Using
        
        Return dataTable
    End Function

    ''' <summary>
    ''' Ejecuta una consulta INSERT, UPDATE o DELETE y retorna el número de filas afectadas
    ''' </summary>
    Public Shared Function ExecuteNonQuery(query As String, Optional parameters As SqlParameter() = Nothing) As Integer
        Using connection As New SqlConnection(ConnectionString)
            Using command As New SqlCommand(query, connection)
                If parameters IsNot Nothing Then
                    command.Parameters.AddRange(parameters)
                End If
                
                connection.Open()
                Return command.ExecuteNonQuery()
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Ejecuta un procedimiento almacenado y retorna un DataTable
    ''' </summary>
    Public Shared Function ExecuteStoredProcedure(procedureName As String, Optional parameters As SqlParameter() = Nothing) As DataTable
        Dim dataTable As New DataTable()
        
        Using connection As New SqlConnection(ConnectionString)
            Using command As New SqlCommand(procedureName, connection)
                command.CommandType = CommandType.StoredProcedure
                
                If parameters IsNot Nothing Then
                    command.Parameters.AddRange(parameters)
                End If
                
                connection.Open()
                Using adapter As New SqlDataAdapter(command)
                    adapter.Fill(dataTable)
                End Using
            End Using
        End Using
        
        Return dataTable
    End Function

    ''' <summary>
    ''' Ejecuta un procedimiento almacenado y retorna un valor escalar
    ''' </summary>
    Public Shared Function ExecuteScalar(query As String, Optional parameters As SqlParameter() = Nothing) As Object
        Using connection As New SqlConnection(ConnectionString)
            Using command As New SqlCommand(query, connection)
                If parameters IsNot Nothing Then
                    command.Parameters.AddRange(parameters)
                End If
                
                connection.Open()
                Return command.ExecuteScalar()
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Verifica la conexión a la base de datos
    ''' </summary>
    Public Shared Function TestConnection() As Boolean
        Try
            Using connection As New SqlConnection(ConnectionString)
                connection.Open()
                Return True
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Obtiene el siguiente ID disponible para una tabla
    ''' </summary>
    Public Shared Function GetNextId(tableName As String) As Integer
        Dim query As String = "SELECT ISNULL(MAX(ID_" & tableName & "), 0) + 1 FROM " & tableName
        Dim result As Object = ExecuteScalar(query)
        Return If(result IsNot Nothing, Convert.ToInt32(result), 1)
    End Function
End Class
