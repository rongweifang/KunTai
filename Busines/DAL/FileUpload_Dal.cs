using Busines.IDAO;
using Common.DotNetCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Busines.DAL
{
    public class FileUpload_Dal: FileUpload_IDal
    {
        public bool SaveImages(string unSubscribeID, string fileType, string extName, string imagePath, string webUrl, string fileName, string showName, string tableNames)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into User_Files(");
            strSql.Append("FileID,UnsubscribeID,FileType,FileContent,ExtName,FilePath,WebUrl,FileName,ShowName,TableNames)");
            strSql.Append(" values (");
            strSql.Append("@FileID,@UnsubscribeID,@FileType,@FileContent,@ExtName,@FilePath,@WebUrl,@FileName,@ShowName,@TableNames)");
            SqlParam[] parameters = {
                    new SqlParam("@FileID", Guid.NewGuid().ToString()),
                    new SqlParam("@UnsubscribeID", unSubscribeID),
                    new SqlParam("@FileType", fileType),
                    new SqlParam("@FileContent", SqlDbType.Image),
                    new SqlParam("@ExtName", extName),
                    new SqlParam("@FilePath", imagePath),
                    new SqlParam("@WebUrl", webUrl),
                    new SqlParam("@FileName", fileName),
                    new SqlParam("@ShowName", showName),
                    new SqlParam("@TableNames", tableNames)};

            int rows = DataFactory.SqlDataBase().ExecuteBySql(strSql, parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable GetFilesByUnsubscribeID(string unsubscribeID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT FileID,UnsubscribeID,WebUrl,ShowName,REPLACE(WebUrl,FileName,FileName+'-Thumbs') AS ThumbnailName  FROM User_Files WHERE UnsubscribeID=@UnsubscribeID order by UploadDate desc");

            SqlParam[] param = new SqlParam[]
            {
                new SqlParam("@UnsubscribeID",unsubscribeID)
            };
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, param);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteUploadFileByFileID(string FileID)
        {
            int rows = DataFactory.SqlDataBase().DeleteData("User_Files", "FileID", FileID);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
