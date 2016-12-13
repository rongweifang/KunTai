using System.Data;
using System.Xml.Linq;

namespace KunTaiServiceLibrary.valueObjects
{
    /// <summary>
    /// 组织机构对象
    /// </summary>
    public class OrganizationObject
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
        /// 组织机构名称
        /// </summary>
        public string NAME { get; set; }


        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string EMPLOYEE { get; set; }


        /// <summary>
        /// 联系电话
        /// </summary>
        public string PHONE { get; set; }


        /// <summary>
        /// 所在地址
        /// </summary>
        public string ADDRESS { get; set; }


        /// <summary>
        /// 城市编码。用于获取温度信息。
        /// </summary>
        public string CITYID { get; set; }


        /// <summary>
        /// 显示顺序
        /// </summary>
        //public string SHOWID { get; set; }


        /// <summary>
        /// 说明
        /// </summary>
        public string NOTE { get; set; }



        public OrganizationObject()
        {

        }

        public OrganizationObject(XElement xml)
        {
            if (xml != null)
            {
                this.NUM = xml.Attribute("NUM") == null ? string.Empty : xml.Attribute("NUM").Value;
                this.ID = xml.Attribute("ID") == null ? string.Empty : xml.Attribute("ID").Value;
                this.NAME = xml.Attribute("NAME") == null ? string.Empty : xml.Attribute("NAME").Value;
                this.EMPLOYEE = xml.Attribute("EMPLOYEE") == null ? string.Empty : xml.Attribute("EMPLOYEE").Value;
                this.PHONE = xml.Attribute("PHONE") == null ? string.Empty : xml.Attribute("PHONE").Value;
                this.ADDRESS = xml.Attribute("ADDRESS") == null ? string.Empty : xml.Attribute("ADDRESS").Value;
                this.CITYID = xml.Attribute("CITYID") == null ? string.Empty : xml.Attribute("CITYID").Value;
                //this.SHOWID = xml.Attribute("SHOWID") == null ? string.Empty : xml.Attribute("SHOWID").Value;
                this.NOTE = xml.Attribute("NOTE") == null ? string.Empty : xml.Attribute("NOTE").Value;
            }

        }

        public OrganizationObject(DataRow dataRow)
        {
            if (dataRow != null)
            {
                this.NUM = dataRow.Table.Columns.Contains("NUM") ? dataRow["NUM"].ToString() : string.Empty;
                this.ID = dataRow.Table.Columns.Contains("ID") ? dataRow["ID"].ToString() : string.Empty;
                this.NAME = dataRow.Table.Columns.Contains("NAME") ? dataRow["NAME"].ToString() : string.Empty;
                this.EMPLOYEE = dataRow.Table.Columns.Contains("EMPLOYEE") ? dataRow["EMPLOYEE"].ToString() : string.Empty;
                this.PHONE = dataRow.Table.Columns.Contains("PHONE") ? dataRow["PHONE"].ToString() : string.Empty;
                this.ADDRESS = dataRow.Table.Columns.Contains("ADDRESS") ? dataRow["ADDRESS"].ToString() : string.Empty;
                this.CITYID = dataRow.Table.Columns.Contains("CITYID") ? dataRow["CITYID"].ToString() : string.Empty;
                //this.SHOWID = dataRow.Table.Columns.Contains("SHOWID") ? dataRow["SHOWID"].ToString() : string.Empty;
                this.NOTE = dataRow.Table.Columns.Contains("NOTE") ? dataRow["NOTE"].ToString() : string.Empty;
            }
        }


    }
}
