/**
 * Racion Page JavaScript
 * Funcionalidades para la página Racion.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('racionModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('racionModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('racionModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombreRacion = document.getElementById('txtNombreRacion');
    const hfRacionId = document.getElementById('hfRacionId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombreRacion) txtNombreRacion.value = '';
    if (hfRacionId) hfRacionId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nueva Ración';
}

// Función para confirmar eliminación
function confirmDeleteRacion(button) {
    // Obtener el ID de la ración del CommandArgument
    const racionId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! La ración será eliminada permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfRacionIdEliminar = document.getElementById('hfRacionIdEliminar');
            if (hfRacionIdEliminar) {
                hfRacionIdEliminar.value = racionId;
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

// Función para abrir modal de nueva ración
function openNewRacionModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('racionModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteRacion = confirmDeleteRacion;
window.openNewRacionModal = openNewRacionModal;

