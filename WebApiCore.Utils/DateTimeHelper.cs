using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore.Utils
{
    public class DateTimeHelper
    {
        #region 时间戳转换
        /// <summary>
        /// 时间戳转换成字符串日期
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static string UnixTimeToString(long ms)
        {
            //int ss = 1000;
            //int mi = ss * 60;
            //int hh = mi * 60;
            //int dd = hh * 24;

            //long day = ms / dd;
            //long hour = (ms - day * dd) / hh;
            //long minute = (ms - day * dd - hour * hh) / mi;
            //long second = (ms - day * dd - hour * hh - minute * mi) / ss;
            //long milliSecond = ms - day * dd - hour * hh - minute * mi - second * ss;

            //string sDay = day < 10 ? "0" + day : "" + day; //天
            //string sHour = hour < 10 ? "0" + hour : "" + hour;//小时
            //string sMinute = minute < 10 ? "0" + minute : "" + minute;//分钟
            //string sSecond = second < 10 ? "0" + second : "" + second;//秒
            //string sMilliSecond = milliSecond < 10 ? "0" + milliSecond : "" + milliSecond;//毫秒
            //sMilliSecond = milliSecond < 100 ? "0" + sMilliSecond : "" + sMilliSecond;

            //return string.Format("{0} 天 {1} 小时 {2} 分 {3} 秒", sDay, sHour, sMinute, sSecond);

            DateTime nowTime = new DateTime(1970, 1, 1).AddMilliseconds(ms);

            return nowTime.ToString();
        }
        #endregion

        #region 获取unix时间戳
        /// <summary>
        /// 获取unix时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>时间戳<see cref="long"></returns>
        public static long GetUnixTimeStamp(DateTime dt)
        {
            return ((DateTimeOffset)dt).ToUnixTimeMilliseconds();
        }
        #endregion

        #region 获取日期天的开始时间
        public static DateTime GetStartDate(DateTime dt)
        {
            DateTime min = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            return min;
        }
        #endregion

        #region 获取日期天的结束时间
        public static DateTime GetEndDate(DateTime dt)
        {
            DateTime max = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            return max;
        }
        #endregion

    }
}
