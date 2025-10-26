Imports System.Data
Imports System.Data.SqlClient

Partial Public Class AnimalControlLechero
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarAnimales()
            CargarControles()
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

    Private Sub CargarControles()
        Try
            Dim query As String = "SELECT ID_Control, CONVERT(VARCHAR, Fecha_Control, 103) AS Fecha_Control_Formato FROM Control_Lechero ORDER BY Fecha_Control DESC"
            Dim controlesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlControl.DataSource = controlesData
            ddlControl.DataTextField = "Fecha_Control_Formato"
            ddlControl.DataValueField = "ID_Control"
            ddlControl.DataBind()
            ddlControl.Items.Insert(0, New ListItem("Seleccione un control", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los controles: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarRegistros()
        Try
            ' Verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Animal_ControlLechero'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Animal_ControlLechero' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Obtener registros con JOINs para mostrar nombres
            Dim query As String = "SELECT ACL.ID_Animal, A.Nombre_Animal, ACL.ID_Control, CL.Fecha_Control, ACL.Litros_Leche " & _
                                  "FROM Animal_ControlLechero ACL " & _
                                  "INNER JOIN Animal A ON ACL.ID_Animal = A.ID_Animal " & _
                                  "INNER JOIN Control_Lechero CL ON ACL.ID_Control = CL.ID_Control " & _
                                  "ORDER BY CL.Fecha_Control DESC, A.Nombre_Animal"
            
            Dim registrosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvRegistros.DataSource = registrosData
            gvRegistros.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los registros: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim animalId As Integer = Convert.ToInt32(hfAnimalIdEliminar.Value)
            Dim controlId As Integer = Convert.ToInt32(hfControlIdEliminar.Value)
            EliminarRegistro(animalId, controlId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el registro: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim animalId As Integer = Convert.ToInt32(ddlAnimal.SelectedValue)
                Dim controlId As Integer = Convert.ToInt32(ddlControl.SelectedValue)
                ' Usar cultura invariante para parsear correctamente decimales con punto
                Dim culture As System.Globalization.CultureInfo = System.Globalization.CultureInfo.InvariantCulture
                Dim litrosLeche As Decimal = Decimal.Parse(txtLitrosLeche.Text, culture)

                ' Verificar si estamos editando o creando
                Dim editando As Boolean = Not String.IsNullOrEmpty(hfAnimalId.Value) AndAlso Not String.IsNullOrEmpty(hfControlId.Value)
                
                Dim result As Integer

                If Not editando Then
                    ' Insertar nuevo registro
                    If ExisteRegistro(animalId, controlId) Then
                        MostrarAlerta("Ya existe un registro de control lechero para este animal en esta fecha", "warning")
                        Return
                    End If

                    result = InsertarRegistro(animalId, controlId, litrosLeche)
                    
                    If result > 0 Then
                        MostrarAlerta("Registro de control lechero guardado correctamente", "success")
                    Else
                        MostrarAlerta("Error al guardar el registro", "danger")
                    End If
                Else
                    ' Actualizar registro existente
                    result = ActualizarRegistro(animalId, controlId, litrosLeche)
                    
                    If result > 0 Then
                        MostrarAlerta("Litros de leche actualizados correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el registro", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarRegistros()
                    registroModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el registro: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvRegistros_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim args As String() = e.CommandArgument.ToString().Split(",")
            
            Select Case e.CommandName
                Case "EditarRegistro"
                    If args.Length = 2 Then
                        Dim animalId As Integer = Convert.ToInt32(args(0))
                        Dim controlId As Integer = Convert.ToInt32(args(1))
                        EditarRegistro(animalId, controlId)
                    End If
                Case "EliminarRegistro"
                    If args.Length = 2 Then
                        Dim animalId As Integer = Convert.ToInt32(args(0))
                        Dim controlId As Integer = Convert.ToInt32(args(1))
                        EliminarRegistro(animalId, controlId)
                    End If
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarRegistro(animalId As Integer, controlId As Integer)
        Try
            Dim query As String = "SELECT ID_Animal, ID_Control, Litros_Leche FROM Animal_ControlLechero WHERE ID_Animal = @ID_Animal AND ID_Control = @ID_Control"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Control", controlId)
            }
            
            Dim registroData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If registroData.Rows.Count > 0 Then
                Dim row As DataRow = registroData.Rows(0)
                
                hfAnimalId.Value = animalId.ToString()
                hfControlId.Value = controlId.ToString()
                
                ' Cargar y seleccionar Animal (no permitir cambiar)
                If ddlAnimal.Items.Count <= 1 Then
                    CargarAnimales()
                End If
                ddlAnimal.SelectedValue = row("ID_Animal").ToString()
                ddlAnimal.Enabled = False
                
                ' Cargar y seleccionar Control (no permitir cambiar)
                If ddlControl.Items.Count <= 1 Then
                    CargarControles()
                End If
                ddlControl.SelectedValue = row("ID_Control").ToString()
                ddlControl.Enabled = False
                
                ' Mostrar litros de leche (campo editable)
                txtLitrosLeche.Text = Convert.ToDecimal(row("Litros_Leche")).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                
                modalTitle.InnerText = "Editar Litros de Leche"
                registroModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del registro: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarRegistro(animalId As Integer, controlId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarAnimalControlLechero @ID_Animal, @ID_Control"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Control", controlId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Registro eliminado correctamente", "success")
                CargarRegistros()
            Else
                MostrarAlerta("Error al eliminar el registro", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el registro: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarRegistro(animalId As Integer, controlId As Integer, litrosLeche As Decimal) As Integer
        Try
            ' Usar procedimiento almacenado para insertar
            Dim query As String = "EXEC InsertarAnimalControlLechero @ID_Animal, @ID_Control, @Litros_Leche"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Control", controlId),
                New SqlParameter("@Litros_Leche", litrosLeche)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el registro: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarRegistro(animalId As Integer, controlId As Integer, litrosLeche As Decimal) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarAnimalControlLechero @ID_Animal, @ID_Control, @Litros_Leche"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Control", controlId),
                New SqlParameter("@Litros_Leche", litrosLeche)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al actualizar el registro: " & ex.Message)
        End Try
    End Function

    Private Function ExisteRegistro(animalId As Integer, controlId As Integer) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM Animal_ControlLechero WHERE ID_Animal = @ID_Animal AND ID_Control = @ID_Control"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Control", controlId)
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

        If ddlControl.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un control", "warning")
            Return False
        End If

        If String.IsNullOrEmpty(txtLitrosLeche.Text) Then
            MostrarAlerta("Debe ingresar los litros de leche", "warning")
            Return False
        End If

        Dim litros As Decimal
        ' Usar cultura invariante para parsear correctamente decimales con punto
        Dim culture As System.Globalization.CultureInfo = System.Globalization.CultureInfo.InvariantCulture
        If Not Decimal.TryParse(txtLitrosLeche.Text, System.Globalization.NumberStyles.Number, culture, litros) OrElse litros <= 0 Then
            MostrarAlerta("Los litros de leche deben ser un número positivo (ej: 2.50)", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlAnimal.Items.Count > 0 Then
            ddlAnimal.SelectedIndex = 0
            ddlAnimal.Enabled = True
        End If
        If ddlControl.Items.Count > 0 Then
            ddlControl.SelectedIndex = 0
            ddlControl.Enabled = True
        End If
        txtLitrosLeche.Text = ""
        hfAnimalId.Value = ""
        hfControlId.Value = ""
        modalTitle.InnerText = "Nuevo Registro de Control Lechero"
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

