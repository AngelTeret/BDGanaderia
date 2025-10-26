<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Ordeno.aspx.vb" Inherits="Ordeno" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Ordeños - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Ordeños</h1>
        <p class="page-subtitle">Administra los ordeños de tu explotación ganadera</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoOrdeno" runat="server" Text="Nuevo Ordeño" 
                CssClass="btn btn-primary" OnClientClick="openNewOrdenoModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Ordeños</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los ordeños registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfOrdenoIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de ordeños -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvOrdenos" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvOrdenos_RowCommand"
                EmptyDataText="No hay ordeños registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Ordeno" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Ordenador" HeaderText="Ordenador" />
                    <asp:BoundField DataField="Fecha_Ordeno" HeaderText="Fecha de Ordeño" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Turno" HeaderText="Turno" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarOrdeno" CommandArgument='<%# Eval("ID_Ordeno") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarOrdeno" CommandArgument='<%# Eval("ID_Ordeno") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteOrdeno(this);" 
                                data-command-argument='<%# Eval("ID_Ordeno") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar ordeño -->
    <div id="ordenoModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Ordeño</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfOrdenoId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Ordenador</label>
                    <asp:DropDownList ID="ddlOrdenador" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Fecha de Ordeño</label>
                    <asp:TextBox ID="txtFechaOrdeno" runat="server" CssClass="form-control" 
                        TextMode="Date" placeholder="Seleccione la fecha del ordeño"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Turno</label>
                    <asp:DropDownList ID="ddlTurno" runat="server" CssClass="form-control">
                        <asp:ListItem Value="" Text="Seleccione un turno"></asp:ListItem>
                        <asp:ListItem Value="Mañana" Text="Mañana"></asp:ListItem>
                        <asp:ListItem Value="Tarde" Text="Tarde"></asp:ListItem>
                        <asp:ListItem Value="Noche" Text="Noche"></asp:ListItem>
                    </asp:DropDownList>
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
    <script src="Scripts/ordeno-page.js"></script>
</asp:Content>
