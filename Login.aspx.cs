﻿using MySql.Data.MySqlClient;
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
    public partial class login : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["trabajadoresConnectionString"].ConnectionString.ToString();
        string bd2 = ConfigurationManager.AppSettings["BD2"].ToString();

        #region Definicion de los Metodos de la Clase

        #region Metodo Page Load
        /* ****************************************************************************/
        /* Metodo que se ejecuta al momento de la carga de la Pagina
        /* ****************************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            txtuser.Focus();
        }
        #endregion

        #region Metodo btnlogin_Click
        /* ****************************************************************************/
        /* Evento del Boton para realizar la autenticacion al Portal de trabajadores
        /* ****************************************************************************/
        protected void btnlogin_Click(object sender, EventArgs e)
        {
            Page.Validate();
            CnMysql Conexion = new CnMysql(Cn);
            try
            {
                MySqlCommand scSqlCommand = new MySqlCommand("SELECT Id_Empleado, Id_Rol, Nombres_Empleado FROM " + bd2 + ".empleados where Id_Empleado = '" + this.txtuser.Text + "' and Contrasena_Empleado = '" + this.txtPass.Text + "' and  Id_Rol = 4;", Conexion.ObtenerCnMysql());
                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;
                //Asegura que los controles de la pagina hayan sido validados
                if (Page.IsValid)
                {
                    Conexion.AbrirCnMysql();
                    sdaSqlDataAdapter.Fill(dsDataSet);
                    dtDataTable = dsDataSet.Tables[0];

                    if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                    {
                        //Creo las Variables de Sesion de la Pagina
                        Session.Add("usuario", txtuser.Text);
                        Session.Add("rol", dtDataTable.Rows[0].ItemArray[1].ToString());
                        Session.Add("nombre", dtDataTable.Rows[0].ItemArray[2].ToString());

                        //redirecciona al usuario a la pagina principal del Portal
                        Response.Redirect("~/Portal/index.aspx");
                    }
                    else
                    {
                        MensajeError("Usuario y/o contraseña equivocados.");
                    }
                }
            }
            catch 
            {
                MensajeError("El sistema no se encuentra disponible en este momento. Intente más tarde.");
            }
            finally
            {
                Conexion.CerrarCnMysql();
            }
        }
        #endregion

        #region Metodo btnPass_Click
        /* ****************************************************************************/
        /* Evento del boton que redirecciona a la pagina de recuperar contraseña
        /* ****************************************************************************/
        protected void btnPass_Click(object sender, EventArgs e)
        {
            //redirecciona al usuario a la pagina principal de la encuesta si no la ha contestado
            Response.Redirect("~/RecuperarContrasena.aspx");
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
        }
        #endregion

        #endregion

    }
}