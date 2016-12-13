<%@ WebHandler Language="C#" Class="IPAddressHandler" %>

using System;
using System.Web;

public class IPAddressHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        context.Response.Charset = "UTF-8";

        string ip = string.Empty;

        if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null) // using proxy
        {
            ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();  // Return real client IP.
        }
        else// not using proxy or can't get the Client IP
        {
            ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString(); //While it can't get the Client IP, it will return proxy IP.
        }

        context.Response.Write(ip == "::1" ? "127.0.0.1" : ip);
        //context.Response.Flush();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}