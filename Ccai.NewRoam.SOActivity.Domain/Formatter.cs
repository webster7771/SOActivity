using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Ccai.NewRoam.SOActivity.Domain
{
    /// <summary>
    /// This class is helper class to format the DateTime to the format used in this application.
    /// </summary>
    public static class DateTimeFormatter
    {
        /// <summary>
        /// Format the DateTime to string with time information, eg: 2014-05-27 13:13:00.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToTimeString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Format the DateTime to string that show the date information only, eg: 2014-05-27.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Format the DateTime to string that show the time information only, eg: 10:00.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToShortTimeString(this DateTime date)
        {
            return date.ToString("HH:mm");
        }
    }
}