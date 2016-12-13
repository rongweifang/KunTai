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
    public class Log2Tests
    {
        [TestMethod()]
        public void wirteTest()
        {
            Log2.wirte("sdfsdfs");
            Log2.wirte("sdfsdfs1");

            Log2.wirte("sdfsdfs2");
            Log2.wirte("sdfsdfs3");
            Log2.wirte("sdfsdfs4");
        }

    }
}