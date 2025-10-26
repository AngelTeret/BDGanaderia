/**
 * Cargo Page JavaScript
 * Funcionalidades para la página Cargo.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('cargoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('cargoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('cargoModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombreCargo = document.getElementById('txtNombreCargo');
    const hfCargoId = document.getElementById('hfCargoId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombreCargo) txtNombreCargo.value = '';
    if (hfCargoId) hfCargoId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Cargo';
}

// Función para confirmar eliminación
function confirmDeleteCargo(button) {
    // Obtener el ID del cargo del CommandArgument
    const cargoId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El cargo será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfCargoIdEliminar = document.getElementById('hfCargoIdEliminar');
            if (hfCargoIdEliminar) {
                hfCargoIdEliminar.value = cargoId;
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

// Función para abrir modal de nuevo cargo
function openNewCargoModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('cargoModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteCargo = confirmDeleteCargo;
window.openNewCargoModal = openNewCargoModal;

