using System;
using System.Collections.Generic;

namespace KunTaiServiceLibrary.valueObjects
{

    public enum WeatherType
    {
        XML,
        Text,
        Json
    }


    public class Weather24Object
    {
        public string h00 { get; set; }
        public string h01 { get; set; }
        public string h02 { get; set; }
        public string h03 { get; set; }
        public string h04 { get; set; }
        public string h05 { get; set; }
        public string h06 { get; set; }
        public string h07 { get; set; }
        public string h08 { get; set; }
        public string h09 { get; set; }
        public string h10 { get; set; }
        public string h11 { get; set; }
        public string h12 { get; set; }
        public string h13 { get; set; }
        public string h14 { get; set; }
        public string h15 { get; set; }
        public string h16 { get; set; }
        public string h17 { get; set; }
        public string h18 { get; set; }
        public string h19 { get; set; }
        public string h20 { get; set; }
        public string h21 { get; set; }
        public string h22 { get; set; }
        public string h23 { get; set; }




        public Weather24Object(string text, WeatherType type = WeatherType.XML)
        {
            if (!string.IsNullOrEmpty(text))
            {
                switch (type)
                {
                    case WeatherType.XML:
                        analysisWebContentTextByXml(ref text);
                        break;
                    case WeatherType.Text:
                        break;
                    case WeatherType.Json:
                        analysisWebContentTextByJson(ref text);
                        break;
                    default:
                        break;
                }
            }
        }

        #region Json

        //http://www.weather.com.cn/weather/101120506.shtml

        private void analysisWebContentTextByJson(ref string text)
        {
            //只留取关键部分；
            string[] textItem = text.Replace(":[{", "|").Split('|');
            if (textItem.Length != 2)
                return;

            string temp = string.Empty;

            temp = textItem[1];
            textItem = temp.Replace("},{", "|").Split('|');

            for (int i = textItem.Length - 2; i >= 0; i--)
            {
                temp = textItem[i];

                setValueByJson(ref temp);

                temp = null;
            }

            //依次检查所有温度值是否有空值，如果存在，则使用前后的温度值。
            checkWeatherValue();

            textItem = null;

        }

