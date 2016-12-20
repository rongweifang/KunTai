using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;


//======================================================================
//
//        All rights reserved
//
//        filename :StringLib
//        description :字符串操作
//
//        created by 荣卫方 
//        created date:2011-12-25
//
//======================================================================
public class StringCS
{
    /// <summary>
    /// 页面数据校验类
    /// Copyright (C) Maticsoft 2004-2011
    /// </summary>
    private static Regex RegPhone = new Regex("^[0-9]+[-]?[0-9]+[-]?[0-9]$");
    private static Regex RegNumber = new Regex("^[0-9]+$");
    private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
    private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
    private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
    private static Regex RegEmail = new Regex("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
    private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
    /// <summary>
    /// 获取参数值(处理异常)
    /// </summary>
    /// <param name="str">参数</param>
    /// <returns></returns>
    public static string GetString(string str)
    {
        return GetString(str, "");
    }
    /// <summary>
    /// 对象转换int(处理异常,默认返回0)
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    #region
    public static int ParseInt(object str)
    {
        try
        {
            return Convert.ToInt32(str);
        }
        catch
        {
            return 0;
        }
    }
    /// <summary>
    /// 字符串转换int(处理异常,默认返回0)
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int ParseInt(string str)
    {
        try
        {
            return int.Parse(str);
        }
        catch
        {
            return 0;
        }
    }
    /// <summary>
    /// 对象转换int(处理异常,返回指定的值)
    /// </summary>
    /// <param name="str"></param>
    /// <param name="def"></param>
    /// <returns></returns>
    public static int ParseInt(object str, int def)
    {
        try
        {
            return Convert.ToInt32(str);
        }
        catch
        {
            return def;
        }
    }
    /// <summary>
    /// 字符串转换int(处理异常,返回指定值)
    /// </summary>
    /// <param name="str"></param>
    /// <param name="def"></param>
    /// <returns></returns>
    public static int ParseInt(string str, int def)
    {
        try
        {
            return int.Parse(str);
        }
        catch
        {
            return def;
        }
    }
    #endregion
    /// <summary>
    /// 获取参数值(处理异常)
    /// </summary>
    /// <param name="str">参数</param>
    /// <returns></returns>
    #region
    public static int GetStringInt(string str)
    {
        try
        {
            return ParseInt(GetString(str, "0"));
        }
        catch
        {
            return 0;
        }
    }
    /// <summary>
    /// 获取参数值(出现异常,返回默认值)
    /// </summary>
    /// <param name="str">参数</param>
    /// <param name="Default">默认值</param>
    /// <returns></returns>
    public static string GetString(string str, string Default)
    {
        try
        {
            return HttpContext.Current.Request[str];
        }
        catch
        {
            return Default;
        }
    }
    /// <summary>
    /// 获取参数值(出现异常,返回默认值)
    /// </summary>
    /// <param name="str">参数</param>
    /// <param name="Default">默认值</param>
    /// <returns></returns>
    public static int GetString(string str, int Default)
    {
        try
        {
            return int.Parse(HttpContext.Current.Request[str]);
        }
        catch
        {
            return Default;
        }
    }
    /// <summary>
    /// 为Session对象赋值
    /// </summary>
    /// <param name="obj">对象名</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    #endregion
    public static void SetSession(string obj, string value)
    {
        HttpContext.Current.Session[obj] = value;
    }
    /// <summary>
    /// 获取Session对象(处理异常)
    /// </summary>
    /// <param name="obj">对象名</param>
    /// <returns></returns>
    public static string GetSession(string obj)
    {
        return GetSession(obj, "");
    }
    /// <summary>
    /// 清楚指定session的值
    /// </summary>
    /// <param name="obj"></param>
    public static void ClearSession(string obj)
    {
        HttpContext.Current.Session[obj] = "";
    }
    /// <summary>
    /// 获取Session对象(处理异常)返回默认值
    /// </summary>
    /// <param name="obj">对象名</param>
    /// <param name="def">默认值</param>
    /// <returns></returns>
    public static string GetSession(string obj, string def)
    {
        try
        {
            if (HttpContext.Current.Session[obj] != null)
                return HttpContext.Current.Session[obj].ToString();
            else
                return def;
        }
        catch
        {
            return def;
        }
    }
    #region Cookie
    /// <summary>
    /// 给cookie赋值(存在就更新值,不存在就添加)
    /// </summary>
    /// <param name="obj">对象名</param>
    /// <param name="value">值</param>
    public static void SetCookie(string obj, string value)
    {
        if (HttpContext.Current.Response.Cookies[obj] == null)
        {
            HttpCookie cookie = new HttpCookie(obj);
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        else
        {
            HttpCookie cookie = new HttpCookie(obj);
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
    /// <summary>
    /// 给cookies赋值(不存在就创建,存在就更新值)
    /// </summary>
    /// <param name="obj">对象名</param>
    /// <param name="name">键</param>
    /// <param name="value">值</param>
    public static void SetCookies(string obj, string name, string value)
    {
        if (HttpContext.Current.Response.Cookies[obj] == null && String.IsNullOrEmpty(HttpContext.Current.Response.Cookies[obj][name]))
        {
            HttpCookie cookie = new HttpCookie(obj);
            cookie.Values.Add(name, value);
            cookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        else
        {
            HttpCookie cookie = new HttpCookie(obj);
            cookie.Values.Set(name, value);
            cookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
    /// <summary>
    /// 获取cookie值(单个)
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>""</returns>
    public static string GetCookie(string obj)
    {
        try
        {
            if (HttpContext.Current.Request.Cookies[obj] != null)
                return HttpContext.Current.Request.Cookies[obj].Value;
            else
                return "";
        }
        catch
        {
            return "";
        }
    }
    /// <summary>
    /// 获取cookies集合值(单个值)
    /// </summary>
    /// <param name="obj">对象名</param>
    /// <param name="name">键</param>
    /// <returns>""</returns>
    public static string GetCookies(string obj, string name)
    {
        try
        {
            if (HttpContext.Current.Request.Cookies[obj] != null)
                return HttpContext.Current.Request.Cookies[obj][name];
            else
                return "";
        }
        catch
        {
            return "";
        }
    }
    /// <summary>
    /// 删除指定的cookie
    /// </summary>
    /// <param name="obj"></param>
    public static void ClearCookie(string obj)
    {
        try
        {
            if (HttpContext.Current.Response.Cookies[obj] != null && !string.IsNullOrEmpty(HttpContext.Current.Response.Cookies[obj].Value))
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[obj];
                cookie.Value = "";
                cookie.Expires = DateTime.Now.AddDays(-5);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        catch (Exception)
        {
            HttpCookie cookie = new HttpCookie(obj);
            cookie.Value = "";
            cookie.Expires = DateTime.Now.AddYears(-5);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
    /// <summary>
    /// 删除cookies中指定的键
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    public static void ClearCookies(string obj, string name)
    {
        try
        {
            if (HttpContext.Current.Response.Cookies[obj] != null && !string.IsNullOrEmpty(HttpContext.Current.Response.Cookies[obj].Value))
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[obj];
                cookie.Values.Remove(name);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        catch (Exception)
        {
            HttpCookie cookie = new HttpCookie(obj);
            cookie.Values.Remove(name);
            cookie.Expires = DateTime.Now.AddYears(-5);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
    #endregion
    #region Cache
    /// <summary>
    /// 获取当前应用程序指定CacheKey的Cache值
    /// </summary>
    /// <param name="CacheKey"></param>
    /// <returns></returns>
    public static object GetCache(string CacheKey)
    {
        try
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// 设置当前应用程序指定CacheKey的Cache值
    /// </summary>
    /// <param name="CacheKey"></param>
    /// <param name="objObject"></param>
    public static void SetCache(string CacheKey, object objObject)
    {
        System.Web.Caching.Cache objCache = HttpRuntime.Cache;
        objCache.Insert(CacheKey, objObject);
    }
    /// <summary>
    /// 设置当前应用程序指定CacheKey的Cache值
    /// </summary>
    /// <param name="CacheKey"></param>
    /// <param name="objObject"></param>
    public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
    {
        System.Web.Caching.Cache objCache = HttpRuntime.Cache;
        objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
    }

    /// <summary>
    /// 删除Cache对象
    /// </summary>
    /// <param name="strCacheName">Cache名称</param>
    public static void ClearCache(string strCacheName)
    {
        if (HttpContext.Current.Cache[strCacheName] != null)
        {
            HttpContext.Current.Cache.Remove(strCacheName);
        }
    }
    #endregion
    /// <summary>
    /// paramChg(string str)
    /// 功能描述：将字符串转换成参数。
    /// </summary>
    /// <param name="str">需转换的字符串</param>
    /// <returns></returns>
    public static string paramChg(string str)
    {
        #region
        str = "@" + str;
        return str;
        #endregion
    }
    /// <summary>
    /// CutString(string str, int num, bool flg)
    /// 截取字符串函数
    /// </summary>
    /// <param name="str">所要截取的字符串</param>
    /// <param name="num">截取字符串的长度</param>
    /// <param name="flg">true:加...,flase:不加</param>
    /// <returns></returns>
    public static string CutString(string str, int num, bool flg)
    {
        #region
        ASCIIEncoding ascii = new ASCIIEncoding();
        int tempLen = 0;
        string tempString = "";
        byte[] s = ascii.GetBytes(str);
        for (int i = 0; i < s.Length; i++)
        {
            if ((int)s[i] == 63)
            {
                tempLen += 2;
            }
            else
            {
                tempLen += 1;
            }

            try
            {
                tempString += str.Substring(i, 1);
            }
            catch
            {
                break;
            }

            if (tempLen > num)
                break;
        }
        //如果截过则加上半个省略号
        byte[] mybyte = System.Text.Encoding.Default.GetBytes(str);
        if (mybyte.Length > num)
            if (flg)
            {
                tempString += "...";
            }
        return tempString;
        #endregion
    }
    ///<summary>
    ///CheckKeyWord(string sWord)
    /// 过滤输入的危险信息
    /// </summary>
    /// <param name="text">内容</param>
    /// <param name="maxLength">最大长度</param>
    /// <returns></returns>
    public static bool CheckKeyWord(string sWord)
    {
        #region
        //过滤关键字
        string StrKeyWord = @"select|insert|delete|from|count\(|drop table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec master|netlocalgroup administrators|:|net user|""|or|and";
        //过滤关键字符
        string StrRegex = @"[-|;|,|/|\(|\)|\[|\]|}|{|%|\@|*|!|']";
        if (Regex.IsMatch(sWord, StrKeyWord, RegexOptions.IgnoreCase) || Regex.IsMatch(sWord, StrRegex))
            return true;
        return false;
        #endregion
    }
    /// <summary>
    /// 过滤输入的危险信息
    /// FilteT_SQL(string strtb, int maxLength)
    /// </summary>
    /// <param name="text">内容</param>
    /// <param name="maxLength">最大长度</param>
    /// <returns></returns>
    public static string FilteT_SQL(string strtb, int maxLength)
    {
        #region
        strtb = strtb.Trim();
        if (string.IsNullOrEmpty(strtb))
            return string.Empty;
        if (strtb.Length > maxLength)
            strtb = strtb.Substring(0, maxLength);
        strtb = strtb.Replace("!", "");
        strtb = strtb.Replace("@", "");
        strtb = strtb.Replace("#", "");
        strtb = strtb.Replace("$", "");
        strtb = strtb.Replace("%", "");
        strtb = strtb.Replace("^", "");
        strtb = strtb.Replace("&", "");
        strtb = strtb.Replace("*", "");
        strtb = strtb.Replace("(", "");
        strtb = strtb.Replace(")", "");
        strtb = strtb.Replace("_", "");
        strtb = strtb.Replace("+", "");
        strtb = strtb.Replace("|", "");
        strtb = strtb.Replace("?", "");
        strtb = strtb.Replace("/", "");
        strtb = strtb.Replace(".", "");
        strtb = strtb.Replace(">", "");
        strtb = strtb.Replace("<", "");
        strtb = strtb.Replace("{", "");
        strtb = strtb.Replace("}", "");
        strtb = strtb.Replace("[", "");
        strtb = strtb.Replace("]", "");
        strtb = strtb.Replace("-", "");
        strtb = strtb.Replace("=", "");
        strtb = strtb.Replace(",", "");
        strtb = strtb.Replace("`", "");
        strtb = strtb.Replace("~", "");
        strtb = strtb.Replace("'", "");
        strtb = strtb.Replace(":", "");
        strtb = strtb.Replace(";", "");
        strtb = Regex.Replace(strtb, "(\\s*[s|S][e|E][l|L][e|E][c|C][t|T]\\s*)+", "");
        strtb = Regex.Replace(strtb, "(\\s*[i|I][n|N][s|S][e|E][r|R][t|T]\\s*)+", "");
        strtb = Regex.Replace(strtb, "(\\s*[d|D][e|E][l|L][e|E][t|T][e|E]\\s*)+", "");
        strtb = Regex.Replace(strtb, "(\\s*[u|U][p|P][d|D][a|A][t|T][a|A]\\s*)+", "");
        strtb = Regex.Replace(strtb, "(\\s*[d|D][r|R][o|O][p|P]\\s*)+", "");
        return strtb;
        #endregion
    }
    /// <summary>
    /// 判断输入是否数字
    /// IsInt(string num)
    /// </summary>
    /// <param name="num">要判断的字符串</param>
    /// <returns></returns>
    /// <summary>
    public static bool IsInt(string num)
    {
        #region
        try
        {
            Convert.ToInt32(num);
            return true;
        }
        catch { return false; }
        #endregion
    }
    #region 数字字符串检查
    public static bool IsPhone(string inputData)
    {
        Match m = RegPhone.Match(inputData);
        return m.Success;
    }
    /// <summary>
    /// 是否数字字符串
    /// </summary>
    /// <param name="inputData">输入字符串</param>
    /// <returns></returns>
    public static bool IsNumber(string inputData)
    {
        Match m = RegNumber.Match(inputData);
        return m.Success;
    }
    /// <summary>
    /// 是否数字字符串 可带正负号
    /// </summary>
    /// <param name="inputData">输入字符串</param>
    /// <returns></returns>
    public static bool IsNumberSign(string inputData)
    {
        Match m = RegNumberSign.Match(inputData);
        return m.Success;
    }
    /// <summary>
    /// 是否是浮点数
    /// </summary>
    /// <param name="inputData">输入字符串</param>
    /// <returns></returns>
    public static bool IsDecimal(string inputData)
    {
        Match m = RegDecimal.Match(inputData);
        return m.Success;
    }
    /// <summary>
    /// 是否是浮点数 可带正负号
    /// </summary>
    /// <param name="inputData">输入字符串</param>
    /// <returns></returns>
    public static bool IsDecimalSign(string inputData)
    {
        Match m = RegDecimalSign.Match(inputData);
        return m.Success;
    }
    #endregion
    /// <summary>
    /// 检测是否有中文字符
    /// </summary>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static bool IsHasCHZN(string inputData)
    {
        Match m = RegCHZN.Match(inputData);
        return m.Success;
    }
    #region 邮件地址
    /// <summary>
    /// 是否是浮点数 可带正负号
    /// </summary>
    /// <param name="inputData">输入字符串</param>
    /// <returns></returns>
    public static bool IsEmail(string inputData)
    {
        Match m = RegEmail.Match(inputData);
        return m.Success;
    }
    #endregion
    /// <summary>
    /// 判断一个字符串是否是时间格式
    ///  IsDatetime(string strValue)
    /// </summary>
    /// <param name="strValue"></param>
    /// <returns></returns>
    public static bool IsDatetime(string strValue)
    {
        #region
        strValue = strValue.Trim();
        string strReg = @"([1-2][0-9][0-9][0-9])(-|/)(0*[1-9]|1[0-2])(-|/)(0*[1-9]|[12][0-9]|3[01])(\ (0*[0-9]|1[0-9]|2[0-3]):(0*[0-9]|[1-5][0-9]):(0[0-9]|[1-5][0-9]))*";
        if (strValue == "")
        {
            return false;
        }
        else
        {
            Regex re = new Regex(strReg);
            MatchCollection mc = re.Matches(strValue);
            if (mc.Count == 1)
                foreach (Match m in mc)
                {
                    if (m.Value == strValue)
                        return true;
                }
        }
        return false;
        #endregion
    }
    /// <summary>
    /// 转全角的函数(SBC case)
    /// ToSBC(string input)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ToSBC(string input)
    {
        #region
        //半角转全角：
        char[] c = input.ToCharArray();
        for (int i = 0; i < c.Length; i++)
        {
            if (c[i] == 32)
            {
                c[i] = (char)12288;
                continue;
            }
            if (c[i] < 127)
                c[i] = (char)(c[i] + 65248);
        }
        return new string(c);
        #endregion
    }
    /// <summary>
    ///  转半角的函数(SBC case)
    /// </summary>
    /// <param name="input">输入</param>
    /// <returns></returns>
    public static string ToDBC(string input)
    {
        #region
        char[] c = input.ToCharArray();
        for (int i = 0; i < c.Length; i++)
        {
            if (c[i] == 12288)
            {
                c[i] = (char)32;
                continue;
            }
            if (c[i] > 65280 && c[i] < 65375)
                c[i] = (char)(c[i] - 65248);
        }
        return new string(c);
        #endregion
    }
    /// <summary>
    /// 判断输入是否中文
    /// </summary>
    /// <param name="CString">要判断的字符串</param>
    /// <returns></returns>
    public static bool IsChinaWords(string str)
    {
        #region
        bool BoolValue = false;
        for (int i = 0; i < str.Length; i++)
        {
            if (Convert.ToInt32(Convert.ToChar(str.Substring(i, 1))) < Convert.ToInt32(Convert.ToChar(128)))
            {
                BoolValue = false;
            }
            else
            {
                BoolValue = true;
            }
        }
        return BoolValue;
        #endregion
    }
    /// <summary>
    /// 字符串转换成INT
    /// </summary>
    /// <param name="str">要转换成int的字符传</param>
    /// <returns>返回0就是错误</returns>
    public static int GetStrToInt(string str)
    {
        #region
        try
        {
            if (str.Length == 0)
                return 0;
            else
            {
                if (Int32.Parse(str) == 0)
                    return 0;
                else
                    return Int32.Parse(str);
            }
        }
        catch
        {
            return 0;
        }
        #endregion
    }
    /// <summary>
    /// 对时间进行格式化，如：2007-1-15,2007/5/2
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="geshi">如：y-m-d；y/m/d；y-m-d h:mm:ss；m-d-y；m/d/y</param>
    /// <param name="spstr">分隔符号，如：-，/</param>
    /// <returns></returns>
    public static string GetDateToString(string datestr, string geshi)
    {
        #region
        string str = "";
        DateTime dt = DateTime.Now;
        if (IsDatetime(datestr))
        {
            dt=DateTime.Parse(datestr);
            str = string.Format("{0:" + geshi + "}",dt);
        }
        return str;
        #endregion
    }
    public static string GetDateToString(DateTime dt, string geshi)
    {
        #region
        string str = "";
        str = string.Format("{0:" + geshi + "}", dt);
        return str;
        #endregion
    }
    /// <summary>
    /// 把秒转换成分钟
    /// SecondToMinute(int Second)
    /// </summary>
    /// <returns></returns>
    public static int SecondToMinute(int Second)
    {
        decimal mm = (decimal)((decimal)Second / (decimal)60);
        return Convert.ToInt32(Math.Ceiling(mm));
    }
    /// <summary>
    /// 返回某年某月最后一天
    ///  GetMonthLastDate(int year, int month)
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <returns>日</returns>
    public static int GetMonthLastDate(int year, int month)
    {
        #region
        DateTime lastDay = new DateTime(year, month, new System.Globalization.GregorianCalendar

().GetDaysInMonth(year, month));
        int Day = lastDay.Day;
        return Day;
        #endregion
    }
    /// <summary>
    ///  返回时间差
    /// DateDiff(DateTime DateTime1, DateTime DateTime2)
    /// </summary>
    /// <param name="DateTime1">DateTime1</param>
    /// <param name="DateTime2">DateTime2</param>
    /// <returns>时间差:XX月XX日/XX小时/XX分钟</returns>
    public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
    {
        #region
        string dateDiff = null;
        try
        {
            //TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            //TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            //TimeSpan ts = ts1.Subtract(ts2).Duration();
            TimeSpan ts = DateTime2 - DateTime1;
            if (ts.Days >= 1)
            {
                dateDiff = DateTime1.Month.ToString() + "月" + DateTime1.Day.ToString() + "日";
            }
            else
            {
                if (ts.Hours > 1)
                {
                    dateDiff = ts.Hours.ToString() + "小时前";
                }
                else
                {
                    dateDiff = ts.Minutes.ToString() + "分钟前";
                }
            }
        }
        catch
        { }
        return dateDiff;
        #endregion
    }
    /// <summary>
    /// 去除html格式
    /// </summary>
    /// <param name="strhtml"></param>
    /// <returns></returns>
    public static string DelHtml(string strhtml)
    {
        string StrNohtml = System.Text.RegularExpressions.Regex.Replace(strhtml, "<[^>]+>", "");
        StrNohtml = System.Text.RegularExpressions.Regex.Replace(StrNohtml, "&[^;]+;", "");
        return StrNohtml;
    }
    /// <summary>
    /// 过滤html标签
    ///  RemoveHTML(string html)
    /// </summary>
    /// <param name="html">需要过滤的字符串</param>
    /// <returns>过滤html后的字符串</returns>
    public static string RemoveHTML(string html)
    {
        #region
        html = Regex.Replace(html, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"-->", "", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"<!--.*", "", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"&#(\d+);", "", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"<img[^>]*>;", "", RegexOptions.IgnoreCase);
        html.Replace("<", "");
        html.Replace(">", "");
        html.Replace("\r\n", "");
        //html = HttpContext.Current.Server.HtmlEncode(html).Trim();
        //html = HttpContext.Current.Server.HtmlDecode(html).Trim();
        return html;
        #endregion
    }
    /// <summary>
    /// 对表单内容进行转换HTML操作, 
    /// HtmlDiscode(string theString)
    /// </summary>
    /// <param name="fString">html字符串</param>
    /// <returns></returns>
    static public string HtmlDiscode(string theString)
    {
        #region
        if (theString != "")
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
           // theString = theString.Replace("&nbsp;", " ");
            //theString = theString.Replace(" &nbsp;", "  ");
            //theString = theString.Replace("&quot;", "\"");
            //theString = theString.Replace("&#39;", "\'");
            //theString = theString.Replace("<br/> ", "\n");
            //theString = theString.Replace("''", "'");
            //theString = theString.Replace("／／", "//");
            //theString = theString.Replace("ｈｔｔｐ", "http");
            //theString = theString.Replace("ｊｓ", "js");
            //theString = theString.Replace("ｇｉｆ", "gif");
            //theString = theString.Replace("ｃｏｍ", "com");
            return theString;
        }
        else
            return string.Empty;
        #endregion
    }
    /// <summary>
    /// 对表单内容进行转换HTML操作
    /// HtmlEncode(string theString)
    /// </summary>
    /// <param name="fString">html字符串</param>
    /// <returns></returns>
    static public string HtmlEncode(string theString)
    {
        #region
        if (theString != "")
        {
            theString = theString.Replace(">", "&gt;");
            theString = theString.Replace("<", "&lt;");
            theString = theString.Replace("  ", " &nbsp;");
            theString = theString.Replace("  ", " &nbsp;");
            theString = theString.Replace("\"", "&quot;");
            theString = theString.Replace("\'", "&#39;");
            theString = theString.Replace("\n", "<br/> ");
            theString = theString.Replace("'", "");
            theString = theString.Replace("//", "／／");
            theString = theString.Replace("http", "ｈｔｔｐ");
            theString = theString.Replace("js", "ｊｓ");
            theString = theString.Replace("gif", "ｇｉｆ");
            theString = theString.Replace("com", "ｃｏｍ");
            theString = theString.Replace(".", "．");
            return theString;
        }
        else
            return string.Empty;
        #endregion
    }
    public static string SqlEncode(string str)
    {
        #region
        str = str.Replace("'", "''");
        return str;
        #endregion
    }
    /// <summary>
    /// 功能描述：从字符串中的尾部删除指定的字符串。
    /// Remove(string sourceString, string removedString)
    /// </summary>
    /// <param name="sourceString">原字符串</param>
    /// <param name="removedString">移除字符串</param>
    /// <returns>留下的字符串</returns>
    public static string Remove(string sourceString, string removedString)
    {
        #region
        try
        {
            if (sourceString.IndexOf(removedString) == -1)
                throw new Exception("原字符串中不包含移除字符串！");
            string result = sourceString;
            int LengthOfsourceString = sourceString.Length;
            int LengthOfremovedString = removedString.Length;
            int startIndex = LengthOfsourceString - LengthOfremovedString;
            string sourceStringSub = sourceString.Substring(startIndex);
            if (sourceStringSub.ToUpper() == removedString.ToUpper())
            {
                result = sourceString.Remove(startIndex, LengthOfremovedString);
            }
            return result;
        }
        catch
        {
            return sourceString;
        }
        #endregion
    }
    /// <summary>
    /// 功能描述：从字符串中的指定位置删除指定的字符串。
    /// StrRemove(string sourceString, string removedString)
    /// </summary>
    /// <param name="sourceString">原字符串</param>
    /// <param name="removedString">移除字符串</param>
    /// <returns>留下的字符串</returns>
    public static string StrRemove(string sourceString, string removedString)
    {
        #region
        try
        {
            if (sourceString.IndexOf(removedString) == -1)
                throw new Exception("原字符串中不包含移除字符串！");
            string result = sourceString;
            int LengthOfremovedString = removedString.Length;
            if (sourceString.IndexOf(removedString) > 0)
            {
                int startIndex = sourceString.IndexOf(removedString);
                result = sourceString.Remove(startIndex, LengthOfremovedString);
            }
            return result;
        }
        catch
        {
            return sourceString;
        }
        #endregion
    }
    /// <summary>
    /// 拆分字符穿串
    /// GetStrArray(string str, char speater, bool toLower)
    /// </summary>
    /// <param name="str">原字符串</param>
    /// <param name="speater">分隔符</param>
    /// <param name="toLower">是否转换为小写</param>
    /// <returns></returns>
    #region
    public static List<string> GetStrArray(string str, char speater, bool toLower)
    {
        List<string> list = new List<string>();
        string[] ss = str.Split(speater);
        foreach (string s in ss)
        {
            if (!string.IsNullOrEmpty(s) && s != speater.ToString())
            {
                string strVal = s;
                if (toLower)
                {
                    strVal = s.ToLower();
                }
                list.Add(strVal);
            }
        }
        return list;
    }
    public static string[] GetStrArray(string str)
    {
        return str.Split(new char[',']);
    }
    public static string GetArrayStr(List<string> list, string speater)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {
            if (i == list.Count - 1)
            {
                sb.Append(list[i]);
            }
            else
            {
                sb.Append(list[i]);
                sb.Append(speater);
            }
        }
        return sb.ToString();
    }
    #endregion
    /// <summary>
    /// 功能描述：获取拆分符右边的字符串。
    /// RightSplit(string sourceString, char splitChar)
    /// </summary>
    /// <param name="sourceString">原字符串</param>
    /// <param name="splitChar">拆分字符</param>
    /// <returns>右边的字符串</returns>
    public static string RightSplit(string sourceString, char splitChar)
    {
        #region
        string result = null;
        string[] tempStr = sourceString.Split(splitChar);
        if (tempStr.Length > 0)
        {
            result = tempStr[tempStr.Length - 1].ToString();
        }
        return result;
        #endregion
    }
    /// <summary>
    /// 功能描述：获取拆分符左边的字符串。
    /// LeftSplit(string sourceString, char splitChar)
    /// </summary>
    /// <param name="sourceString">原字符串</param>
    /// <param name="splitChar">拆分字符</param>
    /// <returns>左边的字符串</returns>
    public static string LeftSplit(string sourceString, char splitChar)
    {
        #region
        string result = null;
        string[] tempStr = sourceString.Split(splitChar);
        if (tempStr.Length > 0)
        {
            result = tempStr[0].ToString();
        }
        return result;
        #endregion
    }
    /// <summary>
    /// 功能描述：去掉最后一个逗号后面的字符串。
    /// DelLsatComma(string sourceString)
    /// </summary>
    /// <param name="sourceString">原字符串</param>
    /// <param name="splitChar">拆分字符</param>
    /// <returns>左边的字符串</returns>
    public static string DelLsatComma(string sourceString)
    {
        #region
        if (sourceString.IndexOf(",") == -1)
        {
            return sourceString;
        }
        return sourceString.Substring(0, sourceString.LastIndexOf(","));
        #endregion
    }
    /// <summary>
    /// 功能描述：删除不可见字符。
    /// DeleteUnVisibleChar(string sourceString)
    /// </summary>
    /// <param name="sourceString">原字符串</param>
    /// <returns></returns>
    public static string DeleteUnVisibleChar(string sourceString)
    {
        #region
        System.Text.StringBuilder strBuilder = new System.Text.StringBuilder(131);
        for (int i = 0; i < sourceString.Length; i++)
        {
            int Unicode = sourceString[i];
            if (Unicode >= 16)
            {
                strBuilder.Append(sourceString[i].ToString());
            }
        }
        return strBuilder.ToString();
        #endregion
    }
    /// <summary>
    /// 功能描述：获取数组元素的合并字符串。
    ///  GetArrayString(string[] stringArray)
    /// </summary>
    /// <param name="stringArray">字符串数组</param>
    /// <returns></returns>
    public static string GetArrayString(string[] stringArray)
    {
        #region
        string totalString = null;
        for (int i = 0; i < stringArray.Length; i++)
        {
            totalString = totalString + stringArray[i];
        }
        return totalString;
        #endregion
    }
    /// <summary>
    /// 功能描述：获取某一字符串在字符串数组中出现的次数。
    ///  GetStringCount(string[] stringArray, string findString)
    /// </summary>
    /// <param name="stringArray">字符串数组</param>
    /// <param name="findString">某一字符串</param>
    /// <returns></returns>
    public static int GetStringCount(string[] stringArray, string findString)
    {
        #region
        int count = 0;
        string totalString = GetArrayString(stringArray);
        int findStringLength = findString.Length;
        string subString = totalString;
        while (subString.IndexOf(findString) >= 0)
        {
            subString = subString.Substring(subString.IndexOf(findString) + findStringLength);
            count += 1;
        }
        return count;
        #endregion
    }
    /// <summary>
    /// 功能描述：获取某一字符串在字符串中出现的次数。
    ///  GetStringCount(string sourceString, string findString)
    /// </summary>
    /// <param name="sourceString">字符串</param>
    /// <param name="findString">某一字符串</param>
    /// <returns></returns>
    public static int GetStringCount(string sourceString, string findString)
    {
        #region
        int count = 0;
        int findStringLength = findString.Length;
        string subString = sourceString;
        while (subString.IndexOf(findString) >= 0)
        {
            subString = subString.Substring(subString.IndexOf(findString) + findStringLength);
            count += 1;
        }
        return count;
        #endregion
    }
    /// <summary>
    /// 功能描述：截取从startString开始到原字符串结尾的所有字符。
    /// GetSubstring(string sourceString, string startString)
    /// </summary>
    /// <param name="sourceString">字符串</param>
    /// <param name="startString">某一字符串</param>
    /// <returns></returns>
    public static string GetSubstring(string sourceString, string startString)
    {
        #region
        int startIndex = sourceString.IndexOf(startString);
        if (startIndex > 0)
            return sourceString.Substring(startIndex);
        return sourceString;
        #endregion
    }
    /// <summary>
    /// 功能描述：按字节数取出字符串的长度。
    ///  public static int GetByteCount(string sourceString)
    /// </summary>
    /// <param name="sourceString">要计算的字符串</param>
    /// <returns>字符串的字节数</returns>
    public static int GetByteCount(string sourceString)
    {
        #region
        int itnCharCount = 0;
        for (int i = 0; i < sourceString.Length; i++)
        {
            if (System.Text.UTF8Encoding.UTF8.GetByteCount(sourceString.Substring(i, 1)) == 3)
            {
                itnCharCount = itnCharCount + 2;
            }
            else
            {
                itnCharCount = itnCharCount + 1;
            }
        }
        return itnCharCount;
        #endregion
    }
    /// <summary>
    /// 功能描述：按字节数要在字符串的位置。
    ///  GetByteIndex(int intIns, string strTmp)
    /// </summary>
    /// <param name="intIns">字符串的位置</param>
    /// <param name="strTmp">要计算的字符串</param>
    /// <returns>字节的位置</returns>
    public static int GetByteIndex(int intIns, string strTmp)
    {
        #region
        int intReIns = 0;
        if (strTmp.Trim() == "")
        {
            return intIns;
        }
        for (int i = 0; i < strTmp.Length; i++)
        {
            if (System.Text.UTF8Encoding.UTF8.GetByteCount(strTmp.Substring(i, 1)) == 3)
            {
                intReIns = intReIns + 2;
            }
            else
            {
                intReIns = intReIns + 1;
            }
            if (intReIns >= intIns)
            {
                intReIns = i + 1;
                break;
            }
        }
        return intReIns;
        #endregion
    }
    /// <summary>
    /// 返回路径分割符号"\"最后一个字符串。
    /// getLastStr(string sourceStr, char splitChar)
    /// </summary>
    /// <param name="sourceStr">原字符串</param>
    /// <param name="splitChar">分割符号</param>
    /// <returns></returns>
    public static string getLastStr(string sourceStr, char splitChar)
    {
        #region
        string[] strArr = sourceStr.Split(splitChar);
        string lastStr = strArr[strArr.Length - 1];
        if (lastStr == "")
            return strArr[strArr.Length - 2];
        return lastStr;
        #endregion
    }
    /// <summary>
    /// 获取路径最后一个分隔符"\"左边的全部字符串
    /// getLeftStr(string sourceStr, char splitChar)
    /// </summary>
    /// <param name="sourceStr"></param>
    /// <param name="splitChar">分隔符</param>
    /// <returns></returns>
    public static string getLeftStr(string sourceStr, char splitChar)
    {
        #region
        string[] strArr = sourceStr.Split(splitChar);
        int length = sourceStr.Length - getLastStr(sourceStr, '\\').Length;
        string leftStr = sourceStr.Substring(0, length);
        return leftStr;
        #endregion
    }
    /// <summary>
    /// 出去空格，并见对尖括号内的空格加入。
    /// ReplaceStr(string contStr)
    /// </summary>
    /// <param name="contStr"></param>
    /// <returns></returns>
    public static string ReplaceStr(string contStr)
    {
        #region
        contStr = System.Text.RegularExpressions.Regex.Replace(contStr, "\\s", "&nbsp;");
        int y, z;
        z = 0;
        int i = 0;
        do
        {
            y = contStr.IndexOf("<", z);
            if (y >= 0)
            {
                z = contStr.IndexOf(">", y);
                if (z >= 0)
                {
                    i += contStr.Substring(y, z - y + 1).Replace("&nbsp;", " ").Length + 4; //统计超级链接标签占用的字符数
                    string tStr1, tStr2;
                    tStr1 = contStr.Substring(0, z + 1);
                    tStr2 = contStr.Substring(z + 1);
                    contStr = tStr1.Replace(tStr1.Substring(y, z - y + 1), tStr1.Substring(y, z - y + 1).Replace("&nbsp;", " ")) + tStr2;
                }
                else
                {

                    z = y + 1;
                    if (z > contStr.Length - 1)
                        break;
                }
            }
        }
        while (y >= 0 && z <= contStr.Length - 1);
        return contStr;
        #endregion
    }
    /// <summary> 
    /// 转换人民币大小金额 
    /// CmycurD(decimal num)
    /// </summary> 
    /// <param name="num">金额</param> 
    /// <returns>返回大写形式</returns> 
    public static string CmycurD(decimal num)
    {
        #region
        string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字 
        string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
        string str3 = "";    //从原num值中取出的值 
        string str4 = "";    //数字的字符串形式 
        string str5 = "";  //人民币大写金额形式 
        int i;    //循环变量 
        int j;    //num的值乘以100的字符串长度 
        string ch1 = "";    //数字的汉语读法 
        string ch2 = "";    //数字位的汉字读法 
        int nzero = 0;  //用来计算连续的零值是几个 
        int temp;            //从原num值中取出的值 

        num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数 
        str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式 
        j = str4.Length;      //找出最高位 
        if (j > 15) { return "溢出"; }
        str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分 

        //循环取出每一位需要转换的值 
        for (i = 0; i < j; i++)
        {
            str3 = str4.Substring(i, 1);          //取出需转换的某一位的值 
            temp = Convert.ToInt32(str3);      //转换为数字 
            if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
            {
                //当所取位数不为元、万、亿、万亿上的数字时 
                if (str3 == "0")
                {
                    ch1 = "";
                    ch2 = "";
                    nzero = nzero + 1;
                }
                else
                {
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        ch1 = str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                }
            }
            else
            {
                //该位是万亿，亿，万，元位等关键位 
                if (str3 != "0" && nzero != 0)
                {
                    ch1 = "零" + str1.Substring(temp * 1, 1);
                    ch2 = str2.Substring(i, 1);
                    nzero = 0;
                }
                else
                {
                    if (str3 != "0" && nzero == 0)
                    {
                        ch1 = str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 == "0" && nzero >= 3)
                        {
                            ch1 = "";
                            ch2 = "";
                            nzero = nzero + 1;
                        }
                        else
                        {
                            if (j >= 11)
                            {
                                ch1 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                ch1 = "";
                                ch2 = str2.Substring(i, 1);
                                nzero = nzero + 1;
                            }
                        }
                    }
                }
            }
            if (i == (j - 11) || i == (j - 3))
            {
                //如果该位是亿位或元位，则必须写上 
                ch2 = str2.Substring(i, 1);
            }
            str5 = str5 + ch1 + ch2;

            if (i == j - 1 && str3 == "0")
            {
                //最后一位（分）为0时，加上“整” 
                str5 = str5 + '整';
            }
        }
        if (num == 0)
        {
            str5 = "零元整";
        }
        return str5;
        #endregion
    }
    /// <summary> 
    /// 一个重载，将字符串先转换成数字在调用CmycurD(decimal num) 
    /// CmycurD(string numstr)
    /// </summary> 
    /// <param name="num">用户输入的金额，字符串形式未转成decimal</param> 
    /// <returns></returns> 
    public static string CmycurD(string numstr)
    {
        #region
        try
        {
            decimal num = Convert.ToDecimal(numstr);
            return CmycurD(num);
        }
        catch
        {
            return "非数字形式！";
        }
        #endregion
    }
    /// <summary>
    /// 匹配html代码中的图片地址
    ///  public static string GetHtmlImgUrl(string str)
    /// </summary>
    /// <param name="str">html字符串</param>
    /// <returns>返回匹配的最后一个图片地址</returns>
    public static string GetHtmlImgUrl(string str)
    {
        if (str.Trim() != "")
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(?i)<img[^>]*?src=(['""\s]?)([^'""\s]+)\1[^>]*?>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.MatchCollection match = reg.Matches(str);
            string ImgUrl = "";
            foreach (System.Text.RegularExpressions.Match item in match)
            {
                ImgUrl = item.Groups[2].Value;
            }
            return ImgUrl;
        }
        else
            return "";
    }
    /// <summary>
    /// 取得HTML中所有图片的 URL。
    ///  public static string[] GetHtmlImgsUrl(string sHtmlText)
    /// </summary>
    /// <param name="sHtmlText">HTML代码</param>
    /// <returns>图片的URL列表</returns>
    public static string[] GetHtmlImgsUrl(string sHtmlText)
    {
        System.Text.RegularExpressions.Regex regImg = new System.Text.RegularExpressions.Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        System.Text.RegularExpressions.MatchCollection matches = regImg.Matches(sHtmlText);
        int i = 0;
        string[] sUrlList = new string[matches.Count];
        foreach (System.Text.RegularExpressions.Match match in matches)
            sUrlList[i++] = match.Groups["imgUrl"].Value;
        return sUrlList;
    }
    /// <summary>
    /// 判断Session值是否为空，为空则跳转到StrUrl页
    /// </summary>
    /// <param name="SessionName">Session名</param>
    /// <param name="StrUrl">跳转到的路径</param>
    public static void Session_Null_Judge(string SessionName, string StrUrl)
    {
        //未登录跳转到登录页
        if (System.Web.HttpContext.Current.Session[SessionName] == null || System.Web.HttpContext.Current.Session[SessionName].ToString() == "")
        {

            System.Web.HttpContext.Current.Response.Write("<script language='javascript'>if(window.parent.opener!=null ) {parent.opener.parent.document.location='" + StrUrl + "';parent.close();}else{ window.top.location='" + StrUrl + "';} </script>");
            System.Web.HttpContext.Current.Response.End();
        }
    }

    public static void Session_Null_Judge(string SessionName, string AlertInfo, string StrUrl)
    {
        //未登录跳转到登录页
        if (System.Web.HttpContext.Current.Session[SessionName] == null || System.Web.HttpContext.Current.Session[SessionName].ToString() == "")
        {
            System.Web.HttpContext.Current.Response.Write("<script language='javascript'>alert('" + AlertInfo + "');if(window.parent.opener!=null ) {parent.opener.parent.document.location='" + StrUrl + "';parent.close();}else{ window.top.location='" + StrUrl + "';} </script>");
            System.Web.HttpContext.Current.Response.End();
        }
    }

    public static void Session_Null_Judge(string SessionName, string AlertInfo, bool IsClose)
    {
        //未登录跳转到登录页
        if (System.Web.HttpContext.Current.Session[SessionName] == null || System.Web.HttpContext.Current.Session[SessionName].ToString() == "")
        {
            string js = IsClose
                ? (@"<script language='javascript'>alert('" + AlertInfo + "');window.close();</script>")
                : (@"<script language='javascript'>alert('" + AlertInfo + "');</script>");

            System.Web.HttpContext.Current.Response.Write(js);
            System.Web.HttpContext.Current.Response.End();
        }
    }

    public static void Session_Null_Judge(string SessionName, string AlertInfo, string StrUrl, bool IsSelf)
    {
        //未登录跳转到登录页
        if (System.Web.HttpContext.Current.Session[SessionName] == null || System.Web.HttpContext.Current.Session[SessionName].ToString() == "")
        {
            string js = "";
            if (IsSelf)
            {
                js = @" <script language='javascript'>

                            alert('" + AlertInfo + @"');
                            window.location.reload('" + StrUrl + @"');

                            </script>";
            }
            else
            {
                js = @"<script language='javascript'>

                            alert('" + AlertInfo + @"'); 

                            if(window.parent.opener!=null ) 
                            {
                                parent.opener.parent.document.location='" + StrUrl + @"';
                                parent.close();
                            }
                            else
                            {
                                window.top.location='" + StrUrl + @"';
                            } 

                            </script>";
            }

            System.Web.HttpContext.Current.Response.Write(js);
            System.Web.HttpContext.Current.Response.End();
        }
    }

    /// <summary>
    /// 汉字转拼音类
    /// </summary>
    //定义拼音区编码数组
    #region
    private static int[] getValue = new int[]
            {
                -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
                -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
                -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
                -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
                -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
                -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
                -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
                -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
                -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
                -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
                -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
                -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
                -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
                -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
                -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
                -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
                -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
                -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
                -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
                -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
                -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
                -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
                -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
                -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
                -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
                -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
                -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
                -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
                -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
                -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
                -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
                -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
                -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
            };

    //定义拼音数组
    private static string[] getName = new string[]
            {
                "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
                "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
                "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
                "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
                "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
                "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
                "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
                "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
                "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
                "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
                "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
                "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
                "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
                "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
                "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
                "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
                "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
                "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
                "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
                "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
                "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
                "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
                "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
                "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
                "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
                "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
                "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
                "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
                "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
                "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
                "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
                "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
                "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
           };
    /// <summary>
    /// 汉字转换成全拼的拼音
    /// convertCh(string Chstr)
    /// </summary>
    /// <param name="Chstr">汉字字符串</param>
    /// <returns>转换后的拼音字符串</returns>
    public static string convertCh(string Chstr)
    {
        Regex reg = new Regex("^[\u4e00-\u9fa5]$");//验证是否输入汉字
        byte[] arr = new byte[2];
        string pystr = "";
        int asc = 0, M1 = 0, M2 = 0;
        char[] mChar = Chstr.ToCharArray();//获取汉字对应的字符数组
        for (int j = 0; j < mChar.Length; j++)
        {
            //如果输入的是汉字
            if (reg.IsMatch(mChar[j].ToString()))
            {
                arr = System.Text.Encoding.Default.GetBytes(mChar[j].ToString());
                M1 = (short)(arr[0]);
                M2 = (short)(arr[1]);
                asc = M1 * 256 + M2 - 65536;
                if (asc > 0 && asc < 160)
                {
                    pystr += mChar[j];
                }
                else
                {
                    switch (asc)
                    {
                        case -9254:
                            pystr += "Zhen"; break;
                        case -8985:
                            pystr += "Qian"; break;
                        case -5463:
                            pystr += "Jia"; break;
                        case -8274:
                            pystr += "Ge"; break;
                        case -5448:
                            pystr += "Ga"; break;
                        case -5447:
                            pystr += "La"; break;
                        case -4649:
                            pystr += "Chen"; break;
                        case -5436:
                            pystr += "Mao"; break;
                        case -5213:
                            pystr += "Mao"; break;
                        case -3597:
                            pystr += "Die"; break;
                        case -5659:
                            pystr += "Tian"; break;
                        default:
                            for (int i = (getValue.Length - 1); i >= 0; i--)
                            {
                                if (getValue[i] <= asc) //判断汉字的拼音区编码是否在指定范围内
                                {
                                    pystr += getName[i];//如果不超出范围则获取对应的拼音
                                    break;
                                }
                            }
                            break;
                    }
                }
            }
            else//如果不是汉字
            {
                pystr += mChar[j].ToString();//如果不是汉字则返回
            }
        }
        return pystr;//返回获取到的汉字拼音
    }
    #endregion
    /// <summary>
    /// 取汉字拼音的首字母
    /// GetFirstLetter(string hz)
    /// </summary>
    /// <param name="UnName">汉字</param>
    /// <returns>首字母</returns>
    public static string GetFirstLetter(string hz)
    {
        string ls_second_eng = "CJWGNSPGCGNESYPBTYYZDXYKYGTDJNNJQMBSGZSCYJSYYQPGKBZGYCYWJKGKLJSWKPJQHYTWDDZLSGMRYPYWWCCKZNKYDGTTNGJEYKKZYTCJNMCYLQLYPYQFQRPZSLWBTGKJFYXJWZLTBNCXJJJJZXDTTSQZYCDXXHGCKBPHFFSSWYBGMXLPBYLLLHLXSPZMYJHSOJNGHDZQYKLGJHSGQZHXQGKEZZWYSCSCJXYEYXADZPMDSSMZJZQJYZCDJZWQJBDZBXGZNZCPWHKXHQKMWFBPBYDTJZZKQHYLYGXFPTYJYYZPSZLFCHMQSHGMXXSXJJSDCSBBQBEFSJYHWWGZKPYLQBGLDLCCTNMAYDDKSSNGYCSGXLYZAYBNPTSDKDYLHGYMYLCXPYCJNDQJWXQXFYYFJLEJBZRXCCQWQQSBNKYMGPLBMJRQCFLNYMYQMSQTRBCJTHZTQFRXQ" +
     "HXMJJCJLXQGJMSHZKBSWYEMYLTXFSYDSGLYCJQXSJNQBSCTYHBFTDCYZDJWYGHQFRXWCKQKXEBPTLPXJZSRMEBWHJLBJSLYYSMDXLCLQKXLHXJRZJMFQHXHWYWSBHTRXXGLHQHFNMNYKLDYXZPWLGGTMTCFPAJJZYLJTYANJGBJPLQGDZYQYAXBKYSECJSZNSLYZHZXLZCGHPXZHZNYTDSBCJKDLZAYFMYDLEBBGQYZKXGLDNDNYSKJSHDLYXBCGHXYPKDJMMZNGMMCLGWZSZXZJFZNMLZZTHCSYDBDLLSCDDNLKJYKJSYCJLKOHQASDKNHCSGANHDAASHTCPLCPQYBSDMPJLPCJOQLCDHJJYSPRCHNWJNLHLYYQYYWZPTCZGWWMZFFJQQQQYXACLBHKDJXDGMMYDJXZLLSYGXGKJRYWZWYCLZMSSJZLDBYDCFCXYHLXCHYZJQSFQAGMNYXPFRKSSB" +
     "JLYXYSYGLNSCMHCWWMNZJJLXXHCHSYDSTTXRYCYXBYHCSMXJSZNPWGPXXTAYBGAJCXLYSDCCWZOCWKCCSBNHCPDYZNFCYYTYCKXKYBSQKKYTQQXFCWCHCYKELZQBSQYJQCCLMTHSYWHMKTLKJLYCXWHEQQHTQHZPQSQSCFYMMDMGBWHWLGSSLYSDLMLXPTHMJHWLJZYHZJXHTXJLHXRSWLWZJCBXMHZQXSDZPMGFCSGLSXYMJSHXPJXWMYQKSMYPLRTHBXFTPMHYXLCHLHLZYLXGSSSSTCLSLDCLRPBHZHXYYFHBBGDMYCNQQWLQHJJZYWJZYEJJDHPBLQXTQKWHLCHQXAGTLXLJXMSLXHTZKZJECXJCJNMFBYCSFYWYBJZGNYSDZSQYRSLJPCLPWXSDWEJBJCBCNAYTWGMPAPCLYQPCLZXSBNMSGGFNZJJBZSFZYNDXHPLQKZCZWALSBCCJXJYZGWKYP" +
     "SGXFZFCDKHJGXDLQFSGDSLQWZKXTMHSBGZMJZRGLYJBPMLMSXLZJQQHZYJCZYDJWBMJKLDDPMJEGXYHYLXHLQYQHKYCWCJMYYXNATJHYCCXZPCQLBZWWYTWBQCMLPMYRJCCCXFPZNZZLJPLXXYZTZLGDLDCKLYRZZGQTGJHHHJLJAXFGFJZSLCFDQZLCLGJDJCSNCLLJPJQDCCLCJXMYZFTSXGCGSBRZXJQQCTZHGYQTJQQLZXJYLYLBCYAMCSTYLPDJBYREGKLZYZHLYSZQLZNWCZCLLWJQJJJKDGJZOLBBZPPGLGHTGZXYGHZMYCNQSYCYHBHGXKAMTXYXNBSKYZZGJZLQJDFCJXDYGJQJJPMGWGJJJPKQSBGBMMCJSSCLPQPDXCDYYKYFCJDDYYGYWRHJRTGZNYQLDKLJSZZGZQZJGDYKSHPZMTLCPWNJAFYZDJCNMWESCYGLBTZCGMSSLLYXQSXSBSJS" +
     "BBSGGHFJLWPMZJNLYYWDQSHZXTYYWHMCYHYWDBXBTLMSYYYFSXJCSDXXLHJHFSSXZQHFZMZCZTQCXZXRTTDJHNNYZQQMNQDMMGYYDXMJGDHCDYZBFFALLZTDLTFXMXQZDNGWQDBDCZJDXBZGSQQDDJCMBKZFFXMKDMDSYYSZCMLJDSYNSPRSKMKMPCKLGDBQTFZSWTFGGLYPLLJZHGJJGYPZLTCSMCNBTJBQFKTHBYZGKPBBYMTTSSXTBNPDKLEYCJNYCDYKZDDHQHSDZSCTARLLTKZLGECLLKJLQJAQNBDKKGHPJTZQKSECSHALQFMMGJNLYJBBTMLYZXDCJPLDLPCQDHZYCBZSCZBZMSLJFLKRZJSNFRGJHXPDHYJYBZGDLQCSEZGXLBLGYXTWMABCHECMWYJYZLLJJYHLGBDJLSLYGKDZPZXJYYZLWCXSZFGWYYDLYHCLJSCMBJHBLYZLYCBLYDPDQYSXQZB" +
     "YTDKYXJYYCNRJMDJGKLCLJBCTBJDDBBLBLCZQRPXJCGLZCSHLTOLJNMDDDLNGKAQHQHJGYKHEZNMSHRPHQQJCHGMFPRXHJGDYCHGHLYRZQLCYQJNZSQTKQJYMSZSWLCFQQQXYFGGYPTQWLMCRNFKKFSYYLQBMQAMMMYXCTPSHCPTXXZZSMPHPSHMCLMLDQFYQXSZYJDJJZZHQPDSZGLSTJBCKBXYQZJSGPSXQZQZRQTBDKYXZKHHGFLBCSMDLDGDZDBLZYYCXNNCSYBZBFGLZZXSWMSCCMQNJQSBDQSJTXXMBLTXZCLZSHZCXRQJGJYLXZFJPHYMZQQYDFQJJLZZNZJCDGZYGCTXMZYSCTLKPHTXHTLBJXJLXSCDQXCBBTJFQZFSLTJBTKQBXXJJLJCHCZDBZJDCZJDCPRNPQCJPFCZLCLZXZDMXMPHJSGZGSZZQJYLWTJPFSYASMCJBTZKYCWMYTCSJJLJCQLWZM" +
     "ALBXYFBPNLSFHTGJWEJJXXGLLJSTGSHJQLZFKCGNNDSZFDEQFHBSAQTGLLBXMMYGSZLDYDQMJJRGBJTKGDHGKBLQKBDMBYLXWCXYTTYBKMRTJZXQJBHLMHMJJZMQASLDCYXYQDLQCAFYWYXQHZ";

        string ls_second_ch = "亍丌兀丐廿卅丕亘丞鬲孬噩丨禺丿匕乇夭爻卮氐囟胤馗毓睾鼗丶亟" +
     "鼐乜乩亓芈孛啬嘏仄厍厝厣厥厮靥赝匚叵匦匮匾赜卦卣刂刈刎刭刳刿剀剌剞剡剜蒯剽劂劁劐劓冂罔亻仃仉仂仨仡仫仞伛仳伢佤仵伥伧伉伫佞佧攸佚佝佟佗伲伽佶佴侑侉侃侏佾佻侪佼侬侔俦俨俪俅俚俣俜俑俟俸倩偌俳倬倏倮倭俾倜倌倥倨偾偃偕偈偎偬偻傥傧傩傺僖儆僭僬僦僮儇儋仝氽佘佥俎龠汆籴兮巽黉馘冁夔勹匍訇匐凫夙兕亠兖亳衮袤亵脔裒禀嬴蠃羸冫冱冽冼凇冖冢冥讠讦讧讪讴讵讷诂诃诋诏诎诒诓诔诖诘诙诜诟诠诤诨诩诮诰诳诶诹诼诿谀谂谄谇谌谏谑谒谔谕谖谙谛谘谝谟谠谡谥谧谪谫谮谯谲谳谵谶卩卺阝阢阡阱阪阽阼" +
     "陂陉陔陟陧陬陲陴隈隍隗隰邗邛邝邙邬邡邴邳邶邺邸邰郏郅邾郐郄郇郓郦郢郜郗郛郫郯郾鄄鄢鄞鄣鄱鄯鄹酃酆刍奂劢劬劭劾哿勐勖勰叟燮矍廴凵凼鬯厶弁畚巯坌垩垡塾墼壅壑圩圬圪圳圹圮圯坜圻坂坩垅坫垆坼坻坨坭坶坳垭垤垌垲埏垧垴垓垠埕埘埚埙埒垸埴埯埸埤埝堋堍埽埭堀堞堙塄堠塥塬墁墉墚墀馨鼙懿艹艽艿芏芊芨芄芎芑芗芙芫芸芾芰苈苊苣芘芷芮苋苌苁芩芴芡芪芟苄苎芤苡茉苷苤茏茇苜苴苒苘茌苻苓茑茚茆茔茕苠苕茜荑荛荜茈莒茼茴茱莛荞茯荏荇荃荟荀茗荠茭茺茳荦荥荨茛荩荬荪荭荮莰荸莳莴莠莪莓莜莅荼莶莩荽莸荻" +
     "莘莞莨莺莼菁萁菥菘堇萘萋菝菽菖萜萸萑萆菔菟萏萃菸菹菪菅菀萦菰菡葜葑葚葙葳蒇蒈葺蒉葸萼葆葩葶蒌蒎萱葭蓁蓍蓐蓦蒽蓓蓊蒿蒺蓠蒡蒹蒴蒗蓥蓣蔌甍蔸蓰蔹蔟蔺蕖蔻蓿蓼蕙蕈蕨蕤蕞蕺瞢蕃蕲蕻薤薨薇薏蕹薮薜薅薹薷薰藓藁藜藿蘧蘅蘩蘖蘼廾弈夼奁耷奕奚奘匏尢尥尬尴扌扪抟抻拊拚拗拮挢拶挹捋捃掭揶捱捺掎掴捭掬掊捩掮掼揲揸揠揿揄揞揎摒揆掾摅摁搋搛搠搌搦搡摞撄摭撖摺撷撸撙撺擀擐擗擤擢攉攥攮弋忒甙弑卟叱叽叩叨叻吒吖吆呋呒呓呔呖呃吡呗呙吣吲咂咔呷呱呤咚咛咄呶呦咝哐咭哂咴哒咧咦哓哔呲咣哕咻咿哌哙哚哜咩" +
     "咪咤哝哏哞唛哧唠哽唔哳唢唣唏唑唧唪啧喏喵啉啭啁啕唿啐唼唷啖啵啶啷唳唰啜喋嗒喃喱喹喈喁喟啾嗖喑啻嗟喽喾喔喙嗪嗷嗉嘟嗑嗫嗬嗔嗦嗝嗄嗯嗥嗲嗳嗌嗍嗨嗵嗤辔嘞嘈嘌嘁嘤嘣嗾嘀嘧嘭噘嘹噗嘬噍噢噙噜噌噔嚆噤噱噫噻噼嚅嚓嚯囔囗囝囡囵囫囹囿圄圊圉圜帏帙帔帑帱帻帼帷幄幔幛幞幡岌屺岍岐岖岈岘岙岑岚岜岵岢岽岬岫岱岣峁岷峄峒峤峋峥崂崃崧崦崮崤崞崆崛嵘崾崴崽嵬嵛嵯嵝嵫嵋嵊嵩嵴嶂嶙嶝豳嶷巅彳彷徂徇徉後徕徙徜徨徭徵徼衢彡犭犰犴犷犸狃狁狎狍狒狨狯狩狲狴狷猁狳猃狺狻猗猓猡猊猞猝猕猢猹猥猬猸猱獐獍獗獠獬獯獾" +
     "舛夥飧夤夂饣饧饨饩饪饫饬饴饷饽馀馄馇馊馍馐馑馓馔馕庀庑庋庖庥庠庹庵庾庳赓廒廑廛廨廪膺忄忉忖忏怃忮怄忡忤忾怅怆忪忭忸怙怵怦怛怏怍怩怫怊怿怡恸恹恻恺恂恪恽悖悚悭悝悃悒悌悛惬悻悱惝惘惆惚悴愠愦愕愣惴愀愎愫慊慵憬憔憧憷懔懵忝隳闩闫闱闳闵闶闼闾阃阄阆阈阊阋阌阍阏阒阕阖阗阙阚丬爿戕氵汔汜汊沣沅沐沔沌汨汩汴汶沆沩泐泔沭泷泸泱泗沲泠泖泺泫泮沱泓泯泾洹洧洌浃浈洇洄洙洎洫浍洮洵洚浏浒浔洳涑浯涞涠浞涓涔浜浠浼浣渚淇淅淞渎涿淠渑淦淝淙渖涫渌涮渫湮湎湫溲湟溆湓湔渲渥湄滟溱溘滠漭滢溥溧溽溻溷滗溴滏溏滂" +
     "溟潢潆潇漤漕滹漯漶潋潴漪漉漩澉澍澌潸潲潼潺濑濉澧澹澶濂濡濮濞濠濯瀚瀣瀛瀹瀵灏灞宀宄宕宓宥宸甯骞搴寤寮褰寰蹇謇辶迓迕迥迮迤迩迦迳迨逅逄逋逦逑逍逖逡逵逶逭逯遄遑遒遐遨遘遢遛暹遴遽邂邈邃邋彐彗彖彘尻咫屐屙孱屣屦羼弪弩弭艴弼鬻屮妁妃妍妩妪妣妗姊妫妞妤姒妲妯姗妾娅娆姝娈姣姘姹娌娉娲娴娑娣娓婀婧婊婕娼婢婵胬媪媛婷婺媾嫫媲嫒嫔媸嫠嫣嫱嫖嫦嫘嫜嬉嬗嬖嬲嬷孀尕尜孚孥孳孑孓孢驵驷驸驺驿驽骀骁骅骈骊骐骒骓骖骘骛骜骝骟骠骢骣骥骧纟纡纣纥纨纩纭纰纾绀绁绂绉绋绌绐绔绗绛绠绡绨绫绮绯绱绲缍绶绺绻绾缁缂缃" +
     "缇缈缋缌缏缑缒缗缙缜缛缟缡缢缣缤缥缦缧缪缫缬缭缯缰缱缲缳缵幺畿巛甾邕玎玑玮玢玟珏珂珑玷玳珀珉珈珥珙顼琊珩珧珞玺珲琏琪瑛琦琥琨琰琮琬琛琚瑁瑜瑗瑕瑙瑷瑭瑾璜璎璀璁璇璋璞璨璩璐璧瓒璺韪韫韬杌杓杞杈杩枥枇杪杳枘枧杵枨枞枭枋杷杼柰栉柘栊柩枰栌柙枵柚枳柝栀柃枸柢栎柁柽栲栳桠桡桎桢桄桤梃栝桕桦桁桧桀栾桊桉栩梵梏桴桷梓桫棂楮棼椟椠棹椤棰椋椁楗棣椐楱椹楠楂楝榄楫榀榘楸椴槌榇榈槎榉楦楣楹榛榧榻榫榭槔榱槁槊槟榕槠榍槿樯槭樗樘橥槲橄樾檠橐橛樵檎橹樽樨橘橼檑檐檩檗檫猷獒殁殂殇殄殒殓殍殚殛殡殪轫轭轱轲轳轵轶" +
     "轸轷轹轺轼轾辁辂辄辇辋辍辎辏辘辚軎戋戗戛戟戢戡戥戤戬臧瓯瓴瓿甏甑甓攴旮旯旰昊昙杲昃昕昀炅曷昝昴昱昶昵耆晟晔晁晏晖晡晗晷暄暌暧暝暾曛曜曦曩贲贳贶贻贽赀赅赆赈赉赇赍赕赙觇觊觋觌觎觏觐觑牮犟牝牦牯牾牿犄犋犍犏犒挈挲掰搿擘耄毪毳毽毵毹氅氇氆氍氕氘氙氚氡氩氤氪氲攵敕敫牍牒牖爰虢刖肟肜肓肼朊肽肱肫肭肴肷胧胨胩胪胛胂胄胙胍胗朐胝胫胱胴胭脍脎胲胼朕脒豚脶脞脬脘脲腈腌腓腴腙腚腱腠腩腼腽腭腧塍媵膈膂膑滕膣膪臌朦臊膻臁膦欤欷欹歃歆歙飑飒飓飕飙飚殳彀毂觳斐齑斓於旆旄旃旌旎旒旖炀炜炖炝炻烀炷炫炱烨烊焐焓焖焯焱" +
     "煳煜煨煅煲煊煸煺熘熳熵熨熠燠燔燧燹爝爨灬焘煦熹戾戽扃扈扉礻祀祆祉祛祜祓祚祢祗祠祯祧祺禅禊禚禧禳忑忐怼恝恚恧恁恙恣悫愆愍慝憩憝懋懑戆肀聿沓泶淼矶矸砀砉砗砘砑斫砭砜砝砹砺砻砟砼砥砬砣砩硎硭硖硗砦硐硇硌硪碛碓碚碇碜碡碣碲碹碥磔磙磉磬磲礅磴礓礤礞礴龛黹黻黼盱眄眍盹眇眈眚眢眙眭眦眵眸睐睑睇睃睚睨睢睥睿瞍睽瞀瞌瞑瞟瞠瞰瞵瞽町畀畎畋畈畛畲畹疃罘罡罟詈罨罴罱罹羁罾盍盥蠲钅钆钇钋钊钌钍钏钐钔钗钕钚钛钜钣钤钫钪钭钬钯钰钲钴钶钷钸钹钺钼钽钿铄铈铉铊铋铌铍铎铐铑铒铕铖铗铙铘铛铞铟铠铢铤铥铧铨铪铩铫铮铯铳铴铵铷铹铼" +
     "铽铿锃锂锆锇锉锊锍锎锏锒锓锔锕锖锘锛锝锞锟锢锪锫锩锬锱锲锴锶锷锸锼锾锿镂锵镄镅镆镉镌镎镏镒镓镔镖镗镘镙镛镞镟镝镡镢镤镥镦镧镨镩镪镫镬镯镱镲镳锺矧矬雉秕秭秣秫稆嵇稃稂稞稔稹稷穑黏馥穰皈皎皓皙皤瓞瓠甬鸠鸢鸨鸩鸪鸫鸬鸲鸱鸶鸸鸷鸹鸺鸾鹁鹂鹄鹆鹇鹈鹉鹋鹌鹎鹑鹕鹗鹚鹛鹜鹞鹣鹦鹧鹨鹩鹪鹫鹬鹱鹭鹳疒疔疖疠疝疬疣疳疴疸痄疱疰痃痂痖痍痣痨痦痤痫痧瘃痱痼痿瘐瘀瘅瘌瘗瘊瘥瘘瘕瘙瘛瘼瘢瘠癀瘭瘰瘿瘵癃瘾瘳癍癞癔癜癖癫癯翊竦穸穹窀窆窈窕窦窠窬窨窭窳衤衩衲衽衿袂裆袷袼裉裢裎裣裥裱褚裼裨裾裰褡褙褓褛褊褴褫褶襁襦疋胥皲皴矜耒" +
     "耔耖耜耠耢耥耦耧耩耨耱耋耵聃聆聍聒聩聱覃顸颀颃颉颌颍颏颔颚颛颞颟颡颢颥颦虍虔虬虮虿虺虼虻蚨蚍蚋蚬蚝蚧蚣蚪蚓蚩蚶蛄蚵蛎蚰蚺蚱蚯蛉蛏蚴蛩蛱蛲蛭蛳蛐蜓蛞蛴蛟蛘蛑蜃蜇蛸蜈蜊蜍蜉蜣蜻蜞蜥蜮蜚蜾蝈蜴蜱蜩蜷蜿螂蜢蝽蝾蝻蝠蝰蝌蝮螋蝓蝣蝼蝤蝙蝥螓螯螨蟒蟆螈螅螭螗螃螫蟥螬螵螳蟋蟓螽蟑蟀蟊蟛蟪蟠蟮蠖蠓蟾蠊蠛蠡蠹蠼缶罂罄罅舐竺竽笈笃笄笕笊笫笏筇笸笪笙笮笱笠笥笤笳笾笞筘筚筅筵筌筝筠筮筻筢筲筱箐箦箧箸箬箝箨箅箪箜箢箫箴篑篁篌篝篚篥篦篪簌篾篼簏簖簋簟簪簦簸籁籀臾舁舂舄臬衄舡舢舣舭舯舨舫舸舻舳舴舾艄艉艋艏艚艟艨衾袅袈裘裟襞羝羟" +
     "羧羯羰羲籼敉粑粝粜粞粢粲粼粽糁糇糌糍糈糅糗糨艮暨羿翎翕翥翡翦翩翮翳糸絷綦綮繇纛麸麴赳趄趔趑趱赧赭豇豉酊酐酎酏酤酢酡酰酩酯酽酾酲酴酹醌醅醐醍醑醢醣醪醭醮醯醵醴醺豕鹾趸跫踅蹙蹩趵趿趼趺跄跖跗跚跞跎跏跛跆跬跷跸跣跹跻跤踉跽踔踝踟踬踮踣踯踺蹀踹踵踽踱蹉蹁蹂蹑蹒蹊蹰蹶蹼蹯蹴躅躏躔躐躜躞豸貂貊貅貘貔斛觖觞觚觜觥觫觯訾謦靓雩雳雯霆霁霈霏霎霪霭霰霾龀龃龅龆龇龈龉龊龌黾鼋鼍隹隼隽雎雒瞿雠銎銮鋈錾鍪鏊鎏鐾鑫鱿鲂鲅鲆鲇鲈稣鲋鲎鲐鲑鲒鲔鲕鲚鲛鲞鲟鲠鲡鲢鲣鲥鲦鲧鲨鲩鲫鲭鲮鲰鲱鲲鲳鲴鲵鲶鲷鲺鲻鲼鲽鳄鳅鳆鳇鳊鳋鳌鳍鳎鳏鳐鳓鳔" +
     "鳕鳗鳘鳙鳜鳝鳟鳢靼鞅鞑鞒鞔鞯鞫鞣鞲鞴骱骰骷鹘骶骺骼髁髀髅髂髋髌髑魅魃魇魉魈魍魑飨餍餮饕饔髟髡髦髯髫髻髭髹鬈鬏鬓鬟鬣麽麾縻麂麇麈麋麒鏖麝麟黛黜黝黠黟黢黩黧黥黪黯鼢鼬鼯鼹鼷鼽鼾齄";

        string return_py = "";
        byte[] array = new byte[2];

        for (int i = 0; i < hz.Length; i++)
        {
            array = System.Text.Encoding.Default.GetBytes(hz[i].ToString());

            //非汉字
            if (array[0] < 176)
            {
                return_py += hz[i];
            }
            //一级汉字
            else if (array[0] >= 176 && array[0] <= 215)
            {

                if (hz[i].ToString().CompareTo("匝") >= 0)
                    return_py += "z";
                else if (hz[i].ToString().CompareTo("压") >= 0)
                    return_py += "y";
                else if (hz[i].ToString().CompareTo("昔") >= 0)
                    return_py += "x";
                else if (hz[i].ToString().CompareTo("哇") >= 0)
                    return_py += "w";
                else if (hz[i].ToString().CompareTo("塌") >= 0)
                    return_py += "t";
                else if (hz[i].ToString().CompareTo("撒") >= 0)
                    return_py += "s";
                else if (hz[i].ToString().CompareTo("然") >= 0)
                    return_py += "r";
                else if (hz[i].ToString().CompareTo("期") >= 0)
                    return_py += "q";
                else if (hz[i].ToString().CompareTo("啪") >= 0)
                    return_py += "p";
                else if (hz[i].ToString().CompareTo("哦") >= 0)
                    return_py += "o";
                else if (hz[i].ToString().CompareTo("拿") >= 0)
                    return_py += "n";
                else if (hz[i].ToString().CompareTo("妈") >= 0)
                    return_py += "m";
                else if (hz[i].ToString().CompareTo("垃") >= 0)
                    return_py += "l";
                else if (hz[i].ToString().CompareTo("喀") >= 0)
                    return_py += "k";
                else if (hz[i].ToString().CompareTo("击") >= 0)
                    return_py += "j";
                else if (hz[i].ToString().CompareTo("哈") >= 0)
                    return_py += "h";
                else if (hz[i].ToString().CompareTo("噶") >= 0)
                    return_py += "g";
                else if (hz[i].ToString().CompareTo("发") >= 0)
                    return_py += "f";
                else if (hz[i].ToString().CompareTo("蛾") >= 0)
                    return_py += "e";
                else if (hz[i].ToString().CompareTo("搭") >= 0)
                    return_py += "d";
                else if (hz[i].ToString().CompareTo("擦") >= 0)
                    return_py += "c";
                else if (hz[i].ToString().CompareTo("芭") >= 0)
                    return_py += "b";
                else if (hz[i].ToString().CompareTo("啊") >= 0)
                    return_py += "a";
            }
            //二级汉字
            else if (array[0] >= 215)
            {
                return_py += ls_second_eng.Substring(ls_second_ch.IndexOf(hz[i].ToString(), 0), 1);
            }
        }
        return return_py.ToUpper();
    }

    /// <summary>
    /// 取汉字拼音的首字母
    /// GetCodstring(string UnName)
    /// </summary>
    /// <param name="UnName">汉字</param>
    /// <returns>首字母</returns>
    public static string GetCodstring(string UnName)
    {
        int i = 0;
        ushort key = 0;
        string strResult = string.Empty;

        Encoding unicode = Encoding.Unicode;
        Encoding gbk = Encoding.GetEncoding(936);
        byte[] unicodeBytes = unicode.GetBytes(UnName);
        byte[] gbkBytes = Encoding.Convert(unicode, gbk, unicodeBytes);
        while (i < gbkBytes.Length)
        {
            if (gbkBytes[i] <= 127)
            {
                strResult = strResult + (char)gbkBytes[i];
                i++;
            }
            #region 生成汉字拼音简码,取拼音首字母
            else
            {
                key = (ushort)(gbkBytes[i] * 256 + gbkBytes[i + 1]);
                if (key >= '\uB0A1' && key <= '\uB0C4')
                {
                    strResult = strResult + "A";
                }
                else if (key >= '\uB0C5' && key <= '\uB2C0')
                {
                    strResult = strResult + "B";
                }
                else if (key >= '\uB2C1' && key <= '\uB4ED')
                {
                    strResult = strResult + "C";
                }
                else if (key >= '\uB4EE' && key <= '\uB6E9')
                {
                    strResult = strResult + "D";
                }
                else if (key >= '\uB6EA' && key <= '\uB7A1')
                {
                    strResult = strResult + "E";
                }
                else if (key >= '\uB7A2' && key <= '\uB8C0')
                {
                    strResult = strResult + "F";
                }
                else if (key >= '\uB8C1' && key <= '\uB9FD')
                {
                    strResult = strResult + "G";
                }
                else if (key >= '\uB9FE' && key <= '\uBBF6')
                {
                    strResult = strResult + "H";
                }
                else if (key >= '\uBBF7' && key <= '\uBFA5')
                {
                    strResult = strResult + "J";
                }
                else if (key >= '\uBFA6' && key <= '\uC0AB')
                {
                    strResult = strResult + "K";
                }
                else if (key >= '\uC0AC' && key <= '\uC2E7')
                {
                    strResult = strResult + "L";
                }
                else if (key >= '\uC2E8' && key <= '\uC4C2')
                {
                    strResult = strResult + "M";
                }
                else if (key >= '\uC4C3' && key <= '\uC5B5')
                {
                    strResult = strResult + "N";
                }
                else if (key >= '\uC5B6' && key <= '\uC5BD')
                {
                    strResult = strResult + "O";
                }
                else if (key >= '\uC5BE' && key <= '\uC6D9')
                {
                    strResult = strResult + "P";
                }
                else if (key >= '\uC6DA' && key <= '\uC8BA')
                {
                    strResult = strResult + "Q";
                }
                else if (key >= '\uC8BB' && key <= '\uC8F5')
                {
                    strResult = strResult + "R";
                }
                else if (key >= '\uC8F6' && key <= '\uCBF9')
                {
                    strResult = strResult + "S";
                }
                else if (key >= '\uCBFA' && key <= '\uCDD9')
                {
                    strResult = strResult + "T";
                }
                else if (key >= '\uCDDA' && key <= '\uCEF3')
                {
                    strResult = strResult + "W";
                }
                else if (key >= '\uCEF4' && key <= '\uD188')
                {
                    strResult = strResult + "X";
                }
                else if (key >= '\uD1B9' && key <= '\uD4D0')
                {
                    strResult = strResult + "Y";
                }
                else if (key >= '\uD4D1' && key <= '\uD7F9')
                {
                    strResult = strResult + "Z";
                }
                else
                {
                    strResult = strResult + "?";
                }
                i = i + 2;
            }
            #endregion
        }
        return strResult;
    }
}

