using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;


public partial class Hotelwf : System.Web.UI.Page
{
    private bool grabar;
    protected void Page_Load(object sender, EventArgs e)
    {
        habilitar_panelRegistro(false);

        if (!Page.IsPostBack)
        {//TODO no funciona esto
            //if (!Page.User.IsInRole("Administradores"))
            //{
            //    btnEliminar.Visible = false;
            //    btnEditar.Visible = false;
            //    btnAgregar.Visible = false;
            //}

            ViewState["GvDatosOrden"] = "descripcion";
            GridView1.AllowPaging = true;
            GridView1.AllowSorting = true;
            //definir columnas que se van a ve dataBound --> .textheader(titulo), datafield(propiedades del datasource), sortexpression
            GridView1.PageSize = 7;
            cargarGrilla(chk_eliminados.Checked);
            accion("");
            cargarCombo();
        }
    }


    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        if (GridView1.SelectedRow == null)
        {
            mensaje("Debe seleccionar un hotel");
        }
        else
        {
            habilitar_panelRegistro(true);
            habilitar_botones("consultar");
            habilitar_campos(false);
            mensaje("");
            accion("Consultando..");

            try
            {
                recuperar(chk_eliminados.Checked);
            }
            catch (Exception ex)
            {
                mensaje(ex.Message);
            }
        }
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {

        if (GridView1.SelectedRow == null)
            mensaje("Debe seleccionar un hotel");

        else
        {
            habilitar_panelRegistro(true);
            habilitar_botones("eliminar_click");
            mensaje("");
            accion("Eliminando..");
            habilitar_campos(false);

            recuperar(chk_eliminados.Checked);
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        habilitar_botones("agregar_editar");
        accion("Agregando..");
        mensaje("");
        habilitar_campos(true);
        reiniciarPaneles();
        habilitar_panelRegistro(true);
        grabar = true;
        lblId.Visible = false;
        txtId.Visible = false;
    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        if (GridView1.SelectedRow == null)
            mensaje("Debe seleccionar un hotel");
        else
        {
            habilitar_panelRegistro(true);
            habilitar_botones("agregar_editar");
            if (chk_eliminados.Checked)
                accion("Recuperando..");
            else
                accion("Editando..");
            habilitar_campos(true);
            grabar = false;
            recuperar(chk_eliminados.Checked);
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        habilitar_panelRegistro(false);
        cargarGrilla(chk_eliminados.Checked);
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
            mensaje(ex.Message);
        }

        habilitar_panelRegistro(false);
        cargarGrilla(chk_eliminados.Checked);
    }
    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        habilitar_panelRegistro(false);
        accion("Grabando..");
        Hotel h = new Hotel();

        if (Page.IsValid)
        {
            mensaje("la página es válida");
        }

        if (validar())
        {
            if (txtCuit.Text != "")
                h.cuit = limpiarCUIT();
            else
                h.cuit = -1;

            h.descripcion = txtdescripcion.Text;

            if (txtCapacidad.Text != "")
                h.capacidad = Convert.ToInt32(txtCapacidad.Text);
            else
                h.capacidad = -1;

            h.destino = Convert.ToInt32(ddlDestino.SelectedValue);

            if (rb_list.SelectedValue == "1")
                h.aceptaMascota = true;
            else
                h.aceptaMascota = false;

            if (txtFechaInicioActividades.Text != "")
                h.inicioActividad = Convert.ToDateTime(txtFechaInicioActividades.Text);
            else
                h.inicioActividad = Convert.ToDateTime("01/01/2000");


            GestorHotel.Grabar(h, grabar); //si está habilitado el textID es porq graba, sino actualiza
            cargarGrilla(chk_eliminados.Checked);
        }

    }
    Regex Validar_numeros = new Regex(@"^[0-9]*$");
    Regex Validar_cuit = new Regex(@"^[0-9]{2}-[0-9]{8}-[0-9]$");
    
    private long limpiarCUIT()
    {
        string var = "-1";
        if (txtCuit.Text != "")
            var = txtCuit.Text.Replace("-", "");
        return Convert.ToInt64(var);
    }
    private Boolean validar()
    {

        if (txtCuit.Text == "" || !Validar_cuit.IsMatch(txtCuit.Text))
        {
            rechazar_grabado(txtCuit);
            return false;
        }

        if (txtCuit.Text == ""  || GestorHotel.existeCuit(limpiarCUIT()) && grabar)
        {
            rechazarCuit_repetido(txtCuit.Text);
            return false;
        }

        if (txtdescripcion.Text == "")
        {
            rechazar_grabado(txtdescripcion);
            return false;
        }
        if (txtCapacidad.Text == "" || !Validar_numeros.IsMatch(txtCapacidad.Text))
        {
            rechazar_grabado(txtCapacidad);
            return false;
        }

        if (rb_list.SelectedItem == null)
        {
            rechazar_grabado(rb_list);
            return false;
        }

        if (ddlDestino.SelectedValue == "0" || ddlDestino.SelectedValue == "" || ddlDestino.SelectedValue == "-1")
        {
            rechazar_grabado(ddlDestino);
            return false;
        }

        return true;
    }
    private void rechazar_grabado(Control c)
    {
        habilitar_panelRegistro(true);
        mensaje("Debe ingresar un " + c.ID + " válido");
        c.Focus();
    }

