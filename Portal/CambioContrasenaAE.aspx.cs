﻿using System;
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
    public partial class CambioContrasenaAE : System.Web.UI.Page
    {
        string CnMysql = ConfigurationManager.ConnectionStrings["trabajadoresConnectionString"].ConnectionString.ToString();
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
                    CnMysql MysqlCn = new CnMysql(CnMysql);

                    try
                    {
                        DataTable dtDataTable = null;
                        MysqlCn.AbrirCnMysql();
                        dtDataTable = MysqlCn.ConsultarRegistros("SELECT descripcion FROM " + bd1 + ".Options_Menu WHERE url = 'CambioContrasenaAE.aspx' AND Tipoportal = 'A'");
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
                        MysqlCn.CerrarCnMysql();
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

        #region Metodo BtnCambiar_Click
        /* ****************************************************************************/
        /* Evento que procede a realizar el cambio de contraseña
        /* ****************************************************************************/
        protected void BtnCambiar_Click(object sender, EventArgs e)
        {
            CnMysql MysqlCn = new CnMysql(CnMysql);
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
                    MysqlCn.AbrirCnMysql();
                    MySqlCommand cmd = new MySqlCommand(bd2 + ".sp_CambioContrasena", MysqlCn.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cedula", txtuser.Text);
                    cmd.Parameters.AddWithValue("@empresa", "AE");
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
                MysqlCn.CerrarCnMysql();     
            }
        }
        #endregion

        #endregion
    }
}