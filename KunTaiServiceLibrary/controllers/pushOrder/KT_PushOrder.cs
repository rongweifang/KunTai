using Aspose.Cells;
using KunTaiServiceLibrary.controllers.pushOrder;
using KunTaiServiceLibrary.valueObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Warrior.DataAccessUtils.Desktop;

namespace KunTaiServiceLibrary
{
    [FluorineFx.RemotingService("坤泰热力公司的运行指令服务")]
    public class KT_PushOrder : IController
    {

        #region command texts
        private const string push_order_stat_select = @"SELECT *  FROM STATION ORDER BY SHOWID";

        private const string push_order_insert = "INSERT INTO [KT_PUSHORDER] ([ID], [RUNDATE], [MAXVALUE], [MINVALUE], [EXPORTTYPE], [FILENAME], [FILEURL], [NOTE]) VALUES ('{0}', '{1}', {2}, {3}, '{4}', '{5}', '{6}', '{7}')";

        private const string send_push_order_insert = "INSERT INTO [PUSHORDER] ([ID],[PUSHID], [RUNDATETIME], [FILEURL], [LOCALFILENAME], [NOTE]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')";

        private const string push_order_details = "SELECT [RUNDATE], [FILENAME], [FILEURL], [NOTE] FROM [KT_PUSHORDER] WHERE [ID]=@PUSHORDERID";

        private const string receive_push_order_insert = "INSERT INTO [PUSHORDERRECEIVE] ([POID], [RECEIVEID]) VALUES ('{0}', '{1}')";

        private const string push_order_select = "SELECT ROW_NUMBER() OVER (ORDER BY [RunDay] DESC) AS NUM1, [ID], [RUNDATE], [MAXVALUE], [MINVALUE], [EXPORTTYPE], [FILENAME], [NOTE] FROM [KT_PUSHORDER] {0} ";

        private const string push_order_total = "SELECT COUNT(*) FROM [KT_PUSHORDER]{0}";

        // private const string push_order_delete = "DELETE [KT_PUSHORDER] WHERE [ID] IN ({0})";
        private const string push_order_delete = @"DELETE [KT_PUSHORDER] WHERE [ID] IN ({0})
DELETE KT_RUNCOMMAND WHERE PUSHID IN ({0})";

        private const string push_order_use_file = "SELECT [FILEURL] FROM [PUSHORDER] WHERE [FILEURL] IN (SELECT [FILEURL] FROM [KT_PUSHORDER] WHERE [ID] IN ({0}))";

        private const string push_order_delete_select = "SELECT [ID], [FILEURL] FROM [KT_PUSHORDER] WHERE [ID] IN ({0})";

        private const string push_order_down_select = "SELECT [ID], [FILENAME], [FILEURL] FROM [KT_PUSHORDER] WHERE [ID] IN ({0})";
        #endregion

        public string addDataItem(string text)
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

            #region 1.解析各个参数值//1.1 最高、最低的温度值
            double maxValue = Convert.ToDouble(xml.Element("MAXVALUE").Value);//最大温度值
            double minValue = Convert.ToDouble(xml.Element("MINVALUE").Value);//最小温度值
            bool isToday = string.IsNullOrEmpty(xml.Element("ISTODAY").Value) ? false : Convert.ToBoolean(xml.Element("ISTODAY").Value);
            #endregion

            string runDateTime = "";
            if (isToday)
            {
                runDateTime = DateTime.Now.ToString("d日MM月yy年");
            }
            else
            {
                runDateTime = DateTime.Now.AddDays(1).ToString("d日MM月yy年");
            }

            //1日12月15年，坤泰热源指令量区间分配表.==================================================================================================================================================================================================================
            string titleName = string.Format("{0}，坤泰热源指令量区间分配表", runDateTime);
            // string titleName = string.Format("{0}，鸿坤热力指令量区间分配表", runDateTime);

            string directory = Path.Combine(Config.UploadExportFileDirectory, string.Format("{0}.xls", titleName));

            Guid ID= Guid.NewGuid();
            //创建文件
            exportPushOrderFile(ID, maxValue,  minValue, isToday, runDateTime, directory);

            string result = string.Empty;
            if (!string.IsNullOrEmpty(directory))
            {
                result = Result.getResultXml(getAddDataItemXml(ID.ToString(),string.Format("{0}.xls", titleName), runDateTime));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = null;
            xml = null;

            return result;
        }

        public string addDataItemAnyday(string text)
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



            double maxValue = Convert.ToDouble(xml.Element("MAXVALUE").Value);//最大温度值
            double minValue = Convert.ToDouble(xml.Element("MINVALUE").Value);//最小温度值
            string strDate = xml.Element("CREATEDATE").Value;
            DateTime dtDate;
            if (DateTime.TryParse(strDate, out dtDate))
            {

            }
            #region 2.初始化基本参数和初始化文件放置的目录