        private void checkWeatherValue()
        {
            if (string.IsNullOrEmpty(this.h00))
            {
                if (!string.IsNullOrEmpty(this.h23))
                    this.h00 = this.h23;
                else if (!string.IsNullOrEmpty(this.h01))
                    this.h00 = this.h01;
            }

            if (string.IsNullOrEmpty(this.h01))
            {
                if (!string.IsNullOrEmpty(this.h00))
                    this.h01 = this.h00;
                else if (!string.IsNullOrEmpty(this.h02))
                    this.h01 = this.h02;
            }

            if (string.IsNullOrEmpty(this.h02))
            {
                if (!string.IsNullOrEmpty(this.h01))
                    this.h02 = this.h01;
                else if (!string.IsNullOrEmpty(this.h03))
                    this.h02 = this.h03;
            }

            if (string.IsNullOrEmpty(this.h03))
            {
                if (!string.IsNullOrEmpty(this.h02))
                    this.h03 = this.h02;
                else if (!string.IsNullOrEmpty(this.h04))
                    this.h03 = this.h04;
            }

            if (string.IsNullOrEmpty(this.h04))
            {
                if (!string.IsNullOrEmpty(this.h03))
                    this.h04 = this.h03;
                else if (!string.IsNullOrEmpty(this.h05))
                    this.h04 = this.h05;
            }

            if (string.IsNullOrEmpty(this.h05))
            {
                if (!string.IsNullOrEmpty(this.h04))
                    this.h05 = this.h04;
                else if (!string.IsNullOrEmpty(this.h06))
                    this.h05 = this.h06;
            }

            if (string.IsNullOrEmpty(this.h06))
            {
                if (!string.IsNullOrEmpty(this.h05))
                    this.h06 = this.h05;
                else if (!string.IsNullOrEmpty(this.h07))
                    this.h06 = this.h07;
            }

            if (string.IsNullOrEmpty(this.h07))
            {
                if (!string.IsNullOrEmpty(this.h06))
                    this.h07 = this.h06;
                else if (!string.IsNullOrEmpty(this.h08))
                    this.h07 = this.h08;
            }

            if (string.IsNullOrEmpty(this.h08))
            {
                if (!string.IsNullOrEmpty(this.h07))
                    this.h08 = this.h07;
                else if (!string.IsNullOrEmpty(this.h09))
                    this.h08 = this.h09;
            }

            if (string.IsNullOrEmpty(this.h09))
            {
                if (!string.IsNullOrEmpty(this.h08))
                    this.h09 = this.h08;
                else if (!string.IsNullOrEmpty(this.h10))
                    this.h09 = this.h10;
            }

            if (string.IsNullOrEmpty(this.h10))
            {
                if (!string.IsNullOrEmpty(this.h09))
                    this.h10 = this.h09;
                else if (!string.IsNullOrEmpty(this.h11))
                    this.h10 = this.h11;
            }

            if (string.IsNullOrEmpty(this.h11))
            {
                if (!string.IsNullOrEmpty(this.h10))
                    this.h11 = this.h10;
                else if (!string.IsNullOrEmpty(this.h12))
                    this.h11 = this.h12;
            }

            if (string.IsNullOrEmpty(this.h12))
            {
                if (!string.IsNullOrEmpty(this.h11))
                    this.h12 = this.h11;
                else if (!string.IsNullOrEmpty(this.h13))
                    this.h12 = this.h13;
            }

            if (string.IsNullOrEmpty(this.h13))
            {
                if (!string.IsNullOrEmpty(this.h12))
                    this.h13 = this.h12;
                else if (!string.IsNullOrEmpty(this.h14))
                    this.h13 = this.h14;
            }

            if (string.IsNullOrEmpty(this.h14))
            {
                if (!string.IsNullOrEmpty(this.h13))
                    this.h14 = this.h13;
                else if (!string.IsNullOrEmpty(this.h15))
                    this.h14 = this.h15;
            }

            if (string.IsNullOrEmpty(this.h15))
            {
                if (!string.IsNullOrEmpty(this.h14))
                    this.h15 = this.h14;
                else if (!string.IsNullOrEmpty(this.h16))
                    this.h15 = this.h16;
            }

            if (string.IsNullOrEmpty(this.h16))
            {
                if (!string.IsNullOrEmpty(this.h15))
                    this.h16 = this.h15;
                else if (!string.IsNullOrEmpty(this.h17))
                    this.h16 = this.h17;
            }

            if (string.IsNullOrEmpty(this.h17))
            {
                if (!string.IsNullOrEmpty(this.h16))
                    this.h17 = this.h16;
                else if (!string.IsNullOrEmpty(this.h18))
                    this.h17 = this.h18;
            }

            if (string.IsNullOrEmpty(this.h18))
            {
                if (!string.IsNullOrEmpty(this.h17))
                    this.h18 = this.h17;
                else if (!string.IsNullOrEmpty(this.h19))
                    this.h18 = this.h19;
            }

            if (string.IsNullOrEmpty(this.h19))
            {
                if (!string.IsNullOrEmpty(this.h18))
                    this.h19 = this.h18;
                else if (!string.IsNullOrEmpty(this.h20))
                    this.h19 = this.h20;
            }

            if (string.IsNullOrEmpty(this.h20))
            {
                if (!string.IsNullOrEmpty(this.h19))
                    this.h20 = this.h19;
                else if (!string.IsNullOrEmpty(this.h21))
                    this.h20 = this.h21;
            }

            if (string.IsNullOrEmpty(this.h21))
            {
                if (!string.IsNullOrEmpty(this.h20))
                    this.h21 = this.h20;
                else if (!string.IsNullOrEmpty(this.h22))
                    this.h21 = this.h22;
            }

            if (string.IsNullOrEmpty(this.h22))
            {
                if (!string.IsNullOrEmpty(this.h21))
                    this.h22 = this.h21;
                else if (!string.IsNullOrEmpty(this.h23))
                    this.h22 = this.h23;
            }

            if (string.IsNullOrEmpty(this.h23))
            {
                if (!string.IsNullOrEmpty(this.h22))
                    this.h23 = this.h22;
                else if (!string.IsNullOrEmpty(this.h00))
                    this.h23 = this.h00;
            }

        }


        private void setValueByJson(ref string text)
        {
            string[] tempItem = text.Replace("\":\"", "|").Replace("\",\"", "|").Split('|');

            string timePoint = tempItem[1].Trim();
            string weatherValue = tempItem[3].Trim();
            //1 - timePoint
            //3 - weatherValue
            if (string.IsNullOrEmpty(timePoint))
                return;

            switch (timePoint)
            {
                case "00":
                    this.h00 = weatherValue;
                    break;
                case "01":
                    this.h01 = weatherValue;
                    break;
                case "02":
                    this.h02 = weatherValue;
                    break;
                case "03":
                    this.h03 = weatherValue;
                    break;
                case "04":
                    this.h04 = weatherValue;
                    break;
                case "05":
                    this.h05 = weatherValue;
                    break;
                case "06":
                    this.h06 = weatherValue;
                    break;
                case "07":
                    this.h07 = weatherValue;
                    break;
                case "08":
                    this.h08 = weatherValue;
                    break;
                case "09":
                    this.h09 = weatherValue;
                    break;
                case "10":
                    this.h10 = weatherValue;
                    break;
                case "11":
                    this.h11 = weatherValue;
                    break;
                case "12":
                    this.h12 = weatherValue;
                    break;
                case "13":
                    this.h13 = weatherValue;
                    break;
                case "14":
                    this.h14 = weatherValue;
                    break;
                case "15":
                    this.h15 = weatherValue;
                    break;
                case "16":
                    this.h16 = weatherValue;
                    break;
                case "17":
                    this.h17 = weatherValue;
                    break;
                case "18":
                    this.h18 = weatherValue;
                    break;
                case "19":
                    this.h19 = weatherValue;
                    break;
                case "20":
                    this.h20 = weatherValue;
                    break;
                case "21":
                    this.h21 = weatherValue;
                    break;
                case "22":
                    this.h22 = weatherValue;
                    break;
                case "23":
                    this.h23 = weatherValue;
                    break;
                default:
                    break;
            }

            tempItem = null;

        }



