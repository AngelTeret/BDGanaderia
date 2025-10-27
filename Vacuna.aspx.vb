Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Vacuna
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarVacunas()
        End If
    End Sub

    Private Sub CargarVacunas()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Vacuna'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Vacuna' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim vacunasData As DataTable = DataAccess.ExecuteStoredProcedure("ListarVacuna")
            gvVacunas.DataSource = vacunasData
            gvVacunas.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar las vacunas: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim vacunaId As Integer = Convert.ToInt32(hfVacunaIdEliminar.Value)
            EliminarVacuna(vacunaId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la vacuna: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim vacunaId As Integer = 0
                If Not String.IsNullOrEmpty(hfVacunaId.Value) AndAlso IsNumeric(hfVacunaId.Value) Then
                    vacunaId = Convert.ToInt32(hfVacunaId.Value)
                End If
                
                Dim nombreVacuna As String = txtNombreVacuna.Text.Trim()

                Dim result As Integer

                If vacunaId = 0 Then
                    ' Insertar nueva vacuna usando procedimiento almacenado
                    result = InsertarVacuna(nombreVacuna)
                    If result > 0 Then
                        MostrarAlerta("Vacuna agregada correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar la vacuna", "danger")
                    End If
                Else
                    ' Actualizar vacuna existente usando procedimiento almacenado
                    result = ActualizarVacuna(vacunaId, nombreVacuna)
                    If result > 0 Then
                        MostrarAlerta("Vacuna actualizada correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar la vacuna", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarVacunas()
                    vacunaModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar la vacuna: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvVacunas_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim vacunaId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarVacuna"
                    EditarVacuna(vacunaId)
                Case "EliminarVacuna"
                    EliminarVacuna(vacunaId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarVacuna(vacunaId As Integer)
        Try
            Dim query As String = "SELECT ID_Vacuna, Nombre_Vacuna FROM Vacuna WHERE ID_Vacuna = @VacunaId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@VacunaId", vacunaId)
            }
            
            Dim vacunaData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If vacunaData.Rows.Count > 0 Then
                Dim row As DataRow = vacunaData.Rows(0)
                
                hfVacunaId.Value = vacunaId.ToString()
                txtNombreVacuna.Text = row("Nombre_Vacuna").ToString()
                
                modalTitle.InnerText = "Editar Vacuna"
                vacunaModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos de la vacuna: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarVacuna(vacunaId As Integer)
        Try
            ' Obtener nombre antes de eliminar
            Dim nombreVacuna As String = ""
            Try
                Dim queryNombre As String = "SELECT Nombre_Vacuna FROM Vacuna WHERE ID_Vacuna = @ID_Vacuna"
                Dim nombreObj As Object = DataAccess.ExecuteScalar(queryNombre, New SqlParameter() {New SqlParameter("@ID_Vacuna", vacunaId)})
                If nombreObj IsNot Nothing Then nombreVacuna = nombreObj.ToString()
            Catch
            End Try
            
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarVacuna @ID_Vacuna"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Vacuna", vacunaId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                ' Registrar en bitácora
                Try
                    BitacoraHelper.RegistrarEliminar("Vacuna", vacunaId.ToString(), "Nombre: " & nombreVacuna)
                Catch
                End Try
                
                MostrarAlerta("Vacuna eliminada correctamente", "success")
                CargarVacunas()
            Else
                MostrarAlerta("Error al eliminar la vacuna", "danger")
            End If
        Catch ex As Exception
            Try
                BitacoraHelper.RegistrarError(ex, "Vacuna.aspx")
            Catch
            End Try
            MostrarAlerta("Error al eliminar la vacuna: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarVacuna(nombreVacuna As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Vacuna), 0) + 1 FROM Vacuna"
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
            Dim query As String = "EXEC InsertarVacuna @ID_Vacuna, @Nombre_Vacuna"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Vacuna", nuevoId),
                New SqlParameter("@Nombre_Vacuna", nombreVacuna)
            }
            
            Dim resultado As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            
            ' Registrar en bitácora
            If resultado > 0 Then
                Try
                    BitacoraHelper.RegistrarCrear("Vacuna", nuevoId.ToString(), "Nombre: " & nombreVacuna)
                Catch
                End Try
            End If
            
            Return resultado
        Catch ex As Exception
            Try
                BitacoraHelper.RegistrarError(ex, "Vacuna.aspx")
            Catch
            End Try
            Throw New Exception("Error al insertar la vacuna: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarVacuna(vacunaId As Integer, nombreVacuna As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarVacuna @ID_Vacuna, @Nombre_Vacuna"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Vacuna", vacunaId),
                New SqlParameter("@Nombre_Vacuna", nombreVacuna)
            }
            
            Dim resultado As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            
            ' Registrar en bitácora
            If resultado > 0 Then
                Try
                    BitacoraHelper.RegistrarActualizar("Vacuna", vacunaId.ToString(), "Nombre: " & nombreVacuna)
                Catch
                End Try
            End If
            
            Return resultado
        Catch ex As Exception
            Try
                BitacoraHelper.RegistrarError(ex, "Vacuna.aspx")
            Catch
            End Try
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreVacuna.Text.Trim()) Then
            MostrarAlerta("El nombre de la vacuna es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreVacuna.Text = ""
        hfVacunaId.Value = "0"
        modalTitle.InnerText = "Nueva Vacuna"
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

