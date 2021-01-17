using System;

namespace WebApiCore.Lib.Utils
{
    /// <summary>
    /// 日期帮助类
    /// </summary>
    public class DateTimeHelper
    {
        #region 时间戳转换
        /// <summary>
        /// 获取字符串日期
        /// </summary>
        /// <param name="ms">时间戳</param>
        /// <returns>时间日期<see cref="string"/></returns>
        public static string GetDateTime(long ms)
        {
            int ss = 1000; //second
            int min = ss * 60;//minute
            int hh = min * 60;//hour
            int dd = hh * 24;//day

            long day = ms / dd;
            long hour = (ms - day * dd) / hh;
            long minute = (ms - day * dd - hour * hh) / min;
            long second = (ms - day * dd - hour * hh - minute * min) / ss;

            string sDay = day < 10 ? "0" + day : "" + day; //天
            string sHour = hour < 10 ? "0" + hour : "" + hour;//小时
            string sMinute = minute < 10 ? "0" + minute : "" + minute;//分钟
            string sSecond = second < 10 ? "0" + second : "" + second;//秒

            return string.Format("{0} 天 {1} 小时 {2} 分 {3} 秒", sDay, sHour, sMinute, sSecond);
        }
        #endregion

        #region 获取unix时间戳
   
        /// <summary>
        /// 获取Unix时间戳
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns>时间戳<see cref="long"/></returns>
        public static long GetUnixTimeStamp(DateTime dt) => ((DateTimeOffset)dt).ToUnixTimeMilliseconds();

        #endregion

        #region 获取日期天的开始时间

        /// <summary>
        /// 获取时间当天的开始时间
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns>开始时间<see cref="DateTime"/></returns>
        public static DateTime GetStartDate(DateTime dt)
        {
            DateTime min = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            return min;
        }
        #endregion

        #region 获取日期天的结束时间

        /// <summary>
        /// 获取时间当天的结束时间
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns>结束时间<see cref="DateTime"/></returns>
        public static DateTime GetEndDate(DateTime dt)
        {
            DateTime max = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            return max;
        }
        #endregion

        #region 获取日期字符串转时间
        /// <summary>
        /// 获取日期字符串转时间
        /// </summary>
        /// <param name="str">日期字符串</param>
        /// <returns>日期<see cref="DateTime"/></returns>
        public static DateTime GetDateTime(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return DateTime.MinValue;
                }
                if (str.Contains("-") || str.Contains("/"))
                {
                    return DateTime.Parse(str);
                }
                else
                {
                    int length = str.Length;
                    return length switch
                    {
                        4 => DateTime.ParseExact(str, "yyyy", System.Globalization.CultureInfo.CurrentCulture),
                        6 => DateTime.ParseExact(str, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture),
                        8 => DateTime.ParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture),
                        10 => DateTime.ParseExact(str, "yyyyMMddHH", System.Globalization.CultureInfo.CurrentCulture),
                        12 => DateTime.ParseExact(str, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture),
                        14 => DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture),
                        _ => DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture),
                    };
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        #endregion


    }
}
