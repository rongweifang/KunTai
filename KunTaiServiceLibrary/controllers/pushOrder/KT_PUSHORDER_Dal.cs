using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KunTaiServiceLibrary
{
    public partial class KT_PUSHORDER_Dal
    {
        public KT_PUSHORDER_Dal()
    { }
    #region  BasicMethod

    /// <summary>
    /// 是否存在该记录
    /// </summary>
    public bool Exists(Guid ID)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("select count(1) from KT_PUSHORDER");
        strSql.Append(" where ID=@ID ");
        SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)          };
        parameters[0].Value = ID;

        return DbHelperSQL.Exists(strSql.ToString(), parameters);
    }


    /// <summary>
    /// 增加一条数据
    /// </summary>
    public static string Add(KT_PUSHORDER_Model model)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("insert into KT_PUSHORDER(");
        strSql.Append("ID,RUNDATE,RunDay,ADDDATETIME,MAXVALUE,MINVALUE,EXPORTTYPE,FILENAME,FILEURL,CommandTime,Time_Ratio,Command_Coal,Command_Water,Command_Ele,Command_Alkali,Command_Salt,Command_Diesel,CreateUser)");
        strSql.Append(" values (");
        strSql.Append("@ID,@RUNDATE,@RunDay,@ADDDATETIME,@MAXVALUE,@MINVALUE,@EXPORTTYPE,@FILENAME,@FILEURL,@CommandTime,@Time_Ratio,@Command_Coal,@Command_Water,@Command_Ele,@Command_Alkali,@Command_Salt,@Command_Diesel,@CreateUser)");
        SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16),
                    new SqlParameter("@RUNDATE", SqlDbType.NVarChar,50),
                    new SqlParameter("@RunDay", SqlDbType.Date,3),
                    new SqlParameter("@ADDDATETIME", SqlDbType.DateTime),
                    new SqlParameter("@MAXVALUE", SqlDbType.Float,8),
                    new SqlParameter("@MINVALUE", SqlDbType.Float,8),
                    new SqlParameter("@EXPORTTYPE", SqlDbType.NVarChar,200),
                    new SqlParameter("@FILENAME", SqlDbType.NVarChar,200),
                    new SqlParameter("@FILEURL", SqlDbType.NVarChar,200),
                    //new SqlParameter("@NOTE", SqlDbType.NVarChar,200),
                    new SqlParameter("@CommandTime", SqlDbType.Decimal,9),
                   // new SqlParameter("@ActulTime", SqlDbType.Decimal,9),
                    new SqlParameter("@Time_Ratio", SqlDbType.NVarChar,50),
                    new SqlParameter("@Command_Coal", SqlDbType.Decimal,9),
                   // new SqlParameter("@Actul_Coal", SqlDbType.Decimal,9),
                    new SqlParameter("@Command_Water", SqlDbType.Decimal,9),
                   // new SqlParameter("@Actul_Water", SqlDbType.Decimal,9),
                    new SqlParameter("@Command_Ele", SqlDbType.Decimal,9),
                   // new SqlParameter("@Actul_Ele", SqlDbType.Decimal,9),
                    new SqlParameter("@Command_Alkali", SqlDbType.Decimal,9),
                   // new SqlParameter("@Actul_Alkali", SqlDbType.Decimal,9),
                    new SqlParameter("@Command_Salt", SqlDbType.Decimal,9),
                   // new SqlParameter("@Actul_Salt", SqlDbType.Decimal,9),
                    new SqlParameter("@Command_Diesel", SqlDbType.Decimal,9),
                    //new SqlParameter("@Actul_Diesel", SqlDbType.Decimal,9),
                   // new SqlParameter("@WatchMan", SqlDbType.NVarChar,50),
                   // new SqlParameter("@WatchManID", SqlDbType.NVarChar,50),
                    new SqlParameter("@CreateUser", SqlDbType.NVarChar,50)};
        parameters[0].Value = model.ID;
        parameters[1].Value = model.RUNDATE;
        parameters[2].Value = model.RunDay;
        parameters[3].Value = model.ADDDATETIME;
        parameters[4].Value = model.MAXVALUE;
        parameters[5].Value = model.MINVALUE;
        parameters[6].Value = model.EXPORTTYPE;
        parameters[7].Value = model.FILENAME;
        parameters[8].Value = model.FILEURL;
        parameters[9].Value = model.CommandTime;
        parameters[10].Value = model.Time_Ratio;
        parameters[11].Value = model.Command_Coal;
        parameters[12].Value = model.Command_Water;
        parameters[13].Value = model.Command_Ele;
        parameters[14].Value = model.Command_Alkali;
        parameters[15].Value = model.Command_Salt;
        parameters[16].Value = model.Command_Diesel;
        parameters[17].Value = model.CreateUser;

        return new DataAccessHandler().executeNonQueryResult(strSql.ToString(), parameters);
       
    }
    /// <summary>
    /// 更新一条数据
    /// </summary>
    public bool Update(KT_PUSHORDER_Model model)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("update KT_PUSHORDER set ");
        strSql.Append("RUNDATE=@RUNDATE,");
        strSql.Append("RunDay=@RunDay,");
        strSql.Append("ADDDATETIME=@ADDDATETIME,");
        strSql.Append("MAXVALUE=@MAXVALUE,");
        strSql.Append("MINVALUE=@MINVALUE,");
        strSql.Append("EXPORTTYPE=@EXPORTTYPE,");
        strSql.Append("FILENAME=@FILENAME,");
        strSql.Append("FILEURL=@FILEURL,");
        strSql.Append("NOTE=@NOTE,");
        strSql.Append("CommandTime=@CommandTime,");
        strSql.Append("ActulTime=@ActulTime,");
        strSql.Append("Time_Ratio=@Time_Ratio,");
        strSql.Append("Command_Coal=@Command_Coal,");
        strSql.Append("Actul_Coal=@Actul_Coal,");
        strSql.Append("Command_Water=@Command_Water,");
        strSql.Append("Actul_Water=@Actul_Water,");
        strSql.Append("Command_Ele=@Command_Ele,");
        strSql.Append("Actul_Ele=@Actul_Ele,");
        strSql.Append("Command_Alkali=@Command_Alkali,");
        strSql.Append("Actul_Alkali=@Actul_Alkali,");
        strSql.Append("Command_Salt=@Command_Salt,");
        strSql.Append("Actul_Salt=@Actul_Salt,");
        strSql.Append("Command_Diesel=@Command_Diesel,");
        strSql.Append("Actul_Diesel=@Actul_Diesel,");
        strSql.Append("WatchMan=@WatchMan,");
        strSql.Append("WatchManID=@WatchManID,");
        strSql.Append("CreateUser=@CreateUser");
        strSql.Append(" where ID=@ID ");
        SqlParameter[] parameters = {
                    new SqlParameter("@RUNDATE", SqlDbType.NVarChar,50),
                    new SqlParameter("@RunDay", SqlDbType.Date,3),
                    new SqlParameter("@ADDDATETIME", SqlDbType.DateTime),
                    new SqlParameter("@MAXVALUE", SqlDbType.Float,8),
                    new SqlParameter("@MINVALUE", SqlDbType.Float,8),
                    new SqlParameter("@EXPORTTYPE", SqlDbType.NVarChar,200),
                    new SqlParameter("@FILENAME", SqlDbType.NVarChar,200),
                    new SqlParameter("@FILEURL", SqlDbType.NVarChar,200),
                    new SqlParameter("@NOTE", SqlDbType.NVarChar,200),
                    new SqlParameter("@CommandTime", SqlDbType.Decimal,9),
                    new SqlParameter("@ActulTime", SqlDbType.Decimal,9),
                    new SqlParameter("@Time_Ratio", SqlDbType.NVarChar,50),
                    new SqlParameter("@Command_Coal", SqlDbType.Decimal,9),
                    new SqlParameter("@Actul_Coal", SqlDbType.Decimal,9),
                    new SqlParameter("@Command_Water", SqlDbType.Decimal,9),
                    new SqlParameter("@Actul_Water", SqlDbType.Decimal,9),
                    new SqlParameter("@Command_Ele", SqlDbType.Decimal,9),
                    new SqlParameter("@Actul_Ele", SqlDbType.Decimal,9),
                    new SqlParameter("@Command_Alkali", SqlDbType.Decimal,9),
                    new SqlParameter("@Actul_Alkali", SqlDbType.Decimal,9),
                    new SqlParameter("@Command_Salt", SqlDbType.Decimal,9),
                    new SqlParameter("@Actul_Salt", SqlDbType.Decimal,9),
                    new SqlParameter("@Command_Diesel", SqlDbType.Decimal,9),
                    new SqlParameter("@Actul_Diesel", SqlDbType.Decimal,9),
                    new SqlParameter("@WatchMan", SqlDbType.NVarChar,50),
                    new SqlParameter("@WatchManID", SqlDbType.NVarChar,50),
                    new SqlParameter("@CreateUser", SqlDbType.NVarChar,50),
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)};
        parameters[0].Value = model.RUNDATE;
        parameters[1].Value = model.RunDay;
        parameters[2].Value = model.ADDDATETIME;
        parameters[3].Value = model.MAXVALUE;
        parameters[4].Value = model.MINVALUE;
        parameters[5].Value = model.EXPORTTYPE;
        parameters[6].Value = model.FILENAME;
        parameters[7].Value = model.FILEURL;
        parameters[8].Value = model.NOTE;
        parameters[9].Value = model.CommandTime;
        parameters[10].Value = model.ActulTime;
        parameters[11].Value = model.Time_Ratio;
        parameters[12].Value = model.Command_Coal;
        parameters[13].Value = model.Actul_Coal;
        parameters[14].Value = model.Command_Water;
        parameters[15].Value = model.Actul_Water;
        parameters[16].Value = model.Command_Ele;
        parameters[17].Value = model.Actul_Ele;
        parameters[18].Value = model.Command_Alkali;
        parameters[19].Value = model.Actul_Alkali;
        parameters[20].Value = model.Command_Salt;
        parameters[21].Value = model.Actul_Salt;
        parameters[22].Value = model.Command_Diesel;
        parameters[23].Value = model.Actul_Diesel;
        parameters[24].Value = model.WatchMan;
        parameters[25].Value = model.WatchManID;
        parameters[26].Value = model.CreateUser;
        parameters[27].Value = model.ID;

        int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        if (rows > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 删除一条数据
    /// </summary>
    public bool Delete(Guid ID)
    {

        StringBuilder strSql = new StringBuilder();
        strSql.Append("delete from KT_PUSHORDER ");
        strSql.Append(" where ID=@ID ");
        SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)          };
        parameters[0].Value = ID;

        int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        if (rows > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 批量删除数据
    /// </summary>
    public bool DeleteList(string IDlist)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("delete from KT_PUSHORDER ");
        strSql.Append(" where ID in (" + IDlist + ")  ");
        int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
        if (rows > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// 得到一个对象实体
    /// </summary>
    public KT_PUSHORDER_Model GetModel(Guid ID)
    {

        StringBuilder strSql = new StringBuilder();
        strSql.Append("select  top 1 ID,RUNDATE,RunDay,ADDDATETIME,MAXVALUE,MINVALUE,EXPORTTYPE,FILENAME,FILEURL,NOTE,CommandTime,ActulTime,Time_Ratio,Command_Coal,Actul_Coal,Command_Water,Actul_Water,Command_Ele,Actul_Ele,Command_Alkali,Actul_Alkali,Command_Salt,Actul_Salt,Command_Diesel,Actul_Diesel,WatchMan,WatchManID,CreateUser from KT_PUSHORDER ");
        strSql.Append(" where ID=@ID ");
        SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)          };
        parameters[0].Value = ID;

        KT_PUSHORDER_Model model = new KT_PUSHORDER_Model();
        DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
        if (ds.Tables[0].Rows.Count > 0)
        {
            return DataRowToModel(ds.Tables[0].Rows[0]);
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// 得到一个对象实体
    /// </summary>
    public KT_PUSHORDER_Model DataRowToModel(DataRow row)
    {
        KT_PUSHORDER_Model model = new KT_PUSHORDER_Model();
        if (row != null)
        {
            if (row["ID"] != null && row["ID"].ToString() != "")
            {
                model.ID = new Guid(row["ID"].ToString());
            }
            if (row["RUNDATE"] != null)
            {
                model.RUNDATE = row["RUNDATE"].ToString();
            }
            if (row["RunDay"] != null && row["RunDay"].ToString() != "")
            {
                model.RunDay = DateTime.Parse(row["RunDay"].ToString());
            }
            if (row["ADDDATETIME"] != null && row["ADDDATETIME"].ToString() != "")
            {
                model.ADDDATETIME = DateTime.Parse(row["ADDDATETIME"].ToString());
            }
            if (row["MAXVALUE"] != null && row["MAXVALUE"].ToString() != "")
            {
                model.MAXVALUE = decimal.Parse(row["MAXVALUE"].ToString());
            }
            if (row["MINVALUE"] != null && row["MINVALUE"].ToString() != "")
            {
                model.MINVALUE = decimal.Parse(row["MINVALUE"].ToString());
            }
            if (row["EXPORTTYPE"] != null)
            {
                model.EXPORTTYPE = row["EXPORTTYPE"].ToString();
            }
            if (row["FILENAME"] != null)
            {
                model.FILENAME = row["FILENAME"].ToString();
            }
            if (row["FILEURL"] != null)
            {
                model.FILEURL = row["FILEURL"].ToString();
            }
            if (row["NOTE"] != null)
            {
                model.NOTE = row["NOTE"].ToString();
            }
            if (row["CommandTime"] != null && row["CommandTime"].ToString() != "")
            {
                model.CommandTime = decimal.Parse(row["CommandTime"].ToString());
            }
            if (row["ActulTime"] != null && row["ActulTime"].ToString() != "")
            {
                model.ActulTime = decimal.Parse(row["ActulTime"].ToString());
            }
            if (row["Time_Ratio"] != null)
            {
                model.Time_Ratio = row["Time_Ratio"].ToString();
            }
            if (row["Command_Coal"] != null && row["Command_Coal"].ToString() != "")
            {
                model.Command_Coal = decimal.Parse(row["Command_Coal"].ToString());
            }
            if (row["Actul_Coal"] != null && row["Actul_Coal"].ToString() != "")
            {
                model.Actul_Coal = decimal.Parse(row["Actul_Coal"].ToString());
            }
            if (row["Command_Water"] != null && row["Command_Water"].ToString() != "")
            {
                model.Command_Water = decimal.Parse(row["Command_Water"].ToString());
            }
            if (row["Actul_Water"] != null && row["Actul_Water"].ToString() != "")
            {
                model.Actul_Water = decimal.Parse(row["Actul_Water"].ToString());
            }
            if (row["Command_Ele"] != null && row["Command_Ele"].ToString() != "")
            {
                model.Command_Ele = decimal.Parse(row["Command_Ele"].ToString());
            }
            if (row["Actul_Ele"] != null && row["Actul_Ele"].ToString() != "")
            {
                model.Actul_Ele = decimal.Parse(row["Actul_Ele"].ToString());
            }
            if (row["Command_Alkali"] != null && row["Command_Alkali"].ToString() != "")
            {
                model.Command_Alkali = decimal.Parse(row["Command_Alkali"].ToString());
            }
            if (row["Actul_Alkali"] != null && row["Actul_Alkali"].ToString() != "")
            {
                model.Actul_Alkali = decimal.Parse(row["Actul_Alkali"].ToString());
            }
            if (row["Command_Salt"] != null && row["Command_Salt"].ToString() != "")
            {
                model.Command_Salt = decimal.Parse(row["Command_Salt"].ToString());
            }
            if (row["Actul_Salt"] != null && row["Actul_Salt"].ToString() != "")
            {
                model.Actul_Salt = decimal.Parse(row["Actul_Salt"].ToString());
            }
            if (row["Command_Diesel"] != null && row["Command_Diesel"].ToString() != "")
            {
                model.Command_Diesel = decimal.Parse(row["Command_Diesel"].ToString());
            }
            if (row["Actul_Diesel"] != null && row["Actul_Diesel"].ToString() != "")
            {
                model.Actul_Diesel = decimal.Parse(row["Actul_Diesel"].ToString());
            }
            if (row["WatchMan"] != null)
            {
                model.WatchMan = row["WatchMan"].ToString();
            }
            if (row["WatchManID"] != null)
            {
                model.WatchManID = row["WatchManID"].ToString();
            }
            if (row["CreateUser"] != null)
            {
                model.CreateUser = row["CreateUser"].ToString();
            }
        }
        return model;
    }

    /// <summary>
    /// 获得数据列表
    /// </summary>
    public DataSet GetList(string strWhere)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("select ID,RUNDATE,RunDay,ADDDATETIME,MAXVALUE,MINVALUE,EXPORTTYPE,FILENAME,FILEURL,NOTE,CommandTime,ActulTime,Time_Ratio,Command_Coal,Actul_Coal,Command_Water,Actul_Water,Command_Ele,Actul_Ele,Command_Alkali,Actul_Alkali,Command_Salt,Actul_Salt,Command_Diesel,Actul_Diesel,WatchMan,WatchManID,CreateUser ");
        strSql.Append(" FROM KT_PUSHORDER ");
        if (strWhere.Trim() != "")
        {
            strSql.Append(" where " + strWhere);
        }
        return DbHelperSQL.Query(strSql.ToString());
    }

    /// <summary>
    /// 获得前几行数据
    /// </summary>
    public DataSet GetList(int Top, string strWhere, string filedOrder)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("select ");
        if (Top > 0)
        {
            strSql.Append(" top " + Top.ToString());
        }
        strSql.Append(" ID,RUNDATE,RunDay,ADDDATETIME,MAXVALUE,MINVALUE,EXPORTTYPE,FILENAME,FILEURL,NOTE,CommandTime,ActulTime,Time_Ratio,Command_Coal,Actul_Coal,Command_Water,Actul_Water,Command_Ele,Actul_Ele,Command_Alkali,Actul_Alkali,Command_Salt,Actul_Salt,Command_Diesel,Actul_Diesel,WatchMan,WatchManID,CreateUser ");
        strSql.Append(" FROM KT_PUSHORDER ");
        if (strWhere.Trim() != "")
        {
            strSql.Append(" where " + strWhere);
        }
        strSql.Append(" order by " + filedOrder);
        return DbHelperSQL.Query(strSql.ToString());
    }

    /// <summary>
    /// 获取记录总数
    /// </summary>
    public int GetRecordCount(string strWhere)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("select count(1) FROM KT_PUSHORDER ");
        if (strWhere.Trim() != "")
        {
            strSql.Append(" where " + strWhere);
        }
        object obj = DbHelperSQL.GetSingle(strSql.ToString());
        if (obj == null)
        {
            return 0;
        }
        else
        {
            return Convert.ToInt32(obj);
        }
    }
    /// <summary>
    /// 分页获取数据列表
    /// </summary>
    public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("SELECT * FROM ( ");
        strSql.Append(" SELECT ROW_NUMBER() OVER (");
        if (!string.IsNullOrEmpty(orderby.Trim()))
        {
            strSql.Append("order by T." + orderby);
        }
        else
        {
            strSql.Append("order by T.ID desc");
        }
        strSql.Append(")AS Row, T.*  from KT_PUSHORDER T ");
        if (!string.IsNullOrEmpty(strWhere.Trim()))
        {
            strSql.Append(" WHERE " + strWhere);
        }
        strSql.Append(" ) TT");
        strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
        return DbHelperSQL.Query(strSql.ToString());
    }

    /*
    /// <summary>
    /// 分页获取数据列表
    /// </summary>
    public DataSet GetList(int PageSize,int PageIndex,string strWhere)
    {
        SqlParameter[] parameters = {
                new SqlParameter("@tblName", SqlDbType.VarChar, 255),
                new SqlParameter("@fldName", SqlDbType.VarChar, 255),
                new SqlParameter("@PageSize", SqlDbType.Int),
                new SqlParameter("@PageIndex", SqlDbType.Int),
                new SqlParameter("@IsReCount", SqlDbType.Bit),
                new SqlParameter("@OrderType", SqlDbType.Bit),
                new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
                };
        parameters[0].Value = "KT_PUSHORDER";
        parameters[1].Value = "ID";
        parameters[2].Value = PageSize;
        parameters[3].Value = PageIndex;
        parameters[4].Value = 0;
        parameters[5].Value = 0;
        parameters[6].Value = strWhere;	
        return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
    }*/

    #endregion  BasicMethod
    #region  ExtensionMethod

    #endregion  ExtensionMethod
}
}

