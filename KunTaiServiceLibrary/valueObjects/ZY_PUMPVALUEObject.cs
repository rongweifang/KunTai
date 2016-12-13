using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KunTaiServiceLibrary.valueObjects
{
    public class ZY_PUMPVALUEObject
    {
        /// <summary>
        /// 显示编号
        /// </summary>
        public string NUM { get; set; }

        /// <summary>
        /// 数据库编号
        /// </summary>
        public string ID { get; set; }
        /*
        
        /// <summary>
        /// 所属泵房编号
        /// </summary>
        public string PUMPID { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public string ADDDATETIME { get; set; }

        /// <summary>
        /// 二级网供水温度
        /// </summary>
        public string SUPPLYWATHERTEMP2 { get; set; }

        /// <summary>
        /// 二级网供水压力
        /// </summary>
        public string SUPPLYWATHERPRESSURE2 { get; set; }

        /// <summary>
        /// 二级网回水温度
        /// </summary>
        public string BACKWATHERTEMP2 { get; set; }

        /// <summary>
        /// 二级网回水压力
        /// </summary>
        public string BACKWATHERPRESSURE2 { get; set; }

        /// <summary>
        /// 三级网供水温度
        /// </summary>
        public string SUPPLYWATHERTEMP3 { get; set; }

        /// <summary>
        /// 三级网供水压力
        /// </summary>
        public string SUPPLYWATHERPRESSURE3 { get; set; }

        /// <summary>
        /// 三级网回水温度
        /// </summary>
        public string BACKWATHERTEMP3 { get; set; }

        /// <summary>
        /// 三级网回水压力
        /// </summary>
        public string BACKWATHERPRESSURE3 { get; set; }
        //*/

            /// <summary>
            /// 站的自定义编号
            /// </summary>
        public string STCD { get; set; }

        /// <summary>
        /// 泵房的自定义编号
        /// </summary>
        public string T_CODE { get; set; }

        /// <summary>
        /// 站的名称
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 泵房的名称
        /// </summary>
        public string ST_NM { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string TYPE { get; set; }

        /// <summary>
        /// 供热面积
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
        /// 频率
        /// </summary>
        public string FREQUENCY { get; set; }

        /// <summary>
        /// 供水温度
        /// </summary>
        public string SUPPLYWATHERTEMP { get; set; }

        /// <summary>
        /// 回水温度
        /// </summary>
        public string BACKWATHERTEMP { get; set; }


        /// <summary>
        /// 差（供水温度 - 回水温度 = 差）
        /// </summary>
        public string CALC { get; set; }



        public ZY_PUMPVALUEObject(XElement xml)
        {
            if (xml != null)
            {
                this.NUM = xml.Attribute("NUM") == null ? string.Empty : xml.Attribute("NUM").Value;
                this.ID = xml.Attribute("ID") == null ? string.Empty : xml.Attribute("ID").Value;
                this.STCD = xml.Attribute("STCD") == null ? string.Empty : xml.Attribute("STCD").Value;
                this.T_CODE = xml.Attribute("T_CODE") == null ? string.Empty : xml.Attribute("T_CODE").Value;
                this.NAME = xml.Attribute("NAME") == null ? string.Empty : xml.Attribute("NAME").Value;
                this.ST_NM = xml.Attribute("ST_NM") == null ? string.Empty : xml.Attribute("ST_NM").Value;
                this.TYPE = xml.Attribute("TYPE") == null ? string.Empty : xml.Attribute("TYPE").Value;
                this.AREA = xml.Attribute("AREA") == null ? string.Empty : xml.Attribute("AREA").Value;
                this.POWER = xml.Attribute("POWER") == null ? string.Empty : xml.Attribute("POWER").Value;
                this.EFFICIENCY = xml.Attribute("EFFICIENCY") == null ? string.Empty : xml.Attribute("EFFICIENCY").Value;
                this.FLOW = xml.Attribute("FLOW") == null ? string.Empty : xml.Attribute("FLOW").Value;
                this.SPEED = xml.Attribute("SPEED") == null ? string.Empty : xml.Attribute("SPEED").Value;
                this.FREQUENCY = xml.Attribute("FREQUENCY") == null ? string.Empty : xml.Attribute("FREQUENCY").Value;
                this.SUPPLYWATHERTEMP = xml.Attribute("SUPPLYWATHERTEMP") == null ? string.Empty : xml.Attribute("SUPPLYWATHERTEMP").Value;
                this.BACKWATHERTEMP = xml.Attribute("BACKWATHERTEMP") == null ? string.Empty : xml.Attribute("BACKWATHERTEMP").Value;
                this.CALC = xml.Attribute("CALC") == null ? string.Empty : xml.Attribute("CALC").Value;
            }
        }


        public ZY_PUMPVALUEObject(DataRow dataRow)
        {
            if (dataRow != null)
            {
                this.NUM = dataRow.Table.Columns.Contains("NUM") ? dataRow["NUM"].ToString() : string.Empty;
                this.ID = dataRow.Table.Columns.Contains("ID") ? dataRow["ID"].ToString() : string.Empty;
                this.STCD = dataRow.Table.Columns.Contains("STCD") ? dataRow["STCD"].ToString() : string.Empty;
                this.T_CODE = dataRow.Table.Columns.Contains("T_CODE") ? dataRow["T_CODE"].ToString() : string.Empty;
                this.NAME = dataRow.Table.Columns.Contains("NAME") ? dataRow["NAME"].ToString() : string.Empty;
                this.ST_NM = dataRow.Table.Columns.Contains("ST_NM") ? dataRow["ST_NM"].ToString() : string.Empty;
                this.TYPE = dataRow.Table.Columns.Contains("TYPE") ? dataRow["TYPE"].ToString() : string.Empty;
                this.AREA = dataRow.Table.Columns.Contains("AREA") ? dataRow["AREA"].ToString() : string.Empty;
                this.POWER = dataRow.Table.Columns.Contains("POWER") ? dataRow["POWER"].ToString() : string.Empty;
                this.EFFICIENCY = dataRow.Table.Columns.Contains("EFFICIENCY") ? dataRow["EFFICIENCY"].ToString() : string.Empty;
                this.FLOW = dataRow.Table.Columns.Contains("FLOW") ? dataRow["FLOW"].ToString() : string.Empty;
                this.SPEED = dataRow.Table.Columns.Contains("SPEED") ? dataRow["SPEED"].ToString() : string.Empty;
                this.FREQUENCY = dataRow.Table.Columns.Contains("FREQUENCY") ? dataRow["FREQUENCY"].ToString() : string.Empty;
                this.SUPPLYWATHERTEMP = dataRow.Table.Columns.Contains("SUPPLYWATHERTEMP") ? dataRow["SUPPLYWATHERTEMP"].ToString() : string.Empty;
                this.BACKWATHERTEMP = dataRow.Table.Columns.Contains("BACKWATHERTEMP") ? dataRow["BACKWATHERTEMP"].ToString() : string.Empty;
                this.CALC = dataRow.Table.Columns.Contains("CALC") ? dataRow["CALC"].ToString() : string.Empty;
            }
        }
    }
}