using System.Data;

namespace KunTaiServiceLibrary.valueObjects
{
    /// <summary>
    /// 角色对象
    /// </summary>
    public class RoleObject
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


        public RoleObject()
        {

        }

        public RoleObject(DataRow dataRow)
        {
            if (dataRow != null)
            {
                this.NUM = dataRow.Table.Columns.Contains("NUM") ? dataRow["NUM"].ToString() : string.Empty;
                this.ID = dataRow.Table.Columns.Contains("ID") ? dataRow["ID"].ToString() : string.Empty;
                this.NAME = dataRow.Table.Columns.Contains("NAME") ? dataRow["NAME"].ToString() : string.Empty;
                this.NOTE = dataRow.Table.Columns.Contains("NOTE") ? dataRow["NOTE"].ToString() : string.Empty;
            }
        }

    }
}
