<%@ Page Language="VB" AutoEventWireup="true" CodeFile="AnimalCorral.aspx.vb" Inherits="AnimalCorral" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Gestión de Corrales de Animales - Sistema Ganadero</title>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header de la página -->
    <div class="page-header">
        <h1 class="page-title">Gestión de Corrales de Animales</h1>
        <p class="page-subtitle">Registra el movimiento de animales entre corrales</p>
        <div class="page-actions">
            <asp:Button ID="btnNuevoMovimiento" runat="server" Text="Nuevo Movimiento" 
                CssClass="btn btn-primary" OnClientClick="openNewMovimientoModal(); return false;" />
        </div>
    </div>

    <!-- Tarjeta principal -->
    <div class="stat-card">
        <div class="stat-card-header">
            <h3 class="stat-card-title">Lista de Movimientos de Corrales</h3>
        </div>
        <p class="stat-card-subtitle">Gestiona todos los movimientos de animales a corrales</p>
        
        <!-- Botón oculto para eliminación -->
        <asp:HiddenField ID="hfAnimalIdEliminar" runat="server" />
        <asp:HiddenField ID="hfCorralIdEliminar" runat="server" />
        <asp:HiddenField ID="hfFechaEntradaEliminar" runat="server" />
        <asp:Button ID="btnEliminarOculto" runat="server" Text="Eliminar" 
            CssClass="btn btn-danger" style="display:none;" 
            OnClick="btnEliminarOculto_Click" />

        <!-- Tabla de movimientos -->
        <div class="table-container" style="margin-top: 20px;">
            <asp:GridView ID="gvMovimientos" runat="server" CssClass="table" 
                AutoGenerateColumns="False" OnRowCommand="gvMovimientos_RowCommand"
                EmptyDataText="No hay movimientos registrados" EnableViewState="True"
                style="width: 100%; border-collapse: collapse;">
                <Columns>
                    <asp:BoundField DataField="Nombre_Animal" HeaderText="Animal" />
                    <asp:BoundField DataField="Nombre_Corral" HeaderText="Corral" />
                    <asp:BoundField DataField="Fecha_Entrada" HeaderText="Fecha Entrada" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Fecha_Salida" HeaderText="Fecha Salida" DataFormatString="{0:dd/MM/yyyy}" 
                        NullDisplayText="Activo" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" Text="Editar" 
                                CommandName="EditarMovimiento" 
                                CommandArgument='<%# Eval("ID_Animal").ToString() + "," + Eval("ID_Corral").ToString() + "," + Eval("Fecha_Entrada", "{0:yyyy-MM-dd}") %>'
                                CssClass="btn btn-warning" />
                            <asp:LinkButton ID="btnEliminar" runat="server" Text="Eliminar" 
                                CommandName="EliminarMovimiento" 
                                CommandArgument='<%# Eval("ID_Animal").ToString() + "," + Eval("ID_Corral").ToString() + "," + Eval("Fecha_Entrada", "{0:yyyy-MM-dd}") %>'
                                CssClass="btn btn-danger"
                                OnClientClick="confirmDeleteMovimiento(this); return false;" 
                                data-animal-id='<%# Eval("ID_Animal") %>'
                                data-corral-id='<%# Eval("ID_Corral") %>'
                                data-fecha-entrada='<%# Eval("Fecha_Entrada", "{0:yyyy-MM-dd}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal para agregar movimiento -->
    <div id="movimientoModal" class="modal" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="modalTitle" runat="server">Nuevo Movimiento a Corral</h3>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="hfAnimalId" runat="server" />
                <asp:HiddenField ID="hfCorralId" runat="server" />
                <asp:HiddenField ID="hfFechaEntrada" runat="server" />
                
                <div class="form-group">
                    <label class="form-label">Animal</label>
                    <asp:DropDownList ID="ddlAnimal" runat="server" CssClass="form-control" Enabled="true">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Corral</label>
                    <asp:DropDownList ID="ddlCorral" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Fecha de Entrada</label>
                    <asp:TextBox ID="txtFechaEntrada" runat="server" CssClass="form-control" 
                        TextMode="Date"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Fecha de Salida (Opcional)</label>
                    <asp:TextBox ID="txtFechaSalida" runat="server" CssClass="form-control" 
                        TextMode="Date"></asp:TextBox>
                    <small class="form-text text-muted">Dejar vacío si el animal aún está en el corral</small>
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
    <script src="Scripts/animal-corral-page.js"></script>
</asp:Content>

