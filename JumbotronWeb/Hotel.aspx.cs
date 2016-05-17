﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;



    //ESTÁ HARDCODEADO EL PASO DE DESTINO Y CAPACIDAD
    
    //AL EDITAR, SE BLOQUEA EL TXT, Y DE ESO DEPENDERÁ QUE DESPUÉS GRABAR() HAGA UN INSERT O UN UPDATE
    //SI TXT_ID ESTA HABILITADO ES PORQUE SE TRATA DE UN INSERT
    //SI TXT_ID ESTA DESHABILITADO ES PORQUE SE TRATA DE UNA MODIFICACIÓN, OSEA UPDATE

    //La cadena de conexión se escribe una sola vez, en la clase GestorHotel
    //La cadena de conexión se escribe una sola vez, en la clase GestorHotel
    //La cadena de conexión se escribe una sola vez, en la clase GestorHotel
    //Todos los campos CODIGO fueron cambiados por ID
    //Todos los campos NOMBRE fueron cambiados por DESCRIPCION
    //se escribieron métodos para validar el relleno de los campos.. validar()
    //se escribió un método para habilitar/deshabilitar los campos.. habilitar(true para q habilite, false para q deshabilite)
    //se escribió un método para rechazar la grabación de campos defectuosos, haciendo foco en el control q lo produjo
    

    //hay q controlar el grabado de nuevos hoteles con ID existente
    //Hay que incluir paginación.. no se q onda pero se rompe tooo
    //no valida bien el ingreso de ID.. podes meter "30AAA" hay q usar expresiones regulares
    



