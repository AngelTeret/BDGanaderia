/**
 * Empleado Page JavaScript
 * Funcionalidades para la página Empleado.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('empleadoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('empleadoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('empleadoModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombreEmpleado = document.getElementById('txtNombreEmpleado');
    const txtFechaContratacion = document.getElementById('txtFechaContratacion');
    const hfEmpleadoId = document.getElementById('hfEmpleadoId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombreEmpleado) txtNombreEmpleado.value = '';
    if (txtFechaContratacion) txtFechaContratacion.value = '';
    if (hfEmpleadoId) hfEmpleadoId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Empleado';
}

// Función para confirmar eliminación
function confirmDeleteEmpleado(button) {
    // Obtener el ID del empleado del CommandArgument
    const empleadoId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El empleado será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfEmpleadoIdEliminar = document.getElementById('hfEmpleadoIdEliminar');
            if (hfEmpleadoIdEliminar) {
                hfEmpleadoIdEliminar.value = empleadoId;
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

// Función para abrir modal de nuevo empleado
function openNewEmpleadoModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('empleadoModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteEmpleado = confirmDeleteEmpleado;
window.openNewEmpleadoModal = openNewEmpleadoModal;

