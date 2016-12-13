using System;
using System.IO;

namespace KunTaiServiceLibrary
{
    /// <summary>
    /// 写本地Log.txt的日志
    /// </summary>
    public class Log2
    {

        private static string filePath = Path.Combine(Config.UploadExportFileDirectory, "Log.txt");


        /// <summary>
        /// 在导出文件夹内追加文本
        /// </summary>
        /// <param name="text">需要记录的文本</param>
        public static void wirte(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                try
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(fileStream))
                        {
                            streamWriter.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text));
                        }
                    }
                    /*
                    FileStream myFs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    StreamWriter mySw = new StreamWriter(myFs);
                    mySw.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text));
                    mySw.Close();
                    myFs.Close();
                    */
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


        }


    }
}
