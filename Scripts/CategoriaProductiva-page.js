/**
 * CATEGORIA PRODUCTIVA PAGE - BDGanaderia
 * JavaScript para la gestión de categorías productivas
 */

(function() {
    'use strict';

    // Variables globales
    let modal = null;

    /**
     * Inicializa la página
     */
    function initCategoriaPage() {
        console.log('🏷️ Inicializando página de Categorías Productivas...');
        
        // Obtener elementos
        modal = document.getElementById('categoriaModal');
        
        console.log('🔍 Elementos encontrados:', {
            modal: !!modal
        });
        
        if (!modal) {
            console.error('❌ Modal no encontrado');
            return;
        }
        
        // Configurar event listeners
        setupEventListeners();
        
        console.log('✅ Página de Categorías Productivas inicializada correctamente');
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
     * Abre el modal para nueva categoría
     */
    function openNewCategoriaModal() {
        console.log('➕ Abriendo modal para nueva categoría productiva');
        
        if (!modal) {
            console.error('❌ Modal no encontrado');
            return;
        }
        
        // Limpiar formulario
        clearForm();
        
        // Cambiar título
        const modalTitle = document.getElementById('modalTitle');
        if (modalTitle) {
            modalTitle.innerText = 'Nueva Categoría Productiva';
        }
        
        // Mostrar modal
        modal.style.display = 'block';
        
        // Enfocar el primer campo
        const txtNombreCategoria = document.getElementById('txtNombreCategoria');
        if (txtNombreCategoria) {
            setTimeout(() => {
                txtNombreCategoria.focus();
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
        
        const txtNombreCategoria = document.getElementById('txtNombreCategoria');
        const hfCategoriaId = document.getElementById('hfCategoriaId');
        
        if (txtNombreCategoria) {
            txtNombreCategoria.value = '';
        }
        
        if (hfCategoriaId) {
            hfCategoriaId.value = '0';
        }
        
        console.log('✅ Formulario limpiado');
    }

    /**
     * Confirma la eliminación de una categoría
     */
    function confirmDeleteCategoria(button) {
        console.log('🗑️ Confirmando eliminación de categoría productiva');
        
        const categoriaId = button.getAttribute('data-command-argument');
        
        if (!categoriaId) {
            console.error('❌ ID de la categoría no encontrado');
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
                console.log('✅ Eliminación confirmada para ID:', categoriaId);
                
                // Establecer el ID en el campo oculto
                const hfCategoriaIdEliminar = document.getElementById('hfCategoriaIdEliminar');
                if (hfCategoriaIdEliminar) {
                    hfCategoriaIdEliminar.value = categoriaId;
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
        
        const txtNombreCategoria = document.getElementById('txtNombreCategoria');
        
        if (!txtNombreCategoria || !txtNombreCategoria.value.trim()) {
            Swal.fire({
                icon: 'warning',
                title: 'Campo requerido',
                text: 'El nombre de la categoría es obligatorio',
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
        document.addEventListener('DOMContentLoaded', initCategoriaPage);
    } else {
        initCategoriaPage();
    }

    // API pública
    window.CategoriaProductivaPage = {
        openNewModal: openNewCategoriaModal,
        closeModal: closeModal,
        clearForm: clearForm,
        confirmDelete: confirmDeleteCategoria,
        validateForm: validateForm,
        showAlert: showAlert
    };

    // Funciones globales para compatibilidad
    window.openNewCategoriaModal = openNewCategoriaModal;
    window.closeModal = closeModal;
    window.confirmDeleteCategoria = confirmDeleteCategoria;

    console.log('🎉 CategoriaProductiva Page cargado y listo');

})();
