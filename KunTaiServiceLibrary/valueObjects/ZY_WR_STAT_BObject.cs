using System.Data;
using System.Xml.Linq;

namespace KunTaiServiceLibrary.valueObjects
{
    public class ZY_WR_STAT_BObject
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
        /// 三级站编号
        /// </summary>
        public string T_CODE { get; set; }

        /// <summary>
        /// 三级站名称
        /// </summary>
        public string ST_NM { get; set; }

        /// <summary>
        /// 三级站名称
        /// </summary>
        public string TYPE { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        public string AREA { get; set; }

        /// <summary>
        /// 功率
        /// </summary>
        public string POWER { get; set; }

        /// <summary>
        /// 效率
        /// </summary>
        public string EFFICIENCY { get; set; }

        /// <summary>
        /// 流量
        /// </summary>
        public string FLOW { get; set; }

        /// <summary>
        /// 流速
        /// </summary>
        public string SPEED { get; set; }

        /// <summary>
        /// 变频
        /// </summary>
        public string FREQUENCY { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string NOTE { get; set; }

        /// <summary>
        /// 供热方式
        /// </summary>
        public string HOTTYPE { get; set; }

        /// <summary>
        /// 铭牌流量
        /// </summary>
        public string NAMEPLATEFLOW { get; set; }

        /// <summary>
        /// 是否参与运行指令的计算
        /// </summary>
        public string ISCALCULATE { get; set; }



        public ZY_WR_STAT_BObject(XElement xml)
        {
            if (xml != null)
            {
                this.NUM = xml.Element("NUM") == null ? string.Empty : xml.Element("NUM").Value;
                this.ID = xml.Element("ID") == null ? string.Empty : xml.Element("ID").Value;
                this.STCD = xml.Element("STCD") == null ? string.Empty : xml.Element("STCD").Value;
                this.T_CODE = xml.Element("T_CODE") == null ? string.Empty : xml.Element("T_CODE").Value;
                this.ST_NM = xml.Element("ST_NM") == null ? string.Empty : xml.Element("ST_NM").Value;
                this.TYPE = xml.Element("TYPE") == null ? string.Empty :
                    xml.Element("TYPE").Value == "NULL" ? "NULL" : string.Format("'{0}'", xml.Element("TYPE").Value);
                this.AREA = xml.Element("AREA") == null ? string.Empty : xml.Element("AREA").Value;
                this.POWER = xml.Element("POWER") == null ? string.Empty : xml.Element("POWER").Value;
                this.EFFICIENCY = xml.Element("EFFICIENCY") == null ? string.Empty : xml.Element("EFFICIENCY").Value;
                this.FLOW = xml.Element("FLOW") == null ? string.Empty : xml.Element("FLOW").Value;
                this.SPEED = xml.Element("SPEED") == null ? string.Empty : xml.Element("SPEED").Value;
                this.FREQUENCY = xml.Element("FREQUENCY") == null ? string.Empty : xml.Element("FREQUENCY").Value;
                this.NOTE = xml.Element("NOTE") == null ? string.Empty : xml.Element("NOTE").Value;
                this.HOTTYPE = xml.Element("HOTTYPE") == null ? string.Empty :
                    xml.Element("HOTTYPE").Value == "NULL" ? "NULL" : string.Format("'{0}'", xml.Element("HOTTYPE").Value);
                this.NAMEPLATEFLOW = xml.Element("NAMEPLATEFLOW") == null ? string.Empty : xml.Element("NAMEPLATEFLOW").Value;
                this.ISCALCULATE = xml.Element("ISCALCULATE") == null ? string.Empty : xml.Element("ISCALCULATE").Value;
            }
        }


        public ZY_WR_STAT_BObject(DataRow dataRow)
        {
            if (dataRow != null)
            {
                this.NUM = dataRow.Table.Columns.Contains("NUM") ? dataRow["NUM"].ToString() : string.Empty;
                this.ID = dataRow.Table.Columns.Contains("ID") ? dataRow["ID"].ToString() : string.Empty;
                this.STCD = dataRow.Table.Columns.Contains("STCD") ? dataRow["STCD"].ToString() : string.Empty;
                this.T_CODE = dataRow.Table.Columns.Contains("T_CODE") ? dataRow["T_CODE"].ToString() : string.Empty;
                this.ST_NM = dataRow.Table.Columns.Contains("ST_NM") ? dataRow["ST_NM"].ToString() : string.Empty;
                this.TYPE = dataRow.Table.Columns.Contains("TYPE") ? dataRow["TYPE"].ToString() : string.Empty;
                this.AREA = dataRow.Table.Columns.Contains("AREA") ? dataRow["AREA"].ToString() : string.Empty;
                this.POWER = dataRow.Table.Columns.Contains("POWER") ? dataRow["POWER"].ToString() : string.Empty;
                this.EFFICIENCY = dataRow.Table.Columns.Contains("EFFICIENCY") ? dataRow["EFFICIENCY"].ToString() : string.Empty;
                this.FLOW = dataRow.Table.Columns.Contains("FLOW") ? dataRow["FLOW"].ToString() : string.Empty;
                this.SPEED = dataRow.Table.Columns.Contains("SPEED") ? dataRow["SPEED"].ToString() : string.Empty;
                this.FREQUENCY = dataRow.Table.Columns.Contains("FREQUENCY") ? dataRow["FREQUENCY"].ToString() : string.Empty;
                this.NOTE = dataRow.Table.Columns.Contains("NOTE") ? dataRow["NOTE"].ToString() : string.Empty;
                this.HOTTYPE = dataRow.Table.Columns.Contains("HOTTYPE") ? dataRow["HOTTYPE"].ToString() : string.Empty;
                this.NAMEPLATEFLOW = dataRow.Table.Columns.Contains("NAMEPLATEFLOW") ? dataRow["NAMEPLATEFLOW"].ToString() : string.Empty;
                this.ISCALCULATE = dataRow.Table.Columns.Contains("ISCALCULATE") ? dataRow["ISCALCULATE"].ToString() : string.Empty;
            }
        }


    }

}