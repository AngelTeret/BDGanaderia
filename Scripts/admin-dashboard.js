/**
 * ADMIN DASHBOARD - BDGanaderia
 * JavaScript funcional para sidebar moderno
 */

(function() {
    'use strict';

    // Variables globales
    let sidebarOpen = false;
    let isDesktop = false;

    // Elementos DOM
    let sidebar = null;
    let hamburgerBtn = null;
    let overlay = null;
    let mainContent = null;

    // Configuración
    const CONFIG = {
        DESKTOP_BREAKPOINT: 1024,
        TRANSITION_DURATION: 300
    };

    /**
     * Inicializa el dashboard
     */
    function initDashboard() {
        console.log('🚀 Inicializando Admin Dashboard...');
        
        // Obtener elementos
        sidebar = document.getElementById('sidebar');
        hamburgerBtn = document.querySelector('.hamburger-menu');
        overlay = document.querySelector('.sidebar-overlay');
        mainContent = document.querySelector('.main-content');
        
        console.log('🔍 Elementos encontrados:', {
            sidebar: !!sidebar,
            hamburgerBtn: !!hamburgerBtn,
            overlay: !!overlay,
            mainContent: !!mainContent
        });
        
        if (!sidebar || !hamburgerBtn) {
            console.error('❌ Elementos principales no encontrados');
            return;
        }
        
        // Detectar tamaño de pantalla
        checkScreenSize();
        
        // Configurar event listeners
        setupEventListeners();
        
        // Configurar estado inicial
        setupInitialState();
        
        console.log('✅ Admin Dashboard inicializado correctamente');
    }

    /**
     * Configura los event listeners
     */
    function setupEventListeners() {
        // Botón hamburguesa
        hamburgerBtn.addEventListener('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            console.log('🖱️ Botón hamburguesa clickeado');
            toggleSidebar();
        });
        
        // Overlay (móvil)
        if (overlay) {
            overlay.addEventListener('click', function(e) {
                e.preventDefault();
                e.stopPropagation();
                console.log('🌫️ Overlay clickeado');
                closeSidebar();
            });
        }
        
        // Resize de ventana
        window.addEventListener('resize', handleResize);
        
        // Enlaces del sidebar
        const navLinks = document.querySelectorAll('.nav-link');
        navLinks.forEach(function(link) {
            link.addEventListener('click', function() {
                if (!isDesktop) {
                    setTimeout(closeSidebar, 100);
                }
            });
        });
        
        console.log('📡 Event listeners configurados');
    }

    /**
     * Detecta el tamaño de pantalla
     */
    function checkScreenSize() {
        isDesktop = window.innerWidth >= CONFIG.DESKTOP_BREAKPOINT;
        console.log(`📱 Tamaño de pantalla: ${isDesktop ? 'Escritorio' : 'Móvil'} (${window.innerWidth}px)`);
    }

    /**
     * Maneja el resize de la ventana
     */
    function handleResize() {
        const wasDesktop = isDesktop;
        checkScreenSize();
        
        if (!wasDesktop && isDesktop) {
            // Cambió de móvil a escritorio
            openSidebar();
        } else if (wasDesktop && !isDesktop) {
            // Cambió de escritorio a móvil
            closeSidebar();
        }
    }

    /**
     * Configura el estado inicial
     */
    function setupInitialState() {
        if (isDesktop) {
            // Escritorio: sidebar siempre visible
            sidebar.classList.remove('collapsed');
            sidebarOpen = true;
            if (overlay) overlay.classList.remove('show');
            if (mainContent) {
                mainContent.classList.remove('sidebar-collapsed');
            }
        } else {
            // Móvil: sidebar oculto por defecto
            sidebar.classList.add('collapsed');
            sidebarOpen = false;
            if (overlay) overlay.classList.remove('show');
            if (mainContent) {
                mainContent.classList.add('sidebar-collapsed');
            }
        }
    }

    /**
     * Función principal del toggle
     */
    function toggleSidebar() {
        console.log('🔄 Toggle ejecutado, estado actual:', sidebarOpen);
        
        if (sidebarOpen) {
            closeSidebar();
        } else {
            openSidebar();
        }
    }

    /**
     * Abre el sidebar
     */
    function openSidebar() {
        if (sidebarOpen) return;
        
        console.log('📂 Abriendo sidebar');
        
        sidebarOpen = true;
        sidebar.classList.remove('collapsed');
        
        if (!isDesktop) {
            // Móvil: mostrar overlay
            if (overlay) overlay.classList.add('show');
            if (mainContent) {
                mainContent.classList.add('sidebar-collapsed');
            }
        } else {
            // Escritorio: ajustar contenido
            if (mainContent) {
                mainContent.classList.remove('sidebar-collapsed');
            }
        }
        
        console.log('✅ Sidebar abierto');
    }

    /**
     * Cierra el sidebar
     */
    function closeSidebar() {
        if (!sidebarOpen) return;
        
        console.log('📁 Cerrando sidebar');
        
        sidebarOpen = false;
        sidebar.classList.add('collapsed');
        
        if (overlay) overlay.classList.remove('show');
        if (mainContent) {
            mainContent.classList.add('sidebar-collapsed');
        }
        
        console.log('✅ Sidebar cerrado');
    }

    /**
     * Función para obtener el estado actual
     */
    function getSidebarState() {
        return {
            open: sidebarOpen,
            isDesktop: isDesktop,
            screenWidth: window.innerWidth
        };
    }

    /**
     * Función para forzar el estado del sidebar
     */
    function forceSidebarState(open) {
        if (open) {
            openSidebar();
        } else {
            closeSidebar();
        }
    }

    // Inicialización automática
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initDashboard);
    } else {
        initDashboard();
    }

    /**
     * Bloquea el scroll del body cuando se abre un modal
     */
    function lockBodyScroll() {
        if (!document.body.style.overflow) {
            const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
            document.body.style.overflow = 'hidden';
            if (scrollbarWidth > 0) {
                document.body.style.paddingRight = scrollbarWidth + 'px';
            }
        }
    }

    /**
     * Libera el scroll del body cuando se cierra un modal
     */
    function unlockBodyScroll() {
        document.body.style.overflow = '';
        document.body.style.paddingRight = '';
    }

    /**
     * Wrapper mejorado para mostrar modales (bloquea el scroll)
     */
    function openModal(modalId) {
        const modal = document.getElementById(modalId);
        if (modal) {
            modal.style.display = 'block';
            lockBodyScroll();
            return true;
        }
        return false;
    }

    /**
     * Wrapper mejorado para cerrar modales (libera el scroll)
     */
    function closeModal(modalId) {
        const modal = document.getElementById(modalId);
        if (modal) {
            modal.style.display = 'none';
            unlockBodyScroll();
            return true;
        }
        return false;
    }

    // API pública
    window.Dashboard = {
        toggle: toggleSidebar,
        open: openSidebar,
        close: closeSidebar,
        getState: getSidebarState,
        forceState: forceSidebarState,
        lockBodyScroll: lockBodyScroll,
        unlockBodyScroll: unlockBodyScroll
    };

    // Funciones globales para compatibilidad
    window.toggleSidebar = toggleSidebar;
    window.openSidebar = openSidebar;
    window.closeSidebar = closeSidebar;
    window.lockBodyScroll = lockBodyScroll;
    window.unlockBodyScroll = unlockBodyScroll;
    window.openModal = openModal;
    window.closeModal = closeModal;

    console.log('🎉 Admin Dashboard cargado y listo');

})();

