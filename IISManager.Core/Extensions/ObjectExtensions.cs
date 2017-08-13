using IISManager.Core.Domain;
using IISManager.Core.Helper;
using Microsoft.Web.Administration;
using System.DirectoryServices;

namespace IISManager.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string GetFirstValue(this PropertyValueCollection vals, string isNullValue = null)
        {
            if (vals == null || vals.Count == 0)
            {
                return isNullValue;
            }
            return vals[0].ToString();
        }

        public static string[] GetAllValue(this PropertyValueCollection vals)
        {
            if (vals == null)
            {
                return null;
            }
            var arr = new string[vals.Count];
            for (int i = 0, length = vals.Count; i < length; i++)
            {
                arr[i] = vals[i].ToString();
            }
            return arr;
        }

        public static ServerState ToServerState(this ObjectState state)
        {
            var t = (int)state;
            return (ServerState)(t + 1);
        }

        public static string ToStringEx(this object obj, string defaultVal = null)
        {
            return ConvertHelper.ToString(obj, defaultVal);
        }
    }
}
