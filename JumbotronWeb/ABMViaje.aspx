﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ABMViaje.aspx.cs" Inherits="ABMViaje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="col-lg-12">
        <h1 class="page-header">Viajes</h1>
        <ol class="breadcrumb">
            <li><a href="Inicio.aspx">Inicio</a></li>
            <li><a href="Viaje.aspx">Viajes</a></li>
            <li class="active">ABM Viaje</li>
            
        </ol>
    </div>

    <div class="col-lg-12">
        <h4><asp:Label ID="lblAccion" CssClass="label label-success" runat="server" Text="Label"></asp:Label></h4> 
    </div>

    <asp:Panel ID="pnlConsulta" runat="server">
        <strong>Viaje:</strong>
        <div class="form-inline">
                <asp:TextBox ID="txtbxBuscar"  CssClass="form-control input-sm" title="Ingrese nombres de viajes" placeholder="Nombre del Viaje" runat="server">
                </asp:TextBox>
                <asp:CheckBox ID="chk_eliminados" CssClass="checkbox-inline" Text="Ver eliminados" runat="server" OnCheckedChanged="chk_eliminados_CheckedChanged" />
                <asp:Button ID="btnBuscar" CssClass="btn-sm btn-primary" role="button" type="submit" runat="server" Text="Buscar" OnClick="btnBuscar_Click"  /> 
                </div>

            <table class="table table-bordered">
                <asp:GridView ID="gvViajes" runat="server" CssClass="table table-responsive table-bordered table-hover" CellPadding="4" DataKeyNames="id" GridLines="None" AutoGenerateColumns="False">   <%-- OnPageIndexChanging="GridView1_PageIndexChanging" OnSorting="GridView1_Sorting"--%>
                    <columns>
                        <asp:CommandField ShowSelectButton="True" />
                        <asp:BoundField DataField="id" HeaderText="ID" />
                        <asp:BoundField DataField="descripcion" HeaderText="Descrip" SortExpression="descripcion" />
                        <asp:BoundField DataField="hotelNombre" HeaderText="Hotel" />
                        <asp:BoundField DataField="precio" HeaderText="Precio" />
                        <asp:BoundField DataField="fechaSalida" HeaderText="Fecha Salida" />
                        <asp:BoundField DataField="fechaLlegada" HeaderText="Fecha Lleg" />
                        <asp:BoundField DataField="destinoNombre" HeaderText="Destino" />
                        <asp:BoundField DataField="cupo" HeaderText="Cupo" />
                        <asp:BoundField DataField="transporteNombre" HeaderText="Transporte" />
                    </columns>
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

      <div class="col-lg-12">
         <asp:Panel ID="pnlRegistro" runat="server">    
            <table class="table-hover">
                <tr>
                    <td><asp:Label ID="lblId" runat="server" Text="Viaje ID:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtId" runat="server"  CssClass="form-control" placeholder="Aquí el número ID"  Enabled="False" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>-Nombre:</td>
                    <td>
                        <asp:TextBox ID="txtdescripcion"  CssClass="form-control" placeholder="Aquí el nombre del viaje"  runat="server" ></asp:TextBox>
                    </td>              
                </tr>
                <tr>
                    <td>
                        Destino
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDestino" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                   
                </tr>
                <tr>
                    <td>Hotel:</td>
                    <td>
                        <asp:DropDownList ID="ddlHotel"  CssClass="form-control" runat="server" >
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Precio Viaje:</td>
                    <td>
                        <asp:TextBox ID="txtPrecio"  CssClass="form-control"  placeholder="Aqui el Precio del Viaje" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Fecha Salida:

                    </td>
                    <td>
                        <asp:TextBox ID="txtFechaSalida" CssClass="datepicker form-control"  placeholder="Click aqui para seleccionar fecha" runat ="server"></asp:TextBox>
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
                    <td>Fecha Llegada:</td>
                    <td>
                        <asp:TextBox ID="txtFechaLlegada" CssClass="datepicker form-control"  placeholder="Click aqui para seleccionar fecha" runat ="server"></asp:TextBox>
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
                    <td>Cupo:</td>
                    <td>
                        <asp:TextBox ID="txtCupo"  CssClass="form-control" placeholder="Aquí el cupo del viaje"  runat="server" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Transporte:</td>
                    <td>
                        <asp:DropDownList ID="ddlTransporte" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Diponibilidad:
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rb_Disponibilidad" runat="server">
                            <asp:ListItem Value="1">Sí</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>Imagen:</td>
                    <td> <asp:TextBox ID="txtimagen"  CssClass="form-control" placeholder="Imagen"  runat="server" ></asp:TextBox>
                        </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnGrabar" runat="server" CssClass="btn-default btn-sm" OnClick="btnGrabar_Click" Text="Grabar" />
                        <asp:Button ID="btnCancelar" runat="server" CssClass="btn-default btn-sm" OnClick="btnCancelar_Click" Text="Cancelar/Volver" />
                        <asp:Button ID="btn_confirmarEliminar" runat="server" CssClass="btn-default btn-sm" OnClick="btn_confirmarEliminar_Click" Text="confirmar Eliminar" />
                    <td>
                </tr>
            </table>       
        </asp:Panel>
        <div class="col-lg-12">
            <h4><asp:Label ID="lbl_mensaje"  CssClass="form-control label alert-danger" runat="server"></asp:Label></h4> 
        </div>
    </div>
</asp:Content>

