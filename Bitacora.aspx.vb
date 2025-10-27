Imports System.Data.SqlClient
Imports System.Data

''' <summary>
''' Página para ver la bitácora del sistema
''' </summary>
Partial Public Class Bitacora
    Inherits System.Web.UI.Page
    
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadBitacora()
        End If
    End Sub
    
    ''' <summary>Carga los registros de bitácora</summary>
    Private Sub LoadBitacora()
        Try
            Using connection As New SqlConnection(ConfigurationManager.ConnectionStrings("BDGanaderiaConnectionString").ConnectionString)
                connection.Open()
                
                Dim query As String = "SELECT TOP 100 * FROM Bitacora_Evento WHERE 1=1"
                Dim params As New List(Of String)
                
                ' Filtro por tipo de evento
                If Not String.IsNullOrEmpty(ddlTipoEvento.SelectedValue) Then
                    query += " AND Tipo_Evento = @TipoEvento"
                End If
                
                ' Filtro por entidad
                If Not String.IsNullOrEmpty(txtEntidad.Text) Then
                    query += " AND Entidad LIKE @Entidad"
                End If
                
                query += " ORDER BY Fecha_Hora DESC"
                
                Using cmd As New SqlCommand(query, connection)
                    If Not String.IsNullOrEmpty(ddlTipoEvento.SelectedValue) Then
                        cmd.Parameters.AddWithValue("@TipoEvento", ddlTipoEvento.SelectedValue)
                    End If
                    If Not String.IsNullOrEmpty(txtEntidad.Text) Then
                        cmd.Parameters.AddWithValue("@Entidad", "%" & txtEntidad.Text & "%")
                    End If
                    
                    Using adapter As New SqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        adapter.Fill(dt)
                        gvBitacora.DataSource = dt
                        gvBitacora.DataBind()
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ' Manejar error silenciosamente
        End Try
    End Sub
    
    ''' <summary>Filtra los registros</summary>
    Protected Sub btnFiltrar_Click(sender As Object, e As EventArgs)
        LoadBitacora()
    End Sub
    
End Class

