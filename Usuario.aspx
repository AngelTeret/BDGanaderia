<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Usuario.aspx.vb" Inherits="Usuario" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Usuarios - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Usuarios</h1>
        <p class="page-subtitle">Administra los usuarios del sistema</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoUsuario" runat="server" Text="Nuevo Usuario" 
                CssClass="btn btn-primary" OnClientClick="openNewUsuarioModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Usuarios</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los usuarios registrados en el sistema</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfUsuarioIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de usuarios -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvUsuarios" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvUsuarios_RowCommand"
                EmptyDataText="No hay usuarios registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="ID_Usuario" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre_Usuario" HeaderText="Nombre de Usuario" />
                    <asp:BoundField DataField="Correo" HeaderText="Correo" />
                    <asp:BoundField DataField="Nombre_Rol" HeaderText="Rol" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarUsuario" CommandArgument='<%# Eval("ID_Usuario") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarUsuario" CommandArgument='<%# Eval("ID_Usuario") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="return confirmDeleteUsuario(this, event);" 
                                data-command-argument='<%# Eval("ID_Usuario") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar/editar usuario -->
    <div id="usuarioModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Usuario</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfUsuarioId" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Nombre de Usuario</label>
                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" 
                        placeholder="Ingrese el nombre de usuario"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Contraseña</label>
                    <div style="display: flex;">
                        <asp:TextBox ID="txtContrasena" runat="server" CssClass="form-control" 
                            placeholder="Ingrese la contraseña (mínimo 6 caracteres)" style="flex: 1;"></asp:TextBox>
                        <span class="input-group-text" style="cursor: pointer; border-left: 0; display: flex; align-items: center; padding: 0.375rem 0.75rem;" onclick="togglePassword('txtContrasena', this)">
                            <i class="fas fa-eye" id="icon-txtContrasena"></i>
                        </span>
                    </div>
                    <small id="contraseñaLength" class="form-text text-danger" style="display: none;">La contraseña debe tener al menos 6 caracteres</small>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Confirmar Contraseña</label>
                    <div style="display: flex;">
                        <asp:TextBox ID="txtConfirmarContrasena" runat="server" CssClass="form-control" 
                            placeholder="Confirme la contraseña" style="flex: 1;"></asp:TextBox>
                        <span class="input-group-text" style="cursor: pointer; border-left: 0; display: flex; align-items: center; padding: 0.375rem 0.75rem;" onclick="togglePassword('txtConfirmarContrasena', this)">
                            <i class="fas fa-eye" id="icon-txtConfirmarContrasena"></i>
                        </span>
                    </div>
                    <small id="contraseñaMatch" class="form-text text-danger" style="display: none;">Las contraseñas no coinciden</small>
                    <small id="contraseñaConfirmLength" class="form-text text-danger" style="display: none;">La contraseña debe tener al menos 6 caracteres</small>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Correo Electrónico</label>
                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" 
                        placeholder="usuario@ejemplo.com" TextMode="Email"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Rol</label>
                    <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-control">
                    </asp:DropDownList>
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
    <script src="Scripts/usuario-page.js"></script>
</asp:Content>

