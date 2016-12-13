using KunTaiServiceLibrary.valueObjects;
using System;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Warrior.DataAccessUtils.Desktop;

namespace KunTaiServiceLibrary
{
    [FluorineFx.RemotingService("换热站服务")]
    public class Station : IController
    {

        #region command texts

        private const string station_insert_commandText = "INSERT INTO [STATION] ([OID], [NAME], [AREA], [CYCLEPOWER], [CYCLEEFFICIENCY], [CYCLEFLOW], [WATERPOWER], [WATEREFFICIENCY], [WATERFLOW], [TEMPERATURE], [HEATLOAD], [NOTE]) VALUES (@OID, @NAME, @AREA, @CYCLEPOWER, @CYCLEEFFICIENCY, @CYCLEFLOW, @WATERPOWER, @WATEREFFICIENCY, @WATERFLOW, @TEMPERATURE, @HEATLOAD, @NOTE)";

        private const string station_update_commandText = "UPDATE [STATION] SET [OID]=@OID, [NAME]=@NAME, [AREA]=@AREA, [CYCLEPOWER]=@CYCLEPOWER, [CYCLEEFFICIENCY]=@CYCLEEFFICIENCY, [CYCLEFLOW]=@CYCLEFLOW, [WATERPOWER]=@WATERPOWER, [WATEREFFICIENCY]=@WATEREFFICIENCY, [WATERFLOW]=@WATERFLOW, [TEMPERATURE]=@TEMPERATURE, [HEATLOAD]=@HEATLOAD, [NOTE]=@NOTE WHERE [ID]=@ID";

        private const string station_delete_commandText = "DELETE [STATION] WHERE [ID] IN ({0})";

        private const string station_select_commandText = "SELECT ROW_NUMBER() OVER(ORDER BY [SHOWID], [NAME]) AS NUM1, [ID], [OID], [NAME], [AREA], [CYCLEPOWER], [CYCLEEFFICIENCY], [CYCLEFLOW], [WATERPOWER], [WATEREFFICIENCY], [WATERFLOW], [TEMPERATURE], [HEATLOAD] FROM [STATION]{0}";

        private const string station_total_commandText = "SELECT COUNT(*) FROM [STATION]{0}";

        private const string station_details_commandText = "SELECT [ID], [OID], [NAME], [AREA], [CYCLEPOWER], [CYCLEEFFICIENCY], [CYCLEFLOW], [WATERPOWER], [WATEREFFICIENCY], [WATERFLOW], [TEMPERATURE], [HEATLOAD], [NOTE] FROM [STATION] WHERE [ID]=@ID";


        #endregion

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
                    station_insert_commandText,
                    SqlServer.GetParameter(xml, new string[] {
                        "OID", "NAME", "AREA",
                        "CYCLEPOWER", "CYCLEEFFICIENCY", "CYCLEFLOW",
                        "WATERPOWER", "WATEREFFICIENCY", "WATERFLOW",
                        "TEMPERATURE", "HEATLOAD", "NOTE" }));
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
                    string.Format(station_delete_commandText, xml.Element("ID").Value), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = null;
            xml = null;

