using System.Text;
using System.Xml.Linq;

namespace KunTaiServiceLibrary
{
    public class Result
    {
        /// <summary>
        /// 获取方法查询结果数据
        /// </summary>
        /// <param name="datas">查询数据</param>
        /// <returns>返回方法查询结果数据</returns>
        public static string getResultXml(string datas)
        {
            StringBuilder result = new StringBuilder();
            result.Append("<RESULT OPERATION=\"SUCCESS\">");
            result.Append(datas);
            result.Append("</RESULT>");

            datas = null;
            return result.ToString();
        }


        /// <summary>
        /// 获取方法查询故障结果数据
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>返回方法查询故障结果数据</returns>
        public static string getFaultXml(string errorMessage)
        {
            StringBuilder result = new StringBuilder();
            result.Append("<RESULT OPERATION=\"FAILURE\">");
            result.Append("<ERROR>");
            result.AppendFormat("<MESSAGE>{0}</MESSAGE>", errorMessage);
            result.Append("</ERROR>");
            result.Append("</RESULT>");

            errorMessage = null;
            return result.ToString();
        }


        /// <summary>
        /// 返回结果是否成功
        /// </summary>
        /// <param name="xml">返回结果</param>
        /// <returns>返回结果是否正确</returns>
        public static bool isSuccess(string xml)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(xml))
            {
                result = isSuccess(XElement.Parse(xml));
            }

            return result;
        }


        public static bool isSuccess(XElement xml)
        {
            bool result = false;

            if (xml != null)
            {
                if (xml.Attribute("OPERATION") != null)
                    result = xml.Attribute("OPERATION") == null ? false : xml.Attribute("OPERATION").Value == "SUCCESS" ? true : false;
            }

            return result;
        }

        public static string getErrorMessage(string text)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(result))
            {
                result = getErrorMessage(XElement.Parse(text));
            }

            return result;
        }


        public static string getErrorMessage(XElement xml)
        {
            string result = string.Empty;
            if (xml != null)
            {
                if (xml.Element("ERROR") != null)
                {
                    xml = xml.Element("ERROR");
                    if (xml.Element("MESSAGE") != null)
                    {
                        result = xml.Element("MESSAGE").Value;
                    }
                }
            }

            return result;
        }


    }
}
