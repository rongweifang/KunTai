using System.Data;
using System.Xml.Linq;

namespace KunTaiServiceLibrary.valueObjects
{
    public class ZY_ConsumeEnergyObject
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
        /// 实际耗能量日期
        /// </summary>
        public string ADDDATETIME { get; set; }

        #region 南线

        /// <summary>
        /// 南线 - 供水 - 温度
        /// </summary>
        public string NX_GS_WD { get; set; }

        /// <summary>
        /// 南线 - 回水- 温度
        /// </summary>
        public string NX_HS_WD { get; set; }

        /// <summary>
        /// 南线 - 供水 - 流量
        /// </summary>
        public string NX_GS_LL { get; set; }

        /// <summary>
        /// 南线 - 回水 - 流量
        /// </summary>
        public string NX_HS_LL { get; set; }

        /// <summary>
        /// 南线 - 供水- 压力
        /// </summary>
        public string NX_GS_YL { get; set; }

        /// <summary>
        /// 南线 - 回水 - 压力
        /// </summary>
        public string NX_HS_YL { get; set; }

        /// <summary>
        /// 南线 - 热量
        /// </summary>
        public string NX_RL { get; set; }

        /// <summary>
        /// 南线 - 换热器
        /// </summary>
        public string NX_HRQ { get; set; }

        #endregion

        #region 中线

        /// <summary>
        /// 中线 - 供水 - 温度
        /// </summary>
        public string ZX_GS_WD { get; set; }

        /// <summary>
        /// 中线 - 回水 - 温度
        /// </summary>
        public string ZX_HS_WD { get; set; }

        /// <summary>
        /// 中线 - 供水 - 流量
        /// </summary>
        public string ZX_GS_LL { get; set; }

        /// <summary>
        /// 中线 - 回水 - 流量
        /// </summary>
        public string ZX_HS_LL { get; set; }

        /// <summary>
        /// 中线 - 供水 - 压力
        /// </summary>
        public string ZX_GS_YL { get; set; }

        /// <summary>
        /// 中线 - 回水 - 压力
        /// </summary>
        public string ZX_HS_YL { get; set; }

        /// <summary>
        /// 中心 - 热量
        /// </summary>
        public string ZX_RL { get; set; }

        /// <summary>
        /// 中心 - 换热器
        /// </summary>
        public string ZX_HRQ { get; set; }

        #endregion

        #region 北线

        /// <summary>
        /// 北线 - 供水- 温度
        /// </summary>
        public string BX_GS_WD { get; set; }

        /// <summary>
        /// 北线 - 回水- 温度
        /// </summary>
        public string BX_HS_WD { get; set; }

        /// <summary>
        /// 北线- 供水- 流量
        /// </summary>
        public string BX_GS_LL { get; set; }

        /// <summary>
        /// 北线- 回水 - 流量
        /// </summary>
        public string BX_HS_LL { get; set; }

        /// <summary>
        /// 北线 - 供水 - 压力
        /// </summary>
        public string BX_GS_YL { get; set; }

        /// <summary>
        /// 北线 - 回水 - 压力
        /// </summary>
        public string BX_HS_YL { get; set; }

        /// <summary>
        /// 北线 - 热量
        /// </summary>
        public string BX_RL { get; set; }

        /// <summary>
        /// 北线 - 换热器
        /// </summary>
        public string BX_HRQ { get; set; }

        #endregion

        #region 热网补水量

        /// <summary>
        /// 热网补水量 - 起码- 室内 
        /// </summary>
        public string BSL_QM_SN { get; set; }

        /// <summary>
        /// 热网补水量 - 起码- 室外
        /// </summary>
        public string BSL_QM_SW { get; set; }

        /// <summary>
        /// 热网补水量 - 止码- 室内 
        /// </summary>
        public string BSL_ZM_SN { get; set; }

        /// <summary>
        /// 热网补水量 - 止码- 室外
        /// </summary>
        public string BSL_ZM_SW { get; set; }

        /// <summary>
        /// 热网补水量 - 日合计- 室内
        /// </summary>
        public string BSL_RHJ_SN { get; set; }

        /// <summary>
        /// 热网补水量 - 日合计- 室外 
        /// </summary>
        public string BSL_RHJ_SW { get; set; }

        public string BSL_QM_BS { get; set; }

        public string BSL_ZM_BS { get; set; }

        public string BSL_RHJ_BS { get; set; }

