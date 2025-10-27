Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

''' <summary>
''' Clase simple para registrar errores en bitácora
''' </summary>
Public Class BitacoraError
    
    ''' <summary>Registra un error en la bitácora</summary>
    Public Shared Sub RegistrarError(ex As Exception, pagina As String, mensaje As String)
        Try
            ' Obtener información del usuario actual
            Dim idUsuario As Integer? = Nothing
            Dim nombreUsuario As String = Nothing
            Dim ip As String = Nothing
            
            Try
                If HttpContext.Current IsNot Nothing Then
                    If HttpContext.Current.User IsNot Nothing AndAlso HttpContext.Current.User.Identity.IsAuthenticated Then
                        Dim sessionId As Object = HttpContext.Current.Session("UserId")
                        If sessionId IsNot Nothing Then
                            idUsuario = Convert.ToInt32(sessionId)
                        End If
                        nombreUsuario = HttpContext.Current.User.Identity.Name
                    End If
                    ip = HttpContext.Current.Request.UserHostAddress
                End If
            Catch
                ' Si hay error, continuar sin usuario
            End Try
            
            ' Información del error
            Dim tipoError As String = If(ex IsNot Nothing, ex.GetType().Name, "Error Desconocido")
            Dim mensajeError As String = If(ex IsNot Nothing, ex.Message, mensaje)
            Dim detalleError As String = If(ex IsNot Nothing, ex.StackTrace, Nothing)
            
            ' Insertar en bitácora
            Using connection As New SqlConnection(ConfigurationManager.ConnectionStrings("BDGanaderiaConnectionString").ConnectionString)
                connection.Open()
                Using cmd As New SqlCommand("InsertarBitacoraError", connection)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@ID_Usuario", If(idUsuario, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Nombre_Usuario", If(nombreUsuario, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Tipo_Error", tipoError)
                    cmd.Parameters.AddWithValue("@Mensaje", mensajeError)
                    cmd.Parameters.AddWithValue("@Detalle_Error", If(detalleError, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Pagina", If(pagina, DBNull.Value))
                    cmd.Parameters.AddWithValue("@IP_Usuario", If(ip, DBNull.Value))
                    
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch
            ' Silenciar errores para no romper el flujo principal
        End Try
    End Sub
    
End Class

