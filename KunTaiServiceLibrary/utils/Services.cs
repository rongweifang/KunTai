using FluorineFx;
using System.Configuration;
using System.Data.OracleClient;

namespace KunTaiServiceLibrary
{
    [RemotingService]
    public class Services
    {
        /// <summary>
        /// 获取数据库连接的状态
        /// </summary>
        /// <returns></returns>
        public string getDBConnectionState()
        {
            return new DataAccessHandler().getDBConnectionState(string.Empty);
        }

        public string getDBConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DATABASE"].ToString();
        }

    }
}
