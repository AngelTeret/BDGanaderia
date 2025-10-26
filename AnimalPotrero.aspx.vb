Imports System.Data
Imports System.Data.SqlClient

Partial Public Class AnimalPotrero
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarAnimales()
            CargarPotreros()
            CargarMovimientos()
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

    Private Sub CargarMovimientos()
        Try
            ' Verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Animal_Potrero'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Animal_Potrero' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Obtener movimientos con JOINs para mostrar nombres
            Dim query As String = "SELECT AP.ID_Animal, A.Nombre_Animal, AP.ID_Potrero, P.Nombre_Potrero, AP.Fecha_Entrada, AP.Fecha_Salida " & _
                                  "FROM Animal_Potrero AP " & _
                                  "INNER JOIN Animal A ON AP.ID_Animal = A.ID_Animal " & _
                                  "INNER JOIN Potrero P ON AP.ID_Potrero = P.ID_Potrero " & _
                                  "ORDER BY AP.Fecha_Entrada DESC, A.Nombre_Animal"
            
            Dim movimientosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvMovimientos.DataSource = movimientosData
            gvMovimientos.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los movimientos: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim animalId As Integer = Convert.ToInt32(hfAnimalIdEliminar.Value)
            Dim potreroId As Integer = Convert.ToInt32(hfPotreroIdEliminar.Value)
            Dim fechaEntrada As Date = Convert.ToDateTime(hfFechaEntradaEliminar.Value)
            EliminarMovimiento(animalId, potreroId, fechaEntrada)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el movimiento: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim animalId As Integer = Convert.ToInt32(ddlAnimal.SelectedValue)
                Dim potreroId As Integer = Convert.ToInt32(ddlPotrero.SelectedValue)
                Dim fechaEntrada As Date = Convert.ToDateTime(txtFechaEntrada.Text)
                
                Dim fechaSalida As Date? = Nothing
                If Not String.IsNullOrEmpty(txtFechaSalida.Text) Then
                    fechaSalida = Convert.ToDateTime(txtFechaSalida.Text)
                    
                    ' Validar que la fecha de salida sea posterior a la de entrada
                    If fechaSalida < fechaEntrada Then
                        MostrarAlerta("La fecha de salida debe ser posterior a la fecha de entrada", "warning")
                        Return
                    End If
                End If

                ' Determinar si es insertar o actualizar
                Dim editando As Boolean = Not String.IsNullOrEmpty(hfAnimalId.Value) AndAlso Not String.IsNullOrEmpty(hfPotreroId.Value) AndAlso Not String.IsNullOrEmpty(hfFechaEntrada.Value)
                
                Dim result As Integer

                If Not editando Then
                    ' Insertar nuevo movimiento
                    If ExisteMovimiento(animalId, potreroId, fechaEntrada) Then
                        MostrarAlerta("Este animal ya está registrado en este potrero en la fecha de entrada seleccionada", "warning")
                        Return
                    End If

                    result = InsertarMovimiento(animalId, potreroId, fechaEntrada, fechaSalida)
                    
                    If result > 0 Then
                        MostrarAlerta("Movimiento registrado correctamente", "success")
                    Else
                        MostrarAlerta("Error al registrar el movimiento", "danger")
                    End If
                Else
                    ' Actualizar movimiento existente (usar IDs originales)
                    animalId = Convert.ToInt32(hfAnimalId.Value)
                    potreroId = Convert.ToInt32(hfPotreroId.Value)
                    fechaEntrada = Convert.ToDateTime(hfFechaEntrada.Value)
                    
                    result = ActualizarMovimiento(animalId, potreroId, fechaEntrada, fechaSalida)
                    
                    If result > 0 Then
                        MostrarAlerta("Movimiento actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el movimiento", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarMovimientos()
                    movimientoModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el movimiento: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvMovimientos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim args As String() = e.CommandArgument.ToString().Split(",")
            
            Select Case e.CommandName
                Case "EditarMovimiento"
                    If args.Length = 3 Then
                        Dim animalId As Integer = Convert.ToInt32(args(0))
                        Dim potreroId As Integer = Convert.ToInt32(args(1))
                        Dim fechaEntrada As Date = Convert.ToDateTime(args(2))
                        EditarMovimiento(animalId, potreroId, fechaEntrada)
                    End If
                Case "EliminarMovimiento"
                    If args.Length = 3 Then
                        Dim animalId As Integer = Convert.ToInt32(args(0))
                        Dim potreroId As Integer = Convert.ToInt32(args(1))
                        Dim fechaEntrada As Date = Convert.ToDateTime(args(2))
                        EliminarMovimiento(animalId, potreroId, fechaEntrada)
                    End If
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarMovimiento(animalId As Integer, potreroId As Integer, fechaEntrada As Date)
        Try
            Dim query As String = "SELECT ID_Animal, ID_Potrero, Fecha_Entrada, Fecha_Salida FROM Animal_Potrero WHERE ID_Animal = @ID_Animal AND ID_Potrero = @ID_Potrero AND Fecha_Entrada = @Fecha_Entrada"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Potrero", potreroId),
                New SqlParameter("@Fecha_Entrada", fechaEntrada)
            }
            
            Dim movimientoData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If movimientoData.Rows.Count > 0 Then
                Dim row As DataRow = movimientoData.Rows(0)
                
                hfAnimalId.Value = animalId.ToString()
                hfPotreroId.Value = potreroId.ToString()
                hfFechaEntrada.Value = fechaEntrada.ToString("yyyy-MM-dd")
                
                ' Cargar y seleccionar Animal (permitir cambiar)
                If ddlAnimal.Items.Count <= 1 Then
                    CargarAnimales()
                End If
                ddlAnimal.SelectedValue = row("ID_Animal").ToString()
                
                ' Cargar y seleccionar Potrero (permitir cambiar)
                If ddlPotrero.Items.Count <= 1 Then
                    CargarPotreros()
                End If
                ddlPotrero.SelectedValue = row("ID_Potrero").ToString()
                
                ' Mostrar fecha entrada (permitir cambiar)
                txtFechaEntrada.Text = fechaEntrada.ToString("yyyy-MM-dd")
                
                ' Mostrar fecha salida (principal campo editable)
                If Not row.IsNull("Fecha_Salida") Then
                    Dim fechaSalida As Date = Convert.ToDateTime(row("Fecha_Salida"))
                    txtFechaSalida.Text = fechaSalida.ToString("yyyy-MM-dd")
                Else
                    txtFechaSalida.Text = ""
                End If
                
                modalTitle.InnerText = "Editar Movimiento de Potrero"
                movimientoModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del movimiento: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarMovimiento(animalId As Integer, potreroId As Integer, fechaEntrada As Date)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarAnimalPotrero @ID_Animal, @ID_Potrero, @Fecha_Entrada"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Potrero", potreroId),
                New SqlParameter("@Fecha_Entrada", fechaEntrada)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Movimiento eliminado correctamente", "success")
                CargarMovimientos()
            Else
                MostrarAlerta("Error al eliminar el movimiento", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el movimiento: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarMovimiento(animalId As Integer, potreroId As Integer, fechaEntrada As Date, fechaSalida As Date?) As Integer
        Try
            ' Usar procedimiento almacenado para insertar
            Dim query As String = "EXEC InsertarAnimalPotrero @ID_Animal, @ID_Potrero, @Fecha_Entrada, @Fecha_Salida"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Potrero", potreroId),
                New SqlParameter("@Fecha_Entrada", fechaEntrada),
                New SqlParameter("@Fecha_Salida", If(fechaSalida.HasValue, DirectCast(fechaSalida.Value, Object), DBNull.Value))
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el movimiento: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarMovimiento(animalId As Integer, potreroId As Integer, fechaEntrada As Date, fechaSalida As Date?) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarAnimalPotrero @ID_Animal, @ID_Potrero, @Fecha_Entrada, @Fecha_Salida"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Potrero", potreroId),
                New SqlParameter("@Fecha_Entrada", fechaEntrada),
                New SqlParameter("@Fecha_Salida", If(fechaSalida.HasValue, DirectCast(fechaSalida.Value, Object), DBNull.Value))
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al actualizar el movimiento: " & ex.Message)
        End Try
    End Function

    Private Function ExisteMovimiento(animalId As Integer, potreroId As Integer, fechaEntrada As Date) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM Animal_Potrero WHERE ID_Animal = @ID_Animal AND ID_Potrero = @ID_Potrero AND Fecha_Entrada = @Fecha_Entrada"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Potrero", potreroId),
                New SqlParameter("@Fecha_Entrada", fechaEntrada)
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

        If ddlPotrero.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un potrero", "warning")
            Return False
        End If

        If String.IsNullOrEmpty(txtFechaEntrada.Text) Then
            MostrarAlerta("La fecha de entrada es requerida", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlAnimal.Items.Count > 0 Then
            ddlAnimal.SelectedIndex = 0
        End If
        If ddlPotrero.Items.Count > 0 Then
            ddlPotrero.SelectedIndex = 0
        End If
        txtFechaEntrada.Text = ""
        txtFechaSalida.Text = ""
        hfAnimalId.Value = ""
        hfPotreroId.Value = ""
        hfFechaEntrada.Value = ""
        modalTitle.InnerText = "Nuevo Movimiento a Potrero"
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

