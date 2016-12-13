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
    [FluorineFx.RemotingService("招远金城热力实际日耗能量服务")]
    public class ZY_ConsumeEnergy : IController
    {

        #region command texts

        private const string consume_energy_insert_commandText = @"INSERT INTO [ZY_CONSUMEENERGY] (
[EMPLOYEEID], [ADDDATETIME], [NX_GS_WD], [NX_HS_WD], [NX_GS_LL], [NX_HS_LL], [NX_GS_YL], [NX_HS_YL], [NX_RL], [NX_HRQ]
, [ZX_GS_WD], [ZX_HS_WD], [ZX_GS_LL], [ZX_HS_LL], [ZX_GS_YL], [ZX_HS_YL], [ZX_RL], [ZX_HRQ], [BX_GS_WD], [BX_HS_WD], [BX_GS_LL], [BX_HS_LL]
, [BX_GS_YL], [BX_HS_YL], [BX_RL], [BX_HRQ], [BSL_QM_SN], [BSL_QM_SW],[BSL_QM_BS], [BSL_ZM_SN], [BSL_ZM_SW],[BSL_ZM_BS], [BSL_RHJ_SN], [BSL_RHJ_SW],[BSL_RHJ_BS], [DYL_QM_XHB]
, [DYL_QM_NXHRQ], [DYL_QM_DBXHRQ], [DYL_QM_WHXC], [DYL_ZM_XHB], [DYL_ZM_NXHRQ], [DYL_ZM_DBXHRQ], [DYL_ZM_WHXC], [DYL_RHJ_XHB]
, [DYL_RHJ_NXHRQ], [DYL_RHJ_DBXHRQ], [DYL_RHJ_WHXC], [YQL_QM_DBX], [YQL_QM_NX], [YQL_ZM_DBX], [YQL_ZM_NX], [YQL_RHJ_DBX], [YQL_RHJ_NX]) 
VALUES (@EMPLOYEEID, @ADDDATETIME, @NX_GS_WD, @NX_HS_WD, @NX_GS_LL, @NX_HS_LL, @NX_GS_YL, @NX_HS_YL, @NX_RL, @NX_HRQ, 
@ZX_GS_WD, @ZX_HS_WD, @ZX_GS_LL, @ZX_HS_LL, @ZX_GS_YL, @ZX_HS_YL, @ZX_RL, @ZX_HRQ, @BX_GS_WD, @BX_HS_WD, @BX_GS_LL, @BX_HS_LL, 
@BX_GS_YL, @BX_HS_YL, @BX_RL, @BX_HRQ, @BSL_QM_SN, @BSL_QM_SW,@BSL_QM_BS, @BSL_ZM_SN, @BSL_ZM_SW,@BSL_ZM_BS, @BSL_RHJ_SN, @BSL_RHJ_SW,@BSL_RHJ_BS, @DYL_QM_XHB, 
@DYL_QM_NXHRQ, @DYL_QM_DBXHRQ, @DYL_QM_WHXC, @DYL_ZM_XHB, @DYL_ZM_NXHRQ, @DYL_ZM_DBXHRQ, @DYL_ZM_WHXC, @DYL_RHJ_XHB, 
@DYL_RHJ_NXHRQ, @DYL_RHJ_DBXHRQ, @DYL_RHJ_WHXC, @YQL_QM_DBX, @YQL_QM_NX, @YQL_ZM_DBX, @YQL_ZM_NX, @YQL_RHJ_DBX, @YQL_RHJ_NX)";

        private const string consume_energy_update_commandText = @"UPDATE [ZY_CONSUMEENERGY] SET [ADDDATETIME]=@ADDDATETIME, 
[NX_GS_WD]=@NX_GS_WD, [NX_HS_WD]=@NX_HS_WD, [NX_GS_LL]=@NX_GS_LL, [NX_HS_LL]=@NX_HS_LL, [NX_GS_YL]=@NX_GS_YL, [NX_HS_YL]=@NX_HS_YL, [NX_RL]=@NX_RL, [NX_HRQ]=@NX_HRQ, 
[ZX_GS_WD]=@ZX_GS_WD, [ZX_HS_WD]=@ZX_HS_WD, [ZX_GS_LL]=@ZX_GS_LL, [ZX_HS_LL]=@ZX_HS_LL, [ZX_GS_YL]=@ZX_GS_YL, [ZX_HS_YL]=@ZX_HS_YL, [ZX_RL]=@ZX_RL, [ZX_HRQ]=@ZX_HRQ, [BX_GS_WD]=@BX_GS_WD, [BX_HS_WD]=@BX_HS_WD, [BX_GS_LL]=@BX_GS_LL, [BX_HS_LL]=@BX_HS_LL, 
[BX_GS_YL]=@BX_GS_YL, [BX_HS_YL]=@BX_HS_YL, [BX_RL]=@BX_RL, [BX_HRQ]=@BX_HRQ, [BSL_QM_SN]=@BSL_QM_SN, [BSL_QM_SW]=@BSL_QM_SW,[BSL_QM_BS]=@BSL_QM_BS, [BSL_ZM_SN]=@BSL_ZM_SN, [BSL_ZM_SW]=@BSL_ZM_SW,[BSL_ZM_BS]=@BSL_ZM_BS, [BSL_RHJ_SN]=@BSL_RHJ_SN, [BSL_RHJ_SW]=@BSL_RHJ_SW, [BSL_RHJ_BS]=@BSL_RHJ_BS, [DYL_QM_XHB]=@DYL_QM_XHB, 
[DYL_QM_NXHRQ]=@DYL_QM_NXHRQ, [DYL_QM_DBXHRQ]=@DYL_QM_DBXHRQ, [DYL_QM_WHXC]=@DYL_QM_WHXC, 
[DYL_ZM_XHB]=@DYL_ZM_XHB, [DYL_ZM_NXHRQ]=@DYL_ZM_NXHRQ, [DYL_ZM_DBXHRQ]=@DYL_ZM_DBXHRQ, [DYL_ZM_WHXC]=@DYL_ZM_WHXC, 
[DYL_RHJ_XHB]=@DYL_RHJ_XHB,  [DYL_RHJ_NXHRQ]=@DYL_RHJ_NXHRQ, [DYL_RHJ_DBXHRQ]=@DYL_RHJ_DBXHRQ, [DYL_RHJ_WHXC]=@DYL_RHJ_WHXC, 
[YQL_QM_DBX]=@YQL_QM_DBX, [YQL_QM_NX]=@YQL_QM_NX, 
[YQL_ZM_DBX]=@YQL_ZM_DBX, [YQL_ZM_NX]=@YQL_ZM_NX, [YQL_RHJ_DBX]=@YQL_RHJ_DBX, [YQL_RHJ_NX]=@YQL_RHJ_NX WHERE [ID]=@ID";

        private const string consume_energy_delete_commandText = "DELETE [ZY_CONSUMEENERGY] WHERE [ID] IN ({0})";

        private const string consume_energy_details_commandText = @"SELECT [ID], [EMPLOYEEID], [STATE], 
