<%@ Page Language="VB" AutoEventWireup="true" CodeFile="AnimalTratamiento.aspx.vb" Inherits="AnimalTratamiento" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Tratamientos Veterinarios - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Tratamientos Veterinarios</h1>
        <p class="page-subtitle">Registra tratamientos aplicados a los animales con veterinario</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoTratamiento" runat="server" Text="Nuevo Tratamiento" 
                CssClass="btn btn-primary" OnClientClick="openNewTratamientoModal(); return false;" />
            <a href="#" class="btn btn-secondary">
                <i class="fas fa-download"></i>
                Exportar
            </a>
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Tratamientos</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los tratamientos aplicados a los animales</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfAnimalIdEliminar" runat="server" />
        <asp:HiddenField ID="hfTratamientoIdEliminar" runat="server" />
        <asp:HiddenField ID="hfFechaTratamientoEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de tratamientos -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvTratamientos" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvTratamientos_RowCommand"
                EmptyDataText="No hay tratamientos registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="Nombre_Animal" HeaderText="Animal" />
                    <asp:BoundField DataField="Nombre_Tratamiento" HeaderText="Tratamiento" />
                    <asp:BoundField DataField="Nombre_Veterinario" HeaderText="Veterinario" />
                    <asp:BoundField DataField="Fecha_Tratamiento" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Observacion" HeaderText="Observación" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarTratamiento" 
                                CommandArgument='<%# Eval("ID_Animal").ToString() + "," + Eval("ID_Tratamiento").ToString() + "," + Eval("Fecha_Tratamiento", "{0:yyyy-MM-dd}") %>'
                                CssClass="btn btn-warning" 
                                OnClientClick="return true;" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarTratamiento" 
                                CommandArgument='<%# Eval("ID_Animal").ToString() + "," + Eval("ID_Tratamiento").ToString() + "," + Eval("Fecha_Tratamiento", "{0:yyyy-MM-dd}") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="confirmDeleteTratamiento(this); return false;" 
                                data-animal-id='<%# Eval("ID_Animal") %>'
                                data-tratamiento-id='<%# Eval("ID_Tratamiento") %>'
                                data-fecha-tratamiento='<%# Eval("Fecha_Tratamiento", "{0:yyyy-MM-dd}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar tratamiento -->
    <div id="tratamientoModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Tratamiento</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfAnimalId" runat="server" />
                <asp:HiddenField ID="hfTratamientoId" runat="server" />
                <asp:HiddenField ID="hfFechaTratamiento" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Animal</label>
                    <asp:DropDownList ID="ddlAnimal" runat="server" CssClass="form-control" Enabled="true">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Tratamiento</label>
                    <asp:DropDownList ID="ddlTratamiento" runat="server" CssClass="form-control" Enabled="true">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Veterinario</label>
                    <asp:DropDownList ID="ddlVeterinario" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Fecha de Tratamiento</label>
                    <asp:TextBox ID="txtFechaTratamiento" runat="server" CssClass="form-control" 
                        TextMode="Date" Enabled="true"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Observación</label>
                    <asp:TextBox ID="txtObservacion" runat="server" CssClass="form-control" 
                        TextMode="MultiLine" Rows="3" placeholder="Ingrese observaciones del tratamiento"></asp:TextBox>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" 
                    CssClass="btn btn-success" OnClick="btnGuardar_Click" />
                <button type="button" class="btn btn-secondary" onclick="closeModal(); return false;">
                    Cancelar
                </button>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="ScriptsContent" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="Scripts/animal-tratamiento-page.js"></script>
</asp:Content>

