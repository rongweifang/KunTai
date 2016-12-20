using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Busines.MODEL
{
    [Serializable]
    public partial class User_Files_Model
    {
        public void User_Files()
        { }
        #region Model
        private Guid _fileid;
        private string _unsubscribeid;
        private string _filetype;
        private byte[] _filecontent;
        private string _extname;
        private string _filepath;
        private string _weburl;
        private string _filename;
        private decimal? _filesize;
        private string _showname;
        private DateTime? _uploaddate = DateTime.Now;
        private string _tablenames;
        /// <summary>
        /// 
        /// </summary>
        public Guid FileID
        {
            set { _fileid = value; }
            get { return _fileid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UnsubscribeID
        {
            set { _unsubscribeid = value; }
            get { return _unsubscribeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FileType
        {
            set { _filetype = value; }
            get { return _filetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] FileContent
        {
            set { _filecontent = value; }
            get { return _filecontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExtName
        {
            set { _extname = value; }
            get { return _extname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FilePath
        {
            set { _filepath = value; }
            get { return _filepath; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WebUrl
        {
            set { _weburl = value; }
            get { return _weburl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FileName
        {
            set { _filename = value; }
            get { return _filename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? FileSize
        {
            set { _filesize = value; }
            get { return _filesize; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ShowName
        {
            set { _showname = value; }
            get { return _showname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UploadDate
        {
            set { _uploaddate = value; }
            get { return _uploaddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TableNames
        {
            set { _tablenames = value; }
            get { return _tablenames; }
        }
        #endregion Model
    }
}
