Imports System.Web
Imports System.Data.SqlClient

Public Class SimpleSecurePage
    Inherits System.Web.UI.Page
    
    Protected CurrentUserId As Integer = 0
    Protected CurrentUserName As String = ""
    Protected CurrentRoleName As String = ""
    
    Protected Overrides Sub OnInit(e As EventArgs)
        MyBase.OnInit(e)
        
        ' Verificar autenticación
        If Not HttpContext.Current.User.Identity.IsAuthenticated Then
            Dim returnUrl As String = HttpContext.Current.Request.Url.PathAndQuery
            Response.Redirect("~/Login.aspx?ReturnUrl=" & HttpUtility.UrlEncode(returnUrl), True)
            Return
        End If
        
        ' Cargar información del usuario
        LoadUserInfo()
    End Sub
    
    ''' <summary>
    ''' Carga la información del usuario actual desde la sesión
    ''' </summary>
    Private Sub LoadUserInfo()
        CurrentUserId = SimpleAuthService.GetCurrentUserId()
        CurrentRoleName = SimpleAuthService.GetCurrentRole()
        
        ' Obtener nombre de usuario desde BD
        If CurrentUserId > 0 Then
            Try
                Dim query As String = "SELECT Nombre_Usuario FROM Usuario WHERE ID_Usuario = @ID_Usuario"
                Dim parameters() As SqlParameter = {
                    New SqlParameter("@ID_Usuario", CurrentUserId)
                }
                
                Dim result As Object = DataAccess.ExecuteScalar(query, parameters)
                If result IsNot Nothing Then
                    CurrentUserName = result.ToString()
                End If
            Catch
                CurrentUserName = "Usuario"
            End Try
        End If
    End Sub
    
    ''' <summary>
    ''' Requiere que el usuario tenga uno de los roles especificados
    ''' </summary>
    Protected Sub RequireRoles(ParamArray roles() As String)
        If Not SimpleAuthService.IsInRole(roles) Then
            Response.Write("<script>alert('Acceso denegado. No tienes permisos para acceder a esta página.'); window.location.href='Default.aspx';</script>")
            Response.End()
        End If
    End Sub
    
    ''' <summary>
    ''' Verifica si el usuario actual tiene uno de los roles especificados
    ''' </summary>
    Protected Function UserIsInRole(ParamArray roles() As String) As Boolean
        Return SimpleAuthService.IsInRole(roles)
    End Function
    
End Class

