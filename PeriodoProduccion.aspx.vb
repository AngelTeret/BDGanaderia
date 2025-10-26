Imports System.Data
Imports System.Data.SqlClient

Partial Public Class PeriodoProduccion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarPeriodos()
        End If
    End Sub

    Private Sub CargarPeriodos()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Periodo_Produccion'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Periodo_Produccion' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim periodosData As DataTable = DataAccess.ExecuteStoredProcedure("ListarPeriodoProduccion")
            gvPeriodos.DataSource = periodosData
            gvPeriodos.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los períodos de producción: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim periodoId As Integer = Convert.ToInt32(hfPeriodoIdEliminar.Value)
            EliminarPeriodo(periodoId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el período de producción: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim periodoId As Integer = 0
                If Not String.IsNullOrEmpty(hfPeriodoId.Value) AndAlso IsNumeric(hfPeriodoId.Value) Then
                    periodoId = Convert.ToInt32(hfPeriodoId.Value)
                End If
                
                Dim nombrePeriodo As String = txtNombrePeriodo.Text.Trim()

                Dim result As Integer

                If periodoId = 0 Then
                    ' Insertar nuevo período usando procedimiento almacenado
                    result = InsertarPeriodo(nombrePeriodo)
                    If result > 0 Then
                        MostrarAlerta("Período de producción agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el período de producción", "danger")
                    End If
                Else
                    ' Actualizar período existente usando procedimiento almacenado
                    result = ActualizarPeriodo(periodoId, nombrePeriodo)
                    If result > 0 Then
                        MostrarAlerta("Período de producción actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el período de producción", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarPeriodos()
                    periodoModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el período de producción: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvPeriodos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim periodoId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarPeriodo"
                    EditarPeriodo(periodoId)
                Case "EliminarPeriodo"
                    EliminarPeriodo(periodoId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarPeriodo(periodoId As Integer)
        Try
            Dim query As String = "SELECT ID_Periodo, Nombre_Periodo FROM Periodo_Produccion WHERE ID_Periodo = @PeriodoId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@PeriodoId", periodoId)
            }
            
            Dim periodoData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If periodoData.Rows.Count > 0 Then
                Dim row As DataRow = periodoData.Rows(0)
                
                hfPeriodoId.Value = periodoId.ToString()
                txtNombrePeriodo.Text = row("Nombre_Periodo").ToString()
                
                modalTitle.InnerText = "Editar Período de Producción"
                periodoModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del período de producción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarPeriodo(periodoId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarPeriodoProduccion @ID_Periodo"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Periodo", periodoId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Período de producción eliminado correctamente", "success")
                CargarPeriodos()
            Else
                MostrarAlerta("Error al eliminar el período de producción", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el período de producción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarPeriodo(nombrePeriodo As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Periodo), 0) + 1 FROM Periodo_Produccion"
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
            Dim query As String = "EXEC InsertarPeriodoProduccion @ID_Periodo, @Nombre_Periodo"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Periodo", nuevoId),
                New SqlParameter("@Nombre_Periodo", nombrePeriodo)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el período de producción: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarPeriodo(periodoId As Integer, nombrePeriodo As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarPeriodoProduccion @ID_Periodo, @Nombre_Periodo"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Periodo", periodoId),
                New SqlParameter("@Nombre_Periodo", nombrePeriodo)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombrePeriodo.Text.Trim()) Then
            MostrarAlerta("El nombre del período es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombrePeriodo.Text = ""
        hfPeriodoId.Value = "0"
        modalTitle.InnerText = "Nuevo Período de Producción"
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

