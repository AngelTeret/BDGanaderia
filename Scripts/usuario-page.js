/**
 * Usuario Page JavaScript
 * Funcionalidades para la página Usuario.aspx
 */

// Función para mostrar/ocultar contraseña
function togglePassword(inputId, button) {
    const input = document.getElementById(inputId);
    const icon = document.getElementById('icon-' + inputId);
    
    if (input && icon) {
        if (input.type === 'password') {
            input.type = 'text';
            icon.classList.remove('fa-eye');
            icon.classList.add('fa-eye-slash');
        } else {
            input.type = 'password';
            icon.classList.remove('fa-eye-slash');
            icon.classList.add('fa-eye');
        }
    }
}

// Función para validar longitud de contraseña
function validatePasswordLength() {
    const txtContrasena = document.getElementById('txtContrasena');
    const txtConfirmarContrasena = document.getElementById('txtConfirmarContrasena');
    const contraseñaLength = document.getElementById('contraseñaLength');
    const contraseñaConfirmLength = document.getElementById('contraseñaConfirmLength');
    
    let isValid = true;
    
    // Validar contraseña principal - solo mostrar error si hay contenido
    if (txtContrasena && contraseñaLength) {
        if (txtContrasena.value.trim() === '') {
            contraseñaLength.classList.remove('show');
            txtContrasena.classList.remove('is-invalid');
        } else if (txtContrasena.value.length < 6) {
            contraseñaLength.classList.add('show');
            txtContrasena.classList.add('is-invalid');
            isValid = false;
        } else {
            contraseñaLength.classList.remove('show');
            txtContrasena.classList.remove('is-invalid');
        }
    }
    
    // Validar confirmación - solo mostrar error si hay contenido
    if (txtConfirmarContrasena && contraseñaConfirmLength) {
        if (txtConfirmarContrasena.value.trim() === '') {
            contraseñaConfirmLength.classList.remove('show');
            txtConfirmarContrasena.classList.remove('is-invalid');
        } else if (txtConfirmarContrasena.value.length < 6) {
            contraseñaConfirmLength.classList.add('show');
            txtConfirmarContrasena.classList.add('is-invalid');
            isValid = false;
        } else {
            contraseñaConfirmLength.classList.remove('show');
            txtConfirmarContrasena.classList.remove('is-invalid');
        }
    }
    
    return isValid;
}

// Función para validar contraseñas
function validatePasswords() {
    const txtContrasena = document.getElementById('txtContrasena');
    const txtConfirmarContrasena = document.getElementById('txtConfirmarContrasena');
    const contraseñaMatch = document.getElementById('contraseñaMatch');
    const btnGuardar = document.getElementById('btnGuardar');
    
    let isValid = true;
    
    if (txtContrasena && txtConfirmarContrasena && contraseñaMatch) {
        // Solo validar si ambos campos tienen contenido
        if (txtContrasena.value !== '' && txtConfirmarContrasena.value !== '') {
            if (txtContrasena.value !== txtConfirmarContrasena.value) {
                contraseñaMatch.classList.add('show');
                txtConfirmarContrasena.classList.add('is-invalid');
                isValid = false;
            } else {
                contraseñaMatch.classList.remove('show');
                txtConfirmarContrasena.classList.remove('is-invalid');
            }
        } else {
            // Si algún campo está vacío, ocultar el error de coincidencia
            contraseñaMatch.classList.remove('show');
        }
    }
    
    // Habilitar/deshabilitar botón de guardar
    updateGuardarButton();
    
    return isValid;
}

