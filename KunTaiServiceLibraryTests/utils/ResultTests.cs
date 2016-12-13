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
    public class ResultTests
    {
        [TestMethod()]
        public void getResultXmlTest()
        {
            string guid = System.Guid.NewGuid().ToString();

            Debug.WriteLine(guid);
        }
    }
}