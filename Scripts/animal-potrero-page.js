/**
 * Animal Potrero Page JavaScript
 * Funcionalidades para la página AnimalPotrero.aspx
 */

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('movimientoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('movimientoModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const ddlAnimal = document.getElementById('ddlAnimal');
    const ddlPotrero = document.getElementById('ddlPotrero');
    const txtFechaEntrada = document.getElementById('txtFechaEntrada');
    const txtFechaSalida = document.getElementById('txtFechaSalida');
    const modalTitle = document.getElementById('modalTitle');
    
    if (ddlAnimal && ddlAnimal.options.length > 0) ddlAnimal.selectedIndex = 0;
    if (ddlPotrero && ddlPotrero.options.length > 0) ddlPotrero.selectedIndex = 0;
    if (txtFechaEntrada) txtFechaEntrada.value = '';
    if (txtFechaSalida) txtFechaSalida.value = '';
    if (modalTitle) modalTitle.innerText = 'Nuevo Movimiento a Potrero';
}

// Función para confirmar eliminación
function confirmDeleteMovimiento(button) {
    // Obtener los IDs del animal, potrero y fecha
    const animalId = button.getAttribute('data-animal-id');
    const potreroId = button.getAttribute('data-potrero-id');
    const fechaEntrada = button.getAttribute('data-fecha-entrada');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El movimiento será eliminado permanentemente.",
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
            const hfPotreroIdEliminar = document.getElementById('hfPotreroIdEliminar');
            const hfFechaEntradaEliminar = document.getElementById('hfFechaEntradaEliminar');
            
            if (hfAnimalIdEliminar && hfPotreroIdEliminar && hfFechaEntradaEliminar) {
                hfAnimalIdEliminar.value = animalId;
                hfPotreroIdEliminar.value = potreroId;
                hfFechaEntradaEliminar.value = fechaEntrada;
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

// Función para abrir modal de nuevo movimiento
function openNewMovimientoModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('movimientoModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteMovimiento = confirmDeleteMovimiento;
window.openNewMovimientoModal = openNewMovimientoModal;

