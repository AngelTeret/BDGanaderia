<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Cargo.aspx.vb" Inherits="Cargo" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Cargos - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Cargos</h1>
        <p class="page-subtitle">Administra los cargos de los empleados</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoCargo" runat="server" Text="Nuevo Cargo" 
                CssClass="btn btn-primary" OnClientClick="openNewCargoModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Cargos</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los cargos registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfCargoIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de cargos -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvCargos" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvCargos_RowCommand"
                EmptyDataText="No hay cargos registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Cargo" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Cargo" HeaderText="Nombre del Cargo" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarCargo" CommandArgument='<%# Eval("ID_Cargo") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarCargo" CommandArgument='<%# Eval("ID_Cargo") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteCargo(this);" 
                                data-command-argument='<%# Eval("ID_Cargo") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar cargo -->
    <div id="cargoModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Cargo</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfCargoId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre del Cargo</label>
                    <asp:TextBox ID="txtNombreCargo" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre del cargo"></asp:TextBox>
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
    <script src="Scripts/cargo-page.js"></script>
</asp:Content>

