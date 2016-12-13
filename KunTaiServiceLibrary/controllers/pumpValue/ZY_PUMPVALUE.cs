using Aspose.Cells;
using KunTaiServiceLibrary.valueObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace KunTaiServiceLibrary
{
    [FluorineFx.RemotingService("山东招远金城热力公司供回水温度服务")]
    public class ZY_PumpValue : IController
    {

        #region command texts

        private const string zy_pmupValue_select_commandText = @"SELECT T.*,
ROUND((CASE WHEN T.SUPPLYWATHERTEMP>0 AND T.BACKWATHERTEMP>0 THEN T.SUPPLYWATHERTEMP-T.BACKWATHERTEMP ELSE 0 END),3) AS CALC
FROM(SELECT A.[STCD],B.[T_CODE],A.[NAME],B.[ST_NM],B.[TYPE],B.[AREA],B.[POWER],B.[EFFICIENCY],B.[FLOW],B.[SPEED],B.[FREQUENCY],
(CASE B.TYPE WHEN '二级' THEN V.[SUPPLYWATHERTEMP2] WHEN '三级' THEN [SUPPLYWATHERTEMP3] END) AS SUPPLYWATHERTEMP,
(CASE B.TYPE WHEN '二级' THEN V.[BACKWATHERTEMP2] WHEN '三级' THEN [BACKWATHERTEMP3] END) AS BACKWATHERTEMP
FROM [ZY_WR_STAT_A] A,[ZY_WR_STAT_B] B,(SELECT [T_CODE],
ROUND(ISNULL(AVG([SUPPLYWATHERTEMP2]),0),3) AS [SUPPLYWATHERTEMP2],
ROUND(ISNULL(AVG([BACKWATHERTEMP2]),0),3) AS [BACKWATHERTEMP2],
ROUND(ISNULL(AVG([SUPPLYWATHERTEMP3]),0),3) AS [SUPPLYWATHERTEMP3],
ROUND(ISNULL(AVG([BACKWATHERTEMP3]),0),3) AS [BACKWATHERTEMP3]
FROM [ZY_PUMPVALUE]
WHERE ([ADDDATETIME] BETWEEN '{0} 00:00:00' AND '{0} 23:59:59')
GROUP BY [T_CODE]) AS V
WHERE A.[STCD]=B.[STCD] AND B.[T_CODE]=V.[T_CODE] AND B.[AREA] IS NOT NULL{1}) T WHERE [AREA]>0
ORDER BY [STCD],[T_CODE]";


        private const string zy_pmupValue_export_commandText = @"SELECT T.[NAME] AS 站名称,T.[ST_NM] AS 泵房名称, T.[TYPE] AS 类型, T.[AREA] AS 面积, 
T.[POWER] AS 功率, T.[EFFICIENCY] AS 效率,T.[FLOW] AS 流量, T.[SPEED] AS 流速, T.[FREQUENCY] AS 频率, T.[SUPPLYWATHERTEMP] AS 供水温度, T.[BACKWATHERTEMP] AS 回水温度,
ROUND((CASE WHEN T.SUPPLYWATHERTEMP>0 AND T.BACKWATHERTEMP>0 THEN T.SUPPLYWATHERTEMP-T.BACKWATHERTEMP ELSE 0 END),3) AS 差
FROM(SELECT A.[STCD],B.[T_CODE],A.[NAME],B.[ST_NM],B.[TYPE],B.[AREA],B.[POWER],B.[EFFICIENCY],B.[FLOW],B.[SPEED],B.[FREQUENCY],
(CASE B.TYPE WHEN '二级' THEN V.[SUPPLYWATHERTEMP2] WHEN '三级' THEN [SUPPLYWATHERTEMP3] END) AS SUPPLYWATHERTEMP,
(CASE B.TYPE WHEN '二级' THEN V.[BACKWATHERTEMP2] WHEN '三级' THEN [BACKWATHERTEMP3] END) AS BACKWATHERTEMP
FROM [ZY_WR_STAT_A] A,[ZY_WR_STAT_B] B,(SELECT [T_CODE],
ROUND(ISNULL(AVG([SUPPLYWATHERTEMP2]),0),3) AS [SUPPLYWATHERTEMP2],
ROUND(ISNULL(AVG([BACKWATHERTEMP2]),0),3) AS [BACKWATHERTEMP2],
ROUND(ISNULL(AVG([SUPPLYWATHERTEMP3]),0),3) AS [SUPPLYWATHERTEMP3],
ROUND(ISNULL(AVG([BACKWATHERTEMP3]),0),3) AS [BACKWATHERTEMP3]
FROM [ZY_PUMPVALUE]
WHERE ([ADDDATETIME] BETWEEN '{0} 00:00:00' AND '{0} 23:59:59')
GROUP BY [T_CODE]) AS V
WHERE A.[STCD]=B.[STCD] AND B.[T_CODE]=V.[T_CODE] AND B.[AREA] IS NOT NULL{1}) T WHERE [AREA]>0
ORDER BY [STCD],[T_CODE]";
        // AND [TYPE]='二级'

        #endregion


        public string addDataItem(string text)
        {
            throw new NotImplementedException();
        }

        public string deleteDataItem(string text)
        {
            throw new NotImplementedException();
        }

        public string editDataItem(string text)
        {
            throw new NotImplementedException();
        }

        public string getDataItem(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Result.getFaultXml(Error.XML_IS_NULL);

            XElement xml = null;
            try
            {
                xml = XElement.Parse(text);
            }
            catch
            {
                return Result.getFaultXml(Error.XML_FORMAT_ERROR);
            }

            string dateTime = xml.Element("DATETIME").Value;
            string name = xml.Element("NAME").Value;
            if (string.IsNullOrEmpty(dateTime))
            {
                dateTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            }
            if (!string.IsNullOrEmpty(name))
            {
                name = string.Format(" AND ({0})", name);
            }
            string commandText = string.Format(zy_pmupValue_select_commandText,
                dateTime, name);

            DataSet dataSetPumpValue = null;
            try
            {
                dataSetPumpValue = new DataAccessHandler().executeDatasetResult(
                    commandText, null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetPumpValue != null && dataSetPumpValue.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemXml(ref dataSetPumpValue));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = dateTime = name = commandText = null;
            xml = null;
            dataSetPumpValue = null;


            return result;
        }

        private string getDataItemXml(ref DataSet dataSetPumpValue)
        {
            //返回所有的数据
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            ZY_PUMPVALUEObject zy_pumpvalueObject = null;
            int num = 1;

            List<string> stationNameList = new List<string>();
            foreach (DataRow row in dataSetPumpValue.Tables[0].Rows)
            {
                zy_pumpvalueObject = new ZY_PUMPVALUEObject(row);

                xml.AppendFormat("<DATA NUM=\"{0}\" STCD=\"{1}\" T_CODE=\"{2}\" NAME=\"{3}\" ST_NM=\"{4}\" TYPE=\"{5}\" AREA=\"{6}\" POWER=\"{7}\" EFFICIENCY=\"{8}\" FLOW=\"{9}\" SPEED=\"{10}\" FREQUENCY=\"{11}\" SUPPLYWATHERTEMP=\"{12}\" BACKWATHERTEMP=\"{13}\" CALC=\"{14}\"/>",
                    num,
                    zy_pumpvalueObject.STCD,
                    zy_pumpvalueObject.T_CODE,
                    stationNameList.Contains(zy_pumpvalueObject.NAME) ? string.Empty : zy_pumpvalueObject.NAME,
                    zy_pumpvalueObject.ST_NM,
                    zy_pumpvalueObject.TYPE,
                    zy_pumpvalueObject.AREA,
                    zy_pumpvalueObject.POWER,
                    zy_pumpvalueObject.EFFICIENCY,
                    zy_pumpvalueObject.FLOW,
                    zy_pumpvalueObject.SPEED,
                    zy_pumpvalueObject.FREQUENCY,
                    zy_pumpvalueObject.SUPPLYWATHERTEMP,
                    zy_pumpvalueObject.BACKWATHERTEMP,
                    zy_pumpvalueObject.CALC
                );

                if (!stationNameList.Contains(zy_pumpvalueObject.NAME))
                    stationNameList.Add(zy_pumpvalueObject.NAME);

                num++;
                zy_pumpvalueObject = null;
            }
            xml.Append("</DATAS>");

            stationNameList.Clear();
            stationNameList = null;

            return xml.ToString();
        }

        public string getDataItemDetails(string text)
        {
            throw new NotImplementedException();
        }


        public string getDateTimeText()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            xml.AppendFormat("<DATA DATETIME=\"{0}\"/>", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            xml.Append("</DATAS>");

            return Result.getResultXml(xml.ToString());
        }



        //导出文件
        public string exportFile(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Result.getFaultXml(Error.XML_IS_NULL);

            XElement xml = null;
            try
            {
                xml = XElement.Parse(text);
            }
            catch
            {
                return Result.getFaultXml(Error.XML_FORMAT_ERROR);
            }

            string dateTime = xml.Element("DATETIME").Value;
            string name = xml.Element("NAME").Value;
            if (string.IsNullOrEmpty(dateTime))
            {
                dateTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            }
            if (!string.IsNullOrEmpty(name))
            {
                name = string.Format(" AND ({0})", name);
            }
            string commandText = string.Format(zy_pmupValue_export_commandText,
                dateTime, name);

            DataSet dataSetPumpValue = null;
            try
            {
                dataSetPumpValue = new DataAccessHandler().executeDatasetResult(
                    commandText, null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetPumpValue != null && dataSetPumpValue.Tables.Count > 0)
            {
                result = Result.getResultXml(exportFileHandler(ref dataSetPumpValue, ref dateTime));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = dateTime = name = commandText = null;
            xml = null;
            dataSetPumpValue = null;


            return result;
        }

        private string exportFileHandler(ref DataSet dataSetPumpValue, ref string dateTime)
        {
            DataTable dataTable = dataSetPumpValue.Tables[0];

            //注册授权文件
            Aspose.Cells.License license = null;
            try
            {
                license = new Aspose.Cells.License();
                license.SetLicense(Config.ExportLicenseUrl);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            //初始化组件变量
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();//工作簿
            Aspose.Cells.Worksheet sheet = workbook.Worksheets[0];//工作表

            string fileName = string.Format("金城热力供回水温度{0}", dateTime);

            sheet.Name = "金城热力供回水温度";//表名

            int colCount = dataTable.Columns.Count;
            int rowCount = dataTable.Rows.Count;

            int rowHeight = 15;//行高

            string cellFontName = "宋体";//字体
            int cellFontSize = 9;//字体大小


            //定义带边界线的样式,供列头和表格使用
            //定义带边界线的样式,供列头和表格使用
            Style cellStyle = workbook.Styles[workbook.Styles.Add()];//新增样式 
            cellStyle.Font.Name = cellFontName;
            cellStyle.Font.Size = cellFontSize;
            cellStyle.IsTextWrapped = true;//单元格内容自动换行
            cellStyle.HorizontalAlignment = TextAlignmentType.Center;
            cellStyle.VerticalAlignment = TextAlignmentType.Center;
            cellStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线 
            cellStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线 
            cellStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线 
            cellStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线 

            //定义列头
            int rowIndex = 0;
            int colIndex = 0;

            sheet.Cells[rowIndex, colIndex].PutValue("序号");
            sheet.Cells[rowIndex, colIndex].Style.Font.IsBold = true;
            sheet.Cells.SetRowHeight(rowIndex, rowHeight);
            sheet.Cells[rowIndex, colIndex].SetStyle(cellStyle);

            colIndex = 1;
            //列名的处理
            for (int i = 0; i < colCount; i++)
            {
                sheet.Cells[rowIndex, colIndex].PutValue(dataTable.Columns[i].ColumnName);
                sheet.Cells[rowIndex, colIndex].Style.Font.IsBold = true;
                sheet.Cells.SetRowHeight(rowIndex, rowHeight);
                sheet.Cells[rowIndex, colIndex].SetStyle(cellStyle);
                colIndex++;
            }

            //行的处理
            Aspose.Cells.Style style = workbook.Styles[workbook.Styles.Add()];
            Aspose.Cells.StyleFlag styleFlag = new Aspose.Cells.StyleFlag();
            sheet.Cells.ApplyStyle(style, styleFlag);

            rowIndex++;
            int NUM = 1;
            for (int i = 0; i < rowCount; i++)
            {
                colIndex = 0;
                sheet.Cells[rowIndex, colIndex].PutValue(NUM);
                sheet.Cells.SetRowHeight(rowIndex, rowHeight);
                sheet.Cells[rowIndex, colIndex].SetStyle(cellStyle);

                colIndex++;
                for (int j = 0; j < colCount; j++)
                {
                    sheet.Cells[rowIndex, colIndex].PutValue(dataTable.Rows[i][j]);
                    sheet.Cells.SetRowHeight(rowIndex, rowHeight);
                    sheet.Cells[rowIndex, colIndex].SetStyle(cellStyle);
                    colIndex++;
                }
                rowIndex++;
                NUM++;
            }
            sheet.AutoFitColumns();


            //指定列宽为自适应
            sheet.AutoFitColumns();
            //指定行高为自适应
            //sheet.AutoFitRows();

            //sheet.FreezePanes("A1", 1, colCount + 1);

            string result = string.Empty;

            //保存到硬盘上
            string GUID = Guid.NewGuid().ToString().ToUpper();
            try
            {
                workbook.Save(Path.Combine(Config.UploadExportFileDirectory, string.Format("{0}.xls", GUID)));

                result = Result.getResultXml(exportExcelFileXml(ref fileName, ref GUID));
            }
            catch (Exception ex)
            {
                result = Result.getFaultXml(ex.Message);
            }


            //
            //删除方法内的变量
            dataTable = null;
            colCount = rowCount = 0;
            license = null;

            sheet = null;
            workbook = null;

            cellFontName = null;
            cellFontSize = 0;
            fileName = null;

            cellStyle = null;

            style = null;
            styleFlag = null;
            GUID = null;

            return result;
        }

        private string exportExcelFileXml(ref string fileName, ref string GUID)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            xml.AppendFormat("<DATA EXCELFILENAME=\"{0}\" EXCELURL=\"{1}\"/>",
                string.Format("{0}.xls", fileName),
                string.Format("{0}/{1}.xls", Config.UploadExportFileHttpUrl, GUID));
            xml.Append("</DATAS>");

            return xml.ToString();
        }



        //删除文件
        public string deleteExportFile(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Result.getFaultXml(Error.XML_IS_NULL);

            XElement xml = null;
            try
            {
                xml = XElement.Parse(text);
            }
            catch
            {
                return Result.getFaultXml(Error.XML_FORMAT_ERROR);
            }

            string fileUrl = xml.Element("FILEURL").Value;

            string[] tempFileItems = fileUrl.Split('/');
            string fileName = tempFileItems[tempFileItems.Length - 1];

            tempFileItems = null;
            fileUrl = null;


            string result = string.Empty;

            string filePath = Path.Combine(Config.UploadExportFileDirectory, fileName);
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);

                    result = Result.getResultXml(string.Empty);
                }
                catch (Exception ex)
                {
                    result = Result.getFaultXml(ex.Message);
                }
            }

            return result;
        }

    }
}
