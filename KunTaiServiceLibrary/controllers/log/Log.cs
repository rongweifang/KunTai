using KunTaiServiceLibrary.valueObjects;
using System;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Warrior.DataAccessUtils.Desktop;

namespace KunTaiServiceLibrary
{
    [FluorineFx.RemotingService("日志服务")]
    public class Log : IController
    {

        #region command texts

        private const string log_insert_commandText = "INSERT INTO [LOG] ([SOURCE], [METHODNAME], [LOGLEVEL], [MESSAGE], [CREATETIME], [OPERATIONID]) VALUES (@SOURCE, @METHODNAME, @LOGLEVEL, @MESSAGE, @CREATETIME, @OPERATIONID)";

        private const string log_delete_commandText = "DELETE [LOG] WHERE [ID] IN ({0})";

        private const string log_update_commandText = "UPDATE [LOG] SET [SOURCE]=SOURCE, [METHODNAME]=@METHODNAME, [LOGLEVEL]=@LOGLEVEL, [MESSAGE]=@MESSAGE, [CREATETIME]=@CREATETIME, [OPERATIONID]=@OPERATIONID WHERE [ID]=@ID";

        private const string log_details_commandText = "SELECT [ID], [SOURCE], [METHODNAME], [LOGLEVEL], [MESSAGE], [CREATETIME], [OPERATIONID] FROM [LOG] WHERE [ID]=@ID";

        private const string log_select_commandText = "SELECT ROW_NUMBER() OVER (ORDER BY [CREATETIME] DESC) AS NUM,[ID],[SOURCE],[METHODNAME],[LOGLEVEL],[MESSAGE],[CREATETIME],[OPERATIONID] FROM [LOG]{0}";

        private const string log_total_commandText = "SELECT COUNT(*) FROM [LOG]{0}";

        #endregion


        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="text"></param>
        /// <param name="type"></param>
        public static void write(LogObject logObject)
        {
            if (logObject == null)
                return;

            try
            {
                new DataAccessHandler().executeNonQueryResult(
                    log_insert_commandText,
                    SqlServer.GetParameter(logObject.toXElment(),
                    new string[] { "SOURCE", "METHODNAME", "LOGLEVEL", "MESSAGE", "CREATETIME", "OPERATIONID" })
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }

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
                    log_insert_commandText,
                    SqlServer.GetParameter(xml, new string[] {
                        "SOURCE", "METHODNAME", "LOGLEVEL", "MESSAGE", "CREATETIME", "OPERATIONID" })
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
                    string.Format(log_delete_commandText, xml.Element("ID").Value), null);
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
                    log_insert_commandText,
                    SqlServer.GetParameter(xml, new string[] {
                        "ID", "SOURCE", "METHODNAME", "LOGLEVEL", "MESSAGE", "CREATETIME", "OPERATIONID" })
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

            string whereText = xml.Element("WHERE").Value;
            whereText = string.IsNullOrEmpty(whereText) ? string.Empty : string.Format(" WHERE {0}", whereText);
            string commandText = string.Format(log_select_commandText, whereText);

            DataSet dataSetLog = null;
            try
            {
                dataSetLog = new DataAccessHandler().executeDatasetResult(
                    commandText, null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }


            string result = string.Empty;
            if (dataSetLog != null && dataSetLog.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemXml(ref dataSetLog, getDataItemTotal(ref whereText)));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = whereText = commandText = null;
            xml = null;
            dataSetLog = null;

            return result;
        }

        private string getDataItemXml(ref DataSet dataSetLog, string total)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            LogObject logObject = null;
            foreach (DataRow row in dataSetLog.Tables[0].Rows)
            {
                logObject = new LogObject(row);
                xml.AppendFormat("<DATA NUM=\"{0}\" ID=\"{1}\" SOURCE=\"{2}\" METHODNAME=\"{3}\" LOGLEVEL=\"{4}\" MESSAGE=\"{5}\" CREATETIME=\"{6}\" OPERATIONID=\"{7}\" OPERATIONNAME=\"{8}\"/>",
                    logObject.NUM,
                    logObject.ID,
                    logObject.SOURCE,
                    logObject.METHODNAME,
                    logObject.LOGLEVEL,
                    logObject.MESSAGE,
                    logObject.CREATETIME,
                    logObject.OPERATIONID,
                    logObject.OPERATIONNAME
                );
            }
            xml.Append("</DATAS>");

            logObject = null;

            return xml.ToString();
        }

        private string getDataItemTotal(ref string whereText)
        {
            return new DataAccessHandler().executeScalarResult(
                string.Format(log_total_commandText, whereText), null);
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

            DataSet dataSetLog = null;
            try
            {
                dataSetLog = new DataAccessHandler().executeDatasetResult(
                    log_details_commandText,
                    SqlServer.GetParameter("ID", xml.Element("ID").Value));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetLog != null && dataSetLog.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemDetailsXml(ref dataSetLog));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = null;
            xml = null;
            dataSetLog = null;

            return result;
        }

        private string getDataItemDetailsXml(ref DataSet dataSetLog)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");

            LogObject logObject = null;
            foreach (DataRow row in dataSetLog.Tables[0].Rows)
            {
                logObject = new LogObject(row);
                xml.AppendFormat("<DATA ID=\"{0}\" SOURCE=\"{1}\" METHODNAME=\"{2}\" LOGLEVEL=\"{3}\" MESSAGE=\"{4}\" CREATETIME=\"{5}\" OPERATIONID=\"{6}\" OPERATIONNAME=\"{7}\"/>",
                    logObject.ID,
                    logObject.SOURCE,
                    logObject.METHODNAME,
                    logObject.LOGLEVEL,
                    logObject.MESSAGE,
                    logObject.CREATETIME,
                    logObject.OPERATIONID,
                    logObject.OPERATIONNAME
                );
            }

            xml.Append("</DATAS>");

            logObject = null;

            return xml.ToString();
        }
        
    }


    /// <summary>
    /// 日志类型。
    /// </summary>
    public enum LogType
    {
        // 可增加，不能更改之前的顺序。

        /// <summary>
        /// 错误
        /// </summary>
        Error,

        /// <summary>
        /// 信息
        /// </summary>
        Information,

        /// <summary>
        /// 登录
        /// </summary>
        Login,

        /// <summary>
        /// 登出
        /// </summary>
        Logout
    }

}