        #endregion

        #region 热网电表用量

        /// <summary>
        /// 热网电表用量- 起码 - 热网循环泵电量
        /// </summary>
        public string DYL_QM_XHB { get; set; }

        /// <summary>
        /// 热网电表用量- 起码 - 南线换热器电量
        /// </summary>
        public string DYL_QM_NXHRQ { get; set; }

        /// <summary>
        /// 热网电表用量- 起码 - 东北线换热器电量
        /// </summary>
        public string DYL_QM_DBXHRQ { get; set; }

        /// <summary>
        /// 热网电表用量- 起码 - 文化新村电量
        /// </summary>
        public string DYL_QM_WHXC { get; set; }

        /// <summary>
        /// 热网电表用量- 止码 - 热网循环泵电量
        /// </summary>
        public string DYL_ZM_XHB { get; set; }

        /// <summary>
        /// 热网电表用量- 止码 - 南线换热器电量
        /// </summary>
        public string DYL_ZM_NXHRQ { get; set; }

        /// <summary>
        /// 热网电表用量- 止码 - 东北线换热器电量
        /// </summary>
        public string DYL_ZM_DBXHRQ { get; set; }

        /// <summary>
        /// 热网电表用量- 止码 - 文化新村电量
        /// </summary>
        public string DYL_ZM_WHXC { get; set; }

        /// <summary>
        /// 热网电表用量- 日合计 - 热网循环泵电量
        /// </summary>
        public string DYL_RHJ_XHB { get; set; }

        /// <summary>
        /// 热网电表用量- 日合计 - 南线换热器电量
        /// </summary>
        public string DYL_RHJ_NXHRQ { get; set; }

        /// <summary>
        /// 热网电表用量- 日合计 - 东北线换热器电量
        /// </summary>
        public string DYL_RHJ_DBXHRQ { get; set; }

        /// <summary>
        /// 热网电表用量- 起码 - 文化新村电量
        /// </summary>
        public string DYL_RHJ_WHXC { get; set; }

        #endregion

        #region 汽动泵用气量

        /// <summary>
        /// 汽动泵用汽量 - 起码 - 东北线用量
        /// </summary>
        public string YQL_QM_DBX { get; set; }

        /// <summary>
        /// 汽动泵用汽量 - 起码 - 南线用量
        /// </summary>
        public string YQL_QM_NX { get; set; }

        /// <summary>
        /// 汽动泵用汽量 - 止码 - 东北线用量
        /// </summary>
        public string YQL_ZM_DBX { get; set; }

        /// <summary>
        /// 汽动泵用汽量 - 止码 - 南线用量
        /// </summary>
        public string YQL_ZM_NX { get; set; }

        /// <summary>
        /// 汽动泵用汽量 - 日合计 - 东北线用量
        /// </summary>
        public string YQL_RHJ_DBX { get; set; }

        /// <summary>
        /// 汽动泵用汽量 - 日合计 - 南线用量
        /// </summary>
        public string YQL_RHJ_NX { get; set; }


        /// <summary>
        /// 热量 = 南线 + 中线 + 北线
        /// </summary>
        public string RL { get; set; }

        /// <summary>
        /// 换热器 = 南线 + 中线 + 北线
        /// </summary>
        public string HRQ { get; set; }

        #endregion


        public ZY_ConsumeEnergyObject()
        {

        }

