<%@ Page Title="Bitácora" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeFile="Bitacora.aspx.vb" Inherits="Bitacora" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Bitácora - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Bitácora del Sistema</h1>
        <p class="page-subtitle">Registro de eventos y actividades realizadas</p>
    </div>

    <!-- Filtros -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Filtros</h3>
        </div>
        
        <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 20px; margin-top: 20px;">
            <div class="form-group">
                <label class="form-label">Tipo de Evento</label>
                <asp:DropDownList ID="ddlTipoEvento" runat="server" CssClass="form-control">
                    <asp:ListItem Value="" Text="Todos"></asp:ListItem>
                    <asp:ListItem Value="LOGIN">Login</asp:ListItem>
                    <asp:ListItem Value="LOGOUT">Logout</asp:ListItem>
                    <asp:ListItem Value="CREATE">Crear</asp:ListItem>
                    <asp:ListItem Value="UPDATE">Actualizar</asp:ListItem>
                    <asp:ListItem Value="DELETE">Eliminar</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <label class="form-label">Entidad</label>
                <asp:TextBox ID="txtEntidad" runat="server" CssClass="form-control" placeholder="Animal, Raza, etc."></asp:TextBox>
            </div>
            <div class="form-group" style="align-self: end;">
                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClick="btnFiltrar_Click" />
            </div>
        </div>
    </div>

    <!-- Tabla de bitácora -->
    <div class="stat-card" style="margin-top: 20px;">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Registros de Bitácora</h3>
        </div>
        
        <div class="table-container" style="margin-top: 20px; overflow: auto;">
            <asp:GridView ID="gvBitacora" runat="server" CssClass="table" 
                AutoGenerateColumns="False"
                EmptyDataText="No hay registros en la bitácora">
                <Columns>
                    <asp:BoundField DataField="Fecha_Hora" HeaderText="Fecha/Hora" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                    <asp:BoundField DataField="Nombre_Usuario" HeaderText="Usuario" />
                    <asp:BoundField DataField="Tipo_Evento" HeaderText="Tipo de Evento" />
                    <asp:BoundField DataField="Entidad" HeaderText="Entidad" />
                    <asp:BoundField DataField="ID_Registro" HeaderText="ID Registro" />
                    <asp:BoundField DataField="Mensaje" HeaderText="Mensaje" />
                    <asp:BoundField DataField="IP_Usuario" HeaderText="IP" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

