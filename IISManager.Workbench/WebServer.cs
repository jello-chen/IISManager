using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISManager.Workbench
{
    public class WebServer : IDisposable
    {
        private IDisposable webApp;

        public void Start()
        {
            webApp = WebApp.Start("http://localhost:6060");
            Console.WriteLine("Web host start at http://localhost:6060.");
        }

        public void Stop()
        {
            webApp?.Dispose();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
