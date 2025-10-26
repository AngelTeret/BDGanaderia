/**
 * Control Nutricional Page JavaScript
 * Funcionalidades para la página ControlNutricional.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('controlModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('controlModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('controlModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const ddlAnimal = document.getElementById('ddlAnimal');
    const txtFechaEvaluacion = document.getElementById('txtFechaEvaluacion');
    const txtCondicionCorporal = document.getElementById('txtCondicionCorporal');
    const hfControlId = document.getElementById('hfControlId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (ddlAnimal && ddlAnimal.options.length > 0) ddlAnimal.selectedIndex = 0;
    if (txtFechaEvaluacion) txtFechaEvaluacion.value = '';
    if (txtCondicionCorporal) txtCondicionCorporal.value = '';
    if (hfControlId) hfControlId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Control Nutricional';
}

// Función para confirmar eliminación
function confirmDeleteControl(button) {
    // Obtener el ID del control del CommandArgument
    const controlId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El control nutricional será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfControlIdEliminar = document.getElementById('hfControlIdEliminar');
            if (hfControlIdEliminar) {
                hfControlIdEliminar.value = controlId;
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

// Función para abrir modal de nuevo control
function openNewControlModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('controlModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteControl = confirmDeleteControl;
window.openNewControlModal = openNewControlModal;

