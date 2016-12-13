using KunTaiServiceLibrary.valueObjects;
using System;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Warrior.DataAccessUtils.Desktop;

namespace KunTaiServiceLibrary
{
    public class Weather : IController
    {
        #region command texts


        private const string weather_insert_commandText = "INSERT INTO [WEATHER] ([OID],[DATETIME],[IMAGE],[TEMPERATURE],[ALERT],[WIND],[HUMIDITY],[MORNINGINDEX],[CLADINDEX],[CARWASHINDEX],[DAY0TYPE],[DAY0VALUE],[DAY0WINDLEVEL],[DAY1TYPE],[DAY1VALUE],[DAY1WINDLEVEL]) VALUES (@OID, @DATETIME, @IMAGE, @TEMPERATURE, @ALERT, @WIND, @HUMIDITY, @MORNINGINDEX, @CLADINDEX, @CARWASHINDEX, @DAY0TYPE, @DAY0VALUE, @DAY0WINDLEVEL, @DAY1TYPE, @DAY1VALUE, @DAY1WINDLEVEL)";

        private const string weather_insert_items_commandText = "INSERT INTO [WEATHER] ([OID],[DATETIME],[IMAGE],[TEMPERATURE],[ALERT],[WIND],[HUMIDITY],[MORNINGINDEX],[CLADINDEX],[CARWASHINDEX],[DAY0TYPE],[DAY0VALUE],[DAY0WINDLEVEL],[DAY1TYPE],[DAY1VALUE],[DAY1WINDLEVEL]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}');\r\n";

        private const string organization_list_commandText = "SELECT [ID], [NAME], [CITYID] FROM [ORGANIZATION] WHERE [CITYID] IS NOT NULL ORDER BY [SHOWID]";

        private const string weather_update_time_1_commandText = "SELECT TOP 1 [ID],[NAME],[TIME] FROM [WEATHERTIME] WHERE CONVERT(time,'{0}')<[TIME]{1} ORDER BY [TIME]";
        private const string weather_update_time_2_commandText = "SELECT TOP 1 [ID],[NAME],[TIME] FROM [WEATHERTIME] WHERE CONVERT(time,'{0}')>[TIME]{1} ORDER BY [TIME]";


        #endregion



