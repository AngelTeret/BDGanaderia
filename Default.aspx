<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" ResponseEncoding="utf-8" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Sistema de Gestión Ganadera - BDGanaderia</title>
    <link href="Content/Site.css" rel="stylesheet" />
    <link href="Content/admin-dashboard.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- HEADER SUPERIOR -->
        <header class="top-header">
            <div class="header-left">
                <button class="hamburger-menu" type="button">
                    <i class="fas fa-bars"></i>
                </button>
                <a href="#" class="logo">BDGanaderia</a>
            </div>
            
            <div class="header-center">
                <div class="search-bar">
                    <input type="text" class="search-input" placeholder="Buscar...">
                    <i class="fas fa-search search-icon"></i>
                </div>
            </div>
            
            <div class="header-right">
                <button class="header-icon" title="Mensajes">
                    <i class="fas fa-envelope"></i>
                </button>
                
                <button class="header-icon" title="Notificaciones">
                    <i class="fas fa-bell"></i>
                    <span class="notification-badge">3</span>
                </button>
                
                <div class="user-profile">
                    <div class="user-avatar">A</div>
                    <div class="user-info">
                        <div class="user-name">Administrador</div>
                        <div class="user-role">Admin</div>
                    </div>
                </div>
                
                <button class="header-icon" title="Configuración">
                    <i class="fas fa-cog"></i>
                </button>
            </div>
        </header>

        <!-- SIDEBAR LATERAL -->
        <aside id="sidebar" class="sidebar">
            <div class="sidebar-header">
                <div class="sidebar-user">
                    <div class="sidebar-avatar">A</div>
                    <div class="sidebar-user-info">
                        <h3>Administrador</h3>
                        <p>Administrador del Sistema</p>
                    </div>
                </div>
            </div>
            
            <nav class="sidebar-nav">
                <div class="nav-section">
                    <div class="nav-section-title">Principal</div>
                    <ul class="nav-menu">
                        <li class="nav-item">
                            <a href="Default.aspx" class="nav-link active">
                                <i class="fas fa-home nav-icon"></i>
                                <span class="nav-text">Dashboard</span>
                            </a>
                        </li>
                    </ul>
                </div>

                <div class="nav-section">
                    <div class="nav-section-title">Gestión</div>
                    <ul class="nav-menu">
                        <li class="nav-item">
                            <a href="Raza.aspx" class="nav-link">
                                <i class="fas fa-horse nav-icon"></i>
                                <span class="nav-text">Razas</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="TipoAnimal.aspx" class="nav-link">
                                <i class="fas fa-paw nav-icon"></i>
                                <span class="nav-text">Tipos de Animal</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="Alimento.aspx" class="nav-link">
                                <i class="fas fa-seedling nav-icon"></i>
                                <span class="nav-text">Alimentos</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="TipoAlimento.aspx" class="nav-link">
                                <i class="fas fa-leaf nav-icon"></i>
                                <span class="nav-text">Tipos de Alimento</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="Racion.aspx" class="nav-link">
                                <i class="fas fa-utensils nav-icon"></i>
                                <span class="nav-text">Raciones</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="Vacuna.aspx" class="nav-link">
                                <i class="fas fa-syringe nav-icon"></i>
                                <span class="nav-text">Vacunas</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="Tratamiento.aspx" class="nav-link">
                                <i class="fas fa-pills nav-icon"></i>
                                <span class="nav-text">Tratamientos</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="Medicamento.aspx" class="nav-link">
                                <i class="fas fa-capsules nav-icon"></i>
                                <span class="nav-text">Medicamentos</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="Veterinario.aspx" class="nav-link">
                                <i class="fas fa-user-md nav-icon"></i>
                                <span class="nav-text">Veterinarios</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="ControlLechero.aspx" class="nav-link">
                                <i class="fas fa-tint nav-icon"></i>
                                <span class="nav-text">Control Lechero</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="PeriodoProduccion.aspx" class="nav-link">
                                <i class="fas fa-calendar-alt nav-icon"></i>
                                <span class="nav-text">Períodos de Producción</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="Ordeno.aspx" class="nav-link">
                                <i class="fas fa-hand-holding-water nav-icon"></i>
                                <span class="nav-text">Ordeños</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="EstadoAnimal.aspx" class="nav-link">
                                <i class="fas fa-heartbeat nav-icon"></i>
                                <span class="nav-text">Estados de Animal</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="CategoriaProductiva.aspx" class="nav-link">
                                <i class="fas fa-tags nav-icon"></i>
                                <span class="nav-text">Categorías Productivas</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="Animal.aspx" class="nav-link">
                                <i class="fas fa-paw nav-icon"></i>
                                <span class="nav-text">Animales</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="#" class="nav-link">
                                <i class="fas fa-chart-bar nav-icon"></i>
                                <span class="nav-text">Reportes</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="#" class="nav-link">
                                <i class="fas fa-cog nav-icon"></i>
                                <span class="nav-text">Configuración</span>
                            </a>
                        </li>
                    </ul>
                </div>
                
                <div class="nav-section">
                    <div class="nav-section-title">Sistema</div>
                    <ul class="nav-menu">
                        <li class="nav-item">
                            <a href="#" class="nav-link">
                                <i class="fas fa-users nav-icon"></i>
                                <span class="nav-text">Usuarios</span>
                                <span class="nav-badge">5</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="#" class="nav-link">
                                <i class="fas fa-calendar nav-icon"></i>
                                <span class="nav-text">Calendario</span>
                                <span class="nav-badge">1</span>
                            </a>
                        </li>
                    </ul>
                </div>
            </nav>
        </aside>

        <!-- OVERLAY PARA MÓVIL -->
        <div class="sidebar-overlay"></div>

        <!-- CONTENIDO PRINCIPAL -->
        <main class="main-content">
            <!-- Header de la página -->
            <div class="page-header">
                <h1 class="page-title">Dashboard</h1>
                <p class="page-subtitle">Sistema de Gestión Ganadera - BDGanaderia</p>
                <div class="page-actions">
                    <a href="#" class="btn btn-primary">
                        <i class="fas fa-plus"></i>
                        Agregar Ganado
                    </a>
                    <a href="#" class="btn btn-secondary">
                        <i class="fas fa-cog"></i>
                        Configurar
                    </a>
                </div>
            </div>

            <!-- Tarjetas de estadísticas -->
            <div class="stats-grid">
                <!-- Tarjeta de estadísticas generales -->
                <div class="stat-card">
                    <div class="stat-card-header">
                        <h3 class="stat-card-title">Estadísticas Generales</h3>
                    </div>
                    <p class="stat-card-subtitle">Información diaria sobre estadísticas del sistema</p>
                    
                    <div class="stat-item">
                        <span class="stat-label">Nuevos Usuarios</span>
                        <span class="stat-value success">5</span>
                    </div>
                    <div class="stat-item">
                        <span class="stat-label">Ventas</span>
                        <span class="stat-value success">36</span>
                    </div>
                    <div class="stat-item">
                        <span class="stat-label">Suscriptores</span>
                        <span class="stat-value warning">12</span>
                    </div>
                </div>

                <!-- Tarjeta de ingresos -->
                <div class="stat-card">
                    <div class="stat-card-header">
                        <h3 class="stat-card-title">Ingresos y Gastos</h3>
                    </div>
                    <p class="stat-card-subtitle">Estadísticas de ingresos y gastos totales</p>
                    
                    <div class="stat-item">
                        <span class="stat-label">INGRESOS TOTALES</span>
                        <span class="stat-value success">$9,782</span>
                    </div>
                    <div class="stat-item">
                        <span class="stat-label">GASTOS TOTALES</span>
                        <span class="stat-value danger">$1,248</span>
                    </div>
                </div>

                <!-- Tarjeta de usuarios -->
                <div class="stat-card">
                    <div class="stat-card-header">
                        <h3 class="stat-card-title">Estadísticas de Usuarios</h3>
                    </div>
                    <p class="stat-card-subtitle">Información sobre usuarios del sistema</p>
                    
                    <div class="stat-item">
                        <span class="stat-label">Usuarios Activos</span>
                        <span class="stat-value">156</span>
                    </div>
                    <div class="stat-item">
                        <span class="stat-label">Nuevos Hoy</span>
                        <span class="stat-value success">8</span>
                    </div>
                    <div class="stat-item">
                        <span class="stat-label">Sesiones</span>
                        <span class="stat-value warning">342</span>
                    </div>
                </div>

                <!-- Tarjeta de ventas diarias -->
                <div class="stat-card" style="background: var(--primary-color); color: white;">
                    <div class="stat-card-header">
                        <h3 class="stat-card-title" style="color: white;">Ventas Diarias</h3>
                    </div>
                    <p class="stat-card-subtitle" style="color: rgba(255,255,255,0.8);">25 Marzo - 02 Abril</p>
                    
                    <div style="font-size: 32px; font-weight: bold; margin: 20px 0;">
                        $4,578.58
                    </div>
                    
                    <div class="stat-item" style="border-color: rgba(255,255,255,0.2);">
                        <span class="stat-label" style="color: rgba(255,255,255,0.8);">Crecimiento</span>
                        <span class="stat-value success">+12.5%</span>
                    </div>
                </div>
            </div>
        </main>
    </form>

    <!-- Scripts -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="Scripts/admin-dashboard.js"></script>
