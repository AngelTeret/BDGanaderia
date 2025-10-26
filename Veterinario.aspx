<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Veterinario.aspx.vb" Inherits="Veterinario" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Veterinarios - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Veterinarios</h1>
        <p class="page-subtitle">Administra los veterinarios de tu explotación ganadera</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoVeterinario" runat="server" Text="Nuevo Veterinario" 
                CssClass="btn btn-primary" OnClientClick="openNewVeterinarioModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Veterinarios</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los veterinarios registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfVeterinarioIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de veterinarios -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvVeterinarios" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvVeterinarios_RowCommand"
                EmptyDataText="No hay veterinarios registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Veterinario" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Veterinario" HeaderText="Nombre del Veterinario" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarVeterinario" CommandArgument='<%# Eval("ID_Veterinario") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarVeterinario" CommandArgument='<%# Eval("ID_Veterinario") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteVeterinario(this);" 
                                data-command-argument='<%# Eval("ID_Veterinario") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar veterinario -->
    <div id="veterinarioModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Veterinario</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfVeterinarioId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre del Veterinario</label>
                    <asp:TextBox ID="txtNombreVeterinario" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre del veterinario"></asp:TextBox>
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
    <script src="Scripts/veterinario-page.js"></script>
</asp:Content>
