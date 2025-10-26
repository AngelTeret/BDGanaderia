/**
 * TIPO ANIMAL PAGE - BDGanaderia
 * JavaScript para la gestión de tipos de animal
 */

(function() {
    'use strict';

    // Variables globales
    let modal = null;

    /**
     * Inicializa la página
     */
    function initTipoAnimalPage() {
        console.log('🐾 Inicializando página de Tipos de Animal...');
        
        // Obtener elementos
        modal = document.getElementById('tipoAnimalModal');
        
        console.log('🔍 Elementos encontrados:', {
            modal: !!modal
        });
        
        if (!modal) {
            console.error('❌ Modal no encontrado');
            return;
        }
        
        // Configurar event listeners
        setupEventListeners();
        
        console.log('✅ Página de Tipos de Animal inicializada correctamente');
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
     * Abre el modal para nuevo tipo de animal
     */
    function openNewTipoAnimalModal() {
        console.log('➕ Abriendo modal para nuevo tipo de animal');
        
        if (!modal) {
            console.error('❌ Modal no encontrado');
            return;
        }
        
        // Limpiar formulario
        clearForm();
        
        // Cambiar título
        const modalTitle = document.getElementById('modalTitle');
        if (modalTitle) {
            modalTitle.innerText = 'Nuevo Tipo de Animal';
        }
        
        // Mostrar modal
        modal.style.display = 'block';
        
        // Enfocar el primer campo
        const txtNombreTipo = document.getElementById('txtNombreTipo');
        if (txtNombreTipo) {
            setTimeout(() => {
                txtNombreTipo.focus();
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
        
        const txtNombreTipo = document.getElementById('txtNombreTipo');
        const hfTipoAnimalId = document.getElementById('hfTipoAnimalId');
        
        if (txtNombreTipo) {
            txtNombreTipo.value = '';
        }
        
        if (hfTipoAnimalId) {
            hfTipoAnimalId.value = '0';
        }
        
        console.log('✅ Formulario limpiado');
    }

    /**
     * Confirma la eliminación de un tipo de animal
     */
    function confirmDeleteTipoAnimal(button) {
        console.log('🗑️ Confirmando eliminación de tipo de animal');
        
        const tipoAnimalId = button.getAttribute('data-command-argument');
        
        if (!tipoAnimalId) {
            console.error('❌ ID del tipo de animal no encontrado');
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
                console.log('✅ Eliminación confirmada para ID:', tipoAnimalId);
                
                // Establecer el ID en el campo oculto
                const hfTipoAnimalIdEliminar = document.getElementById('hfTipoAnimalIdEliminar');
                if (hfTipoAnimalIdEliminar) {
                    hfTipoAnimalIdEliminar.value = tipoAnimalId;
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
        
        const txtNombreTipo = document.getElementById('txtNombreTipo');
        
        if (!txtNombreTipo || !txtNombreTipo.value.trim()) {
            Swal.fire({
                icon: 'warning',
                title: 'Campo requerido',
                text: 'El nombre del tipo de animal es obligatorio',
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
        document.addEventListener('DOMContentLoaded', initTipoAnimalPage);
    } else {
        initTipoAnimalPage();
    }

    // API pública
    window.TipoAnimalPage = {
        openNewModal: openNewTipoAnimalModal,
        closeModal: closeModal,
        clearForm: clearForm,
        confirmDelete: confirmDeleteTipoAnimal,
        validateForm: validateForm,
        showAlert: showAlert
    };

    // Funciones globales para compatibilidad
    window.openNewTipoAnimalModal = openNewTipoAnimalModal;
    window.closeModal = closeModal;
    window.confirmDeleteTipoAnimal = confirmDeleteTipoAnimal;

    console.log('🎉 TipoAnimal Page cargado y listo');

})();
