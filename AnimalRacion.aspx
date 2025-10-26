<%@ Page Language="VB" AutoEventWireup="true" CodeFile="AnimalRacion.aspx.vb" Inherits="AnimalRacion" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Raciones de Animales - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Raciones de Animales</h1>
        <p class="page-subtitle">Asigna raciones a los animales con fecha de asignación</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevaAsignacion" runat="server" Text="Nueva Asignación" 
                CssClass="btn btn-primary" OnClientClick="openNewAsignacionModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Asignaciones de Raciones</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todas las raciones asignadas a los animales</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfAnimalIdEliminar" runat="server" />
        <asp:HiddenField ID="hfRacionIdEliminar" runat="server" />
        <asp:HiddenField ID="hfFechaAsignacionEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de asignaciones -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvAsignaciones" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvAsignaciones_RowCommand"
                EmptyDataText="No hay raciones asignadas" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="Nombre_Animal" HeaderText="Animal" />
                    <asp:BoundField DataField="Nombre_Racion" HeaderText="Ración" />
                    <asp:BoundField DataField="Fecha_Asignacion" HeaderText="Fecha de Asignación" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarAsignacion" 
                                CommandArgument='<%# Eval("ID_Animal").ToString() + "," + Eval("ID_Racion").ToString() + "," + Eval("Fecha_Asignacion", "{0:yyyy-MM-dd}") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteAsignacion(this);" 
                                data-animal-id='<%# Eval("ID_Animal") %>'
                                data-racion-id='<%# Eval("ID_Racion") %>'
                                data-fecha-asignacion='<%# Eval("Fecha_Asignacion", "{0:yyyy-MM-dd}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar asignación de ración -->
    <div id="asignacionModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nueva Asignación de Ración</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                
                <div class="form-group">
                    <label class="form-label">Animal</label>
                    <asp:DropDownList ID="ddlAnimal" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Ración</label>
                    <asp:DropDownList ID="ddlRacion" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Fecha de Asignación</label>
                    <asp:TextBox ID="txtFechaAsignacion" runat="server" CssClass="form-control" 
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
    <script src="Scripts/animal-racion-page.js"></script>
</asp:Content>

