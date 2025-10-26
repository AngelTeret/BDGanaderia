<%@ Page Language="VB" AutoEventWireup="true" CodeFile="TratamientoMedicamento.aspx.vb" Inherits="TratamientoMedicamento" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Medicamentos por Tratamiento - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Medicamentos por Tratamiento</h1>
        <p class="page-subtitle">Define qué medicamentos contiene cada tratamiento y sus dosis</p>
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
            <h3 class="stat-card-title">Lista de Medicamentos por Tratamiento</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los medicamentos y sus dosis en cada tratamiento</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfTratamientoIdEliminar" runat="server" />
        <asp:HiddenField ID="hfMedicamentoIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de asignaciones -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvAsignaciones" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvAsignaciones_RowCommand"
                EmptyDataText="No hay medicamentos asignados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="Nombre_Tratamiento" HeaderText="Tratamiento" />
                    <asp:BoundField DataField="Nombre_Medicamento" HeaderText="Medicamento" />
                    <asp:BoundField DataField="Dosis" HeaderText="Dosis" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarAsignacion" 
                                CommandArgument='<%# Eval("ID_Tratamiento").ToString() + "," + Eval("ID_Medicamento").ToString() %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarAsignacion" 
                                CommandArgument='<%# Eval("ID_Tratamiento").ToString() + "," + Eval("ID_Medicamento").ToString() %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteAsignacion(this);" 
                                data-tratamiento-id='<%# Eval("ID_Tratamiento") %>'
                                data-medicamento-id='<%# Eval("ID_Medicamento") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar asignación de medicamento -->
    <div id="asignacionModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nueva Asignación de Medicamento</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfTratamientoId" runat="server" />
                <asp:HiddenField ID="hfMedicamentoId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Tratamiento</label>
                    <asp:DropDownList ID="ddlTratamiento" runat="server" CssClass="form-control" Enabled="true">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Medicamento</label>
                    <asp:DropDownList ID="ddlMedicamento" runat="server" CssClass="form-control" Enabled="true">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Dosis</label>
                    <asp:TextBox ID="txtDosis" runat="server" CssClass="form-control" 
                        placeholder="Ejemplo: 2ml cada 8 horas"></asp:TextBox>
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
    <script src="Scripts/tratamiento-medicamento-page.js"></script>
</asp:Content>

