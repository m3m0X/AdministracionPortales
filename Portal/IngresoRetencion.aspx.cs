using Excel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PortalTrabajadores.Portal
{
    public partial class IngresoRetencion : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["trabajadoresConnectionString"].ConnectionString.ToString();
        string ruta = ConfigurationManager.AppSettings["RepositorioPDF"].ToString();


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
                    MySqlConnection MySqlCn = new MySqlConnection(Cn);
                    try
                    {
                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM basica_trabajador.Options_Menu WHERE url = 'IngresoRetencion.aspx' AND Tipoportal = 'A'", MySqlCn);
                        MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                        DataSet dsDataSet = new DataSet();
                        DataTable dtDataTable = null;
                        MySqlCn.Open();
                        sdaSqlDataAdapter.Fill(dsDataSet);
                        dtDataTable = dsDataSet.Tables[0];
                        if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                        {
                            this.lblTitulo.Text = dtDataTable.Rows[0].ItemArray[0].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        MensajeError("Ha ocurrido el siguiente error: " + ex.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }
                    finally
                    {
                        MySqlCn.Close();
                    }
                }
            }
        }

        #endregion

        #region Nuevo cargue

        protected void btnNuevoCargue_Click(object sender, EventArgs e)
        {
            MySqlConnection MySqlCn = new MySqlConnection(Cn);

            MySqlCn.Open();

            MySqlCommand deleteCommand = new MySqlCommand("DELETE from trabajadores.ingretenciones", MySqlCn);

            deleteCommand.ExecuteNonQuery();

            MySqlCn.Close();

            btnNuevoCargue.Enabled = false;

            MensajeError("Se eliminó correctamente la información.");
        }
        #endregion

        #region Subir información

        protected void btnSubirArchivo_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile && (Path.GetExtension(FileUpload1.FileName) == ".xlsx" || Path.GetExtension(FileUpload1.FileName) == ".xls"))
            {
                SaveFile(FileUpload1.PostedFile);

                string savePath = System.IO.Path.Combine(ruta, "Retefuente");

                //Valida la Existencia de la ruta Origen para poder descargar el PDF los Archivos
                string filename = System.IO.Path.Combine(savePath, FileUpload1.FileName);

                FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read);

                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                //IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                //...
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //...
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                //DataSet resultado = excelReader.AsDataSet();
                //...
                //4. DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet resultado = excelReader.AsDataSet();

                MySqlConnection MySqlCn = new MySqlConnection(Cn);
                int x = 0;

                //5. Data Reader methods
                while (x < resultado.Tables[0].Rows.Count)
                {                    
                    try
                    {
                        MySqlCn.Open();

                        MySqlCommand scSqlCommand = new MySqlCommand("INSERT INTO trabajadores.ingretenciones " +
                                                                     "VALUES (null,'" + resultado.Tables[0].Rows[x].ItemArray[0].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[1].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[2].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[3].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[4].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[5].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[6].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[7].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[8].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[9].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[10].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[11].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[12].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[13].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[14].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[15].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[16].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[17].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[18].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[19].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[20].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[21].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[22].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[23].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[24].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[25].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[26].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[27].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[28].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[29].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[30].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[31].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[32].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[33].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[34].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[35].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[36].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[37].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[38].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[39].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[40].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[41].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[42].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[43].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[44].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[45].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[46].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[47].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[48].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[49].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[50].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[51].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[52].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[53].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[54].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[55].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[56].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[57].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[58].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[59].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[60].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[61].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[62].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[63].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[64].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[65].ToString() + "','" +
                                                                                  resultado.Tables[0].Rows[x].ItemArray[66].ToString() + "','" +
                                                                                  EmpresaSeleccionada.SelectedValue + "')", MySqlCn);
                        scSqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MensajeError("Ha ocurrido el siguiente error: " + ex.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }
                    finally
                    {
                        MySqlCn.Close();
                    }

                    x++;
                }

                //6. Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();

                MensajeError("La información se cargó exitosamente");
            }
            else
            {
                MensajeError("No se espeficicó un archivo para cargar");
            }
        }
        #endregion

        #region Salvar archivo

        public void SaveFile(HttpPostedFile file)
        {
            // Specify the path to save the uploaded file to.
            string savePath = System.IO.Path.Combine(ruta, "Retefuente");

            //Valida la Existencia de la ruta Origen para poder descargar el PDF los Archivos
            string filename = System.IO.Path.Combine(savePath, FileUpload1.FileName);

            // Get the name of the file to upload.
            string fileName = FileUpload1.FileName;

            // Create the path and file name to check for duplicates.
            string pathToCheck = savePath + fileName;

            // Create a temporary file name to use for checking duplicates.
            string tempfileName = "";

            // Check to see if a file already exists with the
            // same name as the file to upload.        
            if (System.IO.File.Exists(pathToCheck))
            {
                int counter = 2;
                while (System.IO.File.Exists(pathToCheck))
                {
                    // if a file with this name already exists,
                    // prefix the filename with a number.
                    tempfileName = counter.ToString() + fileName;
                    pathToCheck = savePath + tempfileName;
                    counter++;
                }

                fileName = tempfileName;

                // Notify the user that the file name was changed.
                MensajeError("A file with the same name already exists. <br />Your file was saved as " + fileName);
            }
            else
            {
                // Notify the user that the file was saved successfully.
                MensajeError("Your file was uploaded successfully.");
            }

            // Append the name of the file to upload to the path.
            savePath += fileName;

            // Call the SaveAs method to save the uploaded
            // file to the specified directory.
            FileUpload1.SaveAs(filename);
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