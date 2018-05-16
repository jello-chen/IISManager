using System;
using System.Configuration;

namespace IISManager.Core
{
    public static class Globals
    {
        public static readonly string WEB_ROOT = ConfigurationManager.AppSettings["webroot"];
        public static readonly string Url = ConfigurationManager.AppSettings["url"];
        public static string RootPath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf("IISManager"));
    }
}
