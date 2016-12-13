using KunTaiServiceLibrary.valueObjects;
using System;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Warrior.DataAccessUtils.Desktop;

namespace KunTaiServiceLibrary
{
    [FluorineFx.RemotingService("山东金城热力二级站管理")]
    public class ZY_WR_STAT_A : IController
    {
        
        #region command texts

        private const string wr_stat_a_insert_commandText = "INSERT INTO [ZY_WR_STAT_A] ([STCD], [NAME], [LONGITUDE], [LATITUDE], [NOTE]) VALUES (@STCD, @NAME, @LONGITUDE, @LATITUDE, @NOTE)";

        private const string wr_stat_a_delete_commandText = "DELETE [ZY_WR_STAT_A] WHERE ID IN ({0})";

        private const string wr_stat_a_update_commandText = "UPDATE [ZY_WR_STAT_A] SET [STCD]=@STCD, [NAME]=@NAME, [LONGITUDE]=@LONGITUDE, [LATITUDE]=@LATITUDE, [NOTE]=@NOTE WHERE [ID]=@ID";

        private const string wr_stat_a_select_commandText = "SELECT ROW_NUMBER() OVER (ORDER BY [STCD]) AS NUM1, [ID], [STCD], [NAME], [NOTE] FROM [ZY_WR_STAT_A]{0}";

        private const string wr_stat_a_total_commandText = "SELECT COUNT(*) FROM [ZY_WR_STAT_A]{0}";

        private const string wr_stat_a_details_commandText = "SELECT [ID], [STCD], [NAME], [LONGITUDE], [LATITUDE], [NOTE] FROM [ZY_WR_STAT_A] WHERE [ID]=@ID";



        private const string wr_stat_a_select = "SELECT STCD,NAME  FROM ZY_WR_STAT_A ORDER BY STCD";

        private const string group_wr_stat_a_select = @"SELECT ROW_NUMBER() OVER (ORDER BY [STCD]) AS NUM,T.* FROM (
SELECT [ID], [STCD], [NAME], '1' AS [SELECTED] FROM [ZY_WR_STAT_A] WHERE [TYPE]=@TYPE
UNION
SELECT [ID], [STCD], [NAME], '0' AS [SELECTED] FROM [ZY_WR_STAT_A] WHERE [TYPE] IS NULL) T
ORDER BY STCD";


        private const string clear_wr_stat_a_type = "UPDATE [ZY_WR_STAT_A] SET [TYPE]=NULL WHERE [TYPE]=@TYPE";

        private const string set_wr_stat_a_type = "UPDATE [ZY_WR_STAT_A] SET [TYPE]='{0}' WHERE [ID] IN ({1})";

        #endregion




        public string getWR_STAT_A()
        {
            DataSet dataSetStatA = null;
            try
            {
                dataSetStatA = new DataAccessHandler().executeDatasetResult(
                    wr_stat_a_select, null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;

            if (dataSetStatA != null && dataSetStatA.Tables.Count > 0)
            {
                result = Result.getResultXml(getWR_STAT_AXml(ref dataSetStatA));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            dataSetStatA = null;

            return result;
        }

        private string getWR_STAT_AXml(ref DataSet dataSetStatA)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            ZY_WR_STAT_AObject zy_wr_stat_aObject = null;
            foreach (DataRow row in dataSetStatA.Tables[0].Rows)
            {
                zy_wr_stat_aObject = new ZY_WR_STAT_AObject(row);
                xml.AppendFormat("<DATA STCD=\"{0}\" NAME=\"{1}\"/>",
                    zy_wr_stat_aObject.STCD,
                    zy_wr_stat_aObject.NAME
                );

                zy_wr_stat_aObject = null;
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }


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

            try
            {
                result = new DataAccessHandler().executeNonQueryResult(
                    wr_stat_a_insert_commandText,
                    SqlServer.GetParameter(xml, new string[] { "STCD", "NAME", "LONGITUDE", "LATITUDE", "NOTE" })
                );
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
                    string.Format(wr_stat_a_delete_commandText, xml.Element("ID").Value), null);
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
                    wr_stat_a_update_commandText,
                    SqlServer.GetParameter(xml, new string[] { "ID", "STCD", "NAME", "LONGITUDE", "LATITUDE", "NOTE" })
                );
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
            string commandText = string.Format(wr_stat_a_select_commandText, whereText);

            DataSet dataSetWR_STAT_A = null;
            try
            {
                dataSetWR_STAT_A = new DataAccessHandler().executeDatasetResult(
                    SqlServerCommandText.createPagingCommandText(ref commandText, ref xml), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetWR_STAT_A != null && dataSetWR_STAT_A.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemXml(ref dataSetWR_STAT_A, getDataItemTotal(ref whereText)));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }


            text = whereText = commandText = null;
            xml = null;
            dataSetWR_STAT_A = null;

            return result;
        }

        private string getDataItemXml(ref DataSet dataSetWR_STAT_A, string total)
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendFormat("<DATAS COUNT=\"{0}\" TOTAL=\"{1}\">", dataSetWR_STAT_A.Tables[0].Rows.Count, total);
            ZY_WR_STAT_AObject zy_wr_stat_aObject = null;
            foreach (DataRow row in dataSetWR_STAT_A.Tables[0].Rows)
            {
                zy_wr_stat_aObject = new ZY_WR_STAT_AObject(row);
                xml.AppendFormat("<DATA NUM=\"{0}\" ID=\"{1}\" STCD=\"{2}\" NAME=\"{3}\" NOTE=\"{4}\" />",
                    zy_wr_stat_aObject.NUM,
                    zy_wr_stat_aObject.ID,
                    zy_wr_stat_aObject.STCD,
                    zy_wr_stat_aObject.NAME,
                    zy_wr_stat_aObject.NOTE
                );
                zy_wr_stat_aObject = null;
            }
            xml.Append("</DATAS>");


            return xml.ToString();
        }

