/**
 * Auth.js - Validaciones básicas para autenticación
 * No dependencias externas
 */

(function() {
    'use strict';
    
    // Validación en cliente para Login
    const initLoginValidation = function() {
        const form = document.getElementById('form1');
        if (!form) return;
        
        const btnLogin = document.getElementById('btnLogin');
        if (!btnLogin) return;
        
        btnLogin.addEventListener('click', function(e) {
            const username = document.getElementById('txtUsername');
            const password = document.getElementById('txtPassword');
            
            let isValid = true;
            
            // Validar username
            if (!username || username.value.trim() === '') {
                isValid = false;
                username.style.borderColor = '#dc3545';
            } else {
                username.style.borderColor = '#ddd';
            }
            
            // Validar password
            if (!password || password.value === '') {
                isValid = false;
                password.style.borderColor = '#dc3545';
            } else {
                password.style.borderColor = '#ddd';
            }
            
            if (!isValid) {
                e.preventDefault();
                return false;
            }
        });
    };
    
    // Validación para ChangePassword
    const initChangePasswordValidation = function() {
        const form = document.getElementById('form1');
        if (!form) return;
        
        const btnChangePassword = document.getElementById('btnChangePassword');
        if (!btnChangePassword) return;
        
        btnChangePassword.addEventListener('click', function(e) {
            const currentPassword = document.getElementById('txtCurrentPassword');
            const newPassword = document.getElementById('txtNewPassword');
            const confirmPassword = document.getElementById('txtConfirmPassword');
            
            let isValid = true;
            
            // Validar contraseña actual
            if (!currentPassword || currentPassword.value === '') {
                isValid = false;
                currentPassword.style.borderColor = '#dc3545';
            } else {
                currentPassword.style.borderColor = '#ddd';
            }
            
            // Validar nueva contraseña
            if (!newPassword || newPassword.value === '') {
                isValid = false;
                newPassword.style.borderColor = '#dc3545';
            } else if (newPassword.value.length < 3) {
                isValid = false;
                newPassword.style.borderColor = '#dc3545';
                alert('La nueva contraseña debe tener al menos 3 caracteres');
            } else {
                newPassword.style.borderColor = '#ddd';
            }
            
            // Validar confirmación
            if (!confirmPassword || confirmPassword.value === '') {
                isValid = false;
                confirmPassword.style.borderColor = '#dc3545';
            } else if (newPassword.value !== confirmPassword.value) {
                isValid = false;
                confirmPassword.style.borderColor = '#dc3545';
                alert('Las contraseñas nuevas no coinciden');
            } else {
                confirmPassword.style.borderColor = '#ddd';
            }
            
            if (!isValid) {
                e.preventDefault();
                return false;
            }
        });
        
        // Validar coincidencia en tiempo real
        const newPassword = document.getElementById('txtNewPassword');
        const confirmPassword = document.getElementById('txtConfirmPassword');
        
        if (newPassword && confirmPassword) {
            [newPassword, confirmPassword].forEach(function(input) {
                input.addEventListener('input', function() {
                    if (newPassword.value && confirmPassword.value) {
                        if (newPassword.value === confirmPassword.value) {
                            confirmPassword.style.borderColor = '#28a745';
                        } else {
                            confirmPassword.style.borderColor = '#dc3545';
                        }
                    }
                });
            });
        }
    };
    
    // Inicializar al cargar
    document.addEventListener('DOMContentLoaded', function() {
        const pagePath = window.location.pathname.toLowerCase();
        
        if (pagePath.includes('login.aspx')) {
            initLoginValidation();
        } else if (pagePath.includes('changepassword.aspx')) {
            initChangePasswordValidation();
        }
        
        // Auto-focus en primer input de formularios auth
        const firstInput = document.querySelector('.auth-input');
        if (firstInput) {
            firstInput.focus();
        }
    });
})();


