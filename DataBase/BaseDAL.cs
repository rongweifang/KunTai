using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using FluentData;
using Common.DotNetConfig;

namespace DataBase.DAL
{
    public class BaseDAL
    {
        public static IDbContext WDbContext()
        {
            string strConn = ConfigHelper.GetAppSettings("SqlServer_RM_DB");
            return new DbContext().ConnectionString(strConn, new SqlServerProvider());
        }
    }
}