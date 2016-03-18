using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using IndiansInc;
using System.IO;

namespace PortalTrabajadores.Portal
{
    public partial class CargueEmpleados : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["trabajadoresConnectionString"].ConnectionString.ToString();
        string ruta = ConfigurationManager.AppSettings["RutaFisica"].ToString();

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
                        dtDataTable = Conexion.ConsultarRegistros("SELECT descripcion FROM basica_trabajador.Options_Menu WHERE url = 'CargueEmpleados.aspx' AND TipoPortal = 'A'");
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

        #region Metodo generarArchivoEmpleados
        /* ****************************************************************************/
        /* Metodo que ejecuta la actualizacion en la tabla empleados y companias
        /* ****************************************************************************/
        public void generarArchivoEmpleados()
        {
            CnMysql Conexion = new CnMysql(Cn);
            try
            {
                if ((File.Exists(ruta + "companiastemp.txt")) && (File.Exists(ruta + "empleadostemp.txt")))
                {
                    string cmdmysql; int Registros = 0;
                    Conexion.AbrirCnMysql();
                    //Trunca la tabla Companiastemp antes de insertar los datos
                    Conexion.EjecutarComando("truncate table trabajadores.companiastemp");
                    cmdmysql = "LOAD DATA LOCAL INFILE '" + ruta + "companiastemp.txt" + "' INTO TABLE trabajadores.companiastemp fields terminated by '|' ESCAPED BY " + @"'\\'" + " enclosed by '' LINES TERMINATED BY " + "'\\r\\n';";
                    Registros = Conexion.EjecutarComandoCon(cmdmysql);

                    //Trunca la tabla empleadostemp antes de insertar los datos
                    Conexion.EjecutarComando("truncate table trabajadores.empleadostemp");
                    cmdmysql = "LOAD DATA LOCAL INFILE '" + ruta + "empleadostemp.txt" + "' INTO TABLE trabajadores.empleadostemp fields terminated by '|' ESCAPED BY " + @"'\\'" + " enclosed by '' LINES TERMINATED BY " + "'\\r\\n';";
                    Registros = Conexion.EjecutarComandoCon(cmdmysql);

                    //Inserta las compañias nuevas en la tabla companias
                    cmdmysql = "insert into companias select * from trabajadores.companiastemp T where not exists(select 1 from trabajadores.companias C where C.idcompania = T.idcompania and C.Empresas_idEmpresa = T.Empresas_idEmpresa);";
                    Registros = Conexion.EjecutarComandoCon(cmdmysql);

                    //Inserta los empleados nuevos en la tabla empleados
                    cmdmysql = "insert into trabajadores.empleados select * from trabajadores.empleadostemp T3 where not exists(select 1 from trabajadores.empleados E where E.id_empleado = T3.Id_Empleado and E.idContrato = T3.idContrato)";
                    int RegistrosEmple = Conexion.EjecutarComandoCon(cmdmysql);

                    //Actualiza la informacion de los empleados 
                    cmdmysql = "update trabajadores.empleados E, trabajadores.empleadostemp T Set E.Activo_Empleado = T.Estado_Contrato_Empleado, E.Outsourcing = T.Outsourcing, E.Companias_idCompania = T.Companias_idCompania, E.Estado_Contrato_Empleado = T.Estado_Contrato_Empleado,E.Fecha_terminacion_Empleado = T.Fecha_terminacion_Empleado,E.salario_empleado = T.salario_empleado, E.Nombre_Cargo_Empleado = T.Nombre_Cargo_Empleado where T.Id_Empleado = E.Id_Empleado and T.idContrato = E.idContrato and T.TipoId_empleado = E.TipoId_empleado and T.Companias_idEmpresa =  E.Companias_idEmpresa;";
                    int RegistrosAct = Conexion.EjecutarComandoCon(cmdmysql);

                    MensajeError("Se insertaron " + RegistrosEmple + " Empleados Nuevos en la Base de Datos para el Cargue de Empleados. Actualizados: " + RegistrosAct);
                }
                else
                {
                    MensajeError("No Existe el Archivo companiastemp.txt o empleadostemp.txt en la Carpeta Transferencias");
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

        #region Metodo BtnEmpleados_Click
        /* ****************************************************************************/
        /* Metodo del evento generado por el boton BtnEmpleados
        /* ****************************************************************************/
        protected void BtnEmpleados_Click(object sender, EventArgs e)
        {
            generarArchivoEmpleados();
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