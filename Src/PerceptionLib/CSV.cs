using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.IO;


namespace PerceptionLib
{
    /// <summary>
    /// this class gets the data in terms to and from CSV in terms of Data Table 
    /// </summary>
    public class CSV
    {
        public static DataTable GetDataTableFromCSV(string strFileName)
        {
            try
            {
                System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + System.IO.Path.GetDirectoryName(strFileName) + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\"");
                conn.Open();
                string strQuery = "Select * from [" + System.IO.Path.GetFileName(strFileName) + "]";
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(strQuery, conn);
                System.Data.DataSet ds = new System.Data.DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex) { }
            return new DataTable();
        }

        public static void ToCSVFromDataTable(DataTable dtPassed)
        {

            StreamWriter sw = new StreamWriter(@"C:\see-through-project\gt\STColorCorrection\Src\PerceptionLib\bin\GridData.csv");
             //First we will write the headers.
            DataTable dt = dtPassed;

            int iColCount = dt.Columns.Count;
            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dt.Columns[i]);
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
             //Now write all the rows.
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                        sw.Write(",");
                    }
                  
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }

    }
}

