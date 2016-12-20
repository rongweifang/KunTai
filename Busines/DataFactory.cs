using Common.DotNetConfig;
using DataBase;
using PDA_Service.DataBase.DataBase.SqlServer;

namespace Busines
{
    public class DataFactory
    {
        public static IDbHelper SqlDataBase()
        {
            return new SqlServerHelper(ConfigHelper.GetAppSettings("SqlServer_RM_DB"));
        }
    }
}