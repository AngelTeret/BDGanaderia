/**
 * Ordenador Page JavaScript
 * Funcionalidades para la página Ordenador.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('ordenadorModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('ordenadorModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('ordenadorModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombreOrdenador = document.getElementById('txtNombreOrdenador');
    const hfOrdenadorId = document.getElementById('hfOrdenadorId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombreOrdenador) txtNombreOrdenador.value = '';
    if (hfOrdenadorId) hfOrdenadorId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Ordeñador';
}

// Función para confirmar eliminación
function confirmDeleteOrdenador(button) {
    // Obtener el ID del ordeñador del CommandArgument
    const ordenadorId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El ordeñador será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfOrdenadorIdEliminar = document.getElementById('hfOrdenadorIdEliminar');
            if (hfOrdenadorIdEliminar) {
                hfOrdenadorIdEliminar.value = ordenadorId;
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

// Función para abrir modal de nuevo ordeñador
function openNewOrdenadorModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('ordenadorModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteOrdenador = confirmDeleteOrdenador;
window.openNewOrdenadorModal = openNewOrdenadorModal;

