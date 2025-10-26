/**
 * Tratamiento Medicamento Page JavaScript
 * Funcionalidades para la página TratamientoMedicamento.aspx
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
    const ddlTratamiento = document.getElementById('ddlTratamiento');
    const ddlMedicamento = document.getElementById('ddlMedicamento');
    const txtDosis = document.getElementById('txtDosis');
    const modalTitle = document.getElementById('modalTitle');
    
    if (ddlTratamiento && ddlTratamiento.options.length > 0) {
        ddlTratamiento.selectedIndex = 0;
        ddlTratamiento.disabled = false;
    }
    if (ddlMedicamento && ddlMedicamento.options.length > 0) {
        ddlMedicamento.selectedIndex = 0;
        ddlMedicamento.disabled = false;
    }
    if (txtDosis) txtDosis.value = '';
    if (modalTitle) modalTitle.innerText = 'Nueva Asignación de Medicamento';
}

// Función para confirmar eliminación
function confirmDeleteAsignacion(button) {
    // Obtener los IDs de tratamiento y medicamento
    const tratamientoId = button.getAttribute('data-tratamiento-id');
    const medicamentoId = button.getAttribute('data-medicamento-id');
    
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
            const hfTratamientoIdEliminar = document.getElementById('hfTratamientoIdEliminar');
            const hfMedicamentoIdEliminar = document.getElementById('hfMedicamentoIdEliminar');
            
            if (hfTratamientoIdEliminar && hfMedicamentoIdEliminar) {
                hfTratamientoIdEliminar.value = tratamientoId;
                hfMedicamentoIdEliminar.value = medicamentoId;
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

