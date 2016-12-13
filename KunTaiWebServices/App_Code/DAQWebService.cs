using KunTaiServiceLibrary;
using System.Web.Services;
using System.Xml.Linq;

/// <summary>
/// DAQWebService 的摘要说明
/// </summary>
[WebService(Namespace = "http://www.qiangwang.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class DAQWebService : System.Web.Services.WebService
{

    public DAQWebService()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string addDataAcquisition(string text)
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

        //对数据进行解密
        string GUID = new Security().decode(xml.Element("GUID").Value);

        string result = string.Empty;
        //GUID 从[ORGANIZATION]表中获取
        switch (GUID)
        {
            //招远金城热力公司
            case "76fa2a08-4396-46a7-a002-f2cf124ee032":
                result = "";
                break;
            default:
                result = "非法客户端，请求失败。";
                break;
        }

        return result;
    }

}
