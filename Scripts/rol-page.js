/**
 * Rol Page JavaScript
 * Funcionalidades para la página Rol.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('rolModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('rolModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('rolModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombreRol = document.getElementById('txtNombreRol');
    const hfRolId = document.getElementById('hfRolId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombreRol) txtNombreRol.value = '';
    if (hfRolId) hfRolId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Rol';
}

// Función para confirmar eliminación
function confirmDeleteRol(button) {
    // Obtener el ID del rol del CommandArgument
    const rolId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El rol será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfRolIdEliminar = document.getElementById('hfRolIdEliminar');
            if (hfRolIdEliminar) {
                hfRolIdEliminar.value = rolId;
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

// Función para abrir modal de nuevo rol
function openNewRolModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('rolModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteRol = confirmDeleteRol;
window.openNewRolModal = openNewRolModal;

