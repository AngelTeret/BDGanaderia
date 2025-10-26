Imports System.Data
Imports System.Data.SqlClient

Partial Public Class AnimalCategoria
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarAnimales()
            CargarCategorias()
            CargarAsignaciones()
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

    Private Sub CargarCategorias()
        Try
            Dim query As String = "SELECT ID_Categoria, Nombre_Categoria FROM Categoria_Productiva ORDER BY Nombre_Categoria"
            Dim categoriasData As DataTable = DataAccess.ExecuteSelectQuery(query)
            
            ddlCategoria.DataSource = categoriasData
            ddlCategoria.DataTextField = "Nombre_Categoria"
            ddlCategoria.DataValueField = "ID_Categoria"
            ddlCategoria.DataBind()
            ddlCategoria.Items.Insert(0, New ListItem("Seleccione una categoría", "0"))
        Catch ex As Exception
            MostrarAlerta("Error al cargar las categorías: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub CargarAsignaciones()
        Try
            ' Verificar si la tabla existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Animal_Categoria'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) = 0 Then
                MostrarAlerta("La tabla 'Animal_Categoria' no existe en la base de datos. Por favor, créala primero.", "warning")
                Return
            End If
            
            ' Obtener asignaciones con JOINs para mostrar nombres
            Dim query As String = "SELECT AC.ID_Animal, A.Nombre_Animal, AC.ID_Categoria, CP.Nombre_Categoria " & _
                                  "FROM Animal_Categoria AC " & _
                                  "INNER JOIN Animal A ON AC.ID_Animal = A.ID_Animal " & _
                                  "INNER JOIN Categoria_Productiva CP ON AC.ID_Categoria = CP.ID_Categoria " & _
                                  "ORDER BY A.Nombre_Animal, CP.Nombre_Categoria"
            
            Dim asignacionesData As DataTable = DataAccess.ExecuteSelectQuery(query)
            gvAsignaciones.DataSource = asignacionesData
            gvAsignaciones.DataBind()
        Catch ex As Exception
            MostrarAlerta("Error al cargar las asignaciones: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnEliminarOculto_Click(sender As Object, e As EventArgs)
        Try
            Dim animalId As Integer = Convert.ToInt32(hfAnimalIdEliminar.Value)
            Dim categoriaId As Integer = Convert.ToInt32(hfCategoriaIdEliminar.Value)
            EliminarAsignacion(animalId, categoriaId)
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la asignación: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If ValidarFormulario() Then
                Dim animalId As Integer = Convert.ToInt32(ddlAnimal.SelectedValue)
                Dim categoriaId As Integer = Convert.ToInt32(ddlCategoria.SelectedValue)

                ' Verificar si ya existe la asignación
                If ExisteAsignacion(animalId, categoriaId) Then
                    MostrarAlerta("Esta categoría ya está asignada a este animal", "warning")
                    Return
                End If

                Dim result As Integer = InsertarAsignacion(animalId, categoriaId)
                
                If result > 0 Then
                    MostrarAlerta("Categoría asignada correctamente", "success")
                    CargarAsignaciones()
                    asignacionModal.Style("display") = "none"
                    LimpiarFormulario()
                Else
                    MostrarAlerta("Error al asignar la categoría", "danger")
                End If
            End If
        Catch ex As Exception
            MostrarAlerta("Error al guardar la asignación: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub gvAsignaciones_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim args As String() = e.CommandArgument.ToString().Split(",")
            
            Select Case e.CommandName
                Case "EliminarAsignacion"
                    If args.Length = 2 Then
                        Dim animalId As Integer = Convert.ToInt32(args(0))
                        Dim categoriaId As Integer = Convert.ToInt32(args(1))
                        EliminarAsignacion(animalId, categoriaId)
                    End If
            End Select
        Catch ex As Exception
            MostrarAlerta("Error al procesar la acción: " & ex.Message, "danger")
        End Try
    End Sub

    Private Sub EliminarAsignacion(animalId As Integer, categoriaId As Integer)
        Try
            ' Usar procedimiento almacenado para eliminar
            Dim query As String = "EXEC EliminarAnimalCategoria @ID_Animal, @ID_Categoria"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Categoria", categoriaId)
            }
            
            Dim result As Integer = DataAccess.ExecuteNonQuery(query, parameters)
            If result > 0 Then
                MostrarAlerta("Asignación eliminada correctamente", "success")
                CargarAsignaciones()
            Else
                MostrarAlerta("Error al eliminar la asignación", "danger")
            End If
        Catch ex As Exception
            MostrarAlerta("Error al eliminar la asignación: " & ex.Message, "danger")
        End Try
    End Sub

    Private Function InsertarAsignacion(animalId As Integer, categoriaId As Integer) As Integer
        Try
            ' Usar procedimiento almacenado para insertar
            Dim query As String = "EXEC InsertarAnimalCategoria @ID_Animal, @ID_Categoria"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Categoria", categoriaId)
            }
            
            Return DataAccess.ExecuteNonQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("Error al insertar la asignación: " & ex.Message)
        End Try
    End Function

    Private Function ExisteAsignacion(animalId As Integer, categoriaId As Integer) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM Animal_Categoria WHERE ID_Animal = @ID_Animal AND ID_Categoria = @ID_Categoria"
            Dim parameters() As SqlParameter = {
                New SqlParameter("@ID_Animal", animalId),
                New SqlParameter("@ID_Categoria", categoriaId)
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

        If ddlCategoria.SelectedValue = "0" Then
            MostrarAlerta("Debe seleccionar una categoría", "warning")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        If ddlAnimal.Items.Count > 0 Then
            ddlAnimal.SelectedIndex = 0
        End If
        If ddlCategoria.Items.Count > 0 Then
            ddlCategoria.SelectedIndex = 0
        End If
        modalTitle.InnerText = "Nueva Asignación de Categoría"
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

