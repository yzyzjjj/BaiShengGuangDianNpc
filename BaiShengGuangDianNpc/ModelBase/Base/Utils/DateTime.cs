using System;

namespace ModelBase.Base.Utils
{
    public static class DateTimeExtend
    {
        /// <summary>
        /// DateTime扩展方法，获取当前DateTime实例的Unix时间戳（Int32）。
        /// </summary>
        /// <param name="dte">DateTime实例</param>
        /// <returns>Unix时间戳</returns>
        public static int ToUnixTime(this DateTime dte)
        {
            int intResult = 0;
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            intResult = (int)((dte - startTime).TotalSeconds);
            return intResult;
        }

        /// <summary>
        /// DateTime扩展方法，根据Unix时间戳（Int32）设置当前DateTime实例。
        /// </summary>
        /// <param name="dte">DateTime实例</param>
        /// <param name="unix_time">Unix时间戳</param>
        public static void FromUnixTime(this DateTime dte, int unix_time)
        {
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            dte = startTime.AddSeconds(unix_time);
        }

        /// <summary>
        /// DateTime扩展方法，根据Unix时间戳（Int32）设置当前DateTime实例。
        /// </summary>
        /// <param name="dte">DateTime实例</param>
        /// <param name="unix_time">Unix时间戳</param>
        public static DateTime FromUnixTime(int unix_time)
        {
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            return startTime.AddSeconds(unix_time);
        }

        /// <summary>
        /// DateTime扩展方法，根据周起始时间，判断两个DateTime是否属于同一周。
        /// </summary>
        /// <param name="day1">日期一</param>
        /// <param name="day2">日期二</param>
        /// <param name="firstdayofweek">周起始时间</param>
        /// <returns>是否属于同一周</returns>
        public static bool InSameWeek(this DateTime dte, DateTime targetday, DateTime begin_time_of_week)
        {
            return (int)((dte - begin_time_of_week).TotalDays / 7) == (int)((targetday - begin_time_of_week).TotalDays / 7);

        }

        /// <summary>
        /// DateTime扩展方法，根据日起始时间，判断两个DateTime是否属于同一日。
        /// </summary>
        /// <param name="day1">日期一</param>
        /// <param name="day2">日期二</param>
        /// <param name="firstdayofweek">日起始时间</param>
        /// <returns>是否属于同一日</returns>
        public static bool InSameDay(this DateTime dte, DateTime targetday, DateTime beginTimeOfDay)
        {
            return (int)((dte - beginTimeOfDay).TotalDays) == (int)((targetday - beginTimeOfDay).TotalDays);

        }
        public static bool InSameDay(this DateTime dte, DateTime targetday)
        {
            return dte.Year == targetday.Year && dte.Month == targetday.Month && dte.Day == targetday.Day;

        }
        public static bool InSameMonth(this DateTime dte, DateTime targetday)
        {
            return dte.Year == targetday.Year && dte.Month == targetday.Month;

        }
        public static bool InSameYear(this DateTime dte, DateTime targetday)
        {
            return dte.Year == targetday.Year;

        }
        private static readonly DateTime _default = new DateTime(1970, 1, 1);
        public static DateTime Default
        {
            get { return _default; }
        }

        /// <summary>
        /// 中文日期
        /// </summary>
        /// <returns></returns>
        public static string ToCNString(this DateTime date)
        {
            return date.ToString("yyyy年MM月dd日");
        }
        /// <summary>
        /// 中文日期
        /// </summary>
        /// <returns></returns>
        public static string ToPointString(this DateTime date)
        {
            return date.ToString("yyyy.MM.dd");
        }
        /// <summary>
        /// 获取日期对应的下周第一天的刷新时间
        /// </summary>
        /// <param name="date"></param>
        /// <param name="firstDayOfWeek"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfNextWeek(this DateTime date, DateTime firstDayOfWeek)
        {
            int days = (int)(date - firstDayOfWeek).TotalDays;
            return firstDayOfWeek.AddDays(days + (7 - (days % 7)));
        }

        /// <summary>
        /// 获取日期对应的本周第一天的刷新时间
        /// </summary>
        /// <param name="date"></param>
        /// <param name="firstDayOfWeek"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfThisWeek(this DateTime date, DateTime firstDayOfWeek)
        {
            int days = (int)(date - firstDayOfWeek).TotalDays;
            return firstDayOfWeek.AddDays(days - (days % 7));
        }
        /// <summary>
        /// 获取 00:00:00
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime DayBeginTime(this DateTime date)
        {
            return date.Date;
        }

        /// <summary>
        /// 获取 23:59:59
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime DayEndTime(this DateTime date)
        {
            return date.AddDays(1).Date.AddSeconds(-1);
        }

        /// <summary>
        /// 前几个星期  0 表示本星期  -1 表示上周  1表示 下周
        /// 返回: 周一的00:00:00 周日的23:59:59
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static Tuple<DateTime, DateTime> GetWeek(int interval)
        {
            var now = DateTime.Now;
            var day = (int)now.DayOfWeek;
            var weekFirstDay = now.AddDays((1 - (day == 0 ? 7 : day)) + (interval * 7)).DayBeginTime();
            var weeklastDay = weekFirstDay.AddDays(6).DayEndTime();
            return new Tuple<DateTime, DateTime>(weekFirstDay, weeklastDay);
        }

        /// <summary>
        /// 获得本月第一天
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// 获得本月最后一天
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime EndOfMonth(this DateTime date)
        {
            return date.StartOfMonth().AddMonths(1).AddDays(-1);
        }
        /// <summary>
        /// 获得下月第一天 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime StartOfNextMonth(this DateTime date)
        {
            return date.StartOfMonth().AddMonths(1);
        }
        /// <summary>
        /// 去除 小时
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime NoHour(this DateTime date)
        {
            return date.DayBeginTime();
        }
        /// <summary>
        /// 去除分 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime NoMinute(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
        }
        /// <summary>
        /// 去除秒 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime NoSecond(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
        }
        /// <summary>
        /// 去除毫秒 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime NoMillisecond(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }
        public static string ToStr(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string ToStrx(this DateTime date)
        {
            return date.ToString("yyyy/MM/dd HH:mm:ss");
        }
        public static string ToDateStr(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
        public static string ToDateStrx(this DateTime date)
        {
            return date.ToString("yyyy/MM/dd");
        }
        public static string ToStrFile(this DateTime date)
        {
            return date.ToString("yyMMdd_HHmmss");
        }

    }
}
