<%@ Page Language="VB" AutoEventWireup="true" CodeFile="TipoPasto.aspx.vb" Inherits="TipoPasto" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Tipos de Pasto - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Tipos de Pasto</h1>
        <p class="page-subtitle">Administra los tipos de pasto del sistema</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoTipoPasto" runat="server" Text="Nuevo Tipo de Pasto" 
                CssClass="btn btn-primary" OnClientClick="openNewTipoPastoModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Tipos de Pasto</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los tipos de pasto registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfTipoPastoIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de tipos de pasto -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvTipoPastos" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvTipoPastos_RowCommand"
                EmptyDataText="No hay tipos de pasto registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_TipoPasto" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Pasto" HeaderText="Nombre del Pasto" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarTipoPasto" CommandArgument='<%# Eval("ID_TipoPasto") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarTipoPasto" CommandArgument='<%# Eval("ID_TipoPasto") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteTipoPasto(this);" 
                                data-command-argument='<%# Eval("ID_TipoPasto") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar tipo de pasto -->
    <div id="tipoPastoModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Tipo de Pasto</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfTipoPastoId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre del Pasto</label>
                    <asp:TextBox ID="txtNombrePasto" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre del pasto"></asp:TextBox>
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
    <script src="Scripts/tipo-pasto-page.js"></script>
</asp:Content>

