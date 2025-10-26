/**
 * Animal Racion Page JavaScript
 * Funcionalidades para la página AnimalRacion.aspx
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
    const ddlAnimal = document.getElementById('ddlAnimal');
    const ddlRacion = document.getElementById('ddlRacion');
    const txtFechaAsignacion = document.getElementById('txtFechaAsignacion');
    const modalTitle = document.getElementById('modalTitle');
    
    if (ddlAnimal && ddlAnimal.options.length > 0) ddlAnimal.selectedIndex = 0;
    if (ddlRacion && ddlRacion.options.length > 0) ddlRacion.selectedIndex = 0;
    if (txtFechaAsignacion) txtFechaAsignacion.value = '';
    if (modalTitle) modalTitle.innerText = 'Nueva Asignación de Ración';
}

// Función para confirmar eliminación
function confirmDeleteAsignacion(button) {
    // Obtener los IDs del animal, ración y fecha
    const animalId = button.getAttribute('data-animal-id');
    const racionId = button.getAttribute('data-racion-id');
    const fechaAsignacion = button.getAttribute('data-fecha-asignacion');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! La asignación de ración será eliminada permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar los IDs en los campos ocultos
            const hfAnimalIdEliminar = document.getElementById('hfAnimalIdEliminar');
            const hfRacionIdEliminar = document.getElementById('hfRacionIdEliminar');
            const hfFechaAsignacionEliminar = document.getElementById('hfFechaAsignacionEliminar');
            
            if (hfAnimalIdEliminar && hfRacionIdEliminar && hfFechaAsignacionEliminar) {
                hfAnimalIdEliminar.value = animalId;
                hfRacionIdEliminar.value = racionId;
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