            //2.1 导出方案存放的文件夹（打包zip后，会自动删除这个文件夹）
            string excelFileDirectoryName = Guid.NewGuid().ToString().ToUpper();
            string directory = Path.Combine(Config.UploadExportFileDirectory, excelFileDirectoryName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            #endregion

          //  exportPushOrderFile(ref maxValue, ref minValue, isToday, ref directory);

            //3.2 判断导出文件的数量是否跟List的数量一致，如果一致，打包zip，删除原文件夹，返回客户端
            string[] tempFiles = Directory.GetFiles(directory);
            string zipFilePath = string.Empty;
            zipFilePath = string.Format("{0}.zip", directory);

            Zip.CompressionDirectory(
                directory,
                zipFilePath,
                0);
            Directory.Delete(directory, true);

            //3.3 返回客户端
            string result = string.Empty;
            if (!string.IsNullOrEmpty(zipFilePath))
            {
                result = Result.getResultXml(getAnydaDataItemXml(dtDate, ref excelFileDirectoryName));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = null;
            xml = null;
            //weatherItems = null;
            //assignPlanItems = null;
            excelFileDirectoryName = null;
            tempFiles = null;
            zipFilePath = null;


            return result;
        }

        private string getAddDataItemXml(string ID, string titleName, string runDateTime)
        {
            StringBuilder xml = new StringBuilder();
           
            xml.Append("<DATAS>");
            xml.AppendFormat("<DATA FILENAME=\"{0}\" FILEDOWNURL=\"{1}\" RUNDATETIME=\"{2}\" PUSHORDERID=\"{3}\"/>", titleName, titleName,runDateTime,ID);
            xml.Append("</DATAS>");

            return xml.ToString();
        }
        private string getAnydaDataItemXml(DateTime runDateTime, ref string excelFileDirectoryName)
        {
            StringBuilder xml = new StringBuilder();
            //string runDateTime = DateTime.Now.AddDays(1).ToString("d日MM月yy年");

            xml.Append("<DATAS>");
            xml.AppendFormat("<DATA FILENAME=\"{0}\" FILEDOWNURL=\"{1}\" RUNDATETIME=\"{2}\"/>",
                string.Format("{0}，坤泰热源指令量区间分配表.zip", runDateTime),
                 string.Format("{0}/{1}.zip", Config.UploadExportFileHttpUrl, excelFileDirectoryName),
                 runDateTime
            );
            xml.Append("</DATAS>");

            return xml.ToString();
        }
        private string getDataItemXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<PUSHORDER>");
            sb.AppendFormat("<CITYNAME>{0}</CITYNAME>", "呼和浩特坤泰");
            sb.Append("</PUSHORDER>");
            return sb.ToString();
        }
        /// <summary>
        /// 导出分配方案中的运行指令
        /// </summary>
        /// <param name="maxValue">最高温度</param>
        /// <param name="minValue">最低温度</param>
        /// <param name="directory">生成文件路径</param>
        public void exportPushOrderFile(Guid ID,double maxValue,double minValue,bool isToday, string runDateTime,string directory)
        {
            List<ZY_PushOrderWeatherObject> weatherItems = new List<ZY_PushOrderWeatherObject>();
            ZY_PushOrderWeatherObject zy_pushorderweatherObject = null;

            string weatherTxt = new KT_PushOrderWeather().getRunPercentage(getDataItemXml());
            XElement weatherxml = null;
            try
            {
                weatherxml = XElement.Parse(weatherTxt);
            }
            catch
            {
            }

            foreach (XElement Witem in weatherxml.Element("DATAS").Elements("DATA"))
            {
                zy_pushorderweatherObject = new ZY_PushOrderWeatherObject(Witem);
                weatherItems.Add(zy_pushorderweatherObject);

                zy_pushorderweatherObject = null;
            }

            //日运行时间 =(18/28-平均温度/28)*24 
            // 指令运行时间（h）  = 日运行时间
            //weatherItems[5].PERCENTAGE.ToString()
            //平均温度
            double aveTemp = (Convert.ToDouble(maxValue) + Convert.ToDouble(minValue)) / 2;
            decimal dayRunTime = 0m;

            //指令运行时间
            decimal instructTime = 0m;
            OrderAlgorithm OA = new OrderAlgorithm();
            DataTable dt = new DataAccessHandler().executeDatasetResult("SELECT TOP 1 *  FROM KT_Boiler ", null).Tables[0];

            if (dt.Rows.Count>0)
            {
                DataRow dr = dt.Rows[0];
               
                OA.MaxValue = maxValue;
                OA.MinValue = minValue;
                OA.Area = Decimal.Parse(dr["Area"].ToString());
                OA.Tonnage= int.Parse(dr["Tonnage"].ToString());
                OA.Target = int.Parse(dr["Target"].ToString());
                //OA.BoilerCount = (int)dr["BoilerCount"];
                OA.Power = int.Parse(dr["Power"].ToString());
                //OA.Scontent = (float)dr["Scontent"];
                OA.Efficiency = Decimal.Parse(dr["Efficiency"].ToString());
                // OA.Calorie = (float)dr["Calorie"];

                instructTime = OA.GetRunDate();
                dayRunTime = instructTime;
            }
           
            //区间比例
            decimal[] SectionRatio = { Convert.ToDecimal(weatherItems[5].PERCENTAGE.ToString()) , Convert.ToDecimal(weatherItems[11].PERCENTAGE.ToString()), Convert.ToDecimal(weatherItems[17].PERCENTAGE.ToString()), Convert.ToDecimal(weatherItems[23].PERCENTAGE.ToString()) };
            decimal SectionRatio1 = SectionRatio[0];
            decimal SectionRatio2 = SectionRatio[1];
            // decimal SectionRatio3 = SectionRatio[2];
            decimal SectionRatio4 = SectionRatio[3];
            decimal SectionRatio3 = 1 - SectionRatio1- SectionRatio2- SectionRatio4;
            // excel标题和运行指令的日期
            //string runDateTime ="";
            //if (isToday)
            //{
            //    runDateTime = DateTime.Now.ToString("d日MM月yy年");
            //}
            //else
            //{
            //    runDateTime = DateTime.Now.AddDays(1).ToString("d日MM月yy年");
            //}

            string titleName = string.Format("{0}，坤泰热源指令量区间分配表", runDateTime);

            #region InstructModel//保存锅炉指令
            KT_PUSHORDER_Model Model = new KT_PUSHORDER_Model();
            Model.ID = ID;
            Model.RUNDATE =runDateTime;
            Model.RunDay = isToday ? DateTime.Now : DateTime.Now.AddDays(1);
            Model.ADDDATETIME = DateTime.Now;
            Model.MAXVALUE = Convert.ToDecimal(maxValue);
            Model.MINVALUE = Convert.ToDecimal(minValue);
            Model.EXPORTTYPE = "";
            Model.FILENAME = string.Format("{0}.xls", titleName);
            Model.FILEURL = string.Format("{0}.xls", titleName);
            Model.CommandTime = instructTime;
            Model.Time_Ratio = string.Format("{0}:{1}:{2}:{3}", SectionRatio1, SectionRatio2, SectionRatio3, SectionRatio4);
            //Model.Command_Coal= KT_PushOrder_Arithmetic.GetCommand_Coal(instructTime);
            //Model.Command_Water =  KT_PushOrder_Arithmetic.GetCommand_Water(instructTime);
            //Model.Command_Ele= KT_PushOrder_Arithmetic.GetCommand_Ele(instructTime);
            //Model.Command_Alkali = KT_PushOrder_Arithmetic.GetCommand_Alkali(instructTime);
            //Model.Command_Salt = KT_PushOrder_Arithmetic.GetCommand_Salt(instructTime);
            //Model.Command_Diesel = KT_PushOrder_Arithmetic.GetCommand_Diesel(instructTime);

            Model.Command_Coal = OA.GetCoalTotal();
            Model.Command_Water = OA.GetWaterTotal();
            Model.Command_Ele = OA.GetEle();
            Model.Command_Alkali = OA.GetAlkali();
            Model.Command_Salt = OA.GetSalt();
            Model.Command_Diesel = KT_PushOrder_Arithmetic.GetCommand_Diesel(instructTime);

            Model.CreateUser = "C1359ACA-10DF-4338-8D62-66F22724B647";
            KT_PUSHORDER_Dal.Add(Model);
            #endregion

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
            

            int rowHeight = 24;//行高
            string cellFontName = "宋体";//字体
            int cellFontSize = 11;//字体大小

            string fileName = titleName;

            //1 初始化组件变量
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();//工作簿
            Aspose.Cells.Worksheet sheet = workbook.Worksheets[0];//工作表


            sheet.Name = "锅炉房";//Sheet名字

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

            #region 标题
            //标题
            sheet.Cells.Merge(0, 0, 1, 12);
            sheet.Cells[0, 0].PutValue("锅炉房区间分配表");
            sheet.Cells[0, 0].Style.Font.IsBold = true;
            sheet.Cells[0, 0].Style.Font.Size = 24;
            sheet.Cells[0, 0].Style.Font.Name = cellFontName;
            sheet.Cells[0, 0].Style.HorizontalAlignment = TextAlignmentType.Center;

            #endregion

            #region 第2、3行
            #region 日期
            //回路
            sheet.Cells.Merge(1, 0, 2, 1);
            sheet.Cells[1, 0].PutValue("日期");
            sheet.Cells[1, 0].SetStyle(cellStyle);
            sheet.Cells[2, 0].SetStyle(cellStyle);
            #endregion

            #region 日期
            //序号
            sheet.Cells.Merge(1, 1, 2, 1);
            sheet.Cells[1, 1].PutValue(runDateTime);
            sheet.Cells[1, 1].SetStyle(cellStyle);
            sheet.Cells[2, 1].SetStyle(cellStyle);
            #endregion

            #region 所占比例
            sheet.Cells.Merge(1, 2, 2, 1);
            sheet.Cells[1, 2].PutValue("所占比例(%)");
            sheet.Cells[1, 2].SetStyle(cellStyle);
            sheet.Cells[2, 2].SetStyle(cellStyle);
            #endregion

            #region 指令运行时间（h）
            sheet.Cells.Merge(1, 3, 2, 1);
            sheet.Cells[1, 3].PutValue("指令运行时间（h）");
            sheet.Cells[1, 3].SetStyle(cellStyle);
            sheet.Cells[2, 3].SetStyle(cellStyle);
            #endregion

            #region 指令耗煤量（t）
            //指令耗煤量（t）
            sheet.Cells.Merge(1, 4, 2, 1);
            sheet.Cells[1, 4].PutValue("指令耗煤量（t）");
            sheet.Cells[1, 4].SetStyle(cellStyle);
            sheet.Cells[2, 4].SetStyle(cellStyle);
            #endregion

            #region 指令耗水量（t）
            //指令耗水量（t）
            sheet.Cells.Merge(1, 5, 2, 1);
            sheet.Cells[1, 5].PutValue("指令耗水量（t）");
            sheet.Cells[1, 5].SetStyle(cellStyle);
            sheet.Cells[2, 5].SetStyle(cellStyle);
            #endregion

            #region 指令耗电量（kw/h）
            //指令耗电量（kw/h）
            sheet.Cells.Merge(1, 6, 2, 1);
            sheet.Cells[1, 6].PutValue("指令耗电量（kw/h）");
            sheet.Cells[1, 6].SetStyle(cellStyle);
            sheet.Cells[2, 6].SetStyle(cellStyle);
            #endregion

            #region 耗盐量（kg）
            //耗盐量（kg）
            sheet.Cells.Merge(1, 7, 2, 1);
            sheet.Cells[1, 7].PutValue("耗盐量（kg）");
            sheet.Cells[1, 7].SetStyle(cellStyle);
            sheet.Cells[2, 7].SetStyle(cellStyle);
            #endregion

            #region 指令耗碱量（t）
            //指令耗碱量（t）
            sheet.Cells.Merge(1, 8, 2, 1);
            sheet.Cells[1, 8].PutValue("指令耗碱量（t）");
            sheet.Cells[1, 8].SetStyle(cellStyle);
            sheet.Cells[2, 8].SetStyle(cellStyle);
            #endregion

            #region 最高温度（℃）
            //最高温度（℃）
            sheet.Cells.Merge(1, 9, 1, 1);
            sheet.Cells[1, 9].PutValue("最高温度（℃）");
            sheet.Cells[1, 9].SetStyle(cellStyle);

            #endregion

            #region 最高温度（℃） 
            //最高温度（℃）
            sheet.Cells[2, 9].PutValue(maxValue);
            sheet.Cells[2, 9].SetStyle(cellStyle);
            #endregion

            #region 最低温度（℃）
            //最低温度（℃）
            sheet.Cells.Merge(1, 10, 1, 1);
            sheet.Cells[1, 10].PutValue("最低温度（℃）");
            sheet.Cells[1, 10].SetStyle(cellStyle);


            #endregion

            #region 最低温度
            //最低温度
            sheet.Cells[2, 10].PutValue(minValue);
            sheet.Cells[2, 10].SetStyle(cellStyle);
            #endregion

            #region 平均温度（℃）
            sheet.Cells.Merge(1, 11, 1, 1);
            sheet.Cells[1, 11].PutValue("平均温度（℃）");
            sheet.Cells[1, 11].SetStyle(cellStyle);
            sheet.Cells[2, 11].PutValue(aveTemp);
            sheet.Cells[2, 11].SetStyle(cellStyle);
            #endregion
            #endregion

            #region 第4行//指令总耗量		1.00 	28.40 	158.20 	2.00 	10650.00 	16.78 	0.57 	
            //void Cells.Merge(firstRow,firstColumn,rowNumber,columnNumber);

            sheet.Cells.Merge(3, 0, 1, 2);
            sheet.Cells[3, 0].PutValue("指令总耗量");
            sheet.Cells[3, 0].SetStyle(cellStyle);
            sheet.Cells.SetRowHeight(3, 24);
            sheet.Cells[3, 1].SetStyle(cellStyle);

            #region 指令
            sheet.Cells.Merge(3, 2, 1, 1);
            sheet.Cells[3, 2].PutValue("1.00");
            sheet.Cells[3, 2].SetStyle(cellStyle);

            
            //指令运行时间
            sheet.Cells.Merge(3, 3, 1, 1);
            sheet.Cells[3, 3].PutValue(Math.Round(instructTime, 2));
            sheet.Cells[3, 3].SetStyle(cellStyle);
            //指令耗煤
            sheet.Cells.Merge(3, 4, 1, 1);
            sheet.Cells[3, 4].PutValue(OA.GetCoalTotal());
            sheet.Cells[3, 4].SetStyle(cellStyle);
            //指令耗水
            sheet.Cells.Merge(3, 5, 1, 1);
            sheet.Cells[3, 5].PutValue(OA.GetWaterTotal());
            sheet.Cells[3, 5].SetStyle(cellStyle);
            //指令耗电
            sheet.Cells.Merge(3, 6, 1, 1);
            sheet.Cells[3, 6].PutValue(OA.GetEle());
            sheet.Cells[3, 6].SetStyle(cellStyle);
            //耗盐
            sheet.Cells.Merge(3, 7, 1, 1);
            sheet.Cells[3, 7].PutValue(OA.GetSalt());
            sheet.Cells[3, 7].SetStyle(cellStyle);
            //耗碱
            sheet.Cells.Merge(3, 8, 1, 1);
            sheet.Cells[3, 8].PutValue(OA.GetAlkali());
            sheet.Cells[3, 8].SetStyle(cellStyle);

            sheet.Cells.Merge(3, 9, 1, 1);
            sheet.Cells[3, 9].PutValue("");
            sheet.Cells[3, 9].SetStyle(cellStyle);

            sheet.Cells.Merge(3, 10, 1, 1);
            sheet.Cells[3, 10].PutValue("");
            sheet.Cells[3, 10].SetStyle(cellStyle);

            sheet.Cells.Merge(3, 11, 1, 1);
            sheet.Cells[3, 11].PutValue("");
            sheet.Cells[3, 11].SetStyle(cellStyle);
            #endregion
            #endregion

            #region 第5、6、7、8行 区 间
            sheet.Cells.Merge(4, 0, 4, 1);
            sheet.Cells[4, 0].PutValue("区\n间");
            sheet.Cells[4, 0].SetStyle(cellStyle);
            sheet.Cells.SetRowHeight(4, rowHeight);
            sheet.Cells[5, 0].SetStyle(cellStyle);
            sheet.Cells.SetRowHeight(5, rowHeight);
            sheet.Cells[6, 0].SetStyle(cellStyle);
            sheet.Cells.SetRowHeight(6, rowHeight);
            sheet.Cells[7, 0].SetStyle(cellStyle);
            sheet.Cells.SetRowHeight(7, rowHeight);

            sheet.Cells.Merge(4, 1, 1, 1);
            sheet.Cells[4, 1].PutValue("1-6");
            sheet.Cells[4, 1].SetStyle(cellStyle);
            #region 指令
            sheet.Cells.Merge(4, 2, 1, 1);
            sheet.Cells[4, 2].PutValue(SectionRatio1);
            sheet.Cells[4, 2].SetStyle(cellStyle);

            sheet.Cells.Merge(4, 3, 1, 1);
            sheet.Cells[4, 3].PutValue(Math.Round(SectionRatio1 * instructTime, 2));
            sheet.Cells[4, 3].SetStyle(cellStyle);

            sheet.Cells.Merge(4, 4, 1, 1);
            sheet.Cells[4, 4].PutValue(Math.Round(SectionRatio1 * OA.GetCoalTotal(), 2));
            sheet.Cells[4, 4].SetStyle(cellStyle);

            sheet.Cells.Merge(4, 5, 1, 1);
            sheet.Cells[4, 5].PutValue(Math.Round(SectionRatio1 * OA.GetWaterTotal(), 2));
            sheet.Cells[4, 5].SetStyle(cellStyle);

            sheet.Cells.Merge(4, 6, 1, 1);
            sheet.Cells[4, 6].PutValue(Math.Round(SectionRatio1 * OA.GetEle(), 2));
            sheet.Cells[4, 6].SetStyle(cellStyle);

            sheet.Cells.Merge(4, 7, 1, 1);
            sheet.Cells[4, 7].PutValue(Math.Round(SectionRatio1 * OA.GetSalt(), 2));
            sheet.Cells[4, 7].SetStyle(cellStyle);

            sheet.Cells.Merge(4, 8, 1, 1);
            sheet.Cells[4, 8].PutValue(Math.Round(SectionRatio1 * OA.GetAlkali(), 2));
            sheet.Cells[4, 8].SetStyle(cellStyle);

            sheet.Cells.Merge(4, 9, 1, 1);
            sheet.Cells[4, 9].PutValue("");
            sheet.Cells[4, 9].SetStyle(cellStyle);

            sheet.Cells.Merge(4, 10, 1, 1);
            sheet.Cells[4, 10].PutValue("");
            sheet.Cells[4, 10].SetStyle(cellStyle);

            sheet.Cells.Merge(4, 11, 1, 1);
            sheet.Cells[4, 11].PutValue("");
            sheet.Cells[4, 11].SetStyle(cellStyle);
            #endregion

            sheet.Cells.Merge(5, 1, 1, 1);
            sheet.Cells[5, 1].PutValue("7-12");
            sheet.Cells[5, 1].SetStyle(cellStyle);
            #region 指令
            sheet.Cells.Merge(5, 2, 1, 1);
            sheet.Cells[5, 2].PutValue(Math.Round(SectionRatio2, 2));
            sheet.Cells[5, 2].SetStyle(cellStyle);

            sheet.Cells.Merge(5, 3, 1, 1);
            sheet.Cells[5, 3].PutValue(Math.Round(SectionRatio2 * instructTime, 2));
            sheet.Cells[5, 3].SetStyle(cellStyle);

            sheet.Cells.Merge(5, 4, 1, 1);
            sheet.Cells[5, 4].PutValue(Math.Round(SectionRatio2 * OA.GetCoalTotal(), 2));
            sheet.Cells[5, 4].SetStyle(cellStyle);

            sheet.Cells.Merge(5, 5, 1, 1);
            sheet.Cells[5, 5].PutValue(Math.Round(SectionRatio2 *OA.GetWaterTotal(), 2));
            sheet.Cells[5, 5].SetStyle(cellStyle);

            sheet.Cells.Merge(5, 6, 1, 1);
            sheet.Cells[5, 6].PutValue(Math.Round(SectionRatio2 * OA.GetEle(), 2));
            sheet.Cells[5, 6].SetStyle(cellStyle);

            sheet.Cells.Merge(5, 7, 1, 1);
            sheet.Cells[5, 7].PutValue(Math.Round(SectionRatio2 *OA.GetSalt(), 2));
            sheet.Cells[5, 7].SetStyle(cellStyle);

            sheet.Cells.Merge(5, 8, 1, 1);
            sheet.Cells[5, 8].PutValue(Math.Round(SectionRatio2 * OA.GetAlkali(), 2));
            sheet.Cells[5, 8].SetStyle(cellStyle);

            sheet.Cells.Merge(5, 9, 1, 1);
            sheet.Cells[5, 9].PutValue("");
            sheet.Cells[5, 9].SetStyle(cellStyle);

            sheet.Cells.Merge(5, 10, 1, 1);
            sheet.Cells[5, 10].PutValue("");
            sheet.Cells[5, 10].SetStyle(cellStyle);

            sheet.Cells.Merge(5, 11, 1, 1);
            sheet.Cells[5, 11].PutValue("");
            sheet.Cells[5, 11].SetStyle(cellStyle);
            #endregion

            sheet.Cells.Merge(6, 1, 1, 1);
            sheet.Cells[6, 1].PutValue("13-18");
            sheet.Cells[6, 1].SetStyle(cellStyle);
            #region 指令
            sheet.Cells.Merge(6, 2, 1, 1);
            sheet.Cells[6, 2].PutValue(Math.Round(SectionRatio3, 2));
            sheet.Cells[6, 2].SetStyle(cellStyle);

            sheet.Cells.Merge(6, 3, 1, 1);
            sheet.Cells[6, 3].PutValue(Math.Round(instructTime, 2) - Math.Round(SectionRatio1* instructTime, 2) - Math.Round(SectionRatio2 * instructTime, 2) - Math.Round(SectionRatio4 * instructTime, 2));
            sheet.Cells[6, 3].SetStyle(cellStyle);

            sheet.Cells.Merge(6, 4, 1, 1);
            sheet.Cells[6, 4].PutValue(Math.Round(OA.GetCoalTotal(), 2) - Math.Round(SectionRatio1 * OA.GetCoalTotal(), 2) - Math.Round(SectionRatio2 * OA.GetCoalTotal(), 2) - Math.Round(SectionRatio4 * OA.GetCoalTotal(), 2));
            sheet.Cells[6, 4].SetStyle(cellStyle);

            sheet.Cells.Merge(6, 5, 1, 1);
            sheet.Cells[6, 5].PutValue(Math.Round(OA.GetWaterTotal(), 2) - Math.Round(SectionRatio1 * OA.GetWaterTotal(), 2) - Math.Round(SectionRatio2 * OA.GetWaterTotal(), 2) - Math.Round(SectionRatio4 * OA.GetWaterTotal(), 2));
            sheet.Cells[6, 5].SetStyle(cellStyle);

            sheet.Cells.Merge(6, 6, 1, 1);
            sheet.Cells[6, 6].PutValue(Math.Round(OA.GetEle(), 2) - Math.Round(SectionRatio1 * OA.GetEle(), 2) - Math.Round(SectionRatio2 * OA.GetEle(), 2) - Math.Round(SectionRatio4 * OA.GetEle(), 2));
            sheet.Cells[6, 6].SetStyle(cellStyle);

            sheet.Cells.Merge(6, 7, 1, 1);
            sheet.Cells[6, 7].PutValue(Math.Round(OA.GetSalt(), 2) - Math.Round(SectionRatio1 * OA.GetSalt(), 2) - Math.Round(SectionRatio2 * OA.GetSalt(), 2) - Math.Round(SectionRatio4 * OA.GetSalt(), 2));
            sheet.Cells[6, 7].SetStyle(cellStyle);

            sheet.Cells.Merge(6, 8, 1, 1);
            sheet.Cells[6, 8].PutValue(Math.Round(OA.GetAlkali(), 2) - Math.Round(SectionRatio1 * OA.GetAlkali(), 2) - Math.Round(SectionRatio2 * OA.GetAlkali(), 2) - Math.Round(SectionRatio4 * OA.GetAlkali(), 2));
            sheet.Cells[6, 8].SetStyle(cellStyle);

            sheet.Cells.Merge(6, 9, 1, 1);
            sheet.Cells[6, 9].PutValue("");
            sheet.Cells[6, 9].SetStyle(cellStyle);

            sheet.Cells.Merge(6, 10, 1, 1);
            sheet.Cells[6, 10].PutValue("");
            sheet.Cells[6, 10].SetStyle(cellStyle);

            sheet.Cells.Merge(6, 11, 1, 1);
            sheet.Cells[6, 11].PutValue("");
            sheet.Cells[6, 11].SetStyle(cellStyle);
            #endregion

            sheet.Cells.Merge(7, 1, 1, 1);
            sheet.Cells[7, 1].PutValue("19-24");
            sheet.Cells[7, 1].SetStyle(cellStyle);
            #region 指令
            sheet.Cells.Merge(7, 2, 1, 1);
            sheet.Cells[7, 2].PutValue(Math.Round(SectionRatio4, 2));
            sheet.Cells[7, 2].SetStyle(cellStyle);

            sheet.Cells.Merge(7, 3, 1, 1);
            sheet.Cells[7, 3].PutValue(Math.Round(SectionRatio4 * instructTime, 2));
            sheet.Cells[7, 3].SetStyle(cellStyle);

            sheet.Cells.Merge(7, 4, 1, 1);
            sheet.Cells[7, 4].PutValue(Math.Round(SectionRatio4 * OA.GetCoalTotal(), 2));
            sheet.Cells[7, 4].SetStyle(cellStyle);

            sheet.Cells.Merge(7, 5, 1, 1);
            sheet.Cells[7, 5].PutValue(Math.Round(SectionRatio4 *OA.GetWaterTotal(), 2));
            sheet.Cells[7, 5].SetStyle(cellStyle);

            sheet.Cells.Merge(7, 6, 1, 1);
            sheet.Cells[7, 6].PutValue(Math.Round(SectionRatio4 * OA.GetEle(), 2));
            sheet.Cells[7, 6].SetStyle(cellStyle);

            sheet.Cells.Merge(7, 7, 1, 1);
            sheet.Cells[7, 7].PutValue(Math.Round(SectionRatio4 *OA.GetSalt(), 2));
            sheet.Cells[7, 7].SetStyle(cellStyle);

            sheet.Cells.Merge(7, 8, 1, 1);
            sheet.Cells[7, 8].PutValue(Math.Round(SectionRatio4 * OA.GetAlkali(), 2));
            sheet.Cells[7, 8].SetStyle(cellStyle);

            sheet.Cells.Merge(7, 9, 1, 1);
            sheet.Cells[7, 9].PutValue("");
            sheet.Cells[7, 9].SetStyle(cellStyle);

            sheet.Cells.Merge(7, 10, 1, 1);
            sheet.Cells[7, 10].PutValue("");
            sheet.Cells[7, 10].SetStyle(cellStyle);

            sheet.Cells.Merge(7, 11, 1, 1);
            sheet.Cells[7, 11].PutValue("");
            sheet.Cells[7, 11].SetStyle(cellStyle);
            #endregion
            #endregion

            #region 第9行 合     计
            sheet.Cells.Merge(8, 0, 1, 2);
            sheet.Cells[8, 0].PutValue("合     计");
            sheet.Cells[8, 0].SetStyle(cellStyle);
            sheet.Cells.SetRowHeight(8, rowHeight);
            sheet.Cells[8, 1].SetStyle(cellStyle);

            #region 指令
            sheet.Cells.Merge(8, 2, 1, 1);
            sheet.Cells[8, 2].PutValue("1.00");
            sheet.Cells[8, 2].SetStyle(cellStyle);

            //指令运行时间
            sheet.Cells.Merge(8, 3, 1, 1);
            sheet.Cells[8, 3].PutValue(Math.Round(instructTime, 2));
            sheet.Cells[8, 3].SetStyle(cellStyle);
            //指令耗煤
            sheet.Cells.Merge(8, 4, 1, 1);
            sheet.Cells[8, 4].PutValue(Math.Round(OA.GetCoalTotal(), 2));
            sheet.Cells[8, 4].SetStyle(cellStyle);
            //指令耗水
            sheet.Cells.Merge(8, 5, 1, 1);
            sheet.Cells[8, 5].PutValue(Math.Round(OA.GetWaterTotal(), 2));
            sheet.Cells[8, 5].SetStyle(cellStyle);
            //指令耗电
            sheet.Cells.Merge(8, 6, 1, 1);
            sheet.Cells[8, 6].PutValue(Math.Round(OA.GetEle(), 2));
            sheet.Cells[8, 6].SetStyle(cellStyle);
            //耗盐
            sheet.Cells.Merge(8, 7, 1, 1);
            sheet.Cells[8, 7].PutValue(Math.Round(OA.GetSalt(), 2));
            sheet.Cells[8, 7].SetStyle(cellStyle);
            //耗碱
            sheet.Cells.Merge(8, 8, 1, 1);
            sheet.Cells[8, 8].PutValue(Math.Round(OA.GetAlkali(), 2));
            sheet.Cells[8, 8].SetStyle(cellStyle);

            sheet.Cells.Merge(8, 9, 1, 1);
            sheet.Cells[8, 9].PutValue("");
            sheet.Cells[8, 9].SetStyle(cellStyle);

            sheet.Cells.Merge(8, 10, 1, 1);
            sheet.Cells[8, 10].PutValue("");
            sheet.Cells[8, 10].SetStyle(cellStyle);

            sheet.Cells.Merge(8, 11, 1, 1);
            sheet.Cells[8, 11].PutValue("");
            sheet.Cells[8, 11].SetStyle(cellStyle);
            #endregion
            #endregion

            #region 第10、11、12、13行 备注：
            sheet.Cells.Merge(9, 0, 4, 12);
            sheet.Cells.SetRowHeight(9, 14);
            sheet.Cells.SetRowHeight(10, 14);
            sheet.Cells.SetRowHeight(11, 14);
            sheet.Cells.SetRowHeight(12, 14);
            sheet.Cells[9, 0].PutValue("备注：");

            #endregion
            #region 第15行
            sheet.Cells.Merge(14, 5, 1, 7);
            sheet.Cells[14, 5].PutValue("制表人：张利霞                         负责人：");


            #endregion

            #region 高度/宽度
            int[] width = { 6, 12, 12, 12, 10, 11, 10, 10, 11, 11, 11, 11 };
            for (int w = 0; w < width.Length; w++)
            {
                sheet.Cells.SetColumnWidth(w, width[w]);
                sheet.Cells[1, w].Style.Font.IsBold = true;
                sheet.Cells[2, w].Style.Font.IsBold = true;
            }

            int[] height = { 36,33,26,24,24,24,24,24,25,13,13,13,13,14,14};
            for (int h = 0; h < height.Length; h++)
            {
                sheet.Cells.SetRowHeight(h, height[h]);
                sheet.Cells[h, 0].Style.Font.IsBold = true;
                sheet.Cells[h, 1].Style.Font.IsBold = true;
            }
            #endregion

            Aspose.Cells.Worksheet Station = CreateStationSheet(workbook,ID, dayRunTime, maxValue, minValue, aveTemp, SectionRatio, isToday);

            workbook.Save(directory);

        }

