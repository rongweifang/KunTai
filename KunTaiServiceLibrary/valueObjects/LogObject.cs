using System;
using System.Data;
using System.Xml.Linq;

namespace KunTaiServiceLibrary.valueObjects
{
    /// <summary>
    /// 日志对象
    /// </summary>
    public class LogObject
    {
        public string NUM { get; set; }

        /// <summary>
        /// 数据库编号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 日志来源
        /// </summary>
        public string SOURCE { get; set; }

        /// <summary>
        /// 日志方法名
        /// </summary>
        public string METHODNAME { get; set; }


        /// <summary>
        /// 日志级别
        /// </summary>
        public LogType LOGLEVEL { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string MESSAGE { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CREATETIME { get; set; }

        /// <summary>
        /// 操作人编号
        /// </summary>
        public string OPERATIONID { get; set; }

        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string OPERATIONNAME { get; set; }


        public LogObject()
        {

        }

        /// <summary>
        /// 初始化日志对象
        /// </summary>
        /// <param name="id">数据库编号</param>
        /// <param name="source">日志来源</param>
        /// <param name="methodName">日志方法名</param>
        /// <param name="logLevel">日志级别</param>
        /// <param name="message">日志内容</param>
        /// <param name="operationID">操作人编号</param>
        public LogObject(string id, string source, string methodName, LogType logLevel, string message, string operationID, string operationName)
        {
            this.ID = id;
            this.SOURCE = source;
            this.METHODNAME = methodName;
            this.LOGLEVEL = logLevel;
            this.MESSAGE = message;
            this.CREATETIME = Time.getDateTimeNowString();
            this.OPERATIONID = operationID;
            this.OPERATIONNAME = operationName;
        }


        public LogObject(DataRow dataRow)
        {
            if (dataRow != null)
            {
                this.NUM = dataRow.Table.Columns.Contains("NUM") ? dataRow["NUM"].ToString() : string.Empty;
                this.ID = dataRow.Table.Columns.Contains("ID") ? dataRow["ID"].ToString() : string.Empty;
                this.SOURCE = dataRow.Table.Columns.Contains("SOURCE") ? dataRow["SOURCE"].ToString() : string.Empty;
                this.METHODNAME = dataRow.Table.Columns.Contains("SOURCE") ? dataRow["SOURCE"].ToString() : string.Empty;
                if (dataRow.Table.Columns.Contains("LOGLEVEL"))
                {
                    this.LOGLEVEL = (LogType)Enum.Parse(typeof(LogType), dataRow["LOGLEVEL"].ToString(), false);
                }
                this.MESSAGE = dataRow.Table.Columns.Contains("MESSAGE") ? dataRow["MESSAGE"].ToString() : string.Empty;
                this.CREATETIME = dataRow.Table.Columns.Contains("CREATETIME") ? dataRow["CREATETIME"].ToString() : string.Empty;
                this.OPERATIONID = dataRow.Table.Columns.Contains("OPERATIONID") ? dataRow["OPERATIONID"].ToString() : string.Empty;
                this.OPERATIONNAME = dataRow.Table.Columns.Contains("OPERATIONNAME") ? dataRow["OPERATIONNAME"].ToString() : string.Empty;
            }

        }


        public string toXml()
        {
            System.Text.StringBuilder xml = new System.Text.StringBuilder();
            xml.Append("<LOG>");
            xml.AppendFormat("<ID>{0}</ID>", this.ID);
            xml.AppendFormat("<SOURCE>{0}</SOURCE>", this.SOURCE);
            xml.AppendFormat("<METHODNAME>{0}</METHODNAME>", this.METHODNAME);
            xml.AppendFormat("<LOGLEVEL>{0}</LOGLEVEL>", Convert.ToInt32(this.LOGLEVEL));
            xml.AppendFormat("<MESSAGE>{0}</MESSAGE>", this.MESSAGE);
            xml.AppendFormat("<CREATETIME>{0}</CREATETIME>", string.IsNullOrEmpty(this.CREATETIME) ? Time.getDateTimeNowString() : this.CREATETIME);
            xml.AppendFormat("<OPERATIONID>{0}</OPERATIONID>", this.OPERATIONID);
            xml.AppendFormat("<OPERATIONNAME>{0}</OPERATIONNAME>", this.OPERATIONNAME);
            xml.Append("</LOG>");

            return xml.ToString();
        }


        public XElement toXElment()
        {
            return XElement.Parse(this.toXml());
        }


    }
}
