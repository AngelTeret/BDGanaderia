Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

''' <summary>
''' Clase simple para registrar eventos en bitácora
''' </summary>
Public Class BitacoraEvento
    
    ''' <summary>Registra un evento en la bitácora</summary>
    Public Shared Sub RegistrarEvento(tipoEvento As String, mensaje As String, Optional pIdUsuario As Integer? = Nothing, Optional pNombreUsuario As String = Nothing)
        RegistrarEventoConEntidad(tipoEvento, Nothing, Nothing, Nothing, Nothing, mensaje, pIdUsuario, pNombreUsuario)
    End Sub
    
    ''' <summary>Registra un evento con información de entidad</summary>
    Public Shared Sub RegistrarEventoConEntidad(tipoEvento As String, entidad As String, idRegistro As String, valoresAntes As String, valoresNuevos As String, mensaje As String, Optional pIdUsuario As Integer? = Nothing, Optional pNombreUsuario As String = Nothing)
        Try
            ' Obtener información del usuario actual
            Dim idUsuario As Integer? = pIdUsuario
            Dim nombreUsuario As String = pNombreUsuario
            Dim ip As String = Nothing
            
            ' Si no se pasaron parámetros, obtener del contexto
            If Not idUsuario.HasValue OrElse String.IsNullOrEmpty(nombreUsuario) Then
                Try
                    If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.User IsNot Nothing AndAlso HttpContext.Current.User.Identity.IsAuthenticated Then
                        If Not idUsuario.HasValue Then
                            Dim sessionId As Object = HttpContext.Current.Session("UserId")
                            If sessionId IsNot Nothing Then
                                idUsuario = Convert.ToInt32(sessionId)
                            End If
                        End If
                        If String.IsNullOrEmpty(nombreUsuario) Then
                            nombreUsuario = HttpContext.Current.User.Identity.Name
                        End If
                    End If
                    ip = HttpContext.Current.Request.UserHostAddress
                Catch
                    ' Si hay error, continuar sin usuario
                End Try
            Else
                ' Si se pasaron parámetros, obtener solo la IP
                Try
                    ip = HttpContext.Current.Request.UserHostAddress
                Catch
                End Try
            End If
            
            ' Insertar en bitácora (llamada directa, no async para mantener simple)
            Using connection As New SqlConnection(ConfigurationManager.ConnectionStrings("BDGanaderiaConnectionString").ConnectionString)
                connection.Open()
                Using cmd As New SqlCommand("InsertarBitacoraEvento", connection)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@ID_Usuario", If(idUsuario, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Nombre_Usuario", If(nombreUsuario, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Tipo_Evento", tipoEvento)
                    cmd.Parameters.AddWithValue("@Entidad", If(entidad, DBNull.Value))
                    cmd.Parameters.AddWithValue("@ID_Registro", If(idRegistro, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Valores_Antes", If(valoresAntes, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Valores_Nuevos", If(valoresNuevos, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Mensaje", If(mensaje, DBNull.Value))
                    cmd.Parameters.AddWithValue("@IP_Usuario", If(ip, DBNull.Value))
                    
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch
            ' Silenciar errores para no romper el flujo principal
        End Try
    End Sub
    
End Class

