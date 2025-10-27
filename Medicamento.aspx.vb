Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Medicamento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarMedicamentos()
        End If
    End Sub

    Private Sub CargarMedicamentos()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Medicamento'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Medicamento' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim medicamentosData As DataTable = DataAccess.ExecuteStoredProcedure("ListarMedicamento")
            gvMedicamentos.DataSource = medicamentosData
            gvMedicamentos.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los medicamentos: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim medicamentoId As Integer = Convert.ToInt32(hfMedicamentoIdEliminar.Value)
            EliminarMedicamento(medicamentoId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el medicamento: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim medicamentoId As Integer = 0
                If Not String.IsNullOrEmpty(hfMedicamentoId.Value) AndAlso IsNumeric(hfMedicamentoId.Value) Then
                    medicamentoId = Convert.ToInt32(hfMedicamentoId.Value)
                End If
                
                Dim nombreMedicamento As String = txtNombreMedicamento.Text.Trim()

                Dim result As Integer

                If medicamentoId = 0 Then
                    ' Insertar nuevo medicamento usando procedimiento almacenado
                    result = InsertarMedicamento(nombreMedicamento)
                    If result > 0 Then
                        MostrarAlerta("Medicamento agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el medicamento", "danger")
                    End If
                Else
                    ' Actualizar medicamento existente usando procedimiento almacenado
                    result = ActualizarMedicamento(medicamentoId, nombreMedicamento)
                    If result > 0 Then
                        MostrarAlerta("Medicamento actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el medicamento", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarMedicamentos()
                    medicamentoModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el medicamento: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvMedicamentos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim medicamentoId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarMedicamento"
                    EditarMedicamento(medicamentoId)
                Case "EliminarMedicamento"
                    EliminarMedicamento(medicamentoId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarMedicamento(medicamentoId As Integer)
        Try
            Dim query As String = "SELECT ID_Medicamento, Nombre_Medicamento FROM Medicamento WHERE ID_Medicamento = @MedicamentoId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@MedicamentoId", medicamentoId)
            }
            
            Dim medicamentoData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If medicamentoData.Rows.Count > 0 Then
                Dim row As DataRow = medicamentoData.Rows(0)
                
                hfMedicamentoId.Value = medicamentoId.ToString()
                txtNombreMedicamento.Text = row("Nombre_Medicamento").ToString()
                
                modalTitle.InnerText = "Editar Medicamento"
                medicamentoModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del medicamento: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarMedicamento(medicamentoId As Integer)
        Try
            ' Obtener nombre antes de eliminar
            Dim nombreMedicamento As String = ""
            Try
                Dim queryNombre As String = "SELECT Nombre_Medicamento FROM Medicamento WHERE ID_Medicamento = @ID_Medicamento"
                Dim nombreObj As Object = DataAccess.ExecuteScalar(queryNombre, New SqlParameter() {New SqlParameter("@ID_Medicamento", medicamentoId)})
                If nombreObj IsNot Nothing Then nombreMedicamento = nombreObj.ToString()
            Catch
            End Try
            
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarMedicamento @ID_Medicamento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Medicamento", medicamentoId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                ' Registrar en bitácora
                Try
                    BitacoraHelper.RegistrarEliminar("Medicamento", medicamentoId.ToString(), "Nombre: " & nombreMedicamento)
                Catch
                End Try
                
                MostrarAlerta("Medicamento eliminado correctamente", "success")
                CargarMedicamentos()
            Else
                MostrarAlerta("Error al eliminar el medicamento", "danger")
            End If
        Catch ex As Exception
            Try
                BitacoraHelper.RegistrarError(ex, "Medicamento.aspx")
            Catch
            End Try
            MostrarAlerta("Error al eliminar el medicamento: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarMedicamento(nombreMedicamento As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Medicamento), 0) + 1 FROM Medicamento"
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
            Dim query As String = "EXEC InsertarMedicamento @ID_Medicamento, @Nombre_Medicamento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Medicamento", nuevoId),
                New SqlParameter("@Nombre_Medicamento", nombreMedicamento)
            }
            
            Dim resultado As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            
            ' Registrar en bitácora
            If resultado > 0 Then
                Try
                    BitacoraHelper.RegistrarCrear("Medicamento", nuevoId.ToString(), "Nombre: " & nombreMedicamento)
                Catch
                End Try
            End If
            
            Return resultado
        Catch ex As Exception
            Try
                BitacoraHelper.RegistrarError(ex, "Medicamento.aspx")
            Catch
            End Try
            Throw New Exception("Error al insertar el medicamento: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarMedicamento(medicamentoId As Integer, nombreMedicamento As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarMedicamento @ID_Medicamento, @Nombre_Medicamento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Medicamento", medicamentoId),
                New SqlParameter("@Nombre_Medicamento", nombreMedicamento)
            }
            
            Dim resultado As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            
            ' Registrar en bitácora
            If resultado > 0 Then
                Try
                    BitacoraHelper.RegistrarActualizar("Medicamento", medicamentoId.ToString(), "Nombre: " & nombreMedicamento)
                Catch
                End Try
            End If
            
            Return resultado
        Catch ex As Exception
            Try
                BitacoraHelper.RegistrarError(ex, "Medicamento.aspx")
            Catch
            End Try
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreMedicamento.Text.Trim()) Then
            MostrarAlerta("El nombre del medicamento es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreMedicamento.Text = ""
        hfMedicamentoId.Value = "0"
        modalTitle.InnerText = "Nuevo Medicamento"
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

