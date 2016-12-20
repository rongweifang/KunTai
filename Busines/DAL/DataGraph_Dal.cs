using Busines.IDAO;
using Busines.MODEL;
using Busines.ModelRes;
using Common.DotNetCode;
using Common.DotNetData;
using DataBase.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Busines.DAL
{
  public  class DataGraph_Dal: BaseDAL, DataGraph_IDal
    {
        #region 废弃
        public IList HalfYear<T>()
        {
            IList list = new List<T>();
            StringBuilder strsql = new StringBuilder();
            strsql.Append("SELECT TOP 6 * FROM (SELECT DISTINCT CreateMonth FROM View_WorkBase) V ORDER BY CreateMonth");
            DataTable dt = DataFactory.SqlDataBase().GetDataSetBySQL(strsql).Tables[0];
            if (DataTableHelper.IsExistRows(dt))
            {
                // list = DataTableHelper.DataTableToIList<T>(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0]);
                }
            }
            return list;
        }
        public DataTable GetTaskData(int state)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Table_Name_CH AS name,COUNT(*) AS value  FROM View_WorkBase WHERE [State]=@State GROUP BY CreateMonth, Table_Name_CH");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql,new SqlParam[] {new SqlParam("@State", state) });
        }
        public IList<TaskModel> GetTaskList(int state)
        {
            DataTableToList<TaskModel> sList = new DataTableToList<TaskModel>();
            return sList.ToList(GetTaskData(state));
        }
        #endregion
        public Echarts_Bar_TotalRes GetTaskDataTotal()
        {
            using (var context = WDbContext())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"SELECT PointMonth,CreateMonth,(SELECT COUNT(1) FROM View_TaskGraph WHERE State=1 AND CreateMonth=VT.CreateMonth) AS TaskNum,
(SELECT COUNT(1) FROM View_TaskGraph WHERE State=5 AND PointMonth=VT.PointMonth) AS TasOverkNum,
(SELECT COUNT(1) FROM View_TaskGraph WHERE State NOT IN (1,5) AND PointMonth=VT.PointMonth) AS TaskOtherNum
 FROM View_TaskGraph VT GROUP BY PointMonth,CreateMonth");
                //return DataFactory.SqlDataBase().GetDataTableBySQL(sb);

                var userItems = context.Sql(sb.ToString())
                                      .QueryMany<Echarts_Bar_Total>();

                Echarts_Bar_TotalRes res = new Echarts_Bar_TotalRes();
                res.Datas = userItems;

                return res;
            }
        }
        public Echarts_Pie_MonthRes GetTaskMonthData()
        {
            return GetTaskMonthData(DateTime.Now);
        }
        public Echarts_Pie_MonthRes GetTaskMonthData(DateTime SLDate)
        {
            using (var context = WDbContext())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"SELECT Table_Name_CH AS Typename,COUNT(1) AS TypeCount
FROM View_WorkBase 
WHERE  datediff(month,CreateDate,@SLDate)=0 AND [State] =1
GROUP BY Table_Name_CH  ");
                var userItems = context.Sql(sb.ToString())
                                      .Parameter("SLDate", SLDate)
                                      .QueryMany<Echarts_Pie_Month>();

                Echarts_Pie_MonthRes res = new Echarts_Pie_MonthRes();
                res.Datas = userItems;

                return res;
            }
        }

        public Echarts_Receiveable_MonthRes GetReceivableData(int state)
        {
            using (var context = WDbContext())
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(@"SELECT TOP 6 CAST(YEAR(MWR.AcceptDate) AS varchar) + '年' + CAST(MONTH(MWR.AcceptDate) AS varchar) AS CreathMonth,
CAST(YEAR(MWR.AcceptDate) AS varchar) +
CASE WHEN MONTH(MWR.AcceptDate)<10 THEN '0'+CAST(MONTH(MWR.AcceptDate) AS varchar) ELSE CAST(MONTH(MWR.AcceptDate) AS varchar) END
  AS CreateDateMon,
SUM(CASE WHEN IsFinal=0 THEN Fee ELSE 0 END) AS ReceivableFeeFinal,
SUM(CASE WHEN IsFinal=1 THEN Fee ELSE 0 END) AS ReceivableFee 
FROM Meter_WorkResolve MWR, Meter_WorkResolveFee MWF 
WHERE MWR.ResolveID=MWF.ResolveID AND MWR.YS=1 AND MWR.IsPass=1 AND MWF.State=@State
GROUP BY CAST(YEAR(MWR.AcceptDate) AS varchar) + '年' + CAST(MONTH(MWR.AcceptDate) AS varchar),
CAST(YEAR(MWR.AcceptDate) AS varchar) +
CASE WHEN MONTH(MWR.AcceptDate)<10 THEN '0'+CAST(MONTH(MWR.AcceptDate) AS varchar) ELSE CAST(MONTH(MWR.AcceptDate) AS varchar) END
 ORDER BY CreateDateMon DESC");

                var userItems = context.Sql(sb.ToString())
                    .Parameter("State", state)
                                      .QueryMany<Echarts_Receiveable_Total>();

                Echarts_Receiveable_MonthRes res = new Echarts_Receiveable_MonthRes();
                res.Datas = userItems;

                return res;
            }
        }

        public Echarts_Fee_ValueRes GetDepartmentFeeData()
        {
            using (var context = WDbContext())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"SELECT DISTINCT DepartementID AS value, departmentName AS name,'0' AS fee
FROM Meter_WorkResolveFee MWF LEFT JOIN Meter_WorkResolve MW ON MWF.ResolveID=MW.ResolveID,base_department BD
WHERE MW.IsPass=1 AND MW.YS=1 AND MW.DepartementID=BD.departmentID AND MWF.State=1 AND datediff(month,CreateDate,GETDATE())=0 AND ISNULL(Fee,'0')<>'0'
UNION ALL
SELECT DISTINCT MF.FeeID, MF.FeeItem,'0'
FROM Meter_WorkResolveFee MWF LEFT JOIN Meter_WorkResolve MW ON MWF.ResolveID=MW.ResolveID,Meter_FeeItmes MF 
WHERE MWF.FeeID=MF.FeeID AND  MW.IsPass=1 AND MW.YS=1 AND MWF.State=1 AND datediff(month,CreateDate,GETDATE())=0 AND ISNULL(Fee,'0')<>'0'");

                var _Legends = context.Sql(sb.ToString())
                                      .QueryMany<Echarts_Fee_Value>();

                sb.Clear();
                sb.Append(@"SELECT MW.DepartementID AS value,BD.departmentName AS name,SUM(MWF.Fee) AS fee
  FROM Meter_WorkResolveFee MWF LEFT JOIN Meter_WorkResolve MW ON MWF.ResolveID=MW.ResolveID,base_department BD
  WHERE MW.IsPass=1 AND MW.YS=1 AND MW.DepartementID=BD.departmentID AND MWF.State=1 AND datediff(month,CreateDate,GETDATE())=0 AND ISNULL(Fee,'0')<>'0'
  GROUP BY MW.DepartementID,BD.departmentName ");
                var _Series = context.Sql(sb.ToString())
                                     .QueryMany<Echarts_Fee_Value>();

                sb.Clear();
                sb.Append(@"SELECT MWF.FeeID AS value,MF.FeeItem AS name,SUM(Fee) AS fee FROM Meter_WorkResolveFee MWF LEFT JOIN Meter_WorkResolve MW ON MWF.ResolveID=MW.ResolveID,Meter_FeeItmes MF 
  WHERE MWF.FeeID=MF.FeeID AND  MW.IsPass=1 AND MW.YS=1 AND MWF.State=1 AND datediff(month,CreateDate,GETDATE())=0 AND ISNULL(Fee,'0')<>'0'
  GROUP BY MW.DepartementID, MWF.FeeID,MF.FeeItem ORDER BY MW.DepartementID,MWF.FeeID");
                var _Series1 = context.Sql(sb.ToString())
                                     .QueryMany<Echarts_Fee_Value>();

                Echarts_Fee_ValueRes res = new Echarts_Fee_ValueRes();
                res.Legends = _Legends;
                res.Series = _Series;
                res.Series1 = _Series1;

                return res;
            }
        }

        public Echarts_MonthCheckRes GetMonthChecked()
        {
            using (var context = WDbContext())
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(@"SELECT SUM(CASE WHEN MONTHCHECKSTATE=0 THEN CHARGEBCSS ELSE 0 END) AS MONTHSS,
SUM(CASE WHEN MONTHCHECKSTATE=1 THEN CHARGEBCSS ELSE 0 END) AS MONTHYS,
CAST(YEAR(CHARGEDATETIME) AS VARCHAR(4))+'年'+CAST(MONTH(CHARGEDATETIME) AS VARCHAR(2))+'月' AS MONTHS, 
CAST(YEAR(CHARGEDATETIME) AS VARCHAR(4))+CASE WHEN MONTH(CHARGEDATETIME)<10 THEN '0'+CAST(MONTH(CHARGEDATETIME) AS VARCHAR(2)) ELSE CAST(MONTH(CHARGEDATETIME) AS VARCHAR(2)) END AS YEARMONTH
FROM Meter_Charge 
WHERE ISNULL(CHARGECANCEL,'0')<>1
GROUP BY CAST(YEAR(CHARGEDATETIME) AS VARCHAR(4))+'年'+CAST(MONTH(CHARGEDATETIME) AS VARCHAR(2))+'月',
CAST(YEAR(CHARGEDATETIME) AS VARCHAR(4))+CASE WHEN MONTH(CHARGEDATETIME)<10 THEN '0'+CAST(MONTH(CHARGEDATETIME) AS VARCHAR(2)) ELSE CAST(MONTH(CHARGEDATETIME) AS VARCHAR(2)) END
ORDER BY YEARMONTH DESC");

                var userItems = context.Sql(sb.ToString())
                                      .QueryMany<Echarts_MonthCheck>();

                Echarts_MonthCheckRes res = new Echarts_MonthCheckRes();
                res.Datas = userItems;

                return res;
            }
        }

    }
}
