Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.Caching

''' <summary>
''' Clase que genera el menú dinámicamente desde configuración en código
''' No requiere tabla SQL adicional, todo el mapeo está aquí
''' </summary>
Public Class MenuItemDataAccess
    Private Shared ReadOnly CacheKey As String = "SidebarMenuItems"
    Private Shared ReadOnly CacheDuration As Integer = 10 ' minutos

    ''' <summary>
    ''' Obtiene los ítems del menú agrupados por MenuGroupKey
    ''' Configurado directamente en código basado en las tablas existentes del sistema
    ''' </summary>
    Public Shared Function GetMenuItemsGrouped() As Dictionary(Of String, MenuGroup)
        ' Intentar obtener de caché primero
        Dim cacheItem As Dictionary(Of String, MenuGroup) = Nothing
        If HttpContext.Current IsNot Nothing Then
            cacheItem = TryCast(HttpContext.Current.Cache.Get(CacheKey), Dictionary(Of String, MenuGroup))
        End If
        If cacheItem IsNot Nothing Then
            Return cacheItem
        End If

        ' Construir el menú desde configuración en código
        Dim groupedMenu As New Dictionary(Of String, MenuGroup)
        
        ' =========================================
        ' CONFIGURACIÓN DEL MENÚ BASADA EN TU BD
        ' Mapea directamente con las tablas existentes de proyectoGanaderia.sql
        ' =========================================
        
        ' Grupo 1: Principal
        Dim grupoPrincipal As New MenuGroup With {
            .GroupKey = "principal",
            .GroupTitle = "Principal"
        }
        grupoPrincipal.Items.Add(New MenuItem With {
            .ItemKey = "dashboard",
            .ItemText = "Dashboard",
            .ItemUrl = "Default.aspx",
            .ItemOrder = 1,
            .IconCss = "fas fa-home"
        })
        groupedMenu("principal") = grupoPrincipal
        
        ' Grupo 2: Gestión de Animales
        ' Tablas: Raza, Tipo_Animal, Estado_Animal, Categoria_Productiva, Animal
        Dim grupoAnimales As New MenuGroup With {
            .GroupKey = "animales",
            .GroupTitle = "Gestión de Animales"
        }
        grupoAnimales.Items.Add(New MenuItem With {.ItemKey = "raza", .ItemText = "Razas", .ItemUrl = "Raza.aspx", .ItemOrder = 1, .IconCss = "fas fa-horse"})
        grupoAnimales.Items.Add(New MenuItem With {.ItemKey = "tipoAnimal", .ItemText = "Tipos de Animal", .ItemUrl = "TipoAnimal.aspx", .ItemOrder = 2, .IconCss = "fas fa-paw"})
        grupoAnimales.Items.Add(New MenuItem With {.ItemKey = "estadoAnimal", .ItemText = "Estados de Animal", .ItemUrl = "EstadoAnimal.aspx", .ItemOrder = 3, .IconCss = "fas fa-heartbeat"})
        grupoAnimales.Items.Add(New MenuItem With {.ItemKey = "categoriaProductiva", .ItemText = "Categorías Productivas", .ItemUrl = "CategoriaProductiva.aspx", .ItemOrder = 4, .IconCss = "fas fa-tags"})
        grupoAnimales.Items.Add(New MenuItem With {.ItemKey = "animal", .ItemText = "Animales", .ItemUrl = "Animal.aspx", .ItemOrder = 5, .IconCss = "fas fa-paw"})
        groupedMenu("animales") = grupoAnimales
        
        ' Grupo 3: Alimentación
        ' Tablas: Alimento, Tipo_Alimento, Racion, Racion_Alimento
        Dim grupoAlimentacion As New MenuGroup With {
            .GroupKey = "alimentacion",
            .GroupTitle = "Alimentación"
        }
        grupoAlimentacion.Items.Add(New MenuItem With {.ItemKey = "alimento", .ItemText = "Alimentos", .ItemUrl = "Alimento.aspx", .ItemOrder = 1, .IconCss = "fas fa-seedling"})
        grupoAlimentacion.Items.Add(New MenuItem With {.ItemKey = "tipoAlimento", .ItemText = "Tipos de Alimento", .ItemUrl = "TipoAlimento.aspx", .ItemOrder = 2, .IconCss = "fas fa-leaf"})
        grupoAlimentacion.Items.Add(New MenuItem With {.ItemKey = "racion", .ItemText = "Raciones", .ItemUrl = "Racion.aspx", .ItemOrder = 3, .IconCss = "fas fa-utensils"})
        groupedMenu("alimentacion") = grupoAlimentacion
        
        ' Grupo 4: Sanidad
        ' Tablas: Vacuna, Medicamento, Tratamiento, Veterinario, Animal_Vacuna, Animal_Tratamiento, Tratamiento_Medicamento
        Dim grupoSanidad As New MenuGroup With {
            .GroupKey = "sanidad",
            .GroupTitle = "Sanidad"
        }
        grupoSanidad.Items.Add(New MenuItem With {.ItemKey = "vacuna", .ItemText = "Vacunas", .ItemUrl = "Vacuna.aspx", .ItemOrder = 1, .IconCss = "fas fa-syringe"})
        grupoSanidad.Items.Add(New MenuItem With {.ItemKey = "tratamiento", .ItemText = "Tratamientos", .ItemUrl = "Tratamiento.aspx", .ItemOrder = 2, .IconCss = "fas fa-pills"})
        grupoSanidad.Items.Add(New MenuItem With {.ItemKey = "medicamento", .ItemText = "Medicamentos", .ItemUrl = "Medicamento.aspx", .ItemOrder = 3, .IconCss = "fas fa-capsules"})
        grupoSanidad.Items.Add(New MenuItem With {.ItemKey = "veterinario", .ItemText = "Veterinarios", .ItemUrl = "Veterinario.aspx", .ItemOrder = 4, .IconCss = "fas fa-user-md"})
        groupedMenu("sanidad") = grupoSanidad
        
        ' Grupo 5: Producción Lechera
        ' Tablas: Control_Lechero, Periodo_Produccion, Ordeno, Animal_ControlLechero
        Dim grupoProduccion As New MenuGroup With {
            .GroupKey = "produccion",
            .GroupTitle = "Producción Lechera"
        }
        grupoProduccion.Items.Add(New MenuItem With {.ItemKey = "controlLechero", .ItemText = "Control Lechero", .ItemUrl = "ControlLechero.aspx", .ItemOrder = 1, .IconCss = "fas fa-tint"})
        grupoProduccion.Items.Add(New MenuItem With {.ItemKey = "periodoProduccion", .ItemText = "Períodos de Producción", .ItemUrl = "PeriodoProduccion.aspx", .ItemOrder = 2, .IconCss = "fas fa-calendar-alt"})
        grupoProduccion.Items.Add(New MenuItem With {.ItemKey = "ordeno", .ItemText = "Ordeños", .ItemUrl = "Ordeno.aspx", .ItemOrder = 3, .IconCss = "fas fa-hand-holding-water"})
        groupedMenu("produccion") = grupoProduccion
        
        ' Guardar en caché
        If HttpContext.Current IsNot Nothing Then
            HttpContext.Current.Cache.Insert(CacheKey, groupedMenu, Nothing,
                                             DateTime.Now.AddMinutes(CacheDuration),
                                             System.Web.Caching.Cache.NoSlidingExpiration)
        End If
        
        Return groupedMenu
    End Function

    ''' <summary>
    ''' Limpia la caché del menú
    ''' </summary>
    Public Shared Sub ClearCache()
        If HttpContext.Current IsNot Nothing Then
            HttpContext.Current.Cache.Remove(CacheKey)
        End If
    End Sub
End Class

Public Class MenuGroup
    Public Property GroupKey As String
    Public Property GroupTitle As String
    Public Property Items As New List(Of MenuItem)
End Class

Public Class MenuItem
    Public Property ItemKey As String
    Public Property ItemText As String
    Public Property ItemUrl As String
    Public Property ItemOrder As Integer
    Public Property IconCss As String
    Public Property Roles As String
End Class
