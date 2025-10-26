Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Ordenador
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarOrdenadores()
        End If
    End Sub

    Private Sub CargarOrdenadores()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Ordenador'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Ordenador' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim ordenadoresData As DataTable = DataAccess.ExecuteStoredProcedure("ListarOrdenador")
            gvOrdenadores.DataSource = ordenadoresData
            gvOrdenadores.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los ordeñadores: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim ordenadorId As Integer = Convert.ToInt32(hfOrdenadorIdEliminar.Value)
            EliminarOrdenador(ordenadorId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el ordeñador: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim ordenadorId As Integer = 0
                If Not String.IsNullOrEmpty(hfOrdenadorId.Value) AndAlso IsNumeric(hfOrdenadorId.Value) Then
                    ordenadorId = Convert.ToInt32(hfOrdenadorId.Value)
                End If
                
                Dim nombreOrdenador As String = txtNombreOrdenador.Text.Trim()

                Dim result As Integer

                If ordenadorId = 0 Then
                    ' Insertar nuevo ordeñador usando procedimiento almacenado
                    result = InsertarOrdenador(nombreOrdenador)
                    If result > 0 Then
                        MostrarAlerta("Ordeñador agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el ordeñador", "danger")
                    End If
                Else
                    ' Actualizar ordeñador existente usando procedimiento almacenado
                    result = ActualizarOrdenador(ordenadorId, nombreOrdenador)
                    If result > 0 Then
                        MostrarAlerta("Ordeñador actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el ordeñador", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarOrdenadores()
                    ordenadorModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el ordeñador: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvOrdenadores_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim ordenadorId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarOrdenador"
                    EditarOrdenador(ordenadorId)
                Case "EliminarOrdenador"
                    EliminarOrdenador(ordenadorId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarOrdenador(ordenadorId As Integer)
        Try
            Dim query As String = "SELECT ID_Ordenador, Nombre_Ordenador FROM Ordenador WHERE ID_Ordenador = @OrdenadorId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@OrdenadorId", ordenadorId)
            }
            
            Dim ordenadorData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If ordenadorData.Rows.Count > 0 Then
                Dim row As DataRow = ordenadorData.Rows(0)
                
                hfOrdenadorId.Value = ordenadorId.ToString()
                txtNombreOrdenador.Text = row("Nombre_Ordenador").ToString()
                
                modalTitle.InnerText = "Editar Ordeñador"
                ordenadorModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del ordeñador: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarOrdenador(ordenadorId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarOrdenador @ID_Ordenador"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Ordenador", ordenadorId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Ordeñador eliminado correctamente", "success")
                CargarOrdenadores()
            Else
                MostrarAlerta("Error al eliminar el ordeñador", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el ordeñador: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarOrdenador(nombreOrdenador As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Ordenador), 0) + 1 FROM Ordenador"
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
            Dim query As String = "EXEC InsertarOrdenador @ID_Ordenador, @Nombre_Ordenador"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Ordenador", nuevoId),
                New SqlParameter("@Nombre_Ordenador", nombreOrdenador)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el ordeñador: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarOrdenador(ordenadorId As Integer, nombreOrdenador As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarOrdenador @ID_Ordenador, @Nombre_Ordenador"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Ordenador", ordenadorId),
                New SqlParameter("@Nombre_Ordenador", nombreOrdenador)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreOrdenador.Text.Trim()) Then
            MostrarAlerta("El nombre del ordeñador es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreOrdenador.Text = ""
        hfOrdenadorId.Value = "0"
        modalTitle.InnerText = "Nuevo Ordeñador"
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

