Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Raza
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarRazas()
        End If
    End Sub

    Private Sub CargarRazas()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Raza'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Raza' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            Dim query As String = "SELECT ID_Raza, Nombre_Raza FROM Raza ORDER BY ID_Raza ASC"
            Dim razasData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvRazas.DataSource = razasData
            gvRazas.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar las razas: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim razaId As Integer = Convert.ToInt32(hfRazaIdEliminar.Value)
            EliminarRaza(razaId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la raza: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim razaId As Integer = 0
                If Not String.IsNullOrEmpty(hfRazaId.Value) AndAlso IsNumeric(hfRazaId.Value) Then
                    razaId = Convert.ToInt32(hfRazaId.Value)
                End If
                
                Dim nombreRaza As String = txtNombreRaza.Text.Trim()

                Dim result As Integer

                If razaId = 0 Then
                    ' Insertar nueva raza
                    result = InsertarRaza(nombreRaza)
                    If result > 0 Then
                        MostrarAlerta("Raza agregada correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar la raza", "danger")
                    End If
                Else
                    ' Actualizar raza existente
                    result = ActualizarRaza(razaId, nombreRaza)
                    If result > 0 Then
                        MostrarAlerta("Raza actualizada correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar la raza", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarRazas()
                    razaModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar la raza: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvRazas_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim razaId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarRaza"
                    EditarRaza(razaId)
                Case "EliminarRaza"
                    EliminarRaza(razaId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarRaza(razaId As Integer)
        Try
            Dim query As String = "SELECT ID_Raza, Nombre_Raza FROM Raza WHERE ID_Raza = @RazaId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@RazaId", razaId)
            }
            
            Dim razaData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If razaData.Rows.Count > 0 Then
                Dim row As DataRow = razaData.Rows(0)
                
                hfRazaId.Value = razaId.ToString()
                txtNombreRaza.Text = row("Nombre_Raza").ToString()
                
                modalTitle.InnerText = "Editar Raza"
                razaModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos de la raza: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarRaza(razaId As Integer)
        Try
            Dim query As String = "DELETE FROM Raza WHERE ID_Raza = @RazaId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@RazaId", razaId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Raza eliminada correctamente", "success")
                CargarRazas()
            Else
                MostrarAlerta("Error al eliminar la raza", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la raza: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarRaza(nombreRaza As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Raza), 0) + 1 FROM Raza"
            Dim result As Object = DataAccess.ExecuteScalar(queryId)
            Dim nuevoId As Integer = 1
            
            If result IsNot Nothing Then
                Try
                    nuevoId = Convert.ToInt32(result)
                Catch
                    nuevoId = 1
                End Try
            End If
            
            Dim query As String = "INSERT INTO Raza (ID_Raza, Nombre_Raza) VALUES (@ID_Raza, @Nombre_Raza)"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Raza", nuevoId),
                New SqlParameter("@Nombre_Raza", nombreRaza)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar la raza: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarRaza(razaId As Integer, nombreRaza As String) As Integer
        Try
            Dim query As String = "UPDATE Raza SET Nombre_Raza = @Nombre_Raza WHERE ID_Raza = @ID_Raza"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Raza", razaId),
                New SqlParameter("@Nombre_Raza", nombreRaza)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreRaza.Text.Trim()) Then
            MostrarAlerta("El nombre de la raza es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreRaza.Text = ""
        hfRazaId.Value = "0"
        modalTitle.InnerText = "Nueva Raza"
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

