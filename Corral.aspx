<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Corral.aspx.vb" Inherits="Corral" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Corrales - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Corrales</h1>
        <p class="page-subtitle">Administra los corrales del ganado</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoCorral" runat="server" Text="Nuevo Corral" 
                CssClass="btn btn-primary" OnClientClick="openNewCorralModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Corrales</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los corrales registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfCorralIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de corrales -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvCorrales" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvCorrales_RowCommand"
                EmptyDataText="No hay corrales registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Corral" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Corral" HeaderText="Nombre del Corral" />
                    <asp:BoundField DataField="Capacidad" HeaderText="Capacidad" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarCorral" CommandArgument='<%# Eval("ID_Corral") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarCorral" CommandArgument='<%# Eval("ID_Corral") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteCorral(this);" 
                                data-command-argument='<%# Eval("ID_Corral") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar corral -->
    <div id="corralModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Corral</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfCorralId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre del Corral</label>
                    <asp:TextBox ID="txtNombreCorral" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre del corral"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Capacidad</label>
                    <asp:TextBox ID="txtCapacidad" runat="server" CssClass="form-control" 
                        placeholder="Ejemplo: 50" TextMode="Number"></asp:TextBox>
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
    <script src="Scripts/corral-page.js"></script>
</asp:Content>

