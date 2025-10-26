/**
 * Animal Pesaje Page JavaScript
 * Funcionalidades para la página AnimalPesaje.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('pesajeModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('pesajeModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('pesajeModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const ddlAnimal = document.getElementById('ddlAnimal');
    const txtFechaPesaje = document.getElementById('txtFechaPesaje');
    const txtPeso = document.getElementById('txtPeso');
    const hfPesajeId = document.getElementById('hfPesajeId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (ddlAnimal && ddlAnimal.options.length > 0) ddlAnimal.selectedIndex = 0;
    if (txtFechaPesaje) txtFechaPesaje.value = '';
    if (txtPeso) txtPeso.value = '';
    if (hfPesajeId) hfPesajeId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Pesaje';
}

// Función para confirmar eliminación
function confirmDeletePesaje(button) {
    // Obtener el ID del pesaje del CommandArgument
    const pesajeId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El pesaje será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfPesajeIdEliminar = document.getElementById('hfPesajeIdEliminar');
            if (hfPesajeIdEliminar) {
                hfPesajeIdEliminar.value = pesajeId;
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

// Función para abrir modal de nuevo pesaje
function openNewPesajeModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('pesajeModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeletePesaje = confirmDeletePesaje;
window.openNewPesajeModal = openNewPesajeModal;

