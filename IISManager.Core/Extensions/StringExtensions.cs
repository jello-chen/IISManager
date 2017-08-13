using IISManager.Core.Helper;
using System;
using System.Diagnostics;
using System.Globalization;

namespace IISManager.Core.Extensions
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this string str, bool ignoreCase = false)
        {
            return (T)Enum.Parse(typeof(T), str, ignoreCase);
        }

        public static int ToInt(this string str, int defaultVal = 0)
        {
            return ConvertHelper.ToInt(str, defaultVal);
        }

        public static bool ToBool(this string str, bool defaultVal = false)
        {
            return ConvertHelper.ToBool(str, defaultVal);
        }

        public static long ToLong(this string str, long defaultVal = 0L)
        {
            return ConvertHelper.ToLong(str, defaultVal);
        }

        [DebuggerStepThrough]
        public static string FormatWith(this string instance, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, instance, args);
        }
    }
}
