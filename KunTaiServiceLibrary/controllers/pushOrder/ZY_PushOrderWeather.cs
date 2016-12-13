using KunTaiServiceLibrary.valueObjects;
using System;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Warrior.DataAccessUtils.Desktop;

namespace KunTaiServiceLibrary
{
    [FluorineFx.RemotingService("金城数控供热，六区间设备运行百分比自动生成模板")]
    public class ZY_PushOrderWeather
    {

        #region Command texts

        private const string getWeatherUrlCommandText = "SELECT URL FROM WEATHERURL WHERE NAME=@NAME";



        #endregion





        #region 获取天气情况，每个时间点都要得知温度

        //获取一天内时间点的天气情况。
        //从0点到23点
        public string getTimeWeather(string text)
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

            string cityName = string.Empty;
            cityName = xml.Element("CITYNAME").Value;// 城市名称（如招远金城，呼和浩特坤泰）


            string url = string.Empty;

            Weather24Object weather24Object = null;
            try
            {
                url = new DataAccessHandler().executeScalarResult(
                    getWeatherUrlCommandText,
                    SqlServer.GetParameter("NAME", cityName));

                if (string.IsNullOrEmpty(url))
                    throw new Exception(string.Format("该城市({0})没有配置相应的查询天气的URL", cityName));

                //解析内容到对象上
                weather24Object = new Weather24Object(WeatherUtils.getWebContent(url), WeatherType.Json);
                if (weather24Object == null)
                    throw new Exception("解析天气页码出现错误。");

                url = null;
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            StringBuilder result = new StringBuilder();
            result.Append("<DATAS>");
            result.AppendFormat("<DATA H00=\"{0}\"/>", weather24Object.h00);
            result.AppendFormat("<DATA H01=\"{0}\"/>", weather24Object.h01);
            result.AppendFormat("<DATA H02=\"{0}\"/>", weather24Object.h02);
            result.AppendFormat("<DATA H03=\"{0}\"/>", weather24Object.h03);
            result.AppendFormat("<DATA H04=\"{0}\"/>", weather24Object.h04);
            result.AppendFormat("<DATA H05=\"{0}\"/>", weather24Object.h05);
            result.AppendFormat("<DATA H06=\"{0}\"/>", weather24Object.h06);
            result.AppendFormat("<DATA H07=\"{0}\"/>", weather24Object.h07);
            result.AppendFormat("<DATA H08=\"{0}\"/>", weather24Object.h08);
            result.AppendFormat("<DATA H09=\"{0}\"/>", weather24Object.h09);
            result.AppendFormat("<DATA H10=\"{0}\"/>", weather24Object.h10);
            result.AppendFormat("<DATA H11=\"{0}\"/>", weather24Object.h11);
            result.AppendFormat("<DATA H12=\"{0}\"/>", weather24Object.h12);
            result.AppendFormat("<DATA H13=\"{0}\"/>", weather24Object.h13);
            result.AppendFormat("<DATA H14=\"{0}\"/>", weather24Object.h14);
            result.AppendFormat("<DATA H15=\"{0}\"/>", weather24Object.h15);
            result.AppendFormat("<DATA H16=\"{0}\"/>", weather24Object.h16);
            result.AppendFormat("<DATA H17=\"{0}\"/>", weather24Object.h17);
            result.AppendFormat("<DATA H18=\"{0}\"/>", weather24Object.h18);
            result.AppendFormat("<DATA H19=\"{0}\"/>", weather24Object.h19);
            result.AppendFormat("<DATA H20=\"{0}\"/>", weather24Object.h20);
            result.AppendFormat("<DATA H21=\"{0}\"/>", weather24Object.h21);
            result.AppendFormat("<DATA H22=\"{0}\"/>", weather24Object.h22);
            result.AppendFormat("<DATA H23=\"{0}\"/>", weather24Object.h23);
            result.Append("</DATAS>");

            cityName = null;
            weather24Object = null;

            return Result.getResultXml(result.ToString());
        }

        #endregion



        public string getRunPercentage(string text)
        {
            return calculateRunPercentage(getTimeWeather(text));
        }


        public string resetCalculate(string text)
        {
            if (!text.Contains("OPERATION"))
            {
                text = Result.getResultXml(text);
            }

            return calculateRunPercentage(text);
        }



        #region 计算运行百分比

        public string calculateRunPercentage(string text)
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


            if (xml.Attribute("OPERATION").Value == "FAILURE")
                return text;



            DataTable dataTableRunPercentage = null;
            createRunPercentageDataTable(ref xml, ref dataTableRunPercentage);

            if (dataTableRunPercentage == null)
                return Result.getFaultXml("初始化内存数据表时出错。");

            string result = string.Empty;

            result = Result.getResultXml(getCalculateRunPercentageXml(ref dataTableRunPercentage));

            text = null;
            xml = null;
            dataTableRunPercentage = null;

            return result;
        }

        private string getCalculateRunPercentageXml(ref DataTable dataTableRunPercentage)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<DATAS>");
            ZY_PushOrderWeatherObject zy_pushorderweatherObject = null;
            foreach (DataRow row in dataTableRunPercentage.Rows)
            {
                zy_pushorderweatherObject = new ZY_PushOrderWeatherObject(row);
                xml.AppendFormat("<DATA NUM=\"{0}\" ID=\"{1}\" ADDDATETIME=\"{2}\" TIMEPOINT=\"{3}\" W36=\"{4}\" W401=\"{5}\" REVISE=\"{6}\" PERCENTAGE=\"{7}\"/>",
                    zy_pushorderweatherObject.NUM,
                    zy_pushorderweatherObject.ID,
                    zy_pushorderweatherObject.ADDDATETIME,
                    zy_pushorderweatherObject.TIMEPOINT,
                    zy_pushorderweatherObject.W36,
                    zy_pushorderweatherObject.W401,
                    zy_pushorderweatherObject.REVISE,
                    zy_pushorderweatherObject.PERCENTAGE
                );

                zy_pushorderweatherObject = null;
            }
            xml.Append("</DATAS>");

