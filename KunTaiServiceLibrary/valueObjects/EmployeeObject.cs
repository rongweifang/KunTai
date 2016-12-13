using System.Data;

namespace KunTaiServiceLibrary.valueObjects
{
    /// <summary>
    /// 员工对象
    /// </summary>
    public class EmployeeObject
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
        /// 员工名称
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 组织机构编号
        /// </summary>
        public string OID { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        public string ROLEID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string ROLENAME { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string ORGANIZATIONNAME { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string MOBILE { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string EMAIL { get; set; }

        /// <summary>
        /// 登录帐号
        /// </summary>
        public string LOGINNAME { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string LOGINPASSWORD { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string NOTE { get; set; }


        public EmployeeObject()
        {

        }

        public EmployeeObject(DataRow dataRow)
        {
            if (dataRow != null)
            {
                this.NUM = dataRow.Table.Columns.Contains("NUM") ? dataRow["NUM"].ToString() : string.Empty;
                this.ID = dataRow.Table.Columns.Contains("ID") ? dataRow["ID"].ToString() : string.Empty;
                this.NAME = dataRow.Table.Columns.Contains("NAME") ? dataRow["NAME"].ToString() : string.Empty;
                this.OID = dataRow.Table.Columns.Contains("OID") ? dataRow["OID"].ToString() : string.Empty;
                this.ROLEID = dataRow.Table.Columns.Contains("ROLEID") ? dataRow["ROLEID"].ToString() : string.Empty;
                this.ROLENAME = dataRow.Table.Columns.Contains("ROLENAME") ? dataRow["ROLENAME"].ToString() : string.Empty;
                this.ORGANIZATIONNAME = dataRow.Table.Columns.Contains("ORGANIZATIONNAME") ? dataRow["ORGANIZATIONNAME"].ToString() : string.Empty;
                this.MOBILE = dataRow.Table.Columns.Contains("MOBILE") ? dataRow["MOBILE"].ToString() : string.Empty;
                this.EMAIL = dataRow.Table.Columns.Contains("EMAIL") ? dataRow["EMAIL"].ToString() : string.Empty;
                this.LOGINNAME = dataRow.Table.Columns.Contains("LOGINNAME") ? dataRow["LOGINNAME"].ToString() : string.Empty;
                this.LOGINPASSWORD = dataRow.Table.Columns.Contains("LOGINPASSWORD") ? dataRow["LOGINPASSWORD"].ToString() : string.Empty;
                this.NOTE = dataRow.Table.Columns.Contains("NOTE") ? dataRow["NOTE"].ToString() : string.Empty;
            }
        }


    }
}
