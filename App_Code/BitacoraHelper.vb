Imports System.Data.SqlClient
Imports System.Configuration

''' <summary>
''' Helper para registrar eventos en bitácora de forma más simple
''' </summary>
Public Class BitacoraHelper
    
    ''' <summary>Registra un evento CREAR</summary>
    Public Shared Sub RegistrarCrear(tabla As String, idRegistro As String, datos As String)
        Try
            BitacoraEvento.RegistrarEventoConEntidad("CREATE", tabla, idRegistro, Nothing, datos, "Se creó " & tabla & " ID: " & idRegistro)
        Catch
        End Try
    End Sub
    
    ''' <summary>Registra un evento ACTUALIZAR</summary>
    Public Shared Sub RegistrarActualizar(tabla As String, idRegistro As String, datosNuevos As String)
        Try
            BitacoraEvento.RegistrarEventoConEntidad("UPDATE", tabla, idRegistro, "Datos anteriores", datosNuevos, "Se actualizó " & tabla & " ID: " & idRegistro)
        Catch
        End Try
    End Sub
    
    ''' <summary>Registra un evento ELIMINAR</summary>
    Public Shared Sub RegistrarEliminar(tabla As String, idRegistro As String, datosAnteriores As String)
        Try
            BitacoraEvento.RegistrarEventoConEntidad("DELETE", tabla, idRegistro, datosAnteriores, Nothing, "Se eliminó " & tabla & " ID: " & idRegistro)
        Catch
        End Try
    End Sub
    
    ''' <summary>Registra un error</summary>
    Public Shared Sub RegistrarError(ex As Exception, pagina As String)
        Try
            BitacoraError.RegistrarError(ex, pagina, "Error en " & pagina)
        Catch
        End Try
    End Sub
    
    ''' <summary>Registra un evento de login (exitoso o fallido)</summary>
    Public Shared Sub RegistrarLogin(username As String, esExitoso As Boolean, mensaje As String, Optional idUsuario As Integer? = Nothing)
        Try
            Dim tipoEvento As String = If(esExitoso, "LOGIN_SUCCESS", "LOGIN_FAIL")
            BitacoraEvento.RegistrarEvento(tipoEvento, mensaje, idUsuario, username)
        Catch
        End Try
    End Sub
    
    ''' <summary>Registra un evento de logout</summary>
    Public Shared Sub RegistrarLogout(username As String, mensaje As String, Optional idUsuario As Integer? = Nothing)
        Try
            BitacoraEvento.RegistrarEvento("LOGOUT", mensaje, idUsuario, username)
        Catch
        End Try
    End Sub
    
End Class