// Función para actualizar estado del botón Guardar
function updateGuardarButton() {
    const btnGuardar = document.getElementById('btnGuardar');
    const hfUsuarioId = document.getElementById('hfUsuarioId');
    const txtNombreUsuario = document.getElementById('txtNombreUsuario');
    const txtContrasena = document.getElementById('txtContrasena');
    const txtConfirmarContrasena = document.getElementById('txtConfirmarContrasena');
    const txtCorreo = document.getElementById('txtCorreo');
    const ddlRol = document.getElementById('ddlRol');
    
    if (!btnGuardar) return;
    
    // Verificar si es nuevo usuario o edición
    const isNewUsuario = !hfUsuarioId || hfUsuarioId.value === '0';
    
    // Validar nombre de usuario
    const nombreValido = txtNombreUsuario && txtNombreUsuario.value.trim() !== '';
    
    // Validar contraseña
    let contrasenaValida = true;
    if (isNewUsuario) {
        // En nuevo usuario, contraseña es obligatoria
        contrasenaValida = txtContrasena && txtContrasena.value.length >= 6 && 
                          txtConfirmarContrasena && txtContrasena.value === txtConfirmarContrasena.value;
    } else {
        // En edición, contraseña es opcional, pero si se ingresa debe ser válida
        if (txtContrasena && txtContrasena.value !== '') {
            contrasenaValida = txtContrasena.value.length >= 6 && 
                              txtConfirmarContrasena && txtContrasena.value === txtConfirmarContrasena.value;
        }
    }
    
    // Validar rol
    const rolValido = ddlRol && ddlRol.value !== '0';
    
    // Habilitar o deshabilitar botón
    if (nombreValido && contrasenaValida && rolValido) {
        btnGuardar.disabled = false;
        btnGuardar.style.opacity = '1';
    } else {
        btnGuardar.disabled = true;
        btnGuardar.style.opacity = '0.6';
    }
}

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('usuarioModal');
    if (modal) {
        modal.style.display = 'block';
        // Bloquear scroll del body
        if (typeof lockBodyScroll === 'function') {
            lockBodyScroll();
        }
        // Cuando es edición, ocultar todos los mensajes de error
        const contraseñaMatch = document.getElementById('contraseñaMatch');
        const contraseñaLength = document.getElementById('contraseñaLength');
        const contraseñaConfirmLength = document.getElementById('contraseñaConfirmLength');
        
        if (contraseñaMatch) contraseñaMatch.style.display = 'none';
        if (contraseñaLength) contraseñaLength.style.display = 'none';
        if (contraseñaConfirmLength) contraseñaConfirmLength.style.display = 'none';
        
        // Remover clases de error
        const txtContrasena = document.getElementById('txtContrasena');
        const txtConfirmarContrasena = document.getElementById('txtConfirmarContrasena');
        if (txtContrasena) txtContrasena.classList.remove('is-invalid');
        if (txtConfirmarContrasena) txtConfirmarContrasena.classList.remove('is-invalid');
        
        // Actualizar estado del botón después de un pequeño delay
        setTimeout(updateGuardarButton, 100);
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('usuarioModal');
    if (modal) {
        modal.style.display = 'block';
        // Bloquear scroll del body
        if (typeof lockBodyScroll === 'function') {
            lockBodyScroll();
        }
        // Ocultar todos los mensajes de error
        const contraseñaMatch = document.getElementById('contraseñaMatch');
        const contraseñaLength = document.getElementById('contraseñaLength');
        const contraseñaConfirmLength = document.getElementById('contraseñaConfirmLength');
        
        if (contraseñaMatch) contraseñaMatch.style.display = 'none';
        if (contraseñaLength) contraseñaLength.style.display = 'none';
        if (contraseñaConfirmLength) contraseñaConfirmLength.style.display = 'none';
        
        // Remover clases de error
        const txtContrasena = document.getElementById('txtContrasena');
        const txtConfirmarContrasena = document.getElementById('txtConfirmarContrasena');
        if (txtContrasena) txtContrasena.classList.remove('is-invalid');
        if (txtConfirmarContrasena) txtConfirmarContrasena.classList.remove('is-invalid');
        
        // Actualizar estado del botón después de un pequeño delay
        setTimeout(updateGuardarButton, 100);
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('usuarioModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
        // Liberar scroll del body
        if (typeof unlockBodyScroll === 'function') {
            unlockBodyScroll();
        }
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombreUsuario = document.getElementById('txtNombreUsuario');
    const txtContrasena = document.getElementById('txtContrasena');
    const txtConfirmarContrasena = document.getElementById('txtConfirmarContrasena');
    const txtCorreo = document.getElementById('txtCorreo');
    const ddlRol = document.getElementById('ddlRol');
    const hfUsuarioId = document.getElementById('hfUsuarioId');
    const modalTitle = document.getElementById('modalTitle');
    const contraseñaMatch = document.getElementById('contraseñaMatch');
    const contraseñaLength = document.getElementById('contraseñaLength');
    const contraseñaConfirmLength = document.getElementById('contraseñaConfirmLength');
    
    if (txtNombreUsuario) txtNombreUsuario.value = '';
    if (txtContrasena) {
        txtContrasena.value = '';
        txtContrasena.classList.remove('is-invalid');
    }
    if (txtConfirmarContrasena) {
        txtConfirmarContrasena.value = '';
        txtConfirmarContrasena.classList.remove('is-invalid');
    }
    if (txtCorreo) txtCorreo.value = '';
    if (ddlRol && ddlRol.options.length > 0) ddlRol.selectedIndex = 0;
    if (hfUsuarioId) hfUsuarioId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Usuario';
    
    // Ocultar todos los mensajes de error
    if (contraseñaMatch) contraseñaMatch.style.display = 'none';
    if (contraseñaLength) contraseñaLength.style.display = 'none';
    if (contraseñaConfirmLength) contraseñaConfirmLength.style.display = 'none';
    
    // Resetear tipo de input a password si hay iconos
    const icon1 = document.getElementById('icon-txtContrasena');
    const icon2 = document.getElementById('icon-txtConfirmarContrasena');
    if (icon1 && txtContrasena) {
        txtContrasena.type = 'password';
        icon1.classList.remove('fa-eye-slash');
        icon1.classList.add('fa-eye');
    }
    if (icon2 && txtConfirmarContrasena) {
        txtConfirmarContrasena.type = 'password';
        icon2.classList.remove('fa-eye-slash');
        icon2.classList.add('fa-eye');
    }
}

// Función para confirmar eliminación
function confirmDeleteUsuario(button, e) {
    // Prevenir el postback inmediatamente
    if (e) {
        e.preventDefault();
        e.stopPropagation();
    }
    
    // Obtener el ID del usuario del CommandArgument
    const usuarioId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El usuario será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfUsuarioIdEliminar = document.getElementById('hfUsuarioIdEliminar');
            if (hfUsuarioIdEliminar) {
                hfUsuarioIdEliminar.value = usuarioId;
            }
            
            // Hacer clic en el botón oculto
            const btnEliminarOculto = document.getElementById('btnEliminarOculto');
            if (btnEliminarOculto) {
                btnEliminarOculto.click();
            }
        }
    });
    return false; // Prevenir el postback inmediato
}

