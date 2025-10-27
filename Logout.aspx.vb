Partial Public Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Obtener información del usuario antes de cerrar sesión
        Dim username As String = "Anónimo"
        Dim userId As Integer? = Nothing
        If User IsNot Nothing AndAlso User.Identity.IsAuthenticated Then
            username = User.Identity.Name
            If Session("UserId") IsNot Nothing Then
                userId = Convert.ToInt32(Session("UserId"))
            End If
        End If
        
        ' Cerrar sesión
        SimpleAuthService.SignOut()
        
        ' Registrar en bitácora
        Try
            BitacoraHelper.RegistrarLogout(username, "Usuario cerró sesión", userId)
        Catch
        End Try
        
        ' Redirigir al login
        Response.Redirect("~/Login.aspx", True)
    End Sub
End Class


