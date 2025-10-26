Imports System.Data
Imports System.Data.SqlClient

Partial Public Class TratamientoMedicamento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTratamientos()
            CargarMedicamentos()
            CargarAsignaciones()
        End If
    End Sub

    Private Sub CargarTratamientos()
        Try
            Dim query As String = "SELECT ID_Tratamiento, Nombre_Tratamiento FROM Tratamiento ORDER BY Nombre_Tratamiento"
            Dim tratamientosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlTratamiento.DataSource = tratamientosData
            ddlTratamiento.DataTextField = "Nombre_Tratamiento"
            ddlTratamiento.DataValueField = "ID_Tratamiento"
            ddlTratamiento.DataBind()
            ddlTratamiento.Items.Insert(0, New ListItem("Seleccione un tratamiento", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los tratamientos: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarMedicamentos()
        Try
            Dim query As String = "SELECT ID_Medicamento, Nombre_Medicamento FROM Medicamento ORDER BY Nombre_Medicamento"
            Dim medicamentosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlMedicamento.DataSource = medicamentosData
            ddlMedicamento.DataTextField = "Nombre_Medicamento"
            ddlMedicamento.DataValueField = "ID_Medicamento"
            ddlMedicamento.DataBind()
            ddlMedicamento.Items.Insert(0, New ListItem("Seleccione un medicamento", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los medicamentos: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarAsignaciones()
        Try
            ' Verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Tratamiento_Medicamento'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Tratamiento_Medicamento' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Obtener asignaciones con JOINs para mostrar nombres
            Dim query As String = "SELECT TM.ID_Tratamiento, T.Nombre_Tratamiento, TM.ID_Medicamento, M.Nombre_Medicamento, TM.Dosis " & _
                                  "FROM Tratamiento_Medicamento TM " & _
                                  "INNER JOIN Tratamiento T ON TM.ID_Tratamiento = T.ID_Tratamiento " & _
                                  "INNER JOIN Medicamento M ON TM.ID_Medicamento = M.ID_Medicamento " & _
                                  "ORDER BY T.Nombre_Tratamiento, M.Nombre_Medicamento"
            
            Dim asignacionesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvAsignaciones.DataSource = asignacionesData
            gvAsignaciones.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar las asignaciones: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim tratamientoId As Integer = Convert.ToInt32(hfTratamientoIdEliminar.Value)
            Dim medicamentoId As Integer = Convert.ToInt32(hfMedicamentoIdEliminar.Value)
            EliminarAsignacion(tratamientoId, medicamentoId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la asignación: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim tratamientoId As Integer = Convert.ToInt32(ddlTratamiento.SelectedValue)
                Dim medicamentoId As Integer = Convert.ToInt32(ddlMedicamento.SelectedValue)
                Dim dosis As String = txtDosis.Text.Trim()

                ' Verificar si estamos editando o creando
                Dim editandoTratamientoId As Integer = 0
                Dim editandoMedicamentoId As Integer = 0
                
                If Not String.IsNullOrEmpty(hfTratamientoId.Value) AndAlso IsNumeric(hfTratamientoId.Value) Then
                    editandoTratamientoId = Convert.ToInt32(hfTratamientoId.Value)
                End If
                
                If Not String.IsNullOrEmpty(hfMedicamentoId.Value) AndAlso IsNumeric(hfMedicamentoId.Value) Then
                    editandoMedicamentoId = Convert.ToInt32(hfMedicamentoId.Value)
                End If

                Dim result As Integer

                If editandoTratamientoId = 0 AndAlso editandoMedicamentoId = 0 Then
                    ' Insertar nueva asignación
                    If ExisteAsignacion(tratamientoId, medicamentoId) Then
                        MostrarAlerta("Este medicamento ya está asignado a este tratamiento", "warning")
                        Return
                    End If
                    
                    result = InsertarAsignacion(tratamientoId, medicamentoId, dosis)
                    
                    If result > 0 Then
                        MostrarAlerta("Medicamento asignado correctamente", "success")
                    Else
                        MostrarAlerta("Error al asignar el medicamento", "danger")
                    End If
                Else
                    ' Actualizar asignación existente
                    result = ActualizarAsignacion(tratamientoId, medicamentoId, dosis)
                    
                    If result > 0 Then
                        MostrarAlerta("Dosis actualizada correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar la dosis", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarAsignaciones()
                    asignacionModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar la asignación: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvAsignaciones_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim args As String() = e.CommandArgument.ToString().Split(",")
            
            Select Case e.CommandName
                Case "EditarAsignacion"
                    If args.Length = 2 Then
                        Dim tratamientoId As Integer = Convert.ToInt32(args(0))
                        Dim medicamentoId As Integer = Convert.ToInt32(args(1))
                        EditarAsignacion(tratamientoId, medicamentoId)
                    End If
                Case "EliminarAsignacion"
                    If args.Length = 2 Then
                        Dim tratamientoId As Integer = Convert.ToInt32(args(0))
                        Dim medicamentoId As Integer = Convert.ToInt32(args(1))
                        EliminarAsignacion(tratamientoId, medicamentoId)
                    End If
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarAsignacion(tratamientoId As Integer, medicamentoId As Integer)
        Try
            Dim query As String = "SELECT ID_Tratamiento, ID_Medicamento, Dosis FROM Tratamiento_Medicamento WHERE ID_Tratamiento = @ID_Tratamiento AND ID_Medicamento = @ID_Medicamento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Tratamiento", tratamientoId),
                New SqlParameter("@ID_Medicamento", medicamentoId)
            }
            
            Dim asignacionData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If asignacionData.Rows.Count > 0 Then
                Dim row As DataRow = asignacionData.Rows(0)
                
                hfTratamientoId.Value = tratamientoId.ToString()
                hfMedicamentoId.Value = medicamentoId.ToString()
                
                ' Cargar tratamientos si no están cargados
                If ddlTratamiento.Items.Count <= 1 Then
                    CargarTratamientos()
                End If
                ddlTratamiento.SelectedValue = row("ID_Tratamiento").ToString()
                ddlTratamiento.Enabled = False ' No permitir cambiar el tratamiento al editar
                
                ' Cargar medicamentos si no están cargados
                If ddlMedicamento.Items.Count <= 1 Then
                    CargarMedicamentos()
                End If
                ddlMedicamento.SelectedValue = row("ID_Medicamento").ToString()
                ddlMedicamento.Enabled = False ' No permitir cambiar el medicamento al editar
                
                ' Mostrar dosis
                If Not row.IsNull("Dosis") Then
                    txtDosis.Text = row("Dosis").ToString()
                Else
                    txtDosis.Text = ""
                End If
                
                modalTitle.InnerText = "Editar Dosis"
                asignacionModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos de la asignación: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarAsignacion(tratamientoId As Integer, medicamentoId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarTratamientoMedicamento @ID_Tratamiento, @ID_Medicamento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Tratamiento", tratamientoId),
                New SqlParameter("@ID_Medicamento", medicamentoId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Asignación eliminada correctamente", "success")
                CargarAsignaciones()
            Else
                MostrarAlerta("Error al eliminar la asignación", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la asignación: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarAsignacion(tratamientoId As Integer, medicamentoId As Integer, dosis As String) As Integer
        Try
            ' Usar procedimiento almacenado para insertar
            Dim query As String = "EXEC InsertarTratamientoMedicamento @ID_Tratamiento, @ID_Medicamento, @Dosis"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Tratamiento", tratamientoId),
                New SqlParameter("@ID_Medicamento", medicamentoId),
                New SqlParameter("@Dosis", If(String.IsNullOrWhiteSpace(dosis), DBNull.Value, dosis))
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar la asignación: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarAsignacion(tratamientoId As Integer, medicamentoId As Integer, dosis As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarTratamientoMedicamento @ID_Tratamiento, @ID_Medicamento, @Dosis"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Tratamiento", tratamientoId),
                New SqlParameter("@ID_Medicamento", medicamentoId),
                New SqlParameter("@Dosis", If(String.IsNullOrWhiteSpace(dosis), DBNull.Value, dosis))
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al actualizar la asignación: " & ex.Message)
        End Try
    End Function

    Private Function ExisteAsignacion(tratamientoId As Integer, medicamentoId As Integer) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM Tratamiento_Medicamento WHERE ID_Tratamiento = @ID_Tratamiento AND ID_Medicamento = @ID_Medicamento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Tratamiento", tratamientoId),
                New SqlParameter("@ID_Medicamento", medicamentoId)
            }
            
            Dim result As Object = DataAccess.ExecuteScalar(query, parameters)
            Return Convert.ToInt32(result) > 0
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If ddlTratamiento.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un tratamiento", "warning")
            Return False
        End If

        If ddlMedicamento.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un medicamento", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlTratamiento.Items.Count > 0 Then
            ddlTratamiento.SelectedIndex = 0
            ddlTratamiento.Enabled = True
        End If
        If ddlMedicamento.Items.Count > 0 Then
            ddlMedicamento.SelectedIndex = 0
            ddlMedicamento.Enabled = True
        End If
        txtDosis.Text = ""
        hfTratamientoId.Value = ""
        hfMedicamentoId.Value = ""
        modalTitle.InnerText = "Nueva Asignación de Medicamento"
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

