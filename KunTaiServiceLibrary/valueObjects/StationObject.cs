using System.Data;

namespace KunTaiServiceLibrary.valueObjects
{
    /// <summary>
    /// 换热站对象
    /// </summary>
    public class StationObject
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
        /// 所在单位编号
        /// </summary>
        public string OID { get; set; }

        /// <summary>
        /// 换热站名称
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        public string AREA { get; set; }

        /// <summary>
        /// 循环泵功率
        /// </summary>
        public string CYCLEPOWER { get; set; }

        /// <summary>
        /// 循环泵效率
        /// </summary>
        public string CYCLEEFFICIENCY { get; set; }

        /// <summary>
        /// 循环泵流量
        /// </summary>
        public string CYCLEFLOW { get; set; }

        /// <summary>
        /// 补水泵功率
        /// </summary>
        public string WATERPOWER { get; set; }

        /// <summary>
        /// 补水泵效率
        /// </summary>
        public string WATEREFFICIENCY { get; set; }

        /// <summary>
        /// 补水泵流量
        /// </summary>
        public string WATERFLOW { get; set; }

        /// <summary>
        /// 调整温差
        /// </summary>
        public string TEMPERATURE { get; set; }

        /// <summary>
        /// 指令热负荷
        /// </summary>
        public string HEATLOAD { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string NOTE { get; set; }


        public StationObject(DataRow dataRow)
        {
            if (dataRow != null)
            {
                this.NUM = dataRow.Table.Columns.Contains("NUM") ? dataRow["NUM"].ToString() : string.Empty;
                this.ID = dataRow.Table.Columns.Contains("ID") ? dataRow["ID"].ToString() : string.Empty;
                this.OID = dataRow.Table.Columns.Contains("OID") ? dataRow["OID"].ToString() : string.Empty;
                this.NAME = dataRow.Table.Columns.Contains("NAME") ? dataRow["NAME"].ToString() : string.Empty;
                this.AREA = dataRow.Table.Columns.Contains("AREA") ? dataRow["AREA"].ToString() : string.Empty;
                this.CYCLEPOWER = dataRow.Table.Columns.Contains("CYCLEPOWER") ? dataRow["CYCLEPOWER"].ToString() : string.Empty;
                this.CYCLEEFFICIENCY = dataRow.Table.Columns.Contains("CYCLEEFFICIENCY") ? dataRow["CYCLEEFFICIENCY"].ToString() : string.Empty;
                this.CYCLEFLOW = dataRow.Table.Columns.Contains("CYCLEFLOW") ? dataRow["CYCLEFLOW"].ToString() : string.Empty;
                this.WATERPOWER = dataRow.Table.Columns.Contains("WATERPOWER") ? dataRow["WATERPOWER"].ToString() : string.Empty;
                this.WATEREFFICIENCY = dataRow.Table.Columns.Contains("WATEREFFICIENCY") ? dataRow["WATEREFFICIENCY"].ToString() : string.Empty;
                this.WATERFLOW = dataRow.Table.Columns.Contains("WATERFLOW") ? dataRow["WATERFLOW"].ToString() : string.Empty;
                this.TEMPERATURE = dataRow.Table.Columns.Contains("TEMPERATURE") ? dataRow["TEMPERATURE"].ToString() : string.Empty;
                this.HEATLOAD = dataRow.Table.Columns.Contains("HEATLOAD") ? dataRow["HEATLOAD"].ToString() : string.Empty;
                this.NOTE = dataRow.Table.Columns.Contains("NOTE") ? dataRow["NOTE"].ToString() : string.Empty;
            }
        }

    }
}
