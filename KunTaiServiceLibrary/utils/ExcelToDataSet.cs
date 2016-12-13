using Aspose.Cells;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace KunTaiServiceLibrary
{
    public class ExcelToDataSet
    {

        /// <summary>
        /// 加载Excel文件数据到DataSet中
        /// </summary>
        /// <param name="fileUrl">Excel文件</param>
        /// <param name="dataSetExcel">DataSet数据集</param>
        public static void loadExcelToDataSet(string fileUrl, out DataSet dataSetExcel)
        {
            dataSetExcel = null;

            if (string.IsNullOrEmpty(fileUrl) || !File.Exists(fileUrl))
            {
                throw new Exception("无法找到指定的文件，请重新选择。");
            }

            Workbook workBook = new Workbook();
            workBook.Open(fileUrl);

            if (workBook.Worksheets.Count <= 0)
            {
                throw new Exception("在文件中没有找到工作表。");
            }

            dataSetExcel = new DataSet();

            DataTable dataTable = null;
            Cells cells = null;
            //try
            //{
            for (int i = 0; i < workBook.Worksheets.Count; i++)
            {
                cells = workBook.Worksheets[i].Cells;

                if (cells.MaxDataRow > 0 && cells.MaxColumn > 0)
                    dataTable = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, false);

                if (dataTable != null)
                {
                    dataSetExcel.Tables.Add(dataTable.Copy());

                    cells = null;
                    dataTable = null;
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            workBook = null;
        }


        /// <summary>
        /// 读取Excel文件到DataSet中
        /// </summary>
        /// <param name="filePath">Excel文件的所在路径</param>
        /// <returns>返回Excel中对应DataSet的数据</returns>
        public static DataSet Load(string filePath)
        {
            string connStr = string.Empty;
            string fileType = System.IO.Path.GetExtension(filePath);
            if (string.IsNullOrEmpty(fileType)) return null;

            if (fileType == ".xls")
                connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
            else
                connStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
            string sql_F = "Select * FROM [{0}]";

            OleDbConnection connection = null;
            OleDbDataAdapter dataAdapte = null;
            DataTable dataTableSheetName = null;

            DataSet dataSet = new DataSet();
            try
            {
                // 初始化连接，并打开
                connection = new OleDbConnection(connStr);
                connection.Open();

                // 获取数据源的表定义元数据                        
                string SheetName = "";
                dataTableSheetName = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                // 初始化适配器
                dataAdapte = new OleDbDataAdapter();
                for (int i = 0; i < dataTableSheetName.Rows.Count; i++)
                {
                    SheetName = dataTableSheetName.Rows[i]["TABLE_NAME"].ToString();

                    if (SheetName.Contains("$") && !SheetName.Replace("'", "").EndsWith("$"))
                    {
                        continue;
                    }

                    dataAdapte.SelectCommand = new OleDbCommand(String.Format(sql_F, SheetName), connection);
                    DataSet dsItem = new DataSet();
                    dataAdapte.Fill(dsItem, SheetName.Replace("$", ""));

                    dataSet.Tables.Add(dsItem.Tables[0].Copy());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // 关闭连接
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    dataAdapte.Dispose();
                    connection.Dispose();
                }
            }

            return dataSet;
        }

    }
}
