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
    public class ServicesTests
    {
        [TestMethod()]
        public void getDBConnectionStateTest()
        {
            new Services().getDBConnectionState();
        }
    }
}