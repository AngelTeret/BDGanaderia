/**
 * Tipo Pasto Page JavaScript
 * Funcionalidades para la página TipoPasto.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('tipoPastoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('tipoPastoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('tipoPastoModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombrePasto = document.getElementById('txtNombrePasto');
    const hfTipoPastoId = document.getElementById('hfTipoPastoId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombrePasto) txtNombrePasto.value = '';
    if (hfTipoPastoId) hfTipoPastoId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Tipo de Pasto';
}

// Función para confirmar eliminación
function confirmDeleteTipoPasto(button) {
    // Obtener el ID del tipo de pasto del CommandArgument
    const tipoPastoId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El tipo de pasto será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfTipoPastoIdEliminar = document.getElementById('hfTipoPastoIdEliminar');
            if (hfTipoPastoIdEliminar) {
                hfTipoPastoIdEliminar.value = tipoPastoId;
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

// Función para abrir modal de nuevo tipo de pasto
function openNewTipoPastoModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('tipoPastoModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteTipoPasto = confirmDeleteTipoPasto;
window.openNewTipoPastoModal = openNewTipoPastoModal;

