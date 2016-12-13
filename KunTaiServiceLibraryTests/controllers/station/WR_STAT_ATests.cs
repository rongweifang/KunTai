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
    public class WR_STAT_ATests
    {
        [TestMethod()]
        public void addDataItemTest()
        {
            string result = new ZY_WR_STAT_A().addDataItem("");
        }
    }
}