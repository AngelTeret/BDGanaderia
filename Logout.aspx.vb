Partial Public Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Cerrar sesión
        SimpleAuthService.SignOut()
        
        ' Redirigir al login
        Response.Redirect("~/Login.aspx", True)
    End Sub
End Class


