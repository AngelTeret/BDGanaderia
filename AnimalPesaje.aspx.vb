Imports System.Data
Imports System.Data.SqlClient

Partial Public Class AnimalPesaje
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarAnimales()
            CargarPesajes()
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

    Private Sub CargarPesajes()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Animal_Pesaje'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Animal_Pesaje' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar JOIN para obtener el nombre del animal
            Dim query As String = "SELECT AP.ID_Pesaje, AP.ID_Animal, A.Nombre_Animal, AP.Fecha_Pesaje, AP.Peso " & _
                                  "FROM Animal_Pesaje AP " & _
                                  "INNER JOIN Animal A ON AP.ID_Animal = A.ID_Animal " & _
                                  "ORDER BY AP.Fecha_Pesaje DESC"
            
            Dim pesajesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvPesajes.DataSource = pesajesData
            gvPesajes.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los pesajes: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim pesajeId As Integer = Convert.ToInt32(hfPesajeIdEliminar.Value)
            EliminarPesaje(pesajeId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el pesaje: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim pesajeId As Integer = 0
                If Not String.IsNullOrEmpty(hfPesajeId.Value) AndAlso IsNumeric(hfPesajeId.Value) Then
                    pesajeId = Convert.ToInt32(hfPesajeId.Value)
                End If
                
                Dim animalId As Integer = Convert.ToInt32(ddlAnimal.SelectedValue)
                Dim fechaPesaje As Date = Convert.ToDateTime(txtFechaPesaje.Text)
                
                Dim peso As Decimal = 0
                If Not String.IsNullOrEmpty(txtPeso.Text) Then
                    peso = ParseDecimalFlexible(txtPeso.Text)
                End If

                Dim result As Integer

                If pesajeId = 0 Then
                    ' Insertar nuevo pesaje usando procedimiento almacenado
                    result = InsertarPesaje(animalId, fechaPesaje, peso)
                    If result > 0 Then
                        MostrarAlerta("Pesaje registrado correctamente", "success")
                    Else
                        MostrarAlerta("Error al registrar el pesaje", "danger")
                    End If
                Else
                    ' Actualizar pesaje existente usando procedimiento almacenado
                    result = ActualizarPesaje(pesajeId, animalId, fechaPesaje, peso)
                    If result > 0 Then
                        MostrarAlerta("Pesaje actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el pesaje", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarPesajes()
                    pesajeModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el pesaje: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvPesajes_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim pesajeId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarPesaje"
                    EditarPesaje(pesajeId)
                Case "EliminarPesaje"
                    EliminarPesaje(pesajeId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarPesaje(pesajeId As Integer)
        Try
            Dim query As String = "SELECT ID_Pesaje, ID_Animal, Fecha_Pesaje, Peso FROM Animal_Pesaje WHERE ID_Pesaje = @PesajeId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@PesajeId", pesajeId)
            }
            
            Dim pesajeData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If pesajeData.Rows.Count > 0 Then
                Dim row As DataRow = pesajeData.Rows(0)
                
                hfPesajeId.Value = pesajeId.ToString()
                
                ' Cargar animales si no están cargados
                If ddlAnimal.Items.Count <= 1 Then
                    CargarAnimales()
                End If
                ddlAnimal.SelectedValue = row("ID_Animal").ToString()
                
                ' Formatear fecha para input type="date"
                Dim fechaPesaje As DateTime = Convert.ToDateTime(row("Fecha_Pesaje"))
                txtFechaPesaje.Text = fechaPesaje.ToString("yyyy-MM-dd")
                
                ' Manejar valores NULL para Peso y formatear
                If Not row.IsNull("Peso") Then
                    Dim pesoValue As Decimal = Convert.ToDecimal(row("Peso"))
                    txtPeso.Text = pesoValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                Else
                    txtPeso.Text = "0.00"
                End If
                
                modalTitle.InnerText = "Editar Pesaje"
                pesajeModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del pesaje: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarPesaje(pesajeId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarAnimalPesaje @ID_Pesaje"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Pesaje", pesajeId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Pesaje eliminado correctamente", "success")
                CargarPesajes()
            Else
                MostrarAlerta("Error al eliminar el pesaje", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el pesaje: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarPesaje(animalId As Integer, fechaPesaje As Date, peso As Decimal) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Pesaje), 0) + 1 FROM Animal_Pesaje"
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
            Dim query As String = "EXEC InsertarAnimalPesaje @ID_Pesaje, @ID_Animal, @Fecha_Pesaje, @Peso"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Pesaje", nuevoId),
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@Fecha_Pesaje", fechaPesaje),
                New SqlParameter("@Peso", peso)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el pesaje: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarPesaje(pesajeId As Integer, animalId As Integer, fechaPesaje As Date, peso As Decimal) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarAnimalPesaje @ID_Pesaje, @ID_Animal, @Fecha_Pesaje, @Peso"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Pesaje", pesajeId),
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@Fecha_Pesaje", fechaPesaje),
                New SqlParameter("@Peso", peso)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ParseDecimalFlexible(input As String) As Decimal
        If String.IsNullOrWhiteSpace(input) Then Throw New FormatException("Vacío")
        Dim s = input.Trim().Replace(" ", "")
        Dim tienePunto As Boolean = s.Contains(".")
        Dim tieneComa As Boolean = s.Contains(",")
        
        If tienePunto AndAlso tieneComa Then
            If s.LastIndexOf(".") > s.LastIndexOf(",") Then
                s = s.Replace(",", "")
            Else
                s = s.Replace(".", "")
                s = s.Replace(",", ".")
            End If
        ElseIf tieneComa Then
            s = s.Replace(".", "")
            s = s.Replace(",", ".")
        ElseIf tienePunto Then
            s = s.Replace(",", "")
        End If

        Dim value As Decimal
        If Decimal.TryParse(s, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, value) Then
            Return value
        End If
        Throw New FormatException("No se pudo convertir '" & input & "' a decimal")
    End Function

    Private Function ValidarFormulario() As Boolean
        If ddlAnimal.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un animal", "warning")
            Return False
        End If

        If String.IsNullOrEmpty(txtFechaPesaje.Text) Then
            MostrarAlerta("La fecha del pesaje es requerida", "warning")
            Return False
        End If

        If String.IsNullOrEmpty(txtPeso.Text) Then
            MostrarAlerta("El peso es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlAnimal.Items.Count > 0 Then
            ddlAnimal.SelectedIndex = 0
        End If
        txtFechaPesaje.Text = ""
        txtPeso.Text = ""
        hfPesajeId.Value = "0"
        modalTitle.InnerText = "Nuevo Pesaje"
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

