/**
 * Vacuna Page JavaScript
 * Funcionalidades para la página Vacuna.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('vacunaModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('vacunaModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('vacunaModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombreVacuna = document.getElementById('txtNombreVacuna');
    const hfVacunaId = document.getElementById('hfVacunaId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombreVacuna) txtNombreVacuna.value = '';
    if (hfVacunaId) hfVacunaId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nueva Vacuna';
}

// Función para confirmar eliminación
function confirmDeleteVacuna(button) {
    // Obtener el ID de la vacuna del CommandArgument
    const vacunaId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! La vacuna será eliminada permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfVacunaIdEliminar = document.getElementById('hfVacunaIdEliminar');
            if (hfVacunaIdEliminar) {
                hfVacunaIdEliminar.value = vacunaId;
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

// Función para abrir modal de nueva vacuna
function openNewVacunaModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('vacunaModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteVacuna = confirmDeleteVacuna;
window.openNewVacunaModal = openNewVacunaModal;

