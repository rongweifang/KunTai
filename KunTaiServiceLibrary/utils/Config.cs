using System.Configuration;

namespace KunTaiServiceLibrary
{
    public class Config
    {


        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string DBConnectionString
        {
           // get { return "SERVER=.;DATABASE=KUNTAI_DB;USER ID=sa;PASSWORD=sa"; }
            get { return ConfigurationManager.ConnectionStrings["DATABASE"].ToString(); }
        }


        /// <summary>
        /// 导出组件的授权文件路径
        /// </summary>
        public static string ExportLicenseUrl
        {
           // get { return @"C:\uploadFiles\License.lic"; }
            get { return ConfigurationManager.AppSettings["EXPORTLICENSEURL"].ToString(); }
        }


        /// <summary>
        /// 上传或导出文件的物理目录
        /// </summary>
        public static string UploadExportFileDirectory
        {
            //get { return @"C:\uploadFiles"; }
            get { return ConfigurationManager.AppSettings["UPLOADFILEDIRECTORY"].ToString(); }
        }

        /// <summary>
        /// 上传或导出文件的HTTP路径
        /// </summary>
        public static string UploadExportFileHttpUrl
        {
            //get { return @"http://127.0.0.1:809"; }
            get { return ConfigurationManager.AppSettings["UPLOADFILEHTTP"].ToString(); }
        }


    }
}
