using KunTaiServiceLibrary;
using System.Web.Services;

/// <summary>
/// KunTaiWebService 的摘要说明
/// </summary>
[WebService(Namespace = "http://www.qiangwang.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class WeatherWebService : System.Web.Services.WebService
{

    public WeatherWebService()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }


    /// <summary>
    /// 获取组织机构的列表
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public string getOrganizationList()
    {
        return new Weather().getOrganizationList();
    }


    /// <summary>
    /// 获取更新天气的下一次时间
    /// </summary>
    [WebMethod]
    public string getUpdateWeatherNextTime(string text)
    {
        return new Weather().getUpdateWeatherNextTime(text);
    }


    /// <summary>
    /// 添加天气信息
    /// </summary>
    /// <param name="text">天气XML</param>
    /// <returns>添加天气信息的返回结果</returns>
    [WebMethod]
    public string addWeathers(string text)
    {
        return new Weather().addDataItems(text);
    }


    /// <summary>
    /// 根据OID来查询名称
    /// </summary>
    [WebMethod]
    public string searchOID(string text)
    {
        return new Organization().getDataItemDetails(text);
    }




}
