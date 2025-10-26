Imports System.Data
Imports System.Data.SqlClient

Partial Public Class TipoPasto
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTipoPastos()
        End If
    End Sub

    Private Sub CargarTipoPastos()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Tipo_Pasto'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Tipo_Pasto' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim tipoPastosData As DataTable = DataAccess.ExecuteStoredProcedure("ListarTipoPasto")
            gvTipoPastos.DataSource = tipoPastosData
            gvTipoPastos.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los tipos de pasto: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim tipoPastoId As Integer = Convert.ToInt32(hfTipoPastoIdEliminar.Value)
            EliminarTipoPasto(tipoPastoId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el tipo de pasto: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim tipoPastoId As Integer = 0
                If Not String.IsNullOrEmpty(hfTipoPastoId.Value) AndAlso IsNumeric(hfTipoPastoId.Value) Then
                    tipoPastoId = Convert.ToInt32(hfTipoPastoId.Value)
                End If
                
                Dim nombrePasto As String = txtNombrePasto.Text.Trim()

                Dim result As Integer

                If tipoPastoId = 0 Then
                    ' Insertar nuevo tipo de pasto usando procedimiento almacenado
                    result = InsertarTipoPasto(nombrePasto)
                    If result > 0 Then
                        MostrarAlerta("Tipo de pasto agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el tipo de pasto", "danger")
                    End If
                Else
                    ' Actualizar tipo de pasto existente usando procedimiento almacenado
                    result = ActualizarTipoPasto(tipoPastoId, nombrePasto)
                    If result > 0 Then
                        MostrarAlerta("Tipo de pasto actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el tipo de pasto", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarTipoPastos()
                    tipoPastoModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el tipo de pasto: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvTipoPastos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim tipoPastoId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarTipoPasto"
                    EditarTipoPasto(tipoPastoId)
                Case "EliminarTipoPasto"
                    EliminarTipoPasto(tipoPastoId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarTipoPasto(tipoPastoId As Integer)
        Try
            Dim query As String = "SELECT ID_TipoPasto, Nombre_Pasto FROM Tipo_Pasto WHERE ID_TipoPasto = @TipoPastoId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@TipoPastoId", tipoPastoId)
            }
            
            Dim tipoPastoData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If tipoPastoData.Rows.Count > 0 Then
                Dim row As DataRow = tipoPastoData.Rows(0)
                
                hfTipoPastoId.Value = tipoPastoId.ToString()
                txtNombrePasto.Text = row("Nombre_Pasto").ToString()
                
                modalTitle.InnerText = "Editar Tipo de Pasto"
                tipoPastoModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del tipo de pasto: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarTipoPasto(tipoPastoId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarTipoPasto @ID_TipoPasto"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_TipoPasto", tipoPastoId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Tipo de pasto eliminado correctamente", "success")
                CargarTipoPastos()
            Else
                MostrarAlerta("Error al eliminar el tipo de pasto", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el tipo de pasto: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarTipoPasto(nombrePasto As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_TipoPasto), 0) + 1 FROM Tipo_Pasto"
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
            Dim query As String = "EXEC InsertarTipoPasto @ID_TipoPasto, @Nombre_Pasto"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_TipoPasto", nuevoId),
                New SqlParameter("@Nombre_Pasto", nombrePasto)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el tipo de pasto: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarTipoPasto(tipoPastoId As Integer, nombrePasto As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarTipoPasto @ID_TipoPasto, @Nombre_Pasto"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_TipoPasto", tipoPastoId),
                New SqlParameter("@Nombre_Pasto", nombrePasto)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombrePasto.Text.Trim()) Then
            MostrarAlerta("El nombre del pasto es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombrePasto.Text = ""
        hfTipoPastoId.Value = "0"
        modalTitle.InnerText = "Nuevo Tipo de Pasto"
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

