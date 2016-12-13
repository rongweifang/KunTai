using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KunTaiServiceLibrary.valueObjects
{
    [Serializable]
    public partial class KT_RunCommandObject
    {
        public KT_RunCommandObject()
        { }
        //      #region Model
        //      private Guid _id;
        //      private Guid _pushid;
        //      private DateTime _rundate;
        //      private Guid _stationid;
        //      private string _stateionname;
        //      private Guid _employeeid;
        //      private string _employeename;
        //      private DateTime? _adddatetime = DateTime.Now;
        //      private string _maxvalue;
        //      private string _minvalue;
        //      private string _zl_time;
        //      private string _sj_time;
        //      private string _jq_ele;
        //      private string _jq_water;
        //      private string _sj_ele;
        //      private string _sj_water;
        //      private string _zl_ele;
        //      private string _zl_water;
        //      private string _time_ratio;
        //      private decimal? _meter_num;
        //      private DateTime? _modifytime;
        //      private string _modifyuser;
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public Guid ID
        //      {
        //          set { _id = value; }
        //          get { return _id; }
        //      }
        //      /// <summary>
        ///// 
        ///// </summary>
        //public Guid PUSHID
        //      {
        //          set { _pushid = value; }
        //          get { return _pushid; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public DateTime RUNDATE
        //      {
        //          set { _rundate = value; }
        //          get { return _rundate; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public Guid STATIONID
        //      {
        //          set { _stationid = value; }
        //          get { return _stationid; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public Guid EMPLOYEEID
        //      {
        //          set { _employeeid = value; }
        //          get { return _employeeid; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public string EMPLOYEENAME
        //      {
        //          set { _employeename = value; }
        //          get { return _employeename; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public DateTime? ADDDATETIME
        //      {
        //          set { _adddatetime = value; }
        //          get { return _adddatetime; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public string MAXVALUE
        //      {
        //          set { _maxvalue = value; }
        //          get { return _maxvalue; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public string MINVALUE
        //      {
        //          set { _minvalue = value; }
        //          get { return _minvalue; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public string ZL_TIME
        //      {
        //          set { _zl_time = value; }
        //          get { return _zl_time; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public string SJ_TIME
        //      {
        //          set { _sj_time = value; }
        //          get { return _sj_time; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public string JQ_ELE
        //      {
        //          set { _jq_ele = value; }
        //          get { return _jq_ele; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public string JQ_WATER
        //      {
        //          set { _jq_water = value; }
        //          get { return _jq_water; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public string SJ_ELE
        //      {
        //          set { _sj_ele = value; }
        //          get { return _sj_ele; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public string SJ_WATER
        //      {
        //          set { _sj_water = value; }
        //          get { return _sj_water; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public string ZL_ELE
        //      {
        //          set { _zl_ele = value; }
        //          get { return _zl_ele; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public string ZL_WATER
        //      {
        //          set { _zl_water = value; }
        //          get { return _zl_water; }
        //      }

        //      public string STATEIONNAME
        //      {
        //          get
        //          {
        //              return _stateionname;
        //          }

        //          set
        //          {
        //              _stateionname = value;
        //          }
        //      }
        //      /// <summary>
        ///// 
        ///// </summary>
        //public string Time_Ratio
        //      {
        //          set { _time_ratio = value; }
        //          get { return _time_ratio; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public decimal? METER_NUM
        //      {
        //          set { _meter_num = value; }
        //          get { return _meter_num; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public DateTime? MODIFYTIME
        //      {
        //          set { _modifytime = value; }
        //          get { return _modifytime; }
        //      }
        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      public string MODIFYUSER
        //      {
        //          set { _modifyuser = value; }
        //          get { return _modifyuser; }
        //      }
        //      public KT_RunCommandObject(XElement xml)
        //      {
        //          //if (xml != null)
        //          //{
        //          //    this.NUM = xml.Element("NUM") == null ? string.Empty : xml.Element("NUM").Value;
        //          //    this.ID = xml.Element("ID") == null ? string.Empty : xml.Element("ID").Value;
        //          //    this.PUSHID = xml.Element("PUSHID") == null ? string.Empty : xml.Element("PUSHID").Value;
        //          //    this.RUNDATETIME = xml.Element("RUNDATETIME") == null ? string.Empty : xml.Element("RUNDATETIME").Value;
        //          //    this.FILEURL = xml.Element("FILEURL") == null ? string.Empty : xml.Element("FILEURL").Value;
        //          //    this.LOCALFILENAME = xml.Element("LOCALFILENAME") == null ? string.Empty : xml.Element("LOCALFILENAME").Value;
        //          //    this.NOTE = xml.Element("NOTE") == null ? string.Empty : xml.Element("NOTE").Value;



        //          //    this.PUSHDATETIME = string.Empty;
        //          //    this.RECEIVEEMPLOYEE = string.Empty;
        //          //    this.NAME = string.Empty;
        //          //}
        //      }


        //      public KT_RunCommandObject(DataRow dataRow)
        //      {
        //          //if (dataRow != null)
        //          //{
        //          //    this.NUM = dataRow.Table.Columns.Contains("NUM") ? dataRow["NUM"].ToString() : string.Empty;
        //          //    this.ID = dataRow.Table.Columns.Contains("ID") ? dataRow["ID"].ToString() : string.Empty;
        //          //    this.PUSHID = dataRow.Table.Columns.Contains("PUSHID") ? dataRow["PUSHID"].ToString() : string.Empty;
        //          //    this.NAME = dataRow.Table.Columns.Contains("NAME") ? dataRow["NAME"].ToString() : string.Empty;
        //          //    this.RUNDATETIME = dataRow.Table.Columns.Contains("RUNDATETIME") ? dataRow["RUNDATETIME"].ToString() : string.Empty;
        //          //    this.PUSHDATETIME = dataRow.Table.Columns.Contains("PUSHDATETIME") ? dataRow["PUSHDATETIME"].ToString() : string.Empty;
        //          //    this.FILEURL = dataRow.Table.Columns.Contains("FILEURL") ? dataRow["FILEURL"].ToString() : string.Empty;
        //          //    this.LOCALFILENAME = dataRow.Table.Columns.Contains("LOCALFILENAME") ? dataRow["LOCALFILENAME"].ToString() : string.Empty;
        //          //    this.NOTE = dataRow.Table.Columns.Contains("NOTE") ? dataRow["NOTE"].ToString() : string.Empty;
        //          //    RECEIVEITEM = null;
        //          //    this.RECEIVEEMPLOYEE = dataRow.Table.Columns.Contains("RECEIVEEMPLOYEE") ? dataRow["RECEIVEEMPLOYEE"].ToString() : string.Empty;
        //          //}
        //      }

        //      #endregion Model

        #region Model
        private Guid _id;
        private Guid _pushid;
        private DateTime _rundate;
        private Guid _stationid;
        private string _stateionname;
        private Guid _employeeid;
        private string _employeename;
        private DateTime? _adddatetime = DateTime.Now;
        private decimal? _maxvalue;
        private decimal? _minvalue;
        private decimal? _zl_time;
        private decimal? _sj_time;
        private decimal? _jq_ele;
        private decimal? _jq_water;
        private decimal? _sj_ele;
        private decimal? _sj_water;
        private decimal? _zl_ele;
        private decimal? _zl_water;
        private string _time_ratio;
        private decimal? _meter_num;
        private DateTime? _modifytime;
        private string _modifyuser;
        private decimal? _showid;
        /// <summary>
        /// 
        /// </summary>
        public Guid ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid PUSHID
        {
            set { _pushid = value; }
            get { return _pushid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime RUNDATE
        {
            set { _rundate = value; }
            get { return _rundate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid STATIONID
        {
            set { _stationid = value; }
            get { return _stationid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string STATEIONNAME
        {
            set { _stateionname = value; }
            get { return _stateionname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid EMPLOYEEID
        {
            set { _employeeid = value; }
            get { return _employeeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EMPLOYEENAME
        {
            set { _employeename = value; }
            get { return _employeename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ADDDATETIME
        {
            set { _adddatetime = value; }
            get { return _adddatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? MAXVALUE
        {
            set { _maxvalue = value; }
            get { return _maxvalue; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? MINVALUE
        {
            set { _minvalue = value; }
            get { return _minvalue; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? ZL_TIME
        {
            set { _zl_time = value; }
            get { return _zl_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? SJ_TIME
        {
            set { _sj_time = value; }
            get { return _sj_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? JQ_ELE
        {
            set { _jq_ele = value; }
            get { return _jq_ele; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? JQ_WATER
        {
            set { _jq_water = value; }
            get { return _jq_water; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? SJ_ELE
        {
            set { _sj_ele = value; }
            get { return _sj_ele; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? SJ_WATER
        {
            set { _sj_water = value; }
            get { return _sj_water; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? ZL_ELE
        {
            set { _zl_ele = value; }
            get { return _zl_ele; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? ZL_WATER
        {
            set { _zl_water = value; }
            get { return _zl_water; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Time_Ratio
        {
            set { _time_ratio = value; }
            get { return _time_ratio; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? METER_NUM
        {
            set { _meter_num = value; }
            get { return _meter_num; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? MODIFYTIME
        {
            set { _modifytime = value; }
            get { return _modifytime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MODIFYUSER
        {
            set { _modifyuser = value; }
            get { return _modifyuser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? SHOWID
        {
            set { _showid = value; }
            get { return _showid; }
        }
        #endregion Model
    }
}
