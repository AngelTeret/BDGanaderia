<%@ Page Language="VB" AutoEventWireup="true" CodeFile="EstadoAnimal.aspx.vb" Inherits="EstadoAnimal" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Estados de Animal - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Estados de Animal</h1>
        <p class="page-subtitle">Administra los estados de salud de los animales</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoEstadoAnimal" runat="server" Text="Nuevo Estado" 
                CssClass="btn btn-primary" OnClientClick="openNewEstadoAnimalModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Estados de Animal</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los estados de salud de los animales registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfEstadoAnimalIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de estados de animal -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvEstadoAnimal" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvEstadoAnimal_RowCommand"
                EmptyDataText="No hay estados de animal registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_EstadoAnimal" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Estado" HeaderText="Nombre del Estado" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarEstadoAnimal" CommandArgument='<%# Eval("ID_EstadoAnimal") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarEstadoAnimal" CommandArgument='<%# Eval("ID_EstadoAnimal") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteEstadoAnimal(this);" 
                                data-command-argument='<%# Eval("ID_EstadoAnimal") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar estado de animal -->
    <div id="estadoAnimalModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Estado de Animal</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfEstadoAnimalId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre del Estado</label>
                    <asp:TextBox ID="txtNombreEstado" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre del estado (ej: Saludable, Enfermo, En tratamiento)"></asp:TextBox>
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
    <script src="Scripts/EstadoAnimal-page.js"></script>
</asp:Content>
