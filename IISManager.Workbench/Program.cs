using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
