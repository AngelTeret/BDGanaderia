Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Racion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarRaciones()
        End If
    End Sub

    Private Sub CargarRaciones()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Racion'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Racion' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim racionesData As DataTable = DataAccess.ExecuteStoredProcedure("ListarRacion")
            gvRaciones.DataSource = racionesData
            gvRaciones.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar las raciones: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim racionId As Integer = Convert.ToInt32(hfRacionIdEliminar.Value)
            EliminarRacion(racionId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la ración: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim racionId As Integer = 0
                If Not String.IsNullOrEmpty(hfRacionId.Value) AndAlso IsNumeric(hfRacionId.Value) Then
                    racionId = Convert.ToInt32(hfRacionId.Value)
                End If
                
                Dim nombreRacion As String = txtNombreRacion.Text.Trim()

                Dim result As Integer

                If racionId = 0 Then
                    ' Insertar nueva ración usando procedimiento almacenado
                    result = InsertarRacion(nombreRacion)
                    If result > 0 Then
                        MostrarAlerta("Ración agregada correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar la ración", "danger")
                    End If
                Else
                    ' Actualizar ración existente usando procedimiento almacenado
                    result = ActualizarRacion(racionId, nombreRacion)
                    If result > 0 Then
                        MostrarAlerta("Ración actualizada correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar la ración", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarRaciones()
                    racionModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar la ración: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvRaciones_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim racionId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarRacion"
                    EditarRacion(racionId)
                Case "EliminarRacion"
                    EliminarRacion(racionId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarRacion(racionId As Integer)
        Try
            Dim query As String = "SELECT ID_Racion, Nombre_Racion FROM Racion WHERE ID_Racion = @RacionId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@RacionId", racionId)
            }
            
            Dim racionData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If racionData.Rows.Count > 0 Then
                Dim row As DataRow = racionData.Rows(0)
                
                hfRacionId.Value = racionId.ToString()
                txtNombreRacion.Text = row("Nombre_Racion").ToString()
                
                modalTitle.InnerText = "Editar Ración"
                racionModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos de la ración: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarRacion(racionId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarRacion @ID_Racion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Racion", racionId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Ración eliminada correctamente", "success")
                CargarRaciones()
            Else
                MostrarAlerta("Error al eliminar la ración", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la ración: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarRacion(nombreRacion As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Racion), 0) + 1 FROM Racion"
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
            Dim query As String = "EXEC InsertarRacion @ID_Racion, @Nombre_Racion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Racion", nuevoId),
                New SqlParameter("@Nombre_Racion", nombreRacion)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar la ración: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarRacion(racionId As Integer, nombreRacion As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarRacion @ID_Racion, @Nombre_Racion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Racion", racionId),
                New SqlParameter("@Nombre_Racion", nombreRacion)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreRacion.Text.Trim()) Then
            MostrarAlerta("El nombre de la ración es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreRacion.Text = ""
        hfRacionId.Value = "0"
        modalTitle.InnerText = "Nueva Ración"
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

