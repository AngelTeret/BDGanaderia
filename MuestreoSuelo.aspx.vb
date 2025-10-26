Imports System.Data
Imports System.Data.SqlClient

Partial Public Class MuestreoSuelo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarPotreros()
            CargarMuestreos()
        End If
    End Sub

    Private Sub CargarPotreros()
        Try
            Dim query As String = "SELECT ID_Potrero, Nombre_Potrero FROM Potrero ORDER BY Nombre_Potrero"
            Dim potrerosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlPotrero.DataSource = potrerosData
            ddlPotrero.DataTextField = "Nombre_Potrero"
            ddlPotrero.DataValueField = "ID_Potrero"
            ddlPotrero.DataBind()
            ddlPotrero.Items.Insert(0, New ListItem("Seleccione un potrero", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los potreros: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarMuestreos()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Muestreo_Suelo'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Muestreo_Suelo' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar JOIN para obtener el nombre del potrero
            Dim query As String = "SELECT MS.ID_Muestreo, MS.ID_Potrero, P.Nombre_Potrero, MS.Fecha_Muestreo, MS.pH, MS.Materia_Organica " & _
                                  "FROM Muestreo_Suelo MS " & _
                                  "INNER JOIN Potrero P ON MS.ID_Potrero = P.ID_Potrero " & _
                                  "ORDER BY MS.Fecha_Muestreo DESC"
            
            Dim muestreosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvMuestreos.DataSource = muestreosData
            gvMuestreos.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los muestreos de suelo: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim muestreoId As Integer = Convert.ToInt32(hfMuestreoIdEliminar.Value)
            EliminarMuestreo(muestreoId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el muestreo de suelo: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim muestreoId As Integer = 0
                If Not String.IsNullOrEmpty(hfMuestreoId.Value) AndAlso IsNumeric(hfMuestreoId.Value) Then
                    muestreoId = Convert.ToInt32(hfMuestreoId.Value)
                End If
                
                Dim potreroId As Integer = Convert.ToInt32(ddlPotrero.SelectedValue)
                Dim fechaMuestreo As Date = Convert.ToDateTime(txtFechaMuestreo.Text)
                
                Dim pH As Decimal? = Nothing
                If Not String.IsNullOrEmpty(txtpH.Text) Then
                    pH = ParseDecimalFlexible(txtpH.Text)
                End If
                
                Dim materiaOrganica As Decimal? = Nothing
                If Not String.IsNullOrEmpty(txtMateriaOrganica.Text) Then
                    materiaOrganica = ParseDecimalFlexible(txtMateriaOrganica.Text)
                End If

                Dim result As Integer

                If muestreoId = 0 Then
                    ' Insertar nuevo muestreo usando procedimiento almacenado
                    result = InsertarMuestreo(potreroId, fechaMuestreo, pH, materiaOrganica)
                    If result > 0 Then
                        MostrarAlerta("Muestreo de suelo registrado correctamente", "success")
                    Else
                        MostrarAlerta("Error al registrar el muestreo de suelo", "danger")
                    End If
                Else
                    ' Actualizar muestreo existente (no hay stored procedure)
                    result = ActualizarMuestreo(muestreoId, potreroId, fechaMuestreo, pH, materiaOrganica)
                    If result > 0 Then
                        MostrarAlerta("Muestreo de suelo actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el muestreo de suelo", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarMuestreos()
                    muestreoModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el muestreo de suelo: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvMuestreos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim muestreoId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarMuestreo"
                    EditarMuestreo(muestreoId)
                Case "EliminarMuestreo"
                    EliminarMuestreo(muestreoId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarMuestreo(muestreoId As Integer)
        Try
            Dim query As String = "SELECT ID_Muestreo, ID_Potrero, Fecha_Muestreo, pH, Materia_Organica FROM Muestreo_Suelo WHERE ID_Muestreo = @MuestreoId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@MuestreoId", muestreoId)
            }
            
            Dim muestreoData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If muestreoData.Rows.Count > 0 Then
                Dim row As DataRow = muestreoData.Rows(0)
                
                hfMuestreoId.Value = muestreoId.ToString()
                
                ' Cargar potreros si no están cargados
                If ddlPotrero.Items.Count <= 1 Then
                    CargarPotreros()
                End If
                ddlPotrero.SelectedValue = row("ID_Potrero").ToString()
                
                ' Formatear fecha para input type="date"
                Dim fechaMuestreo As DateTime = Convert.ToDateTime(row("Fecha_Muestreo"))
                txtFechaMuestreo.Text = fechaMuestreo.ToString("yyyy-MM-dd")
                
                ' Manejar valores NULL para pH y formatear
                If Not row.IsNull("pH") Then
                    Dim phValue As Decimal = Convert.ToDecimal(row("pH"))
                    txtpH.Text = phValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                Else
                    txtpH.Text = ""
                End If
                
                ' Manejar valores NULL para Materia_Organica y formatear
                If Not row.IsNull("Materia_Organica") Then
                    Dim materiaValue As Decimal = Convert.ToDecimal(row("Materia_Organica"))
                    txtMateriaOrganica.Text = materiaValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                Else
                    txtMateriaOrganica.Text = ""
                End If
                
                modalTitle.InnerText = "Editar Muestreo de Suelo"
                muestreoModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del muestreo de suelo: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarMuestreo(muestreoId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarMuestreoSuelo @ID_Muestreo"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Muestreo", muestreoId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Muestreo de suelo eliminado correctamente", "success")
                CargarMuestreos()
            Else
                MostrarAlerta("Error al eliminar el muestreo de suelo", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el muestreo de suelo: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarMuestreo(potreroId As Integer, fechaMuestreo As Date, pH As Decimal?, materiaOrganica As Decimal?) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Muestreo), 0) + 1 FROM Muestreo_Suelo"
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
            Dim query As String = "EXEC InsertarMuestreoSuelo @ID_Muestreo, @ID_Potrero, @Fecha_Muestreo, @pH, @Materia_Organica"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Muestreo", nuevoId),
                New SqlParameter("@ID_Potrero", potreroId),
                New SqlParameter("@Fecha_Muestreo", fechaMuestreo),
                New SqlParameter("@pH", If(pH.HasValue, CObj(pH.Value), CObj(DBNull.Value))),
                New SqlParameter("@Materia_Organica", If(materiaOrganica.HasValue, CObj(materiaOrganica.Value), CObj(DBNull.Value)))
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el muestreo de suelo: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarMuestreo(muestreoId As Integer, potreroId As Integer, fechaMuestreo As Date, pH As Decimal?, materiaOrganica As Decimal?) As Integer
        Try
            ' No hay procedimiento almacenado de actualizar, usar UPDATE directo
            Dim query As String = "UPDATE Muestreo_Suelo SET ID_Potrero = @ID_Potrero, Fecha_Muestreo = @Fecha_Muestreo, pH = @pH, Materia_Organica = @Materia_Organica WHERE ID_Muestreo = @ID_Muestreo"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Muestreo", muestreoId),
                New SqlParameter("@ID_Potrero", potreroId),
                New SqlParameter("@Fecha_Muestreo", fechaMuestreo),
                New SqlParameter("@pH", If(pH.HasValue, CObj(pH.Value), CObj(DBNull.Value))),
                New SqlParameter("@Materia_Organica", If(materiaOrganica.HasValue, CObj(materiaOrganica.Value), CObj(DBNull.Value)))
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
        If ddlPotrero.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un potrero", "warning")
            Return False
        End If

        If String.IsNullOrEmpty(txtFechaMuestreo.Text) Then
            MostrarAlerta("La fecha de muestreo es requerida", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlPotrero.Items.Count > 0 Then
            ddlPotrero.SelectedIndex = 0
        End If
        txtFechaMuestreo.Text = ""
        txtpH.Text = ""
        txtMateriaOrganica.Text = ""
        hfMuestreoId.Value = "0"
        modalTitle.InnerText = "Nuevo Muestreo de Suelo"
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

