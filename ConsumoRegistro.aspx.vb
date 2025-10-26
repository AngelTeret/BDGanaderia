Imports System.Data
Imports System.Data.SqlClient

Partial Public Class ConsumoRegistro
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarAnimales()
            CargarAlimentos()
            CargarConsumos()
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

    Private Sub CargarAlimentos()
        Try
            Dim query As String = "SELECT ID_Alimento, Nombre_Alimento FROM Alimento ORDER BY Nombre_Alimento"
            Dim alimentosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlAlimento.DataSource = alimentosData
            ddlAlimento.DataTextField = "Nombre_Alimento"
            ddlAlimento.DataValueField = "ID_Alimento"
            ddlAlimento.DataBind()
            ddlAlimento.Items.Insert(0, New ListItem("Seleccione un alimento", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los alimentos: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarConsumos()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Consumo_Registro'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Consumo_Registro' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar JOIN para obtener los nombres del animal y del alimento
            Dim query As String = "SELECT CR.ID_Consumo, CR.ID_Animal, CR.ID_Alimento, A.Nombre_Animal, AL.Nombre_Alimento, CR.Fecha_Consumo, CR.Cantidad " & _
                                  "FROM Consumo_Registro CR " & _
                                  "INNER JOIN Animal A ON CR.ID_Animal = A.ID_Animal " & _
                                  "INNER JOIN Alimento AL ON CR.ID_Alimento = AL.ID_Alimento " & _
                                  "ORDER BY CR.Fecha_Consumo DESC"
            
            Dim consumosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvConsumos.DataSource = consumosData
            gvConsumos.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los registros de consumo: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim consumoId As Integer = Convert.ToInt32(hfConsumoIdEliminar.Value)
            EliminarConsumo(consumoId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el registro de consumo: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim consumoId As Integer = 0
                If Not String.IsNullOrEmpty(hfConsumoId.Value) AndAlso IsNumeric(hfConsumoId.Value) Then
                    consumoId = Convert.ToInt32(hfConsumoId.Value)
                End If
                
                Dim animalId As Integer = Convert.ToInt32(ddlAnimal.SelectedValue)
                Dim alimentoId As Integer = Convert.ToInt32(ddlAlimento.SelectedValue)
                Dim fechaConsumo As Date = Convert.ToDateTime(txtFechaConsumo.Text)
                
                Dim cantidad As Decimal = 0
                If Not String.IsNullOrEmpty(txtCantidad.Text) Then
                    cantidad = ParseDecimalFlexible(txtCantidad.Text)
                End If

                Dim result As Integer

                If consumoId = 0 Then
                    ' Insertar nuevo registro usando procedimiento almacenado
                    result = InsertarConsumo(animalId, alimentoId, fechaConsumo, cantidad)
                    If result > 0 Then
                        MostrarAlerta("Registro de consumo agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el registro de consumo", "danger")
                    End If
                Else
                    ' Actualizar registro existente usando procedimiento almacenado
                    result = ActualizarConsumo(consumoId, animalId, alimentoId, fechaConsumo, cantidad)
                    If result > 0 Then
                        MostrarAlerta("Registro de consumo actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el registro de consumo", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarConsumos()
                    consumoModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el registro de consumo: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvConsumos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim consumoId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarConsumo"
                    EditarConsumo(consumoId)
                Case "EliminarConsumo"
                    EliminarConsumo(consumoId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarConsumo(consumoId As Integer)
        Try
            Dim query As String = "SELECT ID_Consumo, ID_Animal, ID_Alimento, Fecha_Consumo, Cantidad FROM Consumo_Registro WHERE ID_Consumo = @ConsumoId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ConsumoId", consumoId)
            }
            
            Dim consumoData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If consumoData.Rows.Count > 0 Then
                Dim row As DataRow = consumoData.Rows(0)
                
                hfConsumoId.Value = consumoId.ToString()
                
                ' Cargar animales y alimentos si no están cargados
                If ddlAnimal.Items.Count <= 1 Then
                    CargarAnimales()
                End If
                If ddlAlimento.Items.Count <= 1 Then
                    CargarAlimentos()
                End If
                
                ddlAnimal.SelectedValue = row("ID_Animal").ToString()
                ddlAlimento.SelectedValue = row("ID_Alimento").ToString()
                
                ' Formatear fecha para input type="date"
                Dim fechaConsumo As DateTime = Convert.ToDateTime(row("Fecha_Consumo"))
                txtFechaConsumo.Text = fechaConsumo.ToString("yyyy-MM-dd")
                
                ' Manejar valores NULL para Cantidad y formatear
                If Not row.IsNull("Cantidad") Then
                    Dim cantidadValue As Decimal = Convert.ToDecimal(row("Cantidad"))
                    txtCantidad.Text = cantidadValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                Else
                    txtCantidad.Text = "0.00"
                End If
                
                modalTitle.InnerText = "Editar Registro de Consumo"
                consumoModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del registro de consumo: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarConsumo(consumoId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarConsumoRegistro @ID_Consumo"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Consumo", consumoId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Registro de consumo eliminado correctamente", "success")
                CargarConsumos()
            Else
                MostrarAlerta("Error al eliminar el registro de consumo", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el registro de consumo: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarConsumo(animalId As Integer, alimentoId As Integer, fechaConsumo As Date, cantidad As Decimal) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Consumo), 0) + 1 FROM Consumo_Registro"
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
            Dim query As String = "EXEC InsertarConsumoRegistro @ID_Consumo, @ID_Animal, @ID_Alimento, @Fecha_Consumo, @Cantidad"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Consumo", nuevoId),
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Alimento", alimentoId),
                New SqlParameter("@Fecha_Consumo", fechaConsumo),
                New SqlParameter("@Cantidad", cantidad)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el registro de consumo: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarConsumo(consumoId As Integer, animalId As Integer, alimentoId As Integer, fechaConsumo As Date, cantidad As Decimal) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarConsumoRegistro @ID_Consumo, @ID_Animal, @ID_Alimento, @Fecha_Consumo, @Cantidad"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Consumo", consumoId),
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Alimento", alimentoId),
                New SqlParameter("@Fecha_Consumo", fechaConsumo),
                New SqlParameter("@Cantidad", cantidad)
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

        If ddlAlimento.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un alimento", "warning")
            Return False
        End If

        If String.IsNullOrEmpty(txtFechaConsumo.Text) Then
            MostrarAlerta("La fecha de consumo es requerida", "warning")
            Return False
        End If

        If String.IsNullOrEmpty(txtCantidad.Text) Then
            MostrarAlerta("La cantidad es requerida", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlAnimal.Items.Count > 0 Then
            ddlAnimal.SelectedIndex = 0
        End If
        If ddlAlimento.Items.Count > 0 Then
            ddlAlimento.SelectedIndex = 0
        End If
        txtFechaConsumo.Text = ""
        txtCantidad.Text = ""
        hfConsumoId.Value = "0"
        modalTitle.InnerText = "Nuevo Registro de Consumo"
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

