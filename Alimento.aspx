<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Alimento.aspx.vb" Inherits="Alimento" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Alimentos - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Alimentos</h1>
        <p class="page-subtitle">Administra los alimentos de tu explotación ganadera</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoAlimento" runat="server" Text="Nuevo Alimento" 
                CssClass="btn btn-primary" OnClientClick="openNewAlimentoModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Alimentos</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los alimentos registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfAlimentoIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de alimentos -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvAlimentos" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvAlimentos_RowCommand"
                EmptyDataText="No hay alimentos registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Alimento" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Alimento" HeaderText="Nombre del Alimento" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarAlimento" CommandArgument='<%# Eval("ID_Alimento") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarAlimento" CommandArgument='<%# Eval("ID_Alimento") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteAlimento(this);" 
                                data-command-argument='<%# Eval("ID_Alimento") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar alimento -->
    <div id="alimentoModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Alimento</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfAlimentoId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre del Alimento</label>
                    <asp:TextBox ID="txtNombreAlimento" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre del alimento"></asp:TextBox>
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
    <script src="Scripts/alimento-page.js"></script>
</asp:Content>
