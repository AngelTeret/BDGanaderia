<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Rol.aspx.vb" Inherits="Rol" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Roles - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Roles</h1>
        <p class="page-subtitle">Administra los roles del sistema</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoRol" runat="server" Text="Nuevo Rol" 
                CssClass="btn btn-primary" OnClientClick="openNewRolModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Roles</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los roles registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfRolIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de roles -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvRoles" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvRoles_RowCommand"
                EmptyDataText="No hay roles registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Rol" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Rol" HeaderText="Nombre del Rol" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarRol" CommandArgument='<%# Eval("ID_Rol") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarRol" CommandArgument='<%# Eval("ID_Rol") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteRol(this);" 
                                data-command-argument='<%# Eval("ID_Rol") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar rol -->
    <div id="rolModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Rol</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfRolId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre del Rol</label>
                    <asp:TextBox ID="txtNombreRol" runat="server" CssClass="form-control" 
                        placeholder="Ejemplo: Administrador, Usuario, etc."></asp:TextBox>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" 
                    CssClass="btn btn-success" OnClick="btnGuardar_Click" />
                <button type="button" class="btn btn-secondary" onclick="closeModal(); return false;">
                    Cancelar
                </button>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="ScriptsContent" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="Scripts/rol-page.js"></script>
</asp:Content>

