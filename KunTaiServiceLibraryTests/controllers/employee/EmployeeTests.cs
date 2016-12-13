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
    public class EmployeeTests
    {
        [TestMethod()]
        public void addDataItemTest()
        {
            string temp = new Employee().getDataItem("<EMPLOYEE><WHERE>E.[NAME] LIKE '%测试%'</WHERE><PAGENUMBER>1</PAGENUMBER><PAGECOUNT>15</PAGECOUNT></EMPLOYEE>");


            StringBuilder text = new StringBuilder();
            
            text.Append("<EMPLOYEE>");
            text.AppendFormat("<ID>{0}</ID>", "c149277d-0964-4304-b0a3-bb940c7e7415");
            text.Append("</EMPLOYEE>");



            string result = new Employee().getEmployeeAuthority(text.ToString());

            Debug.WriteLine(result);
        }
    }
}