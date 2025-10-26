/**
 * Registro Sanitario Page JavaScript
 * Funcionalidades para la página RegistroSanitario.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('registroModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('registroModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('registroModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const ddlAnimal = document.getElementById('ddlAnimal');
    const txtFechaRegistro = document.getElementById('txtFechaRegistro');
    const txtDescripcion = document.getElementById('txtDescripcion');
    const hfRegistroId = document.getElementById('hfRegistroId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (ddlAnimal && ddlAnimal.options.length > 0) ddlAnimal.selectedIndex = 0;
    if (txtFechaRegistro) txtFechaRegistro.value = '';
    if (txtDescripcion) txtDescripcion.value = '';
    if (hfRegistroId) hfRegistroId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Registro Sanitario';
}

// Función para confirmar eliminación
function confirmDeleteRegistro(button) {
    // Obtener el ID del registro del CommandArgument
    const registroId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El registro sanitario será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfRegistroIdEliminar = document.getElementById('hfRegistroIdEliminar');
            if (hfRegistroIdEliminar) {
                hfRegistroIdEliminar.value = registroId;
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

// Función para abrir modal de nuevo registro
function openNewRegistroModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('registroModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteRegistro = confirmDeleteRegistro;
window.openNewRegistroModal = openNewRegistroModal;

