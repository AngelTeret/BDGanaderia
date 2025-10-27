<%@ Page Title="Reportes" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeFile="Reportes.aspx.vb" Inherits="Reportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Reportes - Sistema Ganadero</title>
    <style>
        .rep-container { padding: 30px; max-width: 1400px; margin: 0 auto; }
        .rep-header { margin-bottom: 30px; border-bottom: 3px solid #3498db; padding-bottom: 15px; }
        .rep-title { font-size: 32px; color: #2c3e50; font-weight: 700; margin: 0; text-transform: uppercase; }
        .rep-controls { background: white; padding: 25px; border-radius: 10px; box-shadow: 0 4px 6px rgba(0,0,0,0.1); margin-bottom: 20px; }
        .rep-label { display: block; font-weight: 600; color: #34495e; margin-bottom: 12px; font-size: 15px; text-transform: uppercase; }
        .rep-select { width: 100%; padding: 14px 16px; border: 2px solid #ddd; border-radius: 8px; font-size: 15px; background: #f8f9fa; cursor: pointer; }
        .rep-select:hover { border-color: #3498db; background: white; }
        .rep-select:focus { outline: none; border-color: #3498db; box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1); }
        .rep-filters { background: #f8f9fa; padding: 25px; border-radius: 10px; margin-bottom: 20px; display: grid; grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); gap: 20px; }
        .rep-filter-item { display: flex; flex-direction: column; }
        .rep-filter-item label { font-weight: 600; color: #34495e; margin-bottom: 8px; font-size: 14px; }
        .rep-filter-item input, .rep-filter-item select { padding: 12px; border: 2px solid #ddd; border-radius: 6px; font-size: 14px; background: white; }
        .rep-actions { display: flex; gap: 15px; margin-bottom: 20px; }
        .rep-btn { padding: 14px 28px; border: none; border-radius: 8px; font-size: 15px; font-weight: 600; cursor: pointer; text-transform: uppercase; transition: all 0.3s; }
        .rep-btn-primary { background: #3498db; color: white; }
        .rep-btn-primary:hover { background: #2980b9; transform: translateY(-2px); box-shadow: 0 4px 8px rgba(52, 152, 219, 0.3); }
        .rep-btn-success { background: #27ae60; color: white; }
        .rep-btn-success:hover { background: #229954; transform: translateY(-2px); box-shadow: 0 4px 8px rgba(39, 174, 96, 0.3); }
        .rep-preview { background: white; border-radius: 10px; box-shadow: 0 4px 6px rgba(0,0,0,0.1); padding: 25px; }
        .rep-table { width: 100%; border-collapse: separate; border-spacing: 0; font-size: 14px; }
        .rep-table thead { background: linear-gradient(135deg, #34495e 0%, #2c3e50 100%); color: white; }
        .rep-table th { padding: 15px 12px; text-align: left; font-weight: 600; font-size: 13px; text-transform: uppercase; }
        .rep-table td { padding: 12px; border-bottom: 1px solid #e0e0e0; }
        .rep-table tbody tr:nth-child(even) { background: #f8f9fa; }
        .rep-table tbody tr:hover { background: #e3f2fd; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="rep-container">
        <div class="rep-header">
            <h1 class="rep-title">Reportes</h1>
        </div>
        
        <div class="rep-controls">
            <div class="rep-select-wrapper">
                <label for="repSelect" class="rep-label">Seleccionar Reporte</label>
                <select id="repSelect" class="rep-select">
                    <option value="">-- Seleccione un reporte --</option>
                </select>
            </div>
        </div>
        
        <div id="repFilters" class="rep-filters" style="display: none;">
            <!-- Los filtros se generan dinámicamente con JavaScript -->
        </div>
        
        <div class="rep-actions">
            <button type="button" id="btnAplicar" class="rep-btn rep-btn-primary">Aplicar Filtros</button>
            <button type="button" id="btnDescargar" class="rep-btn rep-btn-success" style="display: none;">Descargar PDF</button>
        </div>
        
        <div id="repPreview" class="rep-preview" style="display: none;">
            <div class="rep-loader" id="repLoader" style="display: none;">
                <p>Cargando datos...</p>
            </div>
            <div id="repContent" class="rep-content">
                <!-- Aquí se mostrará el preview de los datos -->
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        // Esperar a que todos los scripts del sitio terminen
        setTimeout(function() {
            // Código de reportes aislado en este scope
            (function() {
                'use strict';
                
                var currentReport = null;
                var reportData = null;
                
                var reportCatalog = {
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
                            { name: '@FechaConsulta', type: 'date', label: 'Fecha Consulta' }
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
                
                function initializeReports() {
                    var select = document.getElementById('repSelect');
                    if (!select) return;
                    
                    Object.keys(reportCatalog).forEach(function(key) {
                        var option = document.createElement('option');
                        option.value = key;
                        option.textContent = reportCatalog[key].title;
                        select.appendChild(option);
                    });
                    
                    var lastReport = localStorage.getItem('lastReport');
                    if (lastReport && reportCatalog[lastReport]) {
                        select.value = lastReport;
                        loadReportDetails(lastReport);
                    }
                    
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
                
                function loadReportDetails(reportKey) {
                    currentReport = reportCatalog[reportKey];
                    if (!currentReport) return;
                    
                    // Ocultar tabla anterior y botón de descargar
                    document.getElementById('repPreview').style.display = 'none';
                    document.getElementById('btnDescargar').style.display = 'none';
                    reportData = null;
                    
                    var filtersContainer = document.getElementById('repFilters');
                    filtersContainer.innerHTML = '';
                    filtersContainer.style.display = 'block';
                    
                    var params = currentReport.params;
                    params.forEach(function(param) {
                        var group = document.createElement('div');
                        group.className = 'rep-filter-item';
                        
                        var label = document.createElement('label');
                        label.textContent = param.label;
                        group.appendChild(label);
                        
                        var input;
                        if (param.type === 'select') {
                            input = document.createElement('select');
                            input.id = 'filter_' + param.name;
                            input.name = param.name;
                            
                            var emptyOption = document.createElement('option');
                            emptyOption.value = '';
                            emptyOption.textContent = '-- Todos --';
                            input.appendChild(emptyOption);
                            
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
                        headers: { 'Content-Type': 'application/json; charset=utf-8' },
                        body: JSON.stringify({ spName: spName, textField: textField, valueField: valueField })
                    })
                    .then(function(response) { return response.json(); })
                    .then(function(data) {
                        if (data.d && data.d.success) {
                            data.d.data.forEach(function(item) {
                                var option = document.createElement('option');
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
                    reportData = null;
                }
                
                function applyFilters(e) {
                    e.preventDefault();
                    e.stopPropagation();
                    
                    if (!currentReport) return;
                    
                    document.getElementById('repLoader').style.display = 'block';
                    document.getElementById('repPreview').style.display = 'block';
                    document.getElementById('repContent').innerHTML = '';
                    
                    var params = {};
                    currentReport.params.forEach(function(param) {
                        var input = document.querySelector('[name="' + param.name + '"]');
                        if (input && input.value) {
                            params[param.name] = input.value;
                        }
                    });
                    
                    fetch('Reportes.aspx/GetReportData', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json; charset=utf-8' },
                        body: JSON.stringify({ storedProcedureName: currentReport.sp, parameters: params })
                    })
                    .then(function(response) { return response.json(); })
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
                    
                    return false;
                }
                
                function displayData(rows, columns) {
                    if (!rows || rows.length === 0) {
                        document.getElementById('repContent').innerHTML = '<div class="rep-info">No se encontraron resultados.</div>';
                        return;
                    }
                    
                    var html = '<div style="overflow-x: auto;"><table class="rep-table"><thead><tr>';
                    columns.forEach(function(col) {
                        html += '<th>' + col + '</th>';
                    });
                    html += '</tr></thead><tbody>';
                    
                    rows.forEach(function(row) {
                        html += '<tr>';
                        columns.forEach(function(col) {
                            var value = row[col] !== null && row[col] !== undefined ? row[col] : '';
                            if (value.toString().indexOf('/Date(') === 0) {
                                var timestamp = parseInt(value.match(/\d+/)[0]);
                                var date = new Date(timestamp);
                                value = date.toLocaleDateString('es-ES');
                            }
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
                
                function downloadPDF(e) {
                    e.preventDefault();
                    e.stopPropagation();
                    
                    if (!reportData) {
                        alert('No hay datos para descargar. Aplique filtros primero.');
                        return false;
                    }
                    
                    try {
                        // Obtener el reporte actual y los datos
                        var reportTitle = currentReport ? currentReport.title : 'Reporte';
                        var columns = reportData.columns;
                        var rows = reportData.data;
                        
                        // Crear el PDF en orientación horizontal para más espacio
                        const { jsPDF } = window.jspdf;
                        const doc = new jsPDF('landscape', 'mm', 'a4');
                        
                        // Título del reporte
                        doc.setFontSize(16);
                        doc.setTextColor(44, 62, 80);
                        doc.text(reportTitle, 14, 20);
                        
                        // Agregar fecha
                        doc.setFontSize(9);
                        doc.setTextColor(100, 100, 100);
                        var fecha = new Date().toLocaleDateString('es-ES');
                        doc.text('Fecha de generación: ' + fecha, 14, 28);
                        
                        // Calcular anchos de columna de manera inteligente
                        var totalWidth = 270; // Ancho total disponible en landscape
                        var colWidths = [];
                        var minColWidth = 20;
                        var totalMinWidth = columns.length * minColWidth;
                        
                        // Si tenemos muchas columnas, usar más espacio
                        if (totalMinWidth > totalWidth) {
                            var ratio = totalWidth / totalMinWidth;
                            columns.forEach(function() {
                                colWidths.push(minColWidth * ratio);
                            });
                        } else {
                            // Distribuir equitativamente con márgenes
                            var availableWidth = totalWidth - (columns.length * 2); // 2mm de separación
                            var baseWidth = availableWidth / columns.length;
                            
                            columns.forEach(function() {
                                colWidths.push(Math.max(baseWidth, 15));
                            });
                        }
                        
                        // Preparar datos de la tabla
                        var tableData = [];
                        rows.forEach(function(row) {
                            var rowData = [];
                            columns.forEach(function(col) {
                                var value = row[col];
                                if (value === null || value === undefined) value = '';
                                rowData.push(value.toString());
                            });
                            tableData.push(rowData);
                        });
                        
                        // Configurar fuente y estilo
                        doc.setFontSize(7); // Fuente más pequeña para caber más
                        doc.setTextColor(0, 0, 0);
                        
                        // Encabezados
                        var startY = 36;
                        var startX = 14;
                        
                        columns.forEach(function(col, index) {
                            // Fondo del encabezado
                            doc.setFillColor(52, 73, 94);
                            doc.rect(startX, startY, colWidths[index], 12, 'F');
                            
                            // Texto del encabezado (blanco, auto-ajuste)
                            doc.setTextColor(255, 255, 255);
                            doc.text(col, startX + 1, startY + 7, { 
                                maxWidth: colWidths[index] - 2,
                                align: 'left'
                            });
                            
                            startX += colWidths[index] + 0.5; // Separación entre columnas
                        });
                        
                        // Datos de la tabla
                        var currentY = startY + 12;
                        var maxHeight = 185;
                        
                        rows.forEach(function(row) {
                            // Nueva página si es necesario
                            if (currentY > maxHeight) {
                                doc.addPage('landscape');
                                currentY = 20;
                            }
                            
                            var maxHeightInRow = currentY + 8; // Altura inicial de la fila
                            var currentX = 14;
                            
                            // Primera pasada: calcular altura máxima necesaria
                            columns.forEach(function(col, index) {
                                var value = row[col];
                                if (value === null || value === undefined) value = '';
                                
                                // Calcular cuántas líneas necesita el texto
                                var lines = doc.splitTextToSize(value.toString(), colWidths[index] - 2);
                                var textHeight = lines.length * 4; // Aproximadamente 4mm por línea
                                
                                if (textHeight > 7) {
                                    maxHeightInRow = Math.max(maxHeightInRow, currentY + textHeight);
                                }
                                
                                currentX += colWidths[index] + 0.5;
                            });
                            
                            // Dibujar las celdas con la altura calculada
                            currentX = 14;
                            var rowHeight = maxHeightInRow - currentY;
                            
                            columns.forEach(function(col, index) {
                                var value = row[col];
                                if (value === null || value === undefined) value = '';
                                
                                // Fondo de celda
                                doc.setFillColor(255, 255, 255);
                                doc.rect(currentX, currentY, colWidths[index], rowHeight, 'FD');
                                
                                // Texto de celda (multi-línea)
                                doc.setTextColor(0, 0, 0);
                                doc.text(value.toString(), currentX + 1, currentY + 4, { 
                                    maxWidth: colWidths[index] - 2,
                                    align: 'left'
                                });
                                
                                currentX += colWidths[index] + 0.5;
                            });
                            
                            currentY = maxHeightInRow;
                        });
                        
                        // Descargar
                        var fileName = (reportTitle + '_' + fecha.replace(/\//g, '-')).replace(/[^a-zA-Z0-9_\-]/g, '_');
                        doc.save(fileName + '.pdf');
                        
                    } catch (error) {
                        console.error('Error generando PDF:', error);
                        alert('Error al generar el PDF: ' + error.message);
                    }
                    
                    return false;
                }
                
                // Inicializar
                initializeReports();
            })();
        }, 500);
    </script>
</asp:Content>
