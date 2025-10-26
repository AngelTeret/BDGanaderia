/**
 * Corral Page JavaScript
 * Funcionalidades para la página Corral.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('corralModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('corralModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('corralModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombreCorral = document.getElementById('txtNombreCorral');
    const txtCapacidad = document.getElementById('txtCapacidad');
    const hfCorralId = document.getElementById('hfCorralId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombreCorral) txtNombreCorral.value = '';
    if (txtCapacidad) txtCapacidad.value = '';
    if (hfCorralId) hfCorralId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Corral';
}

// Función para confirmar eliminación
function confirmDeleteCorral(button) {
    // Obtener el ID del corral del CommandArgument
    const corralId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El corral será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfCorralIdEliminar = document.getElementById('hfCorralIdEliminar');
            if (hfCorralIdEliminar) {
                hfCorralIdEliminar.value = corralId;
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

// Función para abrir modal de nuevo corral
function openNewCorralModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('corralModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteCorral = confirmDeleteCorral;
window.openNewCorralModal = openNewCorralModal;

