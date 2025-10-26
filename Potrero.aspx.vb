Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Potrero
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarPotreros()
        End If
    End Sub

    Private Sub CargarPotreros()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Potrero'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Potrero' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim potrerosData As DataTable = DataAccess.ExecuteStoredProcedure("ListarPotrero")
            gvPotreros.DataSource = potrerosData
            gvPotreros.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los potreros: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim potreroId As Integer = Convert.ToInt32(hfPotreroIdEliminar.Value)
            EliminarPotrero(potreroId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el potrero: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim potreroId As Integer = 0
                If Not String.IsNullOrEmpty(hfPotreroId.Value) AndAlso IsNumeric(hfPotreroId.Value) Then
                    potreroId = Convert.ToInt32(hfPotreroId.Value)
                End If
                
                Dim nombrePotrero As String = txtNombrePotrero.Text.Trim()
                Dim area As Decimal = 0
                
                If Not String.IsNullOrEmpty(txtArea.Text) Then
                    Try
                        area = ParseDecimalFlexible(txtArea.Text)
                    Catch ex As Exception
                        MostrarAlerta("El área debe ser un número válido", "warning")
                        Return
                    End Try
                End If

                Dim result As Integer

                If potreroId = 0 Then
                    ' Insertar nuevo potrero usando procedimiento almacenado
                    result = InsertarPotrero(nombrePotrero, area)
                    If result > 0 Then
                        MostrarAlerta("Potrero agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el potrero", "danger")
                    End If
                Else
                    ' Actualizar potrero existente usando procedimiento almacenado
                    result = ActualizarPotrero(potreroId, nombrePotrero, area)
                    If result > 0 Then
                        MostrarAlerta("Potrero actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el potrero", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarPotreros()
                    potreroModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el potrero: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvPotreros_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim potreroId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarPotrero"
                    EditarPotrero(potreroId)
                Case "EliminarPotrero"
                    EliminarPotrero(potreroId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarPotrero(potreroId As Integer)
        Try
            Dim query As String = "SELECT ID_Potrero, Nombre_Potrero, Area FROM Potrero WHERE ID_Potrero = @PotreroId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@PotreroId", potreroId)
            }
            
            Dim potreroData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If potreroData.Rows.Count > 0 Then
                Dim row As DataRow = potreroData.Rows(0)
                
                hfPotreroId.Value = potreroId.ToString()
                txtNombrePotrero.Text = row("Nombre_Potrero").ToString()
                
                ' Manejar valores NULL para Area y convertir formato decimal
                If Not row.IsNull("Area") Then
                    Dim areaValue As Decimal = Convert.ToDecimal(row("Area"))
                    ' Convertir a string usando punto como separador para mostrar
                    txtArea.Text = areaValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                Else
                    txtArea.Text = "0.00"
                End If
                
                modalTitle.InnerText = "Editar Potrero"
                potreroModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del potrero: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarPotrero(potreroId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarPotrero @ID_Potrero"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Potrero", potreroId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Potrero eliminado correctamente", "success")
                CargarPotreros()
            Else
                MostrarAlerta("Error al eliminar el potrero", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el potrero: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarPotrero(nombrePotrero As String, area As Decimal) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Potrero), 0) + 1 FROM Potrero"
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
            Dim query As String = "EXEC InsertarPotrero @ID_Potrero, @Nombre_Potrero, @Area"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Potrero", nuevoId),
                New SqlParameter("@Nombre_Potrero", nombrePotrero),
                New SqlParameter("@Area", area)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el potrero: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarPotrero(potreroId As Integer, nombrePotrero As String, area As Decimal) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarPotrero @ID_Potrero, @Nombre_Potrero, @Area"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Potrero", potreroId),
                New SqlParameter("@Nombre_Potrero", nombrePotrero),
                New SqlParameter("@Area", area)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombrePotrero.Text.Trim()) Then
            MostrarAlerta("El nombre del potrero es requerido", "warning")
            Return False
        End If

        If Not String.IsNullOrEmpty(txtArea.Text) Then
            Try
                Dim testValue = ParseDecimalFlexible(txtArea.Text)
            Catch ex As Exception
                MostrarAlerta("El área debe ser un número válido", "warning")
                Return False
            End Try
        End If

        Return True
    End Function

    Private Function ParseDecimalFlexible(input As String) As Decimal
        If String.IsNullOrWhiteSpace(input) Then Throw New FormatException("Vacío")

        Dim s = input.Trim()
        s = s.Replace(" ", "")

        Dim value As Decimal
        Dim success As Boolean = False

        ' Estrategia: determinar el separador decimal
        ' Si tiene punto, es decimal en inglés; si tiene coma, también puede ser decimal en ES
        ' Si tiene ambos, el último es el separador decimal
        
        Dim tienePunto As Boolean = s.Contains(".")
        Dim tieneComa As Boolean = s.Contains(",")
        
        If tienePunto AndAlso tieneComa Then
            ' Si tiene ambos, el último es el decimal
            If s.LastIndexOf(".") > s.LastIndexOf(",") Then
                s = s.Replace(",", "") ' Eliminar comas (miles)
            Else
                s = s.Replace(".", "") ' Eliminar puntos (miles)
                s = s.Replace(",", ".") ' Convertir coma a punto
            End If
        ElseIf tieneComa Then
            ' Solo tiene coma, es el decimal
            s = s.Replace(".", "") ' Eliminar puntos si hay
            s = s.Replace(",", ".") ' Convertir coma a punto
        ElseIf tienePunto Then
            ' Solo tiene punto, puede ser decimal o miles
            ' Asumimos que si hay punto es decimal
            s = s.Replace(",", "") ' Eliminar comas si hay
        Else
            ' No tiene separador, es un entero
        End If

        If Decimal.TryParse(s, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, value) Then
            Return value
        End If

        Throw New FormatException("No se pudo convertir '" & input & "' a decimal")
    End Function

    Private Sub LimpiarFormulario()
        txtNombrePotrero.Text = ""
        txtArea.Text = ""
        hfPotreroId.Value = "0"
        modalTitle.InnerText = "Nuevo Potrero"
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

