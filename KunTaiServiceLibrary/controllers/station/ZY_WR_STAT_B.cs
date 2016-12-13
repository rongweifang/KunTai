using KunTaiServiceLibrary.valueObjects;
using System;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Warrior.DataAccessUtils.Desktop;

namespace KunTaiServiceLibrary
{
    [FluorineFx.RemotingService("山东金城热力三级站管理")]
    public class ZY_WR_STAT_B : IController
    {

        #region command texts

        private const string zy_wr_stat_b_insert = "INSERT INTO [ZY_WR_STAT_B] ([STCD], [T_CODE], [ST_NM], [TYPE], [NOTE], [AREA], [POWER], [EFFICIENCY], [FLOW], [SPEED], [FREQUENCY], [HOTTYPE], [NAMEPLATEFLOW], [ISCALCULATE]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13})";

        private const string zy_wr_stat_b_update = "UPDATE [ZY_WR_STAT_B] SET [STCD]='{0}', [T_CODE]='{1}', [ST_NM]='{2}', [TYPE]={3}, [NOTE]='{4}', [AREA]={5}, [POWER]={6}, [EFFICIENCY]={7}, [FLOW]={8}, [SPEED]={9}, [FREQUENCY]={10}, [HOTTYPE]={11}, [NAMEPLATEFLOW]={12}, [ISCALCULATE]={13} WHERE [ID]='{14}'";

        private const string zy_wr_stat_b_delete = "DELETE [ZY_WR_STAT_B] WHERE [ID] IN ({0})";


        private const string zy_wr_stat_b_select = "SELECT ROW_NUMBER() OVER (ORDER BY [T_CODE]) AS NUM1, [ID], [T_CODE], [ST_NM], [TYPE], [AREA], [POWER], [EFFICIENCY], [FLOW], [SPEED], [FREQUENCY] FROM [ZY_WR_STAT_B]{0}";

        private const string zy_wr_stat_b_total = "SELECT COUNT(*) FROM [ZY_WR_STAT_B]{0}";

        private const string zy_wr_stat_b_details = "SELECT  [ID], [STCD], [T_CODE], [ST_NM], [TYPE], [NOTE], [AREA], [POWER], [EFFICIENCY], [FLOW], [SPEED], [FREQUENCY], [HOTTYPE], [NAMEPLATEFLOW], [ISCALCULATE] FROM [ZY_WR_STAT_B] WHERE [ID]=@ID";


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


            string result = string.Empty;

