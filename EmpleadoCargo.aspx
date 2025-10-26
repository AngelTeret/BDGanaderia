<%@ Page Language="VB" AutoEventWireup="true" CodeFile="EmpleadoCargo.aspx.vb" Inherits="EmpleadoCargo" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Asignaciones de Empleados - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Asignaciones de Cargos</h1>
        <p class="page-subtitle">Registra los cargos asignados a empleados</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevaAsignacion" runat="server" Text="Nueva Asignación" 
                CssClass="btn btn-primary" OnClientClick="openNewAsignacionModal(); return false;" />
            <a href="#" class="btn btn-secondary">
                <i class="fas fa-download"></i>
                Exportar
            </a>
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Asignaciones de Cargos</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los cargos asignados a empleados</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfEmpleadoIdEliminar" runat="server" />
        <asp:HiddenField ID="hfCargoIdEliminar" runat="server" />
        <asp:HiddenField ID="hfFechaAsignacionEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de asignaciones -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvAsignaciones" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvAsignaciones_RowCommand"
                EmptyDataText="No hay asignaciones registradas" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="Nombre_Empleado" HeaderText="Empleado" />
                    <asp:BoundField DataField="Nombre_Cargo" HeaderText="Cargo" />
                    <asp:BoundField DataField="Fecha_Asignacion" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarAsignacion" 
                                CommandArgument='<%# Eval("ID_Empleado").ToString() + "," + Eval("ID_Cargo").ToString() + "," + Eval("Fecha_Asignacion", "{0:yyyy-MM-dd}") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="confirmDeleteAsignacion(this); return false;" 
                                data-empleado-id='<%# Eval("ID_Empleado") %>'
                                data-cargo-id='<%# Eval("ID_Cargo") %>'
                                data-fecha-asignacion='<%# Eval("Fecha_Asignacion", "{0:yyyy-MM-dd}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar asignación -->
    <div id="asignacionModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nueva Asignación de Cargo</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                
                <div class="form-group">
                    <label class="form-label">Empleado</label>
                    <asp:DropDownList ID="ddlEmpleado" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Cargo</label>
                    <asp:DropDownList ID="ddlCargo" runat="server" CssClass="form-control" Enabled="true">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Fecha de Asignación</label>
                    <asp:TextBox ID="txtFechaAsignacion" runat="server" CssClass="form-control" 
                        TextMode="Date" Enabled="true"></asp:TextBox>
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
    <script src="Scripts/empleado-cargo-page.js"></script>
</asp:Content>