        private string getDataItemTotal(ref string whereText)
        {
            return new DataAccessHandler().executeScalarResult(
                string.Format(wr_stat_a_total_commandText, whereText), null);
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

            DataSet dataSetWR_STAT_A = null;
            try
            {
                dataSetWR_STAT_A = new DataAccessHandler().executeDatasetResult(
                    wr_stat_a_details_commandText,
                    SqlServer.GetParameter("ID", xml.Element("ID").Value));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetWR_STAT_A != null && dataSetWR_STAT_A.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemDetailsXml(ref dataSetWR_STAT_A));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = null;
            xml = null;
            dataSetWR_STAT_A = null;

            return result;
        }

        private string getDataItemDetailsXml(ref DataSet dataSetWR_STAT_A)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            ZY_WR_STAT_AObject zy_wr_stat_aObject = null;
            foreach (DataRow row in dataSetWR_STAT_A.Tables[0].Rows)
            {
                zy_wr_stat_aObject = new ZY_WR_STAT_AObject(row);
                xml.AppendFormat("<DATA ID=\"{0}\" STCD=\"{1}\" NAME=\"{2}\" LONGITUDE=\"{3}\" LATITUDE=\"{4}\" NOTE=\"{5}\"/>",
                    zy_wr_stat_aObject.ID,
                    zy_wr_stat_aObject.STCD,
                    zy_wr_stat_aObject.NAME,
                    zy_wr_stat_aObject.LONGITUDE,
                    zy_wr_stat_aObject.LATITUDE,
                    zy_wr_stat_aObject.NOTE
                );
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }



        public string getGroup_WR_STAT_A(string text)
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

            DataSet dataSetWR_STAT_A = null;
            try
            {
                dataSetWR_STAT_A = new DataAccessHandler().executeDatasetResult(
                    group_wr_stat_a_select,
                    SqlServer.GetParameter("TYPE", xml.Element("TYPE").Value));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if(dataSetWR_STAT_A!= null && dataSetWR_STAT_A.Tables.Count>0)
            {
                result = Result.getResultXml(getGroup_WR_STAT_AXml(ref dataSetWR_STAT_A));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }


            text = null;
            xml = null;
            dataSetWR_STAT_A = null;

            return result;
        }

        private string getGroup_WR_STAT_AXml(ref DataSet dataSetWR_STAT_A)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            ZY_WR_STAT_AObject zy_wr_stat_aObject = null;
            foreach (DataRow row in dataSetWR_STAT_A.Tables[0].Rows)
            {
                zy_wr_stat_aObject = new ZY_WR_STAT_AObject(row);

                xml.AppendFormat("<DATA NUM=\"{0}\" ID=\"{1}\" STCD=\"{2}\" NAME=\"{3}\" SELECTED=\"{4}\"/>",
                    zy_wr_stat_aObject.NUM,
                    zy_wr_stat_aObject.ID,
                    zy_wr_stat_aObject.STCD,
                    zy_wr_stat_aObject.NAME,
                    zy_wr_stat_aObject.SELECTED
                );
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }


        public string saveWR_STAT_AGroupLine(string text)
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

            string type = xml.Element("TYPE").Value;
            string id = xml.Element("ID").Value;

            string result = string.Empty;

            //1、清除当前记录中符合TYPE的记录
            result = new DataAccessHandler().executeNonQueryResult(
                clear_wr_stat_a_type,
                SqlServer.GetParameter("TYPE", type));

            //2、设置ID中的所有记录为TYPE的内容
            if (Result.isSuccess(result) && !string.IsNullOrEmpty(id))
            {
                result = new DataAccessHandler().executeNonQueryResult(
                    string.Format(set_wr_stat_a_type, type, id), null);
            }

            text = null;
            xml = null;
            type = null;
            
            return result;
        }



    }
}
