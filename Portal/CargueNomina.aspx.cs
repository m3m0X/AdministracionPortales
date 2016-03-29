using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace PortalTrabajadores.Portal
{
    public partial class CargueNomina : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["trabajadoresConnectionString"].ConnectionString.ToString();
        string ruta = ConfigurationManager.AppSettings["RutaFisica"].ToString();
        string bd1 = ConfigurationManager.AppSettings["BD1"].ToString();
        string bd2 = ConfigurationManager.AppSettings["BD2"].ToString();

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
                        dtDataTable = Conexion.ConsultarRegistros("SELECT descripcion FROM " + bd1 + ".Options_Menu WHERE url = 'CargueNomina.aspx' AND TipoPortal = 'A'");
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

        #region Metodo BtnNomina_Click
        /* ****************************************************************************/
        /* Metodo del evento generado por el boton BtnNomina
        /* ****************************************************************************/
        protected void BtnNomina_Click(object sender, EventArgs e)
        {
            generarArchivoNomina();
        }
        #endregion

        #region Metodo generarArchivoNomina
        /* ****************************************************************************/
        /* Metodo que ejecuta el cargue de Nomina en la tabla ResumenNomina
        /* ****************************************************************************/
        public void generarArchivoNomina()
        {
            CnMysql Conexion = new CnMysql(Cn);

            try
            {
                string ruta2 = ruta + "resumennomina.txt";
                if ((File.Exists(ruta + "resumennomina.txt")) && (File.Exists(ruta + "conceptonominatemp.txt")))
                {
                    string cmdmysql; int Registros = 0;
                    Conexion.AbrirCnMysql();
                    //Trunca la tabla conceptonominatemp antes de insertar los datos
                    Conexion.EjecutarComando("truncate table " + bd1 + ".conceptonominatemp");
                    cmdmysql = "LOAD DATA LOCAL INFILE '" + ruta + "conceptonominatemp.txt" + "' INTO TABLE " + bd1 + ".conceptonominatemp fields terminated by '|' ESCAPED BY " + @"'\\'" + " enclosed by '' LINES TERMINATED BY " + "'\\r\\n';";
                    Registros = Conexion.EjecutarComandoCon(cmdmysql);

                    //Inserta los conceptonomina nuevos en la tabla conceptonomina
                    cmdmysql = "insert into basica_trabajador.conceptonomina select idConceptoNomina,Empresas_idempresa,Descripcion_Concepto from " + bd1 + ".conceptonominatemp T3 where not exists(select 1 from basica_trabajador.conceptonomina E where E.idConceptoNomina = T3.idConceptoNomina and E.Empresas_idEmpresa = T3.Empresas_idEmpresa)";
                    int RegistrosEmple = Conexion.EjecutarComandoCon(cmdmysql);

                    //Trunca la tabla resumennomina antes de insertar los datos
                    Conexion.EjecutarComando("truncate table " + bd2 + ".resumennomina");
                    cmdmysql = "LOAD DATA LOCAL INFILE '" + ruta + "resumennomina.txt" + "' INTO TABLE " + bd2 + ".resumennomina fields terminated by '|' ESCAPED BY " + @"'\\'" + " enclosed by '' LINES TERMINATED BY " + "'\\r\\n';";
                    Registros = Conexion.EjecutarComandoCon(cmdmysql);

                    MensajeError("Se Cargaron " + Registros + " Registros en la Base de Datos para el Cargue de Nomina");
                }
                else
                {
                    MensajeError("No Existe el Archivo ResumenNomina.txt o conceptonominatemp.txt en la Carpeta Transferencias");
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