</body>
</html>


                            </a>

                        </li>

                        <li class="nav-item">

                            <a href="#" class="nav-link">

                                <i class="fas fa-calendar nav-icon"></i>

                                <span class="nav-text">Calendario</span>

                                <span class="nav-badge">1</span>

                            </a>

                        </li>

                    </ul>

                </div>

            </nav>

        </aside>



        <!-- OVERLAY PARA MÓVIL -->

        <div class="sidebar-overlay"></div>



        <!-- CONTENIDO PRINCIPAL -->

        <main class="main-content">

            <!-- Header de la página -->

            <div class="page-header">

                <h1 class="page-title">Dashboard</h1>

                <p class="page-subtitle">Sistema de Gestión Ganadera - BDGanaderia</p>

                <div class="page-actions">

                    <a href="#" class="btn btn-primary">

                        <i class="fas fa-plus"></i>

                        Agregar Ganado

                    </a>

                    <a href="#" class="btn btn-secondary">

                        <i class="fas fa-cog"></i>

                        Configurar

                    </a>

                </div>

            </div>



            <!-- Tarjetas de estadísticas -->

            <div class="stats-grid">

                <!-- Tarjeta de estadísticas generales -->

                <div class="stat-card">

                    <div class="stat-card-header">

                        <h3 class="stat-card-title">Estadísticas Generales</h3>

                    </div>

                    <p class="stat-card-subtitle">Información diaria sobre estadísticas del sistema</p>

                    

                    <div class="stat-item">

                        <span class="stat-label">Nuevos Usuarios</span>

                        <span class="stat-value success">5</span>

                    </div>

                    <div class="stat-item">

                        <span class="stat-label">Ventas</span>

                        <span class="stat-value success">36</span>

                    </div>

                    <div class="stat-item">

                        <span class="stat-label">Suscriptores</span>

                        <span class="stat-value warning">12</span>

                    </div>

                </div>



                <!-- Tarjeta de ingresos -->

                <div class="stat-card">

                    <div class="stat-card-header">

                        <h3 class="stat-card-title">Ingresos y Gastos</h3>

                    </div>

                    <p class="stat-card-subtitle">Estadísticas de ingresos y gastos totales</p>

                    

                    <div class="stat-item">

                        <span class="stat-label">INGRESOS TOTALES</span>

                        <span class="stat-value success">$9,782</span>

                    </div>

                    <div class="stat-item">

                        <span class="stat-label">GASTOS TOTALES</span>

                        <span class="stat-value danger">$1,248</span>

                    </div>

                </div>



                <!-- Tarjeta de usuarios -->

                <div class="stat-card">

                    <div class="stat-card-header">

                        <h3 class="stat-card-title">Estadísticas de Usuarios</h3>

                    </div>

                    <p class="stat-card-subtitle">Información sobre usuarios del sistema</p>

                    

                    <div class="stat-item">

                        <span class="stat-label">Usuarios Activos</span>

                        <span class="stat-value">156</span>

                    </div>

                    <div class="stat-item">

                        <span class="stat-label">Nuevos Hoy</span>

                        <span class="stat-value success">8</span>

                    </div>

                    <div class="stat-item">

                        <span class="stat-label">Sesiones</span>

                        <span class="stat-value warning">342</span>

                    </div>

                </div>



                <!-- Tarjeta de ventas diarias -->

                <div class="stat-card" style="background: var(--primary-color); color: white;">

                    <div class="stat-card-header">

                        <h3 class="stat-card-title" style="color: white;">Ventas Diarias</h3>

                    </div>

                    <p class="stat-card-subtitle" style="color: rgba(255,255,255,0.8);">25 Marzo - 02 Abril</p>

                    

                    <div style="font-size: 32px; font-weight: bold; margin: 20px 0;">

                        $4,578.58

                    </div>

                    

                    <div class="stat-item" style="border-color: rgba(255,255,255,0.2);">

                        <span class="stat-label" style="color: rgba(255,255,255,0.8);">Crecimiento</span>

                        <span class="stat-value success">+12.5%</span>

                    </div>

                </div>

            </div>

        </main>

    </form>



    <!-- Scripts -->

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <script src="Scripts/admin-dashboard.js"></script>

</body>

</html>