        public string getUpdateWeatherNextTime(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Result.getFaultXml(Error.XML_IS_NULL);

            XElement xml = null;
            try
            {
                xml = XElement.Parse(text);
            }
            catch
            {
                return Result.getFaultXml(Error.XML_FORMAT_ERROR);
            }

            string whereText = string.IsNullOrEmpty(xml.Element("LASTTIME").Value)
                ? string.Empty
                : string.Format(" AND [TIME]!=CONVERT(time,'{0}')", xml.Element("LASTTIME").Value);

            DataSet dataSetWeather = null;
            try
            {
                dataSetWeather = new DataAccessHandler().executeDatasetResult(
                    string.Format(weather_update_time_1_commandText, xml.Element("TIME").Value, whereText), null);

                if (dataSetWeather != null && dataSetWeather.Tables.Count > 0 && dataSetWeather.Tables[0].Rows.Count <= 0)
                {
                    dataSetWeather = new DataAccessHandler().executeDatasetResult(
                        string.Format(weather_update_time_2_commandText, xml.Element("TIME").Value, whereText), null);
                }
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;

            if (dataSetWeather != null && dataSetWeather.Tables.Count > 0)
            {
                result = Result.getResultXml(getUpdateWeatherNextTimeXml(ref dataSetWeather));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = null;
            xml = null;
            dataSetWeather = null;

            return result;
        }

        private string getUpdateWeatherNextTimeXml(ref DataSet dataSetWeather)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            WeatherTimeObject weatherTimeObject = null;
            foreach (DataRow row in dataSetWeather.Tables[0].Rows)
            {
                weatherTimeObject = new WeatherTimeObject(row);
                xml.AppendFormat("<DATA ID=\"{0}\" NAME=\"{1}\" TIME=\"{2}\"/>",
                    weatherTimeObject.ID,
                    weatherTimeObject.NAME,
                    weatherTimeObject.TIME
                );
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }

        public string getOrganizationList()
        {
            DataSet dataSetOrganization = null;
            try
            {
                dataSetOrganization = new DataAccessHandler().executeDatasetResult(
                    organization_list_commandText, null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetOrganization != null && dataSetOrganization.Tables.Count > 0)
            {
                result = Result.getResultXml(getOrganizationListXml(ref dataSetOrganization));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }


            dataSetOrganization = null;

            return result;
        }

        private string getOrganizationListXml(ref DataSet dataSetOrganization)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            OrganizationObject organizationObject = null;
            foreach (DataRow row in dataSetOrganization.Tables[0].Rows)
            {
                organizationObject = new OrganizationObject(row);
                xml.AppendFormat("<DATA ID=\"{0}\" NAME=\"{1}\" CITYID=\"{2}\"/>",
                    organizationObject.ID,
                    organizationObject.NAME,
                    organizationObject.CITYID
                );
            }
            xml.Append("</DATAS>");

            organizationObject = null;

            return xml.ToString();
        }


        /// <summary>
        /// 同时添加多个天气信息
        /// </summary>
        public string addDataItems(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Result.getFaultXml(Error.XML_IS_NULL);

            XElement xml = null;
            try
            {
                xml = XElement.Parse(text);
            }
            catch
            {
                return Result.getFaultXml(Error.XML_FORMAT_ERROR);
            }


            StringBuilder commandText = new StringBuilder();

            WeatherObject weatherObject = null;
            foreach (XElement item in xml.Elements("WEATHER"))
            {
                weatherObject = new WeatherObject(item.ToString());

                commandText.AppendFormat(
                    weather_insert_items_commandText,
                    weatherObject.OID,
                    weatherObject.DATETIME,
                    weatherObject.IMAGE,
                    weatherObject.TEMPERATURE,
                    weatherObject.ALERT,
                    weatherObject.WIND,
                    weatherObject.HUMIDITY,
                    weatherObject.MORNINGINDEX,
                    weatherObject.CLADINDEX,
                    weatherObject.CARWASHINDEX,
                    weatherObject.TODAY.Count == 2 ? weatherObject.TODAY[0].TYPE : string.Empty,
                    weatherObject.TODAY.Count == 2 ? weatherObject.TODAY[0].VALUE : string.Empty,
                    weatherObject.TODAY.Count == 2 ? weatherObject.TODAY[0].WINDLEVEL : string.Empty,
                    weatherObject.TODAY[1].TYPE,
                    weatherObject.TODAY[1].VALUE,
                    weatherObject.TODAY[1].WINDLEVEL
                );
            }

            string result = string.Empty;
            if (commandText.Length > 0)
            {
                //commandText.Insert(0, "BEGIN\r\n");
                //commandText.Append("END;");
                try
                {
                    result = new DataAccessHandler().executeNonQueryResult(commandText.ToString(), null);
                }
                catch (Exception ex)
                {
                    return Result.getFaultXml(ex.Message);
                }
            }

            text = null;
            xml = null;
            commandText = null;
            weatherObject = null;

            return result;
        }


        public string addDataItem(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Result.getFaultXml(Error.XML_IS_NULL);

            XElement xml = null;
            try
            {
                xml = XElement.Parse(text);
            }
            catch
            {
                return Result.getFaultXml(Error.XML_FORMAT_ERROR);
            }

            string result = string.Empty;

            try
            {
                result = new DataAccessHandler().executeNonQueryResult(
                    weather_insert_commandText,
                    SqlServer.GetParameter(xml,
                    new string[] {
                        "OID", "DATETIME", "IMAGE",
                        "TEMPERATURE", "ALERT", "WIND", "HUMIDITY",
                        "MORNINGINDEX", "CLADINDEX", "CARWASHINDEX",
                        "DAY0TYPE", "DAY0VALUE", "DAY0WINDLEVEL",
                        "DAY1TYPE", "DAY1VALUE", "DAY1WINDLEVEL" }
                    )
                );
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = null;
            xml = null;

            return result;
        }

        public string deleteDataItem(string text)
        {
            throw new NotImplementedException();
        }

        public string editDataItem(string text)
        {
            throw new NotImplementedException();
        }

        public string getDataItem(string text)
        {
            throw new NotImplementedException();
        }

        public string getDataItemDetails(string text)
        {
            throw new NotImplementedException();
        }
    }
}
