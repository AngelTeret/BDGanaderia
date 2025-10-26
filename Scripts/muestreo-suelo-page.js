/**
 * Muestreo Suelo Page JavaScript
 * Funcionalidades para la página MuestreoSuelo.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('muestreoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('muestreoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('muestreoModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const ddlPotrero = document.getElementById('ddlPotrero');
    const txtFechaMuestreo = document.getElementById('txtFechaMuestreo');
    const txtpH = document.getElementById('txtpH');
    const txtMateriaOrganica = document.getElementById('txtMateriaOrganica');
    const hfMuestreoId = document.getElementById('hfMuestreoId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (ddlPotrero && ddlPotrero.options.length > 0) ddlPotrero.selectedIndex = 0;
    if (txtFechaMuestreo) txtFechaMuestreo.value = '';
    if (txtpH) txtpH.value = '';
    if (txtMateriaOrganica) txtMateriaOrganica.value = '';
    if (hfMuestreoId) hfMuestreoId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Muestreo de Suelo';
}

// Función para confirmar eliminación
function confirmDeleteMuestreo(button) {
    // Obtener el ID del muestreo del CommandArgument
    const muestreoId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El muestreo de suelo será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfMuestreoIdEliminar = document.getElementById('hfMuestreoIdEliminar');
            if (hfMuestreoIdEliminar) {
                hfMuestreoIdEliminar.value = muestreoId;
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

// Función para abrir modal de nuevo muestreo
function openNewMuestreoModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('muestreoModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteMuestreo = confirmDeleteMuestreo;
window.openNewMuestreoModal = openNewMuestreoModal;

