using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace PortalTrabajadores.Portal
{
    public partial class ModificarProyecto : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["trabajadoresConnectionString"].ConnectionString.ToString();
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
                        txtuser.Focus();

                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM " + bd1 + ".Options_Menu WHERE url = 'ModificarProyecto.aspx' and Tipoportal = 'A'", Conexion.ObtenerCnMysql());
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
                        MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
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
        public void MensajeError(string Msj)
        {
            LblMsj.Text = Msj;
            LblMsj.Visible = true;
            UpdatePanel3.Update();
        }
        #endregion

        #region Metodo BtnBuscar_Click
        /* ****************************************************************************/
        /* Metodo que consulta la información de la Compania a actualizar
        /* ****************************************************************************/
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            //Se valida que se haya digitado un usuario
            if (txtuser.Text.Trim() == "")
            {
                MensajeError("Digite un Código de Proyecto.");
            }
            else
            {
                if (BtnBuscar.Text == "Nueva Búsqueda")
                {
                    txtuser.Text = "";
                    DropListTemp.Enabled = true;
                    txtuser.Enabled = true;
                    BtnBuscar.Text = "Buscar Proyecto";
                    MensajeError("");
                    Container_UpdatePanel2.Visible = false;
                    txtuser.Focus();

                    TxtCodigoProy.Text = "";
                    TxtCodTemporal.Text = "";
                    TxtCorreo.Text = "";
                    TxtDesc2Proyecto.Text = "";
                    TxtDir.Text = "";
                    TxtNitTercero.Text = "";
                    TxtNomProyecto.Text = "";
                    TxtTelefono.Text = "";
                   
                    BtnEditar.Enabled = true;
                    BtnEditar.BackColor = default(System.Drawing.Color);  

                    TxtCorreo.Enabled = true;
                    TxtCorreo.BackColor = System.Drawing.Color.White;
                    TxtDesc2Proyecto.Enabled = true;
                    TxtDesc2Proyecto.BackColor = System.Drawing.Color.White;
                    TxtDir.Enabled = true;
                    TxtDir.BackColor = System.Drawing.Color.White;
                    TxtNitTercero.Enabled = true;
                    TxtNitTercero.BackColor = System.Drawing.Color.White;
                    TxtTelefono.Enabled = true;
                    TxtTelefono.BackColor = System.Drawing.Color.White;

                    DropListTemp.Enabled = true;
                    DropListTemp.BackColor = System.Drawing.Color.White;

                    DropActivoProy.Enabled = true;
                    DropActivoProy.BackColor = System.Drawing.Color.White;
                    DropMarcacionProy.Enabled = true;
                    DropMarcacionProy.BackColor = System.Drawing.Color.White;
                }
                else
                {
                    BtnBuscar.Text = "Nueva Búsqueda";
                    txtuser.Enabled = false;
                    DropListTemp.Enabled = false;
                    DropListTemp.BackColor = System.Drawing.Color.LightGray;
                    MensajeError("");
                    CargarInfoCompania(txtuser.Text, DropListTemp.Text);
                }
            }
        }
        #endregion

        #region Metodo CargarInfoCompania
        /* ****************************************************************************/
        /* Metodo que carga la informacion de la compañia en el formulario
        /* ****************************************************************************/
        public void CargarInfoCompania(string CodCompania, string empresa)
        {
            CnMysql Conexion = new CnMysql(Cn);

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd = new MySqlCommand(bd2 + ".sp_ConsultaCompanias", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCompania", CodCompania);
                cmd.Parameters.AddWithValue("@empresa", empresa);
                MySqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    TxtCodigoProy.Text = rd["idCompania"].ToString();
                    TxtCodTemporal.Text = rd["Empresas_idEmpresa"].ToString();
                    TxtNomProyecto.Text = rd["Descripcion_compania"].ToString();
                    TxtDesc2Proyecto.Text = rd["Descripcion_Compania2"].ToString();
                    this.DropMarcacionProy.SelectedValue = rd["Marcacion_Descripcion"].ToString();
                    this.DropActivoProy.SelectedValue = rd["Activo_Compania"].ToString();
                    TxtNitTercero.Text = rd["Terceros_Nit_Tercero"].ToString();
                    TxtDir.Text = rd["Direccion_Compania"].ToString();
                    TxtTelefono.Text = rd["Telefono_Compania"].ToString();
                    TxtCorreo.Text = rd["Correo_Compania"].ToString();

                    Container_UpdatePanel2.Visible = true;
                    BtnEditar.Text = "Guardar Información";
                }
                else
                {
                    MensajeError("No se encontro el Código asociado al proyecto, por favor verifique. ");
                }
                rd.Close();
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

        #region Metodo BtnEditar_Click
        /* ********************************************************************************************************/
        /* Evento que se produce al dar clic sobre el boton BtnEditar para almacenar la informacion de la compania
        /* ********************************************************************************************************/
        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            Editar();
        }
        #endregion

        #region Metodo Editar
        /* ****************************************************************************/
        /* Metodo que Bloquea los campos y actualiza el tercero al Dar Clic en el Boton Almacenar Informacion
        /* ****************************************************************************/
        protected void Editar()
        {
            int R = ActualizaInfoCompania(TxtCodigoProy.Text, TxtTelefono.Text, TxtNomProyecto.Text.ToUpper(), TxtNitTercero.Text, TxtDir.Text.ToUpper(), TxtDesc2Proyecto.Text.ToUpper(), TxtCorreo.Text, TxtCodTemporal.Text, DropActivoProy.SelectedValue.ToString(), DropMarcacionProy.SelectedValue.ToString());
            if (R == 1)
            {
                MensajeError("La información se ha actualizado correctamente");


                TxtCorreo.Enabled = false;
                TxtCorreo.BackColor = System.Drawing.Color.LightGray;
                TxtDesc2Proyecto.Enabled = false;
                TxtDesc2Proyecto.BackColor = System.Drawing.Color.LightGray;
                TxtDir.Enabled = false;
                TxtDir.BackColor = System.Drawing.Color.LightGray;
                TxtNitTercero.Enabled = false;
                TxtNitTercero.BackColor = System.Drawing.Color.LightGray;
                TxtTelefono.Enabled = false;
                TxtTelefono.BackColor = System.Drawing.Color.LightGray;

                DropActivoProy.Enabled = false;
                DropActivoProy.BackColor = System.Drawing.Color.LightGray;
                DropMarcacionProy.Enabled = false;
                DropMarcacionProy.BackColor = System.Drawing.Color.LightGray;

                BtnEditar.Enabled = false;
                BtnEditar.BackColor = System.Drawing.Color.LightGray;
            }
            else
            {
                MensajeError("La información no se ha actualizado correctamente. Pongase en contacto con el Administrador.");
            }
        }
        #endregion

        #region Metodo ActualizaInfoCompania
        /* ****************************************************************************/
        /* Metodo que actualiza la informacion de la compania en la Base de datos
        /* ****************************************************************************/
        public int ActualizaInfoCompania(string idcompania,string telefono,string descripcion1, string nitTercero,string direccion,string descripcion2,string correo, string idempresa, string activoProyecto, string marcacionProyecto)
        {
            CnMysql Conexion = new CnMysql(Cn);
            int res = 0;

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd = new MySqlCommand(bd2 + ".sp_ActualizaCompania", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCompania", idcompania);
                cmd.Parameters.AddWithValue("@idEmpresa", idempresa);
                cmd.Parameters.AddWithValue("@Descripcion1", descripcion1.ToUpper());
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@Descripcion2", descripcion2.ToUpper());
                cmd.Parameters.AddWithValue("@telefono", telefono);
                cmd.Parameters.AddWithValue("@direccion", direccion.ToUpper());
                cmd.Parameters.AddWithValue("@NitTercero", nitTercero);

                cmd.Parameters.AddWithValue("@MarcacionDescripcion", SqlDbType.Bit).Value = marcacionProyecto;
                cmd.Parameters.AddWithValue("@Activocompania", SqlDbType.Bit).Value = activoProyecto;

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

        #endregion
    }
}