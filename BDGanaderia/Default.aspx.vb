Imports System.Data
Imports System.Data.SqlClient

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarEstadisticas()
        End If
    End Sub

    Private Sub CargarEstadisticas()
        Try
            ' Verificar si la tabla Raza existe
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Raza'"
            Dim tableExists As Object = DataAccess.ExecuteScalar(checkTableQuery)
            
            If Convert.ToInt32(tableExists) > 0 Then
                ' Contar razas registradas
                Dim countRazasQuery As String = "SELECT COUNT(*) FROM Raza"
                Dim razasCount As Object = DataAccess.ExecuteScalar(countRazasQuery)
                
                ' Actualizar el número de razas en el frontend
                Dim script As String = "document.getElementById('razasCount').textContent = '" & razasCount.ToString() & "';"
                ClientScript.RegisterStartupScript(Me.GetType(), "UpdateRazasCount", script, True)
            End If
        Catch ex As Exception
            ' En caso de error, mantener los valores por defecto
        End Try
    End Sub
End Class
