Imports System.Collections.Generic

''' <summary>
''' Catálogo de reportes disponibles en el sistema
''' Define qué SP usar, qué parámetros pide y qué RDLC cargar
''' </summary>
Public Class ReportCatalog

    Public Class ReportDefinition
        Public Property Key As String
        Public Property Title As String
        Public Property StoredProcedureName As String
        Public Property RdlcPath As String
        Public Property Description As String
        Public Property Parameters As List(Of ReportParameter)
        Public Property AllowedRoles As List(Of String) ' Roles que pueden ver este reporte (vacío = todos)
    End Class

    Public Class ReportParameter
        Public Property Name As String
        Public Property Type As String ' "date", "number", "select", "text"
        Public Property Required As Boolean
        Public Property Label As String
        Public Property DefaultValue As Object
        Public Property LoadOptionsQuery As String ' Query para llenar select (ej: "SELECT ID, Name FROM Table")
        Public Property OptionsDataSource As String ' Nombre de SP o query para llenar combo
    End Class

    ''' <summary>Obtiene el catálogo completo de reportes</summary>
    Public Shared Function GetCatalog() As List(Of ReportDefinition)
        Dim catalog As New List(Of ReportDefinition)

        ' Reporte 1: Inventario de Animales por Raza y Estado
        catalog.Add(New ReportDefinition With {
            .Key = "inventario-animales",
            .Title = "Inventario de Animales por Raza y Estado",
            .StoredProcedureName = "Reporte_InventarioAnimales",
            .RdlcPath = "~/Reportes/Templates/InventarioAnimales.rdlc",
            .Description = "Lista todos los animales con sus datos básicos filtrados por raza, estado y tipo",
            .Parameters = New List(Of ReportParameter) From {
                New ReportParameter With {.Name = "@ID_Raza", .Type = "select", .Required = False, .Label = "Raza", .OptionsDataSource = "ListarRaza"},
                New ReportParameter With {.Name = "@ID_EstadoAnimal", .Type = "select", .Required = False, .Label = "Estado", .OptionsDataSource = "ListarEstadoAnimal"},
                New ReportParameter With {.Name = "@ID_TipoAnimal", .Type = "select", .Required = False, .Label = "Tipo", .OptionsDataSource = "ListarTipoAnimal"},
                New ReportParameter With {.Name = "@FechaInicio", .Type = "date", .Required = False, .Label = "Fecha Nacimiento Desde"},
                New ReportParameter With {.Name = "@FechaFin", .Type = "date", .Required = False, .Label = "Fecha Nacimiento Hasta"}
            },
            .AllowedRoles = New List(Of String)
        })

        ' Reporte 2: Producción Lechera
        catalog.Add(New ReportDefinition With {
            .Key = "produccion-lechera",
            .Title = "Producción Lechera por Animal",
            .StoredProcedureName = "Reporte_ProduccionLechera",
            .RdlcPath = "~/Reportes/Templates/ProduccionLechera.rdlc",
            .Description = "Análisis de producción de leche por animal con promedios y totales",
            .Parameters = New List(Of ReportParameter) From {
                New ReportParameter With {.Name = "@ID_Animal", .Type = "select", .Required = False, .Label = "Animal", .OptionsDataSource = "ListarAnimales"},
                New ReportParameter With {.Name = "@FechaInicio", .Type = "date", .Required = False, .Label = "Fecha Desde"},
                New ReportParameter With {.Name = "@FechaFin", .Type = "date", .Required = False, .Label = "Fecha Hasta"},
                New ReportParameter With {.Name = "@ID_Periodo", .Type = "select", .Required = False, .Label = "Período", .OptionsDataSource = "ListarPeriodoProduccion"}
            },
            .AllowedRoles = New List(Of String)
        })

        ' Reporte 3: Historial de Vacunas
        catalog.Add(New ReportDefinition With {
            .Key = "historial-vacunas",
            .Title = "Historial de Vacunas Aplicadas",
            .StoredProcedureName = "Reporte_HistorialVacunas",
            .RdlcPath = "~/Reportes/Templates/HistorialVacunas.rdlc",
            .Description = "Registro completo de vacunación de todos los animales",
            .Parameters = New List(Of ReportParameter) From {
                New ReportParameter With {.Name = "@ID_Animal", .Type = "select", .Required = False, .Label = "Animal", .OptionsDataSource = "ListarAnimales"},
                New ReportParameter With {.Name = "@ID_Vacuna", .Type = "select", .Required = False, .Label = "Vacuna", .OptionsDataSource = "ListarVacuna"},
                New ReportParameter With {.Name = "@FechaInicio", .Type = "date", .Required = False, .Label = "Fecha Desde"},
                New ReportParameter With {.Name = "@FechaFin", .Type = "date", .Required = False, .Label = "Fecha Hasta"}
            },
            .AllowedRoles = New List(Of String)
        })

        ' Reporte 4: Historial de Tratamientos
        catalog.Add(New ReportDefinition With {
            .Key = "historial-tratamientos",
            .Title = "Historial de Tratamientos Veterinarios",
            .StoredProcedureName = "Reporte_HistorialTratamientos",
            .RdlcPath = "~/Reportes/Templates/HistorialTratamientos.rdlc",
            .Description = "Tratamientos médicos aplicados con medicamentos y veterinarios responsables",
            .Parameters = New List(Of ReportParameter) From {
                New ReportParameter With {.Name = "@ID_Animal", .Type = "select", .Required = False, .Label = "Animal", .OptionsDataSource = "ListarAnimales"},
                New ReportParameter With {.Name = "@ID_Veterinario", .Type = "select", .Required = False, .Label = "Veterinario", .OptionsDataSource = "ListarVeterinario"},
                New ReportParameter With {.Name = "@FechaInicio", .Type = "date", .Required = False, .Label = "Fecha Desde"},
                New ReportParameter With {.Name = "@FechaFin", .Type = "date", .Required = False, .Label = "Fecha Hasta"}
            },
            .AllowedRoles = New List(Of String)
        })

        ' Reporte 5: Consumo de Alimentos
        catalog.Add(New ReportDefinition With {
            .Key = "consumo-alimentos",
            .Title = "Consumo de Alimentos por Ración",
            .StoredProcedureName = "Reporte_ConsumoAlimentos",
            .RdlcPath = "~/Reportes/Templates/ConsumoAlimentos.rdlc",
            .Description = "Análisis de raciones asignadas y consumo de alimentos",
            .Parameters = New List(Of ReportParameter) From {
                New ReportParameter With {.Name = "@ID_Racion", .Type = "select", .Required = False, .Label = "Ración", .OptionsDataSource = "ListarRacion"},
                New ReportParameter With {.Name = "@ID_Animal", .Type = "select", .Required = False, .Label = "Animal", .OptionsDataSource = "ListarAnimales"},
                New ReportParameter With {.Name = "@FechaInicio", .Type = "date", .Required = False, .Label = "Fecha Desde"},
                New ReportParameter With {.Name = "@FechaFin", .Type = "date", .Required = False, .Label = "Fecha Hasta"}
            },
            .AllowedRoles = New List(Of String)
        })

        ' Reporte 6: Pesaje Evolución
        catalog.Add(New ReportDefinition With {
            .Key = "pesaje-evolucion",
            .Title = "Evolución del Pesaje de Animales",
            .StoredProcedureName = "Reporte_PesajeEvolucion",
            .RdlcPath = "~/Reportes/Templates/PesajeEvolucion.rdlc",
            .Description = "Historial de pesos con comparación entre pesajes",
            .Parameters = New List(Of ReportParameter) From {
                New ReportParameter With {.Name = "@ID_Animal", .Type = "select", .Required = False, .Label = "Animal", .OptionsDataSource = "ListarAnimales"},
                New ReportParameter With {.Name = "@ID_Raza", .Type = "select", .Required = False, .Label = "Raza", .OptionsDataSource = "ListarRaza"},
                New ReportParameter With {.Name = "@FechaInicio", .Type = "date", .Required = False, .Label = "Fecha Desde"},
                New ReportParameter With {.Name = "@FechaFin", .Type = "date", .Required = False, .Label = "Fecha Hasta"}
            },
            .AllowedRoles = New List(Of String)
        })

        ' Reporte 7: Animales por Ubicación
        catalog.Add(New ReportDefinition With {
            .Key = "animales-ubicacion",
            .Title = "Animales por Potrero y Corral",
            .StoredProcedureName = "Reporte_AnimalesUbicacion",
            .RdlcPath = "~/Reportes/Templates/AnimalesUbicacion.rdlc",
            .Description = "Ubicación actual de animales en potreros y corrales",
            .Parameters = New List(Of ReportParameter) From {
                New ReportParameter With {.Name = "@ID_Potrero", .Type = "select", .Required = False, .Label = "Potrero", .OptionsDataSource = "ListarPotrero"},
                New ReportParameter With {.Name = "@ID_Corral", .Type = "select", .Required = False, .Label = "Corral", .OptionsDataSource = "ListarCorral"},
                New ReportParameter With {.Name = "@FechaConsulta", .Type = "date", .Required = False, .Label = "Fecha de Consulta"}
            },
            .AllowedRoles = New List(Of String)
        })

        ' Reporte 8: Análisis Nutricional
        catalog.Add(New ReportDefinition With {
            .Key = "analisis-nutricional",
            .Title = "Análisis Nutricional y Condición Corporal",
            .StoredProcedureName = "Reporte_AnalisisNutricional",
            .RdlcPath = "~/Reportes/Templates/AnalisisNutricional.rdlc",
            .Description = "Evaluaciones nutricionales con condición corporal y raciones asignadas",
            .Parameters = New List(Of ReportParameter) From {
                New ReportParameter With {.Name = "@ID_Animal", .Type = "select", .Required = False, .Label = "Animal", .OptionsDataSource = "ListarAnimales"},
                New ReportParameter With {.Name = "@ID_Raza", .Type = "select", .Required = False, .Label = "Raza", .OptionsDataSource = "ListarRaza"},
                New ReportParameter With {.Name = "@FechaInicio", .Type = "date", .Required = False, .Label = "Fecha Desde"},
                New ReportParameter With {.Name = "@FechaFin", .Type = "date", .Required = False, .Label = "Fecha Hasta"}
            },
            .AllowedRoles = New List(Of String)
        })

        ' Reporte 9: Rendimiento por Categoría
        catalog.Add(New ReportDefinition With {
            .Key = "rendimiento-categoria",
            .Title = "Rendimiento por Categoría Productiva",
            .StoredProcedureName = "Reporte_RendimientoCategoria",
            .RdlcPath = "~/Reportes/Templates/RendimientoCategoria.rdlc",
            .Description = "Análisis de producción y sanidad por categoría productiva",
            .Parameters = New List(Of ReportParameter) From {
                New ReportParameter With {.Name = "@ID_Categoria", .Type = "select", .Required = False, .Label = "Categoría", .OptionsDataSource = "ListarCategoriaProductiva"},
                New ReportParameter With {.Name = "@FechaInicio", .Type = "date", .Required = False, .Label = "Fecha Desde"},
                New ReportParameter With {.Name = "@FechaFin", .Type = "date", .Required = False, .Label = "Fecha Hasta"}
            },
            .AllowedRoles = New List(Of String)
        })

        ' Reporte 10: Registros Sanitarios
        catalog.Add(New ReportDefinition With {
            .Key = "registros-sanitarios",
            .Title = "Registros Sanitarios Completos",
            .StoredProcedureName = "Reporte_RegistrosSanitarios",
            .RdlcPath = "~/Reportes/Templates/RegistrosSanitarios.rdlc",
            .Description = "Resumen completo de salud animal con vacunas y tratamientos",
            .Parameters = New List(Of ReportParameter) From {
                New ReportParameter With {.Name = "@ID_Animal", .Type = "select", .Required = False, .Label = "Animal", .OptionsDataSource = "ListarAnimales"},
                New ReportParameter With {.Name = "@FechaInicio", .Type = "date", .Required = False, .Label = "Fecha Desde"},
                New ReportParameter With {.Name = "@FechaFin", .Type = "date", .Required = False, .Label = "Fecha Hasta"}
            },
            .AllowedRoles = New List(Of String)
        })

        Return catalog
    End Function

    ''' <summary>Obtiene un reporte por su clave</summary>
    Public Shared Function GetReport(key As String) As ReportDefinition
        Return GetCatalog().FirstOrDefault(Function(r) r.Key = key)
    End Function

End Class

