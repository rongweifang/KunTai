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
    public class LogTests
    {
        [TestMethod()]
        public void writeTest()
        {

            Log.write(new valueObjects.LogObject()
            {
                SOURCE = "LogTests1",
                METHODNAME = "writeTest1",
                LOGLEVEL = LogType.Information,
                MESSAGE = "测试日志写入功能1",
                OPERATIONID = System.Guid.NewGuid().ToString()
            });
        }
    }
}