        public ZY_ConsumeEnergyObject(XElement xml)
        {
            if (xml != null)
            {
                this.NUM = xml.Attribute("NUM") == null ? string.Empty : xml.Attribute("NUM").Value;
                this.ID = xml.Attribute("ID") == null ? string.Empty : xml.Attribute("ID").Value;

                this.ADDDATETIME = xml.Attribute("ADDDATETIME") == null ? string.Empty : xml.Attribute("ADDDATETIME").Value;
                this.NX_GS_WD = xml.Attribute("NX_GS_WD") == null ? string.Empty : xml.Attribute("NX_GS_WD").Value;
                this.NX_HS_WD = xml.Attribute("NX_HS_WD") == null ? string.Empty : xml.Attribute("NX_HS_WD").Value;
                this.NX_GS_LL = xml.Attribute("NX_GS_LL") == null ? string.Empty : xml.Attribute("NX_GS_LL").Value;
                this.NX_HS_LL = xml.Attribute("NX_HS_LL") == null ? string.Empty : xml.Attribute("NX_HS_LL").Value;
                this.NX_GS_YL = xml.Attribute("NX_GS_YL") == null ? string.Empty : xml.Attribute("NX_GS_YL").Value;
                this.NX_HS_YL = xml.Attribute("NX_HS_YL") == null ? string.Empty : xml.Attribute("NX_HS_YL").Value;
                this.NX_RL = xml.Attribute("NX_RL") == null ? string.Empty : xml.Attribute("NX_RL").Value;
                this.NX_HRQ = xml.Attribute("NX_HRQ") == null ? string.Empty : xml.Attribute("NX_HRQ").Value;
                this.ZX_GS_WD = xml.Attribute("ZX_GS_WD") == null ? string.Empty : xml.Attribute("ZX_GS_WD").Value;
                this.ZX_HS_WD = xml.Attribute("ZX_HS_WD") == null ? string.Empty : xml.Attribute("ZX_HS_WD").Value;
                this.ZX_GS_LL = xml.Attribute("ZX_GS_LL") == null ? string.Empty : xml.Attribute("ZX_GS_LL").Value;
                this.ZX_HS_LL = xml.Attribute("ZX_HS_LL") == null ? string.Empty : xml.Attribute("ZX_HS_LL").Value;
                this.ZX_GS_YL = xml.Attribute("ZX_GS_YL") == null ? string.Empty : xml.Attribute("ZX_GS_YL").Value;
                this.ZX_HS_YL = xml.Attribute("ZX_HS_YL") == null ? string.Empty : xml.Attribute("ZX_HS_YL").Value;
                this.ZX_RL = xml.Attribute("ZX_RL") == null ? string.Empty : xml.Attribute("ZX_RL").Value;
                this.ZX_HRQ = xml.Attribute("ZX_HRQ") == null ? string.Empty : xml.Attribute("ZX_HRQ").Value;
                this.BX_GS_WD = xml.Attribute("BX_GS_WD") == null ? string.Empty : xml.Attribute("BX_GS_WD").Value;
                this.BX_HS_WD = xml.Attribute("BX_HS_WD") == null ? string.Empty : xml.Attribute("BX_HS_WD").Value;
                this.BX_GS_YL = xml.Attribute("BX_HS_LL") == null ? string.Empty : xml.Attribute("BX_GS_LL").Value;
                this.BX_HS_YL = xml.Attribute("BX_HS_YL") == null ? string.Empty : xml.Attribute("BX_HS_YL").Value;
                this.BX_RL = xml.Attribute("BX_RL") == null ? string.Empty : xml.Attribute("BX_RL").Value;
                this.BX_HRQ = xml.Attribute("BX_HRQ") == null ? string.Empty : xml.Attribute("BX_HRQ").Value;
                this.BSL_QM_SN = xml.Attribute("BSL_QM_SN") == null ? string.Empty : xml.Attribute("BSL_QM_SN").Value;
                this.BSL_QM_SW = xml.Attribute("BSL_QM_SW") == null ? string.Empty : xml.Attribute("BSL_QM_SW").Value;
                this.BSL_QM_BS = xml.Attribute("BSL_QM_BS") == null ? string.Empty : xml.Attribute("BSL_QM_BS").Value;
                this.BSL_ZM_SN = xml.Attribute("BSL_ZM_SN") == null ? string.Empty : xml.Attribute("BSL_ZM_SN").Value;
                this.BSL_ZM_SW = xml.Attribute("BSL_ZM_SW") == null ? string.Empty : xml.Attribute("BSL_ZM_SW").Value;
                this.BSL_ZM_BS = xml.Attribute("BSL_ZM_BS") == null ? string.Empty : xml.Attribute("BSL_ZM_BS").Value;
                this.BSL_RHJ_SN = xml.Attribute("BSL_RHJ_SN") == null ? string.Empty : xml.Attribute("BSL_RHJ_SN").Value;
                this.BSL_RHJ_SW = xml.Attribute("BSL_RHJ_SW") == null ? string.Empty : xml.Attribute("BSL_RHJ_SW").Value;
                this.BSL_RHJ_BS = xml.Attribute("BSL_RHJ_BS") == null ? string.Empty : xml.Attribute("BSL_RHJ_BS").Value;
                this.DYL_QM_XHB = xml.Attribute("DYL_QM_XHB") == null ? string.Empty : xml.Attribute("DYL_QM_XHB").Value;
                this.DYL_QM_NXHRQ = xml.Attribute("DYL_QM_NXHRQ") == null ? string.Empty : xml.Attribute("DYL_QM_NXHRQ").Value;
                this.DYL_QM_DBXHRQ = xml.Attribute("DYL_QM_DBXHRQ") == null ? string.Empty : xml.Attribute("DYL_QM_DBXHRQ").Value;
                this.DYL_QM_WHXC = xml.Attribute("DYL_QM_WHXC") == null ? string.Empty : xml.Attribute("DYL_QM_WHXC").Value;
                this.DYL_ZM_XHB = xml.Attribute("DYL_ZM_XHB") == null ? string.Empty : xml.Attribute("DYL_ZM_XHB").Value;
                this.DYL_ZM_NXHRQ = xml.Attribute("DYL_ZM_NXHRQ") == null ? string.Empty : xml.Attribute("DYL_ZM_NXHRQ").Value;
                this.DYL_ZM_DBXHRQ = xml.Attribute("DYL_ZM_DBXHRQ") == null ? string.Empty : xml.Attribute("DYL_ZM_DBXHRQ").Value;
                this.DYL_ZM_WHXC = xml.Attribute("DYL_ZM_WHXC") == null ? string.Empty : xml.Attribute("DYL_ZM_WHXC").Value;
                this.DYL_RHJ_XHB = xml.Attribute("DYL_RHJ_XHB") == null ? string.Empty : xml.Attribute("DYL_RHJ_XHB").Value;
                this.DYL_RHJ_NXHRQ = xml.Attribute("DYL_RHJ_NXHRQ") == null ? string.Empty : xml.Attribute("DYL_RHJ_NXHRQ").Value;
                this.DYL_RHJ_DBXHRQ = xml.Attribute("DYL_RHJ_DBXHRQ") == null ? string.Empty : xml.Attribute("DYL_RHJ_DBXHRQ").Value;
                this.DYL_RHJ_WHXC = xml.Attribute("DYL_RHJ_WHXC") == null ? string.Empty : xml.Attribute("DYL_RHJ_WHXC").Value;
                this.YQL_QM_DBX = xml.Attribute("YQL_QM_DBX") == null ? string.Empty : xml.Attribute("YQL_QM_DBX").Value;
                this.YQL_QM_NX = xml.Attribute("YQL_QM_NX") == null ? string.Empty : xml.Attribute("YQL_QM_NX").Value;
                this.YQL_ZM_DBX = xml.Attribute("YQL_ZM_DBX") == null ? string.Empty : xml.Attribute("YQL_ZM_DBX").Value;
                this.YQL_ZM_NX = xml.Attribute("YQL_ZM_NX") == null ? string.Empty : xml.Attribute("YQL_ZM_NX").Value;
                this.YQL_RHJ_DBX = xml.Attribute("YQL_RHJ_DBX") == null ? string.Empty : xml.Attribute("YQL_RHJ_DBX").Value;
                this.YQL_RHJ_NX = xml.Attribute("YQL_RHJ_NX") == null ? string.Empty : xml.Attribute("YQL_RHJ_NX").Value;
            }
        }

