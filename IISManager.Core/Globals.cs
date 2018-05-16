using System.Configuration;

namespace IISManager.Core
{
    public static class Globals
    {
        public static readonly string WEB_ROOT = ConfigurationManager.AppSettings["webroot"];
        public static readonly string Url = ConfigurationManager.AppSettings["url"];
    }
}
