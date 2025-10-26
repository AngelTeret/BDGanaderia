Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Alimento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarAlimentos()
        End If
    End Sub

    Private Sub CargarAlimentos()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Alimento'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Alimento' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Usar procedimiento almacenado para listar
            Dim alimentosData As DataTable = DataAccess.ExecuteStoredProcedure("ListarAlimento")
            gvAlimentos.DataSource = alimentosData
            gvAlimentos.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar los alimentos: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim alimentoId As Integer = Convert.ToInt32(hfAlimentoIdEliminar.Value)
            EliminarAlimento(alimentoId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el alimento: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim alimentoId As Integer = 0
                If Not String.IsNullOrEmpty(hfAlimentoId.Value) AndAlso IsNumeric(hfAlimentoId.Value) Then
                    alimentoId = Convert.ToInt32(hfAlimentoId.Value)
                End If
                
                Dim nombreAlimento As String = txtNombreAlimento.Text.Trim()

                Dim result As Integer

                If alimentoId = 0 Then
                    ' Insertar nuevo alimento usando procedimiento almacenado
                    result = InsertarAlimento(nombreAlimento)
                    If result > 0 Then
                        MostrarAlerta("Alimento agregado correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar el alimento", "danger")
                    End If
                Else
                    ' Actualizar alimento existente usando procedimiento almacenado
                    result = ActualizarAlimento(alimentoId, nombreAlimento)
                    If result > 0 Then
                        MostrarAlerta("Alimento actualizado correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar el alimento", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarAlimentos()
                    alimentoModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar el alimento: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvAlimentos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim alimentoId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarAlimento"
                    EditarAlimento(alimentoId)
                Case "EliminarAlimento"
                    EliminarAlimento(alimentoId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarAlimento(alimentoId As Integer)
        Try
            Dim query As String = "SELECT ID_Alimento, Nombre_Alimento FROM Alimento WHERE ID_Alimento = @AlimentoId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@AlimentoId", alimentoId)
            }
            
            Dim alimentoData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If alimentoData.Rows.Count > 0 Then
                Dim row As DataRow = alimentoData.Rows(0)
                
                hfAlimentoId.Value = alimentoId.ToString()
                txtNombreAlimento.Text = row("Nombre_Alimento").ToString()
                
                modalTitle.InnerText = "Editar Alimento"
                alimentoModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos del alimento: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarAlimento(alimentoId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarAlimento @ID_Alimento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Alimento", alimentoId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Alimento eliminado correctamente", "success")
                CargarAlimentos()
            Else
                MostrarAlerta("Error al eliminar el alimento", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar el alimento: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarAlimento(nombreAlimento As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Alimento), 0) + 1 FROM Alimento"
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
            Dim query As String = "EXEC InsertarAlimento @ID_Alimento, @Nombre_Alimento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Alimento", nuevoId),
                New SqlParameter("@Nombre_Alimento", nombreAlimento)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar el alimento: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarAlimento(alimentoId As Integer, nombreAlimento As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarAlimento @ID_Alimento, @Nombre_Alimento"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Alimento", alimentoId),
                New SqlParameter("@Nombre_Alimento", nombreAlimento)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreAlimento.Text.Trim()) Then
            MostrarAlerta("El nombre del alimento es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreAlimento.Text = ""
        hfAlimentoId.Value = "0"
        modalTitle.InnerText = "Nuevo Alimento"
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

