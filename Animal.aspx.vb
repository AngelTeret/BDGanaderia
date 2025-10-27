Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization

Partial Public Class Animal
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Mostrar mensaje de éxito si viene de una redirección
            Dim successParam As String = Request.QueryString("success")
            If Not String.IsNullOrEmpty(successParam) Then
                ' Asegurar que el modal esté cerrado
                animalModal.Style("display") = "none"
                LimpiarFormulario()
                
                If successParam = "added" Then
                    MostrarAlerta("Animal agregado correctamente", "success")
                ElseIf successParam = "updated" Then
                    MostrarAlerta("Animal actualizado correctamente", "success")
                End If
            End If
            
            ' Script para prevenir el reenvío del formulario si viene de POST
            If IsPostBack AndAlso Not String.IsNullOrEmpty(successParam) Then
                ' Si esto es un POST pero tiene parámetro de éxito, es un back del navegador
                ' Redireccionar para evitar el modal
                Response.Redirect("Animal.aspx", False)
                Return
            End If
            
            CargarDropDownLists()
            CargarAnimales()
        End If
    End Sub

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

        ' Intentar parsear con cultura invariante (siempre punto como decimal)
        success = Decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, value)
        
        If success Then
            Return Math.Round(value, 2)
        End If

        Throw New FormatException("Número inválido: " & input)
    End Function

    Private Sub CargarDropDownLists()
        Try
            ' Cargar Razas
            Dim queryRazas As String = "SELECT ID_Raza, Nombre_Raza FROM Raza ORDER BY Nombre_Raza"
            Dim razasData As DataTable = DataAccess.ExecuteSelectQuery(queryRazas)
            ddlRaza.DataSource = razasData
            ddlRaza.DataValueField = "ID_Raza"
            ddlRaza.DataTextField = "Nombre_Raza"
            ddlRaza.DataBind()
            ddlRaza.Items.Insert(0, New ListItem("Seleccione una raza", "0"))

            ' Cargar Tipos de Animal
            Dim queryTipos As String = "SELECT ID_TipoAnimal, Nombre_Tipo FROM Tipo_Animal ORDER BY Nombre_Tipo"
            Dim tiposData As DataTable = DataAccess.ExecuteSelectQuery(queryTipos)
            ddlTipoAnimal.DataSource = tiposData
            ddlTipoAnimal.DataValueField = "ID_TipoAnimal"
            ddlTipoAnimal.DataTextField = "Nombre_Tipo"
            ddlTipoAnimal.DataBind()
            ddlTipoAnimal.Items.Insert(0, New ListItem("Seleccione un tipo", "0"))

            ' Cargar Estados de Animal
            Dim queryEstados As String = "SELECT ID_EstadoAnimal, Nombre_Estado FROM Estado_Animal ORDER BY Nombre_Estado"
            Dim estadosData As DataTable = DataAccess.ExecuteSelectQuery(queryEstados)
            ddlEstadoAnimal.DataSource = estadosData
            ddlEstadoAnimal.DataValueField = "ID_EstadoAnimal"
            ddlEstadoAnimal.DataTextField = "Nombre_Estado"
            ddlEstadoAnimal.DataBind()
            ddlEstadoAnimal.Items.Insert(0, New ListItem("Seleccione un estado", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarAnimales()
        Try
            ' Verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Animal'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Animal' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Query con JOINs para mostrar nombres en lugar de IDs
            Dim query As String = "SELECT a.ID_Animal, a.Nombre_Animal, a.Fecha_Nacimiento, a.Sexo, a.Peso, " &
                                  "r.Nombre_Raza AS Raza_Nombre, " &
                                  "ta.Nombre_Tipo AS Tipo_Nombre, " &
                                  "ea.Nombre_Estado AS Estado_Nombre " &
                                  "FROM Animal a " &
                                  "LEFT JOIN Raza r ON a.ID_Raza = r.ID_Raza " &
                                  "LEFT JOIN Tipo_Animal ta ON a.ID_TipoAnimal = ta.ID_TipoAnimal " &
                                  "LEFT JOIN Estado_Animal ea ON a.ID_EstadoAnimal = ea.ID_EstadoAnimal " &
                                  "ORDER BY a.ID_Animal DESC"
            
            Dim animalesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvAnimales.DataSource = animalesData
            gvAnimales.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los animales: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim animalId As Integer = Convert.ToInt32(hfAnimalIdEliminar.Value)
            EliminarAnimal(animalId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el animal: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            ' Validar primero antes de procesar
            If Not ValidarFormulario() Then
                ' La función MostrarAlertaModal ya maneja cerrar y reabrir el modal
                Return ' Salir si la validación falla
            End If
            
            Dim animalId As Integer = 0
            If Not String.IsNullOrEmpty(hfAnimalId.Value) AndAlso IsNumeric(hfAnimalId.Value) Then
                animalId = Convert.ToInt32(hfAnimalId.Value)
            End If
            
            Dim nombreAnimal As String = txtNombreAnimal.Text.Trim()
            Dim fechaNacimiento As DateTime = DateTime.Parse(txtFechaNacimiento.Text)
            Dim sexo As String = ddlSexo.SelectedValue
            
            ' Convertir el peso que puede venir con punto o coma
           Dim peso As Decimal = ParseDecimalFlexible(txtPeso.Text)
            
            Dim idRaza As Integer = Convert.ToInt32(ddlRaza.SelectedValue)
            Dim idTipoAnimal As Integer = Convert.ToInt32(ddlTipoAnimal.SelectedValue)
            Dim idEstadoAnimal As Integer = Convert.ToInt32(ddlEstadoAnimal.SelectedValue)

            Dim result As Integer

            If animalId = 0 Then
                ' Insertar nuevo animal
                result = InsertarAnimal(nombreAnimal, fechaNacimiento, sexo, peso, idRaza, idTipoAnimal, idEstadoAnimal)
            Else
                ' Actualizar animal existente
                result = ActualizarAnimal(animalId, nombreAnimal, fechaNacimiento, sexo, peso, idRaza, idTipoAnimal, idEstadoAnimal)
            End If

            If result > 0 Then
                ' Redireccionar para evitar el mensaje de reenvío del formulario
                Response.Redirect("Animal.aspx?success=" & If(animalId = 0, "added", "updated"), False)
                Return
            Else
                If animalId = 0 Then
                    MostrarAlerta("Error al agregar el animal", "danger")
                Else
                    MostrarAlerta("Error al actualizar el animal", "danger")
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el animal: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvAnimales_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim animalId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarAnimal"
                    EditarAnimal(animalId)
                Case "EliminarAnimal"
                    EliminarAnimal(animalId)
            End Select
        Catch ex As Exception
            ' Registrar error en bitácora
            Try
                BitacoraHelper.RegistrarError(ex, "Animal.aspx")
            Catch
            End Try
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarAnimal(animalId As Integer)
        Try
            Dim query As String = "SELECT ID_Animal, Nombre_Animal, Fecha_Nacimiento, Sexo, Peso, ID_Raza, ID_TipoAnimal, ID_EstadoAnimal FROM Animal WHERE ID_Animal = @AnimalId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@AnimalId", animalId)
            }
            
            Dim animalData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If animalData.Rows.Count > 0 Then
                Dim row As DataRow = animalData.Rows(0)
                
                hfAnimalId.Value = animalId.ToString()
                txtNombreAnimal.Text = row("Nombre_Animal").ToString()
                txtFechaNacimiento.Text = DateTime.Parse(row("Fecha_Nacimiento").ToString()).ToString("yyyy-MM-dd")
                ddlSexo.SelectedValue = row("Sexo").ToString()
                
                ' Asegurar que el peso se muestre correctamente
                Try
                    If Not IsDBNull(row("Peso")) Then
                        Dim pesoValue As Object = row("Peso")
                        If pesoValue IsNot Nothing AndAlso Not String.IsNullOrEmpty(pesoValue.ToString()) Then
                            Dim pesoDecimal As Decimal = Convert.ToDecimal(pesoValue)
                            txtPeso.Text = pesoDecimal.ToString("0.##")
                        Else
                            txtPeso.Text = ""
                        End If
                    Else
                        txtPeso.Text = ""
                    End If
                Catch pesoEx As Exception
                    ' Si hay error, intentar con el valor directo
                    txtPeso.Text = row("Peso").ToString()
                End Try
                
                ddlRaza.SelectedValue = row("ID_Raza").ToString()
                ddlTipoAnimal.SelectedValue = row("ID_TipoAnimal").ToString()
                ddlEstadoAnimal.SelectedValue = row("ID_EstadoAnimal").ToString()
                
                modalTitle.InnerText = "Editar Animal"
                animalModal.Style("display") = "block"
                
                ' Forzar el scroll y asegurar que el modal sea visible
                Dim script As String = "setTimeout(function(){ document.getElementById('animalModal').style.display = 'block'; }, 100);"
                ClientScript.RegisterStartupScript(Me.GetType(), "OpenModal", script, True)
            End If
        Catch ex As Exception
            ' Registrar error en bitácora
            Try
                BitacoraHelper.RegistrarError(ex, "Animal.aspx")
            Catch
            End Try
            MostrarAlerta("Error al cargar los datos del animal: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarAnimal(animalId As Integer)
        Try
            ' Obtener nombre del animal antes de eliminar
            Dim nombreAnimal As String = ""
            Try
                Dim queryNombre As String = "SELECT Nombre_Animal FROM Animal WHERE ID_Animal = @ID_Animal"
                Dim paramsNombre() As SqlParameter = {
                    New SqlParameter("@ID_Animal", animalId)
                }
                Dim nombreObj As Object = DataAccess.ExecuteScalar(queryNombre, paramsNombre)
                If nombreObj IsNot Nothing Then nombreAnimal = nombreObj.ToString()
            Catch
            End Try
            
            ' Eliminar el animal
            Dim query As String = "EXEC EliminarAnimal @ID_Animal"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                ' Registrar en bitácora
                Try
                    BitacoraHelper.RegistrarEliminar("Animal", animalId.ToString(), "Nombre: " & nombreAnimal)
                Catch
                End Try
                
                MostrarAlerta("Animal eliminado correctamente", "success")
                CargarAnimales()
            Else
                MostrarAlerta("Error al eliminar el animal", "danger")
            End If
        Catch ex As Exception
            Try
                BitacoraHelper.RegistrarError(ex, "Animal.aspx")
            Catch
            End Try
            MostrarAlerta("Error al eliminar el animal: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarAnimal(nombreAnimal As String, fechaNacimiento As DateTime, sexo As String, peso As Decimal, idRaza As Integer, idTipoAnimal As Integer, idEstadoAnimal As Integer) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Animal), 0) + 1 FROM Animal"
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
            Dim query As String = "EXEC InsertarAnimal @ID_Animal, @Nombre_Animal, @Fecha_Nacimiento, @Sexo, @Peso, @ID_Raza, @ID_TipoAnimal, @ID_EstadoAnimal"
            
            ' Crear parámetro de peso con precisión y escala
            Dim pPeso As New SqlParameter("@Peso", SqlDbType.Decimal)
            pPeso.Precision = 10
            pPeso.Scale = 2
            pPeso.Value = peso
            
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", nuevoId),
                New SqlParameter("@Nombre_Animal", nombreAnimal),
                New SqlParameter("@Fecha_Nacimiento", fechaNacimiento),
                New SqlParameter("@Sexo", sexo),
                pPeso,
                New SqlParameter("@ID_Raza", idRaza),
                New SqlParameter("@ID_TipoAnimal", idTipoAnimal),
                New SqlParameter("@ID_EstadoAnimal", idEstadoAnimal)
            }
            
            Dim resultado As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            
            ' Registrar en bitácora
            If resultado > 0 Then
                Try
                    BitacoraHelper.RegistrarCrear("Animal", nuevoId.ToString(), "Nombre: " & nombreAnimal)
                Catch
                End Try
            End If
            
            Return resultado
        Catch ex As Exception
            Try
                BitacoraHelper.RegistrarError(ex, "Animal.aspx")
            Catch
            End Try
            Throw New Exception("Error al insertar el animal: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarAnimal(animalId As Integer, nombreAnimal As String, fechaNacimiento As DateTime, sexo As String, peso As Decimal, idRaza As Integer, idTipoAnimal As Integer, idEstadoAnimal As Integer) As Integer
        Try
            Dim query As String = "EXEC ActualizarAnimal @ID_Animal, @Nombre_Animal, @Fecha_Nacimiento, @Sexo, @Peso, @ID_Raza, @ID_TipoAnimal, @ID_EstadoAnimal"
            
            ' Crear parámetro de peso con precisión y escala
            Dim pPeso As New SqlParameter("@Peso", SqlDbType.Decimal)
            pPeso.Precision = 10
            pPeso.Scale = 2
            pPeso.Value = peso
            
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@Nombre_Animal", nombreAnimal),
                New SqlParameter("@Fecha_Nacimiento", fechaNacimiento),
                New SqlParameter("@Sexo", sexo),
                pPeso,
                New SqlParameter("@ID_Raza", idRaza),
                New SqlParameter("@ID_TipoAnimal", idTipoAnimal),
                New SqlParameter("@ID_EstadoAnimal", idEstadoAnimal)
            }
            
            Dim resultado As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            
            ' Registrar en bitácora
            If resultado > 0 Then
                Try
                    BitacoraHelper.RegistrarActualizar("Animal", animalId.ToString(), "Nombre: " & nombreAnimal)
                Catch
                End Try
            End If
            
            Return resultado
        Catch ex As Exception
            Try
                BitacoraHelper.RegistrarError(ex, "Animal.aspx")
            Catch
            End Try
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        Dim errores As New List(Of String)
        
        If String.IsNullOrEmpty(txtNombreAnimal.Text.Trim()) Then
            errores.Add("• El nombre del animal es requerido")
        End If
        
        If String.IsNullOrEmpty(txtFechaNacimiento.Text) Then
            errores.Add("• La fecha de nacimiento es requerida")
        End If
        
        If String.IsNullOrWhiteSpace(txtPeso.Text) Then
            errores.Add("• El peso es requerido")
        Else
            Try
                Dim p = ParseDecimalFlexible(txtPeso.Text)
                If p <= 0D Then errores.Add("• El peso debe ser mayor a 0")
            Catch
                errores.Add("• El peso debe ser un número válido (ej.: 25,50 o 25.50)")
            End Try
        End If

        
        If ddlRaza.SelectedValue = "0" Then
            errores.Add("• Debe seleccionar una raza")
        End If
        
        If ddlTipoAnimal.SelectedValue = "0" Then
            errores.Add("• Debe seleccionar un tipo de animal")
        End If
        
        If ddlEstadoAnimal.SelectedValue = "0" Then
            errores.Add("• Debe seleccionar un estado del animal")
        End If
        
        If errores.Count > 0 Then
            Dim mensaje As String = "Por favor complete los siguientes campos:<br><br>" & String.Join("<br>", errores)
            ' Mostrar alerta pero mantener el modal abierto
            MostrarAlertaModal(mensaje)
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreAnimal.Text = ""
        txtFechaNacimiento.Text = ""
        ddlSexo.SelectedIndex = 0
        txtPeso.Text = ""
        ddlRaza.SelectedIndex = 0
        ddlTipoAnimal.SelectedIndex = 0
        ddlEstadoAnimal.SelectedIndex = 0
        hfAnimalId.Value = "0"
        modalTitle.InnerText = "Nuevo Animal"
    End Sub

    Private Sub MostrarAlertaModal(mensaje As String)
        ' Función especial para mostrar alertas cuando el modal está abierto
        ' Escapar comillas para JavaScript
        Dim mensajeEscapado As String = mensaje.Replace("'", "\'").Replace("""", "\""").Replace(vbCrLf, "<br>")
        
        ' Cerrar el modal temporalmente y volver a abrirlo cuando se cierre la alerta
        Dim script As String = 
            "document.getElementById('animalModal').style.display = 'none';" &
            "Swal.fire({" &
            "icon: 'warning', " &
            "title: 'Campos Incompletos', " &
            "html: '" & mensajeEscapado & "', " &
            "confirmButtonText: 'Aceptar', " &
            "confirmButtonColor: '#f39c12'" &
            "}).then(function() {" &
            "  setTimeout(function(){ document.getElementById('animalModal').style.display = 'block'; }, 100);" &
            "});"
        
        ClientScript.RegisterStartupScript(Me.GetType(), "ShowAlertModal", script, True)
    End Sub
    
    Private Sub MostrarAlerta(mensaje As String, tipo As String)
        ' Escapar comillas para JavaScript
        Dim mensajeEscapado As String = mensaje.Replace("'", "\'").Replace("""", "\""").Replace(vbCrLf, "<br>")
        
        Dim script As String = ""
        Select Case tipo
            Case "success"
                script = "Swal.fire({icon: 'success', title: '¡Éxito!', text: '" & mensajeEscapado & "', confirmButtonText: 'Aceptar', confirmButtonColor: '#27ae60', timer: 3000, timerProgressBar: true});"
            Case "danger", "error"
                script = "Swal.fire({icon: 'error', title: 'Error', text: '" & mensajeEscapado & "', confirmButtonText: 'Aceptar', confirmButtonColor: '#e74c3c'});"
            Case "warning"
                ' Usar html en vez de text para permitir HTML en el mensaje
                script = "Swal.fire({icon: 'warning', title: 'Campos Incompletos', html: '" & mensajeEscapado & "', confirmButtonText: 'Aceptar', confirmButtonColor: '#f39c12'});"
            Case "info"
                script = "Swal.fire({icon: 'info', title: 'Información', text: '" & mensajeEscapado & "', confirmButtonText: 'Aceptar', confirmButtonColor: '#3498db'});"
            Case Else
                script = "Swal.fire({icon: 'info', title: 'Información', text: '" & mensajeEscapado & "', confirmButtonText: 'Aceptar', confirmButtonColor: '#3498db'});"
        End Select
        
        ClientScript.RegisterStartupScript(Me.GetType(), "ShowAlert", script, True)
    End Sub
End Class