CONVERT(VARCHAR(19),[ADDDATETIME],120) AS [ADDDATETIME]
, [NX_GS_WD], [NX_HS_WD], [NX_GS_LL], [NX_HS_LL], [NX_GS_YL], [NX_HS_YL]
, [NX_RL], [NX_HRQ], [ZX_GS_WD], [ZX_HS_WD], [ZX_GS_LL]
, [ZX_HS_LL], [ZX_GS_YL], [ZX_HS_YL], [ZX_RL], [ZX_HRQ]
, [BX_GS_WD], [BX_HS_WD], [BX_GS_LL], [BX_HS_LL], [BX_GS_YL]
, [BX_HS_YL], [BX_RL], [BX_HRQ], [BSL_QM_SN], [BSL_QM_SW], [BSL_QM_BS]
, [BSL_ZM_SN], [BSL_ZM_SW],[BSL_ZM_BS], [BSL_RHJ_SN], [BSL_RHJ_SW],[BSL_RHJ_BS], [DYL_QM_XHB]
, [DYL_QM_NXHRQ], [DYL_QM_DBXHRQ], [DYL_QM_WHXC], [DYL_ZM_XHB]
, [DYL_ZM_NXHRQ], [DYL_ZM_DBXHRQ], [DYL_ZM_WHXC], [DYL_RHJ_XHB]
, [DYL_RHJ_NXHRQ], [DYL_RHJ_DBXHRQ], [DYL_RHJ_WHXC], [YQL_QM_DBX]
, [YQL_QM_NX], [YQL_ZM_DBX], [YQL_ZM_NX], [YQL_RHJ_DBX], [YQL_RHJ_NX]
  FROM [ZY_CONSUMEENERGY] WHERE [ID]=@ID";

        private const string consume_energy_select_commandText = @"SELECT ROW_NUMBER() OVER (ORDER BY [ADDDATETIME] DESC) AS NUM1, 
[ID], CONVERT(VARCHAR(19),[ADDDATETIME],120) AS [ADDDATETIME],
([NX_RL]+[ZX_RL]+[BX_RL]) AS RL, ([NX_HRQ]+[ZX_HRQ]+[BX_HRQ]) AS HRQ,
[BSL_RHJ_SN], [BSL_RHJ_SW],[BSL_RHJ_BS],
[DYL_RHJ_XHB], [DYL_RHJ_NXHRQ], [DYL_RHJ_DBXHRQ], [DYL_RHJ_WHXC],
[YQL_RHJ_DBX], [YQL_RHJ_NX]
FROM [ZY_CONSUMEENERGY]{0}";

        private const string consume_energy_total_commandText = "SELECT COUNT(*) FROM [ZY_CONSUMEENERGY]{0}";


        private const string consume_energy_lock_commandText = "UPDATE [ZY_CONSUMEENERGY] SET [STATE]=1 WHERE [ID] IN ({0})";


        private const string consume_energy_state_commandText = "SELECT [STATE] FROM [ZY_CONSUMEENERGY] WHERE [ID]=@ID";


        private const string export_file1_commandText = @"SELECT  CONVERT(VARCHAR(19),[ADDDATETIME],120) AS [ADDDATETIME],
NX_GS_WD, NX_HS_WD, (NX_GS_WD-NX_HS_WD) AS NX_WD_CZ,
ZX_GS_WD, ZX_HS_WD, (ZX_GS_WD-ZX_HS_WD) AS ZX_WD_CZ,
BX_GS_WD, BX_HS_WD, (BX_GS_WD-BX_HS_WD) AS BX_WD_CZ,
NX_GS_LL, NX_HS_LL, (NX_GS_LL-NX_HS_LL) AS NX_LL_CZ,
ZX_GS_LL, ZX_HS_LL, (ZX_GS_LL-ZX_HS_LL) AS ZX_LL_CZ,
BX_GS_LL, BX_HS_LL, (BX_GS_LL-BX_HS_LL) AS BX_LL_CZ,
NX_GS_YL, NX_HS_YL, (NX_GS_YL-NX_HS_YL) AS NX_YL_CZ,
ZX_GS_YL, ZX_HS_YL, (ZX_GS_YL-ZX_HS_YL) AS ZX_YL_CZ,
BX_GS_YL, BX_HS_YL, (BX_GS_YL-BX_HS_YL) AS BX_YL_CZ
FROM [ZY_CONSUMEENERGY] 
WHERE [ADDDATETIME]>='{0}' AND [ADDDATETIME]<='{1}' 
UNION ALL 
SELECT '合计',SUM(NX_GS_WD),SUM(NX_HS_WD),SUM(NX_GS_WD-NX_HS_WD),
SUM(ZX_GS_WD), SUM(ZX_HS_WD), SUM(ZX_GS_WD-ZX_HS_WD),
SUM(BX_GS_WD), SUM(BX_HS_WD), SUM(BX_GS_WD-BX_HS_WD),
SUM(NX_GS_LL), SUM(NX_HS_LL), SUM(NX_GS_LL-NX_HS_LL),
SUM(ZX_GS_LL), SUM(ZX_HS_LL), SUM(ZX_GS_LL-ZX_HS_LL),
SUM(BX_GS_LL), SUM(BX_HS_LL), SUM(BX_GS_LL-BX_HS_LL),
SUM(NX_GS_YL), SUM(NX_HS_YL), SUM(NX_GS_YL-NX_HS_YL) ,
SUM(ZX_GS_YL), SUM(ZX_HS_YL), SUM(ZX_GS_YL-ZX_HS_YL),
SUM(BX_GS_YL), SUM(BX_HS_YL), SUM(BX_GS_YL-BX_HS_YL)
FROM [ZY_CONSUMEENERGY] 
WHERE [ADDDATETIME]>='{0}' AND [ADDDATETIME]<='{1}' 
ORDER BY [ADDDATETIME]";


        private const string export_file2_commandText = @"SELECT CONVERT(VARCHAR(19),[ADDDATETIME],120) AS [ADDDATETIME],
