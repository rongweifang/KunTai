using Aspose.Cells;
using KunTaiServiceLibrary.valueObjects;
using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KunTaiServiceLibrary
{
    [FluorineFx.RemotingService("坤泰指令报表")]
    public class KT_StationReport : IController
    {
        #region
        public string select_Runcommand_Command = @"SELECT ID,STATEIONNAME,RUNDATE,ZL_TIME,SJ_TIME,ZL_ELE,SJ_ELE,ROUND((SJ_ELE-ZL_ELE),2) AS JN_ELE,CASE WHEN SJ_ELE-ZL_ELE=0 THEN 0 ELSE  ROUND(((SJ_ELE-ZL_ELE)/CONVERT(FLOAT,SJ_ELE)),2) END AS JN_ELE_RATIO
,ZL_WATER,SJ_WATER,ROUND((CONVERT(FLOAT,ISNULL(SJ_WATER,0))-CONVERT(FLOAT,ISNULL(ZL_WATER,0))),2) AS JN_WATER,CASE WHEN ((ISNULL(SJ_WATER,0)-ISNULL(ZL_WATER,0))=0 OR ISNULL(SJ_WATER,0)=0) THEN 0 ELSE ROUND(Convert(float,(ISNULL(SJ_WATER,0)-ISNULL(ZL_WATER,0))/ISNULL(SJ_WATER,0)),2) END AS JN_WATER_RATIO
,METER_NUM,SHOWID ,1 AS PX FROM KT_RUNCOMMAND KR WHERE RUNDATE=CONVERT(DATETIME,'{0}')
UNION SELECT  '00000000-0000-0000-0000-000000000000','合计','1900-1-1',SUM(ZL_TIME),SUM(SJ_TIME),SUM(ZL_ELE),SUM(SJ_ELE),ROUND(SUM(SJ_ELE-ZL_ELE),2) 
,CASE WHEN SUM(SJ_ELE)-SUM(ZL_ELE)=0 THEN 0 ELSE  ROUND((SUM(SJ_ELE-ZL_ELE)/SUM(SJ_ELE)),2) END,SUM(ZL_WATER),SUM(SJ_WATER),ROUND(SUM(SJ_WATER)-SUM(ZL_WATER),2)
,CASE WHEN (SUM(SJ_WATER)-SUM(ZL_WATER))=NULL THEN 0 ELSE ROUND(Convert(float,(SUM(SJ_WATER)-SUM(ZL_WATER))/SUM(SJ_WATER)),2) END,SUM(METER_NUM),999,2 AS PX FROM KT_RUNCOMMAND WHERE RUNDATE=CONVERT(DATETIME,'{0}') ORDER BY PX, SHOWID";

        public string select_MonthRuncommand_Command = "SELECT DISTINCT RUNDATE,(SELECT TOP 1 EMPLOYEENAME FROM KT_RUNCOMMAND WHERE RUNDATE=KR.RUNDATE ) AS EMPLOYEENAME,(SELECT TOP 1 MAXVALUE FROM KT_RUNCOMMAND WHERE RUNDATE=KR.RUNDATE) AS MAXVALUE,(SELECT TOP 1 MINVALUE FROM KT_RUNCOMMAND WHERE RUNDATE=KR.RUNDATE) AS MINVALUE  FROM KT_RUNCOMMAND KR WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1} ORDER BY RUNDATE ASC";

        public string select_UserRuncommand_Command = @"SELECT DISTINCT LTRIM(RTRIM(EMPLOYEENAME)) AS EMPLOYEENAME,
(SELECT SUM(MAXVALUE) AS MAXVALUE FROM (SELECT DISTINCT RUNDATE,MAXVALUE,MINVALUE FROM KT_RUNCOMMAND WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1} AND LTRIM(RTRIM(EMPLOYEENAME))=KR.EMPLOYEENAME) T) AS MAXVALUE,
(SELECT SUM(MINVALUE) AS MINVALUE FROM (SELECT DISTINCT RUNDATE,MAXVALUE,MINVALUE FROM KT_RUNCOMMAND WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1} AND LTRIM(RTRIM(EMPLOYEENAME))=KR.EMPLOYEENAME) T) AS MINVALUE,
ROUND(CONVERT(FLOAT,(SELECT (SUM(MAXVALUE)+SUM(MINVALUE))/2/COUNT(*) AS AVEVALUE FROM (SELECT DISTINCT RUNDATE,MAXVALUE,MINVALUE FROM KT_RUNCOMMAND WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1} AND LTRIM(RTRIM(EMPLOYEENAME))=KR.EMPLOYEENAME) T)),2) AS AVEVALUE,
(SELECT COUNT(1) AS DAYNUM FROM (SELECT DISTINCT RUNDATE,MAXVALUE,MINVALUE FROM KT_RUNCOMMAND WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1} AND LTRIM(RTRIM(EMPLOYEENAME))=KR.EMPLOYEENAME) T) AS DAYNUM
FROM KT_RUNCOMMAND KR WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1}  GROUP BY EMPLOYEENAME";

        public string select_UserTotalRuncommand_Command = @"SELECT STATEIONNAME,SUM(ZL_TIME) AS ZL_TIME,SUM(SJ_TIME) AS SJ_TIME
