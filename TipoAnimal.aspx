<%@ Page Language="VB" AutoEventWireup="true" CodeFile="TipoAnimal.aspx.vb" Inherits="TipoAnimal" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Tipos de Animal - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Tipos de Animal</h1>
        <p class="page-subtitle">Administra los tipos de animales de tu explotación</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoTipoAnimal" runat="server" Text="Nuevo Tipo" 
                CssClass="btn btn-primary" OnClientClick="openNewTipoAnimalModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Tipos de Animal</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los tipos de animales registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfTipoAnimalIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de tipos de animal -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvTipoAnimal" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvTipoAnimal_RowCommand"
                EmptyDataText="No hay tipos de animal registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_TipoAnimal" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Tipo" HeaderText="Nombre del Tipo" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarTipoAnimal" CommandArgument='<%# Eval("ID_TipoAnimal") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarTipoAnimal" CommandArgument='<%# Eval("ID_TipoAnimal") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteTipoAnimal(this);" 
                                data-command-argument='<%# Eval("ID_TipoAnimal") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar tipo de animal -->
    <div id="tipoAnimalModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Tipo de Animal</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfTipoAnimalId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre del Tipo de Animal</label>
                    <asp:TextBox ID="txtNombreTipo" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre del tipo de animal"></asp:TextBox>
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
    <script src="Scripts/TipoAnimal-page.js"></script>
</asp:Content>
