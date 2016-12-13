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
    public class ZY_WR_STAT_ATests
    {
        [TestMethod()]
        public void getDataItemTest()
        {
            string text = "<STATION><TYPE>南线</TYPE><ID></ID></STATION>";

            string result = new ZY_WR_STAT_A().saveWR_STAT_AGroupLine(text);
        }
    }
}