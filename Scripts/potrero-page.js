/**
 * Potrero Page JavaScript
 * Funcionalidades para la página Potrero.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('potreroModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('potreroModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('potreroModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombrePotrero = document.getElementById('txtNombrePotrero');
    const txtArea = document.getElementById('txtArea');
    const hfPotreroId = document.getElementById('hfPotreroId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombrePotrero) txtNombrePotrero.value = '';
    if (txtArea) txtArea.value = '';
    if (hfPotreroId) hfPotreroId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Potrero';
}

// Función para confirmar eliminación
function confirmDeletePotrero(button) {
    // Obtener el ID del potrero del CommandArgument
    const potreroId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El potrero será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfPotreroIdEliminar = document.getElementById('hfPotreroIdEliminar');
            if (hfPotreroIdEliminar) {
                hfPotreroIdEliminar.value = potreroId;
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

// Función para abrir modal de nuevo potrero
function openNewPotreroModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('potreroModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeletePotrero = confirmDeletePotrero;
window.openNewPotreroModal = openNewPotreroModal;

