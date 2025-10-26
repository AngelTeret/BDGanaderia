<%@ Page Language="VB" AutoEventWireup="true" CodeFile="GestionAgua.aspx.vb" Inherits="GestionAgua" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Agua - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Agua</h1>
        <p class="page-subtitle">Registra y administra las revisiones de bebederos</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevaGestion" runat="server" Text="Nueva Revisión" 
                CssClass="btn btn-primary" OnClientClick="openNewGestionModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Revisión de Bebederos</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todas las revisiones de bebederos de los potreros</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfGestionIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de gestiones -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvGestiones" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvGestiones_RowCommand"
                EmptyDataText="No hay revisiones de bebederos registradas" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_GestionAgua" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Potrero" HeaderText="Potrero" />
                    <asp:BoundField DataField="Fecha_Revision" HeaderText="Fecha de Revisión" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Estado_Bebedero" HeaderText="Estado del Bebedero" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarGestion" CommandArgument='<%# Eval("ID_GestionAgua") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarGestion" CommandArgument='<%# Eval("ID_GestionAgua") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteGestion(this);" 
                                data-command-argument='<%# Eval("ID_GestionAgua") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar gestión -->
    <div id="gestionModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nueva Revisión de Bebedero</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfGestionId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Potrero</label>
                    <asp:DropDownList ID="ddlPotrero" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Fecha de Revisión</label>
                    <asp:TextBox ID="txtFechaRevision" runat="server" CssClass="form-control" 
                        TextMode="Date"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Estado del Bebedero</label>
                    <asp:TextBox ID="txtEstadoBebedero" runat="server" CssClass="form-control" 
                        placeholder="Ejemplo: Funcional, Dañado, Requiere mantenimiento"></asp:TextBox>
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
    <script src="Scripts/gestion-agua-page.js"></script>
</asp:Content>

