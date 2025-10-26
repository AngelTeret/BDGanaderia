<%@ Page Language="VB" AutoEventWireup="true" CodeFile="ConsumoRegistro.aspx.vb" Inherits="ConsumoRegistro" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Registros de Consumo - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Registros de Consumo</h1>
        <p class="page-subtitle">Registra y administra el consumo de alimentos de los animales</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoRegistro" runat="server" Text="Nuevo Registro" 
                CssClass="btn btn-primary" OnClientClick="openNewRegistroModal(); return false;" />
            <a href="#" class="btn btn-secondary">
                <i class="fas fa-download"></i>
                Exportar
            </a>
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Registros de Consumo</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los registros de consumo de alimentos</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfConsumoIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de registros -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvConsumos" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvConsumos_RowCommand"
                EmptyDataText="No hay registros de consumo" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Consumo" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Animal" HeaderText="Animal" />
                    <asp:BoundField DataField="Nombre_Alimento" HeaderText="Alimento" />
                    <asp:BoundField DataField="Fecha_Consumo" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad (kg)" DataFormatString="{0:F2}" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarConsumo" CommandArgument='<%# Eval("ID_Consumo") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarConsumo" CommandArgument='<%# Eval("ID_Consumo") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteConsumo(this);" 
                                data-command-argument='<%# Eval("ID_Consumo") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar registro -->
    <div id="consumoModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Registro de Consumo</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfConsumoId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Animal</label>
                    <asp:DropDownList ID="ddlAnimal" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Alimento</label>
                    <asp:DropDownList ID="ddlAlimento" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Fecha de Consumo</label>
                    <asp:TextBox ID="txtFechaConsumo" runat="server" CssClass="form-control" 
                        TextMode="Date"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Cantidad (kg)</label>
                    <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control" 
                        placeholder="Ejemplo: 10.50 o 10,50" TextMode="Number" step="0.01"></asp:TextBox>
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
    <script src="Scripts/consumo-registro-page.js"></script>
</asp:Content>

