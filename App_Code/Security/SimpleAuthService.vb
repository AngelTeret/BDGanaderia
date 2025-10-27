Imports System.Data
Imports System.Data.SqlClient
Imports System.Web

Public Class SimpleAuthService
    
    ' Clase para devolver múltiples valores de validación
    Public Class AuthResult
        Public Property IsValid As Boolean
        Public Property UserId As Integer
        Public Property RoleName As String
    End Class
    
    ''' <summary>
    ''' Valida un usuario por nombre y contraseña (comparación en texto plano)
    ''' </summary>
    Public Shared Function ValidateUser(username As String, password As String) As AuthResult
        Try
            Dim normalUsername As String = username.Trim().ToLower()
            
            ' Buscar usuario
            Dim query As String = "SELECT U.ID_Usuario, U.Contrasena, R.Nombre_Rol " & _
                                  "FROM Usuario U " & _
                                  "INNER JOIN Rol R ON U.ID_Rol = R.ID_Rol " & _
                                  "WHERE LOWER(U.Nombre_Usuario) = @Nombre_Usuario"
            
            Dim parameters() As SqlParameter = {
                New SqlParameter("@Nombre_Usuario", normalUsername)
            }
            
            Dim result As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            
            If result.Rows.Count = 0 Then
                Return New AuthResult With {.IsValid = False, .UserId = 0, .RoleName = ""}
            End If
            
            Dim row As DataRow = result.Rows(0)
            Dim storedPassword As String = row("Contrasena").ToString()
            Dim userId As Integer = Convert.ToInt32(row("ID_Usuario"))
            Dim roleName As String = row("Nombre_Rol").ToString()
            
            ' Comparación directa (sin hashing)
            If password = storedPassword Then
                Return New AuthResult With {.IsValid = True, .UserId = userId, .RoleName = roleName}
            Else
                Return New AuthResult With {.IsValid = False, .UserId = 0, .RoleName = ""}
            End If
            
        Catch ex As Exception
            Return New AuthResult With {.IsValid = False, .UserId = 0, .RoleName = ""}
        End Try
    End Function
    
    ''' <summary>
    ''' Inicia sesión del usuario
    ''' </summary>
    Public Shared Sub SignIn(userId As Integer, userName As String, roleName As String, isPersistent As Boolean)
        ' Crear ticket de autenticación
        ' Guardar el rol en UserData
        Dim ticket As New System.Web.Security.FormsAuthenticationTicket(
            1,
            userId.ToString(),
            DateTime.Now,
            DateTime.Now.AddMinutes(60),
            isPersistent,
            roleName
        )
        
        Dim encryptedTicket As String = System.Web.Security.FormsAuthentication.Encrypt(ticket)
        Dim cookie As New HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, encryptedTicket)
        
        If isPersistent Then
            cookie.Expires = DateTime.Now.AddDays(7)
        End If
        
        HttpContext.Current.Response.Cookies.Add(cookie)
    End Sub
    
    ''' <summary>
    ''' Cierra sesión del usuario
    ''' </summary>
    Public Shared Sub SignOut()
        System.Web.Security.FormsAuthentication.SignOut()
    End Sub
    
    ''' <summary>
    ''' Obtiene el rol actual del usuario autenticado
    ''' </summary>
    Public Shared Function GetCurrentRole() As String
        Try
            If HttpContext.Current.User.Identity.IsAuthenticated Then
                Dim ticket As System.Web.Security.FormsAuthenticationTicket = System.Web.Security.FormsAuthentication.Decrypt(
                    HttpContext.Current.Request.Cookies(System.Web.Security.FormsAuthentication.FormsCookieName).Value
                )
                Return ticket.UserData
            End If
            Return ""
        Catch
            Return ""
        End Try
    End Function
    
    ''' <summary>
    ''' Obtiene el ID del usuario actual
    ''' </summary>
    Public Shared Function GetCurrentUserId() As Integer
        Try
            If HttpContext.Current.User.Identity.IsAuthenticated Then
                Return Convert.ToInt32(HttpContext.Current.User.Identity.Name)
            End If
            Return 0
        Catch
            Return 0
        End Try
    End Function
    
    ''' <summary>
    ''' Verifica si el usuario actual tiene uno de los roles especificados
    ''' </summary>
    Public Shared Function IsInRole(ParamArray roles() As String) As Boolean
        If Not HttpContext.Current.User.Identity.IsAuthenticated Then
            Return False
        End If
        
        Dim currentRole As String = GetCurrentRole()
        Return roles.Any(Function(r) r.Equals(currentRole, StringComparison.OrdinalIgnoreCase))
    End Function
    
    ''' <summary>
    ''' Cambia la contraseña de un usuario
    ''' </summary>
    Public Shared Function ChangePassword(userId As Integer, currentPassword As String, newPassword As String) As Boolean
        Try
            ' Verificar contraseña actual
            Dim queryVerify As String = "SELECT Contrasena FROM Usuario WHERE ID_Usuario = @ID_Usuario"
            Dim parametersVerify() As SqlParameter = {
                New SqlParameter("@ID_Usuario", userId)
            }
            
            Dim resultVerify As Object = DataAccess.ExecuteScalar(queryVerify, parametersVerify)
            
            If resultVerify Is Nothing OrElse resultVerify.ToString() <> currentPassword Then
                Return False
            End If
            
            ' Actualizar contraseña
            Dim queryUpdate As String = "UPDATE Usuario SET Contrasena = @NuevaContrasena WHERE ID_Usuario = @ID_Usuario"
            Dim parametersUpdate() As SqlParameter = {
                New SqlParameter("@NuevaContrasena", newPassword),
                New SqlParameter("@ID_Usuario", userId)
            }
            
            Dim affectedRows As Integer = DataAccess.ExecuteNonQuery(queryUpdate, parametersUpdate)
            Return affectedRows > 0
            
        Catch ex As Exception
            Return False
        End Try
    End Function
    
End Class

