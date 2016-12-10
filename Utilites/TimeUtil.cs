using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcFunVideo
{
    public static class TimeUtil
    {
        public static long GetTimeStampSeconds(this DateTime time)
        {
            var dt = new System.DateTime(1970, 1, 1).ToLocalTime();
            return (long) (time - dt).TotalSeconds;
        }

        public static long GetTimeStampSeconds()
        {
            return GetTimeStampSeconds(DateTime.Now);
        }
        /// <summary>
        /// 例如当前时间2016年，时间戳是10位数就是秒，13位数就是毫秒
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static DateTime TimeStampToDate(this long seconds)
        {
            DateTime time = new DateTime(1970, 1, 1).ToLocalTime();
            var stamp = TimeSpan.FromSeconds(seconds);
            time = time.Add(stamp);
            return time.ToLocalTime();
        }
        /// <summary>
        /// 例如当前时间2016年，时间戳是13位数就是毫秒
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static DateTime MillisecondsToDateTime(this long milliseconds)
        {
            DateTime time = new DateTime(1970, 1, 1).ToLocalTime();
            var stamp = TimeSpan.FromMilliseconds(milliseconds);
            time = time.Add(stamp);
            return time.ToLocalTime();
        }
    }
}
