<%@ Page Language="VB" AutoEventWireup="true" CodeFile="PeriodoProduccion.aspx.vb" Inherits="PeriodoProduccion" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Períodos de Producción - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Períodos de Producción</h1>
        <p class="page-subtitle">Administra los períodos de producción de tu explotación ganadera</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoPeriodo" runat="server" Text="Nuevo Período" 
                CssClass="btn btn-primary" OnClientClick="openNewPeriodoModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Períodos de Producción</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los períodos de producción registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfPeriodoIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de períodos de producción -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvPeriodos" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvPeriodos_RowCommand"
                EmptyDataText="No hay períodos de producción registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Periodo" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Periodo" HeaderText="Nombre del Período" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarPeriodo" CommandArgument='<%# Eval("ID_Periodo") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarPeriodo" CommandArgument='<%# Eval("ID_Periodo") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeletePeriodo(this);" 
                                data-command-argument='<%# Eval("ID_Periodo") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar período de producción -->
    <div id="periodoModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Período de Producción</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfPeriodoId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre del Período</label>
                    <asp:TextBox ID="txtNombrePeriodo" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre del período de producción"></asp:TextBox>
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
    <script src="Scripts/periodoProduccion-page.js"></script>
</asp:Content>
