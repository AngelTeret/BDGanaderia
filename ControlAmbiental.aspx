<%@ Page Language="VB" AutoEventWireup="true" CodeFile="ControlAmbiental.aspx.vb" Inherits="ControlAmbiental" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Control Ambiental - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Control Ambiental</h1>
        <p class="page-subtitle">Registra y administra los controles ambientales de los potreros</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoControl" runat="server" Text="Nuevo Control" 
                CssClass="btn btn-primary" OnClientClick="openNewControlModal(); return false;" />
            <a href="#" class="btn btn-secondary">
                <i class="fas fa-download"></i>
                Exportar
            </a>
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Controles Ambientales</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los controles ambientales de los potreros</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfControlIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de controles -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvControles" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvControles_RowCommand"
                EmptyDataText="No hay controles ambientales registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_ControlAmb" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Potrero" HeaderText="Potrero" />
                    <asp:BoundField DataField="Fecha_Control" HeaderText="Fecha de Control" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Observacion" HeaderText="Observación" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarControl" CommandArgument='<%# Eval("ID_ControlAmb") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarControl" CommandArgument='<%# Eval("ID_ControlAmb") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteControl(this);" 
                                data-command-argument='<%# Eval("ID_ControlAmb") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar control -->
    <div id="controlModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Control Ambiental</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfControlId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Potrero</label>
                    <asp:DropDownList ID="ddlPotrero" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Fecha de Control</label>
                    <asp:TextBox ID="txtFechaControl" runat="server" CssClass="form-control" 
                        TextMode="Date"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Observación</label>
                    <asp:TextBox ID="txtObservacion" runat="server" CssClass="form-control" 
                        placeholder="Ejemplo: Condiciones climáticas, estado del pasto, etc." TextMode="MultiLine" Rows="3"></asp:TextBox>
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
    <script src="Scripts/control-ambiental-page.js"></script>
</asp:Content>

