using Common.DotNetBean;
using Common.DotNetConfig;
using System;
using System.IO;
using System.Web.UI.WebControls;

namespace Common.DotNetFile
{
    public class UploadHelper
    {
        public static string UserFileUpload(FileUpload filleupload, ref string filename)
        {
            return FileUpload(ConfigHelper.GetAppSettings("UploadFilePath") + RequestSession.GetSessionUser().UserAccount.ToString() + "\\", filleupload, 0, ref filename);
        }
        public static string FileUpload(string path, FileUpload filleupload, ref string filename)
        {
            return FileUpload(path, filleupload, 0, ref filename);
        }
        public static string UserFileUploadPhoto(FileUpload filleupload, ref string filename)
        {
            return FileUpload(ConfigHelper.GetAppSettings("UploadFilePath") + RequestSession.GetSessionUser().UserAccount.ToString() + "\\", filleupload, 1, ref filename);
        }
        public static string FileUploadPhoto(string path, FileUpload filleupload, ref string filename)
        {
            return FileUpload(path, filleupload, 1, ref filename);
        }

        public static string FileUpload(string path, FileUpload filleupload, int FilenameType, ref string filename)
        {
            string result;
            // string filename;
            try
            {
                bool fileOk = false;
                string fileExtension = Path.GetExtension(filleupload.FileName).ToLower();
                string[] allowExtension = new string[]
				{
                    //".rar",
                    //".zip",
                    //".rar",
                    //".ios",
					".jpg",
                    ".jpeg",
                    //".png",
                    //"bmp",
                    //".gif",
                    //".doc",
                    // ".docx",
                    //".xls",
                    //".xlsx",
					//".txt"
                    ".pdf"
				};
                if (filleupload.HasFile)
                {
                    for (int i = 0; i < allowExtension.Length; i++)
                    {
                        if (fileExtension == allowExtension[i])
                        {
                            fileOk = true;
                            break;
                        }
                    }
                }
                if (fileOk)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    switch (FilenameType)
                    {
                        case 0:
                            filename = filleupload.FileName;
                            break;
                        case 1:
                            filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
                            break;
                        default:
                            filename = filleupload.FileName;
                            break;
                    }

                    if (!FileHelper.IsExistFile(path + filename))
                    {
                        int Size = filleupload.PostedFile.ContentLength / 1024 / 1024;
                        if (Size > 10)
                        {
                            result = "上传失败,文件过大";
                        }
                        else
                        {
                            filleupload.PostedFile.SaveAs(path + filename);
                            result = "上传成功";
                        }
                    }
                    else
                    {
                        result = "上传失败,文件已存在";
                    }
                }
                else
                {
                    result = "不支持【" + fileExtension + "】文件格式";
                }
            }
            catch (Exception)
            {
                result = "上传失败";
            }
            return result;
        }
    }
}