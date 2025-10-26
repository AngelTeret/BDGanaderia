<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Ordenador.aspx.vb" Inherits="Ordenador" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Ordeñadores - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Ordeñadores</h1>
        <p class="page-subtitle">Administra los ordeñadores del ganado</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoOrdenador" runat="server" Text="Nuevo Ordeñador" 
                CssClass="btn btn-primary" OnClientClick="openNewOrdenadorModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Ordeñadores</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los ordeñadores registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfOrdenadorIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de ordeñadores -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvOrdenadores" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvOrdenadores_RowCommand"
                EmptyDataText="No hay ordeñadores registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Ordenador" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Ordenador" HeaderText="Nombre del Ordeñador" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarOrdenador" CommandArgument='<%# Eval("ID_Ordenador") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarOrdenador" CommandArgument='<%# Eval("ID_Ordenador") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteOrdenador(this);" 
                                data-command-argument='<%# Eval("ID_Ordenador") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar ordeñador -->
    <div id="ordenadorModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Ordeñador</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
                <div class="modal-body">
                <asp:HiddenField ID="hfOrdenadorId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre del Ordeñador</label>
                    <asp:TextBox ID="txtNombreOrdenador" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre del ordeñador"></asp:TextBox>
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
    <script src="Scripts/ordenador-page.js"></script>
</asp:Content>

