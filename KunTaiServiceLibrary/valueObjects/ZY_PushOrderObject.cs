using System.Data;
using System.Xml.Linq;

namespace KunTaiServiceLibrary.valueObjects
{
    public class ZY_PushOrderObject
    {
        /// <summary>
        /// 显示序号
        /// </summary>
        public string NUM { get; set; }

        /// <summary>
        /// 数据库编号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 指令运行时间2016-01-20
        /// </summary>
        public string RUNDATE { get; set; }

        /// <summary>
        /// 添加时间。用于排序
        /// </summary>
        public string ADDDATETIME { get; set; }

        /// <summary>
        /// 最高温度值
        /// </summary>
        public string MAXVALUE { get; set; }

        /// <summary>
        /// 最低温度值
        /// </summary>
        public string MINVALUE { get; set; }

        /// <summary>
        /// 导出类型（1启4停等）
        /// </summary>
        public string EXPORTTYPE { get; set; }


        /// <summary>
        /// 指令显示的名字
        /// </summary>
        public string FILENAME { get; set; }

        /// <summary>
        /// 生成指令的zip文件（里面可能存放多个导出类型命名的excel）
        /// </summary>
        public string FILEURL { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string NOTE { get; set; }

        public string ISTODAYORDER { get; set; }

        public ZY_PushOrderObject()
        {

        }


        public ZY_PushOrderObject(XElement xml)
        {
            if (xml != null)
            {
                this.NUM = xml.Element("NUM") == null ? string.Empty : xml.Element("NUM").Value;
                this.ID = xml.Element("ID") == null ? string.Empty : xml.Element("ID").Value;
                this.RUNDATE = xml.Element("RUNDATE") == null ? string.Empty : xml.Element("RUNDATE").Value;
                this.ADDDATETIME = xml.Element("ADDDATETIME") == null ? string.Empty : xml.Element("ADDDATETIME").Value;
                this.MAXVALUE = xml.Element("MAXVALUE") == null ? string.Empty : xml.Element("MAXVALUE").Value;
                this.MINVALUE = xml.Element("MINVALUE") == null ? string.Empty : xml.Element("MINVALUE").Value;
                this.EXPORTTYPE = xml.Element("EXPORTTYPE") == null ? string.Empty : xml.Element("EXPORTTYPE").Value;
                this.FILENAME = xml.Element("FILENAME") == null ? string.Empty : xml.Element("FILENAME").Value;
                this.FILEURL = xml.Element("FILEURL") == null ? string.Empty :
                    xml.Element("FILEURL").Value.Replace(Config.UploadExportFileHttpUrl, "").Replace("/", "");
                this.NOTE = xml.Element("NOTE") == null ? string.Empty : xml.Element("NOTE").Value;
                this.ISTODAYORDER  = xml.Element("ISTODAYORDER") == null ? string.Empty : xml.Element("ISTODAYORDER").Value;
            }
        }

        public ZY_PushOrderObject(DataRow dataRow)
        {
            if (dataRow != null)
            {
                this.NUM = dataRow.Table.Columns.Contains("NUM") ? dataRow["NUM"].ToString() : string.Empty;
                this.ID = dataRow.Table.Columns.Contains("ID") ? dataRow["ID"].ToString() : string.Empty;
                this.RUNDATE = dataRow.Table.Columns.Contains("RUNDATE") ? dataRow["RUNDATE"].ToString() : string.Empty;
                this.ADDDATETIME = dataRow.Table.Columns.Contains("ADDDATETIME") ? dataRow["ADDDATETIME"].ToString() : string.Empty;
                this.MAXVALUE = dataRow.Table.Columns.Contains("MAXVALUE") ? dataRow["MAXVALUE"].ToString() : string.Empty;
                this.MINVALUE = dataRow.Table.Columns.Contains("MINVALUE") ? dataRow["MINVALUE"].ToString() : string.Empty;
                this.EXPORTTYPE = dataRow.Table.Columns.Contains("EXPORTTYPE") ? dataRow["EXPORTTYPE"].ToString() : string.Empty;
                this.FILENAME = dataRow.Table.Columns.Contains("FILENAME") ? dataRow["FILENAME"].ToString() : string.Empty;
                this.FILEURL = dataRow.Table.Columns.Contains("FILEURL") ? dataRow["FILEURL"].ToString() : string.Empty;
                this.NOTE = dataRow.Table.Columns.Contains("NOTE") ? dataRow["NOTE"].ToString() : string.Empty;
                this.ISTODAYORDER = dataRow.Table.Columns.Contains("ISTODAYORDER") ? dataRow["ISTODAYORDER"].ToString() : string.Empty;
            }
        }

    }
}
