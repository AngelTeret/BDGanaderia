Imports System.Data
Imports System.Data.SqlClient

Partial Public Class ControlNutricional
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarAnimales()
            CargarControles()
        End If
    End Sub

    Private Sub CargarAnimales()
        Try
            Dim query As String = "SELECT ID_Animal, Nombre_Animal FROM Animal ORDER BY Nombre_Animal"
            Dim animalesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlAnimal.DataSource = animalesData
            ddlAnimal.DataTextField = "Nombre_Animal"
            ddlAnimal.DataValueField = "ID_Animal"
            ddlAnimal.DataBind()
            ddlAnimal.Items.Insert(0, New ListItem("Seleccione un animal", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los animales: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarControles()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Control_Nutricional'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Control_Nutricional' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar JOIN para obtener el nombre del animal
            Dim query As String = "SELECT CN.ID_ControlNutri, CN.ID_Animal, A.Nombre_Animal, CN.Fecha_Evaluacion, CN.Condicion_Corporal " & _
                                  "FROM Control_Nutricional CN " & _
                                  "INNER JOIN Animal A ON CN.ID_Animal = A.ID_Animal " & _
                                  "ORDER BY CN.Fecha_Evaluacion DESC"
            
            Dim controlesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvControles.DataSource = controlesData
            gvControles.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los controles nutricionales: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim controlId As Integer = Convert.ToInt32(hfControlIdEliminar.Value)
            EliminarControl(controlId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el control nutricional: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim controlId As Integer = 0
                If Not String.IsNullOrEmpty(hfControlId.Value) AndAlso IsNumeric(hfControlId.Value) Then
                    controlId = Convert.ToInt32(hfControlId.Value)
                End If
                
                Dim animalId As Integer = Convert.ToInt32(ddlAnimal.SelectedValue)
                Dim fechaEvaluacion As Date = Convert.ToDateTime(txtFechaEvaluacion.Text)
                Dim condicionCorporal As String = txtCondicionCorporal.Text.Trim()

                Dim result As Integer

                If controlId = 0 Then
                    ' Insertar nuevo control usando procedimiento almacenado
                    result = InsertarControl(animalId, fechaEvaluacion, condicionCorporal)
                    If result > 0 Then
                        MostrarAlerta("Control nutricional registrado correctamente", "success")
                    Else
                        MostrarAlerta("Error al registrar el control nutricional", "danger")
                    End If
                Else
                    ' Actualizar control existente usando procedimiento almacenado
                    result = ActualizarControl(controlId, animalId, fechaEvaluacion, condicionCorporal)
                    If result > 0 Then
                        MostrarAlerta("Control nutricional actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el control nutricional", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarControles()
                    controlModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el control nutricional: " & ex.Message, "danger")
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
            Dim query As String = "SELECT ID_ControlNutri, ID_Animal, Fecha_Evaluacion, Condicion_Corporal FROM Control_Nutricional WHERE ID_ControlNutri = @ControlId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ControlId", controlId)
            }
            
            Dim controlData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If controlData.Rows.Count > 0 Then
                Dim row As DataRow = controlData.Rows(0)
                
                hfControlId.Value = controlId.ToString()
                
                ' Cargar animales si no están cargados
                If ddlAnimal.Items.Count <= 1 Then
                    CargarAnimales()
                End If
                ddlAnimal.SelectedValue = row("ID_Animal").ToString()
                
                ' Formatear fecha para input type="date"
                Dim fechaEvaluacion As DateTime = Convert.ToDateTime(row("Fecha_Evaluacion"))
                txtFechaEvaluacion.Text = fechaEvaluacion.ToString("yyyy-MM-dd")
                
                ' Manejar valores NULL para Condicion_Corporal
                If Not row.IsNull("Condicion_Corporal") Then
                    txtCondicionCorporal.Text = row("Condicion_Corporal").ToString()
                Else
                    txtCondicionCorporal.Text = ""
                End If
                
                modalTitle.InnerText = "Editar Control Nutricional"
                controlModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del control nutricional: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarControl(controlId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarControlNutricional @ID_ControlNutri"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_ControlNutri", controlId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Control nutricional eliminado correctamente", "success")
                CargarControles()
            Else
                MostrarAlerta("Error al eliminar el control nutricional", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el control nutricional: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarControl(animalId As Integer, fechaEvaluacion As Date, condicionCorporal As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_ControlNutri), 0) + 1 FROM Control_Nutricional"
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
            Dim query As String = "EXEC InsertarControlNutricional @ID_ControlNutri, @ID_Animal, @Fecha_Evaluacion, @Condicion_Corporal"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_ControlNutri", nuevoId),
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@Fecha_Evaluacion", fechaEvaluacion),
                New SqlParameter("@Condicion_Corporal", If(String.IsNullOrEmpty(condicionCorporal), CObj(DBNull.Value), condicionCorporal))
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el control nutricional: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarControl(controlId As Integer, animalId As Integer, fechaEvaluacion As Date, condicionCorporal As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarControlNutricional @ID_ControlNutri, @ID_Animal, @Fecha_Evaluacion, @Condicion_Corporal"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_ControlNutri", controlId),
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@Fecha_Evaluacion", fechaEvaluacion),
                New SqlParameter("@Condicion_Corporal", If(String.IsNullOrEmpty(condicionCorporal), CObj(DBNull.Value), condicionCorporal))
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If ddlAnimal.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un animal", "warning")
            Return False
        End If

        If String.IsNullOrEmpty(txtFechaEvaluacion.Text) Then
            MostrarAlerta("La fecha de evaluación es requerida", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlAnimal.Items.Count > 0 Then
            ddlAnimal.SelectedIndex = 0
        End If
        txtFechaEvaluacion.Text = ""
        txtCondicionCorporal.Text = ""
        hfControlId.Value = "0"
        modalTitle.InnerText = "Nuevo Control Nutricional"
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

