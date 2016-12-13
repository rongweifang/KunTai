using KunTaiServiceLibrary.valueObjects;
using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KunTaiServiceLibrary
{
    [FluorineFx.RemotingService("坤泰指令基期对比")]
    public class KT_RunCommand : IController
    {
        public KT_RunCommand()
        { }

        public string getDataItem(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Result.getFaultXml(Error.XML_IS_NULL);

            XElement xml = null;
            try
            {
                xml = XElement.Parse(text);
            }
            catch
            {
                return Result.getFaultXml(Error.XML_FORMAT_ERROR);
            }

            StringBuilder sbstr = new StringBuilder();
            string commandText = "SELECT * FROM View_BasePeriod";
            DataSet dataSetPushOrder = null;
            try
            {
                dataSetPushOrder = new DataAccessHandler().executeDatasetResult(
                    SqlServerCommandText.createPagingCommandText(ref commandText, ref xml, "NUM1"), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetPushOrder != null && dataSetPushOrder.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemXml(ref dataSetPushOrder, getDataItemTotal()));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            return result;
        }

        private string getDataItemTotal()
        {
            return new DataAccessHandler().executeScalarResult("SELECT COUNT(1) FROM View_BasePeriod", null);
        }

        private string getDataItemXml(ref DataSet dataSetPushOrder, string total)
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendFormat("<DATAS COUNT=\"{0}\" TOTAL=\"{1}\">", dataSetPushOrder.Tables[0].Rows.Count, total);
            //  KT_RunCommandObject RunCommandObject = null;
            foreach (DataRow row in dataSetPushOrder.Tables[0].Rows)
            {
                xml.AppendFormat("<DATA RUNDATE=\"{0}\" AVETEMP=\"{1}\" JQ_ELE=\"{2}\" ZL_ELE=\"{3}\" SJ_ELE=\"{4}\" JN_ELE=\"{5}\" JN_ELEROTE=\"{6}\" JQ_WATER=\"{7}\" ZL_WATER=\"{8}\" SJ_WATER=\"{9}\" JN_WATER=\"{10}\" JN_WATERROTE=\"{11}\" />",
                  Convert.ToDateTime(row["RUNDATE"].ToString()).ToString("yyyy年MM月dd日"),
                   row["AVETEMP"].ToString(),
                   row["JQ_ELE"].ToString(),
                   row["ZL_ELE"].ToString(),
                   row["SJ_ELE"].ToString(),
                   row["JN_ELE"].ToString(),
                   row["JN_ELEROTE"].ToString(),
                   row["JQ_WATER"].ToString(),
                   row["ZL_WATER"].ToString(),
                   row["SJ_WATER"].ToString(),
                   row["JN_WATER"].ToString(),
                   row["JN_WATERROTE"].ToString()
                );
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(Guid ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from KT_RUNCOMMAND");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)          };
            parameters[0].Value = ID;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Add(KT_RunCommandObject model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into KT_RUNCOMMAND(");
            strSql.Append("ID,PUSHID,RUNDATE,STATIONID,STATEIONNAME,ADDDATETIME,MAXVALUE,MINVALUE,ZL_TIME,ZL_ELE,ZL_WATER,Time_Ratio)");
            strSql.Append(" values (");
            strSql.Append("@ID,@PUSHID,@RUNDATE,@STATIONID,@STATEIONNAME,@ADDDATETIME,@MAXVALUE,@MINVALUE,@ZL_TIME,@ZL_ELE,@ZL_WATER,@Time_Ratio)");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16),
                    new SqlParameter("@PUSHID", SqlDbType.UniqueIdentifier,16),
                    new SqlParameter("@RUNDATE", SqlDbType.Date,3),
                    new SqlParameter("@STATIONID", SqlDbType.UniqueIdentifier,16),
                    new SqlParameter("@STATEIONNAME", SqlDbType.NVarChar,50),
                    new SqlParameter("@ADDDATETIME", SqlDbType.Date,3),
                    new SqlParameter("@MAXVALUE", SqlDbType.Decimal,9),
                    new SqlParameter("@MINVALUE", SqlDbType.Decimal,9),
                    new SqlParameter("@ZL_TIME", SqlDbType.Decimal,9),
                    new SqlParameter("@ZL_ELE", SqlDbType.Decimal,9),
                    new SqlParameter("@ZL_WATER", SqlDbType.Decimal,9),
                    new SqlParameter("@Time_Ratio", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.PUSHID;
            parameters[2].Value = model.RUNDATE;
            parameters[3].Value = model.STATIONID;
            parameters[4].Value = model.STATEIONNAME;
            parameters[5].Value = model.ADDDATETIME;
            parameters[6].Value = model.MAXVALUE;
            parameters[7].Value = model.MINVALUE;
            parameters[8].Value = model.ZL_TIME;
            parameters[9].Value = model.ZL_ELE;
            parameters[10].Value = model.ZL_WATER;
            parameters[11].Value = model.Time_Ratio;

            //int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            return new DataAccessHandler().executeNonQueryResult(strSql.ToString(), parameters);
            //if (rows > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public bool Import(KT_RunCommandObject model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into KT_RUNCOMMAND(");
            strSql.Append("ID,PUSHID,RUNDATE,STATIONID,STATEIONNAME,EMPLOYEEID,EMPLOYEENAME,ADDDATETIME,MAXVALUE,MINVALUE,ZL_TIME,SJ_TIME,ZL_ELE,SJ_ELE,ZL_WATER,SJ_WATER)");
            strSql.Append(" values (");
            strSql.Append("@ID,@PUSHID,@RUNDATE,@STATIONID,@STATEIONNAME,@EMPLOYEEID,@EMPLOYEENAME,@ADDDATETIME,@MAXVALUE,@MINVALUE,@ZL_TIME,@SJ_TIME,@ZL_ELE,@SJ_ELE,@ZL_WATER,@SJ_WATER)");
            SqlParameter[] parameters = {
                   new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16),
                    new SqlParameter("@PUSHID", SqlDbType.UniqueIdentifier,16),
                    new SqlParameter("@RUNDATE", SqlDbType.Date,3),
                    new SqlParameter("@STATIONID", SqlDbType.UniqueIdentifier,16),
                    new SqlParameter("@STATEIONNAME", SqlDbType.NVarChar,50),
                    new SqlParameter("@EMPLOYEEID", SqlDbType.UniqueIdentifier,16),
                    new SqlParameter("@EMPLOYEENAME", SqlDbType.NVarChar,50),
                    new SqlParameter("@ADDDATETIME", SqlDbType.Date,3),
                    new SqlParameter("@MAXVALUE", SqlDbType.Decimal,9),
                    new SqlParameter("@MINVALUE", SqlDbType.Decimal,9),
                    new SqlParameter("@ZL_TIME", SqlDbType.Decimal,9),
                    new SqlParameter("@SJ_TIME", SqlDbType.Decimal,9),
                    new SqlParameter("@ZL_ELE", SqlDbType.Decimal,9),
                    new SqlParameter("@SJ_ELE", SqlDbType.Decimal,9),
                    new SqlParameter("@ZL_WATER", SqlDbType.Decimal,9),
                    new SqlParameter("@SJ_WATER", SqlDbType.Decimal,9)};
            parameters[0].Value = Guid.NewGuid();
            parameters[1].Value = Guid.NewGuid();
            parameters[2].Value = model.RUNDATE;
            parameters[3].Value = Guid.NewGuid();
            parameters[4].Value = model.STATEIONNAME;
            parameters[5].Value = Guid.NewGuid();
            parameters[6].Value = model.EMPLOYEENAME;
            parameters[7].Value = model.ADDDATETIME;
            parameters[8].Value = model.MAXVALUE;
            parameters[9].Value = model.MINVALUE;
            parameters[10].Value = model.ZL_TIME;
            parameters[11].Value = model.SJ_TIME;
            parameters[12].Value = model.ZL_ELE;
            parameters[13].Value = model.SJ_ELE;
            parameters[14].Value = model.ZL_WATER;
            parameters[15].Value = model.SJ_WATER; ;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(KT_RunCommandObject model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update KT_RUNCOMMAND set ");
            strSql.Append("RUNDATE=@RUNDATE,");
            strSql.Append("STATIONID=@STATIONID,");
            strSql.Append("STATEIONNAME=@STATEIONNAME,");
            strSql.Append("EMPLOYEEID=@EMPLOYEEID,");
            strSql.Append("EMPLOYEENAME=@EMPLOYEENAME,");
            strSql.Append("ADDDATETIME=@ADDDATETIME,");
            strSql.Append("MAXVALUE=@MAXVALUE,");
            strSql.Append("MINVALUE=@MINVALUE,");
            strSql.Append("ZL_TIME=@ZL_TIME,");
            strSql.Append("SJ_TIME=@SJ_TIME,");
            strSql.Append("JQ_ELE=@JQ_ELE,");
            strSql.Append("JQ_WATER=@JQ_WATER,");
            strSql.Append("SJ_ELE=@SJ_ELE,");
            strSql.Append("SJ_WATER=@SJ_WATER,");
            strSql.Append("ZL_ELE=@ZL_ELE,");
            strSql.Append("ZL_WATER=@ZL_WATER");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@RUNDATE", SqlDbType.Date,3),
                    new SqlParameter("@STATIONID", SqlDbType.UniqueIdentifier,16),
                    new SqlParameter("@STATEIONNAME", SqlDbType.NVarChar,50),
                    new SqlParameter("@EMPLOYEEID", SqlDbType.UniqueIdentifier,16),
                    new SqlParameter("@EMPLOYEENAME", SqlDbType.NVarChar,50),
                    new SqlParameter("@ADDDATETIME", SqlDbType.Date,3),
                    new SqlParameter("@MAXVALUE", SqlDbType.NVarChar,50),
                    new SqlParameter("@MINVALUE", SqlDbType.NVarChar,50),
                    new SqlParameter("@ZL_TIME", SqlDbType.NVarChar,50),
                    new SqlParameter("@SJ_TIME", SqlDbType.NVarChar,50),
                    new SqlParameter("@JQ_ELE", SqlDbType.NVarChar,50),
                    new SqlParameter("@JQ_WATER", SqlDbType.NVarChar,50),
                    new SqlParameter("@SJ_ELE", SqlDbType.NVarChar,50),
                    new SqlParameter("@SJ_WATER", SqlDbType.NVarChar,50),
                    new SqlParameter("@ZL_ELE", SqlDbType.NVarChar,50),
                    new SqlParameter("@ZL_WATER", SqlDbType.NVarChar,50),
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)};
            parameters[0].Value = model.RUNDATE;
            parameters[1].Value = model.STATIONID;
            parameters[2].Value = model.STATEIONNAME;
            parameters[3].Value = model.EMPLOYEEID;
            parameters[4].Value = model.EMPLOYEENAME;
            parameters[5].Value = model.ADDDATETIME;
            parameters[6].Value = model.MAXVALUE;
            parameters[7].Value = model.MINVALUE;
            parameters[8].Value = model.ZL_TIME;
            parameters[9].Value = model.SJ_TIME;
            parameters[10].Value = model.JQ_ELE;
            parameters[11].Value = model.JQ_WATER;
            parameters[12].Value = model.SJ_ELE;
            parameters[13].Value = model.SJ_WATER;
            parameters[14].Value = model.ZL_ELE;
            parameters[15].Value = model.ZL_WATER;
            parameters[16].Value = model.ID;

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
            strSql.Append("delete from KT_RUNCOMMAND ");
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

        public Guid GetStationID(string v)
        {
            Guid G = new Guid();

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT TOP 1 ID  FROM STATION WHERE NAME LIKE '%{0}%'", v);

            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return G;
            }
            else
            {
                return new Guid(obj.ToString());
            }
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from KT_RUNCOMMAND ");
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
        public KT_RunCommandObject GetModel(Guid ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,RUNDATE,STATIONID,STATEIONNAME,EMPLOYEEID,EMPLOYEENAME,ADDDATETIME,MAXVALUE,MINVALUE,ZL_TIME,SJ_TIME,JQ_ELE,JQ_WATER,SJ_ELE,SJ_WATER,ZL_ELE,ZL_WATER from KT_RUNCOMMAND ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)          };
            parameters[0].Value = ID;

            KT_RunCommandObject model = new KT_RunCommandObject();
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
        public KT_RunCommandObject DataRowToModel(DataRow row)
        {
            KT_RunCommandObject model = new KT_RunCommandObject();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = new Guid(row["ID"].ToString());
                }
                if (row["RUNDATE"] != null && row["RUNDATE"].ToString() != "")
                {
                    model.RUNDATE = DateTime.Parse(row["RUNDATE"].ToString());
                }
                if (row["STATIONID"] != null && row["STATIONID"].ToString() != "")
                {
                    model.STATIONID = new Guid(row["STATIONID"].ToString());
                }
                if (row["STATEIONNAME"] != null)
                {
                    model.STATEIONNAME = row["STATEIONNAME"].ToString();
                }
                if (row["EMPLOYEEID"] != null && row["EMPLOYEEID"].ToString() != "")
                {
                    model.EMPLOYEEID = new Guid(row["EMPLOYEEID"].ToString());
                }
                if (row["EMPLOYEENAME"] != null)
                {
                    model.EMPLOYEENAME = row["EMPLOYEENAME"].ToString();
                }
                if (row["ADDDATETIME"] != null && row["ADDDATETIME"].ToString() != "")
                {
                    model.ADDDATETIME = DateTime.Parse(row["ADDDATETIME"].ToString());
                }
                if (row["MAXVALUE"] != null)
                {
                    model.MAXVALUE =decimal.Parse(row["MAXVALUE"].ToString());
                }
                if (row["MINVALUE"] != null)
                {
                    model.MINVALUE =decimal.Parse(row["MINVALUE"].ToString());
                }
                if (row["ZL_TIME"] != null)
                {
                    model.ZL_TIME =decimal.Parse(row["ZL_TIME"].ToString());
                }
                if (row["SJ_TIME"] != null)
                {
                    model.SJ_TIME =decimal.Parse(row["SJ_TIME"].ToString());
                }
                if (row["JQ_ELE"] != null)
                {
                    model.JQ_ELE =decimal.Parse(row["JQ_ELE"].ToString());
                }
                if (row["JQ_WATER"] != null)
                {
                    model.JQ_WATER =decimal.Parse(row["JQ_WATER"].ToString());
                }
                if (row["SJ_ELE"] != null)
                {
                    model.SJ_ELE = decimal.Parse(row["SJ_ELE"].ToString());
                }
                if (row["SJ_WATER"] != null)
                {
                    model.SJ_WATER = decimal.Parse(row["SJ_WATER"].ToString());
                }
                if (row["ZL_ELE"] != null)
                {
                    model.ZL_ELE = decimal.Parse(row["ZL_ELE"].ToString());
                }
                if (row["ZL_WATER"] != null)
                {
                    model.ZL_WATER = decimal.Parse(row["ZL_WATER"].ToString());
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
            strSql.Append("select ID,RUNDATE,STATIONID,STATEIONNAME,EMPLOYEEID,EMPLOYEENAME,ADDDATETIME,MAXVALUE,MINVALUE,ZL_TIME,SJ_TIME,JQ_ELE,JQ_WATER,SJ_ELE,SJ_WATER,ZL_ELE,ZL_WATER ");
            strSql.Append(" FROM KT_RUNCOMMAND ");
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
            strSql.Append(" ID,RUNDATE,STATIONID,STATEIONNAME,EMPLOYEEID,EMPLOYEENAME,ADDDATETIME,MAXVALUE,MINVALUE,ZL_TIME,SJ_TIME,JQ_ELE,JQ_WATER,SJ_ELE,SJ_WATER,ZL_ELE,ZL_WATER ");
            strSql.Append(" FROM KT_RUNCOMMAND ");
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
            strSql.Append("select count(1) FROM KT_RUNCOMMAND ");
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
            strSql.Append(")AS Row, T.*  from KT_RUNCOMMAND T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        public string addDataItem(string text)
        {
            throw new NotImplementedException();
        }

        public string editDataItem(string text)
        {
            //更新指令
            //xml += StringUtils.format("<TABLENAME>{0}</TABLENAME>", TableName);
            //xml += StringUtils.format("<PKNAME>{0}</PKNAME>", PkName);
            //xml += StringUtils.format("<PKID>{0}</PKID>", PkID);
            //xml += StringUtils.format("<FILEDNAME>{0}</FILEDNAME>", FiledName);
            //xml += StringUtils.format("<FILEVALUE>{0}</FILEVALUE>", this.text);

            if (string.IsNullOrEmpty(text))
                return Result.getFaultXml(Error.XML_IS_NULL);

            XElement xml = null;
            try
            {
                xml = XElement.Parse(text);
            }
            catch
            {
                return Result.getFaultXml(Error.XML_FORMAT_ERROR);
            }

            EditItemModel model = new EditItemModel(xml);
            string commandText = string.Format("UPDATE {0} SET {1}=@FILEVALUE WHERE {2}=@PKID",model.TABLENAME,model.FILEDNAME,model.PKNAME);

            string result = string.Empty;
            try
            {
                result= new DataAccessHandler().executeNonQueryResult(commandText,new SqlParameter[] { new SqlParameter("@FILEVALUE", model.FILEVALUE),new SqlParameter("@PKID",model.PKID)  });
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }
            return result;
        }

        public string deleteDataItem(string text)
        {
            throw new NotImplementedException();
        }

        public string getDataItemDetails(string text)
        {
            throw new NotImplementedException();
        }

    }

    public class EditItemModel
    {
        public EditItemModel()
        {

        }
        private string _TABLENAME;
        private string _PKNAME;
        private string _PKID;
        private string _FILEDNAME;
        private string _FILEVALUE;

        public string TABLENAME
        {
            get
            {
                return _TABLENAME;
            }

            set
            {
                _TABLENAME = value;
            }
        }

        public string PKNAME
        {
            get
            {
                return _PKNAME;
            }

            set
            {
                _PKNAME = value;
            }
        }

        public string PKID
        {
            get
            {
                return _PKID;
            }

            set
            {
                _PKID = value;
            }
        }

        public string FILEDNAME
        {
            get
            {
                return _FILEDNAME;
            }

            set
            {
                _FILEDNAME = value;
            }
        }

        public string FILEVALUE
        {
            get
            {
                return _FILEVALUE;
            }

            set
            {
                _FILEVALUE = value;
            }
        }

        public EditItemModel(XElement xml)
        {
            if (xml != null)
            {
                this.TABLENAME = xml.Element("TABLENAME") == null ? string.Empty : xml.Element("TABLENAME").Value;
                this.PKNAME = xml.Element("PKNAME") == null ? string.Empty : xml.Element("PKNAME").Value;
                this.PKID = xml.Element("PKID") == null ? string.Empty : xml.Element("PKID").Value;
                this.FILEDNAME = xml.Element("FILEDNAME") == null ? string.Empty : xml.Element("FILEDNAME").Value;
                this.FILEVALUE = xml.Element("FILEVALUE") == null ? string.Empty : xml.Element("FILEVALUE").Value;
            }
        }
    }
}
