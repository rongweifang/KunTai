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
    public class ZY_PUMPVALUETests
    {
        [TestMethod()]
        public void getDataItemTest()
        {
            string result = new ZY_PumpValue().getDataItem("<PUMPVALUE><DATETIME>2016-01-01</DATETIME><NAME></NAME></PUMPVALUE>");
        }
    }
}