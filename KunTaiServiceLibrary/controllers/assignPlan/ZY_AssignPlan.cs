using KunTaiServiceLibrary.valueObjects;
using System;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Warrior.DataAccessUtils.Desktop;

namespace KunTaiServiceLibrary
{
    [FluorineFx.RemotingService("金城供热分配方案")]
    public class ZY_AssignPlan : IController
    {

        #region command texts

        private const string zy_assignPlan_insert = "INSERT INTO [ZY_ASSIGNPLAN] ([NAME], [NOTE]) VALUES (@NAME, @NOTE)";

        private const string zy_assignPlan_update = "UPDATE [ZY_ASSIGNPLAN] SET [NAME]=@NAME, [NOTE]=@NOTE WHERE [ID]=@ID";

        private const string zy_assignPlan_delete = "DELETE [ZY_ASSIGNPLAN] WHERE [ID] IN ({0})";

        private const string zy_assignPlan_select = "SELECT ROW_NUMBER() OVER (ORDER BY [SHOWID], [NAME]) AS NUM1, [ID], [NAME], [NOTE] FROM [ZY_ASSIGNPLAN]{0}";

        private const string zy_assignPlan_total = "SELECT COUNT(*) FROM [ZY_ASSIGNPLAN]{0}";

        private const string zy_assignPlan_details = "SELECT [ID], [NAME], [NOTE] FROM [ZY_ASSIGNPLAN] WHERE [ID]=@ID";


        private const string wr_stat_a_left_list = "SELECT [ID], [STCD], [NAME] FROM [ZY_WR_STAT_A]  WHERE [ID] NOT IN (SELECT [STATAID] FROM [ZY_ASSIGNPLANITEM] WHERE ([TYPE]=@TYPE AND [PID]=@PID) OR [PID]=@PID) ORDER BY [STCD]";

        private const string wr_stat_a_right_list = "SELECT A.[ID], A.[STCD], A.[NAME] FROM [ZY_WR_STAT_A] A, [ZY_ASSIGNPLANITEM] ITEM WHERE A.[ID]=ITEM.[STATAID] AND ITEM.[TYPE]=@TYPE AND ITEM.[PID]=@PID AND ITEM.[GROUPID]=@GROUPID ORDER BY ITEM.SHOWID";


        private const string assign_plan_item_insert = "INSERT INTO [ZY_ASSIGNPLANITEM] ([PID], [TYPE], [STATAID], [GROUPID]) VALUES (@PID, @TYPE, @STATAID, @GROUPID)";


        private const string assign_plan_item_delete = "DELETE [ZY_ASSIGNPLANITEM] WHERE [TYPE]=@TYPE AND [PID]=@PID AND [STATAID]=@STATAID";

        private const string assign_plan_item_clear = "DELETE [ZY_ASSIGNPLANITEM] WHERE [TYPE]=@TYPE AND [PID]=@PID";



        private const string assign_plan_list_select = "SELECT ROW_NUMBER() OVER (ORDER BY [SHOWID], [NAME]) AS NUM, [ID], [NAME] FROM [ZY_ASSIGNPLAN] ORDER BY [SHOWID], [NAME]";


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
            try
            {
                result = new DataAccessHandler().executeNonQueryResult(
                    zy_assignPlan_insert,
                    SqlServer.GetParameter(xml, new string[] { "NAME", "NOTE" }));
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
                    string.Format(zy_assignPlan_delete, xml.Element("ID").Value), null);
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
                    zy_assignPlan_update,
                    SqlServer.GetParameter(xml, new string[] { "ID", "NAME", "NOTE" }));
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
            string commandText = string.Format(zy_assignPlan_select, whereText);


            DataSet dataSetAssignPlan = null;
            try
            {
                dataSetAssignPlan = new DataAccessHandler().executeDatasetResult(
                    SqlServerCommandText.createPagingCommandText(ref commandText, ref xml), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetAssignPlan != null && dataSetAssignPlan.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemXml(ref dataSetAssignPlan, getDataItemTotal(ref whereText)));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }


            text = null;
            xml = null;
            dataSetAssignPlan = null;

