using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace KunTaiServiceLibrary.Tests
{
    [TestClass()]
    public class WeatherTests
    {
        [TestMethod()]
        public void addDataItemsTest()
        {

            new Weather().getUpdateWeatherNextTime("<WEATHER><TIME>23:06:25</TIME><LASTTIME></LASTTIME></WEATHER>");


            string text = "<WEATHERS><WEATHER><OID>01a602cc-0210-4fd8-85e8-1a03e641af1b</OID><DATETIME>2015-10-26 18:00:00</DATETIME><IMAGE>Night00.png</IMAGE><TEMPERATURE>晴 4.7℃</TEMPERATURE><ALERT>无预警</ALERT><WIND>西风微风</WIND><HUMIDITY>30.0%</HUMIDITY><MORNINGINDEX>2</MORNINGINDEX><CLADINDEX>5</CLADINDEX><CARWASHINDEX>2</CARWASHINDEX><TODAY><DAY1 TYPE=\"晴\" VALUE=\" - 3\" WINDLEVEL=\"西风 3 - 4级\"/></TODAY></WEATHER><WEATHER><OID>4f4fe095-b28b-46b2-80f4-27993e8dec4a</OID><DATETIME>2015-10-26 18:00:00</DATETIME><IMAGE>Night18.png</IMAGE><TEMPERATURE>雾 11.6℃</TEMPERATURE><ALERT>无预警</ALERT><WIND>西北风微风</WIND><HUMIDITY>93.0%</HUMIDITY><MORNINGINDEX>2</MORNINGINDEX><CLADINDEX>5</CLADINDEX><CARWASHINDEX>3</CARWASHINDEX><TODAY><DAY1 TYPE=\"阴\" VALUE=\"10\" WINDLEVEL=\"西北风 5 - 6级\"/></TODAY></WEATHER></WEATHERS>";

            string result = new Weather().addDataItems(text);

            Debug.WriteLine(result);


        }
    }
}