            return result;
        }

        public string editDataItem(string text)
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
                    station_update_commandText,
                    SqlServer.GetParameter(xml, new string[] {
                        "ID", "OID", "NAME", "AREA",
                        "CYCLEPOWER", "CYCLEEFFICIENCY", "CYCLEFLOW",
                        "WATERPOWER", "WATEREFFICIENCY", "WATERFLOW",
                        "TEMPERATURE", "HEATLOAD", "NOTE" }));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = null;
            xml = null;

            return result;
        }

        public string getDataItem(string text)
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

            string whereText = string.IsNullOrEmpty(xml.Element("WHERE").Value)
                ? string.Empty
                : string.Format(" WHERE {0}", xml.Element("WHERE").Value);
            string commandText = string.Format(station_select_commandText, whereText);

            DataSet dataSetStation = null;
            try
            {
                dataSetStation = new DataAccessHandler().executeDatasetResult(
                    SqlServerCommandText.createPagingCommandText(ref commandText, ref xml), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;

            if (dataSetStation != null && dataSetStation.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemXml(ref dataSetStation, getDataItemTotal(ref whereText)));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = whereText = commandText = null;
            xml = null;
            dataSetStation = null;

            return result;
        }

        private string getDataItemXml(ref DataSet dataSetStation, string total)
        {
            StringBuilder xml = new StringBuilder();
            StationObject stationObject = null;
            xml.AppendFormat("<DATAS COUNT=\"{0}\" TOTAL=\"{1}\">", dataSetStation.Tables[0].Rows.Count, total);
            foreach (DataRow row in dataSetStation.Tables[0].Rows)
            {
                stationObject = new StationObject(row);

                xml.AppendFormat("<DATA NUM=\"{0}\" ID=\"{1}\" OID=\"{2}\" NAME=\"{3}\" AREA=\"{4}\" CYCLEPOWER=\"{5}\" CYCLEEFFICIENCY=\"{6}\" CYCLEFLOW=\"{7}\" WATERPOWER=\"{8}\" WATEREFFICIENCY=\"{9}\" WATERFLOW=\"{10}\" TEMPERATURE=\"{11}\" HEATLOAD=\"{12}\"/>",
                    stationObject.NUM,
                    stationObject.ID,
                    stationObject.OID,
                    stationObject.NAME,
                    stationObject.AREA,
                    stationObject.CYCLEPOWER,
                    stationObject.CYCLEEFFICIENCY,
                    stationObject.CYCLEFLOW,
                    stationObject.WATERFLOW,
                    stationObject.WATEREFFICIENCY,
                    stationObject.WATERFLOW,
                    stationObject.TEMPERATURE,
                    stationObject.HEATLOAD
                );
            }
            xml.Append("</DATAS>");

            total = null;

            return xml.ToString();
        }

        private string getDataItemTotal(ref string whereText)
        {
            return new DataAccessHandler().executeScalarResult(
                string.Format(station_total_commandText, whereText), null);
        }

        public string getDataItemDetails(string text)
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

            DataSet dataSetStation = null;
            try
            {
                dataSetStation = new DataAccessHandler().executeDatasetResult(
                    station_details_commandText,
                    SqlServer.GetParameter("ID", xml.Element("ID").Value));
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetStation != null && dataSetStation.Tables.Count > 0)
            {
                result = Result.getResultXml(getDataItemDetailsXml(ref dataSetStation));
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = null;
            xml = null;
            dataSetStation = null;

            return result;
        }

        private string getDataItemDetailsXml(ref DataSet dataSetStation)
        {

            StringBuilder xml = new StringBuilder();
            StationObject stationObject = null;
            xml.Append("<DATAS>");
            foreach (DataRow row in dataSetStation.Tables[0].Rows)
            {
                stationObject = new StationObject(row);

                xml.AppendFormat("<DATA ID=\"{0}\" OID=\"{1}\" NAME=\"{2}\" AREA=\"{3}\" CYCLEPOWER=\"{4}\" CYCLEEFFICIENCY=\"{5}\" CYCLEFLOW=\"{6}\" WATERPOWER=\"{7}\" WATEREFFICIENCY=\"{8}\" WATERFLOW=\"{9}\" TEMPERATURE=\"{10}\" HEATLOAD=\"{11}\" NOTE=\"{12}\"/>",
                    stationObject.ID,
                    stationObject.OID,
                    stationObject.NAME,
                    stationObject.AREA,
                    stationObject.CYCLEPOWER,
                    stationObject.CYCLEEFFICIENCY,
                    stationObject.CYCLEFLOW,
                    stationObject.WATERFLOW,
                    stationObject.WATEREFFICIENCY,
                    stationObject.WATERFLOW,
                    stationObject.TEMPERATURE,
                    stationObject.HEATLOAD,
                    stationObject.NOTE
                );
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }


        public string getParameters()
        {
            return new Organization().getOrganizationList(true);
        }

    }
}
