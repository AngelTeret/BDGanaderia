Imports System.Web
Imports System.Web.UI

Public Partial Class SiteMaster
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' La lógica del sidebar (marcar ítems activos) ahora está en Sidebar.ascx.vb
        ' No necesitamos hacer nada aquí
    End Sub
End Class

