Imports System.Data
Imports System.Data.SqlClient

Partial Public Class TipoAnimal
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTipoAnimal()
        End If
    End Sub

    Private Sub CargarTipoAnimal()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Tipo_Animal'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Tipo_Animal' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            Dim query As String = "SELECT ID_TipoAnimal, Nombre_Tipo FROM Tipo_Animal ORDER BY ID_TipoAnimal ASC"
            Dim tipoAnimalData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvTipoAnimal.DataSource = tipoAnimalData
            gvTipoAnimal.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los tipos de animal: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim tipoAnimalId As Integer = Convert.ToInt32(hfTipoAnimalIdEliminar.Value)
            EliminarTipoAnimal(tipoAnimalId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el tipo de animal: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim tipoAnimalId As Integer = 0
                If Not String.IsNullOrEmpty(hfTipoAnimalId.Value) AndAlso IsNumeric(hfTipoAnimalId.Value) Then
                    tipoAnimalId = Convert.ToInt32(hfTipoAnimalId.Value)
                End If
                
                Dim nombreTipo As String = txtNombreTipo.Text.Trim()

                Dim result As Integer

                If tipoAnimalId = 0 Then
                    ' Insertar nuevo tipo de animal usando procedimiento almacenado
                    result = InsertarTipoAnimal(nombreTipo)
                    If result > 0 Then
                        MostrarAlerta("Tipo de animal agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el tipo de animal", "danger")
                    End If
                Else
                    ' Actualizar tipo de animal existente usando procedimiento almacenado
                    result = ActualizarTipoAnimal(tipoAnimalId, nombreTipo)
                    If result > 0 Then
                        MostrarAlerta("Tipo de animal actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el tipo de animal", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarTipoAnimal()
                    tipoAnimalModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el tipo de animal: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvTipoAnimal_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim tipoAnimalId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarTipoAnimal"
                    EditarTipoAnimal(tipoAnimalId)
                Case "EliminarTipoAnimal"
                    EliminarTipoAnimal(tipoAnimalId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarTipoAnimal(tipoAnimalId As Integer)
        Try
            Dim query As String = "SELECT ID_TipoAnimal, Nombre_Tipo FROM Tipo_Animal WHERE ID_TipoAnimal = @TipoAnimalId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@TipoAnimalId", tipoAnimalId)
            }
            
            Dim tipoAnimalData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If tipoAnimalData.Rows.Count > 0 Then
                Dim row As DataRow = tipoAnimalData.Rows(0)
                
                hfTipoAnimalId.Value = tipoAnimalId.ToString()
                txtNombreTipo.Text = row("Nombre_Tipo").ToString()
                
                modalTitle.InnerText = "Editar Tipo de Animal"
                tipoAnimalModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del tipo de animal: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarTipoAnimal(tipoAnimalId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarTipoAnimal @ID_TipoAnimal"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_TipoAnimal", tipoAnimalId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Tipo de animal eliminado correctamente", "success")
                CargarTipoAnimal()
            Else
                MostrarAlerta("Error al eliminar el tipo de animal", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el tipo de animal: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarTipoAnimal(nombreTipo As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_TipoAnimal), 0) + 1 FROM Tipo_Animal"
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
            Dim query As String = "EXEC InsertarTipoAnimal @ID_TipoAnimal, @Nombre_Tipo"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_TipoAnimal", nuevoId),
                New SqlParameter("@Nombre_Tipo", nombreTipo)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el tipo de animal: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarTipoAnimal(tipoAnimalId As Integer, nombreTipo As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarTipoAnimal @ID_TipoAnimal, @Nombre_Tipo"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_TipoAnimal", tipoAnimalId),
                New SqlParameter("@Nombre_Tipo", nombreTipo)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreTipo.Text.Trim()) Then
            MostrarAlerta("El nombre del tipo de animal es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreTipo.Text = ""
        hfTipoAnimalId.Value = "0"
        modalTitle.InnerText = "Nuevo Tipo de Animal"
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
