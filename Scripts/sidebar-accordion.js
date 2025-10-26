/**
 * Sidebar Accordion - Maneja la funcionalidad de expandir/colapsar secciones del menú lateral
 * No interfiere con el comportamiento existente del sidebar
 */
(function() {
    'use strict';

    // Clave para localStorage
    const STORAGE_PREFIX = 'sidebar:group:';
    
    /**
     * Inicializa el accordion del sidebar
     */
    function initSidebarAccordion() {
        const toggleButtons = document.querySelectorAll('[data-accordion-toggle]');
        
        toggleButtons.forEach(button => {
            setupAccordionToggle(button);
        });
        
        // Restaurar estados guardados
        restoreAccordionStates();
        
        // Abrir automáticamente la sección que contiene el link activo
        const activeLink = document.querySelector('.nav-link.active');
        if (activeLink) {
            openActiveSection(activeLink);
        }
    }
    
    /**
     * Configura el comportamiento de un botón toggle
     */
    function setupAccordionToggle(button) {
        const groupKey = button.getAttribute('data-accordion-toggle');
        
        // Click con mouse
        button.addEventListener('click', function(e) {
            e.preventDefault();
            toggleSection(groupKey, button);
        });
        
        // Soporte de teclado (Enter y Espacio)
        button.addEventListener('keydown', function(e) {
            if (e.key === 'Enter' || e.key === ' ') {
                e.preventDefault();
                toggleSection(groupKey, button);
            }
        });
    }
    
    /**
     * Alterna el estado de una sección (abrir/cerrar)
     */
    function toggleSection(groupKey, button) {
        const isExpanded = button.getAttribute('aria-expanded') === 'true';
        const navSection = button.closest('.nav-section');
        const panel = navSection.querySelector('.nav-menu');
        const title = navSection.querySelector('.nav-section-title');
        const arrow = navSection.querySelector('.nav-arrow');
        
        if (isExpanded) {
            // Cerrar
            button.setAttribute('aria-expanded', 'false');
            panel.setAttribute('aria-hidden', 'true');
            panel.style.display = 'none';
            title.classList.remove('is-open');
            
            if (arrow) {
                arrow.classList.remove('fa-chevron-up');
                arrow.classList.add('fa-chevron-down');
            }
            
            // Guardar estado
            saveAccordionState(groupKey, false);
        } else {
            // Abrir
            button.setAttribute('aria-expanded', 'true');
            panel.setAttribute('aria-hidden', 'false');
            panel.style.display = '';
            title.classList.add('is-open');
            
            if (arrow) {
                arrow.classList.remove('fa-chevron-down');
                arrow.classList.add('fa-chevron-up');
            }
            
            // Guardar estado
            saveAccordionState(groupKey, true);
        }
    }
    
    /**
     * Abre una sección automáticamente si contiene el link activo
     */
    function openActiveSection(activeLink) {
        const activeSection = activeLink.closest('.nav-section');
        if (!activeSection) return;
        
        const groupKey = activeSection.getAttribute('data-accordion-group');
        if (!groupKey) return;
        
        const button = activeSection.querySelector('[data-accordion-toggle]');
        if (!button) return;
        
        const isExpanded = button.getAttribute('aria-expanded') === 'true';
        if (!isExpanded) {
            // Abrir la sección sin tocar localStorage (ya que está guardado)
            button.setAttribute('aria-expanded', 'true');
            const panel = activeSection.querySelector('.nav-menu');
            if (panel) {
                panel.setAttribute('aria-hidden', 'false');
                panel.style.display = '';
            }
            
            const title = activeSection.querySelector('.nav-section-title');
            if (title) {
                title.classList.add('is-open');
            }
            
            const arrow = activeSection.querySelector('.nav-arrow');
            if (arrow) {
                arrow.classList.remove('fa-chevron-down');
                arrow.classList.add('fa-chevron-up');
            }
        }
    }
    
    /**
     * Guarda el estado de una sección en localStorage
     */
    function saveAccordionState(groupKey, isOpen) {
        try {
            const key = STORAGE_PREFIX + groupKey;
            localStorage.setItem(key, isOpen ? 'true' : 'false');
        } catch (e) {
            console.warn('No se pudo guardar el estado del accordion:', e);
        }
    }
    
    /**
     * Obtiene el estado guardado de una sección desde localStorage
     */
    function getAccordionState(groupKey) {
        try {
            const key = STORAGE_PREFIX + groupKey;
            return localStorage.getItem(key) === 'true';
        } catch (e) {
            return null;
        }
    }
    
    /**
     * Restaura los estados de todas las secciones desde localStorage
     */
    function restoreAccordionStates() {
        const sections = document.querySelectorAll('.nav-section[data-accordion-group]');
        
        sections.forEach(section => {
            const groupKey = section.getAttribute('data-accordion-group');
            if (!groupKey) return;
            
            const savedState = getAccordionState(groupKey);
            if (savedState === null) {
                // No hay estado guardado, mantener el estado inicial del HTML
                return;
            }
            
            const button = section.querySelector('[data-accordion-toggle]');
            if (!button) return;
            
            const panel = section.querySelector('.nav-menu');
            const title = section.querySelector('.nav-section-title');
            const arrow = section.querySelector('.nav-arrow');
            
            if (savedState) {
                // Abrir
                button.setAttribute('aria-expanded', 'true');
                if (panel) {
                    panel.setAttribute('aria-hidden', 'false');
                    panel.style.display = '';
                }
                if (title) {
                    title.classList.add('is-open');
                }
                if (arrow) {
                    arrow.classList.remove('fa-chevron-down');
                    arrow.classList.add('fa-chevron-up');
                }
            } else {
                // Cerrar
                button.setAttribute('aria-expanded', 'false');
                if (panel) {
                    panel.setAttribute('aria-hidden', 'true');
                    panel.style.display = 'none';
                }
                if (title) {
                    title.classList.remove('is-open');
                }
                if (arrow) {
                    arrow.classList.remove('fa-chevron-up');
                    arrow.classList.add('fa-chevron-down');
                }
            }
        });
    }
    
    // Inicializar cuando el DOM esté listo
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initSidebarAccordion);
    } else {
        initSidebarAccordion();
    }
})();

