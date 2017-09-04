using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegExTest
{
    public class ErrorsService
    {
        public List<Error> GetErrors(string errorsFilePath)
        {
            List<Error> errorsList = new List<Error>();
            try
            {
                //string filepath = String.Empty;
                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(errorsFilePath))
                {

                }


                string connectionString = string.Format(@"provider=Microsoft.Ace.OLEDB.12.0;data source={0};Extended Properties=Excel 12.0;", errorsFilePath);
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                using (DataSet myDataset = new DataSet())
                {
                    //Read From EXCEL and Create DataSet
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        string tablename = "";
                        DataRow dr = dt.Rows[i];
                        tablename = dr["TABLE_NAME"].ToString().Trim();
                        if (tablename != null)
                        {
                            DataTable librariesDt = new DataTable() { TableName = tablename };
                            if (!librariesDt.TableName.ToString().Contains("_xlnm#_FilterDatabase"))
                            {
                                myDataset.Tables.Add(librariesDt);
                                OleDbDataAdapter objAdp = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", tablename), objConn);
                                objAdp.Fill(myDataset.Tables[tablename]);
                                objAdp.Dispose();
                            }
                        }
                    }                    
                    foreach (DataRow itemRow in myDataset.Tables[0].Rows)
                    {
                        Error error = new Error();                        
                        error.ErrorType = GetErrorType(itemRow["ErrorType"]);
                        error.OriginalErrorMessage = Convert.IsDBNull(itemRow["ErrorMessage"]) ? string.Empty : Convert.ToString(itemRow["ErrorMessage"]);
                        error.ErrorMessage = Convert.IsDBNull(itemRow["ErrorMessage"]) ? string.Empty : Convert.ToString(itemRow["ErrorMessage"]);
                        error.ErrorMessage = (error.ErrorMessage).Replace("\"", "");
                        error.ErrorCount = Convert.IsDBNull(itemRow["Count"]) ? 0 : Convert.ToInt32(itemRow["Count"]);
                        error.ErrorIndex = Convert.IsDBNull(itemRow["ErrorIndex"]) ? 0 : Convert.ToInt32(itemRow["ErrorIndex"]);
                        errorsList.Add(error);
                    }

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return errorsList;
        }

        /// <summary>
        /// Converts Simulation error code to SimulationErrorType enum flag value  - 
        /// Type of error or warning 0=Warning, 1=Severe, 2=Fatal.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private ErrorType GetErrorType(object errorTypeValue)
        {
            int errorCode = Convert.IsDBNull(errorTypeValue) ? 0 : Convert.ToInt32(errorTypeValue);

            if (errorCode == 0)//warning
                errorCode = 1;

            else if (errorCode == 1)//severe
                errorCode = 4;

            else if (errorCode == 2)//fatal
                errorCode = 2;
            return (ErrorType)errorCode;
        }
    }
}
