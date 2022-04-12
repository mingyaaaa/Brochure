using System;

namespace Brochure.Extensions
{
    /// <summary>
    /// The date time extends.
    /// </summary>
    public static class DateTimeExtends
    {
        /// <summary>
        /// Tos the unix timestamp by milliseconds.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>A long.</returns>
        public static long ToUnixTimestampByMilliseconds(this DateTime date)
        {
            DateTimeOffset dto = new DateTimeOffset(date);
            return dto.ToUnixTimeMilliseconds();
        }

        /// <summary>
        ///  时间戳转本地时间-时间戳精确到毫秒
        /// </summary>
        public static DateTime ToLocalTimeDateByMilliseconds(this long unix)
        {
            var dto = DateTimeOffset.FromUnixTimeMilliseconds(unix);
            return dto.ToLocalTime().DateTime;
        }

        /// <summary>
        ///  时间转时间戳Unix-时间戳精确到秒
        /// </summary>
        public static long ToUnixTimestampBySeconds(this DateTime dt)
        {
            DateTimeOffset dto = new DateTimeOffset(dt);
            return dto.ToUnixTimeSeconds();
        }

        /// <summary>
        ///  时间戳转本地时间-时间戳精确到秒
        /// </summary>
        public static DateTime ToLocalTimeDateBySeconds(this long unix)
        {
            var dto = DateTimeOffset.FromUnixTimeSeconds(unix);
            return dto.ToLocalTime().DateTime;
        }
    }
}