/**
 * Gestion Agua Page JavaScript
 * Funcionalidades para la página GestionAgua.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('gestionModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('gestionModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('gestionModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const ddlPotrero = document.getElementById('ddlPotrero');
    const txtFechaRevision = document.getElementById('txtFechaRevision');
    const txtEstadoBebedero = document.getElementById('txtEstadoBebedero');
    const hfGestionId = document.getElementById('hfGestionId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (ddlPotrero && ddlPotrero.options.length > 0) ddlPotrero.selectedIndex = 0;
    if (txtFechaRevision) txtFechaRevision.value = '';
    if (txtEstadoBebedero) txtEstadoBebedero.value = '';
    if (hfGestionId) hfGestionId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nueva Revisión de Bebedero';
}

// Función para confirmar eliminación
function confirmDeleteGestion(button) {
    // Obtener el ID de la gestión del CommandArgument
    const gestionId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! La revisión de bebedero será eliminada permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfGestionIdEliminar = document.getElementById('hfGestionIdEliminar');
            if (hfGestionIdEliminar) {
                hfGestionIdEliminar.value = gestionId;
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

// Función para abrir modal de nueva revisión
function openNewGestionModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('gestionModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteGestion = confirmDeleteGestion;
window.openNewGestionModal = openNewGestionModal;

