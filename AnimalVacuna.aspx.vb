Imports System.Data
Imports System.Data.SqlClient

Partial Public Class AnimalVacuna
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarAnimales()
            CargarVacunas()
            CargarVacunaciones()
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

    Private Sub CargarVacunas()
        Try
            Dim query As String = "SELECT ID_Vacuna, Nombre_Vacuna FROM Vacuna ORDER BY Nombre_Vacuna"
            Dim vacunasData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlVacuna.DataSource = vacunasData
            ddlVacuna.DataTextField = "Nombre_Vacuna"
            ddlVacuna.DataValueField = "ID_Vacuna"
            ddlVacuna.DataBind()
            ddlVacuna.Items.Insert(0, New ListItem("Seleccione una vacuna", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar las vacunas: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarVacunaciones()
        Try
            ' Verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Animal_Vacuna'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Animal_Vacuna' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Obtener vacunaciones con JOINs para mostrar nombres
            Dim query As String = "SELECT AV.ID_Animal, A.Nombre_Animal, AV.ID_Vacuna, V.Nombre_Vacuna, AV.Fecha_Aplicacion " & _
                                  "FROM Animal_Vacuna AV " & _
                                  "INNER JOIN Animal A ON AV.ID_Animal = A.ID_Animal " & _
                                  "INNER JOIN Vacuna V ON AV.ID_Vacuna = V.ID_Vacuna " & _
                                  "ORDER BY AV.Fecha_Aplicacion DESC, A.Nombre_Animal"
            
            Dim vacunacionesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvVacunaciones.DataSource = vacunacionesData
            gvVacunaciones.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar las vacunaciones: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim animalId As Integer = Convert.ToInt32(hfAnimalIdEliminar.Value)
            Dim vacunaId As Integer = Convert.ToInt32(hfVacunaIdEliminar.Value)
            Dim fechaAplicacion As Date = Convert.ToDateTime(hfFechaAplicacionEliminar.Value)
            EliminarVacunacion(animalId, vacunaId, fechaAplicacion)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la vacunación: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim animalId As Integer = Convert.ToInt32(ddlAnimal.SelectedValue)
                Dim vacunaId As Integer = Convert.ToInt32(ddlVacuna.SelectedValue)
                Dim fechaAplicacion As Date = Convert.ToDateTime(txtFechaAplicacion.Text)

                ' Verificar si ya existe la vacunación (misma combinación)
                If ExisteVacunacion(animalId, vacunaId, fechaAplicacion) Then
                    MostrarAlerta("Esta vacuna ya está registrada para este animal en la fecha seleccionada", "warning")
                    Return
                End If

                Dim result As Integer = InsertarVacunacion(animalId, vacunaId, fechaAplicacion)
                
                If result > 0 Then
                    MostrarAlerta("Vacunación registrada correctamente", "success")
                    CargarVacunaciones()
                    vacunacionModal.Style("display") = "none"
                    LimpiarFormulario()
                Else
                    MostrarAlerta("Error al registrar la vacunación", "danger")
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar la vacunación: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvVacunaciones_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim args As String() = e.CommandArgument.ToString().Split(",")
            
            Select Case e.CommandName
                Case "EliminarVacunacion"
                    If args.Length = 3 Then
                        Dim animalId As Integer = Convert.ToInt32(args(0))
                        Dim vacunaId As Integer = Convert.ToInt32(args(1))
                        Dim fechaAplicacion As Date = Convert.ToDateTime(args(2))
                        EliminarVacunacion(animalId, vacunaId, fechaAplicacion)
                    End If
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarVacunacion(animalId As Integer, vacunaId As Integer, fechaAplicacion As Date)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarAnimalVacuna @ID_Animal, @ID_Vacuna, @Fecha_Aplicacion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Vacuna", vacunaId),
                New SqlParameter("@Fecha_Aplicacion", fechaAplicacion)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Vacunación eliminada correctamente", "success")
                CargarVacunaciones()
            Else
                MostrarAlerta("Error al eliminar la vacunación", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la vacunación: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarVacunacion(animalId As Integer, vacunaId As Integer, fechaAplicacion As Date) As Integer
        Try
            ' Usar procedimiento almacenado para insertar
            Dim query As String = "EXEC InsertarAnimalVacuna @ID_Animal, @ID_Vacuna, @Fecha_Aplicacion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Vacuna", vacunaId),
                New SqlParameter("@Fecha_Aplicacion", fechaAplicacion)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar la vacunación: " & ex.Message)
        End Try
    End Function

    Private Function ExisteVacunacion(animalId As Integer, vacunaId As Integer, fechaAplicacion As Date) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM Animal_Vacuna WHERE ID_Animal = @ID_Animal AND ID_Vacuna = @ID_Vacuna AND Fecha_Aplicacion = @Fecha_Aplicacion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Vacuna", vacunaId),
                New SqlParameter("@Fecha_Aplicacion", fechaAplicacion)
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

        If ddlVacuna.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar una vacuna", "warning")
            Return False
        End If

        If String.IsNullOrEmpty(txtFechaAplicacion.Text) Then
            MostrarAlerta("La fecha de aplicación es requerida", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlAnimal.Items.Count > 0 Then
            ddlAnimal.SelectedIndex = 0
        End If
        If ddlVacuna.Items.Count > 0 Then
            ddlVacuna.SelectedIndex = 0
        End If
        txtFechaAplicacion.Text = ""
        modalTitle.InnerText = "Nueva Vacunación"
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

