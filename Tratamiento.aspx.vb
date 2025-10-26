Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Tratamiento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTratamientos()
        End If
    End Sub

    Private Sub CargarTratamientos()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Tratamiento'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Tratamiento' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim tratamientosData As DataTable = DataAccess.ExecuteStoredProcedure("ListarTratamiento")
            gvTratamientos.DataSource = tratamientosData
            gvTratamientos.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los tratamientos: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim tratamientoId As Integer = Convert.ToInt32(hfTratamientoIdEliminar.Value)
            EliminarTratamiento(tratamientoId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el tratamiento: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim tratamientoId As Integer = 0
                If Not String.IsNullOrEmpty(hfTratamientoId.Value) AndAlso IsNumeric(hfTratamientoId.Value) Then
                    tratamientoId = Convert.ToInt32(hfTratamientoId.Value)
                End If
                
                Dim nombreTratamiento As String = txtNombreTratamiento.Text.Trim()

                Dim result As Integer

                If tratamientoId = 0 Then
                    ' Insertar nuevo tratamiento usando procedimiento almacenado
                    result = InsertarTratamiento(nombreTratamiento)
                    If result > 0 Then
                        MostrarAlerta("Tratamiento agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el tratamiento", "danger")
                    End If
                Else
                    ' Actualizar tratamiento existente usando procedimiento almacenado
                    result = ActualizarTratamiento(tratamientoId, nombreTratamiento)
                    If result > 0 Then
                        MostrarAlerta("Tratamiento actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el tratamiento", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarTratamientos()
                    tratamientoModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el tratamiento: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvTratamientos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim tratamientoId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarTratamiento"
                    EditarTratamiento(tratamientoId)
                Case "EliminarTratamiento"
                    EliminarTratamiento(tratamientoId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarTratamiento(tratamientoId As Integer)
        Try
            Dim query As String = "SELECT ID_Tratamiento, Nombre_Tratamiento FROM Tratamiento WHERE ID_Tratamiento = @TratamientoId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@TratamientoId", tratamientoId)
            }
            
            Dim tratamientoData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If tratamientoData.Rows.Count > 0 Then
                Dim row As DataRow = tratamientoData.Rows(0)
                
                hfTratamientoId.Value = tratamientoId.ToString()
                txtNombreTratamiento.Text = row("Nombre_Tratamiento").ToString()
                
                modalTitle.InnerText = "Editar Tratamiento"
                tratamientoModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del tratamiento: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarTratamiento(tratamientoId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarTratamiento @ID_Tratamiento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Tratamiento", tratamientoId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Tratamiento eliminado correctamente", "success")
                CargarTratamientos()
            Else
                MostrarAlerta("Error al eliminar el tratamiento", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el tratamiento: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarTratamiento(nombreTratamiento As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Tratamiento), 0) + 1 FROM Tratamiento"
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
            Dim query As String = "EXEC InsertarTratamiento @ID_Tratamiento, @Nombre_Tratamiento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Tratamiento", nuevoId),
                New SqlParameter("@Nombre_Tratamiento", nombreTratamiento)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el tratamiento: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarTratamiento(tratamientoId As Integer, nombreTratamiento As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarTratamiento @ID_Tratamiento, @Nombre_Tratamiento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Tratamiento", tratamientoId),
                New SqlParameter("@Nombre_Tratamiento", nombreTratamiento)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreTratamiento.Text.Trim()) Then
            MostrarAlerta("El nombre del tratamiento es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreTratamiento.Text = ""
        hfTratamientoId.Value = "0"
        modalTitle.InnerText = "Nuevo Tratamiento"
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

