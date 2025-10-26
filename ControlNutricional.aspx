<%@ Page Language="VB" AutoEventWireup="true" CodeFile="ControlNutricional.aspx.vb" Inherits="ControlNutricional" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Control Nutricional - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Control Nutricional</h1>
        <p class="page-subtitle">Registra y administra los controles nutricionales de los animales</p>
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
            <h3 class="stat-card-title">Lista de Controles Nutricionales</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los controles nutricionales del ganado</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfControlIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de controles -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvControles" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvControles_RowCommand"
                EmptyDataText="No hay controles nutricionales registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_ControlNutri" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Animal" HeaderText="Animal" />
                    <asp:BoundField DataField="Fecha_Evaluacion" HeaderText="Fecha de Evaluación" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Condicion_Corporal" HeaderText="Condición Corporal" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarControl" CommandArgument='<%# Eval("ID_ControlNutri") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarControl" CommandArgument='<%# Eval("ID_ControlNutri") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteControl(this);" 
                                data-command-argument='<%# Eval("ID_ControlNutri") %>' />
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
                <h3 id="modalTitle" runat="server">Nuevo Control Nutricional</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfControlId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Animal</label>
                    <asp:DropDownList ID="ddlAnimal" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Fecha de Evaluación</label>
                    <asp:TextBox ID="txtFechaEvaluacion" runat="server" CssClass="form-control" 
                        TextMode="Date"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Condición Corporal</label>
                    <asp:TextBox ID="txtCondicionCorporal" runat="server" CssClass="form-control" 
                        placeholder="Ejemplo: Buena, Regular, Mala, Excelente"></asp:TextBox>
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
    <script src="Scripts/control-nutricional-page.js"></script>
</asp:Content>

