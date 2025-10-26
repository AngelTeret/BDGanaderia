/**
 * Racion Alimento Page JavaScript
 * Funcionalidades para la página RacionAlimento.aspx
 */

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('asignacionModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('asignacionModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const ddlRacion = document.getElementById('ddlRacion');
    const ddlAlimento = document.getElementById('ddlAlimento');
    const txtCantidad = document.getElementById('txtCantidad');
    const modalTitle = document.getElementById('modalTitle');
    
    if (ddlRacion && ddlRacion.options.length > 0) {
        ddlRacion.selectedIndex = 0;
        ddlRacion.disabled = false;
    }
    if (ddlAlimento && ddlAlimento.options.length > 0) {
        ddlAlimento.selectedIndex = 0;
        ddlAlimento.disabled = false;
    }
    if (txtCantidad) txtCantidad.value = '';
    if (modalTitle) modalTitle.innerText = 'Nueva Asignación de Alimento';
}

// Función para confirmar eliminación
function confirmDeleteAsignacion(button) {
    // Obtener los IDs de la ración y alimento
    const racionId = button.getAttribute('data-racion-id');
    const alimentoId = button.getAttribute('data-alimento-id');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! La asignación será eliminada permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar los IDs en los campos ocultos
            const hfRacionIdEliminar = document.getElementById('hfRacionIdEliminar');
            const hfAlimentoIdEliminar = document.getElementById('hfAlimentoIdEliminar');
            
            if (hfRacionIdEliminar && hfAlimentoIdEliminar) {
                hfRacionIdEliminar.value = racionId;
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

// Función para abrir modal de nueva asignación
function openNewAsignacionModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('asignacionModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteAsignacion = confirmDeleteAsignacion;
window.openNewAsignacionModal = openNewAsignacionModal;

