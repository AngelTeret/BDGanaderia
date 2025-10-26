<%@ Page Language="VB" AutoEventWireup="true" CodeFile="AnimalControlLechero.aspx.vb" Inherits="AnimalControlLechero" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Control Lechero - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Control Lechero</h1>
        <p class="page-subtitle">Registra la producción de leche por animal</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoRegistro" runat="server" Text="Nuevo Registro" 
                CssClass="btn btn-primary" OnClientClick="openNewRegistroModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Control Lechero</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los registros de producción de leche</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfAnimalIdEliminar" runat="server" />
        <asp:HiddenField ID="hfControlIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de registros -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvRegistros" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvRegistros_RowCommand"
                EmptyDataText="No hay registros de control lechero" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="Nombre_Animal" HeaderText="Animal" />
                    <asp:BoundField DataField="Fecha_Control" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Litros_Leche" HeaderText="Litros (L)" 
                        DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Right" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarRegistro" 
                                CommandArgument='<%# Eval("ID_Animal").ToString() + "," + Eval("ID_Control").ToString() %>'
                                CssClass="btn btn-warning" 
                                OnClientClick="return true;" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarRegistro" 
                                CommandArgument='<%# Eval("ID_Animal").ToString() + "," + Eval("ID_Control").ToString() %>'
                                CssClass="btn btn-danger"
                                OnClientClick="confirmDeleteRegistro(this); return false;" 
                                data-animal-id='<%# Eval("ID_Animal") %>'
                                data-control-id='<%# Eval("ID_Control") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar registro -->
    <div id="registroModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Registro de Control Lechero</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfAnimalId" runat="server" />
                <asp:HiddenField ID="hfControlId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Animal</label>
                    <asp:DropDownList ID="ddlAnimal" runat="server" CssClass="form-control" Enabled="true">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Control Lechero</label>
                    <asp:DropDownList ID="ddlControl" runat="server" CssClass="form-control" Enabled="true">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Litros de Leche</label>
                    <asp:TextBox ID="txtLitrosLeche" runat="server" CssClass="form-control" 
                        TextMode="Number" step="0.01" placeholder="Ingrese los litros de leche"></asp:TextBox>
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
    <script src="Scripts/animal-control-lechero-page.js"></script>
</asp:Content>

