<%@ Page Language="VB" AutoEventWireup="true" CodeFile="AnimalCategoria.aspx.vb" Inherits="AnimalCategoria" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Categorías de Animales - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Categorías de Animales</h1>
        <p class="page-subtitle">Asigna categorías productivas a los animales</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevaAsignacion" runat="server" Text="Nueva Asignación" 
                CssClass="btn btn-primary" OnClientClick="openNewAsignacionModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Asignaciones de Categorías</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todas las categorías asignadas a los animales</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfAnimalIdEliminar" runat="server" />
        <asp:HiddenField ID="hfCategoriaIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de asignaciones -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvAsignaciones" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvAsignaciones_RowCommand"
                EmptyDataText="No hay categorías asignadas" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="Nombre_Animal" HeaderText="Animal" />
                    <asp:BoundField DataField="Nombre_Categoria" HeaderText="Categoría Productiva" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarAsignacion" 
                                CommandArgument='<%# Eval("ID_Animal").ToString() + "," + Eval("ID_Categoria").ToString() %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteAsignacion(this);" 
                                data-animal-id='<%# Eval("ID_Animal") %>'
                                data-categoria-id='<%# Eval("ID_Categoria") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar asignación de categoría -->
    <div id="asignacionModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nueva Asignación de Categoría</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                
                <div class="form-group">
                    <label class="form-label">Animal</label>
                    <asp:DropDownList ID="ddlAnimal" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Categoría Productiva</label>
                    <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-control">
                    </asp:DropDownList>
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
    <script src="Scripts/animal-categoria-page.js"></script>
</asp:Content>

