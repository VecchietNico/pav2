<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Hotel.aspx.cs" Inherits="Hotelwf" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="col-lg-12">
        <h1 class="page-header">Hoteles</h1>
        <ol class="breadcrumb">
            <li><a href="Inicio.aspx">Inicio</a></li>
            <li class="active">Hoteles</li>
        </ol>
    </div>

    <div class="col-lg-12">
        <h4><asp:Label ID="lblAccion" CssClass="label label-success" runat="server" Text="Label"></asp:Label></h4> 
    </div>

    <div class="col-lg-12">
        <asp:Panel ID="Panel1" runat="server">
            <strong>Hotel:</strong>
            <div class="form-inline">
                <asp:TextBox ID="txtbxBuscar"  CssClass="form-control input-sm" title="Ingrese nombres de hoteles" placeholder="Nombre del Hotel" runat="server">
                </asp:TextBox>
                <asp:CheckBox ID="chk_eliminados" CssClass="checkbox-inline" Text="Ver eliminados" runat="server" OnCheckedChanged="chk_eliminados_CheckedChanged" />
                <asp:Button ID="btnBuscar" CssClass="btn-sm btn-primary" role="button" type="submit" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
            </div>

            <table class="table table-bordered">
                <asp:GridView ID="GridView1" runat="server" CssClass="table table-responsive table-bordered table-hover" CellPadding="4" DataKeyNames="id" GridLines="None" OnPageIndexChanging="GridView1_PageIndexChanging" OnSorting="GridView1_Sorting" AutoGenerateColumns="False">
                    <Columns>
                        <asp:CommandField ShowSelectButton="True"/>
                        <asp:BoundField DataField="descripcion" HeaderText="Nombre" SortExpression="descripcion"/>
                        <asp:BoundField DataField="id" HeaderText="ID" SortExpression="id"/>
                        <asp:BoundField DataField="capacidad" HeaderText="Capacidad" SortExpression="capacidad"/>
                        <asp:BoundField DataField="destino_descripcion" HeaderText="Ubicación" SortExpression="destino_descripcion"/>
                        <asp:BoundField DataField="cuit" HeaderText="CUIT" SortExpression="cuit" />
                    </Columns>
                    <SelectedRowStyle BackColor="Silver" />
                </asp:GridView>
            </table>

            <div class="btn-group-sm">
                <asp:Button ID="btnAgregar" runat="server"  CssClass="btn-default btn-sm" OnClick="btnAgregar_Click" Text="Agregar" />
                <asp:Button ID="btnConsultar" runat="server"  CssClass="btn-default btn-sm" OnClick="btnConsultar_Click" Text="Ver" />
                <asp:Button ID="btnEliminar" runat="server"  CssClass="btn-default btn-sm" OnClick="btnEliminar_Click" Text="Eliminar" />
                <asp:Button ID="btnEditar" runat="server"  CssClass="btn-default btn-sm" OnClick="btnEditar_Click" Text="Editar" />
            </div>

        </asp:Panel>
    </div>

    <div class="col-lg-12">
        <asp:Panel ID="pnlRegistro" runat="server">
            <table class="table-hover">
                <tr>
                    <td><asp:Label ID="lblId" runat="server" Text="Hotel Id:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtId" runat="server"  CssClass="form-control" placeholder="Aquí el número ID"  Enabled="False" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCuit" runat="server" Text="CUIT:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCuit"  CssClass="form-control" placeholder="XX-YYYYYYYY-Z"  runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtCuit" ID="rf_cuit" runat="server" ErrorMessage="Ingrese CUIT"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Nombre:</td>
                    <td>
                        <asp:TextBox ID="txtdescripcion"  CssClass="form-control" placeholder="Aquí el nombre del hotel"  runat="server" ></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtdescripcion" ID="rf_descripcion" runat="server" ErrorMessage="Ingrese Nombre"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Fecha Inicio Actividades:</td>
                    <td>
                        <asp:TextBox ID="txtFechaInicioActividades" CssClass="datepicker form-control"  placeholder="Click aqui para seleccionar fecha" runat ="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtFechaInicioActividades" ID="rf_fecha" runat="server" ErrorMessage="Ingrese Fecha"></asp:RequiredFieldValidator>
                    </td>
                    
                     <script type="text/javascript">
                         $(document).ready(
                             function () {
                    
                                 $(".datepicker").datepicker({
                                     dateFormat: "dd/mm/yy",
                                     monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                                     dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                                     showAnim: "drop",
                                     //minDate: -20, maxDate: "+1M +10D" para que muestre un cierto rango de fechas

                                 });
                             }
                             );
                     </script>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCapacidad" runat="server" Text="Capacidad"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCapacidad" runat="server" CssClass="form-control" placeholder="Capacidad de pasajeros"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtCapacidad" ID="rf_capacidad" runat="server" ErrorMessage="Ingrese Capacidad"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Destino</td>
                    <td>
                        <asp:DropDownList ID="ddlDestino"  CssClass="form-control" runat="server" Enabled="False"></asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddlDestino" ID="fr_destino" runat="server" ErrorMessage="Seleccione"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPermiteMascota" runat="server" Text="Admite Mascotas"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rb_list" runat="server">
                            <asp:ListItem Value="1">Sí</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ControlToValidate="rb_list" ID="rf_mascota" runat="server" ErrorMessage="Seleccione una opción"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnGrabar" runat="server" CssClass="btn-default btn-sm" OnClick="btnGrabar_Click" Text="Grabar" />
                        <asp:Button ID="btnCancelar" runat="server" CssClass="btn-default btn-sm" OnClick="btnCancelar_Click" Text="Cancelar/Volver" />
                        <asp:Button ID="btn_confirmarEliminar" runat="server" CssClass="btn-default btn-sm" OnClick="btn_confirmarEliminar_Click" Text="confirmar Eliminar" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <div class="col-lg-12">
            <h4><asp:Label ID="lbl_mensaje"  CssClass="form-control label alert-danger" runat="server"></asp:Label></h4> 
        </div>
    </div>
</asp:Content>


