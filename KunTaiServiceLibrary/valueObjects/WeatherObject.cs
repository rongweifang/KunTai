using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;

namespace KunTaiServiceLibrary.valueObjects
{
    public class WeatherObject
    {

        /// <summary>
        /// 数据库编号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 组织机构编号
        /// </summary>
        public string OID { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public string ADDDATETIME { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string DATETIME { get; set; }

        /// <summary>
        /// 天气图标
        /// </summary>
        public string IMAGE { get; set; }

        /// <summary>
        /// 实时天气
        /// </summary>
        public string TEMPERATURE { get; set; }

        /// <summary>
        /// 预警情况
        /// </summary>
        public string ALERT { get; set; }

        /// <summary>
        /// 风力
        /// </summary>
        public string WIND { get; set; }

        /// <summary>
        /// 湿度
        /// </summary>
        public string HUMIDITY { get; set; }

        /// <summary>
        /// 晨练指数
        /// </summary>
        public string MORNINGINDEX { get; set; }

        /// <summary>
        /// 穿衣指数
        /// </summary>
        public string CLADINDEX { get; set; }

        /// <summary>
        /// 洗车指数
        /// </summary>
        public string CARWASHINDEX { get; set; }

        /// <summary>
        /// 今天详细情况。[0]-白天；[1]-夜晚
        /// </summary>
        public List<Today> TODAY { get; set; }


        public WeatherObject()
        {

        }


        public WeatherObject(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new Exception("天气信息的内容不能为空");

            XElement xml = null;
            try
            {
                xml = XElement.Parse(text);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("解析天气信息失败。错误：{0}", ex.Message));
            }

            this.ID = xml.Element("ID") == null ? string.Empty : xml.Element("ID").Value.Trim();
            this.OID = xml.Element("OID") == null ? string.Empty : xml.Element("OID").Value.Trim();
            this.ADDDATETIME = xml.Element("ADDDATETIME") == null ? string.Empty : xml.Element("ADDDATETIME").Value.Trim();

            this.DATETIME = xml.Element("DATETIME") == null ? string.Empty : xml.Element("DATETIME").Value.Trim();
            this.IMAGE = xml.Element("IMAGE") == null ? string.Empty : xml.Element("IMAGE").Value.Trim();
            this.TEMPERATURE = xml.Element("TEMPERATURE") == null ? string.Empty : xml.Element("TEMPERATURE").Value.Trim();
            this.ALERT = xml.Element("ALERT") == null ? string.Empty : xml.Element("ALERT").Value.Trim();
            this.WIND = xml.Element("WIND") == null ? string.Empty : xml.Element("WIND").Value.Trim();
            this.HUMIDITY = xml.Element("HUMIDITY") == null ? string.Empty : xml.Element("HUMIDITY").Value.Trim();
            this.MORNINGINDEX = xml.Element("MORNINGINDEX") == null ? string.Empty : xml.Element("MORNINGINDEX").Value.Trim();
            this.CLADINDEX = xml.Element("CLADINDEX") == null ? string.Empty : xml.Element("CLADINDEX").Value.Trim();
            this.CARWASHINDEX = xml.Element("CARWASHINDEX") == null ? string.Empty : xml.Element("CARWASHINDEX").Value.Trim();

            XElement day0XML = xml.Element("TODAY").Element("DAY0");
            XElement day1XML = xml.Element("TODAY").Element("DAY1");

            List<Today> listToday = new List<Today>()
            {
                //白天
                new Today() {
                    TYPE = day0XML ==null || day0XML.Attribute("TYPE") == null ? string.Empty : day0XML.Attribute("TYPE").Value.Trim(),
                    VALUE = day0XML ==null || day0XML.Attribute("VALUE") == null ? string.Empty : day0XML.Attribute("VALUE").Value.Trim(),
                    WINDLEVEL = day0XML ==null || day0XML.Attribute("WINDLEVEL") == null ? string.Empty : day0XML.Attribute("WINDLEVEL").Value.Trim() },
                //夜晚
                new Today()
                {
                    TYPE = day1XML.Attribute("TYPE") == null ? string.Empty : day1XML.Attribute("TYPE").Value.Trim(),
                    VALUE = day1XML.Attribute("VALUE") == null ? string.Empty : day1XML.Attribute("VALUE").Value.Trim(),
                    WINDLEVEL = day1XML.Attribute("WINDLEVEL") == null ? string.Empty : day1XML.Attribute("WINDLEVEL").Value.Trim()
                }
            };

            this.TODAY = listToday;

            xml = day0XML = day1XML = null;
            listToday = null;

        }

    }

    /// <summary>
    /// 天气详细内容
    /// </summary>
    public class Today
    {
        /// <summary>
        /// 天气类型
        /// </summary>
        public string TYPE { get; set; }

        /// <summary>
        /// 温度值
        /// </summary>
        public string VALUE { get; set; }

        /// <summary>
        /// 风向及级别。格式：风向 级别
        /// </summary>
        public string WINDLEVEL { get; set; }

    }
}
