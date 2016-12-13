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
    [FluorineFx.RemotingService("下发指令服务")]
    public class PushOrder : IController
    {

        #region command texts

        private const string pushOrder_insert_commandText = "INSERT INTO [PUSHORDER] ([ID], [PUSHID], [RUNDATETIME], [FILEURL], [LOCALFILENAME], [NOTE]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');\n";

        private const string pushOrder_receive_insert = "INSERT INTO [PUSHORDERRECEIVE] ([POID], [RECEIVEID]) VALUES ('{0}', '{1}');\n";

        private const string pushOrder_receive_update = "UPDATE [PUSHORDER] SET [PUSHDATETIME]=GETDATE(), [RUNDATETIME]='{0}', [FILEURL]='{1}', [LOCALFILENAME]='{2}', [NOTE]='{3}' WHERE [ID]='{4}';\n";
        
        private const string pushOrder_receive_delete = "DELETE [PUSHORDERRECEIVE] WHERE [POID] IN ({0});\n";

        private const string pushOrder_delete_commandText = "DELETE [PUSHORDER] WHERE [ID] IN ({0});\n";

        private const string pushOrder_select_commandText = "SELECT (SELECT ROW_NUMBER() OVER (ORDER BY [PUSHDATETIME])) AS NUM1,T1.* FROM((SELECT T.* FROM (SELECT PO.[ID], E.[NAME], CONVERT(VARCHAR(19),PO.[PUSHDATETIME],120) AS [PUSHDATETIME], CONVERT(VARCHAR(19),PO.[RUNDATETIME],120) AS [RUNDATETIME], PO.[LOCALFILENAME], PO.[FILEURL], (SELECT STUFF((SELECT ',''' + CONVERT(VARCHAR(MAX), [RECEIVEID]) + ''''  FROM [PUSHORDERRECEIVE] WHERE [POID]=PO.[ID] FOR XML PATH('')),1,1,'')) AS RECEIVEEMPLOYEE  FROM [PUSHORDER] PO, [EMPLOYEE] E WHERE (PO.[RUNDATETIME]='{0}' OR PUSHDATETIME BETWEEN '{1} 00:00:01' AND '{1} 23:59:59') AND PO.[PUSHID]=E.ID) T WHERE RECEIVEEMPLOYEE LIKE '%{2}%' UNION ALL SELECT PO.[ID], E.[NAME], CONVERT(VARCHAR(19),PO.[PUSHDATETIME],120) AS [PUSHDATETIME], CONVERT(VARCHAR(19),PO.[RUNDATETIME],120) AS [RUNDATETIME], PO.[LOCALFILENAME], PO.[FILEURL], (SELECT STUFF((SELECT ',''' + CONVERT(VARCHAR(MAX), [RECEIVEID]) + ''''  FROM [PUSHORDERRECEIVE] WHERE [POID]=PO.[ID] FOR XML PATH('')),1,1,'')) AS RECEIVEEMPLOYEE FROM [PUSHORDER] PO, [EMPLOYEE] E  WHERE (PO.[RUNDATETIME]='{0}' OR PUSHDATETIME BETWEEN '{1} 00:00:01' AND '{1} 23:59:59') AND PO.[PUSHID]='{2}' AND PO.[PUSHID]=E.ID)) T1";

        private const string pushOrder_total_commandText = "SELECT COUNT(*) FROM [PUSHORDER] WHERE [PUSHID]='{0}' AND [RUNDATETIME]='{1}'";

        private const string employee_name_select = "SELECT STUFF((SELECT ', ' + CONVERT(VARCHAR(MAX), NAME) + ''  FROM EMPLOYEE WHERE ID IN ({0}) FOR XML PATH('')),1,1,'')";

        private const string pushOrder_receive_item = "SELECT ROW_NUMBER() OVER (ORDER BY E.[ID]) AS NUM,E.[ID], E.[NAME], E.[MOBILE],E.[LOGINNAME], '0' AS STATE FROM [EMPLOYEE] E, [ORGANIZATION] O, [ROLE] R WHERE E.[ROLEID] IN (SELECT [ROLEID] FROM [RECEIVEORDERROLE])  AND E.[OID]=O.[ID] AND E.[ROLEID]=R.[ID] ORDER BY O.[SHOWID], R.[SHOWID], E.[NAME]";
        private const string pushOrder_receive_item1 = "SELECT ROW_NUMBER() OVER (ORDER BY E.[ID]) AS NUM,E.[ID], E.[NAME], E.[MOBILE],E.[LOGINNAME], (CASE WHEN POR.RECEIVEID IS NULL THEN 0 ELSE 1 END) AS STATE FROM EMPLOYEE E LEFT JOIN (SELECT DISTINCT RECEIVEID FROM PUSHORDERRECEIVE WHERE POID='{0}') POR ON E.ID=POR.RECEIVEID WHERE ROLEID IN (SELECT [ROLEID] FROM [RECEIVEORDERROLE])";

        private const string pushOrder_details_commandText = "SELECT PO.[ID], PO.[LOCALFILENAME], PO.[NOTE], (SELECT STUFF((SELECT ',' + CONVERT(VARCHAR(MAX), [RECEIVEID]) + ''  FROM [PUSHORDERRECEIVE] WHERE [POID]=PO.[ID] FOR XML PATH('')),1,1,'')) AS RECEIVEEMPLOYEE FROM [PUSHORDER] PO WHERE PO.ID=@ID";

        #endregion


        public string addDataItem(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Result.getFaultXml(Error.XML_IS_NULL);

            //更新运行指令日期
            string runDateTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            text = text.Replace("@RUNDATETIME@", runDateTime);

            string newID = Guid.NewGuid().ToString().ToUpper();
            if (text.Contains("@ID@"))
                text = text.Replace("@ID@", newID);

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

            StringBuilder command = new StringBuilder();

            PushOrderObject pushOrderObject = new PushOrderObject(xml);

            command.AppendFormat(pushOrder_insert_commandText,
                pushOrderObject.ID,
                pushOrderObject.PUSHID,
                pushOrderObject.RUNDATETIME,
                pushOrderObject.FILEURL,
                pushOrderObject.LOCALFILENAME,
                pushOrderObject.NOTE
            );

            foreach (PushOrderReceiveObject item in pushOrderObject.RECEIVEITEM)
            {
                command.AppendFormat(pushOrder_receive_insert,
                    item.POID,
                    item.RECEIVEID
                );
            }

            //开始插入
            try
            {
                result = new DataAccessHandler().executeNonQueryResult(
                    command.ToString(), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }


            pushOrderObject = null;

            command = null;

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

            StringBuilder command = new StringBuilder();

            string pushOrderID = xml.Element("ID").Value;

            //删除这个ID下面的接收人记录
            command.AppendFormat(pushOrder_receive_delete, pushOrderID);

            //删除记录
            command.AppendFormat(pushOrder_delete_commandText, pushOrderID);

            string result = string.Empty;
            try
            {
                result = new DataAccessHandler().executeNonQueryResult(
                    command.ToString(), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = pushOrderID = null;
            xml = null;
            command = null;

            return result;
        }

        public string editDataItem(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Result.getFaultXml(Error.XML_IS_NULL);

            //更新运行指令日期
            string runDateTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            text = text.Replace("@RUNDATETIME@", runDateTime);

            XElement xml = null;
            try
            {
                xml = XElement.Parse(text);
            }
            catch
            {
                return Result.getFaultXml(Error.XML_FORMAT_ERROR);
            }

            StringBuilder command = new StringBuilder();

            string pushOrderID = xml.Element("ID").Value;

            //删除这个ID下面的接收人记录
            command.AppendFormat(pushOrder_receive_delete, string.Format("'{0}'", pushOrderID));

            PushOrderObject pushOrderObject = new PushOrderObject(xml);
            //重新添加新的接收人记录
            foreach (PushOrderReceiveObject item in pushOrderObject.RECEIVEITEM)
            {
                command.AppendFormat(pushOrder_receive_insert,
                    item.POID,
                    item.RECEIVEID
                );
            }

            //更新记录
            command.AppendFormat(pushOrder_receive_update,
                pushOrderObject.RUNDATETIME,
                pushOrderObject.FILEURL,
                pushOrderObject.LOCALFILENAME,
                pushOrderObject.NOTE,
                pushOrderObject.ID);

            string result = string.Empty;
            try
            {
                result = new DataAccessHandler().executeNonQueryResult(
                    command.ToString(), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = pushOrderID = null;
            xml = null;
            command = null;

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

            string pushID = xml.Element("PUSHID").Value.ToUpper();
            string pushDateTime = DateTime.Now.ToString("yyyy-MM-dd");
            string runDateTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            //这里按照2种方式进行查询
            //1 是PUSHID发布的指令
            //2 是PUSHID接收的指令
            string commandText = string.Format(pushOrder_select_commandText, runDateTime, pushDateTime, pushID);
            DataSet dataSetPushOrder = null;
            try
            {
                dataSetPushOrder = new DataAccessHandler().executeDatasetResult(
                    SqlServerCommandText.createPagingCommandText(ref commandText, ref xml, "[PUSHDATETIME]"), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetPushOrder != null && dataSetPushOrder.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemXml(ref dataSetPushOrder, getDataItemTotal(pushID, runDateTime)));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = pushID = runDateTime = commandText = null;
            xml = null;
            dataSetPushOrder = null;

            return result;
        }

        private string getDataItemXml(ref DataSet dataSetPushOrder, string total)
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendFormat("<DATAS COUNT=\"{0}\" TOTAL=\"{1}\">", dataSetPushOrder.Tables[0].Rows.Count, total);
            PushOrderObject pushOrderObject = null;
            foreach (DataRow row in dataSetPushOrder.Tables[0].Rows)
            {
                pushOrderObject = new PushOrderObject(row);

                xml.AppendFormat("<DATA NUM=\"{0}\" ID=\"{1}\" RUNDATETIME=\"{2}\" NAME=\"{3}\" PUSHDATETIME=\"{4}\" RECEIVEEMPLOYEE=\"{5}\" LOCALFILENAME=\"{6}\" FILEURL=\"{7}\"/>",
                    pushOrderObject.NUM,
                    pushOrderObject.ID,
                    pushOrderObject.RUNDATETIME,
                    pushOrderObject.NAME,
                    pushOrderObject.PUSHDATETIME,
                    getEmployeeText(pushOrderObject.RECEIVEEMPLOYEE),
                    pushOrderObject.LOCALFILENAME,
                    pushOrderObject.FILEURL
                );
            }
            xml.Append("</DATAS>");

            pushOrderObject = null;

            return xml.ToString();
        }

        private string getEmployeeText(string id)
        {
            return new DataAccessHandler().executeScalarResult(
                string.Format(employee_name_select, id), null);
        }

        private string getDataItemTotal(string pushID, string runDateTime)
        {
            return new DataAccessHandler().executeScalarResult(
                string.Format(pushOrder_total_commandText, pushID, runDateTime), null);
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

            DataSet dataSetPushOrder = null;
            try
            {
                dataSetPushOrder = new DataAccessHandler().executeDatasetResult(
                    pushOrder_details_commandText,
                    SqlServer.GetParameter("ID", xml.Element("ID").Value));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetPushOrder != null && dataSetPushOrder.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemDetailsXml(ref dataSetPushOrder));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = null;
            xml = null;
            dataSetPushOrder = null;

            return result;
        }

        private string getDataItemDetailsXml(ref DataSet dataSetPushOrder)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            PushOrderObject pushOrderObject = null;
            foreach (DataRow row in dataSetPushOrder.Tables[0].Rows)
            {
                pushOrderObject = new PushOrderObject(row);

                xml.AppendFormat("<DATA ID=\"{0}\" LOCALFILENAME=\"{1}\" RECEIVEEMPLOYEE=\"{2}\" NOTE=\"{3}\"/>",
                    pushOrderObject.ID,
                    pushOrderObject.LOCALFILENAME,
                    pushOrderObject.RECEIVEEMPLOYEE,
                    pushOrderObject.NOTE
                );
            }
            xml.Append("</DATAS>");

            pushOrderObject = null;

            return xml.ToString();
        }

        public string getPushOrderReceiveItem(string text)
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

            DataSet dataSetPushOrderReceiveItem = null;
            try
            {
                dataSetPushOrderReceiveItem = new DataAccessHandler()
                    .executeDatasetResult(
                    string.IsNullOrEmpty(pushOrderID)
                    ? pushOrder_receive_item //? 添加新的指令
                    : string.Format(pushOrder_receive_item1, pushOrderID), //? 编辑指令，查询状态
                    null);
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#endif
            }

            string result = string.Empty;
            if (dataSetPushOrderReceiveItem != null && dataSetPushOrderReceiveItem.Tables.Count > 0)
            {
                result = Result.getResultXml(getPushOrderReceiveItemXml(ref dataSetPushOrderReceiveItem));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            dataSetPushOrderReceiveItem = null;

            return result;
        }

        private string getPushOrderReceiveItemXml(ref DataSet dataSetPushOrderReceiveItem)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            EmployeeObject employeeObject = null;
            foreach (DataRow row in dataSetPushOrderReceiveItem.Tables[0].Rows)
            {
                employeeObject = new EmployeeObject(row);
                xml.AppendFormat("<DATA NUM=\"{0}\" ID=\"{1}\" NAME=\"{2}\" MOBILE=\"{3}\" LOGINNAME=\"{4}\" SELECTED=\"{5}\"/>",
                    employeeObject.NUM,
                    employeeObject.ID,
                    employeeObject.NAME,
                    string.IsNullOrEmpty(employeeObject.MOBILE) ? "未设置电话" : employeeObject.MOBILE,
                    employeeObject.LOGINNAME,
                    row["STATE"].ToString() == "1" ? "TRUE" : "FALSE" //? 单独判断
                );
            }
            xml.Append("</DATAS>");

            employeeObject = null;

            return xml.ToString();
        }


        public string getEmployeeType(string text)
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

            string employeeID = xml.Element("EMPLOYEEID").Value.ToUpper();

            string result = PushOrderAdminUtils.isAdmin(employeeID)
                ? Result.getResultXml(getEmployeeTypeXml("1"))
                : Result.getResultXml(getEmployeeTypeXml("0"));

            employeeID = null;

            return result;
        }

        private string getEmployeeTypeXml(string type)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            xml.AppendFormat("<DATA TYPE=\"{0}\"/>", type);
            xml.Append("</DATAS>");

            return xml.ToString();
        }



        public string getDownFileName(string text)
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

            string fileName = xml.Element("FILENAME").Value;
            List<PushOrderObject> listFiles = new List<PushOrderObject>();
            foreach (var item in xml.Element("FILES").Elements("FILE"))
            {
                listFiles.Add(new PushOrderObject()
                {
                    FILEURL = item.Attribute("FILEURL").Value,
                    LOCALFILENAME = item.Attribute("LOCALFILENAME").Value
                });
            }

            string fileUrl = string.Empty;//要下载的文件HTTP路径
            //多个文件，需要转移到一个临时文件夹内
            if (listFiles.Count > 1)
            {
                string tempNewDirectory = Path.Combine(Config.UploadExportFileDirectory, fileName);

                //新建一个临时文件夹
                Directory.CreateDirectory(tempNewDirectory);

                //依次将文件复制临时文件夹内
                foreach (PushOrderObject file in listFiles)
                {
                    File.Copy(Path.Combine(Config.UploadExportFileDirectory, file.FILEURL), Path.Combine(tempNewDirectory, file.LOCALFILENAME));
                }

                //生成zip文件
                string zipFileName = string.Format("{0}.zip", fileName);
                string zipFilePath = Path.Combine(Config.UploadExportFileDirectory, zipFileName);
                //在物理目录下创建一个zip文件
                Zip.CompressionDirectory(tempNewDirectory, zipFilePath, 0);

                //删除临时文件夹
                Directory.Delete(tempNewDirectory, true);

                fileUrl = string.Format("{0}/{1}", Config.UploadExportFileHttpUrl, zipFileName);

                zipFilePath = zipFileName = null;
            }
            else
            {
                fileUrl = string.Format("{0}/{1}", Config.UploadExportFileHttpUrl, listFiles[0].FILEURL);
            }

            string result = string.Empty;
            if (!string.IsNullOrEmpty(fileUrl))
            {
                result = Result.getResultXml(getDownFileNameXml(ref fileUrl));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = fileUrl = fileName = null;
            xml = null;
            listFiles.Clear();
            listFiles = null;

            return result;
        }

        private string getDownFileNameXml(ref string fileUrl)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            xml.AppendFormat("<DATA FILEURL=\"{0}\"/>", fileUrl);
            xml.Append("</DATAS>");

            return xml.ToString();
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

            string zipFileName = xml.Element("ZIPFILENAME").Value;

            string zipFilePath = Path.Combine(Config.UploadExportFileDirectory, zipFileName);

            string result = string.Empty;
            if (File.Exists(zipFilePath))
            {
                try
                {
                    File.Delete(zipFilePath);
                    result = "1";
                }
                catch (Exception ex)
                {
                    return Result.getFaultXml(ex.Message);
                }
            }

            text = zipFileName = zipFilePath = null;
            xml = null;

            return result == "1" ? Result.getResultXml(string.Empty) : Result.getFaultXml("执行失败。");
        }



    }
}
