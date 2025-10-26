Imports System.Data
Imports System.Data.SqlClient

Partial Public Class GestionAgua
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarPotreros()
            CargarGestiones()
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

    Private Sub CargarGestiones()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Gestion_Agua'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Gestion_Agua' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar JOIN para obtener el nombre del potrero
            Dim query As String = "SELECT GA.ID_GestionAgua, GA.ID_Potrero, P.Nombre_Potrero, GA.Fecha_Revision, GA.Estado_Bebedero " & _
                                  "FROM Gestion_Agua GA " & _
                                  "INNER JOIN Potrero P ON GA.ID_Potrero = P.ID_Potrero " & _
                                  "ORDER BY GA.Fecha_Revision DESC"
            
            Dim gestionesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvGestiones.DataSource = gestionesData
            gvGestiones.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar las revisiones de bebederos: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim gestionId As Integer = Convert.ToInt32(hfGestionIdEliminar.Value)
            EliminarGestion(gestionId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la revisión de bebedero: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim gestionId As Integer = 0
                If Not String.IsNullOrEmpty(hfGestionId.Value) AndAlso IsNumeric(hfGestionId.Value) Then
                    gestionId = Convert.ToInt32(hfGestionId.Value)
                End If
                
                Dim potreroId As Integer = Convert.ToInt32(ddlPotrero.SelectedValue)
                Dim fechaRevision As Date = Convert.ToDateTime(txtFechaRevision.Text)
                Dim estadoBebedero As String = txtEstadoBebedero.Text.Trim()

                Dim result As Integer

                If gestionId = 0 Then
                    ' Insertar nuevo registro usando procedimiento almacenado
                    result = InsertarGestion(potreroId, fechaRevision, estadoBebedero)
                    If result > 0 Then
                        MostrarAlerta("Revisión de bebedero registrada correctamente", "success")
                    Else
                        MostrarAlerta("Error al registrar la revisión de bebedero", "danger")
                    End If
                Else
                    ' Actualizar registro existente usando procedimiento almacenado
                    result = ActualizarGestion(gestionId, potreroId, fechaRevision, estadoBebedero)
                    If result > 0 Then
                        MostrarAlerta("Revisión de bebedero actualizada correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar la revisión de bebedero", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarGestiones()
                    gestionModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar la revisión de bebedero: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvGestiones_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim gestionId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarGestion"
                    EditarGestion(gestionId)
                Case "EliminarGestion"
                    EliminarGestion(gestionId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarGestion(gestionId As Integer)
        Try
            Dim query As String = "SELECT ID_GestionAgua, ID_Potrero, Fecha_Revision, Estado_Bebedero FROM Gestion_Agua WHERE ID_GestionAgua = @GestionId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@GestionId", gestionId)
            }
            
            Dim gestionData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If gestionData.Rows.Count > 0 Then
                Dim row As DataRow = gestionData.Rows(0)
                
                hfGestionId.Value = gestionId.ToString()
                
                ' Cargar potreros si no están cargados
                If ddlPotrero.Items.Count <= 1 Then
                    CargarPotreros()
                End If
                ddlPotrero.SelectedValue = row("ID_Potrero").ToString()
                
                ' Formatear fecha para input type="date"
                Dim fechaRevision As DateTime = Convert.ToDateTime(row("Fecha_Revision"))
                txtFechaRevision.Text = fechaRevision.ToString("yyyy-MM-dd")
                
                ' Manejar valores NULL para Estado_Bebedero
                If Not row.IsNull("Estado_Bebedero") Then
                    txtEstadoBebedero.Text = row("Estado_Bebedero").ToString()
                Else
                    txtEstadoBebedero.Text = ""
                End If
                
                modalTitle.InnerText = "Editar Revisión de Bebedero"
                gestionModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos de la revisión de bebedero: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarGestion(gestionId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarGestionAgua @ID_GestionAgua"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_GestionAgua", gestionId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Revisión de bebedero eliminada correctamente", "success")
                CargarGestiones()
            Else
                MostrarAlerta("Error al eliminar la revisión de bebedero", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la revisión de bebedero: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarGestion(potreroId As Integer, fechaRevision As Date, estadoBebedero As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_GestionAgua), 0) + 1 FROM Gestion_Agua"
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
            Dim query As String = "EXEC InsertarGestionAgua @ID_GestionAgua, @ID_Potrero, @Fecha_Revision, @Estado_Bebedero"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_GestionAgua", nuevoId),
                New SqlParameter("@ID_Potrero", potreroId),
                New SqlParameter("@Fecha_Revision", fechaRevision),
                New SqlParameter("@Estado_Bebedero", If(String.IsNullOrEmpty(estadoBebedero), CObj(DBNull.Value), estadoBebedero))
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar la revisión de bebedero: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarGestion(gestionId As Integer, potreroId As Integer, fechaRevision As Date, estadoBebedero As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarGestionAgua @ID_GestionAgua, @ID_Potrero, @Fecha_Revision, @Estado_Bebedero"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_GestionAgua", gestionId),
                New SqlParameter("@ID_Potrero", potreroId),
                New SqlParameter("@Fecha_Revision", fechaRevision),
                New SqlParameter("@Estado_Bebedero", If(String.IsNullOrEmpty(estadoBebedero), CObj(DBNull.Value), estadoBebedero))
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

        If String.IsNullOrEmpty(txtFechaRevision.Text) Then
            MostrarAlerta("La fecha de revisión es requerida", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlPotrero.Items.Count > 0 Then
            ddlPotrero.SelectedIndex = 0
        End If
        txtFechaRevision.Text = ""
        txtEstadoBebedero.Text = ""
        hfGestionId.Value = "0"
        modalTitle.InnerText = "Nueva Revisión de Bebedero"
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

