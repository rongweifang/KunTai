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
    public class ZY_PushOrderWeatherTests
    {
        [TestMethod()]
        public void getTimeWeatherTest()
        {
            string text = "<XML><CITYNAME>招远金城</CITYNAME></XML>";
            string result = new ZY_PushOrderWeather().getTimeWeather(text);

            string temp = result;
        }
    }
}