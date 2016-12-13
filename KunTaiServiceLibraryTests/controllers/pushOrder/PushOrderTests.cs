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
    public class PushOrderTests
    {
        [TestMethod()]
        public void getPushOrderReceiveItemTest()
        {

            string result = new PushOrder().getDownFileName("<PUSHORDER><FILENAME></FILENAME><FILES><FILE FILEURL=\"1CA13561-1CF2-465F-9AE2-C9EEEE545663.ppt\" LOCALFILENAME=\"启明星宇大屏幕.ppt\"/></FILES></PUSHORDER>");

        }
    }
}