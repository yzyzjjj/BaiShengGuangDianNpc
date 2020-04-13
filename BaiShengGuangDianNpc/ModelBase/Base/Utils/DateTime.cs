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
            var startTime = TimeZoneInfo.ConvertTime(Default, TimeZoneInfo.Local);
            var intResult = (int)((dte - startTime).TotalSeconds);
            return intResult;
        }

        /// <summary>
        /// 获取1970-01-01至dateTime的毫秒数
        /// </summary>
        public static string ToTimestamp(this DateTime dateTime)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        /// <summary>
        /// DateTime扩展方法，根据Unix时间戳（Int32）设置当前DateTime实例。
        /// </summary>
        /// <param name="date">DateTime实例</param>
        /// <param name="unixTime">Unix时间戳</param>
        public static void FromUnixTime(this DateTime date, int unixTime)
        {
            var startTime = TimeZoneInfo.ConvertTime(Default, TimeZoneInfo.Local);
            date = startTime.AddSeconds(unixTime);
        }

        /// <summary>
        /// DateTime扩展方法，根据Unix时间戳（Int32）设置当前DateTime实例。
        /// </summary>
        /// <param name="unixTime">Unix时间戳</param>
        public static DateTime FromUnixTime(int unixTime)
        {
            DateTime startTime = TimeZoneInfo.ConvertTime(Default, TimeZoneInfo.Local);
            return startTime.AddSeconds(unixTime);
        }

        /// <summary>
        /// 判断两个DateTime是否属于同一秒。
        /// </summary>
        /// <param name="date">日期一</param>
        /// <param name="targetDay">日期二</param>
        /// <returns>是否属于同一周</returns>
        public static bool InSameSecond(this DateTime date, DateTime targetDay)
        {
            return date.NoMillisecond() == targetDay.NoMillisecond();
        }

        /// <summary>
        /// 判断两个DateTime是否属于同一分。
        /// </summary>
        /// <param name="date">日期一</param>
        /// <param name="targetDay">日期二</param>
        /// <returns>是否属于同一周</returns>
        public static bool InSameMinute(this DateTime date, DateTime targetDay)
        {
            return date.NoSecond() == targetDay.NoSecond();
        }

        /// <summary>
        /// 判断两个DateTime是否属于同一小时。
        /// </summary>
        /// <param name="date">日期一</param>
        /// <param name="targetDay">日期二</param>
        /// <returns>是否属于同一周</returns>
        public static bool InSameHour(this DateTime date, DateTime targetDay)
        {
            return date.NoMinute() == targetDay.NoMinute();
        }

        /// <summary>
        /// DateTime扩展方法，根据日起始时间，判断两个DateTime是否属于同一天。
        /// </summary>
        /// <param name="date"></param>
        /// <param name="targetDay"></param>
        /// <param name="beginTimeOfDay"></param>
        /// <returns>是否属于同一日</returns>
        public static bool InSameDay(this DateTime date, DateTime targetDay, DateTime beginTimeOfDay)
        {
            return (int)((date - beginTimeOfDay).TotalDays) == (int)((targetDay - beginTimeOfDay).TotalDays);

        }
        public static bool InSameDay(this DateTime dte, DateTime targetDay)
        {
            return dte.Date == targetDay.Date;
        }

        /// <summary>
        /// DateTime扩展方法，根据周起始时间，判断两个DateTime是否属于同一周。
        /// </summary>
        /// <param name="date">日期一</param>
        /// <param name="targetDay">日期二</param>
        /// <param name="firstDayOfWeek">周起始时间</param>
        /// <returns>是否属于同一周</returns>
        public static bool InSameWeek(this DateTime date, DateTime targetDay, DateTime firstDayOfWeek)
        {
            return (int)((date - firstDayOfWeek).TotalDays / 7) == (int)((targetDay - firstDayOfWeek).TotalDays / 7);
        }

        public static bool InSameWeek(this DateTime date, DateTime targetDay)
        {
            return GetWeek(0, date).Item1 == GetWeek(0, targetDay).Item1;
        }

        /// <summary>
        /// 判断两个DateTime是否属于同一年同一月。
        /// </summary>
        /// <param name="date">日期一</param>
        /// <param name="targetDay">日期二</param>
        /// <returns>是否属于同一周</returns>
        public static bool InSameMonth(this DateTime date, DateTime targetDay)
        {
            return date.Year == targetDay.Year && date.Month == targetDay.Month;
        }
        /// <summary>
        /// 判断两个DateTime是否属于同一年。
        /// </summary>
        /// <param name="date">日期一</param>
        /// <param name="targetDay">日期二</param>
        /// <returns>是否属于同一周</returns>
        public static bool InSameYear(this DateTime date, DateTime targetDay)
        {
            return date.Year == targetDay.Year;
        }

        public static DateTime Default { get; } = new DateTime(1970, 1, 1);

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
        /// 获取 日  00:00:00
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime DayBeginTime(this DateTime date)
        {
            return date.Date;
        }

        /// <summary>
        /// 获取 日 23:59:59
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime DayEndTime(this DateTime date)
        {
            return date.AddDays(1).Date.AddSeconds(-1);
        }
        /// <summary>
        /// 获取 周 00:00:00
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime WeekBeginTime(this DateTime date)
        {
            return GetWeek(0, date).Item1;
        }

        /// <summary>
        /// 获取 周 23:59:59
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime WeekEndTime(this DateTime date)
        {
            return GetWeek(0, date).Item2;
        }

        /// <summary>
        /// 前几个星期  0 表示本星期  -1 表示上周  1表示 下周
        /// 返回: 周一的00:00:00 周日的23:59:59
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static Tuple<DateTime, DateTime> GetWeek(int interval, DateTime time = default(DateTime))
        {
            time = time == default(DateTime) ? DateTime.Now : time;
            var day = (int)time.DayOfWeek;
            var weekFirstDay = time.AddDays(1 - (day == 0 ? 7 : day) + interval * 7).DayBeginTime();
            var weekLastDay = weekFirstDay.AddDays(6).DayEndTime();
            return new Tuple<DateTime, DateTime>(weekFirstDay, weekLastDay);
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
        /// 前几个月  0 表示本月  -1 表示上月  1表示 下月
        /// 返回: 该月第一天的00:00:00 该月最后一天的23:59:59
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static Tuple<DateTime, DateTime> GetMonth(int interval, DateTime time = default(DateTime))
        {
            time = time == default(DateTime) ? DateTime.Now : time;
            var monthFirstDay = time.StartOfMonth().AddMonths(interval);
            var montLastDay = monthFirstDay.StartOfNextMonth().AddDays(-1);
            return new Tuple<DateTime, DateTime>(monthFirstDay, montLastDay);
        }

        /// <summary>
        /// 去除 小时
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime NoHour(this DateTime date)
        {
            return date.Date;
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
            return date.ToString("yyMMddHHmmss");
        }

        /// <summary>
        /// 秒转化为天小时分秒
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public static string ToTimeStr(int second)
        {
            var str = "";
            var day = second / (24 * 3600);
            if (day > 0)
            {
                str += day + "天";
            }
            second -= day * (24 * 3600);
            var hour = second / 3600;
            if (hour > 0)
            {
                str += hour + "小时";
            }
            second -= hour * 3600;
            var min = second / 60;
            if (min > 0)
            {
                str += min + "分";
            }
            second -= min * 60;
            if (second > 0)
            {
                str += second + "秒";
            }
            return str;
        }
    }
}