        public ZY_ConsumeEnergyObject(DataRow dataRow)
        {
            if (dataRow != null)
            {
                this.NUM = dataRow.Table.Columns.Contains("NUM") ? dataRow["NUM"].ToString() : string.Empty;
                this.ID = dataRow.Table.Columns.Contains("ID") ? dataRow["ID"].ToString() : string.Empty;
                this.ADDDATETIME = dataRow.Table.Columns.Contains("ADDDATETIME") ? dataRow["ADDDATETIME"].ToString() : string.Empty;
                this.NX_GS_WD = dataRow.Table.Columns.Contains("NX_GS_WD") ? dataRow["NX_GS_WD"].ToString() : string.Empty;
                this.NX_HS_WD = dataRow.Table.Columns.Contains("NX_HS_WD") ? dataRow["NX_HS_WD"].ToString() : string.Empty;
                this.NX_GS_LL = dataRow.Table.Columns.Contains("NX_GS_LL") ? dataRow["NX_GS_LL"].ToString() : string.Empty;
                this.NX_HS_LL = dataRow.Table.Columns.Contains("NX_HS_LL") ? dataRow["NX_HS_LL"].ToString() : string.Empty;
                this.NX_GS_YL = dataRow.Table.Columns.Contains("NX_GS_YL") ? dataRow["NX_GS_YL"].ToString() : string.Empty;
                this.NX_HS_YL = dataRow.Table.Columns.Contains("NX_HS_YL") ? dataRow["NX_HS_YL"].ToString() : string.Empty;
                this.NX_RL = dataRow.Table.Columns.Contains("NX_RL") ? dataRow["NX_RL"].ToString() : string.Empty;
                this.NX_HRQ = dataRow.Table.Columns.Contains("NX_HRQ") ? dataRow["NX_HRQ"].ToString() : string.Empty;
                this.ZX_GS_WD = dataRow.Table.Columns.Contains("ZX_GS_WD") ? dataRow["ZX_GS_WD"].ToString() : string.Empty;
                this.ZX_HS_WD = dataRow.Table.Columns.Contains("ZX_HS_WD") ? dataRow["ZX_HS_WD"].ToString() : string.Empty;
                this.ZX_GS_LL = dataRow.Table.Columns.Contains("ZX_GS_LL") ? dataRow["ZX_GS_LL"].ToString() : string.Empty;
                this.ZX_HS_LL = dataRow.Table.Columns.Contains("ZX_HS_LL") ? dataRow["ZX_HS_LL"].ToString() : string.Empty;
                this.ZX_GS_YL = dataRow.Table.Columns.Contains("ZX_GS_YL") ? dataRow["ZX_GS_YL"].ToString() : string.Empty;
                this.ZX_HS_YL = dataRow.Table.Columns.Contains("ZX_HS_YL") ? dataRow["ZX_HS_YL"].ToString() : string.Empty;
                this.ZX_RL = dataRow.Table.Columns.Contains("ZX_RL") ? dataRow["ZX_RL"].ToString() : string.Empty;
                this.ZX_HRQ = dataRow.Table.Columns.Contains("ZX_HRQ") ? dataRow["ZX_HRQ"].ToString() : string.Empty;
                this.BX_GS_WD = dataRow.Table.Columns.Contains("BX_GS_WD") ? dataRow["BX_GS_WD"].ToString() : string.Empty;
                this.BX_HS_WD = dataRow.Table.Columns.Contains("BX_HS_WD") ? dataRow["BX_HS_WD"].ToString() : string.Empty;
                this.BX_GS_LL = dataRow.Table.Columns.Contains("BX_GS_LL") ? dataRow["BX_GS_LL"].ToString() : string.Empty;
                this.BX_HS_LL = dataRow.Table.Columns.Contains("BX_HS_LL") ? dataRow["BX_HS_LL"].ToString() : string.Empty;
                this.BX_GS_YL = dataRow.Table.Columns.Contains("BX_GS_YL") ? dataRow["BX_GS_YL"].ToString() : string.Empty;
                this.BX_HS_YL = dataRow.Table.Columns.Contains("BX_HS_YL") ? dataRow["BX_HS_YL"].ToString() : string.Empty;
                this.BX_RL = dataRow.Table.Columns.Contains("BX_RL") ? dataRow["BX_RL"].ToString() : string.Empty;
                this.BX_HRQ = dataRow.Table.Columns.Contains("BX_HRQ") ? dataRow["BX_HRQ"].ToString() : string.Empty;
                this.BSL_QM_SN = dataRow.Table.Columns.Contains("BSL_QM_SN") ? dataRow["BSL_QM_SN"].ToString() : string.Empty;
                this.BSL_QM_SW = dataRow.Table.Columns.Contains("BSL_QM_SW") ? dataRow["BSL_QM_SW"].ToString() : string.Empty;
                this.BSL_QM_BS = dataRow.Table.Columns.Contains("BSL_QM_BS") ? dataRow["BSL_QM_BS"].ToString() : string.Empty;
                this.BSL_ZM_SN = dataRow.Table.Columns.Contains("BSL_ZM_SN") ? dataRow["BSL_ZM_SN"].ToString() : string.Empty;
                this.BSL_ZM_SW = dataRow.Table.Columns.Contains("BSL_ZM_SW") ? dataRow["BSL_ZM_SW"].ToString() : string.Empty;
                this.BSL_ZM_BS = dataRow.Table.Columns.Contains("BSL_ZM_BS") ? dataRow["BSL_ZM_BS"].ToString() : string.Empty;
                this.BSL_RHJ_SN = dataRow.Table.Columns.Contains("BSL_RHJ_SN") ? dataRow["BSL_RHJ_SN"].ToString() : string.Empty;
                this.BSL_RHJ_SW = dataRow.Table.Columns.Contains("BSL_RHJ_SW") ? dataRow["BSL_RHJ_SW"].ToString() : string.Empty;
                this.BSL_RHJ_BS = dataRow.Table.Columns.Contains("BSL_RHJ_BS") ? dataRow["BSL_RHJ_BS"].ToString() : string.Empty;
                this.DYL_QM_XHB = dataRow.Table.Columns.Contains("DYL_QM_XHB") ? dataRow["DYL_QM_XHB"].ToString() : string.Empty;
                this.DYL_QM_NXHRQ = dataRow.Table.Columns.Contains("DYL_QM_NXHRQ") ? dataRow["DYL_QM_NXHRQ"].ToString() : string.Empty;
                this.DYL_QM_DBXHRQ = dataRow.Table.Columns.Contains("DYL_QM_DBXHRQ") ? dataRow["DYL_QM_DBXHRQ"].ToString() : string.Empty;
                this.DYL_QM_WHXC = dataRow.Table.Columns.Contains("DYL_QM_WHXC") ? dataRow["DYL_QM_WHXC"].ToString() : string.Empty;
                this.DYL_ZM_XHB = dataRow.Table.Columns.Contains("DYL_ZM_XHB") ? dataRow["DYL_ZM_XHB"].ToString() : string.Empty;
                this.DYL_ZM_NXHRQ = dataRow.Table.Columns.Contains("DYL_ZM_NXHRQ") ? dataRow["DYL_ZM_NXHRQ"].ToString() : string.Empty;
                this.DYL_ZM_DBXHRQ = dataRow.Table.Columns.Contains("DYL_ZM_DBXHRQ") ? dataRow["DYL_ZM_DBXHRQ"].ToString() : string.Empty;
                this.DYL_ZM_WHXC = dataRow.Table.Columns.Contains("DYL_ZM_WHXC") ? dataRow["DYL_ZM_WHXC"].ToString() : string.Empty;
                this.DYL_RHJ_XHB = dataRow.Table.Columns.Contains("DYL_RHJ_XHB") ? dataRow["DYL_RHJ_XHB"].ToString() : string.Empty;
                this.DYL_RHJ_NXHRQ = dataRow.Table.Columns.Contains("DYL_RHJ_NXHRQ") ? dataRow["DYL_RHJ_NXHRQ"].ToString() : string.Empty;
                this.DYL_RHJ_DBXHRQ = dataRow.Table.Columns.Contains("DYL_RHJ_DBXHRQ") ? dataRow["DYL_RHJ_DBXHRQ"].ToString() : string.Empty;
                this.DYL_RHJ_WHXC = dataRow.Table.Columns.Contains("DYL_RHJ_WHXC") ? dataRow["DYL_RHJ_WHXC"].ToString() : string.Empty;
                this.YQL_QM_DBX = dataRow.Table.Columns.Contains("YQL_QM_DBX") ? dataRow["YQL_QM_DBX"].ToString() : string.Empty;
                this.YQL_QM_NX = dataRow.Table.Columns.Contains("YQL_QM_NX") ? dataRow["YQL_QM_NX"].ToString() : string.Empty;
                this.YQL_ZM_DBX = dataRow.Table.Columns.Contains("YQL_ZM_DBX") ? dataRow["YQL_ZM_DBX"].ToString() : string.Empty;
                this.YQL_ZM_NX = dataRow.Table.Columns.Contains("YQL_ZM_NX") ? dataRow["YQL_ZM_NX"].ToString() : string.Empty;
                this.YQL_RHJ_DBX = dataRow.Table.Columns.Contains("YQL_RHJ_DBX") ? dataRow["YQL_RHJ_DBX"].ToString() : string.Empty;
                this.YQL_RHJ_NX = dataRow.Table.Columns.Contains("YQL_RHJ_NX") ? dataRow["YQL_RHJ_NX"].ToString() : string.Empty;

                this.RL = dataRow.Table.Columns.Contains("RL") ? dataRow["RL"].ToString() : string.Empty;
                this.HRQ = dataRow.Table.Columns.Contains("HRQ") ? dataRow["HRQ"].ToString() : string.Empty;

            }
        }

    }
}
