using Warrior.DataAccessUtils.Desktop;

namespace KunTaiServiceLibrary
{

    public class PushOrderAdminUtils
    {

        #region command texts

        private const string push_order_admin_select = "SELECT COUNT(*) FROM PUSHORDERADMIN WHERE EMPLOYEEID=@EMPLOYEEID";

        #endregion



        /// <summary>
        /// 判断是否是后台配置的管理员
        /// <para>true - 存在; false - 不存在</para>
        /// </summary>
        /// <param name="employeeID">用户编号UUID</param>
        /// <returns>返回是否是配置的管理员</returns>
        public static bool isAdmin(string employeeID)
        {
            bool result = false;

            //从数据库中获取 可以发布指令的用户编号
            string count = new DataAccessHandler().executeScalarResult(
                push_order_admin_select,
                SqlServer.GetParameter("EMPLOYEEID", employeeID));

            if (count == "1")
            {
                result = true;
            }

            /*
            string[] pushOrderAdminValue = Config.PushOrderAdmin.Split(',');

            List<string> listPushOrderAdminID = new List<string>();
            foreach (string item in pushOrderAdminValue)
            {
                listPushOrderAdminID.Add(item.Trim().ToUpper());
            }
            pushOrderAdminValue = null;

            bool result = listPushOrderAdminID.Contains(employeeID.ToUpper());

            listPushOrderAdminID = null;
            */
            return result;
        }

    }
}
