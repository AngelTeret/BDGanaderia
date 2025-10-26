/**
 * ANIMAL PAGE - BDGanaderia
 * JavaScript para la gestión de animales
 */

(function() {
    'use strict';

    // Variables globales
    let modal = null;

    /**
     * Inicializa la página
     */
    function initAnimalPage() {
        console.log('🐄 Inicializando página de Animales...');
        
        // Obtener elementos
        modal = document.getElementById('animalModal');
        
        console.log('🔍 Elementos encontrados:', {
            modal: !!modal
        });
        
        if (!modal) {
            console.error('❌ Modal no encontrado');
            return;
        }
        
        // Configurar event listeners
        setupEventListeners();
        
        console.log('✅ Página de Animales inicializada correctamente');
    }

    /**
     * Configura los event listeners
     */
    function setupEventListeners() {
        // Cerrar modal al hacer clic fuera de él
        window.addEventListener('click', function(event) {
            if (event.target === modal) {
                closeModal();
            }
        });
        
        // Cerrar modal con tecla Escape
        document.addEventListener('keydown', function(event) {
            if (event.key === 'Escape' && modal.style.display === 'block') {
                closeModal();
            }
        });
        
        console.log('📡 Event listeners configurados');
    }

    /**
     * Abre el modal para nuevo animal
     */
    function openNewAnimalModal() {
        console.log('➕ Abriendo modal para nuevo animal');
        
        if (!modal) {
            console.error('❌ Modal no encontrado');
            return;
        }
        
        // Limpiar formulario
        clearForm();
        
        // Cambiar título
        const modalTitle = document.getElementById('modalTitle');
        if (modalTitle) {
            modalTitle.innerText = 'Nuevo Animal';
        }
        
        // Mostrar modal
        modal.style.display = 'block';
        
        // Enfocar el primer campo
        const txtNombreAnimal = document.getElementById('txtNombreAnimal');
        if (txtNombreAnimal) {
            setTimeout(() => {
                txtNombreAnimal.focus();
            }, 100);
        }
        
        console.log('✅ Modal abierto');
    }

    /**
     * Cierra el modal
     */
    function closeModal() {
        console.log('❌ Cerrando modal');
        
        if (!modal) {
            console.error('❌ Modal no encontrado');
            return;
        }
        
        modal.style.display = 'none';
        
        console.log('✅ Modal cerrado');
    }

    /**
     * Limpia el formulario
     */
    function clearForm() {
        console.log('🧹 Limpiando formulario');
        
        const txtNombreAnimal = document.getElementById('txtNombreAnimal');
        const txtFechaNacimiento = document.getElementById('txtFechaNacimiento');
        const ddlSexo = document.getElementById('ddlSexo');
        const txtPeso = document.getElementById('txtPeso');
        const ddlRaza = document.getElementById('ddlRaza');
        const ddlTipoAnimal = document.getElementById('ddlTipoAnimal');
        const ddlEstadoAnimal = document.getElementById('ddlEstadoAnimal');
        const hfAnimalId = document.getElementById('hfAnimalId');
        
        if (txtNombreAnimal) txtNombreAnimal.value = '';
        if (txtFechaNacimiento) txtFechaNacimiento.value = '';
        if (ddlSexo) ddlSexo.selectedIndex = 0;
        if (txtPeso) txtPeso.value = '';
        if (ddlRaza) ddlRaza.selectedIndex = 0;
        if (ddlTipoAnimal) ddlTipoAnimal.selectedIndex = 0;
        if (ddlEstadoAnimal) ddlEstadoAnimal.selectedIndex = 0;
        if (hfAnimalId) hfAnimalId.value = '0';
        
        console.log('✅ Formulario limpiado');
    }

    /**
     * Confirma la eliminación de un animal
     */
    function confirmDeleteAnimal(button) {
        console.log('🗑️ Confirmando eliminación de animal');
        
        const animalId = button.getAttribute('data-command-argument');
        
        if (!animalId) {
            console.error('❌ ID del animal no encontrado');
            return false;
        }
        
        Swal.fire({
            title: '¿Estás seguro?',
            text: 'Esta acción no se puede deshacer',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#e74c3c',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                console.log('✅ Eliminación confirmada para ID:', animalId);
                
                // Establecer el ID en el campo oculto
                const hfAnimalIdEliminar = document.getElementById('hfAnimalIdEliminar');
                if (hfAnimalIdEliminar) {
                    hfAnimalIdEliminar.value = animalId;
                }
                
                // Ejecutar el botón oculto
                const btnEliminarOculto = document.getElementById('btnEliminarOculto');
                if (btnEliminarOculto) {
                    btnEliminarOculto.click();
                }
            } else {
                console.log('❌ Eliminación cancelada');
            }
        });
        
        return false; // Prevenir el comportamiento por defecto
    }

    /**
     * Valida el formulario antes de enviar
     */
    function validateForm() {
        console.log('✅ Validando formulario');
        
        const txtNombreAnimal = document.getElementById('txtNombreAnimal');
        const txtFechaNacimiento = document.getElementById('txtFechaNacimiento');
        const txtPeso = document.getElementById('txtPeso');
        const ddlRaza = document.getElementById('ddlRaza');
        const ddlTipoAnimal = document.getElementById('ddlTipoAnimal');
        const ddlEstadoAnimal = document.getElementById('ddlEstadoAnimal');
        
        if (!txtNombreAnimal || !txtNombreAnimal.value.trim()) {
            Swal.fire({
                icon: 'warning',
                title: 'Campo requerido',
                text: 'El nombre del animal es obligatorio',
                confirmButtonText: 'Aceptar',
                confirmButtonColor: '#f39c12'
            });
            return false;
        }
        
        if (!txtFechaNacimiento || !txtFechaNacimiento.value) {
            Swal.fire({
                icon: 'warning',
                title: 'Campo requerido',
                text: 'La fecha de nacimiento es obligatoria',
                confirmButtonText: 'Aceptar',
                confirmButtonColor: '#f39c12'
            });
            return false;
        }
        
        if (!txtPeso || !txtPeso.value || isNaN(txtPeso.value)) {
            Swal.fire({
                icon: 'warning',
                title: 'Campo requerido',
                text: 'El peso debe ser un número válido',
                confirmButtonText: 'Aceptar',
                confirmButtonColor: '#f39c12'
            });
            return false;
        }
        
        if (!ddlRaza || ddlRaza.value === '0') {
            Swal.fire({
                icon: 'warning',
                title: 'Campo requerido',
                text: 'Debe seleccionar una raza',
                confirmButtonText: 'Aceptar',
                confirmButtonColor: '#f39c12'
            });
            return false;
        }
        
        if (!ddlTipoAnimal || ddlTipoAnimal.value === '0') {
            Swal.fire({
                icon: 'warning',
                title: 'Campo requerido',
                text: 'Debe seleccionar un tipo de animal',
                confirmButtonText: 'Aceptar',
                confirmButtonColor: '#f39c12'
            });
            return false;
        }
        
        if (!ddlEstadoAnimal || ddlEstadoAnimal.value === '0') {
            Swal.fire({
                icon: 'warning',
                title: 'Campo requerido',
                text: 'Debe seleccionar un estado',
                confirmButtonText: 'Aceptar',
                confirmButtonColor: '#f39c12'
            });
            return false;
        }
        
        console.log('✅ Formulario válido');
        return true;
    }

    /**
     * Muestra una alerta personalizada
     */
    function showAlert(message, type = 'info') {
        console.log(`📢 Mostrando alerta: ${message} (${type})`);
        
        const config = {
            text: message,
            confirmButtonText: 'Aceptar',
            confirmButtonColor: '#3498db'
        };
        
        switch (type) {
            case 'success':
                config.icon = 'success';
                config.confirmButtonColor = '#27ae60';
                break;
            case 'error':
            case 'danger':
                config.icon = 'error';
                config.confirmButtonColor = '#e74c3c';
                break;
            case 'warning':
                config.icon = 'warning';
                config.confirmButtonColor = '#f39c12';
                break;
            case 'info':
            default:
                config.icon = 'info';
                config.confirmButtonColor = '#3498db';
                break;
        }
        
        Swal.fire(config);
    }

    // Inicialización automática
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initAnimalPage);
    } else {
        initAnimalPage();
    }

    // API pública
    window.AnimalPage = {
        openNewModal: openNewAnimalModal,
        closeModal: closeModal,
        clearForm: clearForm,
        confirmDelete: confirmDeleteAnimal,
        validateForm: validateForm,
        showAlert: showAlert
    };

    // Funciones globales para compatibilidad
    window.openNewAnimalModal = openNewAnimalModal;
    window.closeModal = closeModal;
    window.confirmDeleteAnimal = confirmDeleteAnimal;

    console.log('🎉 Animal Page cargado y listo');

})();
