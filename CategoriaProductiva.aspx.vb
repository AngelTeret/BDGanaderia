Imports System.Data
Imports System.Data.SqlClient

Partial Public Class CategoriaProductiva
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarCategorias()
        End If
    End Sub

    Private Sub CargarCategorias()
        Try
            ' Primero verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Categoria_Productiva'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Categoria_Productiva' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            Dim query As String = "SELECT ID_Categoria, Nombre_Categoria FROM Categoria_Productiva ORDER BY ID_Categoria ASC"
            Dim categoriasData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvCategorias.DataSource = categoriasData
            gvCategorias.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar las categorías productivas: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim categoriaId As Integer = Convert.ToInt32(hfCategoriaIdEliminar.Value)
            EliminarCategoria(categoriaId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la categoría productiva: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim categoriaId As Integer = 0
                If Not String.IsNullOrEmpty(hfCategoriaId.Value) AndAlso IsNumeric(hfCategoriaId.Value) Then
                    categoriaId = Convert.ToInt32(hfCategoriaId.Value)
                End If
                
                Dim nombreCategoria As String = txtNombreCategoria.Text.Trim()

                Dim result As Integer

                If categoriaId = 0 Then
                    ' Insertar nueva categoría productiva usando procedimiento almacenado
                    result = InsertarCategoria(nombreCategoria)
                    If result > 0 Then
                        MostrarAlerta("Categoría productiva agregada correctamente", "success")
                    Else
                        MostrarAlerta("Error al agregar la categoría productiva", "danger")
                    End If
                Else
                    ' Actualizar categoría productiva existente usando procedimiento almacenado
                    result = ActualizarCategoria(categoriaId, nombreCategoria)
                    If result > 0 Then
                        MostrarAlerta("Categoría productiva actualizada correctamente", "success")
                    Else
                        MostrarAlerta("Error al actualizar la categoría productiva", "danger")
                    End If
                End If

                If result > 0 Then
                    CargarCategorias()
                    categoriaModal.Style("display") = "none"
                    LimpiarFormulario()
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar la categoría productiva: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvCategorias_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim categoriaId As Integer = Convert.ToInt32(e.CommandArgument)

            Select Case e.CommandName
                Case "EditarCategoria"
                    EditarCategoria(categoriaId)
                Case "EliminarCategoria"
                    EliminarCategoria(categoriaId)
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EditarCategoria(categoriaId As Integer)
        Try
            Dim query As String = "SELECT ID_Categoria, Nombre_Categoria FROM Categoria_Productiva WHERE ID_Categoria = @CategoriaId"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@CategoriaId", categoriaId)
            }
            
            Dim categoriaData As DataTable = DataAccess.ExecuteSelectQuery(query, parameters)
            If categoriaData.Rows.Count > 0 Then
                Dim row As DataRow = categoriaData.Rows(0)
                
                hfCategoriaId.Value = categoriaId.ToString()
                txtNombreCategoria.Text = row("Nombre_Categoria").ToString()
                
                modalTitle.InnerText = "Editar Categoría Productiva"
                categoriaModal.Style("display") = "block"
            End If
        Catch ex As Exception
            MostrarAlerta("Error al cargar los datos de la categoría productiva: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarCategoria(categoriaId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarCategoriaProductiva @ID_Categoria"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Categoria", categoriaId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Categoría productiva eliminada correctamente", "success")
                CargarCategorias()
            Else
                MostrarAlerta("Error al eliminar la categoría productiva", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la categoría productiva: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarCategoria(nombreCategoria As String) As Integer
        Try
            ' Obtener el siguiente ID
            Dim queryId As String = "SELECT ISNULL(MAX(ID_Categoria), 0) + 1 FROM Categoria_Productiva"
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
            Dim query As String = "EXEC InsertarCategoriaProductiva @ID_Categoria, @Nombre_Categoria"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Categoria", nuevoId),
                New SqlParameter("@Nombre_Categoria", nombreCategoria)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar la categoría productiva: " & ex.Message)
        End Try
    End Function

    Private Function ActualizarCategoria(categoriaId As Integer, nombreCategoria As String) As Integer
        Try
            ' Usar procedimiento almacenado para actualizar
            Dim query As String = "EXEC ActualizarCategoriaProductiva @ID_Categoria, @Nombre_Categoria"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Categoria", categoriaId),
                New SqlParameter("@Nombre_Categoria", nombreCategoria)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidarFormulario() As Boolean
        If String.IsNullOrEmpty(txtNombreCategoria.Text.Trim()) Then
            MostrarAlerta("El nombre de la categoría es requerido", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtNombreCategoria.Text = ""
        hfCategoriaId.Value = "0"
        modalTitle.InnerText = "Nueva Categoría Productiva"
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
