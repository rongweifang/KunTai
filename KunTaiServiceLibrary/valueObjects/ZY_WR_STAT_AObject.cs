using System.Data;
using System.Xml.Linq;

namespace KunTaiServiceLibrary.valueObjects
{
    public class ZY_WR_STAT_AObject
    {
        /// <summary>
        /// 显示编号
        /// </summary>
        public string NUM { get; set; }

        /// <summary>
        /// 数据库编号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string STCD { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string LONGITUDE { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string LATITUDE { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string NOTE { get; set; }


        /// <summary>
        /// 回路名称
        /// </summary>
        public string SELECTED { get; set; }


        public ZY_WR_STAT_AObject(XElement xml)
        {
            if (xml != null)
            {
                this.NUM = xml.Attribute("NUM") == null ? string.Empty : xml.Attribute("NUM").Value;
                this.ID = xml.Attribute("ID") == null ? string.Empty : xml.Attribute("ID").Value;
                this.STCD = xml.Attribute("STCD") == null ? string.Empty : xml.Attribute("STCD").Value;
                this.NAME = xml.Attribute("NAME") == null ? string.Empty : xml.Attribute("NAME").Value;
                this.LONGITUDE = xml.Attribute("LONGITUDE") == null ? string.Empty : xml.Attribute("LONGITUDE").Value;
                this.LATITUDE = xml.Attribute("LATITUDE") == null ? string.Empty : xml.Attribute("LATITUDE").Value;
                this.NOTE = xml.Attribute("NOTE") == null ? string.Empty : xml.Attribute("NOTE").Value;
                this.SELECTED = xml.Attribute("TYPE") == null ? string.Empty : xml.Attribute("TYPE").Value;
            }
        }


        public ZY_WR_STAT_AObject(DataRow dataRow)
        {
            if (dataRow != null)
            {
                this.NUM = dataRow.Table.Columns.Contains("NUM") ? dataRow["NUM"].ToString() : string.Empty;
                this.ID = dataRow.Table.Columns.Contains("ID") ? dataRow["ID"].ToString() : string.Empty;
                this.STCD = dataRow.Table.Columns.Contains("STCD") ? dataRow["STCD"].ToString() : string.Empty;
                this.NAME = dataRow.Table.Columns.Contains("NAME") ? dataRow["NAME"].ToString() : string.Empty;
                this.LONGITUDE = dataRow.Table.Columns.Contains("LONGITUDE") ? dataRow["LONGITUDE"].ToString() : string.Empty;
                this.LATITUDE = dataRow.Table.Columns.Contains("LATITUDE") ? dataRow["LATITUDE"].ToString() : string.Empty;
                this.NOTE = dataRow.Table.Columns.Contains("NOTE") ? dataRow["NOTE"].ToString() : string.Empty;
                this.SELECTED = dataRow.Table.Columns.Contains("SELECTED") ? dataRow["SELECTED"].ToString() == "0" ? "FALSE" : "TRUE" : string.Empty;


            }
        }


    }

}