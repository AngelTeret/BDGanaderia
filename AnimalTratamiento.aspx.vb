Imports System.Data
Imports System.Data.SqlClient

Partial Public Class AnimalTratamiento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarAnimales()
            CargarTratamientos()
            CargarVeterinarios()
            CargarTratamientosRegistrados()
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

    Private Sub CargarTratamientos()
        Try
            Dim query As String = "SELECT ID_Tratamiento, Nombre_Tratamiento FROM Tratamiento ORDER BY Nombre_Tratamiento"
            Dim tratamientosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlTratamiento.DataSource = tratamientosData
            ddlTratamiento.DataTextField = "Nombre_Tratamiento"
            ddlTratamiento.DataValueField = "ID_Tratamiento"
            ddlTratamiento.DataBind()
            ddlTratamiento.Items.Insert(0, New ListItem("Seleccione un tratamiento", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los tratamientos: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarVeterinarios()
        Try
            Dim query As String = "SELECT ID_Veterinario, Nombre_Veterinario FROM Veterinario ORDER BY Nombre_Veterinario"
            Dim veterinariosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlVeterinario.DataSource = veterinariosData
            ddlVeterinario.DataTextField = "Nombre_Veterinario"
            ddlVeterinario.DataValueField = "ID_Veterinario"
            ddlVeterinario.DataBind()
            ddlVeterinario.Items.Insert(0, New ListItem("Seleccione un veterinario", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los veterinarios: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarTratamientosRegistrados()
        Try
            ' Verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Animal_Tratamiento'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Animal_Tratamiento' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Obtener tratamientos con JOINs para mostrar nombres
            Dim query As String = "SELECT AT.ID_Animal, A.Nombre_Animal, AT.ID_Tratamiento, T.Nombre_Tratamiento, " & _
                                  "AT.ID_Veterinario, V.Nombre_Veterinario, AT.Fecha_Tratamiento, AT.Observacion " & _
                                  "FROM Animal_Tratamiento AT " & _
                                  "INNER JOIN Animal A ON AT.ID_Animal = A.ID_Animal " & _
                                  "INNER JOIN Tratamiento T ON AT.ID_Tratamiento = T.ID_Tratamiento " & _
                                  "INNER JOIN Veterinario V ON AT.ID_Veterinario = V.ID_Veterinario " & _
                                  "ORDER BY AT.Fecha_Tratamiento DESC, A.Nombre_Animal"
            
            Dim tratamientosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvTratamientos.DataSource = tratamientosData
            gvTratamientos.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los tratamientos: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim animalId As Integer = Convert.ToInt32(hfAnimalIdEliminar.Value)
            Dim tratamientoId As Integer = Convert.ToInt32(hfTratamientoIdEliminar.Value)
            Dim fechaTratamiento As Date = Convert.ToDateTime(hfFechaTratamientoEliminar.Value)
            EliminarTratamiento(animalId, tratamientoId, fechaTratamiento)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el tratamiento: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim animalId As Integer = Convert.ToInt32(ddlAnimal.SelectedValue)
                Dim tratamientoId As Integer = Convert.ToInt32(ddlTratamiento.SelectedValue)
                Dim veterinarioId As Integer = Convert.ToInt32(ddlVeterinario.SelectedValue)
                Dim fechaTratamiento As Date = Convert.ToDateTime(txtFechaTratamiento.Text)
                Dim observacion As String = txtObservacion.Text.Trim()

                ' Determinar si es insertar o actualizar
                Dim editando As Boolean = Not String.IsNullOrEmpty(hfAnimalId.Value) AndAlso Not String.IsNullOrEmpty(hfTratamientoId.Value) AndAlso Not String.IsNullOrEmpty(hfFechaTratamiento.Value)
                
                Dim result As Integer

                If Not editando Then
                    ' Insertar nuevo tratamiento
                    If ExisteTratamiento(animalId, tratamientoId, fechaTratamiento) Then
                        MostrarAlerta("Este tratamiento ya está registrado para este animal en la fecha seleccionada", "warning")
                        Return
                    End If

                    result = InsertarTratamiento(animalId, tratamientoId, veterinarioId, fechaTratamiento, observacion)
                    
                    If result > 0 Then
                        MostrarAlerta("Tratamiento registrado correctamente", "success")
                    Else
                        MostrarAlerta("Error al registrar el tratamiento", "danger")
                    End If
                Else
                    ' Actualizar tratamiento existente
                    animalId = Convert.ToInt32(hfAnimalId.Value)
                    tratamientoId = Convert.ToInt32(hfTratamientoId.Value)
                    Dim fechaOriginal As Date = Convert.ToDateTime(hfFechaTratamiento.Value)
                    
                    result = ActualizarTratamiento(animalId, tratamientoId, fechaOriginal, veterinarioId, observacion)
                    
                    If result > 0 Then
                        MostrarAlerta("Tratamiento actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el tratamiento", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarTratamientosRegistrados()
                    tratamientoModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el tratamiento: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvTratamientos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim args As String() = e.CommandArgument.ToString().Split(",")
            
            Select Case e.CommandName
                Case "EditarTratamiento"
                    If args.Length = 3 Then
                        Dim animalId As Integer = Convert.ToInt32(args(0))
                        Dim tratamientoId As Integer = Convert.ToInt32(args(1))
                        Dim fechaTratamiento As Date = Convert.ToDateTime(args(2))
                        EditarTratamiento(animalId, tratamientoId, fechaTratamiento)
                    End If
                Case "EliminarTratamiento"
                    If args.Length = 3 Then
                        Dim animalId As Integer = Convert.ToInt32(args(0))
                        Dim tratamientoId As Integer = Convert.ToInt32(args(1))
                        Dim fechaTratamiento As Date = Convert.ToDateTime(args(2))
                        EliminarTratamiento(animalId, tratamientoId, fechaTratamiento)
                    End If
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarTratamiento(animalId As Integer, tratamientoId As Integer, fechaTratamiento As Date)
        Try
            Dim query As String = "SELECT ID_Animal, ID_Tratamiento, ID_Veterinario, Fecha_Tratamiento, Observacion FROM Animal_Tratamiento WHERE ID_Animal = @ID_Animal AND ID_Tratamiento = @ID_Tratamiento AND Fecha_Tratamiento = @Fecha_Tratamiento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Tratamiento", tratamientoId),
                New SqlParameter("@Fecha_Tratamiento", fechaTratamiento)
            }
            
            Dim tratamientoData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If tratamientoData.Rows.Count > 0 Then
                Dim row As DataRow = tratamientoData.Rows(0)
                
                hfAnimalId.Value = animalId.ToString()
                hfTratamientoId.Value = tratamientoId.ToString()
                hfFechaTratamiento.Value = fechaTratamiento.ToString("yyyy-MM-dd")
                
                ' Cargar y seleccionar Animal
                If ddlAnimal.Items.Count <= 1 Then
                    CargarAnimales()
                End If
                ddlAnimal.SelectedValue = row("ID_Animal").ToString()
                ' Permitir cambiar el animal
                
                ' Cargar y seleccionar Tratamiento
                If ddlTratamiento.Items.Count <= 1 Then
                    CargarTratamientos()
                End If
                ddlTratamiento.SelectedValue = row("ID_Tratamiento").ToString()
                ' Permitir cambiar el tratamiento
                
                ' Cargar y seleccionar Veterinario
                If ddlVeterinario.Items.Count <= 1 Then
                    CargarVeterinarios()
                End If
                ddlVeterinario.SelectedValue = row("ID_Veterinario").ToString()
                
                ' Mostrar fecha (editable para permitir cambiar fecha)
                txtFechaTratamiento.Text = fechaTratamiento.ToString("yyyy-MM-dd")
                ' Permitir cambiar la fecha
                
                ' Mostrar observación
                If Not row.IsNull("Observacion") Then
                    txtObservacion.Text = row("Observacion").ToString()
                Else
                    txtObservacion.Text = ""
                End If
                
                modalTitle.InnerText = "Editar Tratamiento"
                tratamientoModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del tratamiento: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarTratamiento(animalId As Integer, tratamientoId As Integer, fechaTratamiento As Date)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarAnimalTratamiento @ID_Animal, @ID_Tratamiento, @Fecha_Tratamiento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Tratamiento", tratamientoId),
                New SqlParameter("@Fecha_Tratamiento", fechaTratamiento)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Tratamiento eliminado correctamente", "success")
                CargarTratamientosRegistrados()
            Else
                MostrarAlerta("Error al eliminar el tratamiento", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el tratamiento: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarTratamiento(animalId As Integer, tratamientoId As Integer, veterinarioId As Integer, fechaTratamiento As Date, observacion As String) As Integer
        Try
            ' Usar procedimiento almacenado para insertar
            Dim query As String = "EXEC InsertarAnimalTratamiento @ID_Animal, @ID_Tratamiento, @ID_Veterinario, @Fecha_Tratamiento, @Observacion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Tratamiento", tratamientoId),
                New SqlParameter("@ID_Veterinario", veterinarioId),
                New SqlParameter("@Fecha_Tratamiento", fechaTratamiento),
                New SqlParameter("@Observacion", If(String.IsNullOrWhiteSpace(observacion), DBNull.Value, observacion))
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el tratamiento: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarTratamiento(animalId As Integer, tratamientoId As Integer, fechaTratamientoOriginal As Date, veterinarioId As Integer, observacion As String) As Integer
        Try
            ' Obtener la nueva fecha del formulario
            Dim nuevaFecha As Date = Convert.ToDateTime(txtFechaTratamiento.Text)
            
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarAnimalTratamiento @ID_Animal, @ID_Tratamiento, @Fecha_Tratamiento, @ID_Veterinario, @Observacion, @Nueva_Fecha_Tratamiento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Tratamiento", tratamientoId),
                New SqlParameter("@Fecha_Tratamiento", fechaTratamientoOriginal),
                New SqlParameter("@ID_Veterinario", veterinarioId),
                New SqlParameter("@Observacion", If(String.IsNullOrWhiteSpace(observacion), DBNull.Value, observacion)),
                New SqlParameter("@Nueva_Fecha_Tratamiento", nuevaFecha)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al actualizar el tratamiento: " & ex.Message)
        End Try
    End Function

    Private Function ExisteTratamiento(animalId As Integer, tratamientoId As Integer, fechaTratamiento As Date) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM Animal_Tratamiento WHERE ID_Animal = @ID_Animal AND ID_Tratamiento = @ID_Tratamiento AND Fecha_Tratamiento = @Fecha_Tratamiento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Tratamiento", tratamientoId),
                New SqlParameter("@Fecha_Tratamiento", fechaTratamiento)
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

        If ddlTratamiento.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un tratamiento", "warning")
            Return False
        End If

        If ddlVeterinario.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un veterinario", "warning")
            Return False
        End If

        If String.IsNullOrEmpty(txtFechaTratamiento.Text) Then
            MostrarAlerta("La fecha de tratamiento es requerida", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlAnimal.Items.Count > 0 Then
            ddlAnimal.SelectedIndex = 0
            ddlAnimal.Enabled = True
        End If
        If ddlTratamiento.Items.Count > 0 Then
            ddlTratamiento.SelectedIndex = 0
            ddlTratamiento.Enabled = True
        End If
        If ddlVeterinario.Items.Count > 0 Then
            ddlVeterinario.SelectedIndex = 0
        End If
        txtFechaTratamiento.Text = ""
        txtFechaTratamiento.Enabled = True
        txtObservacion.Text = ""
        hfAnimalId.Value = ""
        hfTratamientoId.Value = ""
        hfFechaTratamiento.Value = ""
        modalTitle.InnerText = "Nuevo Tratamiento"
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

