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
            <a href="Animal.aspx" class="btn btn-primary">
                <i class="fas fa-plus"></i>
                Nuevo Animal
            </a>
            <a href="AnimalControlLechero.aspx" class="btn btn-secondary">
                <i class="fas fa-flask"></i>
                Registrar Producción
            </a>
        </div>
    </div>

    <!-- Tarjetas de estadísticas -->
    <div class="stats-grid">
        <!-- Tarjeta de animales -->
        <div class="stat-card">
            <div class="stat-card-header">
                <h3 class="stat-card-title"><i class="fas fa-cow"></i> Ganado</h3>
            </div>
            <p class="stat-card-subtitle">Total de animales en el sistema</p>
            
            <div class="stat-item">
                <span class="stat-label">Total de Animales</span>
                <span class="stat-value" id="totalAnimales">0</span>
            </div>
            <div class="stat-item">
                <span class="stat-label">Pesajes Registrados</span>
                <span class="stat-value success" id="animalesActivos">0</span>
            </div>
            <div class="stat-item">
                <span class="stat-label">Razas Registradas</span>
                <span class="stat-value warning" id="razasRegistradas">0</span>
            </div>
        </div>

        <!-- Tarjeta de producción lechera -->
        <div class="stat-card" style="background: var(--primary-color); color: white;">
            <div class="stat-card-header">
                <h3 class="stat-card-title" style="color: white;"><i class="fas fa-flask"></i> Producción Lechera</h3>
            </div>
            <p class="stat-card-subtitle" style="color: rgba(255,255,255,0.8);">Estadísticas de producción de leche</p>
            
            <div class="stat-item" style="border-color: rgba(255,255,255,0.2);">
                <span class="stat-label" style="color: rgba(255,255,255,0.8);">Controles Realizados</span>
                <span class="stat-value" style="color: white; font-size: 18px;" id="controlesRealizados">0</span>
            </div>
            <div class="stat-item" style="border-color: rgba(255,255,255,0.2);">
                <span class="stat-label" style="color: rgba(255,255,255,0.8);">Tipos de Vacuna</span>
                <span class="stat-value" style="color: white; font-size: 18px;" id="litrosTotales">0</span>
            </div>
        </div>

        <!-- Tarjeta de sanidad -->
        <div class="stat-card">
            <div class="stat-card-header">
                <h3 class="stat-card-title"><i class="fas fa-heartbeat"></i> Sanidad</h3>
            </div>
            <p class="stat-card-subtitle">Estadísticas de salud del ganado</p>
            
            <div class="stat-item">
                <span class="stat-label">Tratamientos Registrados</span>
                <span class="stat-value" id="tratamientosRegistrados">0</span>
            </div>
            <div class="stat-item">
                <span class="stat-label">Total Empleados</span>
                <span class="stat-value success" id="vacunasAplicadas">0</span>
            </div>
        </div>

        <!-- Tarjeta de alimentación -->
        <div class="stat-card" style="background: var(--primary-color); color: white;">
            <div class="stat-card-header">
                <h3 class="stat-card-title" style="color: white;"><i class="fas fa-seedling"></i> Alimentación</h3>
            </div>
            <p class="stat-card-subtitle" style="color: rgba(255,255,255,0.8);">Gestión de alimentación del ganado</p>
            
            <div class="stat-item" style="border-color: rgba(255,255,255,0.2);">
                <span class="stat-label" style="color: rgba(255,255,255,0.8);">Raciones Asignadas</span>
                <span class="stat-value" style="color: white; font-size: 18px;" id="racionesAsignadas">0</span>
            </div>
            <div class="stat-item" style="border-color: rgba(255,255,255,0.2);">
                <span class="stat-label" style="color: rgba(255,255,255,0.8);">Tipos de Alimento</span>
                <span class="stat-value" style="color: white; font-size: 18px;" id="tiposAlimento">0</span>
            </div>
        </div>
    </div>
</asp:Content>
