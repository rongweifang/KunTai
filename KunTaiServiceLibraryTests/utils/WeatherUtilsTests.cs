using Microsoft.VisualStudio.TestTools.UnitTesting;
using KunTaiServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KunTaiServiceLibrary.Tests
{
    [TestClass()]
    public class WeatherUtilsTests
    {
        [TestMethod()]
        public void getWebContentTest()
        {
            string content = WeatherUtils.getWebContent("http://www.weather.com.cn/weather/101120506.shtml");

            string pageHtml = File.ReadAllText(Path.Combine(Config.UploadExportFileDirectory, "weatherContent.txt"));



            string temp = pageHtml;
        }


        /**//// <summary>
            /// 读取日志文件
            /// </summary>
        private string ReadLogFile()
        {
            /**////从指定的目录以打开或者创建的形式读取日志文件
            FileStream fs = new FileStream(@"c:\weatherContent.txt", FileMode.Open, FileAccess.Read);

            /**////定义输出字符串
            StringBuilder output = new StringBuilder();

            /**////初始化该字符串的长度为0
            output.Length = 0;

            /**////为上面创建的文件流创建读取数据流
            StreamReader read = new StreamReader(fs);

            /**////设置当前流的起始位置为文件流的起始点
            read.BaseStream.Seek(0, SeekOrigin.Begin);

            /**////读取文件
            while (read.Peek() > -1)
            {
                /**////取文件的一行内容并换行
                output.Append(read.ReadLine() + "\n");
            }

            /**////关闭释放读数据流
            read.Close();

            /**////返回读到的日志文件内容
            return output.ToString();
        }
    }
}