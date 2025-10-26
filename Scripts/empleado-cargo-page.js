/**
 * Empleado Cargo Page JavaScript
 * Funcionalidades para la página EmpleadoCargo.aspx
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
    const ddlEmpleado = document.getElementById('ddlEmpleado');
    const ddlCargo = document.getElementById('ddlCargo');
    const txtFechaAsignacion = document.getElementById('txtFechaAsignacion');
    const modalTitle = document.getElementById('modalTitle');
    
    if (ddlEmpleado && ddlEmpleado.options.length > 0) ddlEmpleado.selectedIndex = 0;
    if (ddlCargo && ddlCargo.options.length > 0) ddlCargo.selectedIndex = 0;
    if (txtFechaAsignacion) txtFechaAsignacion.value = '';
    if (modalTitle) modalTitle.innerText = 'Nueva Asignación de Cargo';
}

// Función para confirmar eliminación
function confirmDeleteAsignacion(button) {
    // Obtener los IDs del empleado, cargo y fecha
    const empleadoId = button.getAttribute('data-empleado-id');
    const cargoId = button.getAttribute('data-cargo-id');
    const fechaAsignacion = button.getAttribute('data-fecha-asignacion');
    
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
            const hfEmpleadoIdEliminar = document.getElementById('hfEmpleadoIdEliminar');
            const hfCargoIdEliminar = document.getElementById('hfCargoIdEliminar');
            const hfFechaAsignacionEliminar = document.getElementById('hfFechaAsignacionEliminar');
            
            if (hfEmpleadoIdEliminar && hfCargoIdEliminar && hfFechaAsignacionEliminar) {
                hfEmpleadoIdEliminar.value = empleadoId;
                hfCargoIdEliminar.value = cargoId;
                hfFechaAsignacionEliminar.value = fechaAsignacion;
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

