Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Cargo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarCargos()
        End If
    End Sub

    Private Sub CargarCargos()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Cargo'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Cargo' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim cargosData As DataTable = DataAccess.ExecuteStoredProcedure("ListarCargo")
            gvCargos.DataSource = cargosData
            gvCargos.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los cargos: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim cargoId As Integer = Convert.ToInt32(hfCargoIdEliminar.Value)
            EliminarCargo(cargoId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el cargo: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim cargoId As Integer = 0
                If Not String.IsNullOrEmpty(hfCargoId.Value) AndAlso IsNumeric(hfCargoId.Value) Then
                    cargoId = Convert.ToInt32(hfCargoId.Value)
                End If
                
                Dim nombreCargo As String = txtNombreCargo.Text.Trim()

                Dim result As Integer

                If cargoId = 0 Then
                    ' Insertar nuevo cargo usando procedimiento almacenado
                    result = InsertarCargo(nombreCargo)
                    If result > 0 Then
                        MostrarAlerta("Cargo agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el cargo", "danger")
                    End If
                Else
                    ' Actualizar cargo existente usando procedimiento almacenado
                    result = ActualizarCargo(cargoId, nombreCargo)
                    If result > 0 Then
                        MostrarAlerta("Cargo actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el cargo", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarCargos()
                    cargoModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el cargo: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvCargos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim cargoId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarCargo"
                    EditarCargo(cargoId)
                Case "EliminarCargo"
                    EliminarCargo(cargoId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarCargo(cargoId As Integer)
        Try
            Dim query As String = "SELECT ID_Cargo, Nombre_Cargo FROM Cargo WHERE ID_Cargo = @CargoId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@CargoId", cargoId)
            }
            
            Dim cargoData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If cargoData.Rows.Count > 0 Then
                Dim row As DataRow = cargoData.Rows(0)
                
                hfCargoId.Value = cargoId.ToString()
                txtNombreCargo.Text = row("Nombre_Cargo").ToString()
                
                modalTitle.InnerText = "Editar Cargo"
                cargoModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del cargo: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarCargo(cargoId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarCargo @ID_Cargo"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Cargo", cargoId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Cargo eliminado correctamente", "success")
                CargarCargos()
            Else
                MostrarAlerta("Error al eliminar el cargo", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el cargo: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarCargo(nombreCargo As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Cargo), 0) + 1 FROM Cargo"
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
            Dim query As String = "EXEC InsertarCargo @ID_Cargo, @Nombre_Cargo"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Cargo", nuevoId),
                New SqlParameter("@Nombre_Cargo", nombreCargo)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el cargo: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarCargo(cargoId As Integer, nombreCargo As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarCargo @ID_Cargo, @Nombre_Cargo"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Cargo", cargoId),
                New SqlParameter("@Nombre_Cargo", nombreCargo)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreCargo.Text.Trim()) Then
            MostrarAlerta("El nombre del cargo es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreCargo.Text = ""
        hfCargoId.Value = "0"
        modalTitle.InnerText = "Nuevo Cargo"
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

