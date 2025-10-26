/**
 * Veterinario Page JavaScript
 * Funcionalidades para la página Veterinario.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('veterinarioModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('veterinarioModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('veterinarioModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombreVeterinario = document.getElementById('txtNombreVeterinario');
    const hfVeterinarioId = document.getElementById('hfVeterinarioId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombreVeterinario) txtNombreVeterinario.value = '';
    if (hfVeterinarioId) hfVeterinarioId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Veterinario';
}

// Función para confirmar eliminación
function confirmDeleteVeterinario(button) {
    // Obtener el ID del veterinario del CommandArgument
    const veterinarioId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El veterinario será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfVeterinarioIdEliminar = document.getElementById('hfVeterinarioIdEliminar');
            if (hfVeterinarioIdEliminar) {
                hfVeterinarioIdEliminar.value = veterinarioId;
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

// Función para abrir modal de nuevo veterinario
function openNewVeterinarioModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('veterinarioModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteVeterinario = confirmDeleteVeterinario;
window.openNewVeterinarioModal = openNewVeterinarioModal;

