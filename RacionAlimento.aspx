<%@ Page Language="VB" AutoEventWireup="true" CodeFile="RacionAlimento.aspx.vb" Inherits="RacionAlimento" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Alimentos por Ración - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Alimentos por Ración</h1>
        <p class="page-subtitle">Define qué alimentos contiene cada ración y sus cantidades</p>
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
            <h3 class="stat-card-title">Lista de Alimentos por Ración</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los alimentos y sus cantidades en cada ración</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfRacionIdEliminar" runat="server" />
        <asp:HiddenField ID="hfAlimentoIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de asignaciones -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvAsignaciones" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvAsignaciones_RowCommand"
                EmptyDataText="No hay alimentos asignados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="Nombre_Racion" HeaderText="Ración" />
                    <asp:BoundField DataField="Nombre_Alimento" HeaderText="Alimento" />
                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad (kg)" DataFormatString="{0:F2}" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarAsignacion" 
                                CommandArgument='<%# Eval("ID_Racion").ToString() + "," + Eval("ID_Alimento").ToString() %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarAsignacion" 
                                CommandArgument='<%# Eval("ID_Racion").ToString() + "," + Eval("ID_Alimento").ToString() %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteAsignacion(this);" 
                                data-racion-id='<%# Eval("ID_Racion") %>'
                                data-alimento-id='<%# Eval("ID_Alimento") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar asignación de alimento -->
    <div id="asignacionModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nueva Asignación de Alimento</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfRacionId" runat="server" />
                <asp:HiddenField ID="hfAlimentoId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Ración</label>
                    <asp:DropDownList ID="ddlRacion" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Alimento</label>
                    <asp:DropDownList ID="ddlAlimento" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Cantidad (kg)</label>
                    <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control" 
                        placeholder="Ejemplo: 5.5" TextMode="Number" step="0.01"></asp:TextBox>
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
    <script src="Scripts/racion-alimento-page.js"></script>
</asp:Content>

