Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Rol
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarRoles()
        End If
    End Sub

    Private Sub CargarRoles()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Rol'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Rol' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim rolesData As DataTable = DataAccess.ExecuteStoredProcedure("ListarRol")
            gvRoles.DataSource = rolesData
            gvRoles.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los roles: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim rolId As Integer = Convert.ToInt32(hfRolIdEliminar.Value)
            EliminarRol(rolId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el rol: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim rolId As Integer = 0
                If Not String.IsNullOrEmpty(hfRolId.Value) AndAlso IsNumeric(hfRolId.Value) Then
                    rolId = Convert.ToInt32(hfRolId.Value)
                End If
                
                Dim nombreRol As String = txtNombreRol.Text.Trim()

                Dim result As Integer

                If rolId = 0 Then
                    ' Insertar nuevo rol usando procedimiento almacenado
                    result = InsertarRol(nombreRol)
                    If result > 0 Then
                        MostrarAlerta("Rol agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el rol", "danger")
                    End If
                Else
                    ' Actualizar rol existente usando procedimiento almacenado
                    result = ActualizarRol(rolId, nombreRol)
                    If result > 0 Then
                        MostrarAlerta("Rol actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el rol", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarRoles()
                    rolModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el rol: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvRoles_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim rolId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarRol"
                    EditarRol(rolId)
                Case "EliminarRol"
                    EliminarRol(rolId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarRol(rolId As Integer)
        Try
            Dim query As String = "SELECT ID_Rol, Nombre_Rol FROM Rol WHERE ID_Rol = @RolId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@RolId", rolId)
            }
            
            Dim rolData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If rolData.Rows.Count > 0 Then
                Dim row As DataRow = rolData.Rows(0)
                
                hfRolId.Value = rolId.ToString()
                txtNombreRol.Text = row("Nombre_Rol").ToString()
                
                modalTitle.InnerText = "Editar Rol"
                rolModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del rol: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarRol(rolId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarRol @ID_Rol"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Rol", rolId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Rol eliminado correctamente", "success")
                CargarRoles()
            Else
                MostrarAlerta("Error al eliminar el rol", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el rol: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarRol(nombreRol As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Rol), 0) + 1 FROM Rol"
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
            Dim query As String = "EXEC InsertarRol @ID_Rol, @Nombre_Rol"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Rol", nuevoId),
                New SqlParameter("@Nombre_Rol", nombreRol)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el rol: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarRol(rolId As Integer, nombreRol As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarRol @ID_Rol, @Nombre_Rol"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Rol", rolId),
                New SqlParameter("@Nombre_Rol", nombreRol)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreRol.Text.Trim()) Then
            MostrarAlerta("El nombre del rol es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreRol.Text = ""
        hfRolId.Value = "0"
        modalTitle.InnerText = "Nuevo Rol"
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

