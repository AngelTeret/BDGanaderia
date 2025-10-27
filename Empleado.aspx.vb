Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Empleado
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarEmpleados()
        End If
    End Sub

    Private Sub CargarEmpleados()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Empleado'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Empleado' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim empleadosData As DataTable = DataAccess.ExecuteStoredProcedure("ListarEmpleado")
            gvEmpleados.DataSource = empleadosData
            gvEmpleados.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los empleados: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim empleadoId As Integer = Convert.ToInt32(hfEmpleadoIdEliminar.Value)
            EliminarEmpleado(empleadoId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el empleado: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim empleadoId As Integer = 0
                If Not String.IsNullOrEmpty(hfEmpleadoId.Value) AndAlso IsNumeric(hfEmpleadoId.Value) Then
                    empleadoId = Convert.ToInt32(hfEmpleadoId.Value)
                End If
                
                Dim nombreEmpleado As String = txtNombreEmpleado.Text.Trim()
                Dim fechaContratacion As Date = Date.MinValue
                
                If Not String.IsNullOrEmpty(txtFechaContratacion.Text) Then
                    If Date.TryParse(txtFechaContratacion.Text, fechaContratacion) Then
                        ' fechaContratacion ya tiene el valor parseado
                    End If
                End If

                Dim result As Integer

                If empleadoId = 0 Then
                    ' Insertar nuevo empleado usando procedimiento almacenado
                    result = InsertarEmpleado(nombreEmpleado, fechaContratacion)
                    If result > 0 Then
                        MostrarAlerta("Empleado agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el empleado", "danger")
                    End If
                Else
                    ' Actualizar empleado existente usando procedimiento almacenado
                    result = ActualizarEmpleado(empleadoId, nombreEmpleado, fechaContratacion)
                    If result > 0 Then
                        MostrarAlerta("Empleado actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el empleado", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarEmpleados()
                    empleadoModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el empleado: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvEmpleados_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim empleadoId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarEmpleado"
                    EditarEmpleado(empleadoId)
                Case "EliminarEmpleado"
                    EliminarEmpleado(empleadoId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarEmpleado(empleadoId As Integer)
        Try
            Dim query As String = "SELECT ID_Empleado, Nombre_Empleado, Fecha_Contratacion FROM Empleado WHERE ID_Empleado = @EmpleadoId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@EmpleadoId", empleadoId)
            }
            
            Dim empleadoData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If empleadoData.Rows.Count > 0 Then
                Dim row As DataRow = empleadoData.Rows(0)
                
                hfEmpleadoId.Value = empleadoId.ToString()
                txtNombreEmpleado.Text = row("Nombre_Empleado").ToString()
                
                ' Manejar valores NULL para Fecha_Contratacion
                If Not row.IsNull("Fecha_Contratacion") Then
                    Dim fechaContratacion As Date = Convert.ToDateTime(row("Fecha_Contratacion"))
                    txtFechaContratacion.Text = fechaContratacion.ToString("yyyy-MM-dd")
                End If
                
                modalTitle.InnerText = "Editar Empleado"
                empleadoModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del empleado: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarEmpleado(empleadoId As Integer)
        Try
            ' Obtener nombre antes de eliminar
            Dim nombreEmpleado As String = ""
            Try
                Dim queryNombre As String = "SELECT Nombre_Empleado FROM Empleado WHERE ID_Empleado = @ID_Empleado"
                Dim nombreObj As Object = DataAccess.ExecuteScalar(queryNombre, New SqlParameter() {New SqlParameter("@ID_Empleado", empleadoId)})
                If nombreObj IsNot Nothing Then nombreEmpleado = nombreObj.ToString()
            Catch
            End Try
            
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarEmpleado @ID_Empleado"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Empleado", empleadoId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                ' Registrar en bitácora
                Try
                    BitacoraHelper.RegistrarEliminar("Empleado", empleadoId.ToString(), "Nombre: " & nombreEmpleado)
                Catch
                End Try
                
                MostrarAlerta("Empleado eliminado correctamente", "success")
                CargarEmpleados()
            Else
                MostrarAlerta("Error al eliminar el empleado", "danger")
            End If
        Catch ex As Exception
            Try
                BitacoraHelper.RegistrarError(ex, "Empleado.aspx")
            Catch
            End Try
            MostrarAlerta("Error al eliminar el empleado: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarEmpleado(nombreEmpleado As String, fechaContratacion As Date) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Empleado), 0) + 1 FROM Empleado"
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
            Dim query As String = "EXEC InsertarEmpleado @ID_Empleado, @Nombre_Empleado, @Fecha_Contratacion"
            Dim parameters() As SqlParameter
            
            If fechaContratacion = Date.MinValue Then
                parameters = {
                    New SqlParameter("@ID_Empleado", nuevoId),
                    New SqlParameter("@Nombre_Empleado", nombreEmpleado),
                    New SqlParameter("@Fecha_Contratacion", DBNull.Value)
                }
            Else
                parameters = {
                    New SqlParameter("@ID_Empleado", nuevoId),
                    New SqlParameter("@Nombre_Empleado", nombreEmpleado),
                    New SqlParameter("@Fecha_Contratacion", fechaContratacion)
                }
            End If
            
            Dim resultado As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            
            ' Registrar en bitácora
            If resultado > 0 Then
                Try
                    BitacoraHelper.RegistrarCrear("Empleado", nuevoId.ToString(), "Nombre: " & nombreEmpleado)
                Catch
                End Try
            End If
            
            Return resultado
        Catch ex As Exception
            Try
                BitacoraHelper.RegistrarError(ex, "Empleado.aspx")
            Catch
            End Try
            Throw New Exception("Error al insertar el empleado: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarEmpleado(empleadoId As Integer, nombreEmpleado As String, fechaContratacion As Date) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarEmpleado @ID_Empleado, @Nombre_Empleado, @Fecha_Contratacion"
            Dim parameters() As SqlParameter
            
            If fechaContratacion = Date.MinValue Then
                parameters = {
                    New SqlParameter("@ID_Empleado", empleadoId),
                    New SqlParameter("@Nombre_Empleado", nombreEmpleado),
                    New SqlParameter("@Fecha_Contratacion", DBNull.Value)
                }
            Else
                parameters = {
                    New SqlParameter("@ID_Empleado", empleadoId),
                    New SqlParameter("@Nombre_Empleado", nombreEmpleado),
                    New SqlParameter("@Fecha_Contratacion", fechaContratacion)
                }
            End If
            
            Dim resultado As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            
            ' Registrar en bitácora
            If resultado > 0 Then
                Try
                    BitacoraHelper.RegistrarActualizar("Empleado", empleadoId.ToString(), "Nombre: " & nombreEmpleado)
                Catch
                End Try
            End If
            
            Return resultado
        Catch ex As Exception
            Try
                BitacoraHelper.RegistrarError(ex, "Empleado.aspx")
            Catch
            End Try
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreEmpleado.Text.Trim()) Then
            MostrarAlerta("El nombre del empleado es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreEmpleado.Text = ""
        txtFechaContratacion.Text = ""
        hfEmpleadoId.Value = "0"
        modalTitle.InnerText = "Nuevo Empleado"
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

