/**
 * Alimento Page JavaScript
 * Funcionalidades para la página Alimento.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('alimentoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('alimentoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('alimentoModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombreAlimento = document.getElementById('txtNombreAlimento');
    const hfAlimentoId = document.getElementById('hfAlimentoId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombreAlimento) txtNombreAlimento.value = '';
    if (hfAlimentoId) hfAlimentoId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Alimento';
}

// Función para confirmar eliminación
function confirmDeleteAlimento(button) {
    // Obtener el ID del alimento del CommandArgument
    const alimentoId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El alimento será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfAlimentoIdEliminar = document.getElementById('hfAlimentoIdEliminar');
            if (hfAlimentoIdEliminar) {
                hfAlimentoIdEliminar.value = alimentoId;
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

// Función para abrir modal de nuevo alimento
function openNewAlimentoModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('alimentoModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteAlimento = confirmDeleteAlimento;
window.openNewAlimentoModal = openNewAlimentoModal;

