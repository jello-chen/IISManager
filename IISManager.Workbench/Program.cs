using System;

namespace IISManager.Workbench
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var webServer = new WebServer())
            {
                webServer.Start();
                Console.ReadKey();
            }
        }
    }
}
