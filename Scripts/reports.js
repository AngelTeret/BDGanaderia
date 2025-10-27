/**
 * Scripts para el módulo de Reportes
 * Vanilla JS - Sin dependencias externas
 */

(function() {
    'use strict';
    
    // Variables dentro del scope de reportes
    var currentReport = null;
    var reportData = null;
    
    // Catálogo de reportes
    const reportCatalog = {
        'inventario-animales': {
            key: 'inventario-animales',
            title: 'Inventario Completo de Animales',
            sp: 'Reporte_InventarioAnimales',
            params: [
                { name: '@ID_Raza', type: 'select', label: 'Raza', sp: 'ListarRaza', textField: 'Nombre_Raza', valueField: 'ID_Raza' },
                { name: '@ID_EstadoAnimal', type: 'select', label: 'Estado', sp: 'ListarEstadoAnimal', textField: 'Nombre_Estado', valueField: 'ID_EstadoAnimal' },
                { name: '@ID_TipoAnimal', type: 'select', label: 'Tipo de Animal', sp: 'ListarTipoAnimal', textField: 'Nombre_Tipo', valueField: 'ID_TipoAnimal' },
                { name: '@ID_Categoria', type: 'select', label: 'Categoría', sp: 'ListarCategoriaProductiva', textField: 'Nombre_Categoria', valueField: 'ID_Categoria' },
                { name: '@FechaInicio', type: 'date', label: 'Fecha Nacimiento (Desde)' },
                { name: '@FechaFin', type: 'date', label: 'Fecha Nacimiento (Hasta)' }
            ]
        },
        'produccion-lechera': {
            key: 'produccion-lechera',
            title: 'Producción Lechera Detallada',
            sp: 'Reporte_ProduccionLechera',
            params: [
                { name: '@ID_Animal', type: 'select', label: 'Animal', sp: 'ListarAnimal', textField: 'Nombre_Animal', valueField: 'ID_Animal' },
                { name: '@ID_Raza', type: 'select', label: 'Raza', sp: 'ListarRaza', textField: 'Nombre_Raza', valueField: 'ID_Raza' },
                { name: '@ID_Periodo', type: 'select', label: 'Período', sp: 'ListarPeriodoProduccion', textField: 'Nombre_Periodo', valueField: 'ID_Periodo' },
                { name: '@FechaInicio', type: 'date', label: 'Fecha Control (Desde)' },
                { name: '@FechaFin', type: 'date', label: 'Fecha Control (Hasta)' }
            ]
        },
        'historial-vacunas': {
            key: 'historial-vacunas',
            title: 'Historial Completo de Vacunas',
            sp: 'Reporte_HistorialVacunas',
            params: [
                { name: '@ID_Animal', type: 'select', label: 'Animal', sp: 'ListarAnimal', textField: 'Nombre_Animal', valueField: 'ID_Animal' },
                { name: '@ID_Vacuna', type: 'select', label: 'Vacuna', sp: 'ListarVacuna', textField: 'Nombre_Vacuna', valueField: 'ID_Vacuna' },
                { name: '@ID_Raza', type: 'select', label: 'Raza', sp: 'ListarRaza', textField: 'Nombre_Raza', valueField: 'ID_Raza' },
                { name: '@FechaInicio', type: 'date', label: 'Fecha Aplicación (Desde)' },
                { name: '@FechaFin', type: 'date', label: 'Fecha Aplicación (Hasta)' }
            ]
        },
        'historial-tratamientos': {
            key: 'historial-tratamientos',
            title: 'Historial de Tratamientos',
            sp: 'Reporte_HistorialTratamientos',
            params: [
                { name: '@ID_Animal', type: 'select', label: 'Animal', sp: 'ListarAnimal', textField: 'Nombre_Animal', valueField: 'ID_Animal' },
                { name: '@ID_Veterinario', type: 'select', label: 'Veterinario', sp: 'ListarVeterinario', textField: 'Nombre_Veterinario', valueField: 'ID_Veterinario' },
                { name: '@ID_Tratamiento', type: 'select', label: 'Tratamiento', sp: 'ListarTratamiento', textField: 'Nombre_Tratamiento', valueField: 'ID_Tratamiento' },
                { name: '@FechaInicio', type: 'date', label: 'Fecha Tratamiento (Desde)' },
                { name: '@FechaFin', type: 'date', label: 'Fecha Tratamiento (Hasta)' }
            ]
        },
        'consumo-alimentos': {
            key: 'consumo-alimentos',
            title: 'Consumo de Alimentos por Ración',
            sp: 'Reporte_ConsumoAlimentos',
            params: [
                { name: '@ID_Racion', type: 'select', label: 'Ración', sp: 'ListarRacion', textField: 'Nombre_Racion', valueField: 'ID_Racion' },
                { name: '@ID_Animal', type: 'select', label: 'Animal', sp: 'ListarAnimal', textField: 'Nombre_Animal', valueField: 'ID_Animal' },
                { name: '@ID_Alimento', type: 'select', label: 'Alimento', sp: 'ListarAlimento', textField: 'Nombre_Alimento', valueField: 'ID_Alimento' },
                { name: '@FechaInicio', type: 'date', label: 'Fecha Asignación (Desde)' },
                { name: '@FechaFin', type: 'date', label: 'Fecha Asignación (Hasta)' }
            ]
        },
        'pesaje-evolucion': {
            key: 'pesaje-evolucion',
            title: 'Evolución del Pesaje',
            sp: 'Reporte_PesajeEvolucion',
            params: [
                { name: '@ID_Animal', type: 'select', label: 'Animal', sp: 'ListarAnimal', textField: 'Nombre_Animal', valueField: 'ID_Animal' },
                { name: '@ID_Raza', type: 'select', label: 'Raza', sp: 'ListarRaza', textField: 'Nombre_Raza', valueField: 'ID_Raza' },
                { name: '@FechaInicio', type: 'date', label: 'Fecha Pesaje (Desde)' },
                { name: '@FechaFin', type: 'date', label: 'Fecha Pesaje (Hasta)' }
            ]
        },
        'animales-ubicacion': {
            key: 'animales-ubicacion',
            title: 'Animales por Potrero y Corral',
            sp: 'Reporte_AnimalesUbicacion',
            params: [
                { name: '@ID_Potrero', type: 'select', label: 'Potrero', sp: 'ListarPotrero', textField: 'Nombre_Potrero', valueField: 'ID_Potrero' },
                { name: '@ID_Corral', type: 'select', label: 'Corral', sp: 'ListarCorral', textField: 'Nombre_Corral', valueField: 'ID_Corral' },
                { name: '@ID_Raza', type: 'select', label: 'Raza', sp: 'ListarRaza', textField: 'Nombre_Raza', valueField: 'ID_Raza' },
                { name: '@FechaConsulta', type: 'date', label: 'Fecha Consulta', defaultValue: new Date().toISOString().split('T')[0] }
            ]
        },
        'analisis-nutricional': {
            key: 'analisis-nutricional',
            title: 'Análisis Nutricional',
            sp: 'Reporte_AnalisisNutricional',
            params: [
                { name: '@ID_Animal', type: 'select', label: 'Animal', sp: 'ListarAnimal', textField: 'Nombre_Animal', valueField: 'ID_Animal' },
                { name: '@ID_Raza', type: 'select', label: 'Raza', sp: 'ListarRaza', textField: 'Nombre_Raza', valueField: 'ID_Raza' },
                { name: '@FechaInicio', type: 'date', label: 'Fecha Evaluación (Desde)' },
                { name: '@FechaFin', type: 'date', label: 'Fecha Evaluación (Hasta)' }
            ]
        },
        'rendimiento-categoria': {
            key: 'rendimiento-categoria',
            title: 'Rendimiento por Categoría',
            sp: 'Reporte_RendimientoCategoria',
            params: [
                { name: '@ID_Categoria', type: 'select', label: 'Categoría', sp: 'ListarCategoriaProductiva', textField: 'Nombre_Categoria', valueField: 'ID_Categoria' },
                { name: '@ID_Raza', type: 'select', label: 'Raza', sp: 'ListarRaza', textField: 'Nombre_Raza', valueField: 'ID_Raza' },
                { name: '@FechaInicio', type: 'date', label: 'Fecha Control (Desde)' },
                { name: '@FechaFin', type: 'date', label: 'Fecha Control (Hasta)' }
            ]
        },
        'registros-sanitarios': {
            key: 'registros-sanitarios',
            title: 'Registros Sanitarios',
            sp: 'Reporte_RegistrosSanitarios',
            params: [
                { name: '@ID_Animal', type: 'select', label: 'Animal', sp: 'ListarAnimal', textField: 'Nombre_Animal', valueField: 'ID_Animal' },
                { name: '@ID_Raza', type: 'select', label: 'Raza', sp: 'ListarRaza', textField: 'Nombre_Raza', valueField: 'ID_Raza' },
                { name: '@FechaInicio', type: 'date', label: 'Fecha Registro (Desde)' },
                { name: '@FechaFin', type: 'date', label: 'Fecha Registro (Hasta)' }
            ]
        }
    };
    
    // Inicialización cuando el DOM está listo
    function initializeReports() {
        // Verificar que el elemento existe (solo en la página de reportes)
        const select = document.getElementById('repSelect');
        if (!select) return;
        
        // Llenar el select con los reportes disponibles
        Object.keys(reportCatalog).forEach(function(key) {
            const option = document.createElement('option');
            option.value = key;
            option.textContent = reportCatalog[key].title;
            select.appendChild(option);
        });
        
        // Cargar última selección desde localStorage
        const lastReport = localStorage.getItem('lastReport');
        if (lastReport && reportCatalog[lastReport]) {
            select.value = lastReport;
            loadReportDetails(lastReport);
        }
        
        // Event listeners
        select.addEventListener('change', function() {
            if (this.value) {
                localStorage.setItem('lastReport', this.value);
                loadReportDetails(this.value);
            } else {
                hideFilters();
            }
        });
        
        var btnAplicar = document.getElementById('btnAplicar');
        var btnDescargar = document.getElementById('btnDescargar');
        if (btnAplicar) btnAplicar.addEventListener('click', applyFilters);
        if (btnDescargar) btnDescargar.addEventListener('click', downloadPDF);
    }
    
    // Inicializar cuando el DOM esté listo
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initializeReports);
    } else {
        setTimeout(initializeReports, 100);
    }
    
    function loadReportDetails(reportKey) {
        currentReport = reportCatalog[reportKey];
        if (!currentReport) return;
        
        // Mostrar y limpiar filtros
        const filtersContainer = document.getElementById('repFilters');
        filtersContainer.innerHTML = '';
        filtersContainer.style.display = 'block';
        
        // Crear filtros
        const params = currentReport.params;
        params.forEach(function(param) {
            const group = document.createElement('div');
            group.className = 'rep-filter-item';
            
            const label = document.createElement('label');
            label.textContent = param.label;
            group.appendChild(label);
            
            let input;
            if (param.type === 'select') {
                input = document.createElement('select');
                input.id = 'filter_' + param.name;
                input.name = param.name;
                
                // Agregar opción vacía
                const emptyOption = document.createElement('option');
                emptyOption.value = '';
                emptyOption.textContent = '-- Todos --';
                input.appendChild(emptyOption);
                
                // Si tiene SP para cargar opciones
                if (param.sp) {
                    loadSelectOptions(param.sp, param.textField, param.valueField, input);
                }
            } else if (param.type === 'date') {
                input = document.createElement('input');
                input.type = 'date';
                input.id = 'filter_' + param.name;
                input.name = param.name;
                if (param.defaultValue) {
                    input.value = param.defaultValue;
                }
            } else if (param.type === 'number') {
                input = document.createElement('input');
                input.type = 'number';
                input.id = 'filter_' + param.name;
                input.name = param.name;
            } else {
                input = document.createElement('input');
                input.type = 'text';
                input.id = 'filter_' + param.name;
                input.name = param.name;
            }
            
            group.appendChild(input);
            filtersContainer.appendChild(group);
        });
    }
    
    function loadSelectOptions(spName, textField, valueField, selectElement) {
        fetch('Reportes.aspx/GetDropdownOptions', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify({ spName: spName, textField: textField, valueField: valueField })
        })
        .then(response => response.json())
        .then(function(data) {
            if (data.d && data.d.success) {
                data.d.data.forEach(function(item) {
                    const option = document.createElement('option');
                    option.value = item.value;
                    option.textContent = item.text;
                    selectElement.appendChild(option);
                });
            }
        })
        .catch(function(error) {
            console.error('Error loading options:', error);
        });
    }
    
    function hideFilters() {
        document.getElementById('repFilters').style.display = 'none';
        document.getElementById('repPreview').style.display = 'none';
        document.getElementById('btnDescargar').style.display = 'none';
    }
    
    function applyFilters() {
        if (!currentReport) return;
        
        // Mostrar loader
        document.getElementById('repLoader').style.display = 'block';
        document.getElementById('repPreview').style.display = 'block';
        document.getElementById('repContent').innerHTML = '';
        
        // Recopilar valores de filtros
        const params = {};
        currentReport.params.forEach(function(param) {
            const input = document.querySelector('[name="' + param.name + '"]');
            if (input && input.value) {
                params[param.name] = input.value;
            }
        });
        
        // Llamar al WebMethod
        fetch('Reportes.aspx/GetReportData', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify({ storedProcedureName: currentReport.sp, parameters: params })
        })
        .then(response => response.json())
        .then(function(data) {
            document.getElementById('repLoader').style.display = 'none';
            
            if (data.d && data.d.success) {
                reportData = data.d;
                displayData(data.d.data, data.d.columns);
                document.getElementById('btnDescargar').style.display = 'block';
            } else {
                document.getElementById('repContent').innerHTML = '<div class="rep-error">Error: ' + (data.d.message || 'Error desconocido') + '</div>';
            }
        })
        .catch(function(error) {
            document.getElementById('repLoader').style.display = 'none';
            document.getElementById('repContent').innerHTML = '<div class="rep-error">Error al cargar datos: ' + error.message + '</div>';
        });
    }
    
    function displayData(rows, columns) {
        if (!rows || rows.length === 0) {
            document.getElementById('repContent').innerHTML = '<div class="rep-info">No se encontraron resultados para los filtros seleccionados.</div>';
            return;
        }
        
        let html = '<div style="overflow-x: auto;"><table class="rep-table"><thead><tr>';
        columns.forEach(function(col) {
            html += '<th>' + col + '</th>';
        });
        html += '</tr></thead><tbody>';
        
        rows.forEach(function(row) {
            html += '<tr>';
            columns.forEach(function(col) {
                let value = row[col] !== null && row[col] !== undefined ? row[col] : '';
                // Formatear fechas si están en formato Date
                if (value.toString().indexOf('/Date(') === 0) {
                    let timestamp = parseInt(value.match(/\d+/)[0]);
                    let date = new Date(timestamp);
                    value = date.toLocaleDateString('es-ES');
                }
                // Formatear números decimales
                if (typeof value === 'number' && value % 1 !== 0) {
                    value = value.toFixed(2);
                }
                html += '<td>' + value + '</td>';
            });
            html += '</tr>';
        });
        
        html += '</tbody></table></div>';
        document.getElementById('repContent').innerHTML = html;
    }
    
    function downloadPDF() {
        if (!reportData) {
            alert('No hay datos para descargar. Aplique filtros primero.');
            return;
        }
        alert('Funcionalidad de descarga PDF pendiente de implementar con RDLC.');
    }
    
})();

