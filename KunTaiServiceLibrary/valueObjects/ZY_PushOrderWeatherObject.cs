using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KunTaiServiceLibrary.valueObjects
{
    public class ZY_PushOrderWeatherObject
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
        /// 添加日期时间
        /// </summary>
        public string ADDDATETIME { get; set; }

        /// <summary>
        /// 时间点
        /// </summary>
        public string TIMEPOINT { get; set; }

        /// <summary>
        /// 36w
        /// </summary>
        public string W36 { get; set; }

        /// <summary>
        /// 401w/m²/日
        /// </summary>
        public string W401 { get; set; }

        /// <summary>
        /// 小时修正负荷
        /// </summary>
        public string REVISE { get; set; }

        /// <summary>
        /// 区间时间百分比
        /// </summary>
        public string PERCENTAGE { get; set; }



        public ZY_PushOrderWeatherObject()
        {

        }

        public ZY_PushOrderWeatherObject(XElement xml)
        {
            if(xml != null)
            {
                this.NUM = xml.Attribute("NUM") == null ? string.Empty : xml.Attribute("NUM").Value;
                this.ID = xml.Attribute("ID") == null ? string.Empty : xml.Attribute("ID").Value;
                this.ADDDATETIME = xml.Attribute("ADDDATETIME") == null ? string.Empty : xml.Attribute("ADDDATETIME").Value;
                this.TIMEPOINT = xml.Attribute("TIMEPOINT") == null ? string.Empty : xml.Attribute("TIMEPOINT").Value;
                this.W36 = xml.Attribute("W36") == null ? string.Empty : xml.Attribute("W36").Value;
                this.W401 = xml.Attribute("W401") == null ? string.Empty : xml.Attribute("W401").Value;
                this.REVISE = xml.Attribute("REVISE") == null ? string.Empty : xml.Attribute("REVISE").Value;
                this.PERCENTAGE = xml.Attribute("PERCENTAGE") == null ? string.Empty : xml.Attribute("PERCENTAGE").Value;
            }
        }


        public ZY_PushOrderWeatherObject(DataRow dataRow)
        {
            if (dataRow != null)
            {
                this.NUM = dataRow.Table.Columns.Contains("NUM") ? dataRow["NUM"].ToString() : string.Empty;
                this.ID = dataRow.Table.Columns.Contains("ID") ? dataRow["ID"].ToString() : string.Empty;
                this.ADDDATETIME = dataRow.Table.Columns.Contains("ADDDATETIME") ? dataRow["ADDDATETIME"].ToString() : string.Empty;
                this.TIMEPOINT = dataRow.Table.Columns.Contains("TIMEPOINT") ? getDisplayTimePointText(dataRow["TIMEPOINT"].ToString()) : string.Empty;
                this.W36 = dataRow.Table.Columns.Contains("W36") ? Math.Round(Convert.ToDouble(dataRow["W36"]), 2, MidpointRounding.AwayFromZero).ToString() : string.Empty;
                this.W401 = dataRow.Table.Columns.Contains("W401") ? Math.Round(Convert.ToDouble(dataRow["W401"]), 2, MidpointRounding.AwayFromZero).ToString() : string.Empty;
                this.REVISE = dataRow.Table.Columns.Contains("REVISE") ? Math.Round(Convert.ToDouble(dataRow["REVISE"]), 2, MidpointRounding.AwayFromZero).ToString() : string.Empty;
                this.PERCENTAGE = dataRow.Table.Columns.Contains("PERCENTAGE") ? dataRow["PERCENTAGE"].ToString() : string.Empty;
            }

        }

        private string getDisplayTimePointText(string timePoint)
        {
            return string.Format("{0}点", timePoint.Replace("H", ""));
        }


    }
}
