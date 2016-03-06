using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;

namespace PortalTrabajadores.Portal
{
    public partial class CambioContrasenaST : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["trabajadoresConnectionString"].ConnectionString.ToString();

        #region Definicion de los Metodos de la Clase

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
                if (!IsPostBack)
                {
                    CnMysql Conexion = new CnMysql(Cn);
                    try
                    {
                        DataTable dtDataTable = null;
                        Conexion.AbrirCnMysql();
                        dtDataTable = Conexion.ConsultarRegistros("SELECT descripcion FROM basica_trabajador.Options_Menu WHERE url = 'CambioContrasenaST.aspx' AND Tipoportal = 'A'");
                        if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                        {
                            this.lblTitulo.Text = dtDataTable.Rows[0].ItemArray[0].ToString();
                        }
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

        #region Metodo BtnCambiar_Click
        /* ****************************************************************************/
        /* Evento que procede a realizar el cambio de contraseña
        /* ****************************************************************************/
        protected void BtnCambiar_Click(object sender, EventArgs e)
        {
            CnMysql Conexion = new CnMysql(Cn);
            MySqlDataReader rd;

            try
            {
                //Se valida que se haya digitado un usuario
                if (txtuser.Text.Trim() == "")
                {
                    MensajeError("Digite un Número de Identificación.");
                }
                else
                {
                    Conexion.AbrirCnMysql();
                    MySqlCommand cmd = new MySqlCommand("trabajadores.sp_CambioContrasena", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cedula", txtuser.Text);
                    cmd.Parameters.AddWithValue("@empresa", "ST");
                    rd = cmd.ExecuteReader();

                    rd.Read();
                    if (rd.IsDBNull(0))
                    {
                        MensajeError("No Existe un Número de Identificación asociado a un Empleado con el Dato Ingresado.");
                        txtuser.Text = "";
                        txtuser.Focus();
                    }
                    else
                    {
                        MensajeError("Se ha reseteado la Contraseña al empleado identificado con la cédula " + txtuser.Text + ". Contraseña Anterior: " + rd[1].ToString() + " Contraseña Nueva: " + rd[0].ToString());
                    }
                    rd.Close();
                    rd.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            finally
            {
                Conexion.CerrarCnMysql();     
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
        #endregion
        
        #endregion

    }
}