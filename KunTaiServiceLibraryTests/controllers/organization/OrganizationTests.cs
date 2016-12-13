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
    public class OrganizationTests
    {
        [TestMethod()]
        public void addDataItemTest()
        {
            string temp = new Organization().getOrganizationList();


            for (int i = 461; i < 490; i++)
            {
                StringBuilder text = new StringBuilder();
                text.Append("<ORGANIZATION>");
                text.AppendFormat("<NAME>{0}</NAME>", "NAME" + i.ToString());
                text.AppendFormat("<EMPLOYEE>{0}</EMPLOYEE>", "EMPLOYEE" + i.ToString());
                text.AppendFormat("<PHONE>{0}</PHONE>", "PHONE" + i.ToString());
                text.AppendFormat("<ADDRESS>{0}</ADDRESS>", "ADDRESS" + i.ToString());
                text.AppendFormat("<CITYID>{0}</CITYID>", "CITYID" + i.ToString());
                text.AppendFormat("<NOTE>{0}</NOTE>", "NOTE" + i.ToString());
                text.Append("</ORGANIZATION>");

                string result = new Organization().addDataItem(text.ToString());

                Debug.WriteLine(result);
            }
        }
    }
}