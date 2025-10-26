Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Linq

Public Partial Class Controls_Sidebar
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        RenderMenu()
    End Sub

    Private Sub RenderMenu()
        Try
            ' Obtener menú agrupado desde configuración
            Dim menuGroups As Dictionary(Of String, MenuGroup) = MenuItemDataAccess.GetMenuItemsGrouped()
            
            ' Obtener URL actual para marcar activo
            Dim currentPage As String = Request.AppRelativeCurrentExecutionFilePath.ToLower()
            Dim fileName As String = currentPage.Replace("~/", "").ToLower()
            
            ' Renderizar Dashboard primero (fuera del accordion)
            If menuGroups.ContainsKey("principal") Then
                RenderDashboardOnly(menuGroups("principal").Items.First(), fileName)
            End If
            
            ' Obtener clave del grupo que contiene el ítem activo (para abrirlo)
            Dim activeGroupKey As String = GetActiveGroupKey(fileName, menuGroups)
            
            ' Renderizar grupos con accordion (excluyendo principal)
            Dim sortedGroups = From g In menuGroups.Values
                              Where g.GroupKey <> "principal"
                              Let minOrder = If(g.Items.Any(), g.Items.Min(Function(i) i.ItemOrder), 0)
                              Order By minOrder
                              Select g
            
            For Each group In sortedGroups
                RenderNavSection(group, fileName, activeGroupKey)
            Next
        Catch ex As Exception
            ' Si hay error, renderizar menú estático por defecto
            RenderDefaultMenu()
        End Try
    End Sub
    
    Private Sub RenderDashboardOnly(menuItem As MenuItem, currentFileName As String)
        ' Renderizar Dashboard como sección simple sin accordion
        Dim navSection As New HtmlGenericControl("div")
        navSection.Attributes("class") = "nav-section"
        
        Dim itemFileName As String = menuItem.ItemUrl.ToLower().Replace("~/", "")
        Dim isActive As Boolean = (itemFileName = currentFileName)
        
        ' UL directo sin título de sección
        Dim navMenu As New HtmlGenericControl("ul")
        navMenu.Attributes("class") = "nav-menu"
        
        Dim li As New HtmlGenericControl("li")
        li.Attributes("class") = "nav-item"
        
        Dim anchor As New HtmlGenericControl("a")
        anchor.Attributes("href") = menuItem.ItemUrl
        anchor.Attributes("class") = "nav-link" & If(isActive, " active", "")
        
        Dim icon As New LiteralControl("<i class=""" & menuItem.IconCss & " nav-icon""></i>")
        Dim text As New LiteralControl("<span class=""nav-text"">" & menuItem.ItemText & "</span>")
        
        anchor.Controls.Add(icon)
        anchor.Controls.Add(text)
        li.Controls.Add(anchor)
        navMenu.Controls.Add(li)
        navSection.Controls.Add(navMenu)
        sidebarNav.Controls.Add(navSection)
    End Sub

    Private Sub RenderNavSection(group As MenuGroup, currentFileName As String, activeGroupKey As String)
        ' Determinar si este grupo debe estar abierto
        Dim isOpen As Boolean = (group.GroupKey = activeGroupKey)
        
        ' Crear la sección de navegación
        Dim navSection As New HtmlGenericControl("div")
        navSection.Attributes("class") = "nav-section"
        navSection.Attributes("data-accordion-group") = group.GroupKey
        
        ' Título de la sección (ahora es clicable para accordion)
        Dim title As New HtmlGenericControl("div")
        title.Attributes("class") = "nav-section-title" & If(isOpen, " is-open", "")
        title.Attributes("role") = "button"
        title.Attributes("tabindex") = "0"
        title.Attributes("aria-expanded") = If(isOpen, "true", "false")
        title.Attributes("aria-controls") = "nav-panel-" & group.GroupKey
        title.Attributes("data-accordion-toggle") = group.GroupKey
        title.InnerText = group.GroupTitle
        
        ' Agregar ícono de estado (chevron)
        Dim chevron As New LiteralControl(" <i class=""fas fa-chevron-down nav-arrow"" style=""margin-left: auto;""></i>")
        title.Controls.Add(chevron)
        
        navSection.Controls.Add(title)
        
        ' Contenedor del panel de ítems
        Dim panel As New HtmlGenericControl("ul")
        panel.ID = "nav-panel-" & group.GroupKey
        panel.Attributes("class") = "nav-menu"
        panel.Attributes("aria-hidden") = If(isOpen, "false", "true")
        
        If Not isOpen Then
            panel.Style("display") = "none"
        End If
        
        ' Renderizar ítems del menú
        For Each menuItem In group.Items.OrderBy(Function(i) i.ItemOrder)
            RenderMenuItem(panel, menuItem, currentFileName)
        Next
        
        navSection.Controls.Add(panel)
        
        ' Agregar al contenedor principal
        sidebarNav.Controls.Add(navSection)
    End Sub

    Private Sub RenderMenuItem(container As HtmlGenericControl, menuItem As MenuItem, currentFileName As String)
        ' Extraer nombre de archivo de la URL del ítem
        Dim itemFileName As String = menuItem.ItemUrl.ToLower().Replace("~/", "")
        
        ' Determinar si este ítem está activo
        Dim isActive As Boolean = (itemFileName = currentFileName)
        
        ' Crear <li>
        Dim li As New HtmlGenericControl("li")
        li.Attributes("class") = "nav-item"
        
        ' Crear <a> - usar URL directamente sin ResolveUrl para evitar problemas de rutas
        Dim anchor As New HtmlGenericControl("a")
        anchor.Attributes("href") = menuItem.ItemUrl
        anchor.Attributes("class") = "nav-link" & If(isActive, " active", "")
        
        ' Ícono
        If Not String.IsNullOrEmpty(menuItem.IconCss) Then
            Dim icon As New LiteralControl("<i class=""" & menuItem.IconCss & " nav-icon""></i>")
            anchor.Controls.Add(icon)
        End If
        
        ' Texto
        Dim text As New LiteralControl("<span class=""nav-text"">" & menuItem.ItemText & "</span>")
        anchor.Controls.Add(text)
        
        li.Controls.Add(anchor)
        container.Controls.Add(li)
    End Sub

    Private Function GetActiveGroupKey(fileName As String, menuGroups As Dictionary(Of String, MenuGroup)) As String
        For Each group In menuGroups.Values
            For Each item In group.Items
                Dim itemFileName As String = item.ItemUrl.ToLower().Replace("~/", "")
                If itemFileName = fileName Then
                    Return group.GroupKey
                End If
            Next
        Next
        Return String.Empty
    End Function

    Private Sub RenderDefaultMenu()
        ' Fallback si no hay datos en BD
        Dim defaultSection As New LiteralControl(
            "<div class=""nav-section"">" &
            "<div class=""nav-section-title"">Principal</div>" &
            "<ul class=""nav-menu"">" &
            "<li class=""nav-item"">" &
            "<a href=""Default.aspx"" class=""nav-link"">" &
            "<i class=""fas fa-home nav-icon""></i>" &
            "<span class=""nav-text"">Dashboard</span>" &
            "</a></li></ul></div>")
        sidebarNav.Controls.Add(defaultSection)
    End Sub
End Class