            return result;
        }

        private string getDataItemXml(ref DataSet dataSetAssignPlan, string total)
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendFormat("<DATAS COUNT=\"{0}\" TOTAL=\"{1}\">", dataSetAssignPlan.Tables[0].Rows.Count, total);
            ZY_AssignPlanObject zy_assignPlanObject = null;
            foreach (DataRow row in dataSetAssignPlan.Tables[0].Rows)
            {
                zy_assignPlanObject = new ZY_AssignPlanObject(row);
                xml.AppendFormat("<DATA NUM=\"{0}\" ID=\"{1}\" NAME=\"{2}\" NOTE=\"{3}\"/>",
                    zy_assignPlanObject.NUM,
                    zy_assignPlanObject.ID,
                    zy_assignPlanObject.NAME,
                    zy_assignPlanObject.NOTE
                );
                zy_assignPlanObject = null;
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }

        private string getDataItemTotal(ref string whereText)
        {
            return new DataAccessHandler().executeScalarResult(
                string.Format(zy_assignPlan_total, whereText), null);
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

            DataSet dataSetAssignPlan = null;
            try
            {
                dataSetAssignPlan = new DataAccessHandler().executeDatasetResult(
                    zy_assignPlan_details,
                    SqlServer.GetParameter("ID", xml.Element("ID").Value));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;

            if (dataSetAssignPlan != null && dataSetAssignPlan.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemDetailsXml(ref dataSetAssignPlan));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = null;
            xml = null;
            dataSetAssignPlan = null;

            return result;
        }

        private string getDataItemDetailsXml(ref DataSet dataSetAssignPlan)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            ZY_AssignPlanObject zy_assignPlanObject = null;
            foreach (DataRow row in dataSetAssignPlan.Tables[0].Rows)
            {
                zy_assignPlanObject = new ZY_AssignPlanObject(row);
                xml.AppendFormat("<DATA ID=\"{0}\" NAME=\"{1}\" NOTE=\"{2}\"/>",
                    zy_assignPlanObject.ID,
                    zy_assignPlanObject.NAME,
                    zy_assignPlanObject.NOTE
                );
            }
            xml.Append("</DATAS>");

            zy_assignPlanObject = null;

            return xml.ToString();
        }


        public string getGroupWR_STAT_AList(string text)
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

            DataSet dataSetWR_STAT_ALeft = null;
            DataSet dataSetWR_STAT_ARight = null;
            try
            {
                dataSetWR_STAT_ALeft = new DataAccessHandler().executeDatasetResult(
                    wr_stat_a_left_list,
                    SqlServer.GetParameter(xml, new string[] { "TYPE", "PID" }));

                dataSetWR_STAT_ARight = new DataAccessHandler().executeDatasetResult(
                    wr_stat_a_right_list,
                    SqlServer.GetParameter(xml, new string[] { "TYPE", "PID", "GROUPID" }));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetWR_STAT_ALeft != null && dataSetWR_STAT_ARight != null && dataSetWR_STAT_ALeft.Tables.Count > 0 && dataSetWR_STAT_ARight.Tables.Count > 0)
            {
                result = Result.getResultXml(getGroupWR_STAT_AListXml(ref dataSetWR_STAT_ALeft, ref dataSetWR_STAT_ARight));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }


            text = null;
            xml = null;
            dataSetWR_STAT_ALeft = dataSetWR_STAT_ARight = null;


            return result;
        }

        private string getGroupWR_STAT_AListXml(ref DataSet dataSetWR_STAT_ALeft, ref DataSet dataSetWR_STAT_ARight)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            xml.Append("<LEFT>");
            ZY_WR_STAT_AObject zy_wr_stat_aObject = null;
            int numIndex = 1;
            foreach (DataRow row in dataSetWR_STAT_ALeft.Tables[0].Rows)
            {
                zy_wr_stat_aObject = new ZY_WR_STAT_AObject(row);
                xml.AppendFormat("<DATA ID=\"{0}\" STCD=\"{1}\" NAME=\"{2}\" NUM=\"{3}\"/>",
                    zy_wr_stat_aObject.ID,
                    zy_wr_stat_aObject.STCD,
                    zy_wr_stat_aObject.NAME,
                    numIndex
                );
                zy_wr_stat_aObject = null;
                numIndex++;
            }
            xml.Append("</LEFT>");

            numIndex = 1;
            xml.Append("<RIGHT>");
            foreach (DataRow row in dataSetWR_STAT_ARight.Tables[0].Rows)
            {
                zy_wr_stat_aObject = new ZY_WR_STAT_AObject(row);
                xml.AppendFormat("<DATA ID=\"{0}\" STCD=\"{1}\" NAME=\"{2}\" NUM=\"{3}\"/>",
                    zy_wr_stat_aObject.ID,
                    zy_wr_stat_aObject.STCD,
                    zy_wr_stat_aObject.NAME,
                    numIndex
                );
                zy_wr_stat_aObject = null;
                numIndex++;
            }
            xml.Append("</RIGHT>");
            xml.Append("</DATAS>");

            return xml.ToString();
        }



        public string addWR_STAT_A(string text)
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
                    assign_plan_item_insert,
                    SqlServer.GetParameter(xml, new string[] { "PID", "TYPE", "STATAID", "GROUPID" }));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = null;
            xml = null;

            return result;
        }



        public string removeWR_STAT_A(string text)
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
                    assign_plan_item_delete,
                    SqlServer.GetParameter(xml, new string[] { "TYPE", "PID", "STATAID" }));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }


            text = null;
            xml = null;

            return result;
        }




        public string removeAllWR_STAT_A(string text)
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
                    assign_plan_item_clear,
                    SqlServer.GetParameter(xml, new string[] { "TYPE", "PID" }));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = null;
            xml = null;

            return result;
        }



        public string getAssignPlanList()
        {
            DataSet dataSetAssignPlan = null;
            try
            {
                dataSetAssignPlan = new DataAccessHandler().executeDatasetResult(
                    assign_plan_list_select, null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if(dataSetAssignPlan != null && dataSetAssignPlan.Tables.Count>0)
            {
                result = Result.getResultXml(getAssignPlanListXml(ref dataSetAssignPlan));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            dataSetAssignPlan = null;

            return result;
        }

        private string getAssignPlanListXml(ref DataSet dataSetAssignPlan)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            ZY_AssignPlanObject zy_assignPlanObject = null;
            foreach (DataRow row in dataSetAssignPlan.Tables[0].Rows)
            {
                zy_assignPlanObject = new ZY_AssignPlanObject(row);
                xml.AppendFormat("<DATA NUM=\"{0}\" ID=\"{1}\" NAME=\"{2}\"/>",
                    zy_assignPlanObject.NUM,
                    zy_assignPlanObject.ID,
                    zy_assignPlanObject.NAME
                );
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }
    }
}
