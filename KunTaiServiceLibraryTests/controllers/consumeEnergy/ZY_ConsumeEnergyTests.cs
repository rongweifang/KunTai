using Microsoft.VisualStudio.TestTools.UnitTesting;
using KunTaiServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KunTaiServiceLibrary.Tests
{
    [TestClass()]
    public class ZY_ConsumeEnergyTests
    {
        [TestMethod()]
        public void addDataItemTest()
        {

            string result = new ZY_ConsumeEnergy()
                .getDataItem("<CONSUMEENERGY><EMPLOYEEID>97ab9175-43ac-44a8-9ba3-ab4d5d57fecc</EMPLOYEEID><WHERE></WHERE><PAGENUMBER>1</PAGENUMBER><PAGECOUNT>15</PAGECOUNT></CONSUMEENERGY>");
        }


    }
}