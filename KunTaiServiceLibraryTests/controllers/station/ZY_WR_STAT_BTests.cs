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
    public class ZY_WR_STAT_BTests
    {
        [TestMethod()]
        public void addDataItemTest()
        {

            string text = "<STATION><ID>0bfe20de-5c61-47d2-88ed-ace3220c0b45</ID><STCD>00000001</STCD><T_CODE>000000010001</T_CODE><ST_NM>文化新村</ST_NM><TYPE>二级</TYPE><AREA>86644.35</AREA><POWER>7.5</POWER><EFFICIENCY>NULL</EFFICIENCY><FLOW>NULL</FLOW><SPEED>NULL</SPEED><FREQUENCY>NULL</FREQUENCY><NOTE></NOTE></STATION>";

            string result = new ZY_WR_STAT_B().editDataItem(text);
        }

    }
}