using Aspose.Cells;
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
    [FluorineFx.RemotingService("坤泰锅炉月报表")]
    public class KT_Boiler : IController
    {
        #region
        private string push_order_select = @"SELECT ID,RUNDATE,MAXVALUE,MINVALUE,(MAXVALUE+MINVALUE)/2 AS AVEVALUE,CommandTime,ActulTime,
Command_Coal,Actul_Coal,
(ISNULL(Actul_Coal,0)-ISNULL(Command_Coal,0)) AS JN_Coal,
case Actul_Coal-Command_Coal WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(Actul_Coal-Command_Coal)/Command_Coal),4) END AS RATE_Coal,
Command_Water,Actul_Water,
ISNULL(Actul_Water,0)-ISNULL(Command_Water,0) AS JN_Water,
case Actul_Water-Command_Water WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(Actul_Water-Command_Water)/Command_Water),4) END AS RATE_Water,
Command_Ele,Actul_Ele,
ISNULL(Actul_Ele,0)-ISNULL(Command_Ele,0) AS JN_Ele,
case Actul_Ele-Command_Ele WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(Actul_Ele-Command_Ele)/Command_Ele),4) END AS RATE_Ele,
Command_Alkali,Actul_Alkali,
ISNULL(Actul_Alkali,0)-ISNULL(Command_Alkali,0) AS JN_Alkali,
case Actul_Alkali-Command_Alkali WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(Actul_Alkali-Command_Alkali)/Command_Alkali),4) END AS RATE_Alkali,
Command_Salt,Actul_Salt,
ISNULL(Actul_Salt,0)-ISNULL(Command_Salt,0) AS JN_Salt,
case Actul_Salt-Command_Salt WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(Actul_Salt-Command_Salt)/Command_Salt),4) END AS RATE_Salt,
Command_Diesel,Actul_Diesel,
ISNULL(Actul_Diesel,0)-ISNULL(Command_Diesel,0) AS JN_Diesel,
case Actul_Diesel-Command_Diesel WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(Actul_Diesel-Command_Diesel)/Command_Diesel),4) END AS RATE_Diesel,
WatchMan,NOTE,RunDay,1 AS PX 
FROM KT_PUSHORDER WHERE YEAR(RunDay)={0} AND MONTH(RunDay)={1}
UNION 
SELECT '00000000-0000-0000-0000-000000000000',RUNDATE,MAXVALUE,MINVALUE, AVEVALUE,CommandTime,ActulTime,
Command_Coal,Actul_Coal,JN_Coal,RATE_Coal,
Command_Water,Actul_Water,JN_Water,RATE_Water,
Command_Ele,Actul_Ele,JN_Ele,RATE_Ele,
Command_Alkali,Actul_Alkali,JN_Alkali,RATE_Alkali,
Command_Salt,Actul_Salt,JN_Salt,RATE_Salt,
Command_Diesel,Actul_Diesel,JN_Diesel,RATE_Diesel,WatchMan,'',RunDay,2 AS PX 
FROM (SELECT DISTINCT WatchMan AS EMPLOYEENAME,Count(*) AS NUM,(Convert(nvarchar(50),Count(*))+'天') AS WatchMan
, WatchMan+'小计' AS RUNDATE
,SUM(MAXVALUE) AS MAXVALUE,SUM(MINVALUE) AS MINVALUE
,ROUND((SELECT (SUM(MAXVALUE)+SUM(MINVALUE))/COUNT(*)/2 FROM KT_PUSHORDER WHERE YEAR(RunDay)={0} AND MONTH(RunDay)={1} AND WatchMan=KR2.WatchMan),2) AS AVEVALUE 
,SUM(case ISNUMERIC(CommandTime) WHEN 0 THEN 0 ELSE CAST(CommandTime AS decimal(9,2)) END) AS CommandTime,
SUM(case ISNUMERIC(ActulTime) WHEN 0 THEN 0 ELSE CAST(ActulTime AS decimal(9,2)) END) AS ActulTime,
SUM(case ISNUMERIC(Command_Coal) WHEN 0 THEN 0 ELSE CAST(Command_Coal AS decimal(9,2)) END) AS Command_Coal,
SUM(case ISNUMERIC(Actul_Coal) WHEN 0 THEN 0 ELSE CAST(Actul_Coal AS decimal(9,2)) END) AS Actul_Coal,
SUM(Actul_Coal)-SUM(Command_Coal) AS JN_Coal,
case SUM(Actul_Coal)-SUM(Command_Coal) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Coal)-SUM(Command_Coal))/SUM(Command_Coal)),4) END AS RATE_Coal,
SUM(case ISNUMERIC(Command_Water) WHEN 0 THEN 0 ELSE CAST(Command_Water AS decimal(9,2)) END) AS Command_Water,
SUM(case ISNUMERIC(Actul_Water) WHEN 0 THEN 0 ELSE CAST(Actul_Water AS decimal(9,2)) END) AS Actul_Water,
case SUM(Actul_Water)-SUM(Command_Water) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Water)-SUM(Command_Water))/SUM(Command_Water)),4) END AS RATE_Water,
SUM(Actul_Water)-SUM(Command_Water) AS JN_Water,
SUM(case ISNUMERIC(Command_Ele) WHEN 0 THEN 0 ELSE CAST(Command_Ele AS decimal(9,2)) END) AS Command_Ele,
SUM(case ISNUMERIC(Actul_Ele) WHEN 0 THEN 0 ELSE CAST(Actul_Ele AS decimal(9,2)) END) AS Actul_Ele,
case SUM(Actul_Ele)-SUM(Command_Ele) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Ele)-SUM(Command_Ele))/SUM(Command_Ele)),4) END AS RATE_Ele,
SUM(Actul_Ele)-SUM(Command_Ele) AS JN_Ele,
SUM(case ISNUMERIC(Command_Alkali) WHEN 0 THEN 0 ELSE CAST(Command_Alkali AS decimal(9,2)) END) AS Command_Alkali,
SUM(case ISNUMERIC(Actul_Alkali) WHEN 0 THEN 0 ELSE CAST(Actul_Alkali AS decimal(9,2)) END) AS Actul_Alkali,
case SUM(Actul_Alkali)-SUM(Command_Alkali) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Alkali)-SUM(Command_Alkali))/SUM(Command_Alkali)),4) END AS RATE_Alkali,
SUM(Actul_Alkali)-SUM(Command_Alkali) AS JN_Alkali,
SUM(case ISNUMERIC(Command_Salt) WHEN 0 THEN 0 ELSE CAST(Command_Salt AS decimal(9,2)) END) AS Command_Salt,
SUM(case ISNUMERIC(Actul_Salt) WHEN 0 THEN 0 ELSE CAST(Actul_Salt AS decimal(9,2)) END) AS Actul_Salt,
SUM(Actul_Salt)-SUM(Command_Salt) AS JN_Salt,
case SUM(Actul_Salt)-SUM(Command_Salt) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Salt)-SUM(Command_Salt))/SUM(Command_Salt)),4) END AS RATE_Salt,
SUM(case ISNUMERIC(Command_Diesel) WHEN 0 THEN 0 ELSE CAST(Command_Diesel AS decimal(9,2)) END) AS Command_Diesel,
SUM(case ISNUMERIC(Actul_Diesel) WHEN 0 THEN 0 ELSE CAST(Actul_Diesel AS decimal(9,2)) END) AS Actul_Diesel,
SUM(Actul_Diesel)-SUM(Command_Diesel) AS JN_Diesel,
case SUM(Actul_Diesel)-SUM(Command_Diesel) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Diesel)-SUM(Command_Diesel))/SUM(Command_Diesel)),4) END AS RATE_Diesel,
CONVERT(Date,'1900-01-01') AS RunDay
FROM KT_PUSHORDER KR2 WHERE YEAR(RunDay)={0} AND MONTH(RunDay)={1}
GROUP BY WatchMan) M
UNION SELECT '00000000-0000-0000-0000-000000000000','本月合计',SUM(MAXVALUE),SUM(MINVALUE),(ROUND((SELECT (SUM(MAXVALUE)+SUM(MINVALUE))/COUNT(*)/2 FROM KT_PUSHORDER WHERE YEAR(RunDay)={0} AND MONTH(RunDay)={1}),2)) AS AVEVALUE,
SUM(case ISNUMERIC(CommandTime) WHEN 0 THEN 0 ELSE CAST(CommandTime AS decimal(9,2)) END),
SUM(case ISNUMERIC(ActulTime) WHEN 0 THEN 0 ELSE CAST(ActulTime AS decimal(9,2)) END),
SUM(case ISNUMERIC(Command_Coal) WHEN 0 THEN 0 ELSE CAST(Command_Coal AS decimal(9,2)) END),
SUM(case ISNUMERIC(Actul_Coal) WHEN 0 THEN 0 ELSE CAST(Actul_Coal AS decimal(9,2)) END),
SUM(Actul_Coal)-SUM(Command_Coal),
case SUM(Actul_Coal)-SUM(Command_Coal) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Coal)-SUM(Command_Coal))/SUM(Command_Coal)),4) END AS RATE_Coal,
SUM(case ISNUMERIC(Command_Water) WHEN 0 THEN 0 ELSE CAST(Command_Water AS decimal(9,2)) END),
SUM(case ISNUMERIC(Actul_Water) WHEN 0 THEN 0 ELSE CAST(Actul_Water AS decimal(9,2)) END),
SUM(Actul_Water)-SUM(Command_Water),
case SUM(Actul_Water)-SUM(Command_Water) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Water)-SUM(Command_Water))/SUM(Command_Water)),4) END AS RATE_Water,
SUM(case ISNUMERIC(Command_Ele) WHEN 0 THEN 0 ELSE CAST(Command_Ele AS decimal(9,2)) END),
SUM(case ISNUMERIC(Actul_Ele) WHEN 0 THEN 0 ELSE CAST(Actul_Ele AS decimal(9,2)) END),
SUM(Actul_Ele)-SUM(Command_Ele),
case SUM(Actul_Ele)-SUM(Command_Ele) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Ele)-SUM(Command_Ele))/SUM(Command_Ele)),4) END AS RATE_Ele,
SUM(case ISNUMERIC(Command_Alkali) WHEN 0 THEN 0 ELSE CAST(Command_Alkali AS decimal(9,2)) END),
SUM(case ISNUMERIC(Actul_Alkali) WHEN 0 THEN 0 ELSE CAST(Actul_Alkali AS decimal(9,2)) END),
SUM(Actul_Alkali)-SUM(Command_Alkali),
case SUM(Actul_Alkali)-SUM(Command_Alkali) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Alkali)-SUM(Command_Alkali))/SUM(Command_Alkali)),4) END AS RATE_Alkali,
SUM(case ISNUMERIC(Command_Salt) WHEN 0 THEN 0 ELSE CAST(Command_Salt AS decimal(9,2)) END),
SUM(case ISNUMERIC(Actul_Salt) WHEN 0 THEN 0 ELSE CAST(Actul_Salt AS decimal(9,2)) END),
SUM(Actul_Salt)-SUM(Command_Salt),
case SUM(Actul_Salt)-SUM(Command_Salt) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Salt)-SUM(Command_Salt))/SUM(Command_Salt)),4) END AS RATE_Salt,
SUM(case ISNUMERIC(Command_Diesel) WHEN 0 THEN 0 ELSE CAST(Command_Diesel AS decimal(9,2)) END),
SUM(case ISNUMERIC(Actul_Diesel) WHEN 0 THEN 0 ELSE CAST(Actul_Diesel AS decimal(9,2)) END),
SUM(Actul_Diesel)-SUM(Command_Diesel),
case SUM(Actul_Diesel)-SUM(Command_Diesel) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Diesel)-SUM(Command_Diesel))/SUM(Command_Diesel)),4) END AS RATE_Diesel,
 '共计'+CAST(COUNT(1) AS NVARCHAR(20))+'天','' ,CONVERT(Date,'1900-01-01') ,3 AS PX FROM KT_PUSHORDER 