    private void rechazar_repetido(string id)
    {
        habilitar_panelRegistro(true);
        mensaje("Ya existe un hotel con ID " + id);
        txtId.Focus();
    }

    private void rechazarCuit_repetido(string cuit)
    {
        habilitar_panelRegistro(true);
        mensaje("Ya existe un hotel con CUIT: " + cuit);
        txtCuit.Focus();
    }


    private void habilitar_campos(Boolean estado)
    {
        txtdescripcion.Enabled = estado;
        ddlDestino.Enabled = estado;
        txtCapacidad.Enabled = estado;
        txtCuit.Enabled = estado;
        rb_list.Enabled = estado;
        txtFechaInicioActividades.Enabled = estado;
    }



    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        cargarGrilla(chk_eliminados.Checked);
    }

    private void recuperar(bool eliminados)
    {
        int id = (int)GridView1.SelectedValue;
        Hotel h = GestorHotel.buscarPorId(id, eliminados);
        txtId.Text = h.id.ToString();
        txtdescripcion.Text = h.descripcion;
        ddlDestino.SelectedValue = h.destino.ToString();
        txtCuit.Text = h.cuit.ToString();
        txtCapacidad.Text = h.capacidad.ToString();
        if (rb_list.SelectedValue == "1")
            rb_list.Items[1].Selected = true;
        else
            rb_list.Items[0].Selected = true;

        txtFechaInicioActividades.Text =  Convert.ToString( h.inicioActividad); 
    }


    public void mensaje(string msj)
    {
        lbl_mensaje.Text = msj;
    }
    public void accion(string mensaje)
    {
        lblAccion.Text = mensaje;
    }
    public void reiniciarPaneles()
    {
        txtId.Text = "";
        txtCuit.Text = "";
        ddlDestino.ClearSelection();
        rb_list.ClearSelection();
        txtCapacidad.Text = "";
        txtdescripcion.Text = "";
    }

    private void cargarCombo()
    {
        ddlDestino.DataTextField = "descripcion";
        ddlDestino.DataValueField = "id";
        ddlDestino.DataSource = GestorDestino.ObtenerTodas();
        ddlDestino.DataBind();
        ddlDestino.Items.Insert(0, new ListItem("Elija una provincia", "0"));
    }
    public void cargarGrilla(bool eliminados)
    {
        string orden = ViewState["GvDatosOrden"].ToString();
        GridView1.DataSource = GestorHotel.BuscarPordescripcion(txtbxBuscar.Text, orden, eliminados);
        GridView1.DataBind();
        mensaje(GridView1.Rows.Count.ToString()+" hoteles encontrados");
        accion("");
    }
    //TODO 05: no tiene ninguna referencia, ese debe ser el problema de q no anda
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["GvDatosOrden"] = e.SortExpression;
        cargarGrilla(chk_eliminados.Checked);
    }


    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }


    public void habilitar_panelRegistro(bool accion)
    {
        Panel1.Visible = !accion;
        pnlRegistro.Visible = accion;
    }

    public void habilitar_botones(string accion)
    {
            switch (accion)
            {
                case "eliminar_click":

                    btnGrabar.Visible = false;
                    btn_confirmarEliminar.Visible = true;
                    btnCancelar.Visible = true;
                    break;

                case "agregar_editar":
                    btn_confirmarEliminar.Visible = false;
                    btnGrabar.Visible = true;
                    btnCancelar.Visible = true;


                    break;

                case "consultar":
                    btn_confirmarEliminar.Visible = false;
                    btnGrabar.Visible = false;
                    btnCancelar.Visible = true;
                    Panel1.Visible = false;
                    pnlRegistro.Visible = true;
                    break;
            }
    }


    protected void chk_eliminados_CheckedChanged(object sender, EventArgs e)
    {
        btnEliminar.Visible = !chk_eliminados.Checked;
        if (chk_eliminados.Checked)
            btnEditar.Text = "Recuperar";
        else
            btnEditar.Text = "Editar";
        cargarGrilla(chk_eliminados.Checked);
    }
}