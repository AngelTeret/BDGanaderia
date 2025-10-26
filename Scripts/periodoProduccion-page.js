/**
 * Periodo Produccion Page JavaScript
 * Funcionalidades para la página PeriodoProduccion.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('periodoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('periodoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('periodoModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombrePeriodo = document.getElementById('txtNombrePeriodo');
    const hfPeriodoId = document.getElementById('hfPeriodoId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombrePeriodo) txtNombrePeriodo.value = '';
    if (hfPeriodoId) hfPeriodoId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Período de Producción';
}

// Función para confirmar eliminación
function confirmDeletePeriodo(button) {
    // Obtener el ID del período del CommandArgument
    const periodoId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El período de producción será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfPeriodoIdEliminar = document.getElementById('hfPeriodoIdEliminar');
            if (hfPeriodoIdEliminar) {
                hfPeriodoIdEliminar.value = periodoId;
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

// Función para abrir modal de nuevo período
function openNewPeriodoModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('periodoModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeletePeriodo = confirmDeletePeriodo;
window.openNewPeriodoModal = openNewPeriodoModal;

