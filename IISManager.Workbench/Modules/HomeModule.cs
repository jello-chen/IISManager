using System;
using Nancy;
using IISManager.Workbench.Common;
using IISManager.Core;
using System.Linq;

namespace IISManager.Workbench.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => View["Index"];
            Get["/GetAllIISInfos"] = _ => GetAllIISInfos();
            Post["/ExecSiteCommand"] = _ => ExecSiteCommand();
        }

        private dynamic GetAllIISInfos()
        {
            var nodes = ModelBuilder.Build();
            return Response.AsJson<dynamic>(nodes);
        }

        private dynamic ExecSiteCommand()
        {
            var cmd = Context.Request.Form["cmd"].Value;
            var id = long.Parse(Context.Request.Form["id"].Value);
            var controller = IISController.GetController();
            if (cmd != "start" && cmd != "stop")
                return Response.AsJson<dynamic>(new { success = false, message = "Invalid command." });
            if (cmd == "start")
            {
                controller.StartSite(new Core.Domain.IISStartInfo { ParentID = id });
                return Response.AsJson<dynamic>(new { success = true, status = 2 });
            }
            else
            {
                controller.StopSite(new Core.Domain.IISStartInfo { ParentID = id });
                return Response.AsJson<dynamic>(new { success = true, status = 4 });
            }
        }
    }
}
