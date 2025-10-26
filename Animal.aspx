<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Animal.aspx.vb" Inherits="Animal" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Animales - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
            <!-- Header de la página -->
            <div class="page-header">
                <h1 class="page-title">Gestión de Animales</h1>
                <p class="page-subtitle">Administra el inventario de animales del ganado</p>
                <div class="page-actions">
                    <asp:Button ID="btnNuevoAnimal" runat="server" Text="Nuevo Animal" 
                        CssClass="btn btn-primary" OnClientClick="openNewAnimalModal(); return false;" />
                    <a href="#" class="btn btn-secondary">
                        <i class="fas fa-download"></i>
                        Exportar
                    </a>
                </div>
            </div>

            <!-- Tarjeta principal -->
            <div class="stat-card">
                <div class="stat-card-header">
                    <h3 class="stat-card-title">Lista de Animales</h3>
                </div>
                <p class="stat-card-subtitle">Gestiona todos los animales registrados en el sistema</p>
                
                <!-- Botón oculto para eliminación -->
                <asp:HiddenField ID="hfAnimalIdEliminar" runat="server" />
                <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
                    CssClass="btn btn-danger" style="display:none;" 
                    OnClick="btnEliminarOculto_Click" />

                <!-- Tabla de animales -->
                <div class="table-container" style="margin-top: 20px;">
                    <asp:GridView ID="gvAnimales" runat="server" CssClass="table" 
                        AutoGenerateColumns="False" OnRowCommand="gvAnimales_RowCommand"
                        EmptyDataText="No hay animales registrados" EnableViewState="True"
                        style="width: 100%; border-collapse: collapse;">
                        <Columns>
                            <asp:BoundField DataField="ID_Animal" HeaderText="ID" />
                            <asp:BoundField DataField="Nombre_Animal" HeaderText="Nombre" />
                            <asp:BoundField DataField="Fecha_Nacimiento" HeaderText="Fecha Nacimiento" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="Sexo" HeaderText="Sexo" />
                            <asp:BoundField DataField="Peso" HeaderText="Peso (kg)" DataFormatString="{0:F2}" />
                            <asp:BoundField DataField="Raza_Nombre" HeaderText="Raza" />
                            <asp:BoundField DataField="Tipo_Nombre" HeaderText="Tipo" />
                            <asp:BoundField DataField="Estado_Nombre" HeaderText="Estado" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                        CommandName="EditarAnimal" CommandArgument='<%# Eval("ID_Animal") %>'
                                        CssClass="btn btn-warning" />
                                    <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                        CommandName="EliminarAnimal" CommandArgument='<%# Eval("ID_Animal") %>'
                                        CssClass="btn btn-danger"
                                        OnClientClick="return confirmDeleteAnimal(this);" 
                                        data-command-argument='<%# Eval("ID_Animal") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <!-- Modal para agregar/editar animal -->
            <div id="animalModal" class="modal" runat="server">
                <div class="modal-content">
                    <div class="modal-header">
                        <h3 id="modalTitle" runat="server" ClientIDMode="Static">Nuevo Animal</h3>
                        <span class="close" onclick="closeModal()">&times;</span>
                    </div>
                    <div class="modal-body">
                        <asp:HiddenField ID="hfAnimalId" runat="server" />
                        
                        <div class="form-group">
                            <label class="form-label">Nombre del Animal</label>
                            <asp:TextBox ID="txtNombreAnimal" runat="server" CssClass="form-control" 
                                placeholder="Ingrese el nombre del animal"></asp:TextBox>
                        </div>
                        
                        <div class="form-group">
                            <label class="form-label">Fecha de Nacimiento</label>
                            <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control" 
                                TextMode="Date" placeholder="Seleccione la fecha"></asp:TextBox>
                        </div>
                        
                        <div class="form-group">
                            <label class="form-label">Sexo</label>
                            <asp:DropDownList ID="ddlSexo" runat="server" CssClass="form-control">
                                <asp:ListItem Value="M" Text="Macho"></asp:ListItem>
                                <asp:ListItem Value="H" Text="Hembra"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        
                        <div class="form-group">
                            <label class="form-label">Peso (kg)</label>
                            <asp:TextBox ID="txtPeso" runat="server" CssClass="form-control" 
                                placeholder="Ingrese el peso (ej: 120.5)" type="text" pattern="[0-9]+\.?[0-9]*"></asp:TextBox>
                        </div>
                        
                        <div class="form-group">
                            <label class="form-label">Raza</label>
                            <asp:DropDownList ID="ddlRaza" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        
                        <div class="form-group">
                            <label class="form-label">Tipo de Animal</label>
                            <asp:DropDownList ID="ddlTipoAnimal" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        
                        <div class="form-group">
                            <label class="form-label">Estado del Animal</label>
                            <asp:DropDownList ID="ddlEstadoAnimal" runat="server" CssClass="form-control">
                            </asp:DropDownList>
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
</asp:Content>

<asp:Content ID="ScriptsContent" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="Scripts/Animal-page.js"></script>
    
    <!-- Script para asegurar que el modal esté cerrado al cargar -->
    <script>
        document.addEventListener('DOMContentLoaded', function() {
            // Si hay parámetro de éxito, asegurar que el modal esté cerrado
            var urlParams = new URLSearchParams(window.location.search);
            if (urlParams.has('success')) {
                var modal = document.getElementById('animalModal');
                if (modal) {
                    modal.style.display = 'none';
                }
            }
        });
    </script>
</asp:Content>
