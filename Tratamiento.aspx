<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Tratamiento.aspx.vb" Inherits="Tratamiento" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Tratamientos - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Tratamientos</h1>
        <p class="page-subtitle">Administra los tratamientos veterinarios de tu explotación ganadera</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoTratamiento" runat="server" Text="Nuevo Tratamiento" 
                CssClass="btn btn-primary" OnClientClick="openNewTratamientoModal(); return false;" />
            <a href="#" class="btn btn-secondary">
                <i class="fas fa-download"></i>
                Exportar
            </a>
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Tratamientos</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los tratamientos registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfTratamientoIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de tratamientos -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvTratamientos" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvTratamientos_RowCommand"
                EmptyDataText="No hay tratamientos registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Tratamiento" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Tratamiento" HeaderText="Nombre del Tratamiento" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarTratamiento" CommandArgument='<%# Eval("ID_Tratamiento") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarTratamiento" CommandArgument='<%# Eval("ID_Tratamiento") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteTratamiento(this);" 
                                data-command-argument='<%# Eval("ID_Tratamiento") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar tratamiento -->
    <div id="tratamientoModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Tratamiento</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfTratamientoId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre del Tratamiento</label>
                    <asp:TextBox ID="txtNombreTratamiento" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre del tratamiento"></asp:TextBox>
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
    <script src="Scripts/tratamiento-page.js"></script>
</asp:Content>
