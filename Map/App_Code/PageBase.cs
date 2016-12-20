using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using Busines.DAL;
using Busines.IDAO;
using Common.DotNetBean;
using Common.DotNetCode;
using Common.DotNetUI;

namespace Zhaopin.App_Code
{
    public class PageBase : Page
    {
        private RM_System_IDAO sys_idao = new RM_System_Dal();
        protected override void OnLoad(EventArgs e)
        {
            if (RequestSession.GetSessionUser() == null)
            {
                this.Session.Abandon();
                this.Session.Clear();
                base.Response.Redirect("/");
            }
            if (null == this.Session["Token"])
            {
                WebHelper.SetToken();
            }
            this.URLPermission();
            base.OnLoad(e);
        }
        public void URLPermission()
        {
            bool IsOK = false;
            string requestPath = RequestHelper.GetScriptName;
            string[] filterUrl = new string[]
			{
				"/",
				"/UserCenter/login.aspx"
			};
            for (int i = 0; i < filterUrl.Length; i++)
            {
                if (requestPath == filterUrl[i])
                {
                    IsOK = true;
                    break;
                }
            }
            if (!IsOK)
            {
                string UserId = RequestSession.GetSessionUser().UserId.ToString();
                DataTable dt = this.sys_idao.GetPermission_URL(UserId);
                if (new DataView(dt)
                {
                    RowFilter = "NavigateUrl = '" + requestPath + "'"
                }.Count == 0)
                {
                    StringBuilder strHTML = new StringBuilder();
                    strHTML.Append("<div style='text-align: center; line-height: 300px;'>");
                    strHTML.Append("<font style=\"font-size: 13;font-weight: bold; color: red;\">权限不足</font></div>");
                    HttpContext.Current.Response.Write(strHTML.ToString());
                    HttpContext.Current.Response.End();
                }
            }
        }
 
    }
}