NX_RL, NX_HRQ, (NX_RL+NX_HRQ) AS NX_XJ,
ZX_RL, ZX_HRQ, (ZX_RL + ZX_HRQ) AS ZX_XJ,
BX_RL, BX_HRQ, (BX_RL + BX_HRQ) AS BX_XJ,
((NX_RL + NX_HRQ) + (ZX_RL + ZX_HRQ) + (BX_RL + BX_HRQ)) AS RLHJ,
BSL_RHJ_SN, BSL_RHJ_SW,BSL_RHJ_BS, (BSL_RHJ_SN + BSL_RHJ_SW+BSL_RHJ_BS) AS BSL_HJ,
DYL_RHJ_XHB, DYL_RHJ_NXHRQ, DYL_RHJ_DBXHRQ, DYL_RHJ_WHXC,
(DYL_RHJ_XHB + DYL_RHJ_NXHRQ + DYL_RHJ_DBXHRQ + DYL_RHJ_WHXC) AS DYL_HJ,
YQL_RHJ_DBX, YQL_RHJ_NX, (YQL_RHJ_DBX + YQL_RHJ_NX) AS YQL_HJ
FROM ZY_CONSUMEENERGY
WHERE ADDDATETIME >= '{0}' AND ADDDATETIME <= '{1}'
UNION ALL 
SELECT '合计',
SUM(NX_RL), SUM(NX_HRQ), SUM(NX_RL+NX_HRQ),
SUM(ZX_RL), SUM(ZX_HRQ), SUM(ZX_RL + ZX_HRQ),
SUM(BX_RL), SUM(BX_HRQ), SUM(BX_RL + BX_HRQ),
SUM((NX_RL + NX_HRQ) + (ZX_RL + ZX_HRQ) + (BX_RL + BX_HRQ)),
SUM(BSL_RHJ_SN), SUM(BSL_RHJ_SW),SUM(BSL_RHJ_BS), SUM(BSL_RHJ_SN + BSL_RHJ_SW+BSL_RHJ_BS),
SUM(DYL_RHJ_XHB), SUM(DYL_RHJ_NXHRQ), SUM(DYL_RHJ_DBXHRQ), SUM(DYL_RHJ_WHXC),
SUM(DYL_RHJ_XHB + DYL_RHJ_NXHRQ + DYL_RHJ_DBXHRQ + DYL_RHJ_WHXC),
SUM(YQL_RHJ_DBX), SUM(YQL_RHJ_NX), SUM(YQL_RHJ_DBX + YQL_RHJ_NX)
FROM ZY_CONSUMEENERGY
WHERE ADDDATETIME >= '{0}' AND ADDDATETIME <= '{1}'
ORDER BY ADDDATETIME";


        #endregion


        public string addDataItem(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Result.getFaultXml(Error.XML_IS_NULL);

            if (text.Contains("null"))
                text = text.Replace("null", string.Empty);

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

            try
            {
                //验证是否已经重复上报了
                string count = new DataAccessHandler().executeScalarResult(
                    "SELECT COUNT(*) FROM [ZY_CONSUMEENERGY] WHERE [ADDDATETIME]=@ADDDATETIME AND EMPLOYEEID=@EMPLOYEEID",
                    SqlServer.GetParameter(xml, new string[] { "ADDDATETIME", "EMPLOYEEID" }));
                if (Convert.ToInt32(count) == 1)
                {
                    throw new Exception(string.Format("已经上报了今天（{0}）的实际日耗能量信息。", xml.Element("ADDDATETIME").Value));
                }

                result = new DataAccessHandler().executeNonQueryResult(
                    consume_energy_insert_commandText,
                    SqlServer.GetParameter(xml, new string[] {
                        "EMPLOYEEID", "ADDDATETIME",
                        "NX_GS_WD", "NX_HS_WD", "NX_GS_LL", "NX_HS_LL", "NX_GS_YL", "NX_HS_YL", "NX_RL", "NX_HRQ",
                        "ZX_GS_WD", "ZX_HS_WD", "ZX_GS_LL", "ZX_HS_LL", "ZX_GS_YL", "ZX_HS_YL", "ZX_RL", "ZX_HRQ",
                        "BX_GS_WD", "BX_HS_WD", "BX_GS_LL", "BX_HS_LL", "BX_GS_YL", "BX_HS_YL", "BX_RL", "BX_HRQ",
                        "BSL_QM_SN", "BSL_QM_SW",  "BSL_QM_BS", "BSL_ZM_SN", "BSL_ZM_SW","BSL_ZM_BS", "BSL_RHJ_SN", "BSL_RHJ_SW", "BSL_RHJ_BS",
                        "DYL_QM_XHB", "DYL_QM_NXHRQ", "DYL_QM_DBXHRQ", "DYL_QM_WHXC",
                        "DYL_ZM_XHB", "DYL_ZM_NXHRQ", "DYL_ZM_DBXHRQ", "DYL_ZM_WHXC",
                        "DYL_RHJ_XHB", "DYL_RHJ_NXHRQ", "DYL_RHJ_DBXHRQ", "DYL_RHJ_WHXC",
                        "YQL_QM_DBX", "YQL_QM_NX", "YQL_ZM_DBX", "YQL_ZM_NX", "YQL_RHJ_DBX", "YQL_RHJ_NX"
                    }));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = null;
            xml = null;

            return result;
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
            try
            {
                result = new DataAccessHandler().executeNonQueryResult(
                    string.Format(consume_energy_delete_commandText, xml.Element("ID").Value),
                    null);
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

            try
            {
                result = new DataAccessHandler().executeNonQueryResult(
                    consume_energy_update_commandText,
                    SqlServer.GetParameter(xml, new string[] {
                        "ID", "ADDDATETIME",
                        "NX_GS_WD", "NX_HS_WD", "NX_GS_LL", "NX_HS_LL", "NX_GS_YL", "NX_HS_YL", "NX_RL", "NX_HRQ",
                        "ZX_GS_WD", "ZX_HS_WD", "ZX_GS_LL", "ZX_HS_LL", "ZX_GS_YL", "ZX_HS_YL", "ZX_RL", "ZX_HRQ",
                        "BX_GS_WD", "BX_HS_WD", "BX_GS_LL", "BX_HS_LL", "BX_GS_YL", "BX_HS_YL", "BX_RL", "BX_HRQ",
                        "BSL_QM_SN", "BSL_QM_SW", "BSL_QM_BS", "BSL_ZM_SN", "BSL_ZM_SW","BSL_ZM_BS", "BSL_RHJ_SN", "BSL_RHJ_SW","BSL_RHJ_BS",
                        "DYL_QM_XHB", "DYL_QM_NXHRQ", "DYL_QM_DBXHRQ", "DYL_QM_WHXC",
                        "DYL_ZM_XHB", "DYL_ZM_NXHRQ", "DYL_ZM_DBXHRQ", "DYL_ZM_WHXC",
                        "DYL_RHJ_XHB", "DYL_RHJ_NXHRQ", "DYL_RHJ_DBXHRQ", "DYL_RHJ_WHXC",
                        "YQL_QM_DBX", "YQL_QM_NX", "YQL_ZM_DBX", "YQL_ZM_NX", "YQL_RHJ_DBX", "YQL_RHJ_NX"
                    }
                ));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = null;
            xml = null;

            return result;
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

            //判断是否是管理员用户，是的话，则查询记录，不是，则添加EMPLOYEEID=用户编号的条件
            string employeeID = xml.Element("EMPLOYEEID").Value.ToUpper();
            if (!PushOrderAdminUtils.isAdmin(employeeID))
            {
                if (string.IsNullOrEmpty(whereText))
                {
                    whereText = string.Format(" WHERE EMPLOYEEID='{0}'", employeeID);
                }
                else
                {
                    whereText = string.Format("{0} AND {1}", whereText, string.Format("EMPLOYEEID='{0}'", employeeID));
                }
            }

            //只显示本月内的数据
            DateTime dateTimeNow = DateTime.Now;
            DateTime dateTime1 = new DateTime(dateTimeNow.Year, dateTimeNow.Month, 1);//第一天
            DateTime dateTime2 = dateTime1.AddMonths(1).AddDays(-1);//最后一天
            if (string.IsNullOrEmpty(whereText))
            {
                whereText = string.Format(" WHERE [ADDDATETIME] BETWEEN '{0}'AND '{1}'", dateTime1.ToString("yyyy-MM-dd"), dateTime2.ToString("yyyy-MM-dd"));
            }
            else
            {
                whereText = string.Format("{0} AND {1}", whereText, string.Format("[ADDDATETIME] BETWEEN '{0}'AND '{1}'", dateTime1.ToString("yyyy-MM-dd"), dateTime2.ToString("yyyy-MM-dd")));
            }

            string commandText = string.Format(consume_energy_select_commandText, whereText);

            DataSet dataSetConsumeEnergy = null;
            try
            {
                dataSetConsumeEnergy = new DataAccessHandler().executeDatasetResult(
                    SqlServerCommandText.createPagingCommandText(ref commandText, ref xml), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;

            if (dataSetConsumeEnergy != null && dataSetConsumeEnergy.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemXml(ref dataSetConsumeEnergy, getDataItemTotal(ref whereText)));
            }
            else
            {
                return Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = whereText = commandText = null;
            xml = null;
            dataSetConsumeEnergy = null;


            return result;
        }

        private string getDataItemXml(ref DataSet dataSetConsumeEnergy, string total)
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendFormat("<DATAS COUNT=\"{0}\" TOTAL=\"{1}\">", dataSetConsumeEnergy.Tables[0].Rows.Count, total);
            ZY_ConsumeEnergyObject consumeEnergyObject = null;
            foreach (DataRow row in dataSetConsumeEnergy.Tables[0].Rows)
            {
                consumeEnergyObject = new ZY_ConsumeEnergyObject(row);
                xml.AppendFormat("<DATA NUM=\"{0}\" ID=\"{1}\" ADDDATETIME=\"{2}\" RL=\"{3}\" HRQ=\"{4}\" BSL_RHJ_SN=\"{5}\" BSL_RHJ_SW=\"{6}\" BSL_RHJ_BS=\"{7}\" DYL_RHJ_XHB=\"{8}\" DYL_RHJ_NXHRQ=\"{9}\" DYL_RHJ_DBXHRQ=\"{10}\" DYL_RHJ_WHXC=\"{11}\" YQL_RHJ_DBX=\"{12}\" YQL_RHJ_NX=\"{13}\"/>",
                    consumeEnergyObject.NUM,
                    consumeEnergyObject.ID,
                    consumeEnergyObject.ADDDATETIME,
                   consumeEnergyObject.RL,
                   consumeEnergyObject.HRQ,
                   consumeEnergyObject.BSL_RHJ_SN,
                   consumeEnergyObject.BSL_RHJ_SW,
                   consumeEnergyObject.BSL_RHJ_BS,
                   consumeEnergyObject.DYL_RHJ_XHB,
                   consumeEnergyObject.DYL_RHJ_NXHRQ,
                   consumeEnergyObject.DYL_RHJ_DBXHRQ,
                   consumeEnergyObject.DYL_RHJ_WHXC,
                   consumeEnergyObject.YQL_RHJ_DBX,
                   consumeEnergyObject.YQL_RHJ_NX
                );
            }
            xml.Append("</DATAS>");

            consumeEnergyObject = null;

            return xml.ToString();
        }

        private string getDataItemTotal(ref string whereText)
        {
            return new DataAccessHandler().executeScalarResult(
                string.Format(consume_energy_total_commandText, whereText), null);
        }

        public string getDataItemDetails(string text)
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

            DataSet dataSetConsumeEnergy = null;
            try
            {
                dataSetConsumeEnergy = new DataAccessHandler().executeDatasetResult(
                    consume_energy_details_commandText,
                    SqlServer.GetParameter("ID", xml.Element("ID").Value));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetConsumeEnergy != null && dataSetConsumeEnergy.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemDetailsXml(ref dataSetConsumeEnergy));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = null;
            xml = null;
            dataSetConsumeEnergy = null;

            return result;

        }

        private string getDataItemDetailsXml(ref DataSet dataSetConsumeEnergy)
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendFormat("<DATAS>");
            ZY_ConsumeEnergyObject consumeEnergyObject = null;
            foreach (DataRow row in dataSetConsumeEnergy.Tables[0].Rows)
            {
                consumeEnergyObject = new ZY_ConsumeEnergyObject(row);
                xml.AppendFormat("<DATA ID=\"{0}\" ADDDATETIME=\"{1}\" NX_GS_WD=\"{2}\" NX_HS_WD=\"{3}\" NX_GS_LL=\"{4}\" NX_HS_LL=\"{5}\" NX_GS_YL=\"{6}\" NX_HS_YL=\"{7}\" NX_RL=\"{8}\" NX_HRQ=\"{9}\" ZX_GS_WD=\"{10}\" ZX_HS_WD=\"{11}\" ZX_GS_LL=\"{12}\" ZX_HS_LL=\"{13}\" ZX_GS_YL=\"{14}\" ZX_HS_YL=\"{15}\" ZX_RL=\"{16}\" ZX_HRQ=\"{17}\" BX_GS_WD=\"{18}\" BX_HS_WD=\"{19}\" BX_GS_LL=\"{20}\" BX_HS_LL=\"{21}\" BX_GS_YL=\"{22}\" BX_HS_YL=\"{23}\" BX_RL=\"{24}\" BX_HRQ=\"{25}\" BSL_QM_SN=\"{26}\" BSL_QM_SW=\"{27}\" BSL_ZM_SN=\"{28}\" BSL_ZM_SW=\"{29}\" BSL_RHJ_SN=\"{30}\" BSL_RHJ_SW=\"{31}\" DYL_QM_XHB=\"{32}\" DYL_QM_NXHRQ=\"{33}\" DYL_QM_DBXHRQ=\"{34}\" DYL_QM_WHXC=\"{35}\" DYL_ZM_XHB=\"{36}\" DYL_ZM_NXHRQ=\"{37}\" DYL_ZM_DBXHRQ=\"{38}\" DYL_ZM_WHXC=\"{39}\" DYL_RHJ_XHB=\"{40}\" DYL_RHJ_NXHRQ=\"{41}\" DYL_RHJ_DBXHRQ=\"{42}\" DYL_RHJ_WHXC=\"{43}\" YQL_QM_DBX=\"{44}\" YQL_QM_NX=\"{45}\" YQL_ZM_DBX=\"{46}\" YQL_ZM_NX=\"{47}\" YQL_RHJ_DBX=\"{48}\" YQL_RHJ_NX=\"{49}\" BSL_QM_BS=\"{50}\" BSL_ZM_BS=\"{51}\" BSL_RHJ_BS=\"{52}\"/>",
                    consumeEnergyObject.ID,
                    consumeEnergyObject.ADDDATETIME,
                    consumeEnergyObject.NX_GS_WD,
                    consumeEnergyObject.NX_HS_WD,
                    consumeEnergyObject.NX_GS_LL,
                    consumeEnergyObject.NX_HS_LL,
                    consumeEnergyObject.NX_GS_YL,
                    consumeEnergyObject.NX_HS_YL,
                    consumeEnergyObject.NX_RL,
                    consumeEnergyObject.NX_HRQ,
                    consumeEnergyObject.ZX_GS_WD,
                    consumeEnergyObject.ZX_HS_WD,
                    consumeEnergyObject.ZX_GS_LL,
                    consumeEnergyObject.ZX_HS_LL,
                    consumeEnergyObject.ZX_GS_YL,
                    consumeEnergyObject.ZX_HS_YL,
                    consumeEnergyObject.ZX_RL,
                    consumeEnergyObject.ZX_HRQ,
                    consumeEnergyObject.BX_GS_WD,
                    consumeEnergyObject.BX_HS_WD,
                    consumeEnergyObject.BX_GS_LL,
                    consumeEnergyObject.BX_HS_LL,
                    consumeEnergyObject.BX_GS_YL,
                    consumeEnergyObject.BX_HS_YL,
                    consumeEnergyObject.BX_RL,
                    consumeEnergyObject.BX_HRQ,
                    consumeEnergyObject.BSL_QM_SN,
                    consumeEnergyObject.BSL_QM_SW,
                   
                    consumeEnergyObject.BSL_ZM_SN,
                    consumeEnergyObject.BSL_ZM_SW,
                     
                    consumeEnergyObject.BSL_RHJ_SN,
                    consumeEnergyObject.BSL_RHJ_SW,
                    
                    consumeEnergyObject.DYL_QM_XHB,
                    consumeEnergyObject.DYL_QM_NXHRQ,
                    consumeEnergyObject.DYL_QM_DBXHRQ,
                    consumeEnergyObject.DYL_QM_WHXC,
                    consumeEnergyObject.DYL_ZM_XHB,
                    consumeEnergyObject.DYL_ZM_NXHRQ,
                    consumeEnergyObject.DYL_ZM_DBXHRQ,
                    consumeEnergyObject.DYL_ZM_WHXC,
                    consumeEnergyObject.DYL_RHJ_XHB,
                    consumeEnergyObject.DYL_RHJ_NXHRQ,
                    consumeEnergyObject.DYL_RHJ_DBXHRQ,
                    consumeEnergyObject.DYL_RHJ_WHXC,
                    consumeEnergyObject.YQL_QM_DBX,
                    consumeEnergyObject.YQL_QM_NX,
                    consumeEnergyObject.YQL_ZM_DBX,
                    consumeEnergyObject.YQL_ZM_NX,
                    consumeEnergyObject.YQL_RHJ_DBX,
                    consumeEnergyObject.YQL_RHJ_NX,
                     consumeEnergyObject.BSL_QM_BS,
                     consumeEnergyObject.BSL_ZM_BS,
                     consumeEnergyObject.BSL_RHJ_BS
                );
            }
            xml.AppendFormat("</DATAS>");

            consumeEnergyObject = null;

            return xml.ToString();
        }


        public string getServerDateTime()
        {
            DateTime dateTimeNow = DateTime.Now;

            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            xml.AppendFormat("<DATA YEAR=\"{0}\" MONTH=\"{1}\" DAY=\"{2}\" />",
                dateTimeNow.Year,
                dateTimeNow.Month,
                dateTimeNow.Day
            );
            xml.Append("</DATAS>");

            return Result.getResultXml(xml.ToString());
        }


        public string addLockAndExportButton(string text)
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
            
            StringBuilder result = new StringBuilder();
            result.Append("<DATAS>");
            result.AppendFormat("<DATA ISADDBUTTON=\"{0}\"/>", PushOrderAdminUtils.isAdmin(xml.Element("EMPLOYEEID").Value.ToUpper()) ? "TRUE" : "FALSE");
            //result.AppendFormat("<DATA ISADDBUTTON=\"{0}\"/>", listPushOrderAdminID.Contains(xml.Element("EMPLOYEEID").Value.ToUpper()) ? "TRUE" : "FALSE");
            result.Append("</DATAS>");

            text = null;
            xml = null;

            return Result.getResultXml(result.ToString());
        }


        public string lockDataItem(string text)
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
            try
            {
                result = new DataAccessHandler().executeNonQueryResult(
                    string.Format(consume_energy_lock_commandText, xml.Element("ID").Value), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = null;
            xml = null;

            return result;
        }


        public string getDataItemState(string text)
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

            string state = string.Empty;
            try
            {
                state = new DataAccessHandler().executeScalarResult(
                    consume_energy_state_commandText,
                    SqlServer.GetParameter("ID", xml.Element("ID").Value.ToUpper()));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (!string.IsNullOrEmpty(state))
            {
                result = Result.getResultXml(getDataItemStateXml(ref state));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = state = null;
            xml = null;

            return result;
        }

        private string getDataItemStateXml(ref string state)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            xml.AppendFormat("<DATA STATE=\"{0}\"/>", state);
            xml.Append("</DATAS>");

            return xml.ToString();
        }



        public string getExportDateTime()
        {
            DateTime now = DateTime.Now;
            DateTime d1 = new DateTime(now.Year, now.Month, 1);
            DateTime d2 = d1.AddMonths(1).AddDays(-1);

            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            xml.AppendFormat("<DATA STARTDATETIME=\"{0}\" ENDDATETIME=\"{1}\"/>",
                d1.ToString("yyyy-MM-dd"),
                d2.ToString("yyyy-MM-dd"));
            xml.Append("</DATAS>");

            return Result.getResultXml(xml.ToString());
        }


        public string exportExcelFile(string text)
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

            string[] tempDateTime = xml.Element("DATETIME").Value.Split('|');
            string startDateTime = tempDateTime[0];
            string endDateTime = tempDateTime[1];

            string commandText = string.Empty;

            string type = xml.Element("TYPE").Value;
            switch (type)
            {
                case "FILE1"://温度、流量、压力
                    commandText = string.Format(export_file1_commandText, startDateTime, endDateTime);
                    break;
                case "FILE2"://热量、补水量、用电量、用汽量
                    commandText = string.Format(export_file2_commandText, startDateTime, endDateTime);
                    break;
                default:
                    break;
            }

            DataSet dataSetExport = null;
            try
            {
                dataSetExport = new DataAccessHandler().executeDatasetResult(commandText, null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }


            string result = string.Empty;
            if (dataSetExport != null && dataSetExport.Tables.Count > 0)
            {
                //生成导出excel文件，然后返回http下载地址
                result = exportExcelFileHandler(ref dataSetExport, ref type, ref startDateTime, ref endDateTime);
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = commandText = startDateTime = endDateTime = type = null;
            xml = null;
            dataSetExport = null;


            return result;
        }

        /// <summary>
        /// 导出Excel的方法
        /// </summary>
        /// <param name="dataSetExport">需要导出的数据</param>
        /// <param name="type">导出类型（FILE1,FILE2）</param>
        /// <param name="startDateTime">数据查询开始时间</param>
        /// <param name="endDateTime">数据查询结束时间</param>
        /// <returns>返回ExcelHttp下载地址；返回前段显示的文件名称</returns>
        private string exportExcelFileHandler(ref DataSet dataSetExport, ref string type, ref string startDateTime, ref string endDateTime)
        {
            //没有数据时，直接返回空的字符串
            if (dataSetExport.Tables.Count <= 0)
            {
                return string.Empty;
            }
            

            //读取数据
            DataTable dataTable = dataSetExport.Tables[0];

            // 列的数量
            int colCount = dataTable.Columns.Count;
            //行的数量
            int rowCount = dataTable.Rows.Count;


            //注册授权文件
            Aspose.Cells.License license = new Aspose.Cells.License();
            try
            {
                license.SetLicense(Config.ExportLicenseUrl);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }
            
            //初始化组件变量
            Workbook workbook = new Workbook();//工作簿
            Worksheet sheet = workbook.Worksheets[0];//工作表

            //Sheet表名
            sheet.Name = "实际日耗能量统计表";//表名

            int rowHeight = 15;//行高

            string cellFontName = "宋体";//字体
            int cellFontSize = 9;//字体大小

            string fileName = createExcelTitle(ref startDateTime, ref endDateTime, ref type);

            #region 样式定义

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

            #endregion
            
            switch (type)
            {
                case "FILE1":
                    #region 温度、流量、压力
                    //cellSheet.Cells[行, 列]
                    //合并单元格后，也是需要对合并的第一个进行赋值；
                    //然后再对合并单元格内的所有格进行样式赋值

                    //处理列头
                    #region 金城热力公司15-16年度11月份实际日耗能量统计表

                    sheet.Cells.Merge(0, 0, 1, 28);
                    sheet.Cells[0, 0].PutValue(fileName.Substring(0, fileName.Length - 1));
                    sheet.Cells[0, 0].Style.Font.IsBold = true;
                    sheet.Cells[0, 0].Style.Font.Size = 18;
                    sheet.Cells[0, 0].Style.Font.Name = cellFontName;
                    sheet.Cells[0, 0].Style.HorizontalAlignment = TextAlignmentType.Center;
                    sheet.Cells.SetRowHeight(0, 25);

                    #endregion

                    #region 日期

                    //日期
                    sheet.Cells.Merge(1, 0, 3, 1);
                    sheet.Cells[1, 0].PutValue("日期");
                    sheet.Cells[1, 0].SetStyle(cellStyle);
                    sheet.Cells[2, 0].SetStyle(cellStyle);
                    sheet.Cells[3, 0].SetStyle(cellStyle);
                    sheet.Cells.SetRowHeight(1, rowHeight);
                    sheet.Cells.SetRowHeight(2, rowHeight);
                    sheet.Cells.SetRowHeight(3, rowHeight);

                    #endregion

                    #region 供回水温度			

                    //供回水温度	
                    sheet.Cells.Merge(1, 1, 1, 9);
                    sheet.Cells[1, 1].PutValue("供回水温度");
                    sheet.Cells[1, 1].SetStyle(cellStyle);
                    sheet.Cells[1, 2].SetStyle(cellStyle);
                    sheet.Cells[1, 3].SetStyle(cellStyle);
                    sheet.Cells[1, 4].SetStyle(cellStyle);
                    sheet.Cells[1, 5].SetStyle(cellStyle);
                    sheet.Cells[1, 6].SetStyle(cellStyle);
                    sheet.Cells[1, 7].SetStyle(cellStyle);
                    sheet.Cells[1, 8].SetStyle(cellStyle);
                    sheet.Cells[1, 9].SetStyle(cellStyle);

                    //南线
                    sheet.Cells.Merge(2, 1, 1, 3);
                    sheet.Cells[2, 1].PutValue("南线");
                    sheet.Cells[2, 1].SetStyle(cellStyle);
                    sheet.Cells[2, 2].SetStyle(cellStyle);
                    sheet.Cells[2, 3].SetStyle(cellStyle);

                    //南线 - 供   回   差
                    sheet.Cells[3, 1].PutValue("供");
                    sheet.Cells[3, 1].SetStyle(cellStyle);
                    sheet.Cells[3, 2].PutValue("回");
                    sheet.Cells[3, 2].SetStyle(cellStyle);
                    sheet.Cells[3, 3].PutValue("差");
                    sheet.Cells[3, 3].SetStyle(cellStyle);


                    //中线
                    sheet.Cells.Merge(2, 4, 1, 3);
                    sheet.Cells[2, 4].PutValue("中线");
                    sheet.Cells[2, 4].SetStyle(cellStyle);
                    sheet.Cells[2, 5].SetStyle(cellStyle);
                    sheet.Cells[2, 6].SetStyle(cellStyle);

                    //中线 - 供   回   差
                    sheet.Cells[3, 4].PutValue("供");
                    sheet.Cells[3, 4].SetStyle(cellStyle);
                    sheet.Cells[3, 5].PutValue("回");
                    sheet.Cells[3, 5].SetStyle(cellStyle);
                    sheet.Cells[3, 6].PutValue("差");
                    sheet.Cells[3, 6].SetStyle(cellStyle);


                    //北线
                    sheet.Cells.Merge(2, 7, 1, 3);
                    sheet.Cells[2, 7].PutValue("北线");
                    sheet.Cells[2, 7].SetStyle(cellStyle);
                    sheet.Cells[2, 8].SetStyle(cellStyle);
                    sheet.Cells[2, 9].SetStyle(cellStyle);

                    //北线 - 供   回   差
                    sheet.Cells[3, 7].PutValue("供");
                    sheet.Cells[3, 7].SetStyle(cellStyle);
                    sheet.Cells[3, 8].PutValue("回");
                    sheet.Cells[3, 8].SetStyle(cellStyle);
                    sheet.Cells[3, 9].PutValue("差");
                    sheet.Cells[3, 9].SetStyle(cellStyle);


                    #endregion

                    #region 供回水流量

                    //供回水流量
                    sheet.Cells.Merge(1, 10, 1, 9);
                    sheet.Cells[1, 10].PutValue("供回水流量");
                    sheet.Cells[1, 10].SetStyle(cellStyle);
                    sheet.Cells[1, 11].SetStyle(cellStyle);
                    sheet.Cells[1, 12].SetStyle(cellStyle);
                    sheet.Cells[1, 13].SetStyle(cellStyle);
                    sheet.Cells[1, 14].SetStyle(cellStyle);
                    sheet.Cells[1, 15].SetStyle(cellStyle);
                    sheet.Cells[1, 16].SetStyle(cellStyle);
                    sheet.Cells[1, 17].SetStyle(cellStyle);
                    sheet.Cells[1, 18].SetStyle(cellStyle);

                    //南线
                    sheet.Cells.Merge(2, 10, 1, 3);
                    sheet.Cells[2, 10].PutValue("南线");
                    sheet.Cells[2, 10].SetStyle(cellStyle);
                    sheet.Cells[2, 11].SetStyle(cellStyle);
                    sheet.Cells[2, 12].SetStyle(cellStyle);

                    //南线 - 供   回   差
                    sheet.Cells[3, 10].PutValue("供");
                    sheet.Cells[3, 10].SetStyle(cellStyle);
                    sheet.Cells[3, 11].PutValue("回");
                    sheet.Cells[3, 11].SetStyle(cellStyle);
                    sheet.Cells[3, 12].PutValue("差");
                    sheet.Cells[3, 12].SetStyle(cellStyle);

                    //中线
                    sheet.Cells.Merge(2, 13, 1, 3);
                    sheet.Cells[2, 13].PutValue("中线");
                    sheet.Cells[2, 13].SetStyle(cellStyle);
                    sheet.Cells[2, 14].SetStyle(cellStyle);
                    sheet.Cells[2, 15].SetStyle(cellStyle);

                    //中线 - 供   回   差
                    sheet.Cells[3, 13].PutValue("供");
                    sheet.Cells[3, 13].SetStyle(cellStyle);
                    sheet.Cells[3, 14].PutValue("回");
                    sheet.Cells[3, 14].SetStyle(cellStyle);
                    sheet.Cells[3, 15].PutValue("差");
                    sheet.Cells[3, 15].SetStyle(cellStyle);


                    //北线
                    sheet.Cells.Merge(2, 16, 1, 3);
                    sheet.Cells[2, 16].PutValue("北线");
                    sheet.Cells[2, 16].SetStyle(cellStyle);
                    sheet.Cells[2, 16].SetStyle(cellStyle);
                    sheet.Cells[2, 16].SetStyle(cellStyle);

                    //北线 - 供   回   差
                    sheet.Cells[3, 16].PutValue("供");
                    sheet.Cells[3, 16].SetStyle(cellStyle);
                    sheet.Cells[3, 17].PutValue("回");
                    sheet.Cells[3, 17].SetStyle(cellStyle);
                    sheet.Cells[3, 18].PutValue("差");
                    sheet.Cells[3, 18].SetStyle(cellStyle);

                    #endregion

                    #region 供回水压力

                    //供回水压力
                    sheet.Cells.Merge(1, 19, 1, 9);
                    sheet.Cells[1, 19].PutValue("供回水压力");
                    sheet.Cells[1, 19].SetStyle(cellStyle);
                    sheet.Cells[1, 20].SetStyle(cellStyle);
                    sheet.Cells[1, 21].SetStyle(cellStyle);
                    sheet.Cells[1, 22].SetStyle(cellStyle);
                    sheet.Cells[1, 23].SetStyle(cellStyle);
                    sheet.Cells[1, 24].SetStyle(cellStyle);
                    sheet.Cells[1, 25].SetStyle(cellStyle);
                    sheet.Cells[1, 26].SetStyle(cellStyle);
                    sheet.Cells[1, 27].SetStyle(cellStyle);

                    //南线
                    sheet.Cells.Merge(2, 19, 1, 3);
                    sheet.Cells[2, 19].PutValue("南线");
                    sheet.Cells[2, 19].SetStyle(cellStyle);
                    sheet.Cells[2, 20].SetStyle(cellStyle);
                    sheet.Cells[2, 21].SetStyle(cellStyle);

                    //南线 - 供   回   差
                    sheet.Cells[3, 19].PutValue("供");
                    sheet.Cells[3, 19].SetStyle(cellStyle);
                    sheet.Cells[3, 20].PutValue("回");
                    sheet.Cells[3, 20].SetStyle(cellStyle);
                    sheet.Cells[3, 21].PutValue("差");
                    sheet.Cells[3, 21].SetStyle(cellStyle);


                    //中线
                    sheet.Cells.Merge(2, 22, 1, 3);
                    sheet.Cells[2, 22].PutValue("中线");
                    sheet.Cells[2, 22].SetStyle(cellStyle);
                    sheet.Cells[2, 23].SetStyle(cellStyle);
                    sheet.Cells[2, 24].SetStyle(cellStyle);

                    //中线 - 供   回   差
                    sheet.Cells[3, 22].PutValue("供");
                    sheet.Cells[3, 22].SetStyle(cellStyle);
                    sheet.Cells[3, 23].PutValue("回");
                    sheet.Cells[3, 23].SetStyle(cellStyle);
                    sheet.Cells[3, 24].PutValue("差");
                    sheet.Cells[3, 24].SetStyle(cellStyle);

                    //北线
                    sheet.Cells.Merge(2, 25, 1, 3);
                    sheet.Cells[2, 25].PutValue("北线");
                    sheet.Cells[2, 25].SetStyle(cellStyle);
                    sheet.Cells[2, 26].SetStyle(cellStyle);
                    sheet.Cells[2, 27].SetStyle(cellStyle);

                    //北线 - 供   回   差
                    sheet.Cells[3, 25].PutValue("供");
                    sheet.Cells[3, 25].SetStyle(cellStyle);
                    sheet.Cells[3, 26].PutValue("回");
                    sheet.Cells[3, 26].SetStyle(cellStyle);
                    sheet.Cells[3, 27].PutValue("差");
                    sheet.Cells[3, 27].SetStyle(cellStyle);

                    #endregion

                    break;

                #endregion
                case "FILE2":
                    #region 热量、补水量、用电量、用汽量

                    //处理列头
                    #region 金城热力公司15-16年度11月份实际日耗能量统计表

                    sheet.Cells.Merge(0, 0, 1, 22);
                    sheet.Cells[0, 0].PutValue(fileName.Substring(0, fileName.Length - 1));
                    sheet.Cells[0, 0].Style.Font.IsBold = true;
                    sheet.Cells[0, 0].Style.Font.Size = 18;
                    sheet.Cells[0, 0].Style.Font.Name = cellFontName;
                    sheet.Cells[0, 0].Style.HorizontalAlignment = TextAlignmentType.Center;
                    sheet.Cells[0, 0].Style.VerticalAlignment = TextAlignmentType.Center;
                    sheet.Cells.SetRowHeight(0, 25);

                    #endregion

                    #region 日期

                    //日期
                    sheet.Cells.Merge(1, 0, 3, 1);
                    sheet.Cells[1, 0].PutValue("日期");
                    sheet.Cells[1, 0].SetStyle(cellStyle);
                    sheet.Cells[2, 0].SetStyle(cellStyle);
                    sheet.Cells[3, 0].SetStyle(cellStyle);
                    sheet.Cells.SetRowHeight(1, rowHeight);
                    sheet.Cells.SetRowHeight(2, rowHeight);
                    sheet.Cells.SetRowHeight(3, rowHeight);

                    #endregion


                    #region 热量GJ

                    //热量GJ	
                    sheet.Cells.Merge(1, 1, 1, 10);
                    sheet.Cells[1, 1].PutValue("热量（GJ）	");
                    sheet.Cells[1, 1].SetStyle(cellStyle);
                    sheet.Cells[1, 2].SetStyle(cellStyle);
                    sheet.Cells[1, 3].SetStyle(cellStyle);
                    sheet.Cells[1, 4].SetStyle(cellStyle);
                    sheet.Cells[1, 5].SetStyle(cellStyle);
                    sheet.Cells[1, 6].SetStyle(cellStyle);
                    sheet.Cells[1, 7].SetStyle(cellStyle);
                    sheet.Cells[1, 8].SetStyle(cellStyle);
                    sheet.Cells[1, 9].SetStyle(cellStyle);
                    sheet.Cells[1, 10].SetStyle(cellStyle);

                    //南线
                    sheet.Cells.Merge(2, 1, 1, 3);
                    sheet.Cells[2, 1].PutValue("南线");
                    sheet.Cells[2, 1].SetStyle(cellStyle);
                    sheet.Cells[2, 2].SetStyle(cellStyle);
                    sheet.Cells[2, 3].SetStyle(cellStyle);

                    //南线 - 换热站   换热器   小计
                    sheet.Cells[3, 1].PutValue("换热站");
                    sheet.Cells[3, 1].SetStyle(cellStyle);
                    sheet.Cells[3, 2].PutValue("换热器");
                    sheet.Cells[3, 2].SetStyle(cellStyle);
                    sheet.Cells[3, 3].PutValue("小计");
                    sheet.Cells[3, 3].SetStyle(cellStyle);


                    //中线
                    sheet.Cells.Merge(2, 4, 1, 3);
                    sheet.Cells[2, 4].PutValue("中线");
                    sheet.Cells[2, 4].SetStyle(cellStyle);
                    sheet.Cells[2, 5].SetStyle(cellStyle);
                    sheet.Cells[2, 6].SetStyle(cellStyle);

                    //中线 - 换热站   换热器   小计
                    sheet.Cells[3, 4].PutValue("换热站");
                    sheet.Cells[3, 4].SetStyle(cellStyle);
                    sheet.Cells[3, 5].PutValue("换热器");
                    sheet.Cells[3, 5].SetStyle(cellStyle);
                    sheet.Cells[3, 6].PutValue("小计");
                    sheet.Cells[3, 6].SetStyle(cellStyle);


                    //北线
                    sheet.Cells.Merge(2, 7, 1, 3);
                    sheet.Cells[2, 7].PutValue("北线");
                    sheet.Cells[2, 7].SetStyle(cellStyle);
                    sheet.Cells[2, 8].SetStyle(cellStyle);
                    sheet.Cells[2, 9].SetStyle(cellStyle);

                    //北线 - 换热站   换热器   小计
                    sheet.Cells[3, 7].PutValue("换热站");
                    sheet.Cells[3, 7].SetStyle(cellStyle);
                    sheet.Cells[3, 8].PutValue("换热器");
                    sheet.Cells[3, 8].SetStyle(cellStyle);
                    sheet.Cells[3, 9].PutValue("小计");
                    sheet.Cells[3, 9].SetStyle(cellStyle);

                    //合计
                    sheet.Cells.Merge(2, 10, 2, 1);
                    sheet.Cells[2, 10].PutValue("合计");
                    sheet.Cells[2, 10].SetStyle(cellStyle);
                    sheet.Cells[3, 10].SetStyle(cellStyle);

                    #endregion


                    #region 热网补水量

                    //热网补水量（T）
                    sheet.Cells.Merge(1, 11, 1, 3);
                    sheet.Cells[1, 11].PutValue("热网补水量（T）");
                    sheet.Cells[1, 11].SetStyle(cellStyle);
                    sheet.Cells[1, 12].SetStyle(cellStyle);
                    sheet.Cells[1, 13].SetStyle(cellStyle);

                    //室内
                    sheet.Cells.Merge(2, 11, 2, 1);
                    sheet.Cells[2, 11].PutValue("室内");
                    sheet.Cells[2, 11].SetStyle(cellStyle);
                    sheet.Cells[3, 11].SetStyle(cellStyle);

                    //室外
                    sheet.Cells.Merge(2, 12, 2, 1);
                    sheet.Cells[2, 12].PutValue("室外");
                    sheet.Cells[2, 12].SetStyle(cellStyle);
                    sheet.Cells[3, 12].SetStyle(cellStyle);

                    //合计
                    sheet.Cells.Merge(2, 13, 2, 1);
                    sheet.Cells[2, 13].PutValue("合计");
                    sheet.Cells[2, 13].SetStyle(cellStyle);
                    sheet.Cells[3, 13].SetStyle(cellStyle);

                    #endregion


                    #region 热网电表用电量

                    //热网电表用电量（kw.h）
                    sheet.Cells.Merge(1, 14, 1, 5);
                    sheet.Cells[1, 14].PutValue("热网电表用电量（kw.h）");
                    sheet.Cells[1, 14].SetStyle(cellStyle);
                    sheet.Cells[1, 15].SetStyle(cellStyle);
                    sheet.Cells[1, 16].SetStyle(cellStyle);
                    sheet.Cells[1, 17].SetStyle(cellStyle);
                    sheet.Cells[1, 18].SetStyle(cellStyle);

                    //循环泵
                    sheet.Cells.Merge(2, 14, 2, 1);
                    sheet.Cells[2, 14].PutValue("循环泵");
                    sheet.Cells[2, 14].SetStyle(cellStyle);
                    sheet.Cells[3, 14].SetStyle(cellStyle);

                    //南线
                    sheet.Cells.Merge(2, 15, 2, 1);
                    sheet.Cells[2, 15].PutValue("南线");
                    sheet.Cells[2, 15].SetStyle(cellStyle);
                    sheet.Cells[3, 15].SetStyle(cellStyle);

                    //东北线
                    sheet.Cells.Merge(2, 16, 2, 1);
                    sheet.Cells[2, 16].PutValue("东北线");
                    sheet.Cells[2, 16].SetStyle(cellStyle);
                    sheet.Cells[3, 16].SetStyle(cellStyle);

                    //文化新村
                    sheet.Cells.Merge(2, 17, 2, 1);
                    sheet.Cells[2, 17].PutValue("文化新村");
                    sheet.Cells[2, 17].SetStyle(cellStyle);
                    sheet.Cells[3, 17].SetStyle(cellStyle);

                    //合计
                    sheet.Cells.Merge(2, 18, 2, 1);
                    sheet.Cells[2, 18].PutValue("合计");
                    sheet.Cells[2, 18].SetStyle(cellStyle);
                    sheet.Cells[3, 18].SetStyle(cellStyle);


                    #endregion


                    #region 汽动泵用汽量

                    //汽动泵用汽量
                    sheet.Cells.Merge(1, 19, 1, 3);
                    sheet.Cells[1, 19].PutValue("汽动泵用汽量");
                    sheet.Cells[1, 19].SetStyle(cellStyle);
                    sheet.Cells[1, 20].SetStyle(cellStyle);
                    sheet.Cells[1, 21].SetStyle(cellStyle);

                    //东北线
                    sheet.Cells.Merge(2, 19, 2, 1);
                    sheet.Cells[2, 19].PutValue("东北线");
                    sheet.Cells[2, 19].SetStyle(cellStyle);
                    sheet.Cells[3, 19].SetStyle(cellStyle);

                    //南线
                    sheet.Cells.Merge(2, 20, 2, 1);
                    sheet.Cells[2, 20].PutValue("南线");
                    sheet.Cells[2, 20].SetStyle(cellStyle);
                    sheet.Cells[3, 20].SetStyle(cellStyle);

                    //合计
                    sheet.Cells.Merge(2, 21, 2, 1);
                    sheet.Cells[2, 21].PutValue("合计");
                    sheet.Cells[2, 21].SetStyle(cellStyle);
                    sheet.Cells[3, 21].SetStyle(cellStyle);

                    #endregion


                    break;

                #endregion
                default:
                    break;
            }
            

            //处理行数据
            #region 处理行的数据

            //行的处理
            Style style = workbook.Styles[workbook.Styles.Add()];
            style.Font.Name = cellFontName;
            style.Font.Size = cellFontSize;
            StyleFlag styleFlag = new StyleFlag();
            sheet.Cells.ApplyStyle(style, styleFlag);

            //定义列头
            int rowIndex = 4;//前3行是标题+表头
            int colIndex = 0;

            for (int i = 0; i < rowCount; i++)
            {
                colIndex = 0;
                for (int j = 0; j < colCount; j++)
                {
                    sheet.Cells[rowIndex, colIndex].PutValue(dataTable.Rows[i][j]);
                    sheet.Cells.SetRowHeight(rowIndex, rowHeight);
                    sheet.Cells[rowIndex, colIndex].SetStyle(cellStyle);
                    colIndex++;
                }
                rowIndex++;
            }

            #endregion
            

            //指定列宽为自适应
            sheet.AutoFitColumns();
            //指定行高为自适应
            //sheet.AutoFitRows();
            sheet.FreezePanes("B5", 4, 1);

            //返回结果
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

        private string createExcelTitle(ref string startDateTime, ref string endDateTime, ref string type)
        {
            string[] tempItems = startDateTime.Split('-');
            DateTime dateTime1 = new DateTime(Convert.ToInt32(tempItems[0]), Convert.ToInt32(tempItems[1]), Convert.ToInt32(tempItems[2]));

            tempItems = endDateTime.Split('-');
            DateTime dateTime2 = new DateTime(Convert.ToInt32(tempItems[0]), Convert.ToInt32(tempItems[1]), Convert.ToInt32(tempItems[2]));

            string yearText = string.Empty;
            if (dateTime1.Year == dateTime2.Year)
            {
                yearText = string.Format("{0}-{1}", dateTime1.Year.ToString().Substring(2, 2), dateTime2.AddYears(1).Year.ToString().Substring(2, 2));
            }
            else
            {
                yearText = string.Format("{0}-{1}", dateTime1.Year.ToString().Substring(2, 2), dateTime2.Year.ToString().Substring(2, 2));
            }

            string monthText = string.Empty;
            if (dateTime1.Month == dateTime2.Month)
            {
                monthText = dateTime1.Month.ToString();
            }
            else
            {
                monthText = string.Format("{0}~{1}", dateTime1.Month.ToString(), dateTime2.Month.ToString());
            }

            string text = string.Format("金城热力公司{0}年度{1}月份实际日耗能量统计表{2}",
                yearText,
                monthText,
                type == "FILE1" ? "1" : "2");

            yearText = monthText = null;
            tempItems = null;

            return text;
        }



        public string deleteExportExcelFile(string text)
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
