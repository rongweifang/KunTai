using Microsoft.VisualStudio.TestTools.UnitTesting;
using KunTaiServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace KunTaiServiceLibrary.Tests
{
    [TestClass()]
    public class RoleTests
    {
        [TestMethod()]
        public void addDataItemTest()
        {

            string temp = new Role().getRoleAuthority("<ROLE><ID>'f6a24fc9-9fb6-4f6a-9c45-8df8d143c150','6aa31b80-485a-42f8-afc6-bc957733c651'</ID></ROLE>");
            
            StringBuilder text = new StringBuilder();
            
            text.Append("<ROLE>");
            text.Append("<ID>'f6a24fc9-9fb6-4f6a-9c45-8df8d143c150'</ID>");
            text.Append("<AUTHORITY>");
            text.AppendFormat("<DATA ID=\"{0}\" />", "B7E8FF70-B8D7-4A89-A4E1-5019331BCF83");
            text.AppendFormat("<DATA ID=\"{0}\" />", "03FA7F61-EAE9-4991-9F71-5553381111FB");
            text.AppendFormat("<DATA ID=\"{0}\" />", "5A9E26FE-1893-4432-82D4-536CE1734816");
            text.AppendFormat("<DATA ID=\"{0}\" />", "1FBCE230-ACF5-4D28-975D-18E832AA012D");
            text.AppendFormat("<DATA ID=\"{0}\" />", "F6F5D156-86D7-403C-86C5-49D7468D7C85");
            text.AppendFormat("<DATA ID=\"{0}\" />", "EC20F475-37F7-4FE3-83C4-915D45570290");
            text.AppendFormat("<DATA ID=\"{0}\" />", "CAF0510F-6AE1-4747-B2A0-970035CF4095");
            text.AppendFormat("<DATA ID=\"{0}\" />", "2CD5E94E-D000-482D-8A27-2D686521B252");
            text.Append("</AUTHORITY>");
            text.Append("</ROLE>");

            string result = new Role().updateRoleAuthority(text.ToString());

            Debug.WriteLine(result);


        }
    }
}