Imports System.Data
Imports System.Data.SqlClient

Partial Public Class AnimalRacion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarAnimales()
            CargarRaciones()
            CargarAsignaciones()
        End If
    End Sub

    Private Sub CargarAnimales()
        Try
            Dim query As String = "SELECT ID_Animal, Nombre_Animal FROM Animal ORDER BY Nombre_Animal"
            Dim animalesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlAnimal.DataSource = animalesData
            ddlAnimal.DataTextField = "Nombre_Animal"
            ddlAnimal.DataValueField = "ID_Animal"
            ddlAnimal.DataBind()
            ddlAnimal.Items.Insert(0, New ListItem("Seleccione un animal", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los animales: " & ex.Message, "danger")
        End Try
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

    Private Sub CargarAsignaciones()
        Try
            ' Verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Animal_Racion'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Animal_Racion' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Obtener asignaciones con JOINs para mostrar nombres
            Dim query As String = "SELECT AR.ID_Animal, A.Nombre_Animal, AR.ID_Racion, R.Nombre_Racion, AR.Fecha_Asignacion " & _
                                  "FROM Animal_Racion AR " & _
                                  "INNER JOIN Animal A ON AR.ID_Animal = A.ID_Animal " & _
                                  "INNER JOIN Racion R ON AR.ID_Racion = R.ID_Racion " & _
                                  "ORDER BY AR.Fecha_Asignacion DESC, A.Nombre_Animal"
            
            Dim asignacionesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvAsignaciones.DataSource = asignacionesData
            gvAsignaciones.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar las asignaciones: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim animalId As Integer = Convert.ToInt32(hfAnimalIdEliminar.Value)
            Dim racionId As Integer = Convert.ToInt32(hfRacionIdEliminar.Value)
            Dim fechaAsignacion As Date = Convert.ToDateTime(hfFechaAsignacionEliminar.Value)
            EliminarAsignacion(animalId, racionId, fechaAsignacion)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la asignación: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim animalId As Integer = Convert.ToInt32(ddlAnimal.SelectedValue)
                Dim racionId As Integer = Convert.ToInt32(ddlRacion.SelectedValue)
                Dim fechaAsignacion As Date = Convert.ToDateTime(txtFechaAsignacion.Text)

                ' Verificar si ya existe la asignación (misma combinación)
                If ExisteAsignacion(animalId, racionId, fechaAsignacion) Then
                    MostrarAlerta("Esta ración ya está asignada a este animal en la fecha seleccionada", "warning")
                    Return
                End If

                Dim result As Integer = InsertarAsignacion(animalId, racionId, fechaAsignacion)
                
                If result > 0 Then
                    MostrarAlerta("Ración asignada correctamente", "success")
                    CargarAsignaciones()
                    asignacionModal.Style("display") = "none"
                    LimpiarFormulario()
                Else
                    MostrarAlerta("Error al asignar la ración", "danger")
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
                        Dim animalId As Integer = Convert.ToInt32(args(0))
                        Dim racionId As Integer = Convert.ToInt32(args(1))
                        Dim fechaAsignacion As Date = Convert.ToDateTime(args(2))
                        EliminarAsignacion(animalId, racionId, fechaAsignacion)
                    End If
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarAsignacion(animalId As Integer, racionId As Integer, fechaAsignacion As Date)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarAnimalRacion @ID_Animal, @ID_Racion, @Fecha_Asignacion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Racion", racionId),
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

    Private Function InsertarAsignacion(animalId As Integer, racionId As Integer, fechaAsignacion As Date) As Integer
        Try
            ' Usar procedimiento almacenado para insertar
            Dim query As String = "EXEC InsertarAnimalRacion @ID_Animal, @ID_Racion, @Fecha_Asignacion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Racion", racionId),
                New SqlParameter("@Fecha_Asignacion", fechaAsignacion)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar la asignación: " & ex.Message)
        End Try
    End Function

    Private Function ExisteAsignacion(animalId As Integer, racionId As Integer, fechaAsignacion As Date) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM Animal_Racion WHERE ID_Animal = @ID_Animal AND ID_Racion = @ID_Racion AND Fecha_Asignacion = @Fecha_Asignacion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Racion", racionId),
                New SqlParameter("@Fecha_Asignacion", fechaAsignacion)
            }
            
            Dim result As Object = DataAccess.ExecuteScalar(query, parameters)
            Return Convert.ToInt32(result) > 0
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If ddlAnimal.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un animal", "warning")
            Return False
        End If

        If ddlRacion.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar una ración", "warning")
            Return False
        End If

        If String.IsNullOrEmpty(txtFechaAsignacion.Text) Then
            MostrarAlerta("La fecha de asignación es requerida", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlAnimal.Items.Count > 0 Then
            ddlAnimal.SelectedIndex = 0
        End If
        If ddlRacion.Items.Count > 0 Then
            ddlRacion.SelectedIndex = 0
        End If
        txtFechaAsignacion.Text = ""
        modalTitle.InnerText = "Nueva Asignación de Ración"
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