public partial class Hotelwf : System.Web.UI.Page
{
    public string CadenaConexion = @"Data Source=DAVID-PC\SQLEXPRESS;Initial Catalog=4K1_62726;Integrated Security=True";
    protected void Page_Load(object sender, EventArgs e)
    {
        Panel1.Visible = true;
        pnlRegistro.Visible = false;

        lbl_mensaje.Text = "";

        ddlDestino.DataTextField = "descripcion";
        ddlDestino.DataValueField = "id";
        ddlDestino.DataSource = GestorDestino.ObtenerTodas();
        ddlDestino.DataBind();
        ddlDestino.Items.Insert(0, new ListItem("Elija una provincia", "0"));
        lblAccion.Text = "";

        if (!Page.IsPostBack)
        {
            ViewState["GvDatosOrden"] = "descripcion";
            GridView1.AllowPaging = true;
            GridView1.AllowSorting = true;
            //definir columnas que se van a ve dataBound --> .textheader(titulo), datafield(propiedades del datasource), sortexpression
            GridView1.PageSize = 7;
            cargarGrilla();
            lblAccion.Text = "";
        }
    }
    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        Panel1.Visible = false;
        pnlRegistro.Visible = true;
        lblAccion.Text = "Agregando..";
        habilitar(true);
        ddlDestino.ClearSelection();
        btnGrabar.Visible= true;
        btnCancelar.Visible = true;
        btn_confirmarEliminar.Visible = false;
        txtdescripcion.Text="";
        txtId.Text = "";
    }

    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        if (GridView1.SelectedRow == null)
        {
            lbl_mensaje.Visible = true;
            lbl_mensaje.Text = "Debe seleccionar un hotel";
        }
        else
        {
            btn_confirmarEliminar.Visible = false;
            btnGrabar.Visible = false;
            Panel1.Visible = false;
            pnlRegistro.Visible = true;
            habilitar(false);
            lblAccion.Text = "Consultando..";
            lbl_mensaje.Text = "";

            try
            {
                recuperar();
            }
            catch (Exception ex)
            {
                lbl_mensaje.Text = ex.Message;
            }
          }
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {

        if (GridView1.SelectedRow == null)
        {
            lbl_mensaje.Visible = true;
            lbl_mensaje.Text = "Debe seleccionar un hotel";
        }
        else
        {

            lbl_mensaje.Text = "";
            Panel1.Visible = false;
            pnlRegistro.Visible = true;
            lblAccion.Text = "Eliminando..";
            habilitar(false);

            recuperar();

            btnGrabar.Visible = false;
            btn_confirmarEliminar.Visible = true;
            btnCancelar.Visible = true;
        }

    }

    private void habilitar(Boolean estado)
    {
        txtId.Enabled = estado;
        txtdescripcion.Enabled = estado;
        ddlDestino.Enabled = estado;
        
    }

    private void recuperar()
    {
        int id = (int)GridView1.SelectedValue;
        Hotel h = GestorHotel.buscarPorId(id);
        txtId.Text = h.id.ToString();
        txtdescripcion.Text = h.descripcion;
        ddlDestino.SelectedValue = h.destino.ToString();
        txtCuit.Text = h.cuit.ToString();
        txtCapacidad.Text = h.capacidad.ToString();
        if (h.aceptaMascota)
            rbtnSi.Checked = true;
        else
            rbrtnNo.Checked = true;

    }


    protected void btnEditar_Click(object sender, EventArgs e)
    {

        if (GridView1.SelectedRow == null)
        {
            lbl_mensaje.Visible = true;
            lbl_mensaje.Text = "Debe seleccionar un hotel";
        }
        else
        {
            Panel1.Visible = false;
            pnlRegistro.Visible = true;
            lblAccion.Text = "Editando..";
            ddlDestino.Enabled = true;
            txtdescripcion.Enabled = true;
            txtId.Enabled = false; //PERMITIMOS Q PUEDA EDITAR EL ID?

            recuperar();
        }
    }
    protected void btnGrabar_Click(object sender, EventArgs e) //HARDCODEANDO DESTINO Y CAPACIDAD
    {
        Panel1.Visible = true;
        pnlRegistro.Visible = false;
        lblAccion.Text = "Grabando..";
        Hotel h = new Hotel();

        if(validar())
        {
            h.id = Convert.ToInt32(txtId.Text);
            h.descripcion = txtdescripcion.Text;
            h.destino = 1; //DE DÓNDE SACAR EL DESTINO?
            h.capacidad = 5;
            GestorHotel.Grabar(h, txtId.Enabled); //si está habilitado el ID es porq graba, sino actualiza
            cargarGrilla();
        }

    }
    Regex Validar_numeros = new Regex(@"[0-9]{1,9}(\.[0-9]{0,2})?$");
    private Boolean validar()
    {
        if (txtId.Text != "" && !Validar_numeros.IsMatch(txtId.Text))
        {
            rechazar_grabado(txtId);
            return false;
        }
        if (txtdescripcion.Text == "")
        {
            rechazar_grabado(txtdescripcion);
            return false;
        }
        //ESTO TODAVÍA NO LO VALIDO PORQ HAY PROBLEMAS CON EL SELECTEDVALUE. VER MÁS ARRIBA
        //if (ddlDestino.SelectedValue == "0" || ddlDestino.SelectedValue == null)
        //{
        //    rechazar_grabado(ddlDestino);
        //    return false;
        //}
        return true;
    }
    private void rechazar_grabado(Control c )
    {
        Panel1.Visible = false;
        pnlRegistro.Visible = true;
        lbl_mensaje.Visible = true;
        lbl_mensaje.Text = "Debe ingresar un " + c.ID + " válido";
        c.Focus();
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Panel1.Visible = true;
        pnlRegistro.Visible = false;
        cargarGrilla();   
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        cargarGrilla();
    }

    protected void btn_confirmarEliminar_Click(object sender, EventArgs e)
    {
                try
                {
                    int id = (int)GridView1.SelectedValue;
                    GestorHotel.Eliminar(id);
                }
                catch (Exception ex)
                {
                    lbl_mensaje.Text = ex.Message;
                }
            
                 pnlRegistro.Visible = false;
                 Panel1.Visible = true;
                 cargarGrilla();
        
    }

    public void reiniciarPaneles()
    {
        pnlRegistro.Visible = false;
        Panel1.Visible = true;
        lblAccion.Text = "";
    }

    public void cargarGrilla()
    {
        string orden = ViewState["GvDatosOrden"].ToString();
        GridView1.DataSource = GestorHotel.BuscarPordescripcion(txtbxBuscar.Text, orden);
        GridView1.DataBind();
        lblAccion.Text = "";
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["GvDatosOrden"] = e.SortExpression;
        cargarGrilla();
    }


    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
  
}