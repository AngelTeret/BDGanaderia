<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Racion.aspx.vb" Inherits="Racion" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Raciones - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Raciones</h1>
        <p class="page-subtitle">Administra las raciones de alimentación de tu explotación ganadera</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevaRacion" runat="server" Text="Nueva Ración" 
                CssClass="btn btn-primary" OnClientClick="openNewRacionModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Raciones</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todas las raciones registradas en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfRacionIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de raciones -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvRaciones" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvRaciones_RowCommand"
                EmptyDataText="No hay raciones registradas" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Racion" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Racion" HeaderText="Nombre de la Ración" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarRacion" CommandArgument='<%# Eval("ID_Racion") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarRacion" CommandArgument='<%# Eval("ID_Racion") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteRacion(this);" 
                                data-command-argument='<%# Eval("ID_Racion") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar ración -->
    <div id="racionModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nueva Ración</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfRacionId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre de la Ración</label>
                    <asp:TextBox ID="txtNombreRacion" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre de la ración"></asp:TextBox>
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
    <script src="Scripts/racion-page.js"></script>
</asp:Content>
