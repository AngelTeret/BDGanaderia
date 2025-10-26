<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Raza.aspx.vb" Inherits="Raza" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Razas - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
            <!-- Header de la página -->
            <div class="page-header">
                <h1 class="page-title">Gestión de Razas</h1>
                <p class="page-subtitle">Administra las razas de ganado de tu explotación</p>
                <div class="page-actions">
                    <asp:Button ID="btnNuevaRaza" runat="server" Text="Nueva Raza" 
                        CssClass="btn btn-primary" OnClientClick="openNewRazaModal(); return false;" />
                    <a href="#" class="btn btn-secondary">
                        <i class="fas fa-download"></i>
                        Exportar
                    </a>
                </div>
            </div>

            <!-- Tarjeta principal -->
            <div class="stat-card">
                <div class="stat-card-header">
                    <h3 class="stat-card-title">Lista de Razas</h3>
                </div>
                <p class="stat-card-subtitle">Gestiona todas las razas de ganado registradas en el sistema</p>
                
                <!-- Botón oculto para eliminación -->
                <asp:HiddenField ID="hfRazaIdEliminar" runat="server" />
                <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
                    CssClass="btn btn-danger" style="display:none;" 
                    OnClick="btnEliminarOculto_Click" />

                <!-- Tabla de razas -->
                <div class="table-container" style="margin-top: 20px;">
                    <asp:GridView ID="gvRazas" runat="server" CssClass="table" 
                        AutoGenerateColumns="False" OnRowCommand="gvRazas_RowCommand"
                        EmptyDataText="No hay razas registradas" EnableViewState="True"
                        style="width: 100%; border-collapse: collapse;">
                        <Columns>
                            <asp:BoundField DataField="ID_Raza" HeaderText="ID" />
                            <asp:BoundField DataField="Nombre_Raza" HeaderText="Nombre de la Raza" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                        CommandName="EditarRaza" CommandArgument='<%# Eval("ID_Raza") %>'
                                        CssClass="btn btn-warning" />
                                    <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                        CommandName="EliminarRaza" CommandArgument='<%# Eval("ID_Raza") %>'
                                        CssClass="btn btn-danger"
                                        OnClientClick="return confirmDeleteRaza(this);" 
                                        data-command-argument='<%# Eval("ID_Raza") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        <!-- Modal para agregar/editar raza -->
        <div id="razaModal" class="modal" runat="server">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 id="modalTitle" runat="server">Nueva Raza</h3>
                    <span class="close" onclick="closeModal()">&times;</span>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfRazaId" runat="server" />
                    
                    <div class="form-group">
                        <label class="form-label">Nombre de la Raza</label>
                        <asp:TextBox ID="txtNombreRaza" runat="server" CssClass="form-control" 
                            placeholder="Ingrese el nombre de la raza"></asp:TextBox>
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
    <script src="Scripts/raza-page.js"></script>
</asp:Content>
