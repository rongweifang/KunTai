using System.Text;
using System.Xml.Linq;

namespace Common.DotNetJson
{

    public class Result
    {
        /// <summary>
        /// 获取方法查询结果数据
        /// </summary>
        /// <param name="datas">查询数据</param>
        /// <returns>返回方法查询结果数据</returns>
        public static string GetResultXml(string datas)
        {
            StringBuilder _xml = new StringBuilder();
            _xml.Append("<RESULT OPERATION=\"SUCCESS\">");
            _xml.Append(datas);
            _xml.Append("</RESULT>");
            return _xml.ToString();
        }

        /// <summary>
        /// 获取方法查询故障结果数据
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>返回方法查询故障结果数据</returns>
        public static string GetFaultXml(string errorMessage)
        {
            StringBuilder _xml = new StringBuilder();
            _xml.Append("<RESULT OPERATION=\"FAILURE\">");
            _xml.Append("<ERROR>");
            _xml.AppendFormat("<MESSAGE>{0}</MESSAGE>", errorMessage);
            _xml.Append("</ERROR>");
            _xml.Append("</RESULT>");
            return _xml.ToString();
        }

        /// <summary>
        /// 返回结果是否成功
        /// </summary>
        /// <param name="xml">返回结果</param>
        /// <returns>返回结果是否正确</returns>
        public static bool IsSuccess(string xml)
        {
            return XElement.Parse(xml).Attribute("OPERATION").Value == "SUCCESS" ? true : false;
        }
    }

}
