<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Potrero.aspx.vb" Inherits="Potrero" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Potreros - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Potreros</h1>
        <p class="page-subtitle">Administra los potreros del ganado</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoPotrero" runat="server" Text="Nuevo Potrero" 
                CssClass="btn btn-primary" OnClientClick="openNewPotreroModal(); return false;" />
            <a href="#" class="btn btn-secondary">
                <i class="fas fa-download"></i>
                Exportar
            </a>
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Potreros</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los potreros registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfPotreroIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de potreros -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvPotreros" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvPotreros_RowCommand"
                EmptyDataText="No hay potreros registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Potrero" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Potrero" HeaderText="Nombre del Potrero" />
                    <asp:BoundField DataField="Area" HeaderText="Área (ha)" DataFormatString="{0:F2}" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarPotrero" CommandArgument='<%# Eval("ID_Potrero") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarPotrero" CommandArgument='<%# Eval("ID_Potrero") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeletePotrero(this);" 
                                data-command-argument='<%# Eval("ID_Potrero") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar potrero -->
    <div id="potreroModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Potrero</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfPotreroId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre del Potrero</label>
                    <asp:TextBox ID="txtNombrePotrero" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre del potrero"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Área (hectáreas)</label>
                    <asp:TextBox ID="txtArea" runat="server" CssClass="form-control" 
                        placeholder="Ejemplo: 100.50 o 100,50" TextMode="Number" step="0.01"></asp:TextBox>
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
    <script src="Scripts/potrero-page.js"></script>
</asp:Content>

