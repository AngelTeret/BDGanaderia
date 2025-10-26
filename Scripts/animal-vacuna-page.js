/**
 * Animal Vacuna Page JavaScript
 * Funcionalidades para la página AnimalVacuna.aspx
 */

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('vacunacionModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('vacunacionModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const ddlAnimal = document.getElementById('ddlAnimal');
    const ddlVacuna = document.getElementById('ddlVacuna');
    const txtFechaAplicacion = document.getElementById('txtFechaAplicacion');
    const modalTitle = document.getElementById('modalTitle');
    
    if (ddlAnimal && ddlAnimal.options.length > 0) ddlAnimal.selectedIndex = 0;
    if (ddlVacuna && ddlVacuna.options.length > 0) ddlVacuna.selectedIndex = 0;
    if (txtFechaAplicacion) txtFechaAplicacion.value = '';
    if (modalTitle) modalTitle.innerText = 'Nueva Vacunación';
}

// Función para confirmar eliminación
function confirmDeleteVacunacion(button) {
    // Obtener los IDs del animal, vacuna y fecha
    const animalId = button.getAttribute('data-animal-id');
    const vacunaId = button.getAttribute('data-vacuna-id');
    const fechaAplicacion = button.getAttribute('data-fecha-aplicacion');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! La vacunación será eliminada permanentemente.",
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
            const hfVacunaIdEliminar = document.getElementById('hfVacunaIdEliminar');
            const hfFechaAplicacionEliminar = document.getElementById('hfFechaAplicacionEliminar');
            
            if (hfAnimalIdEliminar && hfVacunaIdEliminar && hfFechaAplicacionEliminar) {
                hfAnimalIdEliminar.value = animalId;
                hfVacunaIdEliminar.value = vacunaId;
                hfFechaAplicacionEliminar.value = fechaAplicacion;
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

// Función para abrir modal de nueva vacunación
function openNewVacunacionModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('vacunacionModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteVacunacion = confirmDeleteVacunacion;
window.openNewVacunacionModal = openNewVacunacionModal;

