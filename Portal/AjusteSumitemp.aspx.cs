using MySql.Data.MySqlClient;
using PortalTrabajadores.Class;
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
    public partial class AjusteSumitemp : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["basicaConnectionString"].ConnectionString.ToString();
        string Cn2 = ConfigurationManager.ConnectionStrings["trabajadoresConnectionString"].ConnectionString.ToString();
        string bd1 = ConfigurationManager.AppSettings["BD1"].ToString();
        string bd2 = ConfigurationManager.AppSettings["BD2"].ToString();
        ConsultasGenerales consultas;

        MySqlConnection MySqlCn;

        #region Metodo Page Load
        /* ****************************************************************************/
        /* Metodo que se ejecuta al momento de la carga de la Pagina
        /* ****************************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            MySqlCn = new MySqlConnection(Cn);
            consultas = new ConsultasGenerales();

            if (Session["usuario"] == null)
            {
                //Redirecciona a la pagina de login en caso de que el usuario no se haya autenticado
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    CnMysql Conexion = new CnMysql(Cn);
                    try
                    {
                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM " + bd1 + ".Options_Menu WHERE url = 'AjusteSumitemp.aspx' AND TipoPortal = 'A'", Conexion.ObtenerCnMysql());
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

                        this.LoadProyectos();
                    }
                    catch (Exception E)
                    {
                        MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Sin RED");
                    }
                    finally
                    {
                        Conexion.CerrarCnMysql();
                    }
                }
            }
        }
        #endregion

        #region Metodo MensajeError
        /* ****************************************************************************/
        /* Metodo que habilita el label de mensaje de error
        /* ****************************************************************************/
        //IniciaMetodo
        public void MensajeError(string Msj)
        {
            LblMsj.Text = Msj;
            LblMsj.Visible = true;
            UpdatePanel3.Update();
        }
        #endregion

        protected void BtnProyectos_Click(object sender, EventArgs e)
        {
            try
            {
                bool respuesta = consultas.ConsultarCargosTrabajador(DropListProyecto.SelectedItem.Value);

                if (respuesta)
                {
                    MensajeError("Los usuarios fueron actualizados.");
                }
                else
                {
                    MensajeError("Error al actualizar.");
                }
            }
            catch (Exception ex)
            {
                MensajeError("Error: " + ex.Message);
            }            
        }

        private void LoadProyectos()
        {
            LlenadoDropBox utilLlenar = new LlenadoDropBox();
            string command = "SELECT idCompania, Descripcion_compania FROM " + bd2 +
                             ".companias where Empresas_idempresa = 'ST' and activo_compania = 1 " +
                             " and Terceros_Nit_Tercero = 8002374329";
            DropListProyecto.Items.Clear();
            DropListProyecto.DataSource = utilLlenar.LoadTipoID(command);
            DropListProyecto.DataTextField = "Descripcion_compania";
            DropListProyecto.DataValueField = "idCompania";
            DropListProyecto.DataBind();
        }
    }
}