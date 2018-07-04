using System;
using Nancy;
using IISManager.Workbench.Common;
using IISManager.Core;
using System.Linq;
using IISManager.Core.Utils;
using System.Xml.Serialization;
using System.Xml;
using IISManager.Core.Configuration;
using System.IO;
using Microsoft.AspNet.SignalR;
using IISManager.Workbench.Hubs;

namespace IISManager.Workbench.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => View["Index", new { Url = Globals.Url}];
            Get["/GetAllIISInfos"] = _ => GetAllIISInfos();
            Post["/ExecSiteCommand"] = _ => ExecSiteCommand();
            Post["/Publish"] = _ => Publish();
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

        public dynamic Publish()
        {
            var files = Request.Files.ToList();
            if (files.Count != 1)
                return Response.AsJson(new { success = false, message = "Not upload any file." });

            using (var stream = files[0].Value)
            {
                if (!ZipUtil.IsZip(stream))
                    return Response.AsJson(new { success = false, message = "This is not zip file." });

                if (Directory.Exists(Globals.UploadPath))
                    Directory.Delete(Globals.UploadPath, true);

                if (ZipUtil.UnZip(stream, Globals.UploadPath))
                {
                    ExecuteOperations();
                    return Response.AsJson(new { success = true, message = string.Empty });
                } 
                else
                    return Response.AsJson(new { success = false, message = "Upload failed." });
            }
        }

        private bool ExecuteOperations()
        {
            XmlSerializer xs = new XmlSerializer(typeof(Publish));
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Publish", "Publish.xml");

            using (XmlReader xmlReader = new XmlTextReader(configPath))
            {
                Publish publish = (Publish)xs.Deserialize(xmlReader);
                int count = publish.Operations.Count;
                for (int i = 1; i <= count; i++)
                {
                    Operation op = publish.Operations[i - 1];
                    IOperation operation = GetOperation(op.Type, publish);
                    string result = operation.Execute(op);
                    bool success = string.IsNullOrWhiteSpace(result);
                    GlobalHost.ConnectionManager.GetHubContext<PublishHub>()?.Clients.All.Send(GetOperationMessage(op, publish, i, count, success), success);
                    if (!success)
                    {
                        GlobalHost.ConnectionManager.GetHubContext<PublishHub>()?.Clients.All.Send(result, success);
                        break;
                    }
                }
            }
            return true;
        }

        private IOperation GetOperation(OperationType operationType, Publish publish)
        {
            string op = Enum.GetName(typeof(OperationType), operationType);
            Type type = Type.GetType($"IISManager.Core.{op}Operation,IISManager.Core");
            return (IOperation)Activator.CreateInstance(type, publish);
        }

        private string GetOperationMessage(Operation op, Publish publish, int i, int count, bool success)
        {
            string message = string.Empty;
            string format = "{0} "+(success ? "finished" : "failed") +", process: " + i.ToString() + "/" + count.ToString();
            switch (op.Type)
            {
                case OperationType.Revert:
                    message = string.Format(format, "Reverting " + (string.IsNullOrWhiteSpace(op.Version) ? publish.Version.Previous : op.Version));
                    break;
                case OperationType.Backup:
                    message = string.Format(format, "Backuping " + publish.Version.Previous);
                    break;
                case OperationType.Replace:
                    message = string.Format(format, "Replacing " + op.Target + " with " + op.Src);
                    break;
                case OperationType.Delete:
                    message = string.Format(format, "Deleting " + op.Path);
                    break;
                case OperationType.Execute:
                    message = string.Format(format, "Executing script " + string.Join(",", op.Scripts.Select(s => s.Path)));
                    break;
                case OperationType.Skip:
                    break;
                case OperationType.Add:
                    break;
                default:
                    break;
            }
            return message;
        }
    }
}
