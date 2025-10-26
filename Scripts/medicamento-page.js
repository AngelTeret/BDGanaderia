/**
 * Medicamento Page JavaScript
 * Funcionalidades para la página Medicamento.aspx
 */

// Función para mostrar modal de edición (llamada desde el servidor)
function showEditModal() {
    const modal = document.getElementById('medicamentoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para mostrar modal
function showModal() {
    const modal = document.getElementById('medicamentoModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Función para cerrar modal
function closeModal() {
    const modal = document.getElementById('medicamentoModal');
    if (modal) {
        modal.style.display = 'none';
        clearForm();
    }
}

// Función para limpiar formulario
function clearForm() {
    const txtNombreMedicamento = document.getElementById('txtNombreMedicamento');
    const hfMedicamentoId = document.getElementById('hfMedicamentoId');
    const modalTitle = document.getElementById('modalTitle');
    
    if (txtNombreMedicamento) txtNombreMedicamento.value = '';
    if (hfMedicamentoId) hfMedicamentoId.value = '0';
    if (modalTitle) modalTitle.innerText = 'Nuevo Medicamento';
}

// Función para confirmar eliminación
function confirmDeleteMedicamento(button) {
    // Obtener el ID del medicamento del CommandArgument
    const medicamentoId = button.getAttribute('data-command-argument');
    
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción! El medicamento será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Guardar el ID en el campo oculto
            const hfMedicamentoIdEliminar = document.getElementById('hfMedicamentoIdEliminar');
            if (hfMedicamentoIdEliminar) {
                hfMedicamentoIdEliminar.value = medicamentoId;
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

// Función para abrir modal de nuevo medicamento
function openNewMedicamentoModal() {
    clearForm();
    showModal();
}

// Cerrar modal al hacer clic fuera de él
window.addEventListener('click', function(event) {
    const modal = document.getElementById('medicamentoModal');
    if (event.target === modal) {
        closeModal();
    }
});

// Hacer funciones disponibles globalmente
window.showEditModal = showEditModal;
window.showModal = showModal;
window.closeModal = closeModal;
window.clearForm = clearForm;
window.confirmDeleteMedicamento = confirmDeleteMedicamento;
window.openNewMedicamentoModal = openNewMedicamentoModal;

