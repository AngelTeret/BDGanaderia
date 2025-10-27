<%@ Page Language="VB" AutoEventWireup="true" CodeFile="UserProfile.aspx.vb" Inherits="UserProfile" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Mi Perfil - BDGanaderia</title>
    <link href="Content/auth.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-card">
        <div class="auth-header">
            <h2 class="auth-title">Mi Perfil</h2>
            <p class="auth-subtitle">Gestiona tu información personal</p>
        </div>
        
        <div class="auth-body">
            <div class="auth-alert" id="alertError" runat="server" visible="false">
                <i class="fas fa-exclamation-circle"></i>
                <asp:Label ID="lblError" runat="server" />
            </div>
            
            <div class="auth-alert auth-alert-success" id="alertSuccess" runat="server" visible="false">
                <i class="fas fa-check-circle"></i>
                <asp:Label ID="lblSuccess" runat="server" />
            </div>
            
            <div class="auth-form-group">
                <label class="auth-label">Nombre de Usuario</label>
                <asp:Label ID="lblUsername" runat="server" CssClass="auth-display-value" />
            </div>
            
            <div class="auth-form-group">
                <label class="auth-label">Rol</label>
                <asp:Label ID="lblRole" runat="server" CssClass="auth-display-value" />
            </div>
            
            <div class="auth-form-group">
                <label for="txtEmail" class="auth-label">Correo Electrónico</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="auth-input" TextMode="Email" />
            </div>
            
            <div class="auth-form-actions">
                <asp:Button ID="btnSave" runat="server" Text="Guardar Cambios" CssClass="auth-button" OnClick="btnSave_Click" />
                <a href="ChangePassword.aspx" class="auth-button auth-button-secondary">
                    <i class="fas fa-key"></i> Cambiar Contraseña
                </a>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="ScriptsContent" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="Scripts/auth.js"></script>
</asp:Content>