            ZY_WR_STAT_BObject zy_wr_stat_bObject = null;
            try
            {
                zy_wr_stat_bObject = new ZY_WR_STAT_BObject(xml);

                result = new DataAccessHandler().executeNonQueryResult(
                    string.Format(zy_wr_stat_b_insert, 
                    zy_wr_stat_bObject.STCD,
                    zy_wr_stat_bObject.T_CODE,
                    zy_wr_stat_bObject.ST_NM,
                    zy_wr_stat_bObject.TYPE,
                    zy_wr_stat_bObject.NOTE,
                    zy_wr_stat_bObject.AREA,
                    zy_wr_stat_bObject.POWER,
                    zy_wr_stat_bObject.EFFICIENCY,
                    zy_wr_stat_bObject.FLOW,
                    zy_wr_stat_bObject.SPEED,
                    zy_wr_stat_bObject.FREQUENCY,
                    zy_wr_stat_bObject.HOTTYPE,
                    zy_wr_stat_bObject.NAMEPLATEFLOW,
                    zy_wr_stat_bObject.ISCALCULATE), 
                    null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = null;
            xml = null;
            zy_wr_stat_bObject = null;

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
                    string.Format(zy_wr_stat_b_delete, xml.Element("ID").Value), null);
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

            ZY_WR_STAT_BObject zy_wr_stat_bObject = null;
            try
            {
                zy_wr_stat_bObject = new ZY_WR_STAT_BObject(xml);

                result = new DataAccessHandler().executeNonQueryResult(
                    string.Format(zy_wr_stat_b_update,
                    zy_wr_stat_bObject.STCD,
                    zy_wr_stat_bObject.T_CODE,
                    zy_wr_stat_bObject.ST_NM,
                    zy_wr_stat_bObject.TYPE,
                    zy_wr_stat_bObject.NOTE,
                    zy_wr_stat_bObject.AREA,
                    zy_wr_stat_bObject.POWER,
                    zy_wr_stat_bObject.EFFICIENCY,
                    zy_wr_stat_bObject.FLOW,
                    zy_wr_stat_bObject.SPEED,
                    zy_wr_stat_bObject.FREQUENCY,
                    zy_wr_stat_bObject.HOTTYPE,
                    zy_wr_stat_bObject.NAMEPLATEFLOW,
                    zy_wr_stat_bObject.ISCALCULATE,
                    zy_wr_stat_bObject.ID
                    ), 
                    null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = null;
            xml = null;
            zy_wr_stat_bObject = null;

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
            string commandText = string.Format(zy_wr_stat_b_select, whereText);

            DataSet dataSetZY_STAT_B = null;
            try
            {
                dataSetZY_STAT_B = new DataAccessHandler().executeDatasetResult(
                    SqlServerCommandText.createPagingCommandText(ref commandText, ref xml),
                    null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }
            
            string result = string.Empty;
            if (dataSetZY_STAT_B != null && dataSetZY_STAT_B.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemXml(ref dataSetZY_STAT_B, getDataItemTotal(ref whereText)));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = whereText = commandText = null;
            xml = null;
            dataSetZY_STAT_B = null;

            return result;
        }

        private string getDataItemXml(ref DataSet dataSetZY_STAT_B, string total)
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendFormat("<DATAS COUNT=\"{0}\" TOTAL=\"{1}\">", dataSetZY_STAT_B.Tables[0].Rows.Count, total);
            ZY_WR_STAT_BObject zy_wr_stat_bObject = null;
            foreach (DataRow row in dataSetZY_STAT_B.Tables[0].Rows)
            {
                zy_wr_stat_bObject = new ZY_WR_STAT_BObject(row);

                xml.AppendFormat("<DATA NUM=\"{0}\" ID=\"{1}\" T_CODE=\"{2}\" ST_NM=\"{3}\" TYPE=\"{4}\" AREA=\"{5}\" POWER=\"{6}\" EFFICIENCY=\"{7}\" FLOW=\"{8}\" SPEED=\"{9}\" FREQUENCY=\"{10}\"/>",
                    zy_wr_stat_bObject.NUM,
                    zy_wr_stat_bObject.ID,
                    zy_wr_stat_bObject.T_CODE,
                    zy_wr_stat_bObject.ST_NM,
                    zy_wr_stat_bObject.TYPE,
                    zy_wr_stat_bObject.AREA,
                    zy_wr_stat_bObject.POWER,
                    zy_wr_stat_bObject.EFFICIENCY,
                    zy_wr_stat_bObject.FLOW,
                    zy_wr_stat_bObject.SPEED,
                    zy_wr_stat_bObject.FREQUENCY
                );

                zy_wr_stat_bObject = null;
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }

        private string getDataItemTotal(ref string whereText)
        {
            return new DataAccessHandler().executeScalarResult(
                string.Format(zy_wr_stat_b_total, whereText), null);
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

            DataSet dataSetWR_STAT_B = null;
            try
            {
                dataSetWR_STAT_B = new DataAccessHandler().executeDatasetResult(
                    zy_wr_stat_b_details,
                    SqlServer.GetParameter("ID", xml.Element("ID").Value));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetWR_STAT_B != null && dataSetWR_STAT_B.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemDetailsXml(ref dataSetWR_STAT_B));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = null;
            xml = null;
            dataSetWR_STAT_B = null;

            return result;
        }

        private string getDataItemDetailsXml(ref DataSet dataSetWR_STAT_B)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            ZY_WR_STAT_BObject zy_wr_stat_bObject = null;
            foreach (DataRow row in dataSetWR_STAT_B.Tables[0].Rows)
            {
                zy_wr_stat_bObject = new ZY_WR_STAT_BObject(row);
                xml.AppendFormat("<DATA ID=\"{0}\" STCD=\"{1}\" T_CODE=\"{2}\" ST_NM=\"{3}\" TYPE=\"{4}\" NOTE=\"{5}\" AREA=\"{6}\" POWER=\"{7}\" EFFICIENCY=\"{8}\" FLOW=\"{9}\" SPEED=\"{10}\" FREQUENCY=\"{11}\" HOTTYPE=\"{12}\" NAMEPLATEFLOW=\"{13}\" ISCALCULATE=\"{14}\"/>",
                    zy_wr_stat_bObject.ID,
                    zy_wr_stat_bObject.STCD,
                    zy_wr_stat_bObject.T_CODE,
                    zy_wr_stat_bObject.ST_NM,
                    zy_wr_stat_bObject.TYPE,
                    zy_wr_stat_bObject.NOTE,
                    zy_wr_stat_bObject.AREA,
                    zy_wr_stat_bObject.POWER,
                    zy_wr_stat_bObject.EFFICIENCY,
                    zy_wr_stat_bObject.FLOW,
                    zy_wr_stat_bObject.SPEED,
                    zy_wr_stat_bObject.FREQUENCY,
                    zy_wr_stat_bObject.HOTTYPE,
                    zy_wr_stat_bObject.NAMEPLATEFLOW,
                    zy_wr_stat_bObject.ISCALCULATE
                );
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }
    }
}
