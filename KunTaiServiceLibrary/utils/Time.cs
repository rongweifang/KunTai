
using System;
using System.Text;

namespace KunTaiServiceLibrary
{
    public class Time
    {
        /// <summary>
        /// 获取秒数的分秒格式 M分 S秒
        /// </summary>
        /// <param name="time">秒数</param>
        /// <returns>秒数M分 S秒格式的分秒字符串</returns>
        public static string getTime(int time)
        {
            string text = string.Empty;
            //计算出 秒。
            int seconds = 0;
            seconds = time % 60;

            //计算出 分
            int minutes = 0;
            minutes = time / 60;

            //刷新显示
            if (seconds == 0)
            {
                text = string.Format("{0}分", minutes);
            }
            else if (minutes == 0)
            {
                text = string.Format("{0}秒", seconds);
            }
            else
            {
                text = string.Format("{0}分{1}秒", minutes, seconds);
            }
            return text;
        }

        /// <summary>
        /// 字符串转换为日期对象
        /// </summary>
        /// <param name="dateTime">日期字符串。格式（YYYY-MM-DD HH24:MI:SS)</param>
        /// <returns>返回字符串值的日期对象</returns>
        public static DateTime stringToDateTime(string dateTime)
        {
            //2013-08-08 08:08:08
            if (dateTime.Split(' ').Length <= 1)
                throw new Exception("日期字符串格式错误。");

            string[] dates = dateTime.Split(' ')[0].Split('-');
            if (dates.Length <= 2)
                throw new Exception("初始化日期对象错误。");

            string[] times = dateTime.Split(' ')[1].Split(':');
            if (times.Length <= 2)
                throw new Exception("初始化时间对象错误。");

            int year = Convert.ToInt32(dates[0]);
            int month = Convert.ToInt32(dates[1]);
            int day = Convert.ToInt32(dates[2]);

            int hour = Convert.ToInt32(times[0]);
            int minute = Convert.ToInt32(times[1]);
            int second = Convert.ToInt32(times[2]);

            return new DateTime(year, month, day, hour, minute, second);
        }

        /// <summary>
        /// 将日期字符串转换为Oracle格式
        /// </summary>
        /// <param name="dateTime">日期时间字符串（YYYY-MM-DD HH24:MI:SS）</param>
        /// <returns>返回一个YYYY-MM-DD HH24:MI:SS格式的字符串</returns>
        public static string convertToYYYY_MM_DD_HH24_MI_SS(string dateTime)
        {
            dateTime = dateTime.Replace("/", "-");

            StringBuilder new_dateTime = new StringBuilder();

            string[] dates = dateTime.Split(' ')[0].Split('-');

            new_dateTime.Append(dates[0]);
            if (dates[1].Length == 1)
                new_dateTime.AppendFormat("-0{0}", dates[1]);
            else
                new_dateTime.AppendFormat("-{0}", dates[1]);

            if (dates[2].Length == 1)
                new_dateTime.AppendFormat("-0{0}", dates[2]);
            else
                new_dateTime.AppendFormat("-{0}", dates[2]);

            new_dateTime.Append(" ");

            string[] times = dateTime.Split(' ')[1].Split(':');
            if (times[0].Length == 1)
                new_dateTime.AppendFormat("0{0}", times[0]);
            else
                new_dateTime.AppendFormat("{0}", times[0]);

            if (times[1].Length == 1)
                new_dateTime.AppendFormat(":0{0}", times[1]);
            else
                new_dateTime.AppendFormat(":{0}", times[1]);

            if (times[2].Length == 1)
                new_dateTime.AppendFormat(":0{0}", times[2]);
            else
                new_dateTime.AppendFormat(":{0}", times[2]);


            return new_dateTime.ToString();
        }

        public static void setStartEndDateTime(ref string startDateTime, ref string endDateTime)
        {
            DateTime dateTimeNow;
            if (string.IsNullOrEmpty(startDateTime) && string.IsNullOrEmpty(endDateTime))//都为空时
            {
                dateTimeNow = DateTime.Now;
                startDateTime = dateTimeNow.AddYears(-1).ToString();
                endDateTime = dateTimeNow.ToString();
            }
            else if (string.IsNullOrEmpty(startDateTime) && !string.IsNullOrEmpty(endDateTime))//开始时间为空，结束时间不为空时
            {
                dateTimeNow = Time.stringToDateTime(endDateTime);

                startDateTime = dateTimeNow.AddYears(-1).ToString();

            }
            else if (!string.IsNullOrEmpty(startDateTime) && string.IsNullOrEmpty(endDateTime))//开始时间不为空，结束时间为空时
            {
                dateTimeNow = Time.stringToDateTime(startDateTime);

                endDateTime = dateTimeNow.AddYears(1).ToString();
            }
            else//开始、结束时间都不为空时
            {

            }
        }

        /// <summary>
        /// 获取计算机当前时间
        /// </summary>
        /// <returns>返回一个格式为:yyyy-MM-dd HH:mm:ss的时间字符串</returns>
        public static string getDateTimeNowString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
