/**
 * Tipo Alimento Page JavaScript
 * Funcionalidades para la página TipoAlimento.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('tipoAlimentoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('tipoAlimentoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('tipoAlimentoModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombreTipo = document.getElementById('txtNombreTipo');
    const hfTipoAlimentoId = document.getElementById('hfTipoAlimentoId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombreTipo) txtNombreTipo.value = '';
    if (hfTipoAlimentoId) hfTipoAlimentoId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Tipo de Alimento';
}

// Función para confirmar eliminación
function confirmDeleteTipoAlimento(button) {
    // Obtener el ID del tipo de alimento del CommandArgument
    const tipoAlimentoId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El tipo de alimento será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfTipoAlimentoIdEliminar = document.getElementById('hfTipoAlimentoIdEliminar');
            if (hfTipoAlimentoIdEliminar) {
                hfTipoAlimentoIdEliminar.value = tipoAlimentoId;
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

// Función para abrir modal de nuevo tipo de alimento
function openNewTipoAlimentoModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('tipoAlimentoModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteTipoAlimento = confirmDeleteTipoAlimento;
window.openNewTipoAlimentoModal = openNewTipoAlimentoModal;

