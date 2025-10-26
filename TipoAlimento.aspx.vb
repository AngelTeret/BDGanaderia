Imports System.Data
Imports System.Data.SqlClient

Partial Public Class TipoAlimento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTipoAlimento()
        End If
    End Sub

    Private Sub CargarTipoAlimento()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Tipo_Alimento'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Tipo_Alimento' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim tipoAlimentoData As DataTable = DataAccess.ExecuteStoredProcedure("ListarTipoAlimento")
            gvTipoAlimento.DataSource = tipoAlimentoData
            gvTipoAlimento.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los tipos de alimento: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim tipoAlimentoId As Integer = Convert.ToInt32(hfTipoAlimentoIdEliminar.Value)
            EliminarTipoAlimento(tipoAlimentoId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el tipo de alimento: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim tipoAlimentoId As Integer = 0
                If Not String.IsNullOrEmpty(hfTipoAlimentoId.Value) AndAlso IsNumeric(hfTipoAlimentoId.Value) Then
                    tipoAlimentoId = Convert.ToInt32(hfTipoAlimentoId.Value)
                End If
                
                Dim nombreTipo As String = txtNombreTipo.Text.Trim()

                Dim result As Integer

                If tipoAlimentoId = 0 Then
                    ' Insertar nuevo tipo de alimento usando procedimiento almacenado
                    result = InsertarTipoAlimento(nombreTipo)
                    If result > 0 Then
                        MostrarAlerta("Tipo de alimento agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el tipo de alimento", "danger")
                    End If
                Else
                    ' Actualizar tipo de alimento existente usando procedimiento almacenado
                    result = ActualizarTipoAlimento(tipoAlimentoId, nombreTipo)
                    If result > 0 Then
                        MostrarAlerta("Tipo de alimento actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el tipo de alimento", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarTipoAlimento()
                    tipoAlimentoModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el tipo de alimento: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvTipoAlimento_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim tipoAlimentoId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarTipoAlimento"
                    EditarTipoAlimento(tipoAlimentoId)
                Case "EliminarTipoAlimento"
                    EliminarTipoAlimento(tipoAlimentoId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarTipoAlimento(tipoAlimentoId As Integer)
        Try
            Dim query As String = "SELECT ID_TipoAlimento, Nombre_Tipo FROM Tipo_Alimento WHERE ID_TipoAlimento = @TipoAlimentoId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@TipoAlimentoId", tipoAlimentoId)
            }
            
            Dim tipoAlimentoData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If tipoAlimentoData.Rows.Count > 0 Then
                Dim row As DataRow = tipoAlimentoData.Rows(0)
                
                hfTipoAlimentoId.Value = tipoAlimentoId.ToString()
                txtNombreTipo.Text = row("Nombre_Tipo").ToString()
                
                modalTitle.InnerText = "Editar Tipo de Alimento"
                tipoAlimentoModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del tipo de alimento: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarTipoAlimento(tipoAlimentoId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarTipoAlimento @ID_TipoAlimento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_TipoAlimento", tipoAlimentoId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Tipo de alimento eliminado correctamente", "success")
                CargarTipoAlimento()
            Else
                MostrarAlerta("Error al eliminar el tipo de alimento", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el tipo de alimento: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarTipoAlimento(nombreTipo As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_TipoAlimento), 0) + 1 FROM Tipo_Alimento"
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
            Dim query As String = "EXEC InsertarTipoAlimento @ID_TipoAlimento, @Nombre_Tipo"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_TipoAlimento", nuevoId),
                New SqlParameter("@Nombre_Tipo", nombreTipo)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el tipo de alimento: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarTipoAlimento(tipoAlimentoId As Integer, nombreTipo As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarTipoAlimento @ID_TipoAlimento, @Nombre_Tipo"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_TipoAlimento", tipoAlimentoId),
                New SqlParameter("@Nombre_Tipo", nombreTipo)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreTipo.Text.Trim()) Then
            MostrarAlerta("El nombre del tipo de alimento es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreTipo.Text = ""
        hfTipoAlimentoId.Value = "0"
        modalTitle.InnerText = "Nuevo Tipo de Alimento"
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

