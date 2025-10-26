Imports System.Data
Imports System.Data.SqlClient

Partial Public Class EstadoAnimal
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarEstadoAnimal()
        End If
    End Sub

    Private Sub CargarEstadoAnimal()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Estado_Animal'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Estado_Animal' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            Dim query As String = "SELECT ID_EstadoAnimal, Nombre_Estado FROM Estado_Animal ORDER BY ID_EstadoAnimal ASC"
            Dim estadoAnimalData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvEstadoAnimal.DataSource = estadoAnimalData
            gvEstadoAnimal.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los estados de animal: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim estadoAnimalId As Integer = Convert.ToInt32(hfEstadoAnimalIdEliminar.Value)
            EliminarEstadoAnimal(estadoAnimalId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el estado de animal: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim estadoAnimalId As Integer = 0
                If Not String.IsNullOrEmpty(hfEstadoAnimalId.Value) AndAlso IsNumeric(hfEstadoAnimalId.Value) Then
                    estadoAnimalId = Convert.ToInt32(hfEstadoAnimalId.Value)
                End If
                
                Dim nombreEstado As String = txtNombreEstado.Text.Trim()

                Dim result As Integer

                If estadoAnimalId = 0 Then
                    ' Insertar nuevo estado de animal usando procedimiento almacenado
                    result = InsertarEstadoAnimal(nombreEstado)
                    If result > 0 Then
                        MostrarAlerta("Estado de animal agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el estado de animal", "danger")
                    End If
                Else
                    ' Actualizar estado de animal existente usando procedimiento almacenado
                    result = ActualizarEstadoAnimal(estadoAnimalId, nombreEstado)
                    If result > 0 Then
                        MostrarAlerta("Estado de animal actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el estado de animal", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarEstadoAnimal()
                    estadoAnimalModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el estado de animal: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvEstadoAnimal_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim estadoAnimalId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarEstadoAnimal"
                    EditarEstadoAnimal(estadoAnimalId)
                Case "EliminarEstadoAnimal"
                    EliminarEstadoAnimal(estadoAnimalId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarEstadoAnimal(estadoAnimalId As Integer)
        Try
            Dim query As String = "SELECT ID_EstadoAnimal, Nombre_Estado FROM Estado_Animal WHERE ID_EstadoAnimal = @EstadoAnimalId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@EstadoAnimalId", estadoAnimalId)
            }
            
            Dim estadoAnimalData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If estadoAnimalData.Rows.Count > 0 Then
                Dim row As DataRow = estadoAnimalData.Rows(0)
                
                hfEstadoAnimalId.Value = estadoAnimalId.ToString()
                txtNombreEstado.Text = row("Nombre_Estado").ToString()
                
                modalTitle.InnerText = "Editar Estado de Animal"
                estadoAnimalModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del estado de animal: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarEstadoAnimal(estadoAnimalId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarEstadoAnimal @ID_EstadoAnimal"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_EstadoAnimal", estadoAnimalId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Estado de animal eliminado correctamente", "success")
                CargarEstadoAnimal()
            Else
                MostrarAlerta("Error al eliminar el estado de animal", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el estado de animal: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarEstadoAnimal(nombreEstado As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_EstadoAnimal), 0) + 1 FROM Estado_Animal"
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
            Dim query As String = "EXEC InsertarEstadoAnimal @ID_EstadoAnimal, @Nombre_Estado"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_EstadoAnimal", nuevoId),
                New SqlParameter("@Nombre_Estado", nombreEstado)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el estado de animal: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarEstadoAnimal(estadoAnimalId As Integer, nombreEstado As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarEstadoAnimal @ID_EstadoAnimal, @Nombre_Estado"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_EstadoAnimal", estadoAnimalId),
                New SqlParameter("@Nombre_Estado", nombreEstado)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreEstado.Text.Trim()) Then
            MostrarAlerta("El nombre del estado es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreEstado.Text = ""
        hfEstadoAnimalId.Value = "0"
        modalTitle.InnerText = "Nuevo Estado de Animal"
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
