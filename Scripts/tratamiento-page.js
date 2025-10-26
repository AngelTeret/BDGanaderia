/**
 * Tratamiento Page JavaScript
 * Funcionalidades para la página Tratamiento.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('tratamientoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('tratamientoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('tratamientoModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombreTratamiento = document.getElementById('txtNombreTratamiento');
    const hfTratamientoId = document.getElementById('hfTratamientoId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombreTratamiento) txtNombreTratamiento.value = '';
    if (hfTratamientoId) hfTratamientoId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Tratamiento';
}

// Función para confirmar eliminación
function confirmDeleteTratamiento(button) {
    // Obtener el ID del tratamiento del CommandArgument
    const tratamientoId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El tratamiento será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfTratamientoIdEliminar = document.getElementById('hfTratamientoIdEliminar');
            if (hfTratamientoIdEliminar) {
                hfTratamientoIdEliminar.value = tratamientoId;
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

// Función para abrir modal de nuevo tratamiento
function openNewTratamientoModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('tratamientoModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteTratamiento = confirmDeleteTratamiento;
window.openNewTratamientoModal = openNewTratamientoModal;

