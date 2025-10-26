Imports System.Data
Imports System.Data.SqlClient

Partial Public Class ControlLechero
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarControles()
        End If
    End Sub

    Private Sub CargarControles()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Control_Lechero'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Control_Lechero' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim controlesData As DataTable = DataAccess.ExecuteStoredProcedure("ListarControlLechero")
            gvControles.DataSource = controlesData
            gvControles.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los controles lecheros: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim controlId As Integer = Convert.ToInt32(hfControlIdEliminar.Value)
            EliminarControl(controlId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el control lechero: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim controlId As Integer = 0
                If Not String.IsNullOrEmpty(hfControlId.Value) AndAlso IsNumeric(hfControlId.Value) Then
                    controlId = Convert.ToInt32(hfControlId.Value)
                End If
                
                Dim fechaControl As String = txtFechaControl.Text.Trim()

                Dim result As Integer

                If controlId = 0 Then
                    ' Insertar nuevo control usando procedimiento almacenado
                    result = InsertarControl(fechaControl)
                    If result > 0 Then
                        MostrarAlerta("Control lechero agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el control lechero", "danger")
                    End If
                Else
                    ' Actualizar control existente usando procedimiento almacenado
                    result = ActualizarControl(controlId, fechaControl)
                    If result > 0 Then
                        MostrarAlerta("Control lechero actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el control lechero", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarControles()
                    controlModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el control lechero: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvControles_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim controlId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarControl"
                    EditarControl(controlId)
                Case "EliminarControl"
                    EliminarControl(controlId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarControl(controlId As Integer)
        Try
            Dim query As String = "SELECT ID_Control, Fecha_Control FROM Control_Lechero WHERE ID_Control = @ControlId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ControlId", controlId)
            }
            
            Dim controlData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If controlData.Rows.Count > 0 Then
                Dim row As DataRow = controlData.Rows(0)
                
                hfControlId.Value = controlId.ToString()
                txtFechaControl.Text = Convert.ToDateTime(row("Fecha_Control")).ToString("yyyy-MM-dd")
                
                modalTitle.InnerText = "Editar Control Lechero"
                controlModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del control lechero: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarControl(controlId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarControlLechero @ID_Control"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Control", controlId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Control lechero eliminado correctamente", "success")
                CargarControles()
            Else
                MostrarAlerta("Error al eliminar el control lechero", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el control lechero: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarControl(fechaControl As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Control), 0) + 1 FROM Control_Lechero"
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
            Dim query As String = "EXEC InsertarControlLechero @ID_Control, @Fecha_Control"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Control", nuevoId),
                New SqlParameter("@Fecha_Control", fechaControl)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el control lechero: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarControl(controlId As Integer, fechaControl As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarControlLechero @ID_Control, @Fecha_Control"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Control", controlId),
                New SqlParameter("@Fecha_Control", fechaControl)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtFechaControl.Text.Trim()) Then
            MostrarAlerta("La fecha del control es requerida", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtFechaControl.Text = ""
        hfControlId.Value = "0"
        modalTitle.InnerText = "Nuevo Control Lechero"
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

