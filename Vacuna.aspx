<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Vacuna.aspx.vb" Inherits="Vacuna" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Vacunas - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Vacunas</h1>
        <p class="page-subtitle">Administra las vacunas del ganado de tu explotación</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevaVacuna" runat="server" Text="Nueva Vacuna" 
                CssClass="btn btn-primary" OnClientClick="openNewVacunaModal(); return false;" />
            <a href="#" class="btn btn-secondary">
                <i class="fas fa-download"></i>
                Exportar
            </a>
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Vacunas</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todas las vacunas registradas en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfVacunaIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de vacunas -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvVacunas" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvVacunas_RowCommand"
                EmptyDataText="No hay vacunas registradas" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Vacuna" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Vacuna" HeaderText="Nombre de la Vacuna" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarVacuna" CommandArgument='<%# Eval("ID_Vacuna") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarVacuna" CommandArgument='<%# Eval("ID_Vacuna") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteVacuna(this);" 
                                data-command-argument='<%# Eval("ID_Vacuna") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar vacuna -->
    <div id="vacunaModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nueva Vacuna</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfVacunaId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre de la Vacuna</label>
                    <asp:TextBox ID="txtNombreVacuna" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre de la vacuna"></asp:TextBox>
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
    <script src="Scripts/vacuna-page.js"></script>
</asp:Content>
