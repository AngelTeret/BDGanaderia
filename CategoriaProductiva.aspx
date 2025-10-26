<%@ Page Language="VB" AutoEventWireup="true" CodeFile="CategoriaProductiva.aspx.vb" Inherits="CategoriaProductiva" ResponseEncoding="utf-8" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Gestión de Categorías Productivas - Sistema Ganadero</title>
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
                    <input type="text" class="search-input" placeholder="Buscar categorías productivas...">
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
                            <a href="Default.aspx" class="nav-link">
                                <i class="fas fa-home nav-icon"></i>
                                <span class="nav-text">Dashboard</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="Animal.aspx" class="nav-link">
                                <i class="fas fa-paw nav-icon"></i>
                                <span class="nav-text">Animales</span>
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
                            <a href="EstadoAnimal.aspx" class="nav-link">
                                <i class="fas fa-heartbeat nav-icon"></i>
                                <span class="nav-text">Estados de Animal</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a href="CategoriaProductiva.aspx" class="nav-link active">
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
                <h1 class="page-title">Gestión de Categorías Productivas</h1>
                <p class="page-subtitle">Administra las categorías productivas del ganado</p>
                <div class="page-actions">
                    <asp:Button ID="btnNuevaCategoria" runat="server" Text="Nueva Categoría" 
                        CssClass="btn btn-primary" OnClientClick="openNewCategoriaModal(); return false;" />
                    <a href="#" class="btn btn-secondary">
                        <i class="fas fa-download"></i>
                        Exportar
                    </a>
                </div>
            </div>

            <!-- Tarjeta principal -->
            <div class="stat-card">
                <div class="stat-card-header">
                    <h3 class="stat-card-title">Lista de Categorías Productivas</h3>
                </div>
                <p class="stat-card-subtitle">Gestiona todas las categorías productivas registradas en el sistema</p>
                
                <!-- Botón oculto para eliminación -->
                <asp:HiddenField ID="hfCategoriaIdEliminar" runat="server" />
                <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
                    CssClass="btn btn-danger" style="display:none;" 
                    OnClick="btnEliminarOculto_Click" />

                <!-- Tabla de categorías productivas -->
                <div class="table-container" style="margin-top: 20px;">
                    <asp:GridView ID="gvCategorias" runat="server" CssClass="table" 
                        AutoGenerateColumns="False" OnRowCommand="gvCategorias_RowCommand"
                        EmptyDataText="No hay categorías productivas registradas" EnableViewState="True"
                        style="width: 100%; border-collapse: collapse;">
                        <Columns>
                            <asp:BoundField DataField="ID_Categoria" HeaderText="ID" />
                            <asp:BoundField DataField="Nombre_Categoria" HeaderText="Nombre de la Categoría" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                        CommandName="EditarCategoria" CommandArgument='<%# Eval("ID_Categoria") %>'
                                        CssClass="btn btn-warning" />
                                    <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                        CommandName="EliminarCategoria" CommandArgument='<%# Eval("ID_Categoria") %>'
                                        CssClass="btn btn-danger"
                                        OnClientClick="return confirmDeleteCategoria(this);" 
                                        data-command-argument='<%# Eval("ID_Categoria") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </main>

        <!-- Modal para agregar/editar categoría productiva -->
        <div id="categoriaModal" class="modal" runat="server">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 id="modalTitle" runat="server">Nueva Categoría Productiva</h3>
                    <span class="close" onclick="closeModal()">&times;</span>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfCategoriaId" runat="server" />
                    
                    <div class="form-group">
                        <label class="form-label">Nombre de la Categoría</label>
                        <asp:TextBox ID="txtNombreCategoria" runat="server" CssClass="form-control" 
                            placeholder="Ingrese el nombre de la categoría productiva"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" 
                        CssClass="btn btn-success" OnClick="btnGuardar_Click" />
                    <button type="button" class="btn btn-secondary" onclick="closeModal()">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </form>

    <!-- Scripts -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="Scripts/admin-dashboard.js"></script>
    <script src="Scripts/CategoriaProductiva-page.js"></script>
</body>
</html>
