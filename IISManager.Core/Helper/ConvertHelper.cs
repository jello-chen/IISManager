using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace IISManager.Core.Helper
{
    public sealed class ConvertHelper
    {
        #region  ToInt
        public static int ToInt(object obj, int defaultVal = 0)
        {
            if (obj == null || obj is DBNull) return defaultVal;
            return ToInt(obj.ToString(), defaultVal);
        }
        public static int ToInt(string str, int defaultVal = 0)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                int i = 0;
                if (int.TryParse(str.Trim(), out i))
                {
                    return i;
                }
            }
            return defaultVal;
        }

        public static int? ToNullbleInt(object obj)
        {
            if (obj == null || obj is DBNull) return null;
            return ToNullbleInt(obj.ToString());
        }
        public static int? ToNullbleInt(string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                int i = 0;
                if (int.TryParse(str.Trim(), out i))
                {
                    return i;
                }
            }
            return null;
        }
        #endregion

        #region ToTrim
        public static string ToTrim(object obj, string defaultVal = null)
        {
            if (obj == null || obj is DBNull) return defaultVal;
            return obj.ToString().Trim();
        }
        public static string ToTrim(string str, string defaultVal = null)
        {
            return str == null ? defaultVal : str.Trim();
        }
        public static string ToTrimIfEmpty(string str, string defaultVal = null)
        {
            if (str == null)
            {
                return defaultVal;
            }
            str = str.Trim();
            return str.Length == 0 ? defaultVal : str;
        }

        #endregion

        #region ToDateTime
        public static DateTime ToDateTime(object obj, DateTime defaultVal)
        {
            if (obj == null || obj is DBNull) return defaultVal;
            return ToDateTime(obj.ToString(), defaultVal);
        }
        public static DateTime? ToDateTime(string dateTime)
        {
            if (!string.IsNullOrWhiteSpace(dateTime))
            {
                DateTime i;
                if (DateTime.TryParse(dateTime.Trim(), out i))
                {
                    return i;
                }
            }
            return null;
        }
        public static DateTime ToDateTime(string dateTime, DateTime defalutVal)
        {
            if (!string.IsNullOrWhiteSpace(dateTime))
            {
                DateTime i;
                if (DateTime.TryParse(dateTime.Trim(), out i))
                {
                    return i;
                }
            }
            return defalutVal;
        }
        public static DateTime ToDateTime(string dateTime, string defalutVal)
        {
            return ToDateTime(dateTime, DateTime.Parse(defalutVal));
        }
        #endregion

        #region ToString
        public static string ToString(object obj, string defaultVal = null)
        {
            if (obj == null || obj is DBNull) return defaultVal;
            return obj.ToString();
        }
        public static string ToString(string str, string defaultVal = null)
        {
            return str == null ? defaultVal : str.ToString();
        }
        public static string ToString(byte[] bytes, Encoding encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetString(bytes);
        }
        public static string ToString(Stream stream, Encoding encoding = null, bool autoDispose = false)
        {
            if (autoDispose)
            {
                using (var streamReader = new StreamReader(stream, encoding ?? Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }
            else
            {
                var streamReader = new StreamReader(stream, encoding ?? Encoding.UTF8);
                return streamReader.ReadToEnd();
            }
        }

        public static string ToString<T>(IEnumerable<T> source, Func<T, string> action)
        {
            if (source == null)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var item in source)
            {
                sb.Append(action(item));
            }
            return sb.ToString();
        }
        #endregion

        #region ToShort
        public static short ToShort(string str, short defalutVal = 0)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                short i;
                if (short.TryParse(str.Trim(), out i))
                {
                    return i;
                }
            }
            return defalutVal;
        }
        #endregion

        #region  ToFloat
        public static float ToFloat(string str, float defalutVal = 0)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                float i;
                if (float.TryParse(str.Trim(), out i))
                {
                    return i;
                }
            }
            return defalutVal;
        }
        #endregion

        #region ToLong
        public static long ToLong(string str, long defalutVal = 0)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                long i;
                if (long.TryParse(str.Trim(), out i))
                {
                    return i;
                }
            }
            return defalutVal;
        }
        #endregion

        #region ToDouble
        public static double ToDouble(string str, double defalutVal = 0)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                double i;
                if (double.TryParse(str.Trim(), out i))
                {
                    return i;
                }
            }
            return defalutVal;
        }
        #endregion

        #region Base64
        public static string ToBase64String(string input, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            return Convert.ToBase64String((encoding ?? UTF8Encoding.UTF8).GetBytes(input));
        }
        public static string FromBase64String(string base64String, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                return String.Empty;
            }
            return (encoding ?? UTF8Encoding.UTF8).GetString(Convert.FromBase64String(base64String));
        }
        #endregion

        #region ToBool
        public static bool ToBool(string str, bool defalutVal = false)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                bool i;
                if (bool.TryParse(str.Trim(), out i))
                {
                    return i;
                }
            }
            return defalutVal;
        }
        #endregion

        #region ToByte
        public static byte[] ToByte(string str, Encoding encoding = null)
        {
            if (str == null)
            {
                return null;
            }
            return (encoding ?? Encoding.UTF8).GetBytes(str);
        }

        public static byte[] ToByte(Stream stream)
        {
            using (var memory = new MemoryStream())
            {
                stream.CopyTo(memory);
                return memory.ToArray();
            }
        }
        #endregion

        #region Type As
        public static TValue As<TValue>(string str)
        {
            return As<TValue>(str, default(TValue));
        }
        public static TValue As<TValue>(string str, TValue defaultValue)
        {
            var vType = typeof(TValue);
            TypeConverter converter = TypeDescriptor.GetConverter(vType);
            if (converter != null)
            {
                try
                {
                    if (converter.CanConvertFrom(typeof(string)))
                    {
                        return (TValue)converter.ConvertFrom(str);
                    }
                    if (converter.CanConvertTo(vType))
                    {
                        return (TValue)converter.ConvertTo(str, vType);
                    }
                }
                catch { }
            }
            return defaultValue;
        }
        #endregion

        public static T To<T>(object obj) where T : struct
        {
            return (T)ChangeType(obj, typeof(T));
        }

        static public object ChangeType(object value, Type type)
        {
            if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
            if (value == null) return null;
            if (type == value.GetType()) return value;
            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }
            if (!(value is IConvertible)) return value;
            return Convert.ChangeType(value, type);
        }
    }
}
