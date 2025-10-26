<%@ Page Language="VB" AutoEventWireup="true" CodeFile="RegistroSanitario.aspx.vb" Inherits="RegistroSanitario" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Registros Sanitarios - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Registros Sanitarios</h1>
        <p class="page-subtitle">Registra y administra los registros sanitarios de los animales</p>
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
            <h3 class="stat-card-title">Lista de Registros Sanitarios</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los registros sanitarios del ganado</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfRegistroIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de registros -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvRegistros" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvRegistros_RowCommand"
                EmptyDataText="No hay registros sanitarios registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_RegistroSan" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Animal" HeaderText="Animal" />
                    <asp:BoundField DataField="Fecha_Registro" HeaderText="Fecha de Registro" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarRegistro" CommandArgument='<%# Eval("ID_RegistroSan") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarRegistro" CommandArgument='<%# Eval("ID_RegistroSan") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteRegistro(this);" 
                                data-command-argument='<%# Eval("ID_RegistroSan") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar registro -->
    <div id="registroModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Registro Sanitario</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfRegistroId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Animal</label>
                    <asp:DropDownList ID="ddlAnimal" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Fecha de Registro</label>
                    <asp:TextBox ID="txtFechaRegistro" runat="server" CssClass="form-control" 
                        TextMode="Date"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Descripción</label>
                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" 
                        placeholder="Ejemplo: Vacunación, tratamiento, etc." TextMode="MultiLine" Rows="3"></asp:TextBox>
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
    <script src="Scripts/registro-sanitario-page.js"></script>
</asp:Content>

