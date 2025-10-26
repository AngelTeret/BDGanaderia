Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Veterinario
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarVeterinarios()
        End If
    End Sub

    Private Sub CargarVeterinarios()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Veterinario'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Veterinario' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim veterinariosData As DataTable = DataAccess.ExecuteStoredProcedure("ListarVeterinario")
            gvVeterinarios.DataSource = veterinariosData
            gvVeterinarios.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los veterinarios: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim veterinarioId As Integer = Convert.ToInt32(hfVeterinarioIdEliminar.Value)
            EliminarVeterinario(veterinarioId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el veterinario: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim veterinarioId As Integer = 0
                If Not String.IsNullOrEmpty(hfVeterinarioId.Value) AndAlso IsNumeric(hfVeterinarioId.Value) Then
                    veterinarioId = Convert.ToInt32(hfVeterinarioId.Value)
                End If
                
                Dim nombreVeterinario As String = txtNombreVeterinario.Text.Trim()

                Dim result As Integer

                If veterinarioId = 0 Then
                    ' Insertar nuevo veterinario usando procedimiento almacenado
                    result = InsertarVeterinario(nombreVeterinario)
                    If result > 0 Then
                        MostrarAlerta("Veterinario agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el veterinario", "danger")
                    End If
                Else
                    ' Actualizar veterinario existente usando procedimiento almacenado
                    result = ActualizarVeterinario(veterinarioId, nombreVeterinario)
                    If result > 0 Then
                        MostrarAlerta("Veterinario actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el veterinario", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarVeterinarios()
                    veterinarioModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el veterinario: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvVeterinarios_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim veterinarioId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarVeterinario"
                    EditarVeterinario(veterinarioId)
                Case "EliminarVeterinario"
                    EliminarVeterinario(veterinarioId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarVeterinario(veterinarioId As Integer)
        Try
            Dim query As String = "SELECT ID_Veterinario, Nombre_Veterinario FROM Veterinario WHERE ID_Veterinario = @VeterinarioId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@VeterinarioId", veterinarioId)
            }
            
            Dim veterinarioData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If veterinarioData.Rows.Count > 0 Then
                Dim row As DataRow = veterinarioData.Rows(0)
                
                hfVeterinarioId.Value = veterinarioId.ToString()
                txtNombreVeterinario.Text = row("Nombre_Veterinario").ToString()
                
                modalTitle.InnerText = "Editar Veterinario"
                veterinarioModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del veterinario: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarVeterinario(veterinarioId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarVeterinario @ID_Veterinario"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Veterinario", veterinarioId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Veterinario eliminado correctamente", "success")
                CargarVeterinarios()
            Else
                MostrarAlerta("Error al eliminar el veterinario", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el veterinario: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarVeterinario(nombreVeterinario As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Veterinario), 0) + 1 FROM Veterinario"
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
            Dim query As String = "EXEC InsertarVeterinario @ID_Veterinario, @Nombre_Veterinario"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Veterinario", nuevoId),
                New SqlParameter("@Nombre_Veterinario", nombreVeterinario)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el veterinario: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarVeterinario(veterinarioId As Integer, nombreVeterinario As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarVeterinario @ID_Veterinario, @Nombre_Veterinario"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Veterinario", veterinarioId),
                New SqlParameter("@Nombre_Veterinario", nombreVeterinario)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreVeterinario.Text.Trim()) Then
            MostrarAlerta("El nombre del veterinario es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreVeterinario.Text = ""
        hfVeterinarioId.Value = "0"
        modalTitle.InnerText = "Nuevo Veterinario"
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

