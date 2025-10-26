<%@ Page Language="VB" AutoEventWireup="true" CodeFile="AnimalPesaje.aspx.vb" Inherits="AnimalPesaje" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Pesajes - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Pesajes</h1>
        <p class="page-subtitle">Registra y administra los pesajes de los animales</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoPesaje" runat="server" Text="Nuevo Pesaje" 
                CssClass="btn btn-primary" OnClientClick="openNewPesajeModal(); return false;" />
            <a href="#" class="btn btn-secondary">
                <i class="fas fa-download"></i>
                Exportar
            </a>
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Pesajes</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los registros de pesaje del ganado</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfPesajeIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de pesajes -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvPesajes" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvPesajes_RowCommand"
                EmptyDataText="No hay pesajes registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Pesaje" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Animal" HeaderText="Animal" />
                    <asp:BoundField DataField="Fecha_Pesaje" HeaderText="Fecha del Pesaje" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Peso" HeaderText="Peso (kg)" DataFormatString="{0:F2}" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarPesaje" CommandArgument='<%# Eval("ID_Pesaje") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarPesaje" CommandArgument='<%# Eval("ID_Pesaje") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeletePesaje(this);" 
                                data-command-argument='<%# Eval("ID_Pesaje") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar pesaje -->
    <div id="pesajeModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Pesaje</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfPesajeId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Animal</label>
                    <asp:DropDownList ID="ddlAnimal" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Fecha del Pesaje</label>
                    <asp:TextBox ID="txtFechaPesaje" runat="server" CssClass="form-control" 
                        TextMode="Date"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Peso (kg)</label>
                    <asp:TextBox ID="txtPeso" runat="server" CssClass="form-control" 
                        placeholder="Ejemplo: 450.50 o 450,50" TextMode="Number" step="0.01"></asp:TextBox>
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
    <script src="Scripts/animal-pesaje-page.js"></script>
</asp:Content>

