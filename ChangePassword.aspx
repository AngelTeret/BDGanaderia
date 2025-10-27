<%@ Page Language="VB" AutoEventWireup="true" CodeFile="ChangePassword.aspx.vb" Inherits="ChangePassword" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Cambiar Contraseña - BDGanaderia</title>
    <link href="Content/auth.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-card">
        <div class="auth-header">
            <h2 class="auth-title">Cambiar Contraseña</h2>
            <p class="auth-subtitle">Actualiza tu contraseña de acceso</p>
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
                <label for="txtCurrentPassword" class="auth-label">Contraseña Actual</label>
                <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="auth-input" TextMode="Password" autocomplete="current-password" />
            </div>
            
            <div class="auth-form-group">
                <label for="txtNewPassword" class="auth-label">Nueva Contraseña</label>
                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="auth-input" TextMode="Password" autocomplete="new-password" />
            </div>
            
            <div class="auth-form-group">
                <label for="txtConfirmPassword" class="auth-label">Confirmar Nueva Contraseña</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="auth-input" TextMode="Password" autocomplete="new-password" />
            </div>
            
            <div class="auth-form-actions">
                <asp:Button ID="btnChangePassword" runat="server" Text="Cambiar Contraseña" CssClass="auth-button" OnClick="btnChangePassword_Click" />
                <a href="Default.aspx" class="auth-button auth-button-secondary">Cancelar</a>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="ScriptsContent" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="Scripts/auth.js"></script>
</asp:Content>