        private Worksheet CreateStationSheet(Workbook workbook,Guid ID, decimal dayRunTime, double maxValue, double minValue, double aveTemp, decimal[] sectionRatio, bool isToday)
        {
            decimal SectionRatio1 = sectionRatio[0];
            decimal SectionRatio2 = sectionRatio[1];
           // decimal SectionRatio3 = sectionRatio[2];
            decimal SectionRatio4 = sectionRatio[3];
            decimal SectionRatio3 = 1 - SectionRatio1 - SectionRatio2 - SectionRatio4;

            int rowHeight = 21;//行高
            string cellFontName = "宋体";//字体
            int cellFontSize = 10;//字体大小

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

            Aspose.Cells.Worksheet Station = workbook.Worksheets.Add("换热站");//工作表
            Station.Name = "换热站";

            string ShowDate = GetShowDate(isToday);

            string[] Col = { "站 名 ","  总计运行时间（h）","电（kw / h）","水（t）","	运行时间（h）","	电（kw / h）	","水（t）","	运行时间（h）","	电（kw / h）","	水（t）	","运行时间（h）","	电（kw / h）","	水（t）	","运行时间（h）","	电（kw / h）","	水（t）", maxValue.ToString(), minValue.ToString(), aveTemp.ToString()};

            string[,] Col1 = { {"1","1","日期"}, { "3", "2", "指令总耗量" }, { "12", "1", "区     间    分    配   量" }, { "1", "2", "最高温度（℃）" }, { "1", "2", "最低温度（℃）" }, { "1", "2", "平均温度（℃）" } };
            string[,] Col2 = { { "1", ShowDate }, { "3",""}, { "3", "1-6" }, { "3", "7-12" }, { "3", "13-18" }, { "3", "19-24" }, { "3", "" } };
            int[] widthSt = { 10, 7, 8, 7, 7, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 6, 6, 9 };

            #region 行，列
            int r = 0;//行
            int c = 0;//列
            #endregion

            #region 标题
            //标题
            Station.Cells.Merge(r, c, 1, widthSt.Length);
            Station.Cells[r,c].PutValue("换热站区间耗量分配表");
            Station.Cells[r,c].Style.Font.IsBold = true;
            Station.Cells[r,c].Style.Font.Size = 16;
            Station.Cells[r,c].Style.Font.Name = cellFontName;
            Station.Cells[r,c].Style.HorizontalAlignment = TextAlignmentType.Center;
            Station.Cells.SetRowHeight(r, 26);
            #endregion

            #region 第2、3行
            #region 第2行
            r++;
            c = 0;
            for (int C1 = 0; C1 < Col1.GetLength(0); C1++)
            {
                int L = int.Parse(Col1[C1,0]);
                int Lc = int.Parse(Col1[C1,1]);
                string V = Col1[C1, 2].Trim();
                if (L > 1)
                {
                    Station.Cells.Merge(r, c, Lc, L);
                    Station.Cells[r, c].PutValue(V);
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    for (int C2=0;C2 < L-1; C2++)
                    {
                        Station.Cells[r, c].SetStyle(cellStyle);
                        c++;
                    }
                }
                else
                {
                    Station.Cells.Merge(r, c, Lc, 1);
                    Station.Cells[r, c].PutValue(V);
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                }
            }
            Station.Cells.SetRowHeight(r, 20);
            #endregion

            #region 第3行
            r++;
            c = 0;
            for (int C2 = 0; C2 < Col2.GetLength(0); C2++)
            {
                int L = int.Parse(Col2[C2, 0]);
                string M = Col2[C2, 1];
                if (string.IsNullOrEmpty(M))
                {
                    for (int C3 = 0; C3 < L; C3++)
                    {
                        Station.Cells[r, c].SetStyle(cellStyle);
                        c++;
                    }
                }
                else
                {
                    Station.Cells.Merge(r, c, 1, L);
                    Station.Cells[r, c].PutValue(Col2[C2, 1]);
                    Station.Cells[r, c].SetStyle(cellStyle);

                    for (int C3 = 0; C3 < L; C3++)
                    {
                        c++;
                        Station.Cells[r, c].SetStyle(cellStyle);

                    }
                }
            }
            Station.Cells.SetRowHeight(r, 25);
            #endregion

            #endregion

            #region 第4、5行 站 名
            r++;
            c = 0;
            for (int C3 = 0; C3 < Col.Length; C3++)
            {
                Station.Cells.Merge(r, c, 1, 1);
                Station.Cells[r, c].PutValue(Col[C3].Trim());
                Station.Cells[r, c].SetStyle(cellStyle);
                c++;
            }
            Station.Cells.SetRowHeight(r, 35);
            r++;
            c = 0;
            //第5行
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue("所占比例（%）");
            Station.Cells[r, c].SetStyle(cellStyle);
            Station.Cells.SetRowHeight(4, 21);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue("1");
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue("1");
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue("1");
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue(SectionRatio1);
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue(SectionRatio1);
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue(SectionRatio1);
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue(SectionRatio2);
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue(SectionRatio2);
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue(SectionRatio2);
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue(SectionRatio3);
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue(SectionRatio3);
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue(SectionRatio3);
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue(SectionRatio4);
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue(SectionRatio4);
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].PutValue(SectionRatio4);
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].SetStyle(cellStyle);
            c++;
            Station.Cells.Merge(r, c, 1, 1);
            Station.Cells[r, c].SetStyle(cellStyle);
            #endregion