,SUM(ZL_ELE) AS ZL_ELE,SUM(SJ_ELE) AS SJ_ELE,ROUND(SUM(SJ_ELE-ZL_ELE),2) AS JN_ELE
,CASE WHEN SUM(SJ_ELE)-SUM(ZL_ELE) =0 THEN 0 ELSE  ROUND(((SUM(SJ_ELE)-SUM(ZL_ELE))/CONVERT(FLOAT,SUM(ZL_ELE))),4) END AS JN_ELE_RATIO
,SUM(ZL_WATER) AS ZL_WATER
,SUM(SJ_WATER) AS SJ_WATER
,ROUND(CONVERT(FLOAT,(SUM(SJ_WATER)-SUM(ZL_WATER))),2) AS JN_WATER
,CASE WHEN (SUM(SJ_WATER)-SUM(ZL_WATER))=0 OR ISNULL(SUM(SJ_WATER),0)=0 THEN 0 ELSE ROUND(Convert(float,(SUM(SJ_WATER)-SUM(ZL_WATER))/SUM(ZL_WATER)),4) END AS JN_WATER_RATIO
,SUM(METER_NUM) AS METER_NUM,1 AS PX
FROM KT_RUNCOMMAND 
WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1} AND LTRIM(RTRIM(EMPLOYEENAME))='{2}' 
GROUP BY STATEIONNAME
UNION
SELECT '合计',SUM(ZL_TIME) AS ZL_TIME,SUM(SJ_TIME) AS SJ_TIME
,SUM(ZL_ELE) AS ZL_ELE,SUM(SJ_ELE) AS SJ_ELE,ROUND(SUM(SJ_ELE-ZL_ELE),2) AS JN_ELE
,CASE WHEN SUM(SJ_ELE)-SUM(ZL_ELE) =0 THEN 0 ELSE  ROUND(((SUM(SJ_ELE)-SUM(ZL_ELE))/CONVERT(FLOAT,SUM(ZL_ELE))),4) END AS JN_ELE_RATIO
,SUM(ZL_WATER) AS ZL_WATER
,SUM(SJ_WATER) AS SJ_WATER
,ROUND(CONVERT(FLOAT,(SUM(SJ_WATER)-SUM(ZL_WATER))),2) AS JN_WATER
,CASE WHEN (SUM(SJ_WATER)-SUM(ZL_WATER))=0 OR ISNULL(SUM(SJ_WATER),0)=0 THEN 0 ELSE ROUND(Convert(float,(SUM(SJ_WATER)-SUM(ZL_WATER))/SUM(ZL_WATER)),4) END AS JN_WATER_RATIO
,SUM(METER_NUM) AS METER_NUM,2 AS PX
FROM KT_RUNCOMMAND 
WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1} AND LTRIM(RTRIM(EMPLOYEENAME))='{2}' 
ORDER BY PX";

        public string select_StationRuncommand_Command = @"SELECT COUNT(DISTINCT RUNDATE) AS DAYNUM
,(SELECT SUM(MAXVALUE) FROM (SELECT DISTINCT RUNDATE,MAXVALUE FROM KT_RUNCOMMAND WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1}) MX) AS MAXVALUE
,(SELECT SUM(MINVALUE) FROM (SELECT DISTINCT RUNDATE,MINVALUE FROM KT_RUNCOMMAND WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1}) MN) AS MINVALUE
,ROUND(CONVERT(FLOAT,(SELECT (SUM(MAXVALUE)+SUM(MINVALUE))/2/COUNT(*) AS AVEVALUE FROM (SELECT DISTINCT RUNDATE,MAXVALUE,MINVALUE FROM KT_RUNCOMMAND WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1}) AV)),4) AS AVEVALUE
FROM KT_RUNCOMMAND WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1}";

        public string select_StationTotalRuncommand_Command = @"SELECT STATEIONNAME,SUM(ZL_TIME) AS ZL_TIME,SUM(SJ_TIME) AS SJ_TIME
,SUM(ZL_ELE) AS ZL_ELE,SUM(SJ_ELE) AS SJ_ELE,ROUND(SUM(SJ_ELE-ZL_ELE),2) AS JN_ELE
,CASE WHEN SUM(SJ_ELE)-SUM(ZL_ELE) =0 THEN 0 ELSE  ROUND(((SUM(SJ_ELE)-SUM(ZL_ELE))/CONVERT(FLOAT,SUM(ZL_ELE))),4) END AS JN_ELE_RATIO
,SUM(ZL_WATER) AS ZL_WATER
,SUM(SJ_WATER) AS SJ_WATER
,ROUND(CONVERT(FLOAT,(SUM(SJ_WATER)-SUM(ZL_WATER))),2) AS JN_WATER
,CASE WHEN (SUM(SJ_WATER)-SUM(ZL_WATER))=0 OR ISNULL(SUM(SJ_WATER),0)=0 THEN 0 ELSE ROUND(Convert(float,(SUM(SJ_WATER)-SUM(ZL_WATER))/SUM(ZL_WATER)),4) END AS JN_WATER_RATIO
,SUM(METER_NUM) AS METER_NUM,1 AS PX
FROM KT_RUNCOMMAND 
WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1} 
GROUP BY STATEIONNAME
UNION
SELECT '合计',SUM(ZL_TIME) AS ZL_TIME,SUM(SJ_TIME) AS SJ_TIME
,SUM(ZL_ELE) AS ZL_ELE,SUM(SJ_ELE) AS SJ_ELE,ROUND(SUM(SJ_ELE-ZL_ELE),2) AS JN_ELE
,CASE WHEN SUM(SJ_ELE)-SUM(ZL_ELE) =0 THEN 0 ELSE  ROUND(((SUM(SJ_ELE)-SUM(ZL_ELE))/CONVERT(FLOAT,SUM(ZL_ELE))),4) END AS JN_ELE_RATIO
,SUM(ZL_WATER) AS ZL_WATER
,SUM(SJ_WATER) AS SJ_WATER
,ROUND(CONVERT(FLOAT,(SUM(SJ_WATER)-SUM(ZL_WATER))),2) AS JN_WATER
,CASE WHEN (SUM(SJ_WATER)-SUM(ZL_WATER))=0 OR ISNULL(SUM(SJ_WATER),0)=0 THEN 0 ELSE ROUND(Convert(float,(SUM(SJ_WATER)-SUM(ZL_WATER))/SUM(ZL_WATER)),4) END AS JN_WATER_RATIO
,SUM(METER_NUM) AS METER_NUM,2 AS PX
FROM KT_RUNCOMMAND 
WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1}
ORDER BY PX";

        public string select_TeamTotalRuncommand_Command = @"SELECT LTRIM(RTRIM(EMPLOYEENAME)) AS EMPLOYEENAME,SUM(ZL_TIME) AS ZL_TIME,SUM(SJ_TIME) AS SJ_TIME
