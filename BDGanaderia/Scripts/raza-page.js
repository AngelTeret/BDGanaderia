/**
 * Raza Page JavaScript
 * Funcionalidades para la página Raza.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('razaModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('razaModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('razaModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombreRaza = document.getElementById('txtNombreRaza');
    const hfRazaId = document.getElementById('hfRazaId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombreRaza) txtNombreRaza.value = '';
    if (hfRazaId) hfRazaId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nueva Raza';
}

// Función para confirmar eliminación
function confirmDeleteRaza(button) {
    // Obtener el ID de la raza del CommandArgument
    const razaId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! La raza será eliminada permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfRazaIdEliminar = document.getElementById('hfRazaIdEliminar');
            if (hfRazaIdEliminar) {
                hfRazaIdEliminar.value = razaId;
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

// Función para abrir modal de nueva raza
function openNewRazaModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('razaModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteRaza = confirmDeleteRaza;
window.openNewRazaModal = openNewRazaModal;