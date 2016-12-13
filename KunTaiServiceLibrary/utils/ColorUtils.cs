using System;
using System.Drawing;

namespace KunTaiServiceLibrary
{
    public class ColorUtils
    {

        /// <summary>
        /// 获取一个随机Html颜色值
        /// </summary>
        /// <returns>返回格式为:0xFFFFFF的颜色值</returns>
        public static string getRandomColor()
        {
            Random random = new Random();

            string tempColor = ColorTranslator.ToHtml(Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));
            random = null;

            return tempColor.Replace("#", "0x");
        }

        /// <summary>
        /// 将一个颜色数字转换为Html字符串
        /// </summary>
        /// <param name="colorValue">颜色数值</param>
        /// <returns>返回格式为:0xFFFFFF的颜色值</returns>
        public static string toHtmlString(int colorValue)
        {
            string tempHtmlColor = ColorTranslator.ToHtml(ColorTranslator.FromWin32(colorValue)).Replace("#", "0x");

            //这里转换后的Html颜色值，发生了错位。以下代码为纠正
            string subLeftStr = tempHtmlColor.Substring(2, 2);
            string subRigthStr = tempHtmlColor.Substring(6, 2);

            tempHtmlColor = tempHtmlColor.Remove(2, 2);
            tempHtmlColor = tempHtmlColor.Remove(4, 2);

            tempHtmlColor = tempHtmlColor.Insert(2, subRigthStr);
            tempHtmlColor += subLeftStr;

            return tempHtmlColor;
        }

    }
}
