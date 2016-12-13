using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KunTaiServiceLibrary
{
    /// <summary>
    /// KT_PUSHORDER_Model:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class KT_PUSHORDER_Model
    {
        public KT_PUSHORDER_Model()
        { }
        #region Model
        private Guid _id;
        private string _rundate;
        private DateTime? _runday = DateTime.Now;
        private DateTime _adddatetime = DateTime.Now;
        private decimal _maxvalue;
        private decimal _minvalue;
        private string _exporttype;
        private string _filename;
        private string _fileurl;
        private string _note;
        private decimal? _commandtime;
        private decimal? _actultime;
        private string _time_ratio;
        private decimal? _command_coal;
        private decimal? _actul_coal;
        private decimal? _command_water;
        private decimal? _actul_water;
        private decimal? _command_ele;
        private decimal? _actul_ele;
        private decimal? _command_alkali;
        private decimal? _actul_alkali;
        private decimal? _command_salt;
        private decimal? _actul_salt;
        private decimal? _command_diesel;
        private decimal? _actul_diesel;
        private string _watchman;
        private string _watchmanid;
        private string _createuser;
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
        public string RUNDATE
        {
            set { _rundate = value; }
            get { return _rundate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? RunDay
        {
            set { _runday = value; }
            get { return _runday; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ADDDATETIME
        {
            set { _adddatetime = value; }
            get { return _adddatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal MAXVALUE
        {
            set { _maxvalue = value; }
            get { return _maxvalue; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal MINVALUE
        {
            set { _minvalue = value; }
            get { return _minvalue; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EXPORTTYPE
        {
            set { _exporttype = value; }
            get { return _exporttype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FILENAME
        {
            set { _filename = value; }
            get { return _filename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FILEURL
        {
            set { _fileurl = value; }
            get { return _fileurl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NOTE
        {
            set { _note = value; }
            get { return _note; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? CommandTime
        {
            set { _commandtime = value; }
            get { return _commandtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? ActulTime
        {
            set { _actultime = value; }
            get { return _actultime; }
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
        public decimal? Command_Coal
        {
            set { _command_coal = value; }
            get { return _command_coal; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Actul_Coal
        {
            set { _actul_coal = value; }
            get { return _actul_coal; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Command_Water
        {
            set { _command_water = value; }
            get { return _command_water; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Actul_Water
        {
            set { _actul_water = value; }
            get { return _actul_water; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Command_Ele
        {
            set { _command_ele = value; }
            get { return _command_ele; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Actul_Ele
        {
            set { _actul_ele = value; }
            get { return _actul_ele; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Command_Alkali
        {
            set { _command_alkali = value; }
            get { return _command_alkali; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Actul_Alkali
        {
            set { _actul_alkali = value; }
            get { return _actul_alkali; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Command_Salt
        {
            set { _command_salt = value; }
            get { return _command_salt; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Actul_Salt
        {
            set { _actul_salt = value; }
            get { return _actul_salt; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Command_Diesel
        {
            set { _command_diesel = value; }
            get { return _command_diesel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Actul_Diesel
        {
            set { _actul_diesel = value; }
            get { return _actul_diesel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WatchMan
        {
            set { _watchman = value; }
            get { return _watchman; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WatchManID
        {
            set { _watchmanid = value; }
            get { return _watchmanid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreateUser
        {
            set { _createuser = value; }
            get { return _createuser; }
        }
        #endregion Model

    }
}