            return xml.ToString();
        }

        private void createRunPercentageDataTable(ref XElement xml, ref DataTable dataTable)
        {
            dataTable = new DataTable();

            dataTable.Columns.Add("NUM", typeof(string));
            dataTable.Columns.Add("ADDDATETIME", typeof(string));
            dataTable.Columns.Add("TIMEPOINT", typeof(string));
            dataTable.Columns.Add("W36", typeof(double));
            dataTable.Columns.Add("W401", typeof(double));
            dataTable.Columns.Add("REVISE", typeof(double));
            dataTable.Columns.Add("PERCENTAGE", typeof(double));

            DataRow dataRowNew = null;
            int numIndex = 1;

            string addDateTime = System.DateTime.Now.ToString("yyyy-MM-dd");

            foreach (XElement item in xml.Element("DATAS").Elements("DATA"))
            {
                dataRowNew = dataTable.NewRow();
                switch (item.FirstAttribute.Name.ToString())
                {
                    case "H00":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H01":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H02":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H03":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H04":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H05":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H06":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H07":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H08":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H09":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H10":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H11":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H12":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H13":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H14":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H15":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H16":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H17":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H18":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H19":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H20":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H21":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H22":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    case "H23":
                        dataRowNew["TIMEPOINT"] = item.FirstAttribute.Name.ToString();
                        dataRowNew["W36"] = item.Attribute(dataRowNew["TIMEPOINT"].ToString()).Value;
                        dataRowNew["W401"] = w401(dataRowNew["W36"].ToString());
                        dataRowNew["REVISE"] = revise(dataRowNew["W401"].ToString());
                        break;
                    default:
                        break;
                }
                dataRowNew["NUM"] = numIndex;
                dataRowNew["ADDDATETIME"] = addDateTime;
                dataTable.Rows.Add(dataRowNew);
                numIndex++;

                dataRowNew = null;
            }


            //计算运行比例（在这里计算时，一定要注意使用原始值计算完后再进行四舍五入，否则会少一些值）
            double w401Sum = Convert.ToDouble(dataTable.Compute("SUM(W401)", string.Empty));
            double reviseSum = this.reviseSum(ref w401Sum);

            w401Sum = Math.Round(w401Sum, 1, MidpointRounding.AwayFromZero);
            reviseSum = Math.Round(reviseSum, 2, MidpointRounding.AwayFromZero);

            //计算每组几个数字和
            int areaCount = dataTable.Rows.Count / 6;
            int index = areaCount;
            double groupSum = 0d;
            DataRow dataRow = null;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dataRow = dataTable.Rows[i];

                groupSum += Convert.ToDouble(dataRow["REVISE"]);
                groupSum = Math.Round(groupSum, 2, MidpointRounding.AwayFromZero);

                //计算每个分区段内的值
                if (i % index == 3)
                {
                    dataTable.Rows[i]["PERCENTAGE"] = percentage(ref groupSum, ref reviseSum);
                    groupSum = 0d;
                }
            }
            //for (int i = 0; i < dataTable.Rows.Count; i++)
            //{
            //    dataRow = dataTable.Rows[i];
            //    //计算每个分区段内的值
            //    if (index == i)
            //    {
            //        dataTable.Rows[index - 1]["PERCENTAGE"] = percentage(ref groupSum, ref reviseSum);

            //        index += areaCount;
            //        groupSum = 0d;
            //    }

            //    groupSum += Convert.ToDouble(dataRow["REVISE"]);
            //}

            //处理最后一个组
            //if (groupSum > 0)
            //{
            //    dataTable.Rows[dataTable.Rows.Count - 1]["PERCENTAGE"] = percentage(ref groupSum, ref reviseSum);

            //    index = 0;
            //    groupSum = 0d;
            //}

            dataRow = null;

            // 这里的percentageSum == 1 的话，则说明，运算正确。
            //double percentageSum = Convert.ToDouble(dataTable.Compute("SUM(PERCENTAGE)", string.Empty));


            /*
            //打印一份
            StringBuilder result = new StringBuilder();
            foreach (DataRow row in dataTable.Rows)
            {
                result.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\r\n",
                    row["TIMEPOINT"].ToString(),
                    row["W36"].ToString(),
                    row["W401"].ToString(),
                    row["REVISE"].ToString(),
                    row["PERCENTAGE"].ToString()
                );
            }
            string temp = result.ToString();
            */
        }

        private double percentage(ref double groupSum, ref double reviseSum)
        {
            //groupSum = Math.Round(groupSum, 2, MidpointRounding.AwayFromZero);
            return Math.Round(groupSum / reviseSum, 2, MidpointRounding.AwayFromZero); ;
        }

        private double reviseSum(ref double w401Sum)
        {
            //=401/354.9*D27
            return 401 / 354.9d * w401Sum;
        }

        private double w401(string w36)
        {
            //401W=(18/28-C3/28)*36
            return (18 / 28d - Convert.ToDouble(w36) / 28d) * 36d;
        }


        private double revise(string w401)
        {
            //=401/354.9*D3
            return 401 / 354.9d * Convert.ToDouble(w401);
        }


        #endregion







        #region 添加设备运行百分比

        public string addRunPercentage(string text)
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





            return "";
        }

        #endregion



    }
}
