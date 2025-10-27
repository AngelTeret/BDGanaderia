<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Login.aspx.vb" Inherits="Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Iniciar Sesión - BDGanaderia</title>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #2c3e50 0%, #34495e 50%, #2c3e50 100%);
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px;
            position: relative;
        }
        
        body::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: 
                radial-gradient(circle at 20% 50%, rgba(255, 255, 255, 0.1) 0%, transparent 50%),
                radial-gradient(circle at 80% 80%, rgba(255, 255, 255, 0.1) 0%, transparent 50%);
            pointer-events: none;
        }
        
        .login-container {
            width: 100%;
            max-width: 420px;
            position: relative;
            z-index: 1;
        }
        
        .login-card {
            background: white;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
            overflow: hidden;
        }
        
        .login-header {
            background: linear-gradient(135deg, #2c3e50 0%, #34495e 100%);
            color: white;
            padding: 40px 35px;
            text-align: center;
        }
        
        .login-icon {
            width: 80px;
            height: 80px;
            background: rgba(255, 255, 255, 0.15);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 20px;
            font-size: 36px;
            backdrop-filter: blur(10px);
            border: 2px solid rgba(255, 255, 255, 0.2);
        }
        
        .login-title {
            margin: 0 0 8px 0;
            font-size: 28px;
            font-weight: 700;
            color: white;
        }
        
        .login-subtitle {
            margin: 0;
            font-size: 14px;
            opacity: 0.9;
            font-weight: 300;
            color: white;
        }
        
        .login-body {
            padding: 40px 35px;
        }
        
        .form-group {
            margin-bottom: 25px;
            position: relative;
        }
        
        .form-label {
            display: block;
            margin-bottom: 10px;
            font-weight: 600;
            color: #2c3e50;
            font-size: 13px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
        
        .input-wrapper {
            position: relative;
        }
        
        .input-icon {
            position: absolute;
            left: 15px;
            top: 50%;
            transform: translateY(-50%);
            color: #95a5a6;
            font-size: 18px;
            pointer-events: none;
            transition: color 0.3s ease;
            z-index: 2;
        }
        
        .input-wrapper:focus-within .input-icon {
            color: #2c3e50;
        }
        
        .form-input {
            width: 100%;
            padding: 15px 15px 15px 45px;
            border: 2px solid #e9ecef;
            border-radius: 12px;
            font-size: 15px;
            transition: all 0.3s ease;
            background: #f8f9fa;
            box-sizing: border-box;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            color: #2c3e50;
            position: relative;
        }
        
        .form-input:focus {
            outline: none;
            border-color: #2c3e50;
            background: white;
            box-shadow: 0 0 0 4px rgba(44, 62, 80, 0.1);
            transform: translateY(-1px);
        }
        
        .form-input:hover {
            border-color: #34495e;
        }
        
        /* Botón de toggle de contraseña */
        .password-toggle {
            position: absolute;
            right: 15px;
            top: 50%;
            transform: translateY(-50%);
            background: none;
            border: none;
            color: #95a5a6;
            font-size: 18px;
            cursor: pointer;
            padding: 5px;
            transition: all 0.3s ease;
            z-index: 2;
        }
        
        .password-toggle:hover {
            color: #2c3e50;
        }
        
        .checkbox-group {
            display: flex;
            align-items: center;
            margin-bottom: 25px;
        }
        
        .checkbox-group input[type="checkbox"] {
            width: 20px;
            height: 20px;
            margin-right: 10px;
            cursor: pointer;
            accent-color: #2c3e50;
        }
        
        .checkbox-group label {
            font-size: 14px;
            color: #666;
            cursor: pointer;
            font-weight: 500;
        }
        
        .btn-login {
            width: 100%;
            padding: 16px;
            background: linear-gradient(135deg, #2c3e50 0%, #34495e 100%);
            color: white;
            border: none;
            border-radius: 12px;
            font-size: 16px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            box-shadow: 0 4px 15px rgba(44, 62, 80, 0.3);
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }
        
        .btn-login:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(44, 62, 80, 0.4);
        }
        
        .btn-login:active {
            transform: translateY(0);
        }
        
        .alert {
            padding: 16px 20px;
            border-radius: 10px;
            margin-bottom: 25px;
            display: flex;
            align-items: center;
            gap: 12px;
            font-size: 14px;
            font-weight: 500;
            border-left: 4px solid;
        }
        
        .alert-error {
            background: #fff3cd;
            color: #856404;
            border-color: #f39c12;
        }
        
        .alert-error i {
            color: #f39c12;
            font-size: 18px;
        }
        
        .alert:empty {
            display: none;
        }
        
        .login-footer {
            text-align: center;
            margin-top: 30px;
            color: rgba(255, 255, 255, 0.9);
            font-size: 13px;
            position: relative;
            z-index: 1;
        }
        
        @media (max-width: 480px) {
            .login-header {
                padding: 30px 25px;
            }
            
            .login-body {
                padding: 30px 25px;
            }
            
            .login-title {
                font-size: 24px;
            }
            
            .login-icon {
                width: 70px;
                height: 70px;
                font-size: 32px;
            }
        }
    </style>
</head>
<body class="auth-page">
    <div class="login-container">
        <div class="login-card">
            <div class="login-header">
                <div class="login-icon">
                    <i class="fas fa-user-shield"></i>
                </div>
                <h1 class="login-title">Iniciar Sesión</h1>
                <p class="login-subtitle">Sistema de Gestión Ganadera</p>
            </div>
            
            <form id="form1" runat="server">
                <div class="login-body">
                    <div class="alert alert-error" id="alertError" runat="server" visible="false">
                        <i class="fas fa-exclamation-circle"></i>
                        <asp:Label ID="lblError" runat="server" />
                    </div>
                    
                    <div class="form-group">
                        <label for="txtUsername" class="form-label">Nombre de Usuario</label>
                        <div class="input-wrapper">
                            <i class="fas fa-user input-icon"></i>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-input" autocomplete="username" placeholder="Ingresa tu usuario" />
                        </div>
                    </div>
                    
                    <div class="form-group">
                        <label for="txtPassword" class="form-label">Contraseña</label>
                        <div class="input-wrapper">
                            <i class="fas fa-lock input-icon"></i>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input" TextMode="Password" autocomplete="current-password" placeholder="Ingresa tu contraseña" ClientIDMode="Static" />
                            <button type="button" class="password-toggle" onclick="togglePassword()">
                                <i class="fas fa-eye" id="passwordToggleIcon"></i>
                            </button>
                        </div>
                    </div>
                    
                    <div class="checkbox-group">
                        <asp:CheckBox ID="chkRemember" runat="server" />
                        <label for="<%= chkRemember.ClientID %>">Recordarme</label>
                    </div>
                    
                    <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión" CssClass="btn-login" OnClick="btnLogin_Click" />
                </div>
            </form>
        </div>
        
        <div class="login-footer">
            <p>&copy; 2024 BDGanaderia. Todos los derechos reservados.</p>
        </div>
    </div>
    
    <script>
        function togglePassword() {
            const passwordInput = document.getElementById('<%= txtPassword.ClientID %>');
            const toggleIcon = document.getElementById('passwordToggleIcon');
            
            if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
                toggleIcon.classList.remove('fa-eye');
                toggleIcon.classList.add('fa-eye-slash');
            } else {
                passwordInput.type = 'password';
                toggleIcon.classList.remove('fa-eye-slash');
                toggleIcon.classList.add('fa-eye');
            }
        }
    </script>
</body>
</html>
