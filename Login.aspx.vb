Imports System.Web

Partial Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Si ya está autenticado, redirigir al dashboard
        If User.Identity.IsAuthenticated Then
            Response.Redirect("~/Default.aspx", True)
        End If
        
        ' Enfocar en username al cargar
        If Not IsPostBack Then
            txtUsername.Focus()
        End If
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        Try
            Dim username As String = txtUsername.Text.Trim()
            Dim password As String = txtPassword.Text
            Dim rememberMe As Boolean = chkRemember.Checked
            
            ' Validar campos
            If String.IsNullOrWhiteSpace(username) Then
                ShowError("El nombre de usuario es requerido")
                Return
            End If
            
            If String.IsNullOrWhiteSpace(password) Then
                ShowError("La contraseña es requerida")
                Return
            End If
            
            ' Validar usuario
            Dim authResult = SimpleAuthService.ValidateUser(username, password)
            
            If authResult.IsValid Then
                ' Iniciar sesión
                SimpleAuthService.SignIn(authResult.UserId, username, authResult.RoleName, rememberMe)
                
                ' Redirigir
                Dim returnUrl As String = Request.QueryString("ReturnUrl")
                
                ' Validar que ReturnUrl sea local
                If Not String.IsNullOrWhiteSpace(returnUrl) AndAlso returnUrl.StartsWith("/") Then
                    Response.Redirect(returnUrl, True)
                Else
                    Response.Redirect("~/Default.aspx", True)
                End If
            Else
                ShowError("Usuario o contraseña incorrectos")
            End If
            
        Catch ex As Exception
            ShowError("Error al iniciar sesión: " & ex.Message)
        End Try
    End Sub
    
    Private Sub ShowError(message As String)
        lblError.Text = message
        alertError.Visible = True
    End Sub
End Class


