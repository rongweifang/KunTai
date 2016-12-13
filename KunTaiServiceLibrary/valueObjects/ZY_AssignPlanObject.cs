using System.Data;
using System.Xml.Linq;

namespace KunTaiServiceLibrary.valueObjects
{
    public class ZY_AssignPlanObject
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
        /// 角色名称
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 角色备注
        /// </summary>
        public string NOTE { get; set; }

        /// <summary>
        /// ？启？停（只要启动前面的数字，如：3启1停，START_NUMBER=3）
        /// </summary>
        public string START_NUMBER { get; set; }


        public ZY_AssignPlanObject()
        {

        }

        public ZY_AssignPlanObject(XElement xml)
        {
            if (xml != null)
            {
                this.NUM = xml.Attribute("NUM") == null ? string.Empty : xml.Attribute("NUM").Value;
                this.ID = xml.Attribute("ID") == null ? string.Empty : xml.Attribute("ID").Value;
                this.NAME = xml.Attribute("NAME") == null ? string.Empty : xml.Attribute("NAME").Value;
                this.NOTE = xml.Attribute("NOTE") == null ? string.Empty : xml.Attribute("NOTE").Value;

                this.START_NUMBER = this.NAME.Replace("启", "|").Split('|')[0];
            }
        }



        public ZY_AssignPlanObject(DataRow dataRow)
        {
            if (dataRow != null)
            {
                this.NUM = dataRow.Table.Columns.Contains("NUM") ? dataRow["NUM"].ToString() : string.Empty;
                this.ID = dataRow.Table.Columns.Contains("ID") ? dataRow["ID"].ToString() : string.Empty;
                this.NAME = dataRow.Table.Columns.Contains("NAME") ? dataRow["NAME"].ToString() : string.Empty;
                this.NOTE = dataRow.Table.Columns.Contains("NOTE") ? dataRow["NOTE"].ToString() : string.Empty;

                this.START_NUMBER = this.NAME.Replace("启", "|").Split('|')[0];
            }
        }

    }
}
