using KunTaiServiceLibrary.valueObjects;
using System;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Warrior.DataAccessUtils.Desktop;

namespace KunTaiServiceLibrary
{
    [FluorineFx.RemotingService("用户管理服务")]
    public class Employee : IController
    {

        #region command texts

        private const string employee_insert_commandText = "INSERT INTO [EMPLOYEE] ([NAME], [OID], [ROLEID], [MOBILE], [EMAIL], [LOGINNAME], [LOGINPASSWORD], [NOTE]) VALUES (@NAME, @OID, @ROLEID, @MOBILE, @EMAIL, @LOGINNAME, @LOGINPASSWORD, @NOTE)";

        private const string employee_update_commandText = "UPDATE [EMPLOYEE] SET [NAME]=@NAME,[OID]=@OID,[ROLEID]=@ROLEID,[MOBILE]=@MOBILE,[EMAIL]=@EMAIL,[LOGINNAME]=@LOGINNAME,[LOGINPASSWORD]=@LOGINPASSWORD,[NOTE]=@NOTE WHERE [ID]=@ID";

        private const string employee_delete_commandText = "DELETE [EMPLOYEE] WHERE [ID] IN ({0})";

        private const string employee_login_select_commandText = "SELECT E.[ID], E.[NAME], O.[NAME] AS ORGANIZATIONAME FROM [EMPLOYEE] E, [ORGANIZATION] O WHERE E.[OID]=O.ID AND E.[LOGINNAME]=@LOGINNAME AND E.[LOGINPASSWORD]=@LOGINPASSWORD";

        private const string employee_role_select_commandText = "SELECT [ROLEID] FROM EMPLOYEE WHERE ID=@ID";

        private const string employee_select_commandText = "SELECT ROW_NUMBER() OVER (ORDER BY O.[SHOWID],R.[SHOWID],O.[NAME],R.[NAME]) AS NUM1, E.[ID], E.[NAME], O.[NAME] AS ORGANIZATIONNAME, R.[NAME] AS ROLENAME, E.[MOBILE], E.[LOGINNAME], E.[LOGINPASSWORD] FROM [EMPLOYEE] E, [ORGANIZATION] O, [ROLE] R WHERE E.ROLEID=R.ID AND E.OID=O.ID{0}";

        private const string employee_total_commandText = "SELECT COUNT(*) FROM [EMPLOYEE] E, [ORGANIZATION] O, [ROLE] R WHERE E.ROLEID=R.ID AND E.OID=O.ID{0}";

        private const string employee_details_commandText = "SELECT [ID], [NAME], [OID], [ROLEID], [MOBILE], [EMAIL], [LOGINNAME], [LOGINPASSWORD], [NOTE] FROM [EMPLOYEE] WHERE ID=@ID";

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
                //检查是否有重复的登录名
                //判断当前数据中是否存在了相同的数据
                string count = new DataAccessHandler().executeScalarResult(
                    "SELECT COUNT(*) AS COUNT FROM EMPLOYEE WHERE LOGINNAME=@LOGINNAME",
                    SqlServer.GetParameter("LOGINNAME", xml.Element("LOGINNAME").Value));
                if (Convert.ToInt32(count) > 0)
                {
                    throw new Exception("系统中已经存在相同的登录名。");
                }

                result = new DataAccessHandler().executeNonQueryResult(
                    employee_insert_commandText,
                    SqlServer.GetParameter(xml,
                    new string[] { "NAME", "OID", "ROLEID", "MOBILE",
                        "EMAIL", "LOGINNAME", "LOGINPASSWORD", "NOTE" }));
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
                    string.Format(employee_delete_commandText, xml.Element("ID").Value), null);
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
                //检查是否有重复的登录名
                //判断当前数据中是否存在了相同的数据
                string count = new DataAccessHandler().executeScalarResult(
                    "SELECT COUNT(*) AS COUNT FROM EMPLOYEE WHERE LOGINNAME=@LOGINNAME AND ID!=@ID",
                    SqlServer.GetParameter(xml, new string[] { "LOGINNAME", "ID" }));
                if (Convert.ToInt32(count) > 0)
                {
                    throw new Exception("系统中已经存在相同的登录名。");
                }

                result = new DataAccessHandler().executeNonQueryResult(
                    employee_update_commandText,
                    SqlServer.GetParameter(xml,
                    new string[] { "ID", "NAME", "OID", "ROLEID", "MOBILE",
                        "EMAIL", "LOGINNAME", "LOGINPASSWORD", "NOTE" }));
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
                : string.Format(" AND {0}", xml.Element("WHERE").Value);
            string commandText = string.Format(employee_select_commandText, whereText);

            DataSet dataSetEmployee = null;
            try
            {
                dataSetEmployee = new DataAccessHandler().executeDatasetResult(
                    SqlServerCommandText.createPagingCommandText(ref commandText, ref xml), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetEmployee != null && dataSetEmployee.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemXml(ref dataSetEmployee, getDataItemTotal(ref whereText)));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = whereText = commandText = null;
            xml = null;
            dataSetEmployee = null;

            return result;
        }