// Función para abrir modal de nuevo usuario
function openNewUsuarioModal() {
    clearForm();
    hideAllErrorMessages();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('usuarioModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Función para ocultar mensajes de error al inicio
function hideAllErrorMessages() {
    const contraseñaMatch = document.getElementById('contraseñaMatch');
    const contraseñaLength = document.getElementById('contraseñaLength');
    const contraseñaConfirmLength = document.getElementById('contraseñaConfirmLength');
    
    if (contraseñaMatch) contraseñaMatch.classList.remove('show');
    if (contraseñaLength) contraseñaLength.classList.remove('show');
    if (contraseñaConfirmLength) contraseñaConfirmLength.classList.remove('show');
    
    // Remover clases de error
    const txtContrasena = document.getElementById('txtContrasena');
    const txtConfirmarContrasena = document.getElementById('txtConfirmarContrasena');
    if (txtContrasena) txtContrasena.classList.remove('is-invalid');
    if (txtConfirmarContrasena) txtConfirmarContrasena.classList.remove('is-invalid');
}

// Eventos para validar en tiempo real
document.addEventListener('DOMContentLoaded', function() {
    // Ocultar mensajes de error al cargar la página
    hideAllErrorMessages();
    
    const txtNombreUsuario = document.getElementById('txtNombreUsuario');
    const txtContrasena = document.getElementById('txtContrasena');
    const txtConfirmarContrasena = document.getElementById('txtConfirmarContrasena');
    const txtCorreo = document.getElementById('txtCorreo');
    const ddlRol = document.getElementById('ddlRol');
    
    // Validar cada campo
    if (txtNombreUsuario) {
        txtNombreUsuario.addEventListener('input', updateGuardarButton);
    }
    
    if (txtContrasena) {
        txtContrasena.addEventListener('input', function() {
            validatePasswordLength();
            if (txtConfirmarContrasena && txtConfirmarContrasena.value !== '') {
                validatePasswords();
            }
        });
    }
    
    if (txtConfirmarContrasena) {
        txtConfirmarContrasena.addEventListener('input', function() {
            validatePasswordLength();
            validatePasswords();
        });
    }
    
    if (txtCorreo) {
        txtCorreo.addEventListener('input', updateGuardarButton);
    }
    
    if (ddlRol) {
        ddlRol.addEventListener('change', updateGuardarButton);
    }
    
    // Ejecutar validación inicial
    setTimeout(updateGuardarButton, 100);
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteUsuario = confirmDeleteUsuario;
window.openNewUsuarioModal = openNewUsuarioModal;
window.togglePassword = togglePassword;
window.validatePasswords = validatePasswords;
window.validatePasswordLength = validatePasswordLength;
window.updateGuardarButton = updateGuardarButton;

