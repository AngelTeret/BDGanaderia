/**
 * Consumo Registro Page JavaScript
 * Funcionalidades para la página ConsumoRegistro.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('consumoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('consumoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('consumoModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const ddlAnimal = document.getElementById('ddlAnimal');
    const ddlAlimento = document.getElementById('ddlAlimento');
    const txtFechaConsumo = document.getElementById('txtFechaConsumo');
    const txtCantidad = document.getElementById('txtCantidad');
    const hfConsumoId = document.getElementById('hfConsumoId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (ddlAnimal && ddlAnimal.options.length > 0) ddlAnimal.selectedIndex = 0;
    if (ddlAlimento && ddlAlimento.options.length > 0) ddlAlimento.selectedIndex = 0;
    if (txtFechaConsumo) txtFechaConsumo.value = '';
    if (txtCantidad) txtCantidad.value = '';
    if (hfConsumoId) hfConsumoId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Registro de Consumo';
}

// Función para confirmar eliminación
function confirmDeleteConsumo(button) {
    // Obtener el ID del consumo del CommandArgument
    const consumoId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El registro de consumo será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfConsumoIdEliminar = document.getElementById('hfConsumoIdEliminar');
            if (hfConsumoIdEliminar) {
                hfConsumoIdEliminar.value = consumoId;
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
    const modal = document.getElementById('consumoModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteConsumo = confirmDeleteConsumo;
window.openNewRegistroModal = openNewRegistroModal;