WHERE YEAR(RunDay)={0} AND MONTH(RunDay)={1}
order by PX, RunDay";

        private string select_TeamTotal_Command = @"SELECT DISTINCT WatchMan AS EMPLOYEENAME,(Convert(nvarchar(50),Count(*))+'天') AS RUNDAY
, LTRIM(RTRIM(REPLACE(WatchMan,'.','')))+'小计' AS RUNDATE
,SUM(MAXVALUE) AS MAXVALUE,SUM(MINVALUE) AS MINVALUE
,ROUND((SELECT (SUM(MAXVALUE)+SUM(MINVALUE))/COUNT(*)/2 FROM KT_PUSHORDER WHERE YEAR(RunDay)={0} AND MONTH(RunDay)={1} AND WatchMan=KR2.WatchMan),2) AS AVEVALUE
,SUM(case ISNUMERIC(CommandTime) WHEN 0 THEN 0 ELSE CAST(CommandTime AS decimal(9,2)) END) AS CommandTime,
SUM(case ISNUMERIC(ActulTime) WHEN 0 THEN 0 ELSE CAST(ActulTime AS decimal(9,2)) END) AS ActulTime,
SUM(case ISNUMERIC(Command_Coal) WHEN 0 THEN 0 ELSE CAST(Command_Coal AS decimal(9,2)) END) AS Command_Coal,
SUM(case ISNUMERIC(Actul_Coal) WHEN 0 THEN 0 ELSE CAST(Actul_Coal AS decimal(9,2)) END) AS Actul_Coal,
(SUM(Actul_Coal)-SUM(Command_Coal)) AS JN_Coal,
case SUM(Actul_Coal)-SUM(Command_Coal) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Coal)-SUM(Command_Coal))/SUM(Command_Coal)),4) END AS RATE_Coal,
SUM(case ISNUMERIC(Command_Water) WHEN 0 THEN 0 ELSE CAST(Command_Water AS decimal(9,2)) END) AS Command_Water,
SUM(case ISNUMERIC(Actul_Water) WHEN 0 THEN 0 ELSE CAST(Actul_Water AS decimal(9,2)) END) AS Actul_Water,
(SUM(Actul_Water)-SUM(Command_Water)) AS JN_Water,
case SUM(Actul_Water)-SUM(Command_Water) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Water)-SUM(Command_Water))/SUM(Command_Water)),4) END AS RATE_Water,
SUM(case ISNUMERIC(Command_Ele) WHEN 0 THEN 0 ELSE CAST(Command_Ele AS decimal(9,2)) END) AS Command_Ele,
SUM(case ISNUMERIC(Actul_Ele) WHEN 0 THEN 0 ELSE CAST(Actul_Ele AS decimal(9,2)) END) AS Actul_Ele,
(SUM(Actul_Ele)-SUM(Command_Ele)) AS JN_Ele,
case SUM(Actul_Ele)-SUM(Command_Ele) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Ele)-SUM(Command_Ele))/SUM(Command_Ele)),4) END AS RATE_Ele,
SUM(case ISNUMERIC(Command_Alkali) WHEN 0 THEN 0 ELSE CAST(Command_Alkali AS decimal(9,2)) END) AS Command_Alkali,
SUM(case ISNUMERIC(Actul_Alkali) WHEN 0 THEN 0 ELSE CAST(Actul_Alkali AS decimal(9,2)) END) AS Actul_Alkali,
(SUM(Actul_Alkali)-SUM(Command_Alkali)) AS JN_Alkali,
case SUM(Actul_Alkali)-SUM(Command_Alkali) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Alkali)-SUM(Command_Alkali))/SUM(Command_Alkali)),4) END AS RATE_Alkali,
SUM(case ISNUMERIC(Command_Salt) WHEN 0 THEN 0 ELSE CAST(Command_Salt AS decimal(9,2)) END) AS Command_Salt,
SUM(case ISNUMERIC(Actul_Salt) WHEN 0 THEN 0 ELSE CAST(Actul_Salt AS decimal(9,2)) END) AS Actul_Salt,
(SUM(Actul_Salt)-SUM(Command_Salt)) AS JN_Salt,
case SUM(Actul_Salt)-SUM(Command_Salt) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Salt)-SUM(Command_Salt))/SUM(Command_Salt)),4) END AS RATE_Salt,
SUM(case ISNUMERIC(Command_Diesel) WHEN 0 THEN 0 ELSE CAST(Command_Diesel AS decimal(9,2)) END) AS Command_Diesel,
SUM(case ISNUMERIC(Actul_Diesel) WHEN 0 THEN 0 ELSE CAST(Actul_Diesel AS decimal(9,2)) END) AS Actul_Diesel,
(SUM(Actul_Diesel)-SUM(Command_Diesel)) AS JN_Diesel,
case SUM(Actul_Diesel)-SUM(Command_Diesel) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Diesel)-SUM(Command_Diesel))/SUM(Command_Diesel)),4) END AS RATE_Diesel,
1 AS PX
FROM KT_PUSHORDER KR2 WHERE YEAR(RunDay)={0} AND MONTH(RunDay)={1}
GROUP BY WatchMan
UNION SELECT '本月合计', '共计'+CAST(COUNT(1) AS NVARCHAR(20))+'天' ,'本月合计',SUM(MAXVALUE),SUM(MINVALUE)
,(ROUND((SELECT (SUM(MAXVALUE)+SUM(MINVALUE))/COUNT(*)/2 FROM KT_PUSHORDER WHERE YEAR(RunDay)={0} AND MONTH(RunDay)={1}),2)) AS AVEVALUE,
SUM(case ISNUMERIC(CommandTime) WHEN 0 THEN 0 ELSE CAST(CommandTime AS decimal(9,2)) END),
SUM(case ISNUMERIC(ActulTime) WHEN 0 THEN 0 ELSE CAST(ActulTime AS decimal(9,2)) END),
SUM(case ISNUMERIC(Command_Coal) WHEN 0 THEN 0 ELSE CAST(Command_Coal AS decimal(9,2)) END),
SUM(case ISNUMERIC(Actul_Coal) WHEN 0 THEN 0 ELSE CAST(Actul_Coal AS decimal(9,2)) END),
(SUM(Actul_Coal)-SUM(Command_Coal)) AS JN_Coal,
case SUM(Actul_Coal)-SUM(Command_Coal) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Coal)-SUM(Command_Coal))/SUM(Command_Coal)),4) END AS RATE_Coal,
SUM(case ISNUMERIC(Command_Water) WHEN 0 THEN 0 ELSE CAST(Command_Water AS decimal(9,2)) END),
SUM(case ISNUMERIC(Actul_Water) WHEN 0 THEN 0 ELSE CAST(Actul_Water AS decimal(9,2)) END),
(SUM(Actul_Water)-SUM(Command_Water)) AS JN_Water,
case SUM(Actul_Water)-SUM(Command_Water) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Water)-SUM(Command_Water))/SUM(Command_Water)),4) END AS RATE_Water,
SUM(case ISNUMERIC(Command_Ele) WHEN 0 THEN 0 ELSE CAST(Command_Ele AS decimal(9,2)) END),
SUM(case ISNUMERIC(Actul_Ele) WHEN 0 THEN 0 ELSE CAST(Actul_Ele AS decimal(9,2)) END),
(SUM(Actul_Ele)-SUM(Command_Ele)) AS JN_Ele,
case SUM(Actul_Ele)-SUM(Command_Ele) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Ele)-SUM(Command_Ele))/SUM(Command_Ele)),4) END AS RATE_Ele,
SUM(case ISNUMERIC(Command_Alkali) WHEN 0 THEN 0 ELSE CAST(Command_Alkali AS decimal(9,2)) END),
SUM(case ISNUMERIC(Actul_Alkali) WHEN 0 THEN 0 ELSE CAST(Actul_Alkali AS decimal(9,2)) END),
(SUM(Actul_Alkali)-SUM(Command_Alkali)) AS JN_Alkali,
case SUM(Actul_Alkali)-SUM(Command_Alkali) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Alkali)-SUM(Command_Alkali))/SUM(Command_Alkali)),4) END AS RATE_Alkali,
SUM(case ISNUMERIC(Command_Salt) WHEN 0 THEN 0 ELSE CAST(Command_Salt AS decimal(9,2)) END),
SUM(case ISNUMERIC(Actul_Salt) WHEN 0 THEN 0 ELSE CAST(Actul_Salt AS decimal(9,2)) END),
(SUM(Actul_Salt)-SUM(Command_Salt)) AS JN_Salt,
case SUM(Actul_Salt)-SUM(Command_Salt) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Salt)-SUM(Command_Salt))/SUM(Command_Salt)),4) END AS RATE_Salt,
SUM(case ISNUMERIC(Command_Diesel) WHEN 0 THEN 0 ELSE CAST(Command_Diesel AS decimal(9,2)) END),
SUM(case ISNUMERIC(Actul_Diesel) WHEN 0 THEN 0 ELSE CAST(Actul_Diesel AS decimal(9,2)) END) ,
(SUM(Actul_Diesel)-SUM(Command_Diesel)) AS JN_Diesel,
case SUM(Actul_Diesel)-SUM(Command_Diesel) WHEN 0 THEN 0 ELSE ROUND(CONVERT(FLOAT,(SUM(Actul_Diesel)-SUM(Command_Diesel))/SUM(Command_Diesel)),4) END AS RATE_Diesel,
2 AS PX 
FROM KT_PUSHORDER 
WHERE YEAR(RunDay)={0} AND MONTH(RunDay)={1}
order by PX, RunDay";
        #endregion
        #region 日期选择
        public string GetDateSelect()
        {
            string commandText = "SELECT DATENAME(Year,RunDay)+N'年'+CAST(DATEPART(Month,RunDay) AS varchar)+N'月' AS NAME,DATENAME(Year,RunDay)+N'-'+CAST(DATEPART(Month,RunDay) AS varchar) AS VALUE FROM KT_PUSHORDER GROUP BY DATENAME(Year,RunDay)+N'年'+CAST(DATEPART(Month,RunDay) AS varchar)+N'月',DATENAME(Year,RunDay)+N'-'+CAST(DATEPART(Month,RunDay) AS varchar) ORDER BY VALUE DESC";

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
                result = Result.getResultXml(getDateSelectItemXml(dataSetPushOrder));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            return result;
        }

        private string getDateSelectItemXml(DataSet dataSetPushOrder)
        {
            StringBuilder xml = new StringBuilder();

            xml.AppendFormat("<DATAS>");
            foreach (DataRow row in dataSetPushOrder.Tables[0].Rows)
            {
                xml.AppendFormat("<DATA NAME=\"{0}\" VALUE=\"{1}\" />",
                 row["NAME"].ToString(),
                 row["VALUE"].ToString()
                );
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }

        #endregion

        #region 获取列表数据
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
               ? DateTime.Now.ToString("yyyy-MM")
               : xml.Element("DATETIME").Value;
            string[] Months = datetime.Split('-');
            string slYear = Months[0];
            string slMonth = Months[1];
            string commandText = string.Format(push_order_select, slYear, slMonth);

            DataSet dataSetPushOrder = null;
            try
            {
                dataSetPushOrder = new DataAccessHandler().executeDatasetResult(commandText, null);
                DataRow dr = dataSetPushOrder.Tables[0].NewRow();
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

            xml.AppendFormat("<DATAS>");

            //  KT_RunCommandObject RunCommandObject = null;
            foreach (DataRow row in dataSetPushOrder.Tables[0].Rows)
            {
                xml.AppendFormat("<DATA ID=\"{0}\" RUNDATE=\"{1}\" MAXVALUE=\"{2}\" MINVALUE=\"{3}\" AVEVALUE=\"{4}\" CommandTime=\"{5}\" ActulTime=\"{6}\" Command_Coal=\"{7}\" Actul_Coal=\"{8}\" Command_Water=\"{9}\" Actul_Water=\"{10}\" Command_Ele=\"{11}\" Actul_Ele=\"{12}\" Command_Alkali=\"{13}\" Actul_Alkali=\"{14}\" Command_Salt=\"{15}\" Actul_Salt=\"{16}\" Command_Diesel=\"{17}\" Actul_Diesel=\"{18}\" WatchMan=\"{19}\"/>",
                 row["ID"].ToString(),
                 row["RUNDATE"].ToString(),
                 row["MAXVALUE"].ToString(),
                   row["MINVALUE"].ToString(),
                    row["AVEVALUE"].ToString(),
                   row["CommandTime"].ToString(),
                  row["ActulTime"].ToString(),
                   row["Command_Coal"].ToString(),
                   row["Actul_Coal"].ToString(),
                   row["Command_Water"].ToString(),
                   row["Actul_Water"].ToString(),
                   row["Command_Ele"].ToString(),
                   row["Actul_Ele"].ToString(),
                   row["Command_Alkali"].ToString(),
                   row["Actul_Alkali"].ToString(),
                   row["Command_Salt"].ToString(),
                   row["Actul_Salt"].ToString(),
                   row["Command_Diesel"].ToString(),
                   row["Actul_Diesel"].ToString(),
                   row["WatchMan"].ToString()
                );
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }

        public string getDataItemDetails(string text)
        {
            throw new NotImplementedException();
        }
        #endregion

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
            string[] Months = datetime.Split('-');
            string slYear = Months[0];
            string slMonth = Months[1];
            string commandText = string.Format(push_order_select, slYear, slMonth);

            DataSet dataSetDownReport = null;
            try
            {
                dataSetDownReport = new DataAccessHandler().executeDatasetResult(commandText, null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }
            string FileName = string.Empty;
            string directory = string.Empty;
            if (dataSetDownReport.Tables.Count > 0)
            {
                DataTable dt = dataSetDownReport.Tables[0];
                CreateReportFile(dt, slYear, slMonth, ref FileName);
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
            //坤泰热源15-16年度1月份锅炉日运行耗煤量统计表.xlsx
            FileName = string.Format("坤泰热源{0}年度{1}月份锅炉日运行耗量统计表.xls", GetYears(slYear, slMonth), slMonth);

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
            Aspose.Cells.Worksheet sheet0 = CreateSheet("煤", "t", slYear, slMonth, workbook, dt);
            Aspose.Cells.Worksheet sheet1 = CreateSheet("水", "t", slYear, slMonth, workbook, dt);
            Aspose.Cells.Worksheet sheet2 = CreateSheet("电", "kw.h", slYear, slMonth, workbook, dt);
            Aspose.Cells.Worksheet sheet3 = CreateSheet("碱", "t", slYear, slMonth, workbook, dt);
            Aspose.Cells.Worksheet sheet4 = CreateSheet("盐", "kg", slYear, slMonth, workbook, dt);
            Aspose.Cells.Worksheet sheet5 = CreateSheet("柴油", "kg", slYear, slMonth, workbook, dt);
            //耗量汇总表
            Aspose.Cells.Worksheet sheet6 = CreateTotalSheet(slYear, slMonth, workbook, dt);
            //节能率汇总
            string commandTeamTotal = string.Format(select_TeamTotal_Command,slYear,slMonth);
            DataTable dTotal= new DataAccessHandler().executeDatasetResult(commandTeamTotal, null).Tables[0];
            if (dTotal.Rows.Count>0)
            {
                Aspose.Cells.Worksheet sheet7 = CreateTeamTotalSheet(slYear, slMonth, workbook, dTotal);
            }
            workbook.Save(directory);
        }

        private Worksheet CreateTeamTotalSheet(string slYear, string slMonth, Workbook workbook, DataTable dTotal)
        {
            string sheetName = "节能率汇总";
            Aspose.Cells.Worksheet sheet = workbook.Worksheets.Add(sheetName);
            sheet.Name = sheetName;//Sheet名字

            int rowHeight = 22;//行高
            string cellFontName = "宋体";//字体
            int cellFontSize = 10;//字体大小
            //坤泰热源15-16年度12月份耗量汇总表
            string titleName = string.Format("坤泰热源{0}年度{1}月份节能率汇总表", GetYears(slYear, slMonth), slMonth);


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

            //Style ss= setSheetStyle(workbook, rowHeight, cellFontName, cellFontSize, Color.Gray);

            #region 行，列
            int r = 0;//行
            int r2 = r;
            int c = 0;//列
            #endregion

            #region 标题
            sheet.Cells.Merge(r, c, 1, 31);
            sheet.Cells[r, c].PutValue(titleName);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells[r, c].Style.Font.Size = 24;
            sheet.Cells[r, c].Style.Font.Name = cellFontName;
            sheet.Cells[r, c].Style.HorizontalAlignment = TextAlignmentType.Center;
            sheet.Cells.SetRowHeight(r, 40);
            #endregion

            #region 第2、3行
            #region 姓名
            r++;
            r2 = r + 1;
            sheet.Cells.Merge(r, c, 2, 1);
            sheet.Cells[r, c].PutValue("姓名");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.SetRowHeight(r, 22);
            sheet.Cells.SetColumnWidth(c, 10);

            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells.SetRowHeight(r2, 29);
            #endregion

            #region 天数
            c++;
            sheet.Cells.Merge(r, c, 2, 1);
            sheet.Cells[r, c].PutValue("天数");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells[r2, c].SetStyle(cellStyle);
            #endregion

            #region 最高温度（℃）
            c++;
            sheet.Cells.Merge(r, c, 2, 1);
            sheet.Cells[r, c].PutValue("最高温度（℃）");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(2, 13);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells[r2, c].SetStyle(cellStyle);
            #endregion

            #region 最低温度（℃）
            c++;
            sheet.Cells.Merge(r, c, 2, 1);
            sheet.Cells[r, c].PutValue("最低温度（℃）");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(3, 13);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells[r2, c].SetStyle(cellStyle);
            #endregion

            #region 平均温度（℃）
            c++;
            sheet.Cells.Merge(r, c, 2, 1);
            sheet.Cells[r, c].PutValue("平均温度（℃）");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 13);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells[r2, c].SetStyle(cellStyle);
            #endregion

            #region 指令运行时间(h)
            c++;
            sheet.Cells.Merge(r, c, 1, 1);
            sheet.Cells[r, c].PutValue("指令运行时间(h)");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 8);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("(4区间) ");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            #endregion

            #region 实际运行时间(h)
            c++;
            sheet.Cells.Merge(r, c, 1, 1);
            sheet.Cells[r, c].PutValue("实际运行时间(h)");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 8);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("(4区间)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            #endregion

            #region 煤
            c++;
            sheet.Cells.Merge(r, c, 1, 4);
            sheet.Cells[r, c].PutValue("煤");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("指令煤(t)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;


            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("实际煤(t)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("节能量（t）");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("节能率");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            #endregion

            #region 水
            c++;
            sheet.Cells.Merge(r, c, 1, 4);
            sheet.Cells[r, c].PutValue("水");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("指令水(t)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("实际水(t)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("节能量（t）");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("节能率");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            #endregion

            #region 电
            c++;
            sheet.Cells.Merge(r, c, 1, 4);
            sheet.Cells[r, c].PutValue("电");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("指令电(kw.h)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("实际电(kw.h)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("节能量（t）");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("节能率");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            #endregion

            #region 碱
            c++;
            sheet.Cells.Merge(r, c, 1, 4);
            sheet.Cells[r, c].PutValue("碱");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("指令碱(t)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("实际碱(t)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("节能量（t）");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("节能率");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            #endregion

            #region 盐
            c++;
            sheet.Cells.Merge(r, c, 1, 4);
            sheet.Cells[r, c].PutValue("盐");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("指令盐(kg)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("实际盐(kg)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("节能量（t）");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("节能率");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            #endregion

            #region 柴油
            c++;
            sheet.Cells.Merge(r, c, 1, 4);
            sheet.Cells[r, c].PutValue("柴油");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("指令油(kg)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("实际油(kg)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("节能量（t）");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("节能率");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            #endregion

            #endregion

            #region
            r = r2;
            DataView dv = new DataView(dTotal);
            string[] Col = { "EMPLOYEENAME", "RunDay", "MAXVALUE", "MINVALUE", "AVEVALUE", "CommandTime", "ActulTime", "Command_Coal", "Actul_Coal", "JN_Coal", "RATE_Coal", "Command_Water", "Actul_Water","JN_Water","RATE_Water", "Command_Ele", "Actul_Ele","JN_Ele","RATE_Ele", "Command_Alkali", "Actul_Alkali", "JN_Alkali","RATE_Alkali","Command_Salt", "Actul_Salt","JN_Salt","RATE_Salt", "Command_Diesel", "Actul_Diesel","JN_Diesel","RATE_Diesel" };
            DataTable dtTotal = dv.ToTable(true, Col);
            for (int i = 0; i < dtTotal.Rows.Count; i++)
            {
                Color col = i % 2 == 0 ? Color.Yellow : Color.FromArgb(0, 204, 255);
                r++;
                for (int j = 0; j < Col.Length; j++)
                {
                    if (j==0 && dtTotal.Rows[i][j].ToString().Equals("本月合计"))
                    {
                        col = Color.Red;
                        rowHeight = 31;
                    }
                    sheet.Cells.Merge(r, j, 1, 1);
                    sheet.Cells[r, j].PutValue(dtTotal.Rows[i][j].ToString());
                    sheet.Cells[r, j].SetStyle(setSheetStyle(workbook, col));
                    sheet.Cells.SetRowHeight(r, rowHeight);
                }
            }

            #endregion
            return sheet;
        }

        private Worksheet CreateTotalSheet(string slYear, string slMonth, Workbook workbook, DataTable dt)
        {
            string sheetName = "耗量汇总表";
            Aspose.Cells.Worksheet sheet = workbook.Worksheets.Add(sheetName);
            sheet.Name = sheetName;//Sheet名字

            int rowHeight = 22;//行高
            string cellFontName = "宋体";//字体
            int cellFontSize = 10;//字体大小
            //坤泰热源15-16年度12月份耗量汇总表
            string titleName = string.Format("坤泰热源{0}年度{1}月份耗量汇总表", GetYears(slYear, slMonth), slMonth);


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

            //Style ss= setSheetStyle(workbook, rowHeight, cellFontName, cellFontSize, Color.Gray);

            #region 行，列
            int r = 0;//行
            int r2 = r;
            int c = 0;//列
            #endregion

            #region 标题
            sheet.Cells.Merge(r, c, 1, 19);
            sheet.Cells[r, c].PutValue(titleName);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells[r, c].Style.Font.Size = 24;
            sheet.Cells[r, c].Style.Font.Name = cellFontName;
            sheet.Cells[r, c].Style.HorizontalAlignment = TextAlignmentType.Center;
            sheet.Cells.SetRowHeight(r, 40);
            #endregion

            #region 第2、3行
            #region 日期
            r++;
            r2 = r + 1;
            sheet.Cells.Merge(r, c, 2, 1);
            sheet.Cells[r, c].PutValue("日期");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.SetRowHeight(r, 22);
            sheet.Cells.SetColumnWidth(c, 10);

            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells.SetRowHeight(r2, 29);
            #endregion

            #region 姓名
            c++;
            sheet.Cells.Merge(r, c, 2, 1);
            sheet.Cells[r, c].PutValue("姓名");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells[r2, c].SetStyle(cellStyle);
            #endregion

            #region 最高温度（℃）
            c++;
            sheet.Cells.Merge(r, c, 2, 1);
            sheet.Cells[r, c].PutValue("最高温度（℃）");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(2, 13);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells[r2, c].SetStyle(cellStyle);
            #endregion

            #region 最低温度（℃）
            c++;
            sheet.Cells.Merge(r, c, 2, 1);
            sheet.Cells[r, c].PutValue("最低温度（℃）");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(3, 13);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells[r2, c].SetStyle(cellStyle);
            #endregion

            #region 平均温度（℃）
            c++;
            sheet.Cells.Merge(r, c, 2, 1);
            sheet.Cells[r, c].PutValue("平均温度（℃）");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 13);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells[r2, c].SetStyle(cellStyle);
            #endregion

            #region 指令运行时间(h)
            c++;
            sheet.Cells.Merge(r, c, 1, 1);
            sheet.Cells[r, c].PutValue("指令运行时间(h)");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 8);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("(4区间) ");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            #endregion

            #region 实际运行时间(h)
            c++;
            sheet.Cells.Merge(r, c, 1, 1);
            sheet.Cells[r, c].PutValue("实际运行时间(h)");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 8);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("(4区间)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;
            #endregion

            #region 煤
            c++;
            sheet.Cells.Merge(r, c, 1, 2);
            sheet.Cells[r, c].PutValue("煤");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("指令煤(t)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;


            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("实际煤(t)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            #endregion

            #region 水
            c++;
            sheet.Cells.Merge(r, c, 1, 2);
            sheet.Cells[r, c].PutValue("水");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("指令水(t)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("实际水(t)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            #endregion

            #region 电
            c++;
            sheet.Cells.Merge(r, c, 1, 2);
            sheet.Cells[r, c].PutValue("电");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("指令电(kw.h)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("实际电(kw.h)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            #endregion

            #region 碱
            c++;
            sheet.Cells.Merge(r, c, 1, 2);
            sheet.Cells[r, c].PutValue("碱");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("指令碱(t)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("实际碱(t)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            #endregion

            #region 盐
            c++;
            sheet.Cells.Merge(r, c, 1, 2);
            sheet.Cells[r, c].PutValue("盐");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("指令盐(kg)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("实际盐(kg)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            #endregion

            #region 柴油
            c++;
            sheet.Cells.Merge(r, c, 1, 2);
            sheet.Cells[r, c].PutValue("柴油");
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells[r, c].Style.Font.IsBold = true;
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("指令油(kg)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            c++;
            sheet.Cells[r, c].SetStyle(cellStyle);
            sheet.Cells.SetColumnWidth(c, 9);
            sheet.Cells.Merge(r2, c, 1, 1);
            sheet.Cells[r2, c].PutValue("实际油(kg)");
            sheet.Cells[r2, c].SetStyle(cellStyle);
            sheet.Cells[r2, c].Style.Font.IsBold = true;

            #endregion

            #endregion

            #region
            r = r2;
            DataView dv = new DataView(dt);
            string[] Col = { "RunDay", "WatchMan", "MAXVALUE", "MINVALUE", "AVEVALUE", "CommandTime", "ActulTime", "Command_Coal", "Actul_Coal", "Command_Water", "Actul_Water", "Command_Ele", "Actul_Ele", "Command_Alkali", "Actul_Alkali", "Command_Salt", "Actul_Salt", "Command_Diesel", "Actul_Diesel" };
            DataTable dtTotal = dv.ToTable(true, Col);
            for (int i = 0; i < dtTotal.Rows.Count; i++)
            {
                Color col = i % 2 == 0 ? Color.Yellow : Color.FromArgb(0, 204, 255);
                r++;
                for (int j = 0; j < Col.Length; j++)
                {
                    string DataValue = dtTotal.Rows[i][j].ToString();
                    if (j == 0)
                    {
                        DateTime dTime = DateTime.Now;
                        if (DateTime.TryParse(DataValue, out dTime))
                        {
                            DataValue = dTime.ToString("yyyy-MM-dd").Equals("1900-01-01") ? dt.Rows[i]["RUNDATE"].ToString() : Convert.ToDateTime(dtTotal.Rows[i]["RunDay"].ToString()).ToString("M.dd");
                        }
                    }
                    if (DataValue.Equals("本月合计"))
                    {
                        col = Color.Red;
                        rowHeight = 31;
                    }
                    sheet.Cells.Merge(r, j, 1, 1);
                    sheet.Cells[r, j].PutValue(DataValue);
                    sheet.Cells[r, j].SetStyle(setSheetStyle(workbook, col));
                    sheet.Cells.SetRowHeight(r, rowHeight);
                }
            }

            #endregion
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

        private Worksheet CreateSheet(string sheetName,string Unit, string slYear, string slMonth, Workbook workbook, DataTable dt)
        {
            #region
            string Command = "";
            string Actul = "";
            string JN = "";
            string RATE = "";
            switch (sheetName)
            {
                case "煤":
                    Command ="Command_Coal";
                    Actul = "Actul_Coal";
                    JN = "JN_Coal";
                    RATE = "RATE_Coal";
                    break;
                case "水":
                    Command = "Command_Water";
                    Actul = "Actul_Water";
                    JN = "JN_Water";
                    RATE = "RATE_Water";
                    break;
                case "电":
                    Command = "Command_Ele";
                    Actul = "Actul_Ele";
                    JN = "JN_Ele";
                    RATE = "RATE_Ele";
                    break;
                case "碱":
                    Command = "Command_Alkali";
                    Actul = "Actul_Alkali";
                    JN = "JN_Alkali";
                    RATE = "RATE_Alkali";
                    break;
                case "盐":
                    Command = "Command_Salt";
                    Actul = "Actul_Salt";
                    JN = "JN_Salt";
                    RATE = "RATE_Salt";
                    break;
                case "柴油":
                    Command = "Command_Diesel";
                    Actul = "Actul_Diesel";
                    JN = "JN_Diesel";
                    RATE = "RATE_Diesel";
                    break;
                default:
                    Command = "Command_Coal";
                    Actul = "Actul_Coal";
                    JN = "JN_Coal";
                    RATE = "RATE_Coal";
                    break;
            }
            #endregion

            Aspose.Cells.Worksheet sheet = workbook.Worksheets.Add(sheetName);
            sheet.Name = sheetName;//Sheet名字

            int rowHeight = 22;//行高
            string cellFontName = "宋体";//字体
            int cellFontSize = 10;//字体大小
            string titleName = string.Format("坤泰热源{0}年度{1}月份耗{2}量统计表", GetYears(slYear, slMonth), slMonth, sheetName);

          //  日 期    姓 名    最高温度（℃）	最低温度（℃）	平均温度（℃）	指令运行时间(h)   实际运行时间(h)   运行指令人工(t)   实际耗人工量(t)   节能量(t)  节能率 备注

            //      (8区间) 	(8区间)					
            string[,] Col = { { "日  期", "" }, { "姓 名", "" }, { "最高温度（℃）", "" }, { "最低温度（℃）", "" }, { "平均温度（℃）", "" }, { "指令运行时间(h)", "(4区间)" }, { "实际运行时间(h)", "(4区间)" }, { string.Format("运行指令{0}({1})", sheetName, Unit), "" }, { string.Format("实际耗{0}量({1})", sheetName, Unit), "" }, { string.Format("节能量({0})", Unit), "" }, { "节能率", "" }, { "备注", "" } };
            string[] ColName = { "RunDay", "WatchMan", "MAXVALUE", "MINVALUE", "AVEVALUE", "CommandTime", "ActulTime", Command, Actul, JN, RATE, "NOTE" };
            int[] width = { 10, 13, 13, 13, 13, 15, 15, 14, 14, 13, 13, 26 };

            int Len = Col.GetLength(0);

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
            int r2 =0;
            int c = 0;//列
            #endregion

            #region 标题
            sheet.Cells.Merge(r,c, 1, Len);
            sheet.Cells[r,c].PutValue(titleName);
            sheet.Cells[r,c].Style.Font.IsBold = true;
            sheet.Cells[r,c].Style.Font.Size = 24;
            sheet.Cells[r,c].Style.Font.Name = cellFontName;
            sheet.Cells[r,c].Style.HorizontalAlignment = TextAlignmentType.Center;
            sheet.Cells.SetRowHeight(r, 40);
            #endregion
            r++;
            r2 = r + 1;
            c = 0;
            for (int i = 0; i < Len; i++)
            {
                if (string.IsNullOrEmpty(Col[i, 1]))
                {
                    sheet.Cells.Merge(r, c, 2, 1);
                    sheet.Cells[r, c].PutValue(Col[i, 0]);
                    sheet.Cells[r, c].SetStyle(cellStyle);
                    sheet.Cells[r, c].Style.Font.IsBold = true;
                    sheet.Cells.SetRowHeight(r, rowHeight);
                    sheet.Cells[r2, c].SetStyle(cellStyle);
                    sheet.Cells.SetRowHeight(r2, rowHeight);
                }
                else
                {
                    sheet.Cells.Merge(r, c, 1, 1);
                    sheet.Cells[r, c].PutValue(Col[i, 0]);
                    sheet.Cells[r, c].SetStyle(cellStyle);
                    sheet.Cells[r, c].Style.Font.IsBold = true;
                    sheet.Cells.Merge(r2, c, 1, 1);
                    sheet.Cells[r2, c].PutValue(Col[i, 1]);
                    sheet.Cells[r2, c].SetStyle(cellStyle);
                    sheet.Cells[r2, c].Style.Font.IsBold = true;
                }

                c++;
            }
            
            #region
            r = r2 + 1;
            c = 0;

            DataView dv = new DataView(dt);
            DataTable dtTotal = dv.ToTable(true, ColName);
            for (int i = 0; i < dtTotal.Rows.Count; i++)
            {
                Color clr = i % 2 == 0 ? Color.FromArgb(0, 204, 255) : Color.Yellow;

                string dateTimes = Convert.ToDateTime(dtTotal.Rows[i]["RunDay"].ToString()).ToString("yyyy").Equals("1900") ? dt.Rows[i]["RUNDATE"].ToString() : Convert.ToDateTime(dtTotal.Rows[i]["RunDay"].ToString()).ToString("M.dd");
                if (dateTimes.Equals("本月合计"))
                {
                    clr = Color.Red;
                    rowHeight = 31;
                }

                for (int j = 0; j < ColName.Length; j++)
                {

                    sheet.Cells.Merge(r, j, 1, 1);
                    if (j==0)
                    {
                        sheet.Cells[r, j].PutValue(dateTimes);
                    }
                    else
                    {
                        sheet.Cells[r, j].PutValue(dtTotal.Rows[i][j].ToString());
                    }
                    sheet.Cells[r, j].SetStyle(setSheetStyle(workbook, clr));
                    sheet.Cells.SetRowHeight(r, rowHeight);
                }
                r++;
            }
            
            #endregion

            for (int w = 0; w < width.Length; w++)
            {
                sheet.Cells.SetColumnWidth(w, width[w]);
                sheet.Cells[1, w].Style.Font.IsBold = true;
                sheet.Cells[2, w].Style.Font.IsBold = true;
            }

            return sheet;
        }
        #endregion

        #region 设置样式
        private Style setSheetStyle(Workbook workbook, int rowHeight, string cellFontName, int cellFontSize, Color color)
        {
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
            cellStyle.ForegroundColor = color;
            cellStyle.Pattern = BackgroundType.Solid;
            return cellStyle;
        }
        private Style setSheetStyle(Workbook workbook)
        {
            return setSheetStyle(workbook, 15, "宋体", 10, Color.White);
        }
        private Style setSheetStyle(Workbook workbook, Color color)
        {
            return setSheetStyle(workbook, 15, "宋体", 10, color);
        }
        #endregion

    }
}
