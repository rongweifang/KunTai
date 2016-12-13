using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace KunTaiServiceLibrary
{
    public class WeatherUtils
    {

        /// <summary>
        /// 获取指定Web页码的详细内容
        /// </summary>
        /// <param name="Url">http的网址</param>
        /// <returns>返回页码的详细内容（UTF-8）</returns>
        public static string getWebContent(string url)
        {
            WebClient webClient = new WebClient();
            string fileUrl = Path.Combine(Config.UploadExportFileDirectory, "weatherContent.txt");

            webClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            webClient.DownloadFile(url, fileUrl);

            webClient.Dispose();
            webClient = null;

            //RONG 2016-21
            string Webcontent = File.ReadAllText(fileUrl);
            string content = Webcontent;
            Regex reg2 = new Regex(@"observe24h_data =[\s\S]*?;", RegexOptions.ExplicitCapture);
            Match match = Regex.Match(Webcontent, @"observe24h_data =[\s\S]*?;");
            if (match.Success)
            {
                content = match.Groups[0].Value;
            }
                //打开指定的文件，然后将内容返回

                //717行是24小时温度值

                //注释于2016年6月21日
                //string[] line = File.ReadAllLines(fileUrl);
                //string content = line[718];

                //CHECK_FLAG:
                //if (content.IndexOf("observe24h_data") == -1)
                //{
                //    content = line[717];
                //    goto CHECK_FLAG;
                //}

                //line = null;

                return content;

            /*
            string htmlCode;
            HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            webRequest.Timeout = 30000;
            webRequest.Method = "GET";
            webRequest.UserAgent = "Mozilla/4.0";
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
            if (webResponse.ContentEncoding.ToLower() == "gzip")//如果使用了GZip则先解压
            {
                using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                {
                    using (var zipStream =
                        new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                    {
                        using (StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.Default))
                        {
                            htmlCode = sr.ReadToEnd();
                        }
                    }
                }
            }
            else
            {
                using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(streamReceive, Encoding.Default))
                    {
                        htmlCode = sr.ReadToEnd();
                    }
                }
            }
            */
            //return htmlCode;
        }


    }
}
