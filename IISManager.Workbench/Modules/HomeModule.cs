using System;
using Nancy;
using IISManager.Workbench.Common;

namespace IISManager.Workbench.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => View["Index"];
            Get["/GetAllIISInfos"] = _ => GetAllIISInfos();
        }

        private dynamic GetAllIISInfos()
        {
            var nodes = ModelBuilder.Build();
            return Response.AsJson<dynamic>(nodes);
        }
    }
}
