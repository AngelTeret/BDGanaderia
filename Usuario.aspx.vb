Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Usuario
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarRoles()
            CargarUsuarios()
        End If
    End Sub

    Private Sub CargarRoles()
        Try
            Dim query As String = "SELECT ID_Rol, Nombre_Rol FROM Rol ORDER BY Nombre_Rol"
            Dim rolesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlRol.DataSource = rolesData
            ddlRol.DataTextField = "Nombre_Rol"
            ddlRol.DataValueField = "ID_Rol"
            ddlRol.DataBind()
            ddlRol.Items.Insert(0, New ListItem("Seleccione un rol", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los roles: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarUsuarios()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Usuario'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Usuario' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar JOIN para obtener el nombre del rol
            Dim query As String = "SELECT U.ID_Usuario, U.Nombre_Usuario, U.Correo, U.ID_Rol, R.Nombre_Rol " & _
                                  "FROM Usuario U " & _
                                  "INNER JOIN Rol R ON U.ID_Rol = R.ID_Rol " & _
                                  "ORDER BY U.Nombre_Usuario"
            
            Dim usuariosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvUsuarios.DataSource = usuariosData
            gvUsuarios.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los usuarios: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim usuarioId As Integer = Convert.ToInt32(hfUsuarioIdEliminar.Value)
            EliminarUsuario(usuarioId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el usuario: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim usuarioId As Integer = 0
                If Not String.IsNullOrEmpty(hfUsuarioId.Value) AndAlso IsNumeric(hfUsuarioId.Value) Then
                    usuarioId = Convert.ToInt32(hfUsuarioId.Value)
                End If
                
                Dim nombreUsuario As String = txtNombreUsuario.Text.Trim()
                Dim txtConfirmarContrasena As TextBox = DirectCast(usuarioModal.FindControl("txtConfirmarContrasena"), TextBox)
                Dim contrasena As String = txtContrasena.Text
                Dim confirmarContrasena As String = If(txtConfirmarContrasena IsNot Nothing, txtConfirmarContrasena.Text, "")
                Dim correo As String = txtCorreo.Text.Trim()
                Dim rolId As Integer = Convert.ToInt32(ddlRol.SelectedValue)

                ' Validar que las contraseñas coincidan
                If usuarioId = 0 Then
                    ' Nuevo usuario: contraseña es obligatoria
                    If contrasena <> confirmarContrasena Then
                        MostrarAlerta("Las contraseñas no coinciden", "warning")
                        Return
                    End If
                Else
                    ' Editar usuario: si se ingresó nueva contraseña, deben coincidir
                    If Not String.IsNullOrEmpty(contrasena) OrElse Not String.IsNullOrEmpty(confirmarContrasena) Then
                        If contrasena <> confirmarContrasena Then
                            MostrarAlerta("Las contraseñas no coinciden", "warning")
                            Return
                        End If
                    End If
                End If

                ' Validar que el nombre de usuario no exista (excepto en edición del mismo usuario)
                If Not ValidarNombreUsuarioUnico(nombreUsuario, usuarioId) Then
                    MostrarAlerta("El nombre de usuario ya existe. Por favor, elija otro.", "warning")
                    Return
                End If

                Dim result As Integer

                If usuarioId = 0 Then
                    ' Insertar nuevo usuario usando procedimiento almacenado
                    result = InsertarUsuario(nombreUsuario, contrasena, correo, rolId)
                    If result > 0 Then
                        MostrarAlerta("Usuario agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el usuario", "danger")
                    End If
                Else
                    ' Actualizar usuario existente
                    Dim contrasenaActual As String = If(String.IsNullOrEmpty(contrasena), "", contrasena)
                    result = ActualizarUsuario(usuarioId, nombreUsuario, contrasenaActual, correo, rolId)
                    If result > 0 Then
                        MostrarAlerta("Usuario actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el usuario", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarUsuarios()
                    usuarioModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el usuario: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvUsuarios_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            ' Solo procesar edición desde aquí
            If e.CommandName = "EditarUsuario" Then
                Dim usuarioId As Integer = Convert.ToInt32(e.CommandArgument)
                EditarUsuario(usuarioId)
            End If
            ' EliminarUsuario se maneja desde btnEliminarOculto_Click
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarUsuario(usuarioId As Integer)
        Try
            Dim query As String = "SELECT ID_Usuario, Nombre_Usuario, Contrasena, Correo, ID_Rol FROM Usuario WHERE ID_Usuario = @UsuarioId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@UsuarioId", usuarioId)
            }
            
            Dim usuarioData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If usuarioData.Rows.Count > 0 Then
                Dim row As DataRow = usuarioData.Rows(0)
                
                hfUsuarioId.Value = usuarioId.ToString()
                txtNombreUsuario.Text = row("Nombre_Usuario").ToString()
                ' No mostrar la contraseña por seguridad - dejar vacío
                txtContrasena.Text = ""
                
                ' Manejar valores NULL para Correo
                If Not row.IsNull("Correo") Then
                    txtCorreo.Text = row("Correo").ToString()
                Else
                    txtCorreo.Text = ""
                End If
                
                ' Cargar roles si no están cargados
                If ddlRol.Items.Count <= 1 Then
                    CargarRoles()
                End If
                ddlRol.SelectedValue = row("ID_Rol").ToString()
                
                modalTitle.InnerText = "Editar Usuario"
                usuarioModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del usuario: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarUsuario(usuarioId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarUsuario @ID_Usuario"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Usuario", usuarioId)
            }
            
            ' Obtener datos del usuario antes de eliminarlo
            Dim queryUsuario As String = "SELECT Nombre_Usuario FROM Usuario WHERE ID_Usuario = @ID_Usuario"
            Dim paramsUsuario() As SqlParameter = {New SqlParameter("@ID_Usuario", usuarioId)}
            Dim nombreUsuario As String = ""
            Try
                Dim nombre As Object = DataAccess.ExecuteScalar(queryUsuario, paramsUsuario)
                If nombre IsNot Nothing Then nombreUsuario = nombre.ToString()
            Catch
            End Try
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                ' Registrar en bitácora
                Try
                    BitacoraEvento.RegistrarEventoConEntidad("DELETE", "Usuario", usuarioId.ToString(), "Nombre: " & nombreUsuario, Nothing, "Se eliminó usuario: " & nombreUsuario)
                Catch
                    ' Ignorar errores de bitácora
                End Try
                
                MostrarAlerta("Usuario eliminado correctamente", "success")
                CargarUsuarios()
            Else
                MostrarAlerta("Error al eliminar el usuario", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el usuario: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarUsuario(nombreUsuario As String, contrasena As String, correo As String, rolId As Integer) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Usuario), 0) + 1 FROM Usuario"
            Dim result As Object = DataAccess.ExecuteScalar(queryId)
            Dim nuevoId As Integer = 1
            
            If result IsNot Nothing Then
                Try
                    nuevoId = Convert.ToInt32(result)
                Catch
                    nuevoId = 1
                End Try
            End If
            
            ' Usar procedimiento almacenado para insertar
            Dim query As String = "EXEC InsertarUsuario @ID_Usuario, @Nombre_Usuario, @Contrasena, @Correo, @ID_Rol"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Usuario", nuevoId),
                New SqlParameter("@Nombre_Usuario", nombreUsuario),
                New SqlParameter("@Contrasena", contrasena),
                New SqlParameter("@Correo", If(String.IsNullOrEmpty(correo), CObj(DBNull.Value), correo)),
                New SqlParameter("@ID_Rol", rolId)
            }
            
            Dim resultado As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            
            ' Registrar en bitácora
            If resultado > 0 Then
                Try
                    BitacoraEvento.RegistrarEventoConEntidad("CREATE", "Usuario", nuevoId.ToString(), Nothing, "Nombre: " & nombreUsuario & ", Email: " & correo, "Se creó nuevo usuario: " & nombreUsuario)
                Catch
                    ' Ignorar errores de bitácora
                End Try
            End If
            
            Return resultado
        Catch ex As Exception
            Throw New Exception("Error al insertar el usuario: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarUsuario(usuarioId As Integer, nombreUsuario As String, contrasena As String, correo As String, rolId As Integer) As Integer
        Try
            ' Si la contraseña está vacía, no actualizarla - usar la actual
            If String.IsNullOrEmpty(contrasena) Then
                ' Obtener la contraseña actual de la BD
                Dim queryActual As String = "SELECT Contrasena FROM Usuario WHERE ID_Usuario = @ID_Usuario"
                Dim paramsActual() As SqlParameter = {
                    New SqlParameter("@ID_Usuario", usuarioId)
                }
                Dim contrasenaActual As Object = DataAccess.ExecuteScalar(queryActual, paramsActual)
                
                If contrasenaActual IsNot Nothing Then
                    contrasena = contrasenaActual.ToString()
                Else
                    contrasena = ""
                End If
            End If
            
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarUsuario @ID_Usuario, @Nombre_Usuario, @Contrasena, @Correo, @ID_Rol"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Usuario", usuarioId),
                New SqlParameter("@Nombre_Usuario", nombreUsuario),
                New SqlParameter("@Contrasena", contrasena),
                New SqlParameter("@Correo", If(String.IsNullOrEmpty(correo), CObj(DBNull.Value), correo)),
                New SqlParameter("@ID_Rol", rolId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            
            ' Registrar en bitácora
            If result > 0 Then
                Try
                    BitacoraEvento.RegistrarEventoConEntidad("UPDATE", "Usuario", usuarioId.ToString(), "Datos anteriores", "Nombre: " & nombreUsuario & ", Email: " & correo, "Se actualizó usuario ID: " & usuarioId.ToString())
                Catch
                    ' Ignorar errores de bitácora
                End Try
            End If
            
            Return result
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarNombreUsuarioUnico(nombreUsuario As String, usuarioId As Integer) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM Usuario WHERE Nombre_Usuario = @NombreUsuario AND ID_Usuario <> @UsuarioId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@NombreUsuario", nombreUsuario),
                New SqlParameter("@UsuarioId", usuarioId)
            }
            
            Dim count As Object = DataAccess.ExecuteScalar(query, parameters)
            Return Convert.ToInt32(count) = 0
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreUsuario.Text.Trim()) Then
            MostrarAlerta("El nombre de usuario es requerido", "warning")
            Return False
        End If

        ' Validar formato de email si se proporciona
        If Not String.IsNullOrEmpty(txtCorreo.Text) Then
            If Not System.Text.RegularExpressions.Regex.IsMatch(txtCorreo.Text, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$") Then
                MostrarAlerta("El formato del correo electrónico no es válido", "warning")
                Return False
            End If
        End If

        ' Validar longitud de contraseña
        If String.IsNullOrEmpty(txtContrasena.Text) Then
            ' Verificar si es edición
            If hfUsuarioId.Value <> "0" Then
                ' En edición, la contraseña es opcional
            Else
                ' En nuevo usuario, la contraseña es obligatoria
                MostrarAlerta("La contraseña es requerida", "warning")
                Return False
            End If
        ElseIf txtContrasena.Text.Length < 6 Then
            MostrarAlerta("La contraseña debe tener al menos 6 caracteres", "warning")
            Return False
        End If

        If ddlRol.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un rol", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreUsuario.Text = ""
        txtContrasena.Text = ""
        Dim txtConfirmar As TextBox = DirectCast(usuarioModal.FindControl("txtConfirmarContrasena"), TextBox)
        If txtConfirmar IsNot Nothing Then
            txtConfirmar.Text = ""
        End If
        txtCorreo.Text = ""
        If ddlRol.Items.Count > 0 Then
            ddlRol.SelectedIndex = 0
        End If
        hfUsuarioId.Value = "0"
        modalTitle.InnerText = "Nuevo Usuario"
    End Sub

    Private Sub MostrarAlerta(mensaje As String, tipo As String)
        Dim script As String = ""
        Select Case tipo
            Case "success"
                script = "Swal.fire({icon: 'success', title: '¡Éxito!', text: '" & mensaje & "', confirmButtonText: 'Aceptar', confirmButtonColor: '#27ae60', timer: 3000, timerProgressBar: true});"
            Case "danger", "error"
                script = "Swal.fire({icon: 'error', title: 'Error', text: '" & mensaje & "', confirmButtonText: 'Aceptar', confirmButtonColor: '#e74c3c'});"
            Case "warning"
                script = "Swal.fire({icon: 'warning', title: 'Advertencia', text: '" & mensaje & "', confirmButtonText: 'Aceptar', confirmButtonColor: '#f39c12'});"
            Case "info"
                script = "Swal.fire({icon: 'info', title: 'Información', text: '" & mensaje & "', confirmButtonText: 'Aceptar', confirmButtonColor: '#3498db'});"
            Case Else
                script = "Swal.fire({icon: 'info', title: 'Información', text: '" & mensaje & "', confirmButtonText: 'Aceptar', confirmButtonColor: '#3498db'});"
        End Select
        
        ClientScript.RegisterStartupScript(Me.GetType(), "ShowAlert", script, True)
    End Sub
End Class

