Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Corral
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarCorrales()
        End If
    End Sub

    Private Sub CargarCorrales()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Corral'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Corral' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim corralesData As DataTable = DataAccess.ExecuteStoredProcedure("ListarCorral")
            gvCorrales.DataSource = corralesData
            gvCorrales.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los corrales: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim corralId As Integer = Convert.ToInt32(hfCorralIdEliminar.Value)
            EliminarCorral(corralId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el corral: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim corralId As Integer = 0
                If Not String.IsNullOrEmpty(hfCorralId.Value) AndAlso IsNumeric(hfCorralId.Value) Then
                    corralId = Convert.ToInt32(hfCorralId.Value)
                End If
                
                Dim nombreCorral As String = txtNombreCorral.Text.Trim()
                Dim capacidad As Integer = 0
                
                If Not String.IsNullOrEmpty(txtCapacidad.Text) AndAlso IsNumeric(txtCapacidad.Text) Then
                    capacidad = Convert.ToInt32(txtCapacidad.Text)
                End If

                Dim result As Integer

                If corralId = 0 Then
                    ' Insertar nuevo corral usando procedimiento almacenado
                    result = InsertarCorral(nombreCorral, capacidad)
                    If result > 0 Then
                        MostrarAlerta("Corral agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el corral", "danger")
                    End If
                Else
                    ' Actualizar corral existente usando procedimiento almacenado
                    result = ActualizarCorral(corralId, nombreCorral, capacidad)
                    If result > 0 Then
                        MostrarAlerta("Corral actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el corral", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarCorrales()
                    corralModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el corral: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvCorrales_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim corralId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarCorral"
                    EditarCorral(corralId)
                Case "EliminarCorral"
                    EliminarCorral(corralId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarCorral(corralId As Integer)
        Try
            Dim query As String = "SELECT ID_Corral, Nombre_Corral, Capacidad FROM Corral WHERE ID_Corral = @CorralId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@CorralId", corralId)
            }
            
            Dim corralData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If corralData.Rows.Count > 0 Then
                Dim row As DataRow = corralData.Rows(0)
                
                hfCorralId.Value = corralId.ToString()
                txtNombreCorral.Text = row("Nombre_Corral").ToString()
                
                ' Manejar valores NULL para Capacidad
                If Not row.IsNull("Capacidad") Then
                    txtCapacidad.Text = row("Capacidad").ToString()
                Else
                    txtCapacidad.Text = ""
                End If
                
                modalTitle.InnerText = "Editar Corral"
                corralModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del corral: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarCorral(corralId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarCorral @ID_Corral"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Corral", corralId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Corral eliminado correctamente", "success")
                CargarCorrales()
            Else
                MostrarAlerta("Error al eliminar el corral", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el corral: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarCorral(nombreCorral As String, capacidad As Integer) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Corral), 0) + 1 FROM Corral"
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
            Dim query As String = "EXEC InsertarCorral @ID_Corral, @Nombre_Corral, @Capacidad"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Corral", nuevoId),
                New SqlParameter("@Nombre_Corral", nombreCorral),
                New SqlParameter("@Capacidad", capacidad)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el corral: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarCorral(corralId As Integer, nombreCorral As String, capacidad As Integer) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarCorral @ID_Corral, @Nombre_Corral, @Capacidad"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Corral", corralId),
                New SqlParameter("@Nombre_Corral", nombreCorral),
                New SqlParameter("@Capacidad", capacidad)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreCorral.Text.Trim()) Then
            MostrarAlerta("El nombre del corral es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreCorral.Text = ""
        txtCapacidad.Text = ""
        hfCorralId.Value = "0"
        modalTitle.InnerText = "Nuevo Corral"
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

