/**
 * Animal Control Lechero Page JavaScript
 * Funcionalidades para la página AnimalControlLechero.aspx
 */

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
    const ddlControl = document.getElementById('ddlControl');
    const txtLitrosLeche = document.getElementById('txtLitrosLeche');
    const modalTitle = document.getElementById('modalTitle');
    
    // Limpiar campos ocultos para evitar confusión entre crear y editar
    const hfAnimalId = document.getElementById('hfAnimalId');
    const hfControlId = document.getElementById('hfControlId');
    
    if (ddlAnimal && ddlAnimal.options.length > 0) {
        ddlAnimal.selectedIndex = 0;
        ddlAnimal.disabled = false;
    }
    if (ddlControl && ddlControl.options.length > 0) {
        ddlControl.selectedIndex = 0;
        ddlControl.disabled = false;
    }
    if (txtLitrosLeche) txtLitrosLeche.value = '';
    if (hfAnimalId) hfAnimalId.value = '';
    if (hfControlId) hfControlId.value = '';
    if (modalTitle) modalTitle.innerText = 'Nuevo Registro de Control Lechero';
}

// Función para confirmar eliminación
function confirmDeleteRegistro(button) {
    // Obtener los IDs del animal y control
    const animalId = button.getAttribute('data-animal-id');
    const controlId = button.getAttribute('data-control-id');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El registro será eliminado permanentemente.",
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
            const hfControlIdEliminar = document.getElementById('hfControlIdEliminar');
            
            if (hfAnimalIdEliminar && hfControlIdEliminar) {
                hfAnimalIdEliminar.value = animalId;
                hfControlIdEliminar.value = controlId;
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
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteRegistro = confirmDeleteRegistro;
window.openNewRegistroModal = openNewRegistroModal;

