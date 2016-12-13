<%@ WebHandler Language="C#" Class="UpLoadFLVFileHandler" %>

using System;
using System.Web;
using System.IO;
using System.Configuration;

public class UpLoadFLVFileHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        HttpFileCollection files = context.Request.Files;

        HttpPostedFile fileData = null;
        if (files.Count > 0)
        {
            fileData = files[0];
        }
        else
        {
            context.Response.Write("不允许直接执行");
            return;
        }


        string result = string.Empty;

        try
        {
            string path = ConfigurationManager.AppSettings["UPLOADFILEDIRECTORY"].ToString();

            //result = Path.GetFileName(FileData.FileName);//获得文件名  
            string extension = Path.GetExtension(fileData.FileName);//获得文件扩展名  
            string newFileName = Guid.NewGuid().ToString().ToUpper();//使用新的名称
            string saveName = newFileName + extension;//实际保存文件名  

            saveFile(fileData, path, saveName);//保存文件  
            result = saveName;

            path = extension = newFileName = saveName = null;
        }
        catch
        {
            result = string.Empty;
        }

        //成功时，返回上传的名字
        context.Response.ContentType = "text/plain";
        context.Response.Write(result);
    }

    private void saveFile(HttpPostedFile postedFile, string path, string saveName)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        try
        {
            postedFile.SaveAs(Path.Combine(path, saveName));
        }
        catch (Exception e)
        {
            throw new ApplicationException(e.Message);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}