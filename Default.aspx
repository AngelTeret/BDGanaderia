<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Dashboard - Sistema de Gestión Ganadera - BDGanaderia</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
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
</asp:Content>
