<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Empleado.aspx.vb" Inherits="Empleado" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Empleados - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Empleados</h1>
        <p class="page-subtitle">Administra el personal del sistema</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoEmpleado" runat="server" Text="Nuevo Empleado" 
                CssClass="btn btn-primary" OnClientClick="openNewEmpleadoModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Empleados</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los empleados registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfEmpleadoIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de empleados -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvEmpleados" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvEmpleados_RowCommand"
                EmptyDataText="No hay empleados registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Empleado" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Empleado" HeaderText="Nombre del Empleado" />
                    <asp:BoundField DataField="Fecha_Contratacion" HeaderText="Fecha de Contratación" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarEmpleado" CommandArgument='<%# Eval("ID_Empleado") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarEmpleado" CommandArgument='<%# Eval("ID_Empleado") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteEmpleado(this);" 
                                data-command-argument='<%# Eval("ID_Empleado") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar empleado -->
    <div id="empleadoModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Empleado</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfEmpleadoId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre del Empleado</label>
                    <asp:TextBox ID="txtNombreEmpleado" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre del empleado"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Fecha de Contratación</label>
                    <asp:TextBox ID="txtFechaContratacion" runat="server" CssClass="form-control" 
                        TextMode="Date"></asp:TextBox>
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
    <script src="Scripts/empleado-page.js"></script>
</asp:Content>

