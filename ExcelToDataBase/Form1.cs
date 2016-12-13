using KunTaiServiceLibrary;
using KunTaiServiceLibrary.valueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelToDataBase
{
    public partial class Form1 : Form
    {
        private DirectoryInfo Dirhuanrezhan = new DirectoryInfo(@"D:\ExcelInput\huanrezhan\");
        private DirectoryInfo Dirguolu = new DirectoryInfo(@"D:\ExcelInput\guolu\");

        private const string push_order_insert = "INSERT INTO [KT_PUSHORDER] ([ID], [RUNDATE], [MAXVALUE], [MINVALUE], [EXPORTTYPE], [FILENAME], [FILEURL], [NOTE],[ADDDATETIME]) VALUES ('{0}', '{1}', {2}, {3}, '{4}', '{5}', '{6}', '{7}','{8}')";
        private const string send_push_order_insert = "INSERT INTO [PUSHORDER] ([ID],[PUSHID], [RUNDATETIME], [FILEURL], [LOCALFILENAME], [NOTE],[PUSHDATETIME]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}')";
        private const string receive_push_order_insert = "INSERT INTO [PUSHORDERRECEIVE] ([POID], [RECEIVEID]) VALUES ('{0}', '{1}')";
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            GetAll(Dirhuanrezhan, true);

            // string fileUrl = @"D:\ExcelInput\huanrezhan\坤泰热源15-16年度1月份换热站日运行水、电耗量统计表.xlsx";
            //DataSet dataSetExcel = null;
            //ExcelToDataSet.loadExcelToDataSet(fileUrl, out dataSetExcel);
            //dataGridView1.DataSource = dataSetExcel;
        }

        private void GetAll(DirectoryInfo dir, bool IsCommand)
        {
            FileInfo[] allFile = dir.GetFiles();
            foreach (FileInfo fi in allFile)
            {
                DataSet dataSetExcel = null;
                ExcelToDataSet.loadExcelToDataSet(fi.FullName, out dataSetExcel);
                if (dataSetExcel != null)
                {
                    if (IsCommand)
                    {
                        ErgodicTable(dataSetExcel, fi.FullName);
                    }
                    else
                    {
                        GulouTable(dataSetExcel, fi.FullName);
                    }
                }


            }
        }

        private void GulouTable(DataSet DS, string fullName)
        {
            if (DS.Tables.Count > 0)
            {
                DataTable dt = DS.Tables[0];
                if (dt != null)
                {
                    string EmployeeID = "C1359ACA-10DF-4338-8D62-66F22724B647";
                    string pushID = "C149277D-0964-4304-B0A3-BB940C7E7415";

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("BEGIN");
                   
                    for (int i = 3; i < 34; i++)
                    {
                        //2日12月15年，坤泰热源指令量区间分配表.xlsx
                  

                        DataRow dr = dt.Rows[i];
                        DateTime dtDate;
                        if (DateTime.TryParse(dr[0].ToString(), out dtDate))
                        {
                            string GUID = Guid.NewGuid().ToString().ToUpper();
                            string runDateTime = dtDate.ToString("d日MM月yy年");
                            
                            string titleName = string.Format("{0}，坤泰热源指令量区间分配表.xlsx", runDateTime);

                            sb.Append("INSERT INTO KT_PUSHORDER (RUNDATE,RunDay,MAXVALUE,MINVALUE,FILENAME,FILEURL,CommandTime,  ActulTime, Command_Coal, Actul_Coal, Command_Water, Actul_Water, Command_Ele,  Actul_Ele, Command_Alkali, Actul_Alkali, Command_Salt, Actul_Salt, Command_Diesel, Actul_Diesel,  WatchMan, WatchManID, CreateUser) VALUES ");
                            sb.Append(" (");
                            sb.AppendFormat("'{0}',", runDateTime);//RUNDATE运行日期
                            sb.AppendFormat("'{0}',", dtDate);//RUNDATE运行日期
                            sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[2].ToString())?0m:decimal.Parse(dr[2].ToString()));//MAXVALUE最高温度
                            sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[3].ToString()) ? 0m : decimal.Parse(dr[3].ToString()));//MINVALUE最低温度
                            sb.AppendFormat("'{0}',", titleName);//FILENAME文件名字
                            sb.AppendFormat("'{0}',", titleName);//FILEURL文件路径
                            sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[5].ToString()) ? 0m : decimal.Parse(dr[5].ToString()));//CommandTime-指令时间
                            sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[6].ToString()) ? 0m : decimal.Parse(dr[6].ToString()));//ActulTime-实际运行时间
                            //sb.AppendFormat("'{0}',", "");//Time_Ratio-分区比例 0.1:0.3:03:03
                            sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[7].ToString()) ? 0m : decimal.Parse(dr[7].ToString()));//Command_Coal-指令煤
                            sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[8].ToString()) ? 0m : decimal.Parse(dr[8].ToString()));//Actul_Coal-实际煤
                            sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[9].ToString()) ? 0m : decimal.Parse(dr[9].ToString()));//Command_Water-指令水
                            sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[10].ToString()) ? 0m : decimal.Parse(dr[10].ToString()));//Actul_Water-实际水
                            sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[11].ToString()) ? 0m : decimal.Parse(dr[11].ToString()));//Command_Ele-指令电
                            sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[12].ToString()) ? 0m : decimal.Parse(dr[12].ToString()));//Actul_Ele-实际电
                            sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[13].ToString()) ? 0m : decimal.Parse(dr[13].ToString()));//Command_Alkali-指令碱
                            sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[14].ToString()) ? 0m : decimal.Parse(dr[14].ToString()));//Actul_Alkali-实际碱
                            sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[15].ToString()) ? 0m : decimal.Parse(dr[15].ToString()));//Command_Salt-指令盐
                            sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[16].ToString()) ? 0m : decimal.Parse(dr[16].ToString()));//Actul_Salt-实际盐
                            if (dr.ItemArray.Length>17)
                            {
                                sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[17].ToString()) ? 0m : decimal.Parse(dr[17].ToString()));//Command_Diesel, 指令油
                                sb.AppendFormat("{0},", string.IsNullOrEmpty(dr[18].ToString()) ? 0m : decimal.Parse(dr[18].ToString()));//Actul_Diesel, 实际油
                            }
                            else
                            {
                                sb.AppendFormat("{0},", 0);//Command_Diesel, 指令油
                                sb.AppendFormat("{0},",0);//Actul_Diesel, 实际油
                            }
                           
                            sb.AppendFormat("'{0}',", dr[1].ToString());// WatchMan, 负责人
                            sb.AppendFormat("'{0}',", "");//WatchManID, 负责人ID
                            sb.AppendFormat("'{0}'", EmployeeID);//CreateUser 创建人
                            sb.Append(" )\n");

                            //"INSERT INTO [PUSHORDER] ([ID],[PUSHID], [RUNDATETIME], [FILEURL], [LOCALFILENAME], [NOTE],[PUSHDATETIME]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}')";
                            sb.Append("INSERT INTO [PUSHORDER] ([ID],[PUSHID], [RUNDATETIME], [FILEURL], [LOCALFILENAME], [PUSHDATETIME]) VALUES ");
                            sb.Append(" (");
                            sb.AppendFormat("'{0}',", GUID);
                            sb.AppendFormat("'{0}',", pushID);
                            sb.AppendFormat("'{0}',", dtDate);
                            sb.AppendFormat("'{0}',", titleName);
                            sb.AppendFormat("'{0}',", titleName);
                            sb.AppendFormat("'{0}'", dtDate);
                            sb.Append(" )\n");
                            //"INSERT INTO [PUSHORDERRECEIVE] ([POID], [RECEIVEID]) VALUES ('{0}', '{1}')"
                            sb.Append("INSERT INTO [PUSHORDERRECEIVE] ([POID], [RECEIVEID]) VALUES ");
                            sb.Append(" (");
                            sb.AppendFormat("'{0}',", GUID);
                            sb.AppendFormat("'{0}'", EmployeeID);
                            sb.Append(" )\n");
                        }
                        else
                        {
                            break;
                        }

                    }
                    sb.AppendLine("END");
                    new DataAccessHandler().executeNonQueryResult(sb.ToString(),null);
                }
            }
        }

        private void ErgodicTable(DataSet DS, string filename)
        {
            if (DS.Tables.Count > 0)
            {
                for (int i = 0; i < DS.Tables.Count; i++)
                {
                    DataTable dt = DS.Tables[i];
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 5)
                        {
                            //dt.Rows[1][0]或[1][1]为日期
                            //dt.Rows[1][3]或[1][4]为最高温度
                            //dt.Rows[1][5]或[1][6]为最低温度

                            //如果dt.Rows[1][0]=="日  期"
                            if (dt.Rows[1][0].Equals("日  期"))
                            {
                                string strDate = dt.Rows[1][1].ToString();
                                DateTime dtDate;
                                if (DateTime.TryParse(strDate, out dtDate))
                                {
                                    InxertKT_RUNCOMMAND(dt, 1, dtDate, dt.Rows[1][6].ToString(), dt.Rows[1][8].ToString(), dt.Rows[1][4].ToString());
                                    // InsertKT_TEMP(dtDate, dt.Rows[1][6].ToString(), dt.Rows[1][8].ToString(), filename);

                                }
                            }
                            else
                            {
                                string strDate = dt.Rows[1][0].ToString();
                                DateTime dtDate;
                                if (DateTime.TryParse(strDate, out dtDate))
                                {
                                    InxertKT_RUNCOMMAND(dt, 2, dtDate, dt.Rows[1][3].ToString(), dt.Rows[1][5].ToString(), dt.Rows[1][1].ToString());
                                    // InsertKT_TEMP(dtDate, dt.Rows[1][3].ToString(), dt.Rows[1][5].ToString(), filename);

                                }
                            }
                        }
                    }
                }
            }
        }


        #region 导入温度
        private void InsertKT_TEMP(DateTime v1, string v2, string v3, string filename)
        {
            //2日12月15年，坤泰热源指令量区间分配表.xlsx
            string GUID = Guid.NewGuid().ToString().ToUpper();
            string runDateTime = v1.AddDays(1).ToString("d日MM月yy年");
            string titleName = string.Format("{0}，坤泰热源指令量区间分配表.xlsx", runDateTime);

            string EmployeeID = "C1359ACA-10DF-4338-8D62-66F22724B647";
            string pushID = "C149277D-0964-4304-B0A3-BB940C7E7415";

            new DataAccessHandler().executeNonQueryResult(
                    string.Format(push_order_insert,
                    Guid.NewGuid().ToString().ToUpper(),
                    v1.AddDays(1).ToString("yyyy年MM月dd日"),
                   v2,
                   v3,
                    "",
                    titleName,
                    titleName,
                    "",
                    v1
                    ), null);

            new DataAccessHandler().executeNonQueryResult(
                    string.Format(send_push_order_insert,
                     GUID,
                    pushID,
                    v1,
                    //data.RUNDATE,
                    titleName,
                    titleName,
                    "",
                    v1
                    ), null);

            //谁来接收该文件？
            new DataAccessHandler().executeNonQueryResult(
               string.Format(receive_push_order_insert, GUID, EmployeeID),
               null);
        }
        #endregion

        #region 写入指令
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="strDate">运行时间</param>
        /// <param name="v1">最高温度</param>
        /// <param name="v2">最低温度</param>
        /// <param name="v3">值班负责人</param>
        private void InxertKT_RUNCOMMAND(DataTable dt, int op, DateTime strDate, string v1, string v2, string v3)
        {
            string EmployeeID = "C1359ACA-10DF-4338-8D62-66F22724B647";
            for (int i = 3; i < 23; i++)
            {
                DataRow dr = dt.Rows[i];
                KT_RunCommandObject ob = new KT_RunCommandObject();
                //ID,PUSHID,RUNDATE,STATIONID,STATEIONNAME,EMPLOYEEID,EMPLOYEENAME,ADDDATETIME,MAXVALUE,MINVALUE,ZL_TIME,SJ_TIME,ZL_ELE,SJ_ELE,ZL_WATER,SJ_WATER)
                ob.ID = Guid.NewGuid();
                ob.PUSHID = Guid.NewGuid();
                ob.RUNDATE = strDate;
                ob.STATIONID = GetStationID(dr[0].ToString());
                ob.STATEIONNAME = dr[0].ToString();
                ob.EMPLOYEEID = new Guid(EmployeeID);
                ob.EMPLOYEENAME = v3;
                ob.ADDDATETIME = strDate;
                ob.MAXVALUE =decimal.Parse(v1);
                ob.MINVALUE =decimal.Parse(v2);
                if (op == 1)
                {
                    ob.ZL_TIME =string.IsNullOrEmpty(dr[1].ToString())?0m:decimal.Parse(dr[1].ToString());
                    ob.SJ_TIME =string.IsNullOrEmpty(dr[2].ToString()) ? 0m : decimal.Parse(dr[2].ToString());
                    ob.ZL_ELE =  string.IsNullOrEmpty(dr[3].ToString()) ? 0m : decimal.Parse(dr[3].ToString());
                    ob.SJ_ELE =  string.IsNullOrEmpty(dr[4].ToString()) ? 0m : decimal.Parse(dr[4].ToString());
                    ob.ZL_WATER =  string.IsNullOrEmpty(dr[7].ToString()) ? 0m : decimal.Parse(dr[7].ToString());
                    ob.SJ_WATER = string.IsNullOrEmpty(dr[8].ToString()) ? 0m : decimal.Parse(dr[8].ToString());
                }
                else if (op == 2)
                {
                    ob.ZL_TIME = 0m;
                    ob.SJ_TIME = 0m;
                    ob.ZL_ELE = string.IsNullOrEmpty(dr[1].ToString()) ? 0m : decimal.Parse(dr[1].ToString());
                    ob.SJ_ELE = string.IsNullOrEmpty(dr[2].ToString()) ? 0m : decimal.Parse(dr[2].ToString());
                    ob.ZL_WATER = string.IsNullOrEmpty(dr[5].ToString()) ? 0m : decimal.Parse(dr[5].ToString());
                    ob.SJ_WATER = string.IsNullOrEmpty(dr[6].ToString()) ? 0m : decimal.Parse(dr[6].ToString());
                }

                new KT_RunCommand().Import(ob);
            }
        }

        private Guid GetStationID(string v)
        {
            return new KT_RunCommand().GetStationID(v);
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            GetAll(Dirguolu, false);
        }
    }
}
