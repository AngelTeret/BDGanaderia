<%@ Page Language="VB" AutoEventWireup="true" CodeFile="AnimalVacuna.aspx.vb" Inherits="AnimalVacuna" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Vacunaciones - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Vacunaciones</h1>
        <p class="page-subtitle">Registra las vacunas aplicadas a los animales con fecha</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevaVacunacion" runat="server" Text="Nueva Vacunación" 
                CssClass="btn btn-primary" OnClientClick="openNewVacunacionModal(); return false;" />   
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Vacunaciones</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todas las vacunas aplicadas a los animales</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfAnimalIdEliminar" runat="server" />
        <asp:HiddenField ID="hfVacunaIdEliminar" runat="server" />
        <asp:HiddenField ID="hfFechaAplicacionEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de vacunaciones -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvVacunaciones" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvVacunaciones_RowCommand"
                EmptyDataText="No hay vacunaciones registradas" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="Nombre_Animal" HeaderText="Animal" />
                    <asp:BoundField DataField="Nombre_Vacuna" HeaderText="Vacuna" />
                    <asp:BoundField DataField="Fecha_Aplicacion" HeaderText="Fecha de Aplicación" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarVacunacion" 
                                CommandArgument='<%# Eval("ID_Animal").ToString() + "," + Eval("ID_Vacuna").ToString() + "," + Eval("Fecha_Aplicacion", "{0:yyyy-MM-dd}") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteVacunacion(this);" 
                                data-animal-id='<%# Eval("ID_Animal") %>'
                                data-vacuna-id='<%# Eval("ID_Vacuna") %>'
                                data-fecha-aplicacion='<%# Eval("Fecha_Aplicacion", "{0:yyyy-MM-dd}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar vacunación -->
    <div id="vacunacionModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nueva Vacunación</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                
                <div class="form-group">
                    <label class="form-label">Animal</label>
                    <asp:DropDownList ID="ddlAnimal" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Vacuna</label>
                    <asp:DropDownList ID="ddlVacuna" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Fecha de Aplicación</label>
                    <asp:TextBox ID="txtFechaAplicacion" runat="server" CssClass="form-control" 
                        TextMode="Date"></asp:TextBox>
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
    <script src="Scripts/animal-vacuna-page.js"></script>
</asp:Content>

