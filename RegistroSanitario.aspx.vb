Imports System.Data
Imports System.Data.SqlClient

Partial Public Class RegistroSanitario
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarAnimales()
            CargarRegistros()
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

    Private Sub CargarRegistros()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Registro_Sanitario'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Registro_Sanitario' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar JOIN para obtener el nombre del animal
            Dim query As String = "SELECT RS.ID_RegistroSan, RS.ID_Animal, A.Nombre_Animal, RS.Fecha_Registro, RS.Descripcion " & _
                                  "FROM Registro_Sanitario RS " & _
                                  "INNER JOIN Animal A ON RS.ID_Animal = A.ID_Animal " & _
                                  "ORDER BY RS.Fecha_Registro DESC"
            
            Dim registrosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvRegistros.DataSource = registrosData
            gvRegistros.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los registros sanitarios: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim registroId As Integer = Convert.ToInt32(hfRegistroIdEliminar.Value)
            EliminarRegistro(registroId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el registro sanitario: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim registroId As Integer = 0
                If Not String.IsNullOrEmpty(hfRegistroId.Value) AndAlso IsNumeric(hfRegistroId.Value) Then
                    registroId = Convert.ToInt32(hfRegistroId.Value)
                End If
                
                Dim animalId As Integer = Convert.ToInt32(ddlAnimal.SelectedValue)
                Dim fechaRegistro As Date = Convert.ToDateTime(txtFechaRegistro.Text)
                Dim descripcion As String = txtDescripcion.Text.Trim()

                Dim result As Integer

                If registroId = 0 Then
                    ' Insertar nuevo registro usando procedimiento almacenado
                    result = InsertarRegistro(animalId, fechaRegistro, descripcion)
                    If result > 0 Then
                        MostrarAlerta("Registro sanitario agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el registro sanitario", "danger")
                    End If
                Else
                    ' Actualizar registro existente usando procedimiento almacenado
                    result = ActualizarRegistro(registroId, animalId, fechaRegistro, descripcion)
                    If result > 0 Then
                        MostrarAlerta("Registro sanitario actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el registro sanitario", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarRegistros()
                    registroModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el registro sanitario: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvRegistros_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim registroId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarRegistro"
                    EditarRegistro(registroId)
                Case "EliminarRegistro"
                    EliminarRegistro(registroId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarRegistro(registroId As Integer)
        Try
            Dim query As String = "SELECT ID_RegistroSan, ID_Animal, Fecha_Registro, Descripcion FROM Registro_Sanitario WHERE ID_RegistroSan = @RegistroId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@RegistroId", registroId)
            }
            
            Dim registroData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If registroData.Rows.Count > 0 Then
                Dim row As DataRow = registroData.Rows(0)
                
                hfRegistroId.Value = registroId.ToString()
                
                ' Cargar animales si no están cargados
                If ddlAnimal.Items.Count <= 1 Then
                    CargarAnimales()
                End If
                ddlAnimal.SelectedValue = row("ID_Animal").ToString()
                
                ' Formatear fecha para input type="date"
                Dim fechaRegistro As DateTime = Convert.ToDateTime(row("Fecha_Registro"))
                txtFechaRegistro.Text = fechaRegistro.ToString("yyyy-MM-dd")
                
                ' Manejar valores NULL para Descripcion
                If Not row.IsNull("Descripcion") Then
                    txtDescripcion.Text = row("Descripcion").ToString()
                Else
                    txtDescripcion.Text = ""
                End If
                
                modalTitle.InnerText = "Editar Registro Sanitario"
                registroModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del registro sanitario: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarRegistro(registroId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarRegistroSanitario @ID_RegistroSan"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_RegistroSan", registroId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Registro sanitario eliminado correctamente", "success")
                CargarRegistros()
            Else
                MostrarAlerta("Error al eliminar el registro sanitario", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el registro sanitario: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarRegistro(animalId As Integer, fechaRegistro As Date, descripcion As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_RegistroSan), 0) + 1 FROM Registro_Sanitario"
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
            Dim query As String = "EXEC InsertarRegistroSanitario @ID_RegistroSan, @ID_Animal, @Fecha_Registro, @Descripcion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_RegistroSan", nuevoId),
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@Fecha_Registro", fechaRegistro),
                New SqlParameter("@Descripcion", If(String.IsNullOrEmpty(descripcion), CObj(DBNull.Value), descripcion))
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el registro sanitario: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarRegistro(registroId As Integer, animalId As Integer, fechaRegistro As Date, descripcion As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarRegistroSanitario @ID_RegistroSan, @ID_Animal, @Fecha_Registro, @Descripcion"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_RegistroSan", registroId),
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@Fecha_Registro", fechaRegistro),
                New SqlParameter("@Descripcion", If(String.IsNullOrEmpty(descripcion), CObj(DBNull.Value), descripcion))
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If ddlAnimal.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un animal", "warning")
            Return False
        End If

        If String.IsNullOrEmpty(txtFechaRegistro.Text) Then
            MostrarAlerta("La fecha de registro es requerida", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlAnimal.Items.Count > 0 Then
            ddlAnimal.SelectedIndex = 0
        End If
        txtFechaRegistro.Text = ""
        txtDescripcion.Text = ""
        hfRegistroId.Value = "0"
        modalTitle.InnerText = "Nuevo Registro Sanitario"
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