,SUM(ZL_ELE) AS ZL_ELE,SUM(SJ_ELE) AS SJ_ELE,ROUND(SUM(SJ_ELE-ZL_ELE),2) AS JN_ELE
,CASE WHEN SUM(SJ_ELE)-SUM(ZL_ELE) =0 THEN 0 ELSE  ROUND(((SUM(SJ_ELE)-SUM(ZL_ELE))/CONVERT(FLOAT,SUM(ZL_ELE))),4) END AS JN_ELE_RATIO
,SUM(ZL_WATER) AS ZL_WATER
,SUM(SJ_WATER) AS SJ_WATER
,ROUND(CONVERT(FLOAT,(SUM(SJ_WATER)-SUM(ZL_WATER))),2) AS JN_WATER
,CASE WHEN (SUM(SJ_WATER)-SUM(ZL_WATER))=0 OR ISNULL(SUM(SJ_WATER),0)=0 THEN 0 ELSE ROUND(Convert(float,(SUM(SJ_WATER)-SUM(ZL_WATER))/SUM(ZL_WATER)),4) END AS JN_WATER_RATIO
,SUM(METER_NUM) AS METER_NUM,1 AS PX
FROM KT_RUNCOMMAND 
WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1}
GROUP BY LTRIM(RTRIM(EMPLOYEENAME))
UNION
SELECT '合计',SUM(ZL_TIME) AS ZL_TIME,SUM(SJ_TIME) AS SJ_TIME
,SUM(ZL_ELE) AS ZL_ELE,SUM(SJ_ELE) AS SJ_ELE,ROUND(SUM(SJ_ELE-ZL_ELE),2) AS JN_ELE
,CASE WHEN SUM(SJ_ELE)-SUM(ZL_ELE) =0 THEN 0 ELSE  ROUND(((SUM(SJ_ELE)-SUM(ZL_ELE))/CONVERT(FLOAT,SUM(ZL_ELE))),4) END AS JN_ELE_RATIO
,SUM(ZL_WATER) AS ZL_WATER
,SUM(SJ_WATER) AS SJ_WATER
,ROUND(CONVERT(FLOAT,(SUM(SJ_WATER)-SUM(ZL_WATER))),2) AS JN_WATER
,CASE WHEN (SUM(SJ_WATER)-SUM(ZL_WATER))=0 OR ISNULL(SUM(SJ_WATER),0)=0 THEN 0 ELSE ROUND(Convert(float,(SUM(SJ_WATER)-SUM(ZL_WATER))/SUM(ZL_WATER)),4) END AS JN_WATER_RATIO
,SUM(METER_NUM) AS METER_NUM,2 AS PX
FROM KT_RUNCOMMAND 
WHERE YEAR(RUNDATE)={0} AND MONTH(RUNDATE)={1}
ORDER BY PX";
        #endregion
        public KT_StationReport()
        { }

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

            string datetime = string.IsNullOrEmpty(xml.Element("DATETIME").Value)
               ? DateTime.Now.ToString("yyyy-MM-dd")
               : xml.Element("DATETIME").Value;
            string commandText = string.Format(select_Runcommand_Command, datetime);

            DataSet dataSetPushOrder = null;
            try
            {
                dataSetPushOrder = new DataAccessHandler().executeDatasetResult(commandText, null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetPushOrder != null && dataSetPushOrder.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemXml(ref dataSetPushOrder, datetime));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            return result;
        }
        private string getDataItemXml(ref DataSet dataSetPushOrder, string datetime)
        {
            StringBuilder xml = new StringBuilder();
            string sqlstr = string.Format("SELECT COUNT(1) AS TOTAL,convert(varchar, RUNDATE, 110),EMPLOYEENAME,MAXVALUE,MINVALUE,(CONVERT(FLOAT,MAXVALUE)+CONVERT(FLOAT,MINVALUE))/2 AS AVEVALUE FROM KT_RUNCOMMAND KR WHERE RUNDATE='{0}' GROUP BY RUNDATE,EMPLOYEENAME,MAXVALUE,MINVALUE", datetime);
            DataSet ds = new DataAccessHandler().executeDatasetResult(sqlstr, null);
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    xml.AppendFormat("<DATAS COUNT=\"{0}\" TOTAL=\"{0}\" RUNDATE=\"{1}\" EMPLOYEENAME=\"{2}\" MAXVALUE=\"{3}\" MINVALUE=\"{4}\" AVEVALUE=\"{5}\" >", dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
                }
                else
                {
                    xml.AppendFormat("<DATAS>");
                }
            }
            //  KT_RunCommandObject RunCommandObject = null;
            foreach (DataRow row in dataSetPushOrder.Tables[0].Rows)
            {
                xml.AppendFormat("<DATA STATEIONNAME=\"{0}\" ZL_TIME=\"{1}\" SJ_TIME=\"{2}\" ZL_ELE=\"{3}\" SJ_ELE=\"{4}\" JN_ELE=\"{5}\" JN_ELE_RATIO=\"{6}\" ZL_WATER=\"{7}\" SJ_WATER=\"{8}\" JN_WATER=\"{9}\" JN_WATER_RATIO=\"{10}\" METER_NUM=\"{11}\" ID=\"{12}\"/>",
                 row["STATEIONNAME"].ToString(),
                 row["ZL_TIME"].ToString(),
                 row["SJ_TIME"].ToString(),
                   row["ZL_ELE"].ToString(),
                    row["SJ_ELE"].ToString(),
                   row["JN_ELE"].ToString(),
                  row["JN_ELE_RATIO"].ToString(),
                   row["ZL_WATER"].ToString(),
                   row["SJ_WATER"].ToString(),
                   row["JN_WATER"].ToString(),
                   row["JN_WATER_RATIO"].ToString(),
                   row["METER_NUM"].ToString(),
                    row["ID"].ToString()
                );
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }

        public string getDataItemDetails(string text)
        {
            throw new NotImplementedException();
        }

        #region 获取下载数据
        public string downReportFile(string text)
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

            string datetime = string.IsNullOrEmpty(xml.Element("DATETIME").Value)
             ? DateTime.Now.ToString("yyyy-MM")
             : xml.Element("DATETIME").Value;

            string FileName = string.Empty;
            string directory = string.Empty;

            string[] Months = datetime.Split('-');
            string slYear = Months[0];
            string slMonth = Months[1];

            string commandText = string.Format(select_MonthRuncommand_Command, slYear, slMonth);

            DataSet dataSetDownReport = null;
            try
            {
                dataSetDownReport = new DataAccessHandler().executeDatasetResult(commandText, null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            if (dataSetDownReport.Tables.Count > 0)
            {
                DataTable dt = dataSetDownReport.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    CreateReportFile(dt, slYear, slMonth, ref FileName);
                }
            }

            StringBuilder result = new StringBuilder();
            result.Append("<DATAS>");
            result.AppendFormat("<DATA FILENAME=\"{0}\" FILEDOWNURL=\"{1}\"/>", FileName, string.Format("{0}/{1}", Config.UploadExportFileHttpUrl, FileName));
            result.Append("</DATAS>");

            text = null;
            xml = null;
            dataSetDownReport = null;

            return Result.getResultXml(result.ToString());
        }

        private void CreateReportFile(DataTable dt, string slYear, string slMonth, ref string FileName)
        {
            //坤泰热源15-16年度12月份换热站日运行水、电耗量统计表.xlsx
            FileName = string.Format("坤泰热源{0}年度{1}月份换热站日运行水、电耗量统计表.xls", GetYears(slYear, slMonth), slMonth);

            //0 注册授权文件
            Aspose.Cells.License license = null;
            try
            {
                license = new Aspose.Cells.License();
                license.SetLicense(Config.ExportLicenseUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string directory = Config.UploadExportFileDirectory + "\\" + FileName;

            if (File.Exists(directory))
            {
                File.Delete(directory);
            }
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
            workbook.Worksheets.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Aspose.Cells.Worksheet Sheet = CreateSheet(workbook, dt.Rows[i], slYear, slMonth);
            }

            //按班组汇总-3组
            string commandUserText = string.Format(select_UserRuncommand_Command, slYear, slMonth);
            DataSet dataSetUserReport = null;
            try
            {
                dataSetUserReport = new DataAccessHandler().executeDatasetResult(commandUserText, null);
            }
            catch (Exception ex)
            {
            }
            if (dataSetUserReport.Tables.Count > 0)
            {
                DataTable dtUser = dataSetUserReport.Tables[0];
                foreach (DataRow dr in dtUser.Rows)
                {
                    Aspose.Cells.Worksheet Sheet1 = CreateTotalSheet(workbook, dr, slYear, slMonth);
                }
            }

            //各站汇总
            //各班组汇总
            string commandStationText = string.Format(select_StationRuncommand_Command, slYear, slMonth);
            DataSet dataSetStationReport = null;
            try
            {
                dataSetStationReport = new DataAccessHandler().executeDatasetResult(commandStationText, null);
            }
            catch (Exception ex)
            {
            }
            if (dataSetStationReport.Tables.Count > 0)
            {
                DataRow drStation = dataSetStationReport.Tables[0].Rows[0];
                Aspose.Cells.Worksheet Sheet2 = CreateStationSheet(workbook, drStation, slYear, slMonth);
                Aspose.Cells.Worksheet Sheet3 = CreateTeamSheet(workbook, drStation, slYear, slMonth);
            }
            

            workbook.Save(directory);
        }

        private Worksheet CreateTeamSheet(Workbook workbook, DataRow dr, string slYear, string slMonth)
        {
            string sheetName = "各班组汇总";
            Aspose.Cells.Worksheet sheet = workbook.Worksheets.Add(sheetName);
            sheet.Name = sheetName;

            string[] L3 = { "姓名", "指令运行时间(h) ", "实际运行时间(h)", "指令耗能电(kw.h) ", "实际耗能电(kw.h)  ", "节能量（T） ", "节能率 ", " 指令耗能水(T) ", "实际耗能水(T) ", " 节能量（T） ", "节能率 "};
            string[] Col = { "STATEIONNAME", "ZL_TIME", "SJ_TIME", "ZL_ELE", "SJ_ELE", "JN_ELE", "JN_ELE_RATIO", "ZL_WATER", "SJ_WATER", "JN_WATER", "JN_WATER_RATIO" };

            string commandTeamTotalText = string.Format(select_TeamTotalRuncommand_Command, slYear, slMonth);
            DataSet dataSetTeamTotalReport = null;
            try
            {
                dataSetTeamTotalReport = new DataAccessHandler().executeDatasetResult(commandTeamTotalText, null);
            }
            catch (Exception ex)
            {

            }
            if (dataSetTeamTotalReport.Tables.Count > 0)
            {
                int rowHeight = 19;//行高
                string cellFontName = "宋体";//字体
                int cellFontSize = 10;//字体大小

                //坤泰热源15-16年度12月份锅炉及各换热站日运行统计表(王忠)
                string titleName = string.Format("坤泰热源{0}年度{1}月份耗锅炉及各换热站日运行统计表", GetYears(slYear, slMonth), slMonth);

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

                #region 行，列
                int r = 0;//行
                int c = 0;//列
                #endregion

                #region 标题
                sheet.Cells.Merge(0, 0, 1, L3.Length);
                sheet.Cells[0, 0].PutValue(titleName);
                sheet.Cells[0, 0].Style.Font.IsBold = true;
                sheet.Cells[0, 0].Style.Font.Size = 18;
                sheet.Cells[0, 0].Style.Font.Name = cellFontName;
                sheet.Cells[0, 0].Style.HorizontalAlignment = TextAlignmentType.Center;
                sheet.Cells.SetRowHeight(0, 31);
                #endregion

                #region 第2行
                #region 天数
                r++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue("天数");
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetRowHeight(r, 27);
                sheet.Cells.SetColumnWidth(c, 14);
                c++;
                sheet.Cells.Merge(r, c, 1, 2);
                sheet.Cells[r, c].PutValue(dr["DAYNUM"].ToString());
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells.SetColumnWidth(c, 11);
                #endregion

                #region 最高温度（℃）
                c++;
                sheet.Cells.Merge(r, c, 1, 2);
                sheet.Cells[r, c].PutValue("最高温度（℃）");
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue(dr["MAXVALUE"].ToString());
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                #endregion

                #region 最低温度（℃）
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue("最低温度（℃）");
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue(dr["MINVALUE"].ToString());
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                #endregion

                #region 平均温度（℃）
                c++;
                sheet.Cells.Merge(r, c, 1, 2);
                sheet.Cells[r, c].PutValue("日平均温度（℃）");
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells[r, c].SetStyle(cellStyle);
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue(dr["AVEVALUE"].ToString());
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                #endregion

                #endregion

                #region 第3行
                r++;
                for (int j = 0; j < L3.Length; j++)
                {
                    sheet.Cells.Merge(r, j, 1, 1);
                    sheet.Cells[r, j].PutValue(L3[j].Trim());
                    sheet.Cells[r, j].SetStyle(cellStyle);
                    sheet.Cells[r, j].Style.Font.IsBold = true;
                    sheet.Cells.SetRowHeight(r, 27);
                }
                #endregion

                #region 各站数据 第4-23行

                for (int i = 0; i < dataSetTeamTotalReport.Tables[0].Rows.Count; i++)
                {
                    r++;
                    for (int j = 0; j < L3.Length; j++)
                    {
                        sheet.Cells.Merge(r, j, 1, 1);
                        sheet.Cells[r, j].PutValue(dataSetTeamTotalReport.Tables[0].Rows[i][j].ToString());
                        sheet.Cells[r, j].SetStyle(cellStyle);
                        sheet.Cells.SetRowHeight(r, rowHeight);
                    }
                }
                #endregion

                #region 制表人
                r++;
                sheet.Cells.Merge(r, 0, 3, L3.Length);
                sheet.Cells[r, 0].PutValue("               制表人：                              制控负责人：                                 巡站负责人：");
                sheet.Cells.SetRowHeight(r, 16.5);
                sheet.Cells.SetRowHeight(r++, 16.5);
                sheet.Cells.SetRowHeight(r++, 16.5);
                #endregion
            }
            return sheet;
        }

        private Worksheet CreateStationSheet(Workbook workbook, DataRow dr, string slYear, string slMonth)
        {
            string sheetName = "各站汇总";
            Aspose.Cells.Worksheet sheet = workbook.Worksheets.Add(sheetName);
            sheet.Name = sheetName;

            string[] L3 = { "站名  ", "指令运行时间(h) ", "实际运行时间(h) ", "指令耗能电(kw.h) ", "实际耗能电(kw.h)  ", "节能量（T） ", "节能率 ", " 指令耗能水(T) ", "实际耗能水(T) ", " 节能量（T） ", "节能率 ", " 电表用电(kw.h) " };
            string[] Col = { "STATEIONNAME", "ZL_TIME", "SJ_TIME", "ZL_ELE", "SJ_ELE", "JN_ELE", "JN_ELE_RATIO", "ZL_WATER", "SJ_WATER", "JN_WATER", "JN_WATER_RATIO", "METER_NUM" };

            string commandStationTotalText = string.Format(select_StationTotalRuncommand_Command, slYear, slMonth);
            DataSet dataSetStationTotalReport = null;
            try
            {
                dataSetStationTotalReport = new DataAccessHandler().executeDatasetResult(commandStationTotalText, null);
            }
            catch (Exception ex)
            {

            }
            if (dataSetStationTotalReport.Tables.Count > 0)
            {
                int rowHeight = 19;//行高
                string cellFontName = "宋体";//字体
                int cellFontSize = 10;//字体大小

                //坤泰热源15-16年度12月份锅炉及各换热站日运行统计表(王忠)
                string titleName = string.Format("坤泰热源{0}年度{1}月份耗锅炉及各换热站日运行统计表", GetYears(slYear, slMonth), slMonth);

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

                #region 行，列
                int r = 0;//行
                int c = 0;//列
                #endregion

                #region 标题
                sheet.Cells.Merge(0, 0, 1, L3.Length);
                sheet.Cells[0, 0].PutValue(titleName);
                sheet.Cells[0, 0].Style.Font.IsBold = true;
                sheet.Cells[0, 0].Style.Font.Size = 18;
                sheet.Cells[0, 0].Style.Font.Name = cellFontName;
                sheet.Cells[0, 0].Style.HorizontalAlignment = TextAlignmentType.Center;
                sheet.Cells.SetRowHeight(0, 31);
                #endregion

                #region 第2行
                #region 天数
                r++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue("天数");
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetRowHeight(r, 27);
                sheet.Cells.SetColumnWidth(c, 14);
                c++;
                sheet.Cells.Merge(r, c, 1, 4);
                sheet.Cells[r, c].PutValue(dr["DAYNUM"].ToString());
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells.SetColumnWidth(c, 11);
                #endregion

                #region 最高温度（℃）
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue("最高温度（℃）");
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue(dr["MAXVALUE"].ToString());
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                #endregion

                #region 最低温度（℃）
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue("最低温度（℃）");
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue(dr["MINVALUE"].ToString());
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                #endregion

                #region 平均温度（℃）
                c++;
                sheet.Cells.Merge(r, c, 1, 2);
                sheet.Cells[r, c].PutValue("日平均温度（℃）");
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells[r, c].SetStyle(cellStyle);
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue(dr["AVEVALUE"].ToString());
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                #endregion

                #endregion

                #region 第3行
                r++;
                for (int j = 0; j < L3.Length; j++)
                {
                    sheet.Cells.Merge(r, j, 1, 1);
                    sheet.Cells[r, j].PutValue(L3[j].Trim());
                    sheet.Cells[r, j].SetStyle(cellStyle);
                    sheet.Cells[r, j].Style.Font.IsBold = true;
                    sheet.Cells.SetRowHeight(r, 27);
                }
                #endregion

                #region 各站数据 第4-23行

                for (int i = 0; i < dataSetStationTotalReport.Tables[0].Rows.Count; i++)
                {
                    r++;
                    for (int j = 0; j < L3.Length; j++)
                    {
                        sheet.Cells.Merge(r, j, 1, 1);
                        sheet.Cells[r, j].PutValue(dataSetStationTotalReport.Tables[0].Rows[i][j].ToString());
                        sheet.Cells[r, j].SetStyle(cellStyle);
                        sheet.Cells.SetRowHeight(r, rowHeight);
                    }

                }
                #endregion

                #region 制表人
                r++;
                sheet.Cells.Merge(r, 0, 3, L3.Length);
                sheet.Cells[r, 0].PutValue("               制表人：                              制控负责人：                                 巡站负责人：");
                sheet.Cells.SetRowHeight(r, 16.5);
                sheet.Cells.SetRowHeight(r++, 16.5);
                sheet.Cells.SetRowHeight(r++, 16.5);
                #endregion
            }
            return sheet;
        }

        private Worksheet CreateTotalSheet(Workbook workbook, DataRow dr, string slYear, string slMonth)
        {
            string EMPLOYEENAME = dr["EMPLOYEENAME"].ToString();
            string sheetName = string.Format("{0}汇总", EMPLOYEENAME);
            Aspose.Cells.Worksheet sheet = workbook.Worksheets.Add(sheetName);
            sheet.Name = sheetName;//Sheet名字

            string[] L3 = { "站名  ", "指令运行时间(h) ", "实际运行时间(h) ", "指令耗能电(kw.h) ", "实际耗能电(kw.h)  ", "节能量（T） ", "节能率 ", " 指令耗能水(T) ", "实际耗能水(T) ", " 节能量（T） ", "节能率 ", " 电表用电(kw.h) " };
            string[] Col = { "STATEIONNAME", "ZL_TIME", "SJ_TIME", "ZL_ELE", "SJ_ELE", "JN_ELE", "JN_ELE_RATIO", "ZL_WATER", "SJ_WATER", "JN_WATER", "JN_WATER_RATIO", "METER_NUM" };

            string commandUserTotalText = string.Format(select_UserTotalRuncommand_Command, slYear, slMonth, EMPLOYEENAME);
            DataSet dataSetUserTotalReport = null;
            try
            {
                dataSetUserTotalReport = new DataAccessHandler().executeDatasetResult(commandUserTotalText, null);
            }
            catch (Exception ex)
            {

            }
            if (dataSetUserTotalReport.Tables.Count > 0)
            {
                int rowHeight = 19;//行高
                string cellFontName = "宋体";//字体
                int cellFontSize = 10;//字体大小

                //坤泰热源15-16年度12月份锅炉及各换热站日运行统计表(王忠)
                string titleName = string.Format("坤泰热源{0}年度{1}月份耗锅炉及各换热站日运行统计表({2})", GetYears(slYear, slMonth), slMonth, EMPLOYEENAME);

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

                #region 行，列
                int r = 0;//行
                int c = 0;//列
                #endregion

                #region 标题
                sheet.Cells.Merge(0, 0, 1, L3.Length);
                sheet.Cells[0, 0].PutValue(titleName);
                sheet.Cells[0, 0].Style.Font.IsBold = true;
                sheet.Cells[0, 0].Style.Font.Size = 18;
                sheet.Cells[0, 0].Style.Font.Name = cellFontName;
                sheet.Cells[0, 0].Style.HorizontalAlignment = TextAlignmentType.Center;
                sheet.Cells.SetRowHeight(0, 31);
                #endregion

                #region 第2行
                #region 日期
                r++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue("天数");
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetRowHeight(r, 27);
                sheet.Cells.SetColumnWidth(c, 14);

                c++;
                sheet.Cells.Merge(r, c, 1, 2);
                sheet.Cells[r, c].PutValue(dr["DAYNUM"].ToString());
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells.SetColumnWidth(c, 11);
                #endregion

                #region 姓名
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue("姓名");
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue(EMPLOYEENAME);
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                #endregion

                #region 最高温度（℃）
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue("最高温度（℃）");
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue(dr["MAXVALUE"].ToString());
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                #endregion

                #region 最低温度（℃）
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue("最低温度（℃）");
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue(dr["MINVALUE"].ToString());
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                #endregion

                #region 平均温度（℃）
                c++;
                sheet.Cells.Merge(r, c, 1, 2);
                sheet.Cells[r, c].PutValue("平均温度（℃）");
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                c++;
                sheet.Cells[r, c].SetStyle(cellStyle);
                c++;
                sheet.Cells.Merge(r, c, 1, 1);
                sheet.Cells[r, c].PutValue(dr["AVEVALUE"].ToString());
                sheet.Cells[r, c].SetStyle(cellStyle);
                sheet.Cells[r, c].Style.Font.IsBold = true;
                sheet.Cells.SetColumnWidth(c, 11);
                #endregion

                #endregion

                #region 第3行
                r++;
                for (int j = 0; j < L3.Length; j++)
                {
                    sheet.Cells.Merge(r, j, 1, 1);
                    sheet.Cells[r, j].PutValue(L3[j].Trim());
                    sheet.Cells[r, j].SetStyle(cellStyle);
                    sheet.Cells[r, j].Style.Font.IsBold = true;
                    sheet.Cells.SetRowHeight(r, 27);
                }
                #endregion

                #region 各站数据 第4-23行

                for (int i = 0; i < dataSetUserTotalReport.Tables[0].Rows.Count; i++)
                {
                    r++;
                    for (int j = 0; j < L3.Length; j++)
                    {
                        sheet.Cells.Merge(r, j, 1, 1);
                        sheet.Cells[r, j].PutValue(dataSetUserTotalReport.Tables[0].Rows[i][j].ToString());
                        sheet.Cells[r, j].SetStyle(cellStyle);
                        sheet.Cells.SetRowHeight(r, rowHeight);
                    }

                }
                #endregion

                #region 制表人
                r++;
                sheet.Cells.Merge(r, 0, 3, L3.Length);
                sheet.Cells[r, 0].PutValue("               制表人：                              制控负责人：                                 巡站负责人：");
                sheet.Cells.SetRowHeight(r, 16.5);
                sheet.Cells.SetRowHeight(r++, 16.5);
                sheet.Cells.SetRowHeight(r++, 16.5);
                #endregion
            }
            return sheet;
        }

        private string GetYears(string slYear, string slMonth)
        {
            string result = "";
            int month = 1;
            int year = 2015;
            if (int.TryParse(slYear, out year))
            {
                if (int.TryParse(slMonth, out month))
                {
                    if (month > 9)
                    {
                        int nextYear = year + 1;
                        result = string.Format("{0}-{1}", year, nextYear);
                    }
                    else if (month < 6)
                    {
                        int lastYear = year - 1;
                        result = string.Format("{0}-{1}", lastYear, year);
                    }
                }
            }

            return result;
        }

        private Worksheet CreateSheet(Workbook workbook, DataRow dr, string slYear, string slMonth)
        {
            string day = Convert.ToDateTime(dr["RUNDATE"].ToString()).ToString("dd");
            string runday = Convert.ToDateTime(dr["RUNDATE"].ToString()).ToString("yyyy-MM-dd");

            string sheetName = string.Format("{0}.{1}", slMonth, day);
            Aspose.Cells.Worksheet sheet = workbook.Worksheets.Add(day);
            sheet.Name = sheetName;//Sheet名字

            string commandText = string.Format(select_Runcommand_Command, runday);
            DataSet dataSetDayReportt = null;
            try
            {
                dataSetDayReportt = new DataAccessHandler().executeDatasetResult(commandText, null);
            }
            catch (Exception ex)
            {

            }
            if (dataSetDayReportt.Tables.Count > 0)
            {
                DataView dv = new DataView(dataSetDayReportt.Tables[0]);
                string[] L3 = { "站名  ", "指令运行时间(h) ", "实际运行时间(h) ", "指令耗能电(kw.h) ", "实际耗能电(kw.h)  ", "节能量（T） ", "节能率 ", " 指令耗能水(T) ", "实际耗能水(T) ", " 节能量（T） ", "节能率 ", " 电表用电(kw.h) " };
                string[] Col = { "STATEIONNAME", "ZL_TIME", "SJ_TIME", "ZL_ELE", "SJ_ELE", "JN_ELE", "JN_ELE_RATIO", "ZL_WATER", "SJ_WATER", "JN_WATER", "JN_WATER_RATIO", "METER_NUM" };
                DataTable dt = dv.ToTable(true, Col);

                if (dt.Rows.Count > 0)
                {
                    int rowHeight = 19;//行高
                    string cellFontName = "宋体";//字体
                    int cellFontSize = 10;//字体大小
                    string EMPLOYEENAME = dr["EMPLOYEENAME"].ToString();
                    //坤泰热源15-16年度12月份锅炉及各换热站日运行统计表(王忠)
                    string titleName = string.Format("坤泰热源{0}年度{1}月份耗锅炉及各换热站日运行统计表({2})", GetYears(slYear, slMonth), slMonth, EMPLOYEENAME);

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

                    #region 行，列
                    int r = 0;//行
                    int c = 0;//列
                    #endregion

                    #region 标题
                    sheet.Cells.Merge(0, 0, 1, L3.Length);
                    sheet.Cells[0, 0].PutValue(titleName);
                    sheet.Cells[0, 0].Style.Font.IsBold = true;
                    sheet.Cells[0, 0].Style.Font.Size = 18;
                    sheet.Cells[0, 0].Style.Font.Name = cellFontName;
                    sheet.Cells[0, 0].Style.HorizontalAlignment = TextAlignmentType.Center;
                    sheet.Cells.SetRowHeight(0, 31);
                    #endregion

                    #region 第2行
                    #region 日期
                    r++;
                    sheet.Cells.Merge(r, c, 1, 1);
                    sheet.Cells[r, c].PutValue("日期");
                    sheet.Cells[r, c].SetStyle(cellStyle);
                    sheet.Cells[r, c].Style.Font.IsBold = true;
                    sheet.Cells.SetRowHeight(r, 27);
                    sheet.Cells.SetColumnWidth(c, 14);

                    c++;
                    sheet.Cells.Merge(r, c, 1, 2);
                    sheet.Cells[r, c].PutValue(sheetName);
                    sheet.Cells[r, c].SetStyle(cellStyle);
                    sheet.Cells[r, c].Style.Font.IsBold = true;
                    sheet.Cells.SetColumnWidth(c, 11);
                    c++;
                    sheet.Cells[r, c].SetStyle(cellStyle);
                    sheet.Cells.SetColumnWidth(c, 11);
                    #endregion

                    #region 姓名
                    c++;
                    sheet.Cells.Merge(r, c, 1, 1);
                    sheet.Cells[r, c].PutValue("姓名");
                    sheet.Cells[r, c].Style.Font.IsBold = true;
                    sheet.Cells[r, c].SetStyle(cellStyle);
                    sheet.Cells.SetColumnWidth(c, 11);
                    c++;
                    sheet.Cells.Merge(r, c, 1, 1);
                    sheet.Cells[r, c].PutValue(EMPLOYEENAME);
                    sheet.Cells[r, c].SetStyle(cellStyle);
                    sheet.Cells[r, c].Style.Font.IsBold = true;
                    sheet.Cells.SetColumnWidth(c, 11);
                    #endregion

                    #region 最高温度（℃）
                    c++;
                    sheet.Cells.Merge(r, c, 1, 1);
                    sheet.Cells[r, c].PutValue("最高温度（℃）");
                    sheet.Cells[r, c].SetStyle(cellStyle);
                    sheet.Cells[r, c].Style.Font.IsBold = true;
                    sheet.Cells.SetColumnWidth(c, 11);
                    c++;
                    sheet.Cells.Merge(r, c, 1, 1);
                    sheet.Cells[r, c].PutValue(dr["MAXVALUE"].ToString());
                    sheet.Cells[r, c].SetStyle(cellStyle);
                    sheet.Cells[r, c].Style.Font.IsBold = true;
                    sheet.Cells.SetColumnWidth(c, 11);
                    #endregion

                    #region 最低温度（℃）
                    c++;
                    sheet.Cells.Merge(r, c, 1, 1);
                    sheet.Cells[r, c].PutValue("最低温度（℃）");
                    sheet.Cells[r, c].SetStyle(cellStyle);
                    sheet.Cells[r, c].Style.Font.IsBold = true;
                    sheet.Cells.SetColumnWidth(c, 11);
                    c++;
                    sheet.Cells.Merge(r, c, 1, 1);
                    sheet.Cells[r, c].PutValue(dr["MINVALUE"].ToString());
                    sheet.Cells[r, c].SetStyle(cellStyle);
                    sheet.Cells[r, c].Style.Font.IsBold = true;
                    sheet.Cells.SetColumnWidth(c, 11);
                    #endregion

                    #region 平均温度（℃）
                    c++;
                    sheet.Cells.Merge(r, c, 1, 2);
                    sheet.Cells[r, c].PutValue("平均温度（℃）");
                    sheet.Cells[r, c].SetStyle(cellStyle);
                    sheet.Cells[r, c].Style.Font.IsBold = true;
                    sheet.Cells.SetColumnWidth(c, 11);
                    c++;
                    sheet.Cells[r, c].SetStyle(cellStyle);
                    double aveTemp = Math.Round(((Convert.ToDouble(dr["MAXVALUE"].ToString()) + Convert.ToDouble(dr["MINVALUE"].ToString())) / 2), 2);
                    c++;
                    sheet.Cells.Merge(r, c, 1, 1);
                    sheet.Cells[r, c].PutValue(aveTemp.ToString());
                    sheet.Cells[r, c].SetStyle(cellStyle);
                    sheet.Cells[r, c].Style.Font.IsBold = true;
                    sheet.Cells.SetColumnWidth(c, 11);
                    #endregion

                    #endregion

                    #region 第3行
                    r++;
                    for (int j = 0; j < L3.Length; j++)
                    {
                        sheet.Cells.Merge(r, j, 1, 1);
                        sheet.Cells[r, j].PutValue(L3[j].Trim());
                        sheet.Cells[r, j].SetStyle(cellStyle);
                        sheet.Cells[r, j].Style.Font.IsBold = true;
                        sheet.Cells.SetRowHeight(r, 27);
                    }
                    #endregion

                    #region 各站数据 第4-23行

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        r++;
                        for (int j = 0; j < L3.Length; j++)
                        {
                            sheet.Cells.Merge(r, j, 1, 1);
                            sheet.Cells[r, j].PutValue(dt.Rows[i][j].ToString());
                            sheet.Cells[r, j].SetStyle(cellStyle);
                            sheet.Cells.SetRowHeight(r, rowHeight);
                        }

                    }
                    #endregion

                    #region 制表人
                    r++;
                    sheet.Cells.Merge(r, 0, 3, L3.Length);
                    sheet.Cells[r, 0].PutValue("               制表人：                              制控负责人：                                 巡站负责人：");
                    sheet.Cells.SetRowHeight(r, 16.5);
                    sheet.Cells.SetRowHeight(r++, 16.5);
                    sheet.Cells.SetRowHeight(r++, 16.5);
                    #endregion
                }
            }
            return sheet;
        }
        #endregion
    }
}
