using System;
using System.Xml.Linq;

namespace KunTaiServiceLibrary
{
    public class SqlServerCommandText
    {

        private const string pagingCommandText = "SELECT ROW_NUMBER() OVER (ORDER BY ({0})) AS NUM,* FROM ({1}) DATATABLE WHERE NUM1>={2} AND NUM1<={3}";

        public static string createPagingCommandText(ref string commandText, ref XElement xml, string orderFieldName = "NUM1")
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(commandText))
                return result;

            if (xml == null)
                return result;

            uint pageNumber = xml.Element("PAGENUMBER").Value != null ? uint.Parse(xml.Element("PAGENUMBER").Value) : 0;
            uint pageCount = xml.Element("PAGECOUNT").Value != null ? uint.Parse(xml.Element("PAGECOUNT").Value) : 0;

            if (pageNumber == 0 || pageCount == 0)
                throw new Exception("页码参数不正确。请检查是否有&lt;PAGENUMBER&gt;或&lt;PAGECOUNT&gt;节点。");

            return string.Format(pagingCommandText, orderFieldName, commandText, getMinNumber(ref pageNumber, ref pageCount), getMaxNumber(ref pageNumber, ref pageCount));
        }

        private static uint getMaxNumber(ref uint pageNumber, ref uint pageCount)
        {
            return pageNumber * pageCount;
        }

        private static uint getMinNumber(ref uint pageNumber, ref uint pageCount)
        {
            return pageCount * (pageNumber - 1) + 1;
        }

    }
}