        #endregion



        #region XML

        //http://www.22086.com/h_zhaoyuan1.html

        private void analysisWebContentTextByXml(ref string text)
        {
            //截取 招远24小时天气预报
            string startSubText = "<div class=\"zxs\">";

            int index = text.IndexOf(startSubText);
            if (index != -1)
            {
                text = text.Substring(index, text.Length - index);

                index = text.IndexOf("<div class=\"c-splitter\"></div>");
                if (index != -1)
                    text = text.Substring(0, index);

                //开始分析
                List<string> listLefs = null;
                analysisLefsText(ref listLefs, ref text);

                List<string> listRighs = null;
                analysisRighsText(ref listRighs, ref text);

                text = null;

                setValueByXml(ref listLefs, ref listRighs);
            }
        }


        private void setValueByXml(ref List<string> listLefs, ref List<string> listRighs)
        {
            string item = null;
            for (int i = 0; i < listLefs.Count; i++)
            {
                item = listLefs[i];

                switch (item)
                {
                    case "00时":
                        this.h00 = listRighs[i];
                        break;
                    case "01时":
                        this.h01 = listRighs[i];
                        break;
                    case "02时":
                        this.h02 = listRighs[i];
                        break;
                    case "03时":
                        this.h03 = listRighs[i];
                        break;
                    case "04时":
                        this.h04 = listRighs[i];
                        break;
                    case "05时":
                        this.h05 = listRighs[i];
                        break;
                    case "06时":
                        this.h06 = listRighs[i];
                        break;
                    case "07时":
                        this.h07 = listRighs[i];
                        break;
                    case "08时":
                        this.h08 = listRighs[i];
                        break;
                    case "09时":
                        this.h09 = listRighs[i];
                        break;
                    case "10时":
                        this.h10 = listRighs[i];
                        break;
                    case "11时":
                        this.h11 = listRighs[i];
                        break;
                    case "12时":
                        this.h12 = listRighs[i];
                        break;
                    case "13时":
                        this.h13 = listRighs[i];
                        break;
                    case "14时":
                        this.h14 = listRighs[i];
                        break;
                    case "15时":
                        this.h15 = listRighs[i];
                        break;
                    case "16时":
                        this.h16 = listRighs[i];
                        break;
                    case "17时":
                        this.h17 = listRighs[i];
                        break;
                    case "18时":
                        this.h18 = listRighs[i];
                        break;
                    case "19时":
                        this.h19 = listRighs[i];
                        break;
                    case "20时":
                        this.h20 = listRighs[i];
                        break;
                    case "21时":
                        this.h21 = listRighs[i];
                        break;
                    case "22时":
                        this.h22 = listRighs[i];
                        break;
                    case "23时":
                        this.h23 = listRighs[i];
                        break;
                    default:
                        break;
                }
            }
        }


        private void analysisLefsText(ref List<string> listLefs, ref string text)
        {
            listLefs = new List<string>();

            string tempText = text;

            int index = tempText.IndexOf("<ul class=\"lefs\">");
            if (index != -1)
            {
                tempText = tempText.Substring(index);

                index = tempText.IndexOf("<ul class=\"righs\">");
                if (index != -1)
                    tempText = tempText.Substring(0, index);

                tempText = tempText.Replace("</li>", "|");

                string[] tempTextItem = tempText.Split('|');

                tempText = null;
                foreach (string item in tempTextItem)
                {
                    if (item.IndexOf("时") == -1)
                        continue;

                    listLefs.Add(item.Replace("<li>", "|").Split('|')[1].Replace("明天 ", ""));
                }

                tempTextItem = null;
            }
        }

        private void analysisRighsText(ref List<string> listRighs, ref string text)
        {

            listRighs = new List<string>();

            string tempText = text;

            int index = tempText.IndexOf("<ul class=\"righs\">");
            if (index != 1)
            {
                tempText = tempText.Substring(index);

                tempText = tempText.Replace("℃", "|").Replace("<span style='width:30%'>", "|");

                string[] tempTextItem = tempText.Split('|');
                tempText = null;

                for (int i = 1; i < tempTextItem.Length; i += 2)
                {
                    listRighs.Add(tempTextItem[i]);
                }

                tempTextItem = null;

            }

        }

        #endregion

    }
}
