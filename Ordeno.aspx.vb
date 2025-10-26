Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Ordeno
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarOrdenadores()
            CargarOrdenos()
        End If
    End Sub

    Private Sub CargarOrdenadores()
        Try
            Dim query As String = "SELECT ID_Ordenador, Nombre_Ordenador FROM Ordenador ORDER BY Nombre_Ordenador"
            Dim ordenadoresData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlOrdenador.DataSource = ordenadoresData
            ddlOrdenador.DataTextField = "Nombre_Ordenador"
            ddlOrdenador.DataValueField = "ID_Ordenador"
            ddlOrdenador.DataBind()
            ddlOrdenador.Items.Insert(0, New ListItem("Seleccione un ordenador", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar los ordenadores: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarOrdenos()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Ordeno'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Ordeno' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar JOIN para obtener el nombre del ordenador
            Dim query As String = "SELECT O.ID_Ordeno, O.ID_Ordenador, ORD.Nombre_Ordenador, O.Fecha_Ordeno, O.Turno " & _
                                  "FROM Ordeno O " & _
                                  "INNER JOIN Ordenador ORD ON O.ID_Ordenador = ORD.ID_Ordenador " & _
                                  "ORDER BY O.Fecha_Ordeno DESC"
            
            Dim ordenosData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvOrdenos.DataSource = ordenosData
            gvOrdenos.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los ordeños: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim ordenoId As Integer = Convert.ToInt32(hfOrdenoIdEliminar.Value)
            EliminarOrdeno(ordenoId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el ordeño: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim ordenoId As Integer = 0
                If Not String.IsNullOrEmpty(hfOrdenoId.Value) AndAlso IsNumeric(hfOrdenoId.Value) Then
                    ordenoId = Convert.ToInt32(hfOrdenoId.Value)
                End If
                
                Dim idOrdenador As Integer = Convert.ToInt32(ddlOrdenador.SelectedValue)
                Dim fechaOrdeno As String = txtFechaOrdeno.Text.Trim()
                Dim turno As String = ddlTurno.SelectedValue

                Dim result As Integer

                If ordenoId = 0 Then
                    ' Insertar nuevo ordeño usando procedimiento almacenado
                    result = InsertarOrdeno(idOrdenador, fechaOrdeno, turno)
                    If result > 0 Then
                        MostrarAlerta("Ordeño agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el ordeño", "danger")
                    End If
                Else
                    ' Actualizar ordeño existente usando procedimiento almacenado
                    result = ActualizarOrdeno(ordenoId, idOrdenador, fechaOrdeno, turno)
                    If result > 0 Then
                        MostrarAlerta("Ordeño actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el ordeño", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarOrdenos()
                    ordenoModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el ordeño: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvOrdenos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim ordenoId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarOrdeno"
                    EditarOrdeno(ordenoId)
                Case "EliminarOrdeno"
                    EliminarOrdeno(ordenoId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarOrdeno(ordenoId As Integer)
        Try
            Dim query As String = "SELECT ID_Ordeno, ID_Ordenador, Fecha_Ordeno, Turno FROM Ordeno WHERE ID_Ordeno = @OrdenoId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@OrdenoId", ordenoId)
            }
            
            Dim ordenoData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If ordenoData.Rows.Count > 0 Then
                Dim row As DataRow = ordenoData.Rows(0)
                
                hfOrdenoId.Value = ordenoId.ToString()
                
                ' Cargar ordenadores si no están cargados
                If ddlOrdenador.Items.Count <= 1 Then
                    CargarOrdenadores()
                End If
                ddlOrdenador.SelectedValue = row("ID_Ordenador").ToString()
                
                txtFechaOrdeno.Text = Convert.ToDateTime(row("Fecha_Ordeno")).ToString("yyyy-MM-dd")
                ddlTurno.SelectedValue = row("Turno").ToString()
                
                modalTitle.InnerText = "Editar Ordeño"
                ordenoModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del ordeño: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarOrdeno(ordenoId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarOrdeno @ID_Ordeno"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Ordeno", ordenoId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Ordeño eliminado correctamente", "success")
                CargarOrdenos()
            Else
                MostrarAlerta("Error al eliminar el ordeño", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el ordeño: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarOrdeno(idOrdenador As Integer, fechaOrdeno As String, turno As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Ordeno), 0) + 1 FROM Ordeno"
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
            Dim query As String = "EXEC InsertarOrdeno @ID_Ordeno, @ID_Ordenador, @Fecha_Ordeno, @Turno"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Ordeno", nuevoId),
                New SqlParameter("@ID_Ordenador", idOrdenador),
                New SqlParameter("@Fecha_Ordeno", fechaOrdeno),
                New SqlParameter("@Turno", turno)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el ordeño: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarOrdeno(ordenoId As Integer, idOrdenador As Integer, fechaOrdeno As String, turno As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarOrdeno @ID_Ordeno, @ID_Ordenador, @Fecha_Ordeno, @Turno"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Ordeno", ordenoId),
                New SqlParameter("@ID_Ordenador", idOrdenador),
                New SqlParameter("@Fecha_Ordeno", fechaOrdeno),
                New SqlParameter("@Turno", turno)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If ddlOrdenador.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar un ordenador", "warning")
            Return False
        End If
        
        If String.IsNullOrEmpty(txtFechaOrdeno.Text.Trim()) Then
            MostrarAlerta("La fecha del ordeño es requerida", "warning")
            Return False
        End If
        
        If String.IsNullOrEmpty(ddlTurno.SelectedValue) Then
            MostrarAlerta("El turno es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlOrdenador.Items.Count > 0 Then
            ddlOrdenador.SelectedIndex = 0
        End If
        txtFechaOrdeno.Text = ""
        ddlTurno.SelectedIndex = 0
        hfOrdenoId.Value = "0"
        modalTitle.InnerText = "Nuevo Ordeño"
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

