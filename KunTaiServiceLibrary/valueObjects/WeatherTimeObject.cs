using System.Data;
using System.Xml.Linq;

namespace KunTaiServiceLibrary.valueObjects
{
    public class WeatherTimeObject
    {
        /// <summary>
        /// 数据库编号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 更新天气时间的标题。如：下午1点。
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 更新天气的时间。如：13:00:00
        /// </summary>
        public string TIME { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string NOTE { get; set; }


        public WeatherTimeObject(DataRow dataRow)
        {
            if (dataRow != null)
            {
                this.ID = dataRow.Table.Columns.Contains("ID") ? dataRow["ID"].ToString() : string.Empty;
                this.NAME = dataRow.Table.Columns.Contains("NAME") ? dataRow["NAME"].ToString() : string.Empty;
                this.TIME = dataRow.Table.Columns.Contains("TIME") ? dataRow["TIME"].ToString() : string.Empty;
                this.NOTE = dataRow.Table.Columns.Contains("NOTE") ? dataRow["NOTE"].ToString() : string.Empty;
            }
        }

        public WeatherTimeObject(XElement xml)
        {
            if (xml != null)
            {
                xml = xml.Element("DATAS").Element("DATA");
                if (xml != null)
                {
                    this.ID = xml == null || xml.Attribute("ID") == null ? string.Empty : xml.Attribute("ID").Value;
                    this.NAME = xml == null || xml.Attribute("NAME") == null ? string.Empty : xml.Attribute("NAME").Value;
                    this.TIME = xml == null || xml.Attribute("TIME") == null ? string.Empty : xml.Attribute("TIME").Value;
                    this.NOTE = xml == null || xml.Attribute("NOTE") == null ? string.Empty : xml.Attribute("NOTE").Value;
                }
            }

        }
    }
}
