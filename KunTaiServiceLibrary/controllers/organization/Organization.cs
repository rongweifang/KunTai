using KunTaiServiceLibrary.valueObjects;
using System;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Warrior.DataAccessUtils.Desktop;

namespace KunTaiServiceLibrary
{

    [FluorineFx.RemotingService("组织机构服务")]
    public class Organization : IController
    {
        #region command texts


        private const string organization_insert_commandText = "INSERT INTO [ORGANIZATION] ([NAME], [EMPLOYEE], [PHONE], [ADDRESS], [CITYID], [NOTE]) VALUES (@NAME,@EMPLOYEE,@PHONE,@ADDRESS,@CITYID,@NOTE)";

        private const string organization_delete_commandText = "DELETE [ORGANIZATION] WHERE [ID] IN ({0})";

        private const string organization_update_commandText = "UPDATE [ORGANIZATION] SET [NAME]=@NAME, [EMPLOYEE]=@EMPLOYEE, [PHONE]=@PHONE, [ADDRESS]=@ADDRESS, [CITYID]=@CITYID, [NOTE]=@NOTE WHERE ID=@ID";

        private const string organization_select_commandText = "SELECT ROW_NUMBER() OVER (ORDER BY [SHOWID], [NAME]) AS NUM1, [ID], [NAME], [EMPLOYEE], [PHONE], [ADDRESS], [CITYID] FROM [ORGANIZATION]{0}";

        private const string organization_total_commandText = "SELECT COUNT(*) FROM [ORGANIZATION]{0}";

        private const string organization_details_commandText = "SELECT [ID], [NAME], [EMPLOYEE], [PHONE], [ADDRESS], [CITYID], [NOTE] FROM [ORGANIZATION] WHERE [ID]=@ID";

        private const string organization_list_commandText = "SELECT [ID], [NAME] FROM [ORGANIZATION] WHERE [ISHIDE]=0 ORDER BY [SHOWID], [ID]";


        private const string organization_select_where_text = "[ISHIDE] = 0";

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
                    organization_insert_commandText,
                    SqlServer.GetParameter(xml, new string[] { "NAME", "EMPLOYEE", "PHONE", "ADDRESS", "CITYID", "NOTE" }));
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
                //TODO 检查该组织机构下是否有可用的供热站或锅炉房

                result = new DataAccessHandler().executeNonQueryResult(
                    string.Format(organization_delete_commandText, xml.Element("ID").Value),
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
                    organization_update_commandText,
                    SqlServer.GetParameter(xml, new string[] { "ID", "NAME", "EMPLOYEE", "PHONE", "ADDRESS", "CITYID", "NOTE" }));
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
            //<ORGANIZATION><WHERE></WHERE><PAGENUMBER>1</PAGENUMBER><PAGECOUNT>15</PAGECOUNT></ORGANIZATION>
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

            string whereText = xml.Element("WHERE").Value;
            whereText = string.IsNullOrEmpty(whereText)
                ? string.Empty
                : string.Format(" WHERE ({0})", whereText);
            //添加一个额外标识，不显示 内蒙古强网公司 的记录
            whereText = string.IsNullOrEmpty(whereText)
                ? string.Format(" WHERE {0}", organization_select_where_text)
                : string.Format("{0} AND {1}", whereText, organization_select_where_text);

            string commandText = string.Format(organization_select_commandText, whereText);

            DataSet dataSetOrganization = null;
            try
            {
                dataSetOrganization = new DataAccessHandler().executeDatasetResult(
                    SqlServerCommandText.createPagingCommandText(ref commandText, ref xml), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;

            if (dataSetOrganization != null && dataSetOrganization.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemXml(ref dataSetOrganization, getDataItemTotal(ref whereText)));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = whereText = commandText = null;
            xml = null;
            dataSetOrganization = null;

            return result;
        }

        private string getDataItemTotal(ref string whereText)
        {
            return new DataAccessHandler().executeScalarResult(
                string.Format(organization_total_commandText, whereText), null);
        }

        private string getDataItemXml(ref DataSet dataSetOrganization, string total)
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendFormat("<DATAS COUNT=\"{0}\" TOTAL=\"{1}\">", dataSetOrganization.Tables[0].Rows.Count, total);

            OrganizationObject organizationObject = null;
            foreach (DataRow row in dataSetOrganization.Tables[0].Rows)
            {
                organizationObject = new OrganizationObject(row);

                xml.AppendFormat("<DATA NUM=\"{0}\" ID=\"{1}\" NAME=\"{2}\" EMPLOYEE=\"{3}\" PHONE=\"{4}\" ADDRESS=\"{5}\" CITYID=\"{6}\"/>",
                    organizationObject.NUM,
                    organizationObject.ID,
                    organizationObject.NAME,
                    organizationObject.EMPLOYEE,
                    organizationObject.PHONE,
                    organizationObject.ADDRESS,
                    organizationObject.CITYID
                );
            }
            xml.Append("</DATAS>");

            total = null;
            organizationObject = null;

            return xml.ToString();
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

            DataSet dataSetOrganization = null;
            try
            {
                dataSetOrganization = new DataAccessHandler().executeDatasetResult(
                    organization_details_commandText,
                    SqlServer.GetParameter("ID", xml.Element("ID").Value));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetOrganization != null && dataSetOrganization.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemDetailsXml(ref dataSetOrganization));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = null;
            xml = null;
            dataSetOrganization = null;

            return result;
        }

        private string getDataItemDetailsXml(ref DataSet dataSetOrganization)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");

            OrganizationObject organizationObject = null;
            foreach (DataRow row in dataSetOrganization.Tables[0].Rows)
            {
                organizationObject = new OrganizationObject(row);

                xml.AppendFormat("<DATA ID=\"{0}\" NAME=\"{1}\" EMPLOYEE=\"{2}\" PHONE=\"{3}\" ADDRESS=\"{4}\" CITYID=\"{5}\" NOTE=\"{6}\" />",
                    organizationObject.ID,
                    organizationObject.NAME,
                    organizationObject.EMPLOYEE,
                    organizationObject.PHONE,
                    organizationObject.ADDRESS,
                    organizationObject.CITYID,
                    organizationObject.NOTE
                );
            }
            xml.Append("</DATAS>");

            organizationObject = null;

            return xml.ToString();
        }



        public string getOrganizationList(bool isResultOperation = false)
        {
            DataSet dataSetOrganization = null;
            try
            {
                dataSetOrganization = new DataAccessHandler().executeDatasetResult(
                    organization_list_commandText, null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetOrganization != null && dataSetOrganization.Tables.Count > 0)
            {
                if (isResultOperation)
                {
                    result = Result.getResultXml(getOrganizationListXml(ref dataSetOrganization));
                }
                else
                {
                    result = getOrganizationListXml(ref dataSetOrganization);
                }
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            dataSetOrganization = null;

            return result;
        }

        private string getOrganizationListXml(ref DataSet dataSetOrganization)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<ORGANIZATION>");
            OrganizationObject organizationObject = null;
            foreach (DataRow row in dataSetOrganization.Tables[0].Rows)
            {
                organizationObject = new OrganizationObject(row);
                xml.AppendFormat("<DATA ID=\"{0}\" NAME=\"{1}\"/>",
                    organizationObject.ID,
                    organizationObject.NAME
                );
            }
            xml.Append("</ORGANIZATION>");

            return xml.ToString();
        }
    }
}
