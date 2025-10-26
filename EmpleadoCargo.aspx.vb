Imports System.Data
Imports System.Data.SqlClient

Partial Public Class EmpleadoCargo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarEmpleados()
            CargarCargos()
            CargarAsignaciones()
        End If
    End Sub

    Private Sub CargarEmpleados()
        Try
            Dim query As String = "SELECT ID_Empleado, Nombre_Empleado FROM Empleado ORDER BY Nombre_Empleado"
            Dim empleadosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlEmpleado.DataSource = empleadosData
            ddlEmpleado.DataTextField = "Nombre_Empleado"
            ddlEmpleado.DataValueField = "ID_Empleado"
            ddlEmpleado.DataBind()
            ddlEmpleado.Items.Insert(0, New ListItem("Seleccione un empleado", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los empleados: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarCargos()
        Try
            Dim query As String = "SELECT ID_Cargo, Nombre_Cargo FROM Cargo ORDER BY Nombre_Cargo"
            Dim cargosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlCargo.DataSource = cargosData
            ddlCargo.DataTextField = "Nombre_Cargo"
            ddlCargo.DataValueField = "ID_Cargo"
            ddlCargo.DataBind()
            ddlCargo.Items.Insert(0, New ListItem("Seleccione un cargo", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los cargos: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarAsignaciones()
        Try
            ' Verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Empleado_Cargo'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Empleado_Cargo' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Obtener asignaciones con JOINs para mostrar nombres
            Dim query As String = "SELECT EC.ID_Empleado, E.Nombre_Empleado, EC.ID_Cargo, C.Nombre_Cargo, EC.Fecha_Asignacion " & _
                                  "FROM Empleado_Cargo EC " & _
                                  "INNER JOIN Empleado E ON EC.ID_Empleado = E.ID_Empleado " & _
                                  "INNER JOIN Cargo C ON EC.ID_Cargo = C.ID_Cargo " & _
                                  "ORDER BY EC.Fecha_Asignacion DESC, E.Nombre_Empleado"
            
            Dim asignacionesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvAsignaciones.DataSource = asignacionesData
            gvAsignaciones.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar las asignaciones: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim empleadoId As Integer = Convert.ToInt32(hfEmpleadoIdEliminar.Value)
            Dim cargoId As Integer = Convert.ToInt32(hfCargoIdEliminar.Value)
            Dim fechaAsignacion As Date = Convert.ToDateTime(hfFechaAsignacionEliminar.Value)
            EliminarAsignacion(empleadoId, cargoId, fechaAsignacion)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la asignación: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim empleadoId As Integer = Convert.ToInt32(ddlEmpleado.SelectedValue)
                Dim cargoId As Integer = Convert.ToInt32(ddlCargo.SelectedValue)
                Dim fechaAsignacion As Date = Convert.ToDateTime(txtFechaAsignacion.Text)

                ' Verificar si ya existe la asignación
                If ExisteAsignacion(empleadoId, cargoId, fechaAsignacion) Then
                    MostrarAlerta("Esta asignación ya está registrada", "warning")
                    Return
                End If

                Dim result As Integer = InsertarAsignacion(empleadoId, cargoId, fechaAsignacion)
                
                If result > 0 Then
                    MostrarAlerta("Asignación registrada correctamente", "success")
                    CargarAsignaciones()
                    asignacionModal.Style("display") = "none"
                    LimpiarFormulario()
                Else
                    MostrarAlerta("Error al registrar la asignación", "danger")
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
                Case "EliminarAsignacion"
                    If args.Length = 3 Then
                        Dim empleadoId As Integer = Convert.ToInt32(args(0))
                        Dim cargoId As Integer = Convert.ToInt32(args(1))
                        Dim fechaAsignacion As Date = Convert.ToDateTime(args(2))
                        EliminarAsignacion(empleadoId, cargoId, fechaAsignacion)
                    End If
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarAsignacion(empleadoId As Integer, cargoId As Integer, fechaAsignacion As Date)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarEmpleadoCargo @ID_Empleado, @ID_Cargo, @Fecha_Asignacion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Empleado", empleadoId),
                New SqlParameter("@ID_Cargo", cargoId),
                New SqlParameter("@Fecha_Asignacion", fechaAsignacion)
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

    Private Function InsertarAsignacion(empleadoId As Integer, cargoId As Integer, fechaAsignacion As Date) As Integer
        Try
            ' Usar procedimiento almacenado para insertar
            Dim query As String = "EXEC InsertarEmpleadoCargo @ID_Empleado, @ID_Cargo, @Fecha_Asignacion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Empleado", empleadoId),
                New SqlParameter("@ID_Cargo", cargoId),
                New SqlParameter("@Fecha_Asignacion", fechaAsignacion)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar la asignación: " & ex.Message)
        End Try
    End Function


    Private Function ExisteAsignacion(empleadoId As Integer, cargoId As Integer, fechaAsignacion As Date) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM Empleado_Cargo WHERE ID_Empleado = @ID_Empleado AND ID_Cargo = @ID_Cargo AND Fecha_Asignacion = @Fecha_Asignacion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Empleado", empleadoId),
                New SqlParameter("@ID_Cargo", cargoId),
                New SqlParameter("@Fecha_Asignacion", fechaAsignacion)
            }
            
            Dim result As Object = DataAccess.ExecuteScalar(query, parameters)
            Return Convert.ToInt32(result) > 0
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If ddlEmpleado.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un empleado", "warning")
            Return False
        End If

        If ddlCargo.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un cargo", "warning")
            Return False
        End If

        If String.IsNullOrEmpty(txtFechaAsignacion.Text) Then
            MostrarAlerta("La fecha de asignación es requerida", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlEmpleado.Items.Count > 0 Then
            ddlEmpleado.SelectedIndex = 0
        End If
        If ddlCargo.Items.Count > 0 Then
            ddlCargo.SelectedIndex = 0
        End If
        txtFechaAsignacion.Text = ""
        modalTitle.InnerText = "Nueva Asignación de Cargo"
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

