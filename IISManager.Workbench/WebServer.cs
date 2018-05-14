using IISManager.Core;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            string url = Globals.Url;
            webApp = WebApp.Start(url);
            Console.WriteLine($"Web host start at {url}.");
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
