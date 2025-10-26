Imports System.Data
Imports System.Data.SqlClient

Partial Public Class RacionAlimento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarRaciones()
            CargarAlimentos()
            CargarAsignaciones()
        End If
    End Sub

    Private Sub CargarRaciones()
        Try
            Dim query As String = "SELECT ID_Racion, Nombre_Racion FROM Racion ORDER BY Nombre_Racion"
            Dim racionesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlRacion.DataSource = racionesData
            ddlRacion.DataTextField = "Nombre_Racion"
            ddlRacion.DataValueField = "ID_Racion"
            ddlRacion.DataBind()
            ddlRacion.Items.Insert(0, New ListItem("Seleccione una ración", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar las raciones: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarAlimentos()
        Try
            Dim query As String = "SELECT ID_Alimento, Nombre_Alimento FROM Alimento ORDER BY Nombre_Alimento"
            Dim alimentosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlAlimento.DataSource = alimentosData
            ddlAlimento.DataTextField = "Nombre_Alimento"
            ddlAlimento.DataValueField = "ID_Alimento"
            ddlAlimento.DataBind()
            ddlAlimento.Items.Insert(0, New ListItem("Seleccione un alimento", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los alimentos: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarAsignaciones()
        Try
            ' Verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Racion_Alimento'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Racion_Alimento' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Obtener asignaciones con JOINs para mostrar nombres
            Dim query As String = "SELECT RA.ID_Racion, R.Nombre_Racion, RA.ID_Alimento, A.Nombre_Alimento, RA.Cantidad " & _
                                  "FROM Racion_Alimento RA " & _
                                  "INNER JOIN Racion R ON RA.ID_Racion = R.ID_Racion " & _
                                  "INNER JOIN Alimento A ON RA.ID_Alimento = A.ID_Alimento " & _
                                  "ORDER BY R.Nombre_Racion, A.Nombre_Alimento"
            
            Dim asignacionesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvAsignaciones.DataSource = asignacionesData
            gvAsignaciones.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar las asignaciones: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim racionId As Integer = Convert.ToInt32(hfRacionIdEliminar.Value)
            Dim alimentoId As Integer = Convert.ToInt32(hfAlimentoIdEliminar.Value)
            EliminarAsignacion(racionId, alimentoId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la asignación: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim racionId As Integer = Convert.ToInt32(ddlRacion.SelectedValue)
                Dim alimentoId As Integer = Convert.ToInt32(ddlAlimento.SelectedValue)
                Dim cantidad As Decimal = 0
                
                If Not String.IsNullOrEmpty(txtCantidad.Text) Then
                    cantidad = ParseDecimalFlexible(txtCantidad.Text)
                End If

                ' Verificar si estamos editando o creando
                Dim editandoRacionId As Integer = 0
                Dim editandoAlimentoId As Integer = 0
                
                If Not String.IsNullOrEmpty(hfRacionId.Value) AndAlso IsNumeric(hfRacionId.Value) Then
                    editandoRacionId = Convert.ToInt32(hfRacionId.Value)
                End If
                
                If Not String.IsNullOrEmpty(hfAlimentoId.Value) AndAlso IsNumeric(hfAlimentoId.Value) Then
                    editandoAlimentoId = Convert.ToInt32(hfAlimentoId.Value)
                End If

                Dim result As Integer

                If editandoRacionId = 0 AndAlso editandoAlimentoId = 0 Then
                    ' Insertar nueva asignación
                    If ExisteAsignacion(racionId, alimentoId) Then
                        MostrarAlerta("Este alimento ya está asignado a esta ración", "warning")
                        Return
                    End If
                    
                    result = InsertarAsignacion(racionId, alimentoId, cantidad)
                    
                    If result > 0 Then
                        MostrarAlerta("Alimento asignado correctamente", "success")
                    Else
                        MostrarAlerta("Error al asignar el alimento", "danger")
                    End If
                Else
                    ' Actualizar asignación existente
                    result = ActualizarAsignacion(racionId, alimentoId, cantidad)
                    
                    If result > 0 Then
                        MostrarAlerta("Cantidad actualizada correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar la cantidad", "danger")
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
                        Dim racionId As Integer = Convert.ToInt32(args(0))
                        Dim alimentoId As Integer = Convert.ToInt32(args(1))
                        EditarAsignacion(racionId, alimentoId)
                    End If
                Case "EliminarAsignacion"
                    If args.Length = 2 Then
                        Dim racionId As Integer = Convert.ToInt32(args(0))
                        Dim alimentoId As Integer = Convert.ToInt32(args(1))
                        EliminarAsignacion(racionId, alimentoId)
                    End If
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarAsignacion(racionId As Integer, alimentoId As Integer)
        Try
            Dim query As String = "SELECT ID_Racion, ID_Alimento, Cantidad FROM Racion_Alimento WHERE ID_Racion = @ID_Racion AND ID_Alimento = @ID_Alimento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Racion", racionId),
                New SqlParameter("@ID_Alimento", alimentoId)
            }
            
            Dim asignacionData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If asignacionData.Rows.Count > 0 Then
                Dim row As DataRow = asignacionData.Rows(0)
                
                hfRacionId.Value = racionId.ToString()
                hfAlimentoId.Value = alimentoId.ToString()
                
                ' Cargar raciones si no están cargadas
                If ddlRacion.Items.Count <= 1 Then
                    CargarRaciones()
                End If
                ddlRacion.SelectedValue = row("ID_Racion").ToString()
                ddlRacion.Enabled = False ' No permitir cambiar la ración al editar
                
                ' Cargar alimentos si no están cargados
                If ddlAlimento.Items.Count <= 1 Then
                    CargarAlimentos()
                End If
                ddlAlimento.SelectedValue = row("ID_Alimento").ToString()
                ddlAlimento.Enabled = False ' No permitir cambiar el alimento al editar
                
                ' Mostrar cantidad
                If Not row.IsNull("Cantidad") Then
                    Dim cantidadValue As Decimal = Convert.ToDecimal(row("Cantidad"))
                    txtCantidad.Text = cantidadValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                Else
                    txtCantidad.Text = "0.00"
                End If
                
                modalTitle.InnerText = "Editar Cantidad"
                asignacionModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos de la asignación: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarAsignacion(racionId As Integer, alimentoId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarRacionAlimento @ID_Racion, @ID_Alimento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Racion", racionId),
                New SqlParameter("@ID_Alimento", alimentoId)
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

    Private Function InsertarAsignacion(racionId As Integer, alimentoId As Integer, cantidad As Decimal) As Integer
        Try
            ' Usar procedimiento almacenado para insertar
            Dim query As String = "EXEC InsertarRacionAlimento @ID_Racion, @ID_Alimento, @Cantidad"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Racion", racionId),
                New SqlParameter("@ID_Alimento", alimentoId),
                New SqlParameter("@Cantidad", cantidad)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar la asignación: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarAsignacion(racionId As Integer, alimentoId As Integer, cantidad As Decimal) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarRacionAlimento @ID_Racion, @ID_Alimento, @Cantidad"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Racion", racionId),
                New SqlParameter("@ID_Alimento", alimentoId),
                New SqlParameter("@Cantidad", cantidad)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al actualizar la asignación: " & ex.Message)
        End Try
    End Function

    Private Function ExisteAsignacion(racionId As Integer, alimentoId As Integer) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM Racion_Alimento WHERE ID_Racion = @ID_Racion AND ID_Alimento = @ID_Alimento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Racion", racionId),
                New SqlParameter("@ID_Alimento", alimentoId)
            }
            
            Dim result As Object = DataAccess.ExecuteScalar(query, parameters)
            Return Convert.ToInt32(result) > 0
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function ParseDecimalFlexible(input As String) As Decimal
        If String.IsNullOrWhiteSpace(input) Then Throw New FormatException("Vacío")
        
        Dim s = input.Trim().Replace(" ", "")
        Dim tienePunto As Boolean = s.Contains(".")
        Dim tieneComa As Boolean = s.Contains(",")
        
        If tienePunto AndAlso tieneComa Then
            If s.LastIndexOf(".") > s.LastIndexOf(",") Then
                s = s.Replace(",", "")
            Else
                s = s.Replace(".", "")
                s = s.Replace(",", ".")
            End If
        ElseIf tieneComa Then
            s = s.Replace(".", "")
            s = s.Replace(",", ".")
        ElseIf tienePunto Then
            s = s.Replace(",", "")
        End If

        Dim value As Decimal
        If Decimal.TryParse(s, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, value) Then
            Return value
        End If
        Throw New FormatException("No se pudo convertir '" & input & "' a decimal")
    End Function

    Private Function ValidarFormulario() As Boolean
        If ddlRacion.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar una ración", "warning")
            Return False
        End If

        If ddlAlimento.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un alimento", "warning")
            Return False
        End If

        If String.IsNullOrEmpty(txtCantidad.Text) Then
            MostrarAlerta("La cantidad es requerida", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlRacion.Items.Count > 0 Then
            ddlRacion.SelectedIndex = 0
            ddlRacion.Enabled = True
        End If
        If ddlAlimento.Items.Count > 0 Then
            ddlAlimento.SelectedIndex = 0
            ddlAlimento.Enabled = True
        End If
        txtCantidad.Text = ""
        hfRacionId.Value = ""
        hfAlimentoId.Value = ""
        modalTitle.InnerText = "Nueva Asignación de Alimento"
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

