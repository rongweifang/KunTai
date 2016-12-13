using KunTaiServiceLibrary.valueObjects;
using System;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Warrior.DataAccessUtils.Desktop;

namespace KunTaiServiceLibrary
{
    [FluorineFx.RemotingService("用户角色服务")]
    public class Role : IController
    {

        #region command texts

        private const string role_select_commandText = "SELECT ROW_NUMBER() OVER (ORDER BY [SHOWID], [NAME]) AS NUM1, [ID], [NAME], [NOTE] FROM [ROLE]{0}";

        private const string role_total_commandText = "SELECT COUNT(*) FROM [ROLE]{0}";


        private const string role_insert_commandText = "INSERT INTO [ROLE] ([NAME], [NOTE]) VALUES (@NAME, @NOTE)";

        private const string role_update_commandText = "UPDATE [ROLE] SET [NAME]=@NAME, [NOTE]=@NOTE WHERE [ID]=@ID";

        private const string role_delete_commandText = "DELETE [ROLE] WHERE [ID] IN ({0});\n";

        private const string role_details_commandText = "SELECT [ID], [NAME], [NOTE] FROM [ROLE] WHERE [ID]=@ID";


        private const string role_list_select_commandText = "SELECT [ID], [NAME] FROM [ROLE] ORDER BY [SHOWID], [NAME]";


        private const string role_id_select_commandText = "SELECT [ID] FROM [ROLE] WHERE [NAME]=@NAME";

        private const string role_authority_delete_commandText = "DELETE [ROLEAUTHORITY] WHERE [ROLEID] IN ({0});\n";


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
                //判断当前数据中是否存在了相同的数据
                string count = new DataAccessHandler().executeScalarResult(
                    "SELECT COUNT(*) AS COUNT FROM ROLE WHERE NAME=@NAME",
                    SqlServer.GetParameter("NAME", xml.Element("NAME").Value));
                if (Convert.ToInt32(count) > 0)
                {
                    throw new Exception("系统中已经存在相同的角色。");
                }

                result = new DataAccessHandler().executeNonQueryResult(
                    role_insert_commandText,
                    SqlServer.GetParameter(xml, new string[] { "NAME", "NOTE" }));

                /*
                //查询当前插入的角色记录
                string roleID = string.Empty;
                roleID = new DataAccessHandler().executeScalarResult(
                    role_id_select_commandText,
                    SqlServer.GetParameter("NAME", xml.Element("NAME").Value));

                updateRoleAuthority(string.Format(text, roleID));
                */
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
                StringBuilder commandText = new StringBuilder();
                //commandText.Append("BEGIN\n");
                commandText.AppendFormat(role_delete_commandText, xml.Element("ID").Value);
                commandText.AppendFormat(role_authority_delete_commandText, xml.Element("ID").Value);
                //commandText.Append("END;");

                result = new DataAccessHandler().executeNonQueryResult(
                    commandText.ToString(), null);

                commandText = null;
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

                //判断当前数据中是否存在了相同的数据
                string count = new DataAccessHandler().executeScalarResult(
                    "SELECT COUNT(*) AS COUNT FROM ROLE WHERE NAME=@NAME AND ID!=@ID",
                    SqlServer.GetParameter(xml, new string[] { "ID", "NAME" }));
                if (Convert.ToInt32(count) > 0)
                {
                    throw new Exception("系统中已经存在相同的角色。");
                }

                result = new DataAccessHandler().executeNonQueryResult(
                    role_update_commandText,
                    SqlServer.GetParameter(xml, new string[] { "ID", "NAME", "NOTE" }));

                /*
                updateRoleAuthority(text);
                */
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
            string commandText = string.Format(role_select_commandText, whereText);

            DataSet dataSetRole = null;
            try
            {
                dataSetRole = new DataAccessHandler().executeDatasetResult(
                    SqlServerCommandText.createPagingCommandText(ref commandText, ref xml),
                    null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetRole != null && dataSetRole.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemXml(ref dataSetRole, getDataItemTotal(ref whereText)));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = whereText = commandText = null;
            xml = null;
            dataSetRole = null;

            return result;
        }

        private string getDataItemXml(ref DataSet dataSetRole, string total)
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendFormat("<DATAS COUNT=\"{0}\" TOTAL=\"{1}\">", dataSetRole.Tables[0].Rows.Count, total);
            RoleObject roleObject = null;
            foreach (DataRow row in dataSetRole.Tables[0].Rows)
            {
                roleObject = new RoleObject(row);
                xml.AppendFormat("<DATA NUM=\"{0}\" ID=\"{1}\" NAME=\"{2}\" NOTE=\"{3}\"/>",
                    roleObject.NUM,
                    roleObject.ID,
                    roleObject.NAME,
                    roleObject.NOTE
                );
                roleObject = null;
            }
            xml.Append("</DATAS>");
            
            return xml.ToString();
        }

        private string getDataItemTotal(ref string whereText)
        {
            return new DataAccessHandler().executeScalarResult(
                string.Format(role_total_commandText, whereText), null);
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

            DataSet dataSetRole = null;
            try
            {
                dataSetRole = new DataAccessHandler().executeDatasetResult(
                    role_details_commandText,
                    SqlServer.GetParameter("ID", xml.Element("ID").Value));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetRole != null && dataSetRole.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemDetailsXml(ref dataSetRole));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = null;
            xml = null;
            dataSetRole = null;

            return result;
        }

        private string getDataItemDetailsXml(ref DataSet dataSetRole)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            RoleObject roleObject = null;
            foreach (DataRow row in dataSetRole.Tables[0].Rows)
            {
                roleObject = new RoleObject(row);
                xml.AppendFormat("<DATA ID=\"{0}\" NAME=\"{1}\" NOTE=\"{2}\"/>",
                    roleObject.ID,
                    roleObject.NAME,
                    roleObject.NOTE
                );
            }
            xml.Append("</DATAS>");

            roleObject = null;

            return xml.ToString();
        }


        #region get role list

        public string getRoleList(bool isResultOperation = false)
        {
            DataSet dataSetRole = null;
            try
            {
                dataSetRole = new DataAccessHandler().executeDatasetResult(
                    role_list_select_commandText, null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetRole != null && dataSetRole.Tables.Count > 0)
            {
                if (isResultOperation)
                {
                    result = Result.getResultXml(getRoleListXml(ref dataSetRole));
                }
                else
                {
                    result = getRoleListXml(ref dataSetRole);
                }
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            dataSetRole = null;

            return result;
        }

        private string getRoleListXml(ref DataSet dataSetRole)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<ROLE>");
            RoleObject roleObject = null;
            foreach (DataRow row in dataSetRole.Tables[0].Rows)
            {
                roleObject = new RoleObject(row);
                xml.AppendFormat("<DATA ID=\"{0}\" NAME=\"{1}\"/>",
                    roleObject.ID,
                    roleObject.NAME
                );
            }
            xml.Append("</ROLE>");

            return xml.ToString();
        }

        #endregion


        #region get role authority

        public string getRoleAuthority(string text)
        {
            return new Authority().getRoleAuthority(text);
        }

        #endregion


        #region update role authority

        public string updateRoleAuthority(string text)
        {
            return new Authority().updateRoleAuthority(text);
        }

        #endregion

    }
}
