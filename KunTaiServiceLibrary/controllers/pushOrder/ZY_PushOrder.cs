using Aspose.Cells;
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
    [FluorineFx.RemotingService("招远金城热力公司的运行指令服务")]
    public class ZY_PushOrder : IController
    {

        #region command texts


        private const string push_order_stat_select = @"SELECT APLAN.NAME 方案名称, ITEM.[TYPE] 回路, ITEM.GROUPID 组,
A.[STCD] STCD, A.[NAME] 二级站名,
B.[T_CODE] T_CODE, B.[ST_NM] 三级站名,B.[AREA] 面积, B.[POWER] 功率, 
B.[NAMEPLATEFLOW] 铭牌流量, B.[EFFICIENCY] 效率, B.[TYPE] 类型, B.[SPEED] 流速, B.[FREQUENCY] 变频,
B.[FLOW] 流量实测, NULL 流量指令, 
NULL 供水温度, NULL 回水温度, NULL 实测温差, NULL 指令温差,
B.[HOTTYPE] 供热方式,
NULL 最高温度, NULL 最低温度, NULL 平均温度, NULL 日运行时间, NULL 总热量,
NULL STARTTIME0, NULL RUNTIME0, NULL POWER0,
NULL STARTTIME4, NULL RUNTIME4, NULL POWER4,
NULL STARTTIME8, NULL RUNTIME8, NULL POWER8,
NULL STARTTIME12, NULL RUNTIME12, NULL POWER12,
NULL STARTTIME16, NULL RUNTIME16, NULL POWER16,
NULL STARTTIME20, NULL RUNTIME20, NULL POWER20
FROM [ZY_ASSIGNPLANITEM] ITEM, [ZY_ASSIGNPLANTYPE] ATYPE, [ZY_ASSIGNPLAN] APLAN, [ZY_WR_STAT_A] A, [ZY_WR_STAT_B] B 
WHERE ITEM.[PID]=@ASSIGNPLANID AND ITEM.[TYPE]=ATYPE.[NAME]
AND ITEM.[PID]=APLAN.[ID] AND ITEM.[STATAID]=A.[ID] AND B.[STCD]=A.[STCD]
AND B.ISCALCULATE=1
ORDER BY ATYPE.[SHOWID], ITEM.[SHOWID], B.[T_CODE]";


        private const string push_order_insert = "INSERT INTO [ZY_PUSHORDER] ([ID], [RUNDATE], [MAXVALUE], [MINVALUE], [EXPORTTYPE], [FILENAME], [FILEURL], [NOTE]) VALUES ('{0}', '{1}', {2}, {3}, '{4}', '{5}', '{6}', '{7}')";


        private const string send_push_order_insert = "INSERT INTO [PUSHORDER] ([ID],[PUSHID], [RUNDATETIME], [FILEURL], [LOCALFILENAME], [NOTE]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')";

        private const string push_order_details = "SELECT [RUNDATE], [FILENAME], [FILEURL], [NOTE] FROM [ZY_PUSHORDER] WHERE [ID]=@PUSHORDERID";

        private const string receive_push_order_insert = "INSERT INTO [PUSHORDERRECEIVE] ([POID], [RECEIVEID]) VALUES ('{0}', '{1}')";

        private const string push_order_select = "SELECT ROW_NUMBER() OVER (ORDER BY [ADDDATETIME]) AS NUM1, [ID], [RUNDATE], [MAXVALUE], [MINVALUE], [EXPORTTYPE], [FILENAME], [NOTE] FROM [ZY_PUSHORDER]{0}";

        private const string push_order_total = "SELECT COUNT(*) FROM [ZY_PUSHORDER]{0}";

        private const string push_order_delete = "DELETE [ZY_PUSHORDER] WHERE [ID] IN ({0})";

        private const string push_order_use_file = "SELECT [FILEURL] FROM [PUSHORDER] WHERE [FILEURL] IN (SELECT [FILEURL] FROM [ZY_PUSHORDER] WHERE [ID] IN ({0}))";

        private const string push_order_delete_select = "SELECT [ID], [FILEURL] FROM [ZY_PUSHORDER] WHERE [ID] IN ({0})";

        private const string push_order_down_select = "SELECT [ID], [FILENAME], [FILEURL] FROM [ZY_PUSHORDER] WHERE [ID] IN ({0})";



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



            #region 1.解析各个参数值

            //1.1 最高、最低的温度值
            double maxValue = Convert.ToDouble(xml.Element("MAXVALUE").Value);//最大温度值
            double minValue = Convert.ToDouble(xml.Element("MINVALUE").Value);//最小温度值
            //1.2 各个时间段的区间运行百分比
            List<ZY_PushOrderWeatherObject> weatherItems = new List<ZY_PushOrderWeatherObject>();
            ZY_PushOrderWeatherObject zy_pushorderweatherObject = null;
            foreach (XElement item in xml.Element("WEATHER").Elements("DATA"))
            {
                zy_pushorderweatherObject = new ZY_PushOrderWeatherObject(item);
                weatherItems.Add(zy_pushorderweatherObject);

                zy_pushorderweatherObject = null;
            }
            //1.3 需要导出的分配方案
            List<ZY_AssignPlanObject> assignPlanItems = new List<ZY_AssignPlanObject>();
            ZY_AssignPlanObject zy_assignplanObject = null;
            foreach (var item in xml.Element("ASSIGNPLAN").Elements("DATA"))
            {
                zy_assignplanObject = new ZY_AssignPlanObject(item);
                assignPlanItems.Add(zy_assignplanObject);

                zy_assignplanObject = null;
            }

            #endregion

            #region 2.初始化基本参数和初始化文件放置的目录


            //2.1 导出方案存放的文件夹（打包zip后，会自动删除这个文件夹）
            string excelFileDirectoryName = Guid.NewGuid().ToString().ToUpper();
            string directory = Path.Combine(Config.UploadExportFileDirectory, excelFileDirectoryName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            #endregion


            //3.根据选择的方案依次生成excel文件

            //3.1 依次生成Excel文件
            foreach (ZY_AssignPlanObject item in assignPlanItems)
            {
                exportPushOrderFile(item, ref maxValue, ref minValue, ref weatherItems, ref directory);
            }


            //3.2 判断导出文件的数量是否跟List的数量一致，如果一致，打包zip，删除原文件夹，返回客户端
            string[] tempFiles = Directory.GetFiles(directory);
            string zipFilePath = string.Empty;
            if (tempFiles.Length == assignPlanItems.Count)
            {
                zipFilePath = string.Format("{0}.zip", directory);

                Zip.CompressionDirectory(
                    directory,
                    zipFilePath,
                    0);

                Directory.Delete(directory, true);
            }


            //3.3 返回客户端
            string result = string.Empty;
            if (!string.IsNullOrEmpty(zipFilePath))
            {
                result = Result.getResultXml(getAddDataItemXml(ref excelFileDirectoryName));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = null;
            xml = null;
            weatherItems = null;
            assignPlanItems = null;
            excelFileDirectoryName = null;
            tempFiles = null;
            zipFilePath = null;


            return result;
        }

        private string getAddDataItemXml(ref string excelFileDirectoryName)
        {
            StringBuilder xml = new StringBuilder();
            string runDateTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            xml.Append("<DATAS>");
            xml.AppendFormat("<DATA FILENAME=\"{0}\" FILEDOWNURL=\"{1}\" RUNDATETIME=\"{2}\"/>",
                string.Format("{0}，金诚热力公司三级泵运行指令.zip", runDateTime),
                 string.Format("{0}/{1}.zip", Config.UploadExportFileHttpUrl, excelFileDirectoryName),
                 runDateTime
            );
            xml.Append("</DATAS>");

            return xml.ToString();
        }

        /// <summary>
        /// 导出分配方案中的运行指令
        /// </summary>
        /// <param name="item">分配方案对象</param>
        /// <param name="maxValue">最高温度</param>
        /// <param name="minValue">最低温度</param>
        /// <param name="weatherItems">区间时间百分比</param>
        public void exportPushOrderFile(ZY_AssignPlanObject item,
            ref double maxValue,
            ref double minValue,
            ref List<ZY_PushOrderWeatherObject> weatherItems,
            ref string directory)
        {
            // excel标题和运行指令的日期
            string runDateTime = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
            //金诚热力公司2015-2016年度三级泵4启1停方案（2016-01-25）
            string titleName = string.Format("金诚热力公司{0}年度三级泵{1}方案（{2}）", "2015-2016", item.NAME, runDateTime);

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


            int rowHeight = 15;//行高
            string cellFontName = "宋体";//字体
            int cellFontSize = 10;//字体大小

            string fileName = item.NAME;

            //1 初始化组件变量
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();//工作簿
            Aspose.Cells.Worksheet sheet = workbook.Worksheets[0];//工作表

            sheet.Name = fileName;//Sheet名字


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


            //2 生成表头

            #region 标题
            //标题
            sheet.Cells.Merge(0, 0, 1, 41);
            sheet.Cells[0, 0].PutValue(titleName);
            sheet.Cells[0, 0].Style.Font.IsBold = true;
            sheet.Cells[0, 0].Style.Font.Size = 18;
            sheet.Cells[0, 0].Style.Font.Name = cellFontName;
            sheet.Cells[0, 0].Style.HorizontalAlignment = TextAlignmentType.Center;
            sheet.Cells.SetRowHeight(0, 27);
            #endregion


            #region 回路
            //回路
            sheet.Cells.Merge(1, 0, 3, 1);
            sheet.Cells[1, 0].PutValue("回路");
            sheet.Cells[1, 0].SetStyle(cellStyle);
            sheet.Cells[2, 0].SetStyle(cellStyle);
            sheet.Cells[3, 0].SetStyle(cellStyle);

            //设置这3行的高度，后面的就不用再设置了。
            sheet.Cells.SetRowHeight(1, rowHeight);
            sheet.Cells.SetRowHeight(2, rowHeight);
            sheet.Cells.SetRowHeight(3, rowHeight * 2);
            #endregion

            #region 序号
            //序号
            sheet.Cells.Merge(1, 1, 3, 1);
            sheet.Cells[1, 1].PutValue("序号");
            sheet.Cells[1, 1].SetStyle(cellStyle);
            sheet.Cells[2, 1].SetStyle(cellStyle);
            sheet.Cells[3, 1].SetStyle(cellStyle);
            #endregion

            #region 二级站名
            //二级站名
            sheet.Cells.Merge(1, 2, 3, 1);
            sheet.Cells[1, 2].PutValue("二级站名");
            sheet.Cells[1, 2].SetStyle(cellStyle);
            sheet.Cells[2, 2].SetStyle(cellStyle);
            sheet.Cells[3, 2].SetStyle(cellStyle);
            #endregion

            #region 三级站名
            //三级站名
            sheet.Cells.Merge(1, 3, 3, 1);
            sheet.Cells[1, 3].PutValue("三级站名");
            sheet.Cells[1, 3].SetStyle(cellStyle);
            sheet.Cells[2, 3].SetStyle(cellStyle);
            sheet.Cells[3, 3].SetStyle(cellStyle);
            #endregion

            #region 面积
            //面积
            sheet.Cells.Merge(1, 4, 3, 1);
            sheet.Cells[1, 4].PutValue("面积");
            sheet.Cells[1, 4].SetStyle(cellStyle);
            sheet.Cells[2, 4].SetStyle(cellStyle);
            sheet.Cells[3, 4].SetStyle(cellStyle);
            #endregion

            #region 功率
            //功率
            sheet.Cells.Merge(1, 5, 3, 1);
            sheet.Cells[1, 5].PutValue("功率");
            sheet.Cells[1, 5].SetStyle(cellStyle);
            sheet.Cells[2, 5].SetStyle(cellStyle);
            sheet.Cells[3, 5].SetStyle(cellStyle);
            #endregion

            #region 铭牌流量
            //铭牌流量
            sheet.Cells.Merge(1, 6, 3, 1);
            sheet.Cells[1, 6].PutValue("铭牌流量");
            sheet.Cells[1, 6].SetStyle(cellStyle);
            sheet.Cells[2, 6].SetStyle(cellStyle);
            sheet.Cells[3, 6].SetStyle(cellStyle);
            #endregion

            #region 效率
            //效率
            sheet.Cells.Merge(1, 7, 3, 1);
            sheet.Cells[1, 7].PutValue("效率");
            sheet.Cells[1, 7].SetStyle(cellStyle);
            sheet.Cells[2, 7].SetStyle(cellStyle);
            sheet.Cells[3, 7].SetStyle(cellStyle);
            #endregion

            #region 类型
            //类型
            sheet.Cells.Merge(1, 8, 3, 1);
            sheet.Cells[1, 8].PutValue("类型");
            sheet.Cells[1, 8].SetStyle(cellStyle);
            sheet.Cells[2, 8].SetStyle(cellStyle);
            sheet.Cells[3, 8].SetStyle(cellStyle);
            #endregion

            #region 流速
            //流速
            sheet.Cells.Merge(1, 9, 3, 1);
            sheet.Cells[1, 9].PutValue("流速");
            sheet.Cells[1, 9].SetStyle(cellStyle);
            sheet.Cells[2, 9].SetStyle(cellStyle);
            sheet.Cells[3, 9].SetStyle(cellStyle);
            #endregion

            #region 变频
            //变频
            sheet.Cells.Merge(1, 10, 3, 1);
            sheet.Cells[1, 10].PutValue("变频");
            sheet.Cells[1, 10].SetStyle(cellStyle);
            sheet.Cells[2, 10].SetStyle(cellStyle);
            sheet.Cells[3, 10].SetStyle(cellStyle);
            #endregion


            #region 流量
            //流量
            sheet.Cells.Merge(1, 11, 2, 2);
            sheet.Cells[1, 11].PutValue("流量");
            sheet.Cells[1, 11].SetStyle(cellStyle);
            sheet.Cells[2, 11].SetStyle(cellStyle);
            sheet.Cells[1, 12].SetStyle(cellStyle);
            sheet.Cells[2, 12].SetStyle(cellStyle);

            #endregion

            #region 流量 - 实测
            //实测
            sheet.Cells[3, 11].PutValue("实测");
            sheet.Cells[3, 11].SetStyle(cellStyle);
            #endregion


            #region 流量 - 指令
            //指令
            sheet.Cells[3, 12].PutValue("指令");
            sheet.Cells[3, 12].SetStyle(cellStyle);
            #endregion

            #region 温差
            //温差
            sheet.Cells.Merge(1, 13, 2, 4);
            sheet.Cells[1, 13].PutValue("温差");
            sheet.Cells[1, 13].SetStyle(cellStyle);
            sheet.Cells[2, 13].SetStyle(cellStyle);
            sheet.Cells[1, 14].SetStyle(cellStyle);
            sheet.Cells[2, 14].SetStyle(cellStyle);
            sheet.Cells[1, 15].SetStyle(cellStyle);
            sheet.Cells[2, 15].SetStyle(cellStyle);
            sheet.Cells[1, 16].SetStyle(cellStyle);
            sheet.Cells[2, 16].SetStyle(cellStyle);

            #endregion

            #region 温差 - 供水温度
            //供水温度
            sheet.Cells[3, 13].PutValue("供水温度");
            sheet.Cells[3, 13].SetStyle(cellStyle);
            #endregion

            #region 温差 - 回水温度
            //回水温度
            sheet.Cells[3, 14].PutValue("回水温度");
            sheet.Cells[3, 14].SetStyle(cellStyle);
            #endregion

            #region 温差 - 实测温差
            //实测温差
            sheet.Cells[3, 15].PutValue("实测温差");
            sheet.Cells[3, 15].SetStyle(cellStyle);

            #endregion

            #region 温差 - 指令温差
            //指令温差
            sheet.Cells[3, 16].PutValue("指令温差");
            sheet.Cells[3, 16].SetStyle(cellStyle);


            #endregion


            #region 供热方式
            //供热方式
            sheet.Cells.Merge(1, 17, 3, 1);
            sheet.Cells[1, 17].PutValue("供热方式");
            sheet.Cells[1, 17].SetStyle(cellStyle);
            sheet.Cells[2, 17].SetStyle(cellStyle);
            sheet.Cells[3, 17].SetStyle(cellStyle);
            #endregion

            #region 最高温度
            //最高温度
            sheet.Cells.Merge(1, 18, 3, 1);
            sheet.Cells[1, 18].PutValue("最高温度");
            sheet.Cells[1, 18].SetStyle(cellStyle);
            sheet.Cells[2, 18].SetStyle(cellStyle);
            sheet.Cells[3, 18].SetStyle(cellStyle);
            #endregion

            #region 最低温度
            //最低温度
            sheet.Cells.Merge(1, 19, 3, 1);
            sheet.Cells[1, 19].PutValue("最低温度");
            sheet.Cells[1, 19].SetStyle(cellStyle);
            sheet.Cells[2, 19].SetStyle(cellStyle);
            sheet.Cells[3, 19].SetStyle(cellStyle);
            #endregion

            #region 平均温度
            //平均温度
            sheet.Cells.Merge(1, 20, 3, 1);
            sheet.Cells[1, 20].PutValue("平均温度");
            sheet.Cells[1, 20].SetStyle(cellStyle);
            sheet.Cells[2, 20].SetStyle(cellStyle);
            sheet.Cells[3, 20].SetStyle(cellStyle);
            #endregion

            #region 日运行时间
            //日运行时间
            sheet.Cells.Merge(1, 21, 3, 1);
            sheet.Cells[1, 21].PutValue("日运行时间");
            sheet.Cells[1, 21].SetStyle(cellStyle);
            sheet.Cells[2, 21].SetStyle(cellStyle);
            sheet.Cells[3, 21].SetStyle(cellStyle);
            #endregion

            #region 总热量（GJ）
            //总热量（GJ）
            sheet.Cells.Merge(1, 22, 3, 1);
            sheet.Cells[1, 22].PutValue("总热量（GJ）");
            sheet.Cells[1, 22].SetStyle(cellStyle);
            sheet.Cells[2, 22].SetStyle(cellStyle);
            sheet.Cells[3, 22].SetStyle(cellStyle);
            #endregion



            #region 区间
            //区间
            sheet.Cells.Merge(1, 23, 1, 18);
            sheet.Cells[1, 23].PutValue("区间");
            sheet.Cells[1, 23].SetStyle(cellStyle);
            sheet.Cells[1, 24].SetStyle(cellStyle);
            sheet.Cells[1, 25].SetStyle(cellStyle);
            sheet.Cells[1, 26].SetStyle(cellStyle);
            sheet.Cells[1, 27].SetStyle(cellStyle);
            sheet.Cells[1, 28].SetStyle(cellStyle);
            sheet.Cells[1, 29].SetStyle(cellStyle);
            sheet.Cells[1, 30].SetStyle(cellStyle);
            sheet.Cells[1, 31].SetStyle(cellStyle);
            sheet.Cells[1, 32].SetStyle(cellStyle);
            sheet.Cells[1, 33].SetStyle(cellStyle);
            sheet.Cells[1, 34].SetStyle(cellStyle);
            sheet.Cells[1, 35].SetStyle(cellStyle);
            sheet.Cells[1, 36].SetStyle(cellStyle);
            sheet.Cells[1, 37].SetStyle(cellStyle);
            sheet.Cells[1, 38].SetStyle(cellStyle);
            sheet.Cells[1, 39].SetStyle(cellStyle);
            sheet.Cells[1, 40].SetStyle(cellStyle);
            #endregion

            #region 0-3
            //0-3
            sheet.Cells.Merge(2, 23, 1, 3);
            sheet.Cells[2, 23].PutValue("0-3");
            sheet.Cells[2, 23].SetStyle(cellStyle);
            sheet.Cells[2, 24].SetStyle(cellStyle);
            sheet.Cells[2, 25].SetStyle(cellStyle);


            sheet.Cells[3, 23].PutValue("循环泵指令起动时点（h）");
            sheet.Cells[3, 23].SetStyle(cellStyle);

            sheet.Cells[3, 24].PutValue("指令运行时间（h）");
            sheet.Cells[3, 24].SetStyle(cellStyle);

            sheet.Cells[3, 25].PutValue("电（kw.h）");
            sheet.Cells[3, 25].SetStyle(cellStyle);

            #endregion

            #region 4-7
            //4-7
            sheet.Cells.Merge(2, 26, 1, 3);
            sheet.Cells[2, 26].PutValue("4-7");
            sheet.Cells[2, 26].SetStyle(cellStyle);
            sheet.Cells[2, 27].SetStyle(cellStyle);
            sheet.Cells[2, 28].SetStyle(cellStyle);

            sheet.Cells[3, 26].PutValue("循环泵指令起动时点（h）");
            sheet.Cells[3, 26].SetStyle(cellStyle);

            sheet.Cells[3, 27].PutValue("指令运行时间（h）");
            sheet.Cells[3, 27].SetStyle(cellStyle);

            sheet.Cells[3, 28].PutValue("电（kw.h）");
            sheet.Cells[3, 28].SetStyle(cellStyle);

            #endregion

            #region 8-11
            //8-11
            sheet.Cells.Merge(2, 29, 1, 3);
            sheet.Cells[2, 29].PutValue("8-11");
            sheet.Cells[2, 29].SetStyle(cellStyle);
            sheet.Cells[2, 30].SetStyle(cellStyle);
            sheet.Cells[2, 31].SetStyle(cellStyle);


            sheet.Cells[3, 29].PutValue("循环泵指令起动时点（h）");
            sheet.Cells[3, 29].SetStyle(cellStyle);

            sheet.Cells[3, 30].PutValue("指令运行时间（h）");
            sheet.Cells[3, 30].SetStyle(cellStyle);

            sheet.Cells[3, 31].PutValue("电（kw.h）");
            sheet.Cells[3, 31].SetStyle(cellStyle);

            #endregion

            #region 12-15
            //12-15
            sheet.Cells.Merge(2, 32, 1, 3);
            sheet.Cells[2, 32].PutValue("12-15");
            sheet.Cells[2, 32].SetStyle(cellStyle);
            sheet.Cells[2, 33].SetStyle(cellStyle);
            sheet.Cells[2, 34].SetStyle(cellStyle);

            sheet.Cells[3, 32].PutValue("循环泵指令起动时点（h）");
            sheet.Cells[3, 32].SetStyle(cellStyle);

            sheet.Cells[3, 33].PutValue("指令运行时间（h）");
            sheet.Cells[3, 33].SetStyle(cellStyle);

            sheet.Cells[3, 34].PutValue("电（kw.h）");
            sheet.Cells[3, 34].SetStyle(cellStyle);

            #endregion

            #region 16-19
            //16-19
            sheet.Cells.Merge(2, 35, 1, 3);
            sheet.Cells[2, 35].PutValue("16-19");
            sheet.Cells[2, 35].SetStyle(cellStyle);
            sheet.Cells[2, 36].SetStyle(cellStyle);
            sheet.Cells[2, 37].SetStyle(cellStyle);

            sheet.Cells[3, 35].PutValue("循环泵指令起动时点（h）");
            sheet.Cells[3, 35].SetStyle(cellStyle);

            sheet.Cells[3, 36].PutValue("指令运行时间（h）");
            sheet.Cells[3, 36].SetStyle(cellStyle);

            sheet.Cells[3, 37].PutValue("电（kw.h）");
            sheet.Cells[3, 37].SetStyle(cellStyle);

            #endregion

            #region 20-23
            //20-23
            sheet.Cells.Merge(2, 38, 1, 3);
            sheet.Cells[2, 38].PutValue("20-23");
            sheet.Cells[2, 38].SetStyle(cellStyle);
            sheet.Cells[2, 39].SetStyle(cellStyle);
            sheet.Cells[2, 40].SetStyle(cellStyle);

            sheet.Cells[3, 38].PutValue("循环泵指令起动时点（h）");
            sheet.Cells[3, 38].SetStyle(cellStyle);

            sheet.Cells[3, 39].PutValue("指令运行时间（h）");
            sheet.Cells[3, 39].SetStyle(cellStyle);

            sheet.Cells[3, 40].PutValue("电（kw.h）");
            sheet.Cells[3, 40].SetStyle(cellStyle);

            #endregion


            //3 数据查询
            DataSet dataSetPushOrder = null;
            try
            {
                dataSetPushOrder = new DataAccessHandler().executeDatasetResult(
                    push_order_stat_select,
                    SqlServer.GetParameter("ASSIGNPLANID", item.ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (dataSetPushOrder != null && dataSetPushOrder.Tables.Count > 0)
            {
                //因为要参与计算，所以，这些字段要变成数值型的
                DataTable dataTable = dataSetPushOrder.Tables[0].Clone();

                dataTable.Columns["最高温度"].DataType = typeof(double);
                dataTable.Columns["最低温度"].DataType = typeof(double);
                dataTable.Columns["平均温度"].DataType = typeof(double);
                dataTable.Columns["日运行时间"].DataType = typeof(double);
                dataTable.Columns["总热量"].DataType = typeof(double);
                dataTable.Columns["STARTTIME0"].DataType = typeof(string);
                dataTable.Columns["RUNTIME0"].DataType = typeof(double);
                dataTable.Columns["POWER0"].DataType = typeof(double);
                dataTable.Columns["STARTTIME4"].DataType = typeof(string);
                dataTable.Columns["RUNTIME4"].DataType = typeof(double);
                dataTable.Columns["POWER4"].DataType = typeof(double);
                dataTable.Columns["STARTTIME8"].DataType = typeof(string);
                dataTable.Columns["RUNTIME8"].DataType = typeof(double);
                dataTable.Columns["POWER8"].DataType = typeof(double);
                dataTable.Columns["STARTTIME12"].DataType = typeof(string);
                dataTable.Columns["RUNTIME12"].DataType = typeof(double);
                dataTable.Columns["POWER12"].DataType = typeof(double);
                dataTable.Columns["STARTTIME16"].DataType = typeof(string);
                dataTable.Columns["RUNTIME16"].DataType = typeof(double);
                dataTable.Columns["POWER16"].DataType = typeof(double);
                dataTable.Columns["STARTTIME20"].DataType = typeof(string);
                dataTable.Columns["RUNTIME20"].DataType = typeof(double);
                dataTable.Columns["POWER20"].DataType = typeof(double);

                //添加数据
                DataRow newRow = null;
                foreach (DataRow oldRow in dataSetPushOrder.Tables[0].Rows)
                {
                    newRow = dataTable.NewRow();
                    newRow.ItemArray = oldRow.ItemArray;

                    dataTable.Rows.Add(newRow);
                }

                dataSetPushOrder = null;
                DataRow row = null;
                //依次开始计算
                int dataCount = dataTable.Rows.Count;

                for (int i = 0; i < dataCount; i++)
                {
                    row = dataTable.Rows[i];

                    row["最高温度"] = maxValue;
                    row["最低温度"] = minValue;

                    // 平均温度 = (最高温度 + 最低温度) / 2
                    row["平均温度"] = (Convert.ToDouble(row["最高温度"]) + Convert.ToDouble(row["最低温度"])) / 2;

                    // 日运行时间 =(18/28-平均温度/28)*24 
                    row["日运行时间"] = (18 / 28d - Convert.ToDouble(row["平均温度"]) / 28d) * 24d;

                    //总热量（GJ） = [[1307123*(18/23-平均温度/23)]*0.010327795]*0.78261
                    row["总热量"] = 1307123 * (18 / 23d - Convert.ToDouble(row["平均温度"]) / 23d) * 0.010327795 * 0.78261;


                    //0-3
                    //循环泵指令起动时点（h）
                    row["STARTTIME0"] = "00:00:00";
                    // 指令运行时间（h）  = 日运行时间 * 第1个区间百分比
                    row["RUNTIME0"] = Convert.ToDouble(row["日运行时间"]) * Convert.ToDouble(weatherItems[3].PERCENTAGE);
                    //电（kw.h） = 指令运行时间（h） * 功率
                    if (!string.IsNullOrEmpty(row["功率"].ToString()))
                        row["POWER0"] = Convert.ToDouble(row["RUNTIME0"]) * Convert.ToDouble(row["功率"]);

                    //4-7
                    //循环泵指令起动时点（h）
                    row["STARTTIME4"] = "04:00:00";
                    // 指令运行时间（h）  = 日运行时间 * 第2个区间百分比
                    row["RUNTIME4"] = Convert.ToDouble(row["日运行时间"]) * Convert.ToDouble(weatherItems[7].PERCENTAGE);
                    //电（kw.h） = 指令运行时间（h） * 功率
                    if (!string.IsNullOrEmpty(row["功率"].ToString()))
                        row["POWER4"] = Convert.ToDouble(row["RUNTIME4"]) * Convert.ToDouble(row["功率"]);

                    //8-11
                    //循环泵指令起动时点（h）
                    row["STARTTIME8"] = "08:00:00";
                    // 指令运行时间（h）  = 日运行时间 * 第3个区间百分比
                    row["RUNTIME8"] = Convert.ToDouble(row["日运行时间"]) * Convert.ToDouble(weatherItems[11].PERCENTAGE);
                    //电（kw.h） = 指令运行时间（h） * 功率
                    if (!string.IsNullOrEmpty(row["功率"].ToString()))
                        row["POWER8"] = Convert.ToDouble(row["RUNTIME8"]) * Convert.ToDouble(row["功率"]);

                    //12-15
                    //循环泵指令起动时点（h）
                    row["STARTTIME12"] = "12:00:00";
                    // 指令运行时间（h）  = 日运行时间 * 第4个区间百分比
                    row["RUNTIME12"] = Convert.ToDouble(row["日运行时间"]) * Convert.ToDouble(weatherItems[15].PERCENTAGE);
                    //电（kw.h） = 指令运行时间（h） * 功率
                    if (!string.IsNullOrEmpty(row["功率"].ToString()))
                        row["POWER12"] = Convert.ToDouble(row["RUNTIME12"]) * Convert.ToDouble(row["功率"]);

                    //16-19
                    //循环泵指令起动时点（h）
                    row["STARTTIME16"] = "16:00:00";
                    // 指令运行时间（h）  = 日运行时间 * 第5个区间百分比
                    row["RUNTIME16"] = Convert.ToDouble(row["日运行时间"]) * Convert.ToDouble(weatherItems[19].PERCENTAGE);
                    //电（kw.h） = 指令运行时间（h） * 功率
                    if (!string.IsNullOrEmpty(row["功率"].ToString()))
                        row["POWER16"] = Convert.ToDouble(row["RUNTIME16"]) * Convert.ToDouble(row["功率"]);

                    //20-23
                    //循环泵指令起动时点（h）
                    row["STARTTIME20"] = "16:00:00";
                    // 指令运行时间（h）  = 日运行时间 * 第6个区间百分比
                    row["RUNTIME20"] = Convert.ToDouble(row["日运行时间"]) * Convert.ToDouble(weatherItems[23].PERCENTAGE);
                    //电（kw.h） = 指令运行时间（h） * 功率
                    if (!string.IsNullOrEmpty(row["功率"].ToString()))
                        row["POWER20"] = Convert.ToDouble(row["RUNTIME20"]) * Convert.ToDouble(row["功率"]);

                }


                //处理 总热量的大列
                sheet.Cells.Merge(4, 22, dataCount, 1);
                sheet.Cells[4, 22].PutValue(round(dataTable.Rows[0]["总热量"].ToString()));
                for (int i = 4; i < dataCount; i++)
                {
                    sheet.Cells[i, 22].SetStyle(cellStyle);
                }


                //行 - 左边标题的处理
                row = null;
                string lineName = string.Empty;//回路
                string groupNumber = string.Empty;//序号
                DataRow[] tempDataRowItems = null;//存放临时的查询数组
                List<string> groupNumberList = new List<string>();//临时查询数组中的序号列表
                int lineStartIndex = 4;
                int numberStartIndex = 4;
                int statStartIndex = 4;

                //特殊处理的列。这样，在前面做的汇总就可以不用再动了。
                List<int> otherColumns = new List<int>();
                //23，26，29，32，35，38
                for (int i = 23; i <= dataTable.Columns.Count - 6; i += 3)//这里的6是后面要移除的字段数
                {
                    otherColumns.Add(i);
                }

                for (int i = 0; i < dataCount;)
                {
                    row = dataTable.Rows[i];

                    #region 处理 回路 列

                    lineName = row["回路"].ToString();

                    tempDataRowItems = dataTable.Select(string.Format("回路=\'{0}\'", lineName));

                    sheet.Cells.Merge(lineStartIndex, 0, tempDataRowItems.Length, 1);
                    sheet.Cells[lineStartIndex, 0].PutValue(round(lineName, 0));
                    for (int line = 0; line < tempDataRowItems.Length; line++)
                    {
                        sheet.Cells[lineStartIndex + line, 0].SetStyle(cellStyle);
                    }

                    lineStartIndex += tempDataRowItems.Length;

                    #endregion


                    if (groupNumberList.Count > 0)
                        groupNumberList.Clear();

                    foreach (DataRow tempItem in tempDataRowItems)
                    {
                        groupNumber = tempItem["组"].ToString();
                        if (!groupNumberList.Contains(groupNumber))
                            groupNumberList.Add(groupNumber);

                        groupNumber = null;
                    }

                    tempDataRowItems = null;

                    //处理 序号 列
                    string STCD = string.Empty;
                    string STAT_A_NAME = string.Empty;
                    foreach (string number in groupNumberList)
                    {
                        tempDataRowItems = dataTable.Select(string.Format("回路=\'{0}\' AND 组=\'{1}\'", lineName, number));

                        sheet.Cells.Merge(numberStartIndex, 1, tempDataRowItems.Length, 1);
                        sheet.Cells[numberStartIndex, 1].PutValue(number);
                        for (int num = 0; num < tempDataRowItems.Length; num++)
                        {
                            sheet.Cells[numberStartIndex + num, 1].SetStyle(cellStyle);
                        }

                        for (int p = 0; p < otherColumns.Count; p++)
                        {
                            sheet.Cells.Merge(numberStartIndex, otherColumns[p], tempDataRowItems.Length, 1);

                            if (Convert.ToInt32(number) <= Convert.ToInt32(item.START_NUMBER))
                            {
                                sheet.Cells[numberStartIndex, otherColumns[p]].PutValue(string.Format("{0}:00:00", (otherColumns[p] - 23) + p).PadLeft(8, '0'));
                            }
                            else
                            {
                                sheet.Cells[numberStartIndex, otherColumns[p]].PutValue(string.Format("{0}:30:00", (otherColumns[p] - 23) + p).PadLeft(8, '0'));
                            }

                            for (int num = 0; num < tempDataRowItems.Length; num++)
                            {
                                sheet.Cells[numberStartIndex + num, otherColumns[p]].SetStyle(cellStyle);
                            }
                        }


                        numberStartIndex += tempDataRowItems.Length;

                        //处理 二级站名
                        Dictionary<string, string> stcdList = new Dictionary<string, string>();
                        foreach (DataRow tempRow in tempDataRowItems)
                        {
                            STCD = tempRow["STCD"].ToString();
                            if (!stcdList.ContainsKey(STCD))
                            {
                                STAT_A_NAME = tempRow["二级站名"].ToString();

                                stcdList.Add(STCD, STAT_A_NAME);
                            }
                        }

                        tempDataRowItems = null;


                        foreach (var stcdItem in stcdList)
                        {
                            tempDataRowItems = dataTable.Select(string.Format("回路=\'{0}\' AND 组=\'{1}\' AND STCD=\'{2}\'", lineName, number, stcdItem.Key));

                            sheet.Cells.Merge(statStartIndex, 2, tempDataRowItems.Length, 1);
                            sheet.Cells[statStartIndex, 2].PutValue(stcdItem.Value);
                            for (int stcd = 0; stcd < tempDataRowItems.Length; stcd++)
                            {
                                sheet.Cells[statStartIndex + stcd, 2].SetStyle(cellStyle);
                            }

                            statStartIndex += tempDataRowItems.Length;

                            tempDataRowItems = null;
                        }

                    }

                    i += (lineStartIndex - i) - 4;//跳过这一大组数据
                }


                //移除不导出的列
                dataTable.Columns.Remove("方案名称");
                dataTable.Columns.Remove("回路");
                dataTable.Columns.Remove("组");
                dataTable.Columns.Remove("STCD");
                dataTable.Columns.Remove("二级站名");
                dataTable.Columns.Remove("T_CODE");

                int colCount = dataTable.Columns.Count;
                int rowCount = dataTable.Rows.Count;


                //行的处理
                //定义列头
                int rowIndex = 4;//前3行是标题+表头
                int colIndex = 3;

                for (int i = 0; i < rowCount; i++)
                {
                    colIndex = 3;
                    for (int j = 0; j < colCount; j++)
                    {
                        if (!otherColumns.Contains(colIndex))
                        {
                            sheet.Cells[rowIndex, colIndex].PutValue(round(dataTable.Rows[i][j].ToString()));
                            sheet.Cells.SetRowHeight(rowIndex, rowHeight);
                            sheet.Cells[rowIndex, colIndex].SetStyle(cellStyle);
                        }
                        colIndex++;
                    }
                    rowIndex++;
                }



                //指定列宽为自适应
                sheet.AutoFitColumns();
                sheet.Cells.SetColumnWidth(12, 7.4);
                sheet.Cells.SetColumnWidth(23, 11);
                sheet.Cells.SetColumnWidth(24, 11);
                sheet.Cells.SetColumnWidth(25, 11);
                sheet.Cells.SetColumnWidth(26, 11);
                sheet.Cells.SetColumnWidth(27, 11);
                sheet.Cells.SetColumnWidth(28, 11);
                sheet.Cells.SetColumnWidth(29, 11);
                sheet.Cells.SetColumnWidth(30, 11);
                sheet.Cells.SetColumnWidth(31, 11);
                sheet.Cells.SetColumnWidth(32, 11);
                sheet.Cells.SetColumnWidth(33, 11);
                sheet.Cells.SetColumnWidth(34, 11);
                sheet.Cells.SetColumnWidth(35, 11);
                sheet.Cells.SetColumnWidth(36, 11);
                sheet.Cells.SetColumnWidth(37, 11);
                sheet.Cells.SetColumnWidth(38, 11);
                sheet.Cells.SetColumnWidth(39, 11);
                sheet.Cells.SetColumnWidth(40, 11);

                workbook.Save(Path.Combine(directory, string.Format("{0}.xls", item.NAME)));
            }

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
            text = text.Replace("{@ID}", GUID);

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
            try
            {
                ZY_PushOrderObject zy_pushorderObject = new ZY_PushOrderObject(xml);

                resut = new DataAccessHandler().executeNonQueryResult(
                    string.Format(push_order_insert,
                    zy_pushorderObject.ID,
                    zy_pushorderObject.RUNDATE,
                    zy_pushorderObject.MAXVALUE,
                    zy_pushorderObject.MINVALUE,
                    zy_pushorderObject.EXPORTTYPE,
                    zy_pushorderObject.FILENAME,
                    zy_pushorderObject.FILEURL,
                    zy_pushorderObject.NOTE
                    ), null);

                zy_pushorderObject = null;
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            if (Result.isSuccess(resut))
            {
                resut = Result.getResultXml(getCreatePushOrderXml(ref GUID));
            }

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

            string EmployeeID = "8FF86948-7C20-4DF6-AB08-E4DD55B4CFAB";

            string result = string.Empty;
            try
            {
                result = new DataAccessHandler().executeNonQueryResult(
                    string.Format(send_push_order_insert,
                    GUID,
                    pushID,
                    data.RUNDATE,
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
