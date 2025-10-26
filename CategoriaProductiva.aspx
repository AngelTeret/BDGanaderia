<%@ Page Language="VB" AutoEventWireup="true" CodeFile="CategoriaProductiva.aspx.vb" Inherits="CategoriaProductiva" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Categorías Productivas - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Categorías Productivas</h1>
        <p class="page-subtitle">Administra las categorías productivas del ganado</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevaCategoria" runat="server" Text="Nueva Categoría" 
                CssClass="btn btn-primary" OnClientClick="openNewCategoriaModal(); return false;" />
            <a href="#" class="btn btn-secondary">
                <i class="fas fa-download"></i>
                Exportar
            </a>
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Categorías Productivas</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todas las categorías productivas registradas en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfCategoriaIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de categorías productivas -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvCategorias" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvCategorias_RowCommand"
                EmptyDataText="No hay categorías productivas registradas" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Categoria" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Categoria" HeaderText="Nombre de la Categoría" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarCategoria" CommandArgument='<%# Eval("ID_Categoria") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarCategoria" CommandArgument='<%# Eval("ID_Categoria") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteCategoria(this);" 
                                data-command-argument='<%# Eval("ID_Categoria") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar categoría productiva -->
    <div id="categoriaModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nueva Categoría Productiva</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfCategoriaId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre de la Categoría</label>
                    <asp:TextBox ID="txtNombreCategoria" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre de la categoría productiva"></asp:TextBox>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" 
                    CssClass="btn btn-success" OnClick="btnGuardar_Click" />
                <button type="button" class="btn btn-secondary" onclick="closeModal()">
                    Cancelar
                </button>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="ScriptsContent" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="Scripts/CategoriaProductiva-page.js"></script>
</asp:Content>
