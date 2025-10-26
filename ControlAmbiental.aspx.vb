Imports System.Data
Imports System.Data.SqlClient

Partial Public Class ControlAmbiental
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarPotreros()
            CargarControles()
        End If
    End Sub

    Private Sub CargarPotreros()
        Try
            Dim query As String = "SELECT ID_Potrero, Nombre_Potrero FROM Potrero ORDER BY Nombre_Potrero"
            Dim potrerosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlPotrero.DataSource = potrerosData
            ddlPotrero.DataTextField = "Nombre_Potrero"
            ddlPotrero.DataValueField = "ID_Potrero"
            ddlPotrero.DataBind()
            ddlPotrero.Items.Insert(0, New ListItem("Seleccione un potrero", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los potreros: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarControles()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Control_Ambiental'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Control_Ambiental' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar JOIN para obtener el nombre del potrero
            Dim query As String = "SELECT CA.ID_ControlAmb, CA.ID_Potrero, P.Nombre_Potrero, CA.Fecha_Control, CA.Observacion " & _
                                  "FROM Control_Ambiental CA " & _
                                  "INNER JOIN Potrero P ON CA.ID_Potrero = P.ID_Potrero " & _
                                  "ORDER BY CA.Fecha_Control DESC"
            
            Dim controlesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvControles.DataSource = controlesData
            gvControles.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los controles ambientales: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim controlId As Integer = Convert.ToInt32(hfControlIdEliminar.Value)
            EliminarControl(controlId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el control ambiental: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim controlId As Integer = 0
                If Not String.IsNullOrEmpty(hfControlId.Value) AndAlso IsNumeric(hfControlId.Value) Then
                    controlId = Convert.ToInt32(hfControlId.Value)
                End If
                
                Dim potreroId As Integer = Convert.ToInt32(ddlPotrero.SelectedValue)
                Dim fechaControl As Date = Convert.ToDateTime(txtFechaControl.Text)
                Dim observacion As String = txtObservacion.Text.Trim()

                Dim result As Integer

                If controlId = 0 Then
                    ' Insertar nuevo control usando procedimiento almacenado
                    result = InsertarControl(potreroId, fechaControl, observacion)
                    If result > 0 Then
                        MostrarAlerta("Control ambiental registrado correctamente", "success")
                    Else
                        MostrarAlerta("Error al registrar el control ambiental", "danger")
                    End If
                Else
                    ' Actualizar control existente usando procedimiento almacenado
                    result = ActualizarControl(controlId, potreroId, fechaControl, observacion)
                    If result > 0 Then
                        MostrarAlerta("Control ambiental actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el control ambiental", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarControles()
                    controlModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el control ambiental: " & ex.Message, "danger")
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
            Dim query As String = "SELECT ID_ControlAmb, ID_Potrero, Fecha_Control, Observacion FROM Control_Ambiental WHERE ID_ControlAmb = @ControlId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ControlId", controlId)
            }
            
            Dim controlData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If controlData.Rows.Count > 0 Then
                Dim row As DataRow = controlData.Rows(0)
                
                hfControlId.Value = controlId.ToString()
                
                ' Cargar potreros si no están cargados
                If ddlPotrero.Items.Count <= 1 Then
                    CargarPotreros()
                End If
                ddlPotrero.SelectedValue = row("ID_Potrero").ToString()
                
                ' Formatear fecha para input type="date"
                Dim fechaControl As DateTime = Convert.ToDateTime(row("Fecha_Control"))
                txtFechaControl.Text = fechaControl.ToString("yyyy-MM-dd")
                
                ' Manejar valores NULL para Observacion
                If Not row.IsNull("Observacion") Then
                    txtObservacion.Text = row("Observacion").ToString()
                Else
                    txtObservacion.Text = ""
                End If
                
                modalTitle.InnerText = "Editar Control Ambiental"
                controlModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del control ambiental: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarControl(controlId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarControlAmbiental @ID_ControlAmb"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_ControlAmb", controlId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Control ambiental eliminado correctamente", "success")
                CargarControles()
            Else
                MostrarAlerta("Error al eliminar el control ambiental", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el control ambiental: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarControl(potreroId As Integer, fechaControl As Date, observacion As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_ControlAmb), 0) + 1 FROM Control_Ambiental"
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
            Dim query As String = "EXEC InsertarControlAmbiental @ID_ControlAmb, @ID_Potrero, @Fecha_Control, @Observacion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_ControlAmb", nuevoId),
                New SqlParameter("@ID_Potrero", potreroId),
                New SqlParameter("@Fecha_Control", fechaControl),
                New SqlParameter("@Observacion", If(String.IsNullOrEmpty(observacion), CObj(DBNull.Value), observacion))
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el control ambiental: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarControl(controlId As Integer, potreroId As Integer, fechaControl As Date, observacion As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarControlAmbiental @ID_ControlAmb, @ID_Potrero, @Fecha_Control, @Observacion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_ControlAmb", controlId),
                New SqlParameter("@ID_Potrero", potreroId),
                New SqlParameter("@Fecha_Control", fechaControl),
                New SqlParameter("@Observacion", If(String.IsNullOrEmpty(observacion), CObj(DBNull.Value), observacion))
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If ddlPotrero.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un potrero", "warning")
            Return False
        End If

        If String.IsNullOrEmpty(txtFechaControl.Text) Then
            MostrarAlerta("La fecha de control es requerida", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlPotrero.Items.Count > 0 Then
            ddlPotrero.SelectedIndex = 0
        End If
        txtFechaControl.Text = ""
        txtObservacion.Text = ""
        hfControlId.Value = "0"
        modalTitle.InnerText = "Nuevo Control Ambiental"
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

