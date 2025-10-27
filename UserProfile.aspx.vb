Imports System.Data
Imports System.Data.SqlClient

Partial Public Class UserProfile
    Inherits SimpleSecurePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadUserInfo()
        End If
    End Sub

    Private Sub LoadUserInfo()
        Try
            Dim query As String = "SELECT Nombre_Usuario, Correo, ID_Rol FROM Usuario WHERE ID_Usuario = @ID_Usuario"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Usuario", CurrentUserId)
            }
            
            Dim data As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            
            If data.Rows.Count > 0 Then
                Dim row As DataRow = data.Rows(0)
                
                lblUsername.Text = row("Nombre_Usuario").ToString()
                lblRole.Text = CurrentRoleName
                
                If Not row.IsNull("Correo") Then
                    txtEmail.Text = row("Correo").ToString()
                End If
            End If
            
        Catch ex As Exception
            ShowError("Error al cargar la información del usuario")
        End Try
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        Try
            alertError.Visible = False
            alertSuccess.Visible = False
            
            Dim email As String = txtEmail.Text.Trim()
            
            ' Actualizar correo
            Dim query As String = "UPDATE Usuario SET Correo = @Correo WHERE ID_Usuario = @ID_Usuario"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@Correo", If(String.IsNullOrEmpty(email), DBNull.Value, email)),
                New SqlParameter("@ID_Usuario", CurrentUserId)
            }
            
            Dim affected As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            
            If affected > 0 Then
                ShowSuccess("Información actualizada correctamente")
            Else
                ShowError("No se pudieron guardar los cambios")
            End If
            
        Catch ex As Exception
            ShowError("Error al actualizar la información: " & ex.Message)
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

