Imports System.Data.SqlClient

Public Class SiteMaster
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If HttpContext.Current.User.Identity.IsAuthenticated Then
            ' Mostrar información del usuario
            Dim userId As Integer = SimpleAuthService.GetCurrentUserId()
            Dim roleName As String = SimpleAuthService.GetCurrentRole()
            
            ' Obtener nombre de usuario
            Try
                Dim query As String = "SELECT Nombre_Usuario FROM Usuario WHERE ID_Usuario = @ID_Usuario"
                Dim parameters() As SqlParameter = {
                    New SqlParameter("@ID_Usuario", userId)
                }
                
                Dim result As Object = DataAccess.ExecuteScalar(query, parameters)
                If result IsNot Nothing Then
                    userName.InnerText = result.ToString()
                End If
            Catch
                userName.InnerText = "Usuario"
            End Try
            
            userRole.InnerText = roleName
        End If
    End Sub

End Class
