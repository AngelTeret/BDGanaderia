<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Medicamento.aspx.vb" Inherits="Medicamento" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Medicamentos - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Medicamentos</h1>
        <p class="page-subtitle">Administra los medicamentos veterinarios de tu explotación ganadera</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoMedicamento" runat="server" Text="Nuevo Medicamento" 
                CssClass="btn btn-primary" OnClientClick="openNewMedicamentoModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Medicamentos</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los medicamentos registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfMedicamentoIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de medicamentos -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvMedicamentos" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvMedicamentos_RowCommand"
                EmptyDataText="No hay medicamentos registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Medicamento" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Medicamento" HeaderText="Nombre del Medicamento" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarMedicamento" CommandArgument='<%# Eval("ID_Medicamento") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarMedicamento" CommandArgument='<%# Eval("ID_Medicamento") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteMedicamento(this);" 
                                data-command-argument='<%# Eval("ID_Medicamento") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar medicamento -->
    <div id="medicamentoModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Medicamento</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfMedicamentoId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre del Medicamento</label>
                    <asp:TextBox ID="txtNombreMedicamento" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre del medicamento"></asp:TextBox>
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
    <script src="Scripts/medicamento-page.js"></script>
</asp:Content>
