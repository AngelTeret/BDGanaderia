Partial Public Class ChangePassword
    Inherits SimpleSecurePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            alertError.Visible = False
            alertSuccess.Visible = False
        End If
    End Sub

    Protected Sub btnChangePassword_Click(sender As Object, e As EventArgs)
        Try
            alertError.Visible = False
            alertSuccess.Visible = False
            
            Dim currentPassword As String = txtCurrentPassword.Text
            Dim newPassword As String = txtNewPassword.Text
            Dim confirmPassword As String = txtConfirmPassword.Text
            
            ' Validar campos
            If String.IsNullOrWhiteSpace(currentPassword) Then
                ShowError("La contraseña actual es requerida")
                Return
            End If
            
            If String.IsNullOrWhiteSpace(newPassword) Then
                ShowError("La nueva contraseña es requerida")
                Return
            End If
            
            If newPassword <> confirmPassword Then
                ShowError("Las contraseñas nuevas no coinciden")
                Return
            End If
            
            If newPassword.Length < 3 Then
                ShowError("La nueva contraseña debe tener al menos 3 caracteres")
                Return
            End If
            
            ' Cambiar contraseña
            Dim success As Boolean = SimpleAuthService.ChangePassword(CurrentUserId, currentPassword, newPassword)
            
            If success Then
                ShowSuccess("Contraseña actualizada correctamente")
                txtCurrentPassword.Text = ""
                txtNewPassword.Text = ""
                txtConfirmPassword.Text = ""
            Else
                ShowError("La contraseña actual es incorrecta")
            End If
            
        Catch ex As Exception
            ShowError("Error al cambiar la contraseña: " & ex.Message)
        End Try
    End Sub
    
    Private Sub ShowError(message As String)
        lblError.Text = message
        alertError.Visible = True
    End Sub
    
    Private Sub ShowSuccess(message As String)
        lblSuccess.Text = message
        alertSuccess.Visible = True
    End Sub
End Class


