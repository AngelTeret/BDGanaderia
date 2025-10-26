/**
 * Animal Tratamiento Page JavaScript
 * Funcionalidades para la página AnimalTratamiento.aspx
 */

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('tratamientoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('tratamientoModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const ddlAnimal = document.getElementById('ddlAnimal');
    const ddlTratamiento = document.getElementById('ddlTratamiento');
    const ddlVeterinario = document.getElementById('ddlVeterinario');
    const txtFechaTratamiento = document.getElementById('txtFechaTratamiento');
    const txtObservacion = document.getElementById('txtObservacion');
    const modalTitle = document.getElementById('modalTitle');
    
    if (ddlAnimal && ddlAnimal.options.length > 0) {
        ddlAnimal.selectedIndex = 0;
        ddlAnimal.disabled = false; // Asegurar que esté habilitado
    }
    if (ddlTratamiento && ddlTratamiento.options.length > 0) {
        ddlTratamiento.selectedIndex = 0;
        ddlTratamiento.disabled = false; // Asegurar que esté habilitado
    }
    if (ddlVeterinario && ddlVeterinario.options.length > 0) {
        ddlVeterinario.selectedIndex = 0;
        ddlVeterinario.disabled = false;
    }
    if (txtFechaTratamiento) {
        txtFechaTratamiento.value = '';
        txtFechaTratamiento.disabled = false; // Asegurar que esté habilitado
    }
    if (txtObservacion) txtObservacion.value = '';
    if (modalTitle) modalTitle.innerText = 'Nuevo Tratamiento';
}

// Función para confirmar eliminación
function confirmDeleteTratamiento(button) {
    // Obtener los IDs del animal, tratamiento y fecha
    const animalId = button.getAttribute('data-animal-id');
    const tratamientoId = button.getAttribute('data-tratamiento-id');
    const fechaTratamiento = button.getAttribute('data-fecha-tratamiento');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El tratamiento será eliminado permanentemente.",
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
            const hfTratamientoIdEliminar = document.getElementById('hfTratamientoIdEliminar');
            const hfFechaTratamientoEliminar = document.getElementById('hfFechaTratamientoEliminar');
            
            if (hfAnimalIdEliminar && hfTratamientoIdEliminar && hfFechaTratamientoEliminar) {
                hfAnimalIdEliminar.value = animalId;
                hfTratamientoIdEliminar.value = tratamientoId;
                hfFechaTratamientoEliminar.value = fechaTratamiento;
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

// Función para abrir modal de nuevo tratamiento
function openNewTratamientoModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('tratamientoModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteTratamiento = confirmDeleteTratamiento;
window.openNewTratamientoModal = openNewTratamientoModal;