        private string getDataItemXml(ref DataSet dataSetEmployee, string total)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            EmployeeObject employeeObject = null;
            foreach (DataRow row in dataSetEmployee.Tables[0].Rows)
            {
                employeeObject = new EmployeeObject(row);
                xml.AppendFormat("<DATA NUM=\"{0}\" ID=\"{1}\" NAME=\"{2}\" ORGANIZATIONNAME=\"{3}\" ROLENAME=\"{4}\" MOBILE=\"{5}\" LOGINNAME=\"{6}\" LOGINPASSWORD=\"{7}\"/>",
                    employeeObject.NUM,
                    employeeObject.ID,
                    employeeObject.NAME,
                    employeeObject.ORGANIZATIONNAME,
                    employeeObject.ROLENAME,
                    employeeObject.MOBILE,
                    employeeObject.LOGINNAME,
                    employeeObject.LOGINPASSWORD
                );
            }
            xml.Append("</DATAS>");

            employeeObject = null;

            return xml.ToString();
        }

        private string getDataItemTotal(ref string whereText)
        {
            return new DataAccessHandler().executeScalarResult(
                string.Format(employee_total_commandText, whereText), null);
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

            DataSet dataSetEmployee = null;
            try
            {
                dataSetEmployee = new DataAccessHandler().executeDatasetResult(
                    employee_details_commandText,
                    SqlServer.GetParameter("ID", xml.Element("ID").Value));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }


            string result = string.Empty;
            if (dataSetEmployee != null && dataSetEmployee.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemDetailsXml(ref dataSetEmployee));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = null;
            xml = null;
            dataSetEmployee = null;

            return result;
        }

        private string getDataItemDetailsXml(ref DataSet dataSetEmployee)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            EmployeeObject employeeObject = null;
            foreach (DataRow row in dataSetEmployee.Tables[0].Rows)
            {
                employeeObject = new EmployeeObject(row);
                xml.AppendFormat("<DATA ID=\"{0}\" NAME=\"{1}\" OID=\"{2}\" ROLEID=\"{3}\" MOBILE=\"{4}\" EMAIL=\"{5}\" LOGINNAME=\"{6}\" LOGINPASSWORD=\"{7}\" NOTE=\"{8}\"/>",
                    employeeObject.ID,
                    employeeObject.NAME,
                    employeeObject.OID,
                    employeeObject.ROLEID,
                    employeeObject.MOBILE,
                    employeeObject.EMAIL,
                    employeeObject.LOGINNAME,
                    employeeObject.LOGINPASSWORD,
                    employeeObject.NOTE
                );
            }
            xml.Append("</DATAS>");

            employeeObject = null;

            return xml.ToString();
        }



        #region login

        public string employeeLogin(string text)
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


            //3.正常登录
            DataSet dataSetEmployee = null;
            try
            {
                dataSetEmployee = new DataAccessHandler().executeDatasetResult(
                    employee_login_select_commandText,
                    SqlServer.GetParameter(xml, new string[] { "LOGINNAME", "LOGINPASSWORD" }));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetEmployee != null && dataSetEmployee.Tables.Count > 0 && dataSetEmployee.Tables[0].Rows.Count > 0)
            {
                result = Result.getResultXml(getEmployeeLoginXml(ref dataSetEmployee));
            }
            else
            {

                result = Result.getFaultXml("登录失败。请检查密码是否输入正确，或者联系管理员。");
            }

            text = null;
            xml = null;
            dataSetEmployee = null;

            return result;
        }

        private string getEmployeeLoginXml(ref DataSet dataSetEmployee)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            foreach (DataRow row in dataSetEmployee.Tables[0].Rows)
            {
                xml.AppendFormat("<DATA ID=\"{0}\" NAME=\"{1}\" ORGANIZATIONAME=\"{2}\"/>",
                    row["ID"].ToString(),
                    row["NAME"].ToString(),
                    row["ORGANIZATIONAME"].ToString()
                );
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }

        #endregion


        #region get employee authority

        public string getEmployeeAuthority(string text)
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
                string roleID = new DataAccessHandler().executeScalarResult(
                    employee_role_select_commandText,
                    SqlServer.GetParameter("ID", xml.Element("ID").Value));

                result = new Authority().getRoleAuthority(
                    string.Format("<ROLEAUTHORITY><ID>'{0}'</ID><TYPE>MENU</TYPE></ROLEAUTHORITY>", roleID));

                roleID = null;
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = null;

            return result;
        }

        #endregion


        public string getParameters()
        {
            StringBuilder result = new StringBuilder();
            result.Append("<DATAS>");
            result.Append(new Role().getRoleList());
            result.Append(new Organization().getOrganizationList());
            result.Append("</DATAS>");

            return Result.getResultXml(result.ToString());
        }


    }
}