            #region 各站
            DataSet dataSetPushOrder = null;
            try
            {
                dataSetPushOrder = new DataAccessHandler().executeDatasetResult(
                    push_order_stat_select, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            r++;
            c = 0;
            //decimal hj_RATIO = Math.Round(dayRunTime * 1.510444444m, 2);
            //decimal hj_RATE = Math.Round(dayRunTime * 1.510444444m * 132.5509784m, 2);
            //decimal hj_FLOW = Math.Round(dayRunTime * 1.510444444m * 1.301064195m, 2);
            if (dataSetPushOrder != null && dataSetPushOrder.Tables.Count > 0)
            {
                decimal _Total_Time = 0m;
                decimal _Total_Ele = 0m;
                decimal _Total_Water = 0m;

                decimal _Total_Sec1_Time = 0m;
                decimal _Total_Sec1_Ele = 0m;
                decimal _Total_Sec1_Water = 0m;

                decimal _Total_Sec2_Time = 0m;
                decimal _Total_Sec2_Ele = 0m;
                decimal _Total_Sec2_Water = 0m;

                decimal _Total_Sec3_Time = 0m;
                decimal _Total_Sec3_Ele = 0m;
                decimal _Total_Sec3_Water = 0m;

                decimal _Total_Sec4_Time = 0m;
                decimal _Total_Sec4_Ele = 0m;
                decimal _Total_Sec4_Water = 0m;

                foreach (DataRow oldRow in dataSetPushOrder.Tables[0].Rows)
                {
                    decimal _ZL_WaterTotal = 0m;
                    decimal _ZL_Ele = 0m;
                    OrderAlgorithm OA = new OrderAlgorithm();
                    OA.MaxValue = maxValue;
                    OA.MinValue = minValue;
                    OA.Area = Decimal.Parse(oldRow["Area"].ToString());
                    OA.Target = int.Parse(oldRow["Target"].ToString());//热负荷
                    OA.Power = Decimal.Parse(oldRow["CYCLEPOWER"].ToString());
                    OA.CYCLEFLOW = Decimal.Parse(oldRow["CYCLEFLOW"].ToString());
                    OA.Efficiency = Decimal.Parse(oldRow["Efficiency"].ToString());
                    OA.Power = Decimal.Parse(oldRow["CYCLEPOWER"].ToString());
                    OA.Ratio= Decimal.Parse(oldRow["Ratio"].ToString());
                    _ZL_WaterTotal = OA.GetWater2();
                    dayRunTime = OA.GetStationRunDate();
                    _ZL_Ele = OA.GetStationEle();

                    _Total_Time += dayRunTime;
                    _Total_Ele += _ZL_Ele;
                    _Total_Water += _ZL_WaterTotal;


                    if (oldRow["NAME"].ToString().Equals("锅炉循环泵"))
                    {
                        _ZL_WaterTotal = OA.GetWaterTotal();
                        dayRunTime = OA.GetPumpDate(_ZL_WaterTotal);
                        _ZL_Ele = dayRunTime * OA.Power;
                    }

                    #region 保存数据

                    KT_RunCommandObject ob = new KT_RunCommandObject();

                    ob.ID = Guid.NewGuid();
                    ob.PUSHID = ID;
                    ob.RUNDATE = isToday ? DateTime.Now : DateTime.Now.AddDays(1);
                    ob.STATIONID =new Guid(oldRow["ID"].ToString());
                    ob.STATEIONNAME = oldRow["NAME"].ToString();
                    ob.ADDDATETIME = DateTime.Now;
                    ob.MAXVALUE =Convert.ToDecimal(maxValue);
                    ob.MINVALUE = Convert.ToDecimal(minValue);
                    ob.ZL_TIME = dayRunTime;
                    ob.ZL_ELE = _ZL_Ele;
                    ob.ZL_WATER = _ZL_WaterTotal;
                    ob.Time_Ratio = string.Format("{0}:{1}:{2}:{3}", SectionRatio1, SectionRatio2, SectionRatio3, SectionRatio4);

                    new KT_RunCommand().Add(ob);

                    #endregion

                    c = 0;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(oldRow["NAME"].ToString());
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(Math.Round(dayRunTime , 2));
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(Math.Round(_ZL_Ele, 2));
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(Math.Round(_ZL_WaterTotal, 2));
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(Math.Round(dayRunTime * SectionRatio1, 2));
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(Math.Round(_ZL_Ele * SectionRatio1, 2));
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(Math.Round(_ZL_WaterTotal* SectionRatio1, 2));
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(Math.Round(dayRunTime * SectionRatio2, 2));
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(Math.Round(_ZL_Ele * SectionRatio2, 2));
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(Math.Round(_ZL_WaterTotal* SectionRatio2, 2));
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(Math.Round(dayRunTime * SectionRatio3, 2));
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(Math.Round(_ZL_Ele * SectionRatio3, 2));
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(Math.Round(_ZL_WaterTotal* SectionRatio3, 2));
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(Math.Round(dayRunTime * SectionRatio4, 2));
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(Math.Round(_ZL_Ele * SectionRatio4, 2));
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].PutValue(Math.Round(_ZL_WaterTotal* SectionRatio4, 2));
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].SetStyle(cellStyle);
                    c++;
                    Station.Cells.Merge(r, c, 1, 1);
                    Station.Cells[r, c].SetStyle(cellStyle);
                    r++;
                }
            }
            #endregion

