<%@ Page Language="VB" AutoEventWireup="true" CodeFile="MuestreoSuelo.aspx.vb" Inherits="MuestreoSuelo" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Muestreos de Suelo - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Muestreos de Suelo</h1>
        <p class="page-subtitle">Registra y administra los muestreos de suelo de los potreros</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoMuestreo" runat="server" Text="Nuevo Muestreo" 
                CssClass="btn btn-primary" OnClientClick="openNewMuestreoModal(); return false;" />
            <a href="#" class="btn btn-secondary">
                <i class="fas fa-download"></i>
                Exportar
            </a>
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Muestreos de Suelo</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los muestreos de suelo de los potreros</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfMuestreoIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de muestreos -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvMuestreos" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvMuestreos_RowCommand"
                EmptyDataText="No hay muestreos de suelo registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Muestreo" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Potrero" HeaderText="Potrero" />
                    <asp:BoundField DataField="Fecha_Muestreo" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="pH" HeaderText="pH" DataFormatString="{0:F2}" />
                    <asp:BoundField DataField="Materia_Organica" HeaderText="Materia Orgánica (%)" DataFormatString="{0:F2}" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarMuestreo" CommandArgument='<%# Eval("ID_Muestreo") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarMuestreo" CommandArgument='<%# Eval("ID_Muestreo") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteMuestreo(this);" 
                                data-command-argument='<%# Eval("ID_Muestreo") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar muestreo -->
    <div id="muestreoModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Muestreo de Suelo</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfMuestreoId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Potrero</label>
                    <asp:DropDownList ID="ddlPotrero" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Fecha de Muestreo</label>
                    <asp:TextBox ID="txtFechaMuestreo" runat="server" CssClass="form-control" 
                        TextMode="Date"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">pH</label>
                    <asp:TextBox ID="txtpH" runat="server" CssClass="form-control" 
                        placeholder="Ejemplo: 6.5 o 6,5" TextMode="Number" step="0.01"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Materia Orgánica (%)</label>
                    <asp:TextBox ID="txtMateriaOrganica" runat="server" CssClass="form-control" 
                        placeholder="Ejemplo: 3.50 o 3,50" TextMode="Number" step="0.01"></asp:TextBox>
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
    <script src="Scripts/muestreo-suelo-page.js"></script>
</asp:Content>

