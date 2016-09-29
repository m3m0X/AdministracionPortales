using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PortalTrabajadores.Portal
{
    public partial class Modulos : System.Web.UI.Page
    {
        string Cn1 = ConfigurationManager.ConnectionStrings["basicaConnectionString"].ConnectionString.ToString();
        string Cn2 = ConfigurationManager.ConnectionStrings["trabajadoresConnectionString"].ConnectionString.ToString();
        string bd1 = ConfigurationManager.AppSettings["BD1"].ToString();
        string bd2 = ConfigurationManager.AppSettings["BD2"].ToString();
        MySqlConnection MySqlCn;

        #region Metodo Page Load
        /* ****************************************************************************/
        /* Metodo que se ejecuta al momento de la carga de la Pagina
        /* ****************************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                //Redirecciona a la pagina de login en caso de que el usuario no se halla autenticado
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                CnMysql Conexion = new CnMysql(Cn1);

                if (!IsPostBack)
                {
                    MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM " + bd1 + ".Options_Menu WHERE url = 'Modulos.aspx' and Tipoportal = 'A'", Conexion.ObtenerCnMysql());
                    MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                    DataSet dsDataSet = new DataSet();
                    DataTable dtDataTable = null;

                    Conexion.AbrirCnMysql();
                    sdaSqlDataAdapter.Fill(dsDataSet);
                    dtDataTable = dsDataSet.Tables[0];
                    if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                    {
                        this.lblTitulo.Text = dtDataTable.Rows[0].ItemArray[0].ToString();
                    }
                }
            }
        }
        #endregion

        #region Metodo MensajeError
        /* ****************************************************************************/
        /* Metodo que habilita el label de mensaje de error
        /* ****************************************************************************/
        public void MensajeError(string Msj)
        {
            LblMsj.Text = Msj;
            LblMsj.Visible = true;
            UpdatePanel3.Update();
        }

        /* ****************************************************************************/
        /* Metodo que limpia los mensajes
        /* ****************************************************************************/
        public void LimpiarMensajes()
        {
            LblMsj.Text = string.Empty;
            LblMsj.Visible = false;
            UpdatePanel3.Update();
        }

        #endregion

        /* ****************************************************************************/
        /* Busca el nit en BD
        /* ****************************************************************************/
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            this.LimpiarMensajes();

            try
            {
                if (this.txtNit.Text != string.Empty)
                {
                    this.txtNit.Focus();

                    LlenadoDropBox utilLlenar = new LlenadoDropBox();
                    string command = "SELECT idCompania, " +
                                     "if (Descripcion_compania2 != '' AND Descripcion_Compania2 != 'NA', " +
                                     "Descripcion_Compania2, Descripcion_compania) as Descripcion FROM " + bd2 +
                                     ".companias where Empresas_idempresa = '" + this.ddlEmpresa.SelectedItem.Value +
                                     "' and activo_compania = 1 and Terceros_Nit_Tercero =" + this.txtNit.Text;

                    DataTable datos = utilLlenar.LoadTipoID(command);

                    if (datos != null)
                    {
                        this.DropListProyecto.Items.Clear();
                        this.DropListProyecto.DataSource = utilLlenar.LoadTipoID(command);
                        this.DropListProyecto.DataTextField = "Descripcion";
                        this.DropListProyecto.DataValueField = "idCompania";
                        this.DropListProyecto.DataBind();

                        this.Container_UpdatePanel2.Visible = true;
                        this.UpdatePanel1.Update();
                        this.txtNit.Enabled = false;
                        this.ddlEmpresa.Enabled = false;
                        this.BtnBuscar.Enabled = false;
                        this.BtnRestablecer.Enabled = true;
                    }
                    else
                    {
                        this.MensajeError("No se han encontrado resultados.");
                        this.Container_UpdatePanel2.Visible = false;
                        this.UpdatePanel1.Update();
                        this.txtNit.Enabled = true;
                        this.ddlEmpresa.Enabled = true;
                        this.BtnBuscar.Enabled = true;
                        this.BtnRestablecer.Enabled = false;
                    }
                }
                else
                {
                    this.MensajeError("Debe ingresar un nit.");
                }
            }
            catch (Exception E)
            {
                this.MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /* ****************************************************************************/
        /* Reestablece el formulario
        /* ****************************************************************************/
        protected void BtnRestablecer_Click(object sender, EventArgs e)
        {
            this.LimpiarMensajes();
            this.Container_UpdatePanel2.Visible = false;
            this.Container_UpdatePanel3.Visible = false;
            this.UpdatePanel1.Update();
            this.txtNit.Enabled = true;
            this.txtNit.Text = string.Empty;
            this.ddlEmpresa.Enabled = true;
            this.BtnBuscar.Enabled = true;
            this.BtnRestablecer.Enabled = false;
        }

        /* ****************************************************************************/
        /* Carga los proyectos
        /* ****************************************************************************/
        protected void BtnProyectos_Click(object sender, EventArgs e)
        {
            this.LimpiarMensajes();
            this.CargarProyectos();
        }

        /* ****************************************************************************/
        /* Carga la grilla con las opciones de activar y desactivar
        /* ****************************************************************************/
        protected void gvModulosActivos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            this.LimpiarMensajes();
            int res = 0;

            CnMysql Conexion = new CnMysql(Cn1);
            Conexion.AbrirCnMysql();

            try
            {
                string idmat = e.CommandArgument.ToString().Split(';')[0];
                string idmod = e.CommandArgument.ToString().Split(';')[1];

                if (e.CommandName == "On" || e.CommandName == "Off")
                {
                    if (e.CommandName == "Off")
                    {
                        MySqlCommand cmd = new MySqlCommand("sp_CrearMatrizMod", Conexion.ObtenerCnMysql());
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idmodulo", idmod);
                        cmd.Parameters.AddWithValue("@idtercero", this.txtNit.Text);
                        cmd.Parameters.AddWithValue("@idcompania", DropListProyecto.SelectedItem.Value);
                        cmd.Parameters.AddWithValue("@idempresa", ddlEmpresa.SelectedItem.Value);

                        // Crea un parametro de salida para el SP
                        MySqlParameter outputIdParam = new MySqlParameter("@respuesta", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        cmd.Parameters.Add(outputIdParam);
                        cmd.ExecuteNonQuery();
                        //Almacena la respuesta de la variable de retorno del SP
                        res = int.Parse(outputIdParam.Value.ToString());

                        if (res > 0)
                        {
                            this.CargarProyectos();
                        }
                        else 
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:CargarMensaje('No se puede activar. Revise con su administrador.'); ", true);
                        }
                    }
                    else
                    {
                        MySqlCommand cmd = new MySqlCommand("sp_EliminarMatrizMod", Conexion.ObtenerCnMysql());
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idMatriz", idmat);

                        // Crea un parametro de salida para el SP
                        MySqlParameter outputIdParam = new MySqlParameter("@respuesta", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        cmd.Parameters.Add(outputIdParam);
                        cmd.ExecuteNonQuery();
                        //Almacena la respuesta de la variable de retorno del SP
                        res = int.Parse(outputIdParam.Value.ToString());

                        if (res == 1)
                        {
                            this.CargarProyectos();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:CargarMensaje('No se pudo desactivar. Revise con su administrador'); ", true);
                        }
                    }
                }
            }
            catch (Exception E)
            {
                this.MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /* ****************************************************************************/
        /* Metodo se ejecuta al cargar los datos en la grilla
        /* ****************************************************************************/
        protected void gvModulosActivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            this.LimpiarMensajes();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgbtnON = (ImageButton)e.Row.FindControl("btnON");
                ImageButton imgbtnOFF = (ImageButton)e.Row.FindControl("btnOFF");

                string estado = DataBinder.Eval(e.Row.DataItem, "ModActivo").ToString();

                if (estado == "1")
                {
                    imgbtnON.Visible = true;
                    imgbtnOFF.Visible = false;
                }
                else
                {
                    imgbtnON.Visible = false;
                    imgbtnOFF.Visible = true;
                }
            }
        }

        /* ****************************************************************************/
        /* Carga los proyectos en el ddlist
        /* ****************************************************************************/
        private void CargarProyectos()
        {
            try
            {
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;

                MySqlCn = new MySqlConnection(Cn1);
                MySqlCommand scSqlCommand;
                scSqlCommand = new MySqlCommand("sp_ConsultarModulosCompania", MySqlCn);
                scSqlCommand.CommandType = CommandType.StoredProcedure;
                scSqlCommand.Parameters.AddWithValue("@idCompania", DropListProyecto.SelectedItem.Value);
                scSqlCommand.Parameters.AddWithValue("@idEmpresa", ddlEmpresa.SelectedItem.Value);

                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    this.gvModulosActivos.DataSource = dtDataTable;
                }
                else
                {
                    this.gvModulosActivos.DataSource = null;
                }

                this.gvModulosActivos.DataBind();
                this.Container_UpdatePanel3.Visible = true;
                this.UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                this.MensajeError("Ha ocurrido el siguiente error: " + ex.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}