            #region

            #endregion

            #region 制表人
            c = 0;
            Station.Cells.Merge(r, c, 1, widthSt.Length);
            Station.Cells[r, c].PutValue("                                           制表人：张利霞                                     负责人：");
            for (int w = 0; w < widthSt.Length; w++)
            {
                Station.Cells[r, w].SetStyle(cellStyle);
            }
            Station.Cells.SetRowHeight(r, 15);
            #endregion

            for (int w = 0; w < widthSt.Length; w++)
            {
                Station.Cells.SetColumnWidth(w, widthSt[w]);
                Station.Cells[1, w].Style.Font.IsBold = true;
                Station.Cells[2, w].Style.Font.IsBold = true;
                Station.Cells[3, w].Style.Font.IsBold = true;
            }
            for (int h = 4; h < r; h++)
            {
                Station.Cells.SetRowHeight(h, 21);
            }
            return Station;
        }

        private string GetShowDate(bool isToday)
        {
            return isToday ? DateTime.Now.ToString("MM.dd") : DateTime.Now.AddDays(1).ToString("MM.dd");
        }

        /// <summary>
        /// 返回四舍五入的数值。
        /// 1、如果为NULL，则返回NULL；
        /// 2、如果转换数值失败，则返回最初的d；
        /// 3、d转换为double数值后，进行四舍五入。
        /// </summary>
        /// <param name="d">需要进行四舍五入的值</param>
        /// <param name="digits">舍去的位数</param>
        /// <returns>返回四舍五入的数值</returns>
        private object round(string d, int digits = 2)
        {
            if (string.IsNullOrEmpty(d))
                return string.Empty;

            double v;
            if (Double.TryParse(d, out v))
            {
                v = Math.Round(v, digits);

                return v;
            }
            else
            {
                return d;
            }
        }

        //返回插入的ID
        public string createPushOrder(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Result.getFaultXml(Error.XML_IS_NULL);

            //待插入的编号
            string GUID = Guid.NewGuid().ToString().ToUpper();
            //text = text.Replace("{@ID}", GUID);

            XElement xml = null;
            try
            {
                xml = XElement.Parse(text);
            }
            catch
            {
                return Result.getFaultXml(Error.XML_FORMAT_ERROR);
            }

            string resut = string.Empty;
            //try
            //{
            //    ZY_PushOrderObject zy_pushorderObject = new ZY_PushOrderObject(xml);

            //resut = new DataAccessHandler().executeNonQueryResult(
            //    //    string.Format(push_order_insert,
            //    //    zy_pushorderObject.ID,
            //    //    zy_pushorderObject.RUNDATE,
            //    //    zy_pushorderObject.MAXVALUE,
            //    //    zy_pushorderObject.MINVALUE,
            //    //    zy_pushorderObject.EXPORTTYPE,
            //    //    zy_pushorderObject.FILENAME,
            //    //    zy_pushorderObject.FILEURL,
            //    //    zy_pushorderObject.NOTE
            //    //    ), null);

            //    zy_pushorderObject = null;
            //}
            //catch (Exception ex)
            //{
            //    return Result.getFaultXml(ex.Message);
            //}

            //if (Result.isSuccess(resut))
            //{
                resut = Result.getResultXml(getCreatePushOrderXml(ref GUID));
            //}

            text = null;
            xml = null;
            GUID = null;

            return resut;
        }

        private string getCreatePushOrderXml(ref string pushOrderID)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            xml.AppendFormat("<DATA PUSHORDERID=\"{0}\"/>", pushOrderID);
            xml.Append("</DATAS>");

            return xml.ToString();
        }

        public string sendPushOrder(string text)
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

            string pushID = xml.Element("PUSHID").Value;
            string pushOrderID = xml.Element("PUSHORDERID").Value;

            DataSet dataSetPushOrder = null;
            try
            {
                dataSetPushOrder = new DataAccessHandler().executeDatasetResult(
                    push_order_details,
                    SqlServer.GetParameter("PUSHORDERID", pushOrderID));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }


            ZY_PushOrderObject data = null;
            if (dataSetPushOrder != null && dataSetPushOrder.Tables.Count > 0)
            {
                createPushOrderObject(ref data, dataSetPushOrder.Tables[0].Rows[0]);
            }

            if (data == null)
                return Result.getFaultXml(Error.RESULT_ERROR);

            string GUID = Guid.NewGuid().ToString().ToUpper();

            string EmployeeID = "C1359ACA-10DF-4338-8D62-66F22724B647";

            string result = string.Empty;
            try
            {
                result = new DataAccessHandler().executeNonQueryResult(
                    string.Format(send_push_order_insert,
                    GUID,
                    pushID,
                    DateTime.Now.ToString(),
                    //data.RUNDATE,
                    data.FILEURL,
                    data.FILENAME,
                    data.NOTE
                    ), null);

                //谁来接收该文件？
                result = new DataAccessHandler().executeNonQueryResult(
                    string.Format(receive_push_order_insert, GUID, EmployeeID),
                    null);

            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = null;
            xml = null;
            dataSetPushOrder = null;
            data = null;

            return result;
        }

        private void createPushOrderObject(ref ZY_PushOrderObject data, DataRow dataRow)
        {
            if (dataRow == null)
                return;

            if (data == null)
                data = new ZY_PushOrderObject();

            data.RUNDATE = dataRow["RUNDATE"].ToString();
            data.FILENAME = dataRow["FILENAME"].ToString();
            data.FILEURL = dataRow["FILEURL"].ToString().Replace(Config.UploadExportFileHttpUrl, "").Replace("/", "");
            data.NOTE = dataRow["NOTE"].ToString();
        }

        public string deleteDataItem(string text)
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

            string result = string.Empty;

            DataSet dataSetUse = null;
            DataSet dataSetDelete = null;

            List<string> deleteZipFileItems = new List<string>(); //要被删除的zip文件
            try
            {
                //依次检查每个ID生成的zip文件是否被占用了，如果占用了，则不删除zip文件，否则删除记录的同时也要删除zip文件
                //1 获取被占用了的记录
                dataSetUse = new DataAccessHandler().executeDatasetResult(
                    string.Format(push_order_use_file, xml.Element("ID").Value),
                    null);

                dataSetDelete = new DataAccessHandler().executeDatasetResult(
                    string.Format(push_order_delete_select, xml.Element("ID").Value), null);

                DataRow[] dataRows = null;
                foreach (DataRow row in dataSetDelete.Tables[0].Rows)
                {
                    dataRows = dataSetUse.Tables[0].Select(string.Format("FILEURL=\'{0}\'", row["FILEURL"].ToString()));
                    if (dataRows.Length == 0)
                    {
                        deleteZipFileItems.Add(row["FILEURL"].ToString());
                    }
                }

                result = new DataAccessHandler().executeNonQueryResult(
                    string.Format(push_order_delete, xml.Element("ID").Value),
                    null);

                //删除文件
                foreach (string item in deleteZipFileItems)
                {
                    File.Delete(Path.Combine(Config.UploadExportFileDirectory, item));
                }
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = null;
            xml = null;

            return result;
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

            string whereText = string.IsNullOrEmpty(xml.Element("WHERE").Value)
                ? string.Empty
                : string.Format(" WHERE {0}", xml.Element("WHERE").Value);
            string commandText = string.Format(push_order_select, whereText);

            DataSet dataSetPushOrder = null;
            try
            {
                dataSetPushOrder = new DataAccessHandler().executeDatasetResult(
                    SqlServerCommandText.createPagingCommandText(ref commandText, ref xml), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetPushOrder != null && dataSetPushOrder.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemXml(ref dataSetPushOrder, getDataItemTotal(ref whereText)));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = whereText = commandText = null;
            xml = null;
            dataSetPushOrder = null;

            return result;
        }

        private string getDataItemXml(ref DataSet dataSetPushOrder, string total)
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendFormat("<DATAS COUNT=\"{0}\" TOTAL=\"{1}\">", dataSetPushOrder.Tables[0].Rows.Count, total);
            ZY_PushOrderObject zy_pushOrderObject = null;
            foreach (DataRow row in dataSetPushOrder.Tables[0].Rows)
            {
                zy_pushOrderObject = new ZY_PushOrderObject(row);
                xml.AppendFormat("<DATA NUM=\"{0}\" ID=\"{1}\" RUNDATE=\"{2}\" MAXVALUE=\"{3}\" MINVALUE=\"{4}\" EXPORTTYPE=\"{5}\" FILENAME=\"{6}\" NOTE=\"{7}\"/>",
                    zy_pushOrderObject.NUM,
                    zy_pushOrderObject.ID,
                    zy_pushOrderObject.RUNDATE,
                    zy_pushOrderObject.MAXVALUE,
                    zy_pushOrderObject.MINVALUE,
                    zy_pushOrderObject.EXPORTTYPE,
                    zy_pushOrderObject.FILENAME,
                    zy_pushOrderObject.NOTE
                );
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }

        private string getDataItemTotal(ref string whereText)
        {
            return new DataAccessHandler().executeScalarResult(
                string.Format(push_order_total, whereText), null);
        }

        public string getDataItemDetails(string text)
        {
            throw new NotImplementedException();
        }

        public string downPushOrder(string text)
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

            string pushOrderID = xml.Element("ID").Value;
            if (pushOrderID.IndexOf(',') == -1)
            {
                pushOrderID = pushOrderID.Replace("'", "");

                pushOrderID = string.Format("'{0}'", pushOrderID);
            }


            DataSet dataSetPushOrder = null;
            try
            {
                dataSetPushOrder = new DataAccessHandler().executeDatasetResult(
                    string.Format(push_order_down_select, pushOrderID), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string GUID = string.Empty;
            if (pushOrderID.Split(',').Length > 1)
            {
                GUID = Guid.NewGuid().ToString().ToUpper();

                Directory.CreateDirectory(Path.Combine(Config.UploadExportFileDirectory, GUID));


                int fileFlag = 1;//用来区别两个名字一致的情况

                //依次将文件复制进新的目录下，并使用FileName命名。如果出现重名，则使用（1）来标识

                string fileName = string.Empty;
                foreach (DataRow row in dataSetPushOrder.Tables[0].Rows)
                {
                    fileName = row["FILENAME"].ToString();
                CHECK_FILE_NAME:
                    if (File.Exists(Path.Combine(Path.Combine(Config.UploadExportFileDirectory, GUID), fileName)))
                    {
                        fileName = string.Format("{0} ({1}).zip", fileName.Replace(".zip", ""), fileFlag);
                        fileFlag++;
                        goto CHECK_FILE_NAME;
                    }

                    File.Copy(Path.Combine(
                        Config.UploadExportFileDirectory, row["FILEURL"].ToString()),
                        Path.Combine(Path.Combine(Config.UploadExportFileDirectory, GUID), fileName));
                }

                //压缩GUID文件夹
                Zip.CompressionDirectory(
                    Path.Combine(Config.UploadExportFileDirectory, GUID),
                    string.Format("{0}/{1}.zip", Config.UploadExportFileDirectory, GUID),
                    0);

                //删除GUID文件夹
                Directory.Delete(Path.Combine(Config.UploadExportFileDirectory, GUID), true);

                fileName = null;
            }


            StringBuilder result = new StringBuilder();
            result.Append("<DATAS>");
            result.AppendFormat("<DATA FILENAME=\"{0}\" FILEDOWNURL=\"{1}\"/>",
                string.IsNullOrEmpty(GUID) ?
                dataSetPushOrder.Tables[0].Rows[0]["FILENAME"].ToString() :
                string.Format("{0}+{1}.zip", dataSetPushOrder.Tables[0].Rows[0]["FILENAME"].ToString().Replace(".zip", ""), dataSetPushOrder.Tables[0].Rows.Count),
                string.IsNullOrEmpty(GUID) ?
                string.Format("{0}/{1}", Config.UploadExportFileHttpUrl, dataSetPushOrder.Tables[0].Rows[0]["FILEURL"].ToString()) :
                string.Format("{0}/{1}.zip", Config.UploadExportFileHttpUrl, GUID));
            result.Append("</DATAS>");

            text = null;
            xml = null;
            dataSetPushOrder = null;

            return Result.getResultXml(result.ToString());
        }

        public string deleteDownZipFile(string text)
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

            string fileName = xml.Element("FILEURL").Value.Replace(Config.UploadExportFileHttpUrl, "").Replace("/", "");

            if (File.Exists(Path.Combine(Config.UploadExportFileDirectory, fileName)))
            {
                File.Delete(Path.Combine(Config.UploadExportFileDirectory, fileName));
            }

            text = fileName = null;
            xml = null;

            return Result.getResultXml(string.Empty);
        }



    }
}
