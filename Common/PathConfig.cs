using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;

namespace Plupload.Web.Common
{
    public class PathConfig
    {

        /// <summary>
        /// 读取 Web.Config 中的键值。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <returns>键值。</returns>
        public static string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }

        /// <summary>
        /// 获取系统安装根目录的虚拟路径（相对路径）。
        /// </summary>
        public static string VirtualPath
        {
            get { return GetSetting("Path").ToString(); }
        }

        /// <summary>
        /// 获取系统安装根目录的物理文件路径。
        /// </summary>
        public static string Path
        {
            get { return HttpContext.Current.Server.MapPath(VirtualPath); }
        }

        /// <summary>
        /// 定义系统文件上传文件路径。
        /// </summary>
        public static string UploadFilePath = "{0}Uplaod/Attachment/";
        /// <summary>
        /// 获取系统上传文件路径。
        /// </summary>
        public static string UploadPath
        {
            get
            {
                return string.Format(UploadFilePath, Path);
            }
        }


        /// <summary>
        /// 获取指定文件的虚拟路径（相对于网站根目录）。
        /// </summary>
        /// <param name="path">指定的 Web 服务器物理文件路径。</param>
        /// <returns>返回 Web 服务器上的虚拟路径。</returns>
        public static string GetVirtualPath(string path)
        {
            return path.Replace(Path, VirtualPath);
        }
    }
}