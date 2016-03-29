using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace PortalTrabajadores.Portal
{
    public partial class CargueArchivos : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["trabajadoresConnectionString"].ConnectionString.ToString();
        string ruta = ConfigurationManager.AppSettings["RepositorioPDF"].ToString();
        string bd1 = ConfigurationManager.AppSettings["BD1"].ToString();
        string bd2 = ConfigurationManager.AppSettings["BD2"].ToString();
        string Combine = "";

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
                    ListArchivos.Visible = false;
                    DropListAño.Items.Clear();
                    DropListAño.Items.Add(DateTime.Now.Year.ToString());
                    DropListAño.Items.Add((DateTime.Now.Year - 1).ToString());

                    CnMysql Conexion = new CnMysql(Cn);
                    try
                    {
                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM " + bd1 + ".Options_Menu WHERE url = 'CargueArchivos.aspx' AND TipoPortal = 'A'", Conexion.ObtenerCnMysql());
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

        #region Metodo BtnCargue_Click
        /* ****************************************************************************/
        /* Evento que procede a realizar el cargue de los PDF
        /* ****************************************************************************/
        protected void BtnCargue_Click(object sender, EventArgs e)
        {
            int MesCargue = Convert.ToInt16(DropListMes.SelectedValue);
            int AñoCargue = Convert.ToInt16(DropListAño.SelectedValue);
            int TipoCargueValue = Convert.ToInt16(DropListTipoCargue.SelectedValue);

            string Origen = "";
            string repositorio = "";
            ListArchivos.Visible = false;

            switch (DropListTemp.SelectedValue.ToString())
            {
                case "ST":
                    repositorio = "SumitempPDF";
                    break;
                case "AE":
                    repositorio = "AliadosPDF";
                    break;
                case "SS":
                    repositorio = "SumiservisPDF";
                    break;
            }

            switch (TipoCargueValue)
            {
                //Cargue Cesantias
                case 1:

                    //Trunca la tabla a cargar
                    TruncarCargues();

                    //Ruta donde se encuentran los PDF a cargar
                    Origen = System.IO.Path.Combine(repositorio, "Cesantias");

                    //Carga Los Archivos                    
                    CargarArchivos(Origen, MesCargue, AñoCargue);
                    break;
                //Cargue Fondo
                case 2:
                    //Trunca la tabla a cargar
                    TruncarCargues();

                    //Ruta donde se encuentran los PDF a cargar
                    Origen = "FondoPDF";

                    //Carga Los Archivos
                    CargarArchivos(Origen, MesCargue, AñoCargue);
                    break;
                //Cargue Seguridad Social
                case 3:
                    //Trunca la tabla a cargar
                    TruncarCargues();

                    //Ruta donde se encuentran los PDF a cargar
                    Origen = System.IO.Path.Combine(repositorio, "SegSocial");

                    //Carga Los Archivos
                    CargarArchivos(Origen, MesCargue, AñoCargue);
                    break;
                //Cargue Retencion en la Fuente
                case 4:
                    //Trunca la tabla a cargar
                    TruncarCargues();

                    //Ruta donde se encuentran los PDF a cargar
                    Origen = System.IO.Path.Combine(repositorio, "IngRet");

                    //Carga Los Archivos
                    CargarArchivos(Origen, MesCargue, AñoCargue);
                    break;
                /* default:
                     Console.WriteLine("Default case");
                     break;*/
            }
        }
        #endregion

        #region Metodo eliminarArchivos
        /* ****************************************************************************/
        /* Metodo que elimina los archivos dada una Ruta
        /* ****************************************************************************/
        public void eliminarArchivos(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath(path));

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }
        }
        #endregion

        #region Metodo TruncarCargues
        /* **********************************************************************************************************/
        /* Metodo que realiza el truncamiento de los datos en la tabla de los cargues antes de realizar la insercion
        /* **********************************************************************************************************/
        public int TruncarCargues()
        {
            CnMysql Conexion = new CnMysql(Cn);
            int TipoCargueValue = Convert.ToInt16(DropListTipoCargue.SelectedValue);
            int res = 0;
            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd = new MySqlCommand(bd2 + ".sp_TruncarCargues", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;

                //Cuando el tipo de cargue es igual a 1 = Cesantias; 2 = Fondo; 3 = SeguridadSocial
                cmd.Parameters.AddWithValue("@tipoCargue", TipoCargueValue);
                cmd.Parameters.AddWithValue("@empresa", DropListTemp.SelectedValue.ToString());

                // Crea un parametro de salida para el SP
                MySqlParameter outputIdParam = new MySqlParameter("@respuesta", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdParam);
                cmd.ExecuteNonQuery();

                //Almacena la respuesta de la variable de retorno del SP
                res = int.Parse(outputIdParam.Value.ToString());

                return res;
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                return res;
            }
            finally
            {
                Conexion.CerrarCnMysql();
            }
        }
        #endregion

        #region Metodo InsertarCargue
        /* ****************************************************************************/
        /* Metodo que realiza la insercion del cargue de los PDF en la BD
        /* ****************************************************************************/
        public int InsertarCargue(string id_Empleado, string mes, string año, string nombreArchivo, string rutaArchivo, string temporal)
        {
            CnMysql Conexion = new CnMysql(Cn);
            int TipoCargueValue = Convert.ToInt16(DropListTipoCargue.SelectedValue);
            int res = 0;
            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd = new MySqlCommand(bd2 + ".sp_CarguesPDF", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idEmpleado", id_Empleado);
                cmd.Parameters.AddWithValue("@Mes", mes);
                cmd.Parameters.AddWithValue("@anio", año);
                cmd.Parameters.AddWithValue("@nombreArchivo", nombreArchivo);
                cmd.Parameters.AddWithValue("@rutaArchivo", rutaArchivo);
                cmd.Parameters.AddWithValue("@empresa", temporal);

                //Cuando el tipo de cargue es igual a 1 = Cesantias; 2 = Fondo; 3 = SeguridadSocial
                cmd.Parameters.AddWithValue("@tipoCargue", TipoCargueValue);

                // Crea un parametro de salida para el SP
                MySqlParameter outputIdParam = new MySqlParameter("@respuesta", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdParam);
                cmd.ExecuteNonQuery();

                //Almacena la respuesta de la variable de retorno del SP
                res = int.Parse(outputIdParam.Value.ToString());

                return res;
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                return res;
            }
            finally
            {
                Conexion.CerrarCnMysql();
            }
        }
        #endregion

        #region Metodo CargarArchivos
        /* ****************************************************************************/
        /* Metodo que realiza el cargue de los archivos PDF para cualquier tipo de cargue
        /* ****************************************************************************/
        public void CargarArchivos(string Origen, int MesCargue, int añoCargue)
        {
            string fileName, filenameSExt;
            Combine = System.IO.Path.Combine(ruta, Origen);

            //Array donde se almacenaran los nombres de los archivos que no se pudieron mover
            List<string> _items = new List<string>();

            try
            {
                string[] files = System.IO.Directory.GetFiles(Combine, "*.pdf");

                //Valida que haya archivos que cargar
                if (files.Length == 0)
                {
                    MensajeError("No hay Archivos para cargue en la Ruta definida: " + Origen);
                }
                else
                {
                    // Mueve los Archivos en el destino
                    foreach (string s in files)
                    {
                        // Extrae el nombre del archivo
                        fileName = System.IO.Path.GetFileName(s);
                        filenameSExt = System.IO.Path.GetFileNameWithoutExtension(s);

                        //Si el cargue no fue satisfactorio en la Base de datos almacena los datos en List
                        if (InsertarCargue(filenameSExt, MesCargue.ToString(), añoCargue.ToString(), fileName, Origen, DropListTemp.SelectedValue.ToString()) == 0)
                        {
                            //Guardar en list los archivos no cargados
                            _items.Add(fileName);
                        }
                    }
                    //Muestra en un listbox los archivos que no fueron cargados
                    if (_items.Count() > 0)
                    {
                        ListArchivos.Items.Clear();
                        for (int i = 0; i < _items.Count(); i++)
                        {
                            ListArchivos.Items.Add(_items.ElementAt(i));
                        }
                        ListArchivos.Visible = true;
                        MensajeError("Los archivos que aparecen listados no se cargaron puesto que los usuarios no se encuentran en la Base de Datos.");
                    }
                    else
                    {
                        MensajeError("Los archivos se cargaron correctamente.");
                    }
                }
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion

        #region Metodo DropListTipoCargue_SelectedIndexChanged
        /* ****************************************************************************/
        /* Metodo que cambia la habilitacion del boton de cargue al seleccionar la lista
        /* ****************************************************************************/
        protected void DropListTipoCargue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt16(DropListTipoCargue.SelectedValue) != 0 && Convert.ToInt16(DropListMes.SelectedValue) != 0)
            {
                BtnCargue.Enabled = true;
            }
            else
            {
                BtnCargue.Enabled = false;
            }

            if (Convert.ToInt16(DropListTipoCargue.SelectedValue) == 2)
            {
                DropListTemp.Enabled = false;
                DropListTemp.BackColor = System.Drawing.Color.LightGray;
            }
            else{
                DropListTemp.Enabled = true;
                DropListTemp.BackColor = System.Drawing.Color.White;
            }
        }
        #endregion

        #region Metodo DropListMes_SelectedIndexChanged
        /* ****************************************************************************/
        /* Metodo que cambia la habilitacion del boton de cargue al seleccionar la lista
        /* ****************************************************************************/
        protected void DropListMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt16(DropListTipoCargue.SelectedValue) != 0 && Convert.ToInt16(DropListMes.SelectedValue) != 0)
            {
                BtnCargue.Enabled = true;
            }
            else
            {
                BtnCargue.Enabled = false;
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