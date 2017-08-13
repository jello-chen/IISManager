using IISManager.Core.Domain;
using IISManager.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;

namespace IISManager.Core
{
    internal class IIS6Controller : BaseController
    {
        const string IIS_WEBSERVER = "IIsWebServer";
        const string IIS_WEBVIRTUALDIR = "IIsWebVirtualDir";
        const string IIS_WEBDIRECTORY = "IIsWebDirectory";
        const string IIS_APPLICATIONPOOL = "IIsApplicationPool";

        public string IISPoolPath
        {
            get { return "IIS://LOCALHOST/W3SVC/AppPools"; }
        }
        public string IISWebPath
        {
            get { return "IIS://LOCALHOST/W3SVC"; }
        }
        public override IISResultInfo<List<IISAppPoolInfo>> GetAllPoolInfos()
        {
            var data = new IISResultInfo<List<IISAppPoolInfo>>();
            var list = new List<IISAppPoolInfo>();
            try
            {
                var rootEntity = GetAppPoolRootEntry();
                foreach (DirectoryEntry item in rootEntity.Children)
                {
                    list.Add(new IISAppPoolInfo()
                    {
                        Name = item.Name,
                        //Site = item.Site.Name,
                        //Path = item.Path,
                        IdentityType = item.Properties["AppPoolIdentityType"].GetFirstValue().ToEnum<AppPoolIdentityType>(),
                        AnonymousUserName = item.Properties["WAMUserName"].GetFirstValue(string.Empty),
                        ManagedRuntimeVersion = item.Properties["ManagedRuntimeVersion"].GetFirstValue(),
                        State = (AppPoolState)(item.Properties["AppPoolState"].GetFirstValue().ToInt() - 1),
                        AppPoolAutoStart = item.Properties["AppPoolAutoStart"].GetFirstValue().ToBool(),
                        Enable32BitAppOnWin64 = item.Properties["Enable32BitAppOnWin64"].GetFirstValue().ToBool(),
                        //WorkerProcessesCount = item.Properties["WorkerProcessesCount"].GetFirstValue().ToLong(),
                        //MaxProcessesCount = item.Properties["AspProcessorThreadMax"].GetFirstValue().ToLong()
                    });
                }
                data.Data = list;
                rootEntity.Close();
            }
            catch (Exception e)
            {
                Trace.TraceError("{0} Exception:{1}", DateTime.Now.ToString(), e.ToString());
                data.SetError(e);
            }
            return data;
        }


        public override IISResultInfo<List<IISSiteInfo>> GetAllSites()
        {
            var data = new IISResultInfo<List<IISSiteInfo>>();
            var list = new List<IISSiteInfo>();
            try
            {
                var rootEntity = GetWebRootEntry();
                foreach (DirectoryEntry item in rootEntity.Children)
                {
                    if (item.SchemaClassName == IIS_WEBSERVER)
                    {
                        var site = new IISSiteInfo();
                        site.ID = item.Name.ToLong();
                        site.Name = item.Properties["ServerComment"].GetFirstValue();
                        site.State = item.Properties["ServerState"].GetFirstValue().ToEnum<ServerState>();
                        site.ServerAutoStart = item.Properties["ServerAutoStart"].GetFirstValue().ToBool();
                        site.ServerBindings = string.Join(",", item.Properties["ServerBindings"].GetAllValue());
                        //site.AnonymousUserName = item.Properties["AnonymousUserName"].GetFirstValue();
                        site.DefaultDoc = item.Properties["DefaultDoc"].GetFirstValue();
                        //site.EnableDirBrowsing = item.Properties["EnableDirBrowsing"].GetFirstValue().ToBool();
                        //site.DefaultDoc = item.Properties["EnableDirBrowsing"].GetFirstValue();
                        var apps = new List<SiteApplicationInfo>();
                        foreach (DirectoryEntry item1 in item.Children)
                        {
                            if (item1.SchemaClassName != IIS_WEBVIRTUALDIR && item1.SchemaClassName != IIS_WEBDIRECTORY)
                            {
                                continue;
                            }
                            apps.Add(new SiteApplicationInfo()
                            {
                                Path = "",
                                //Path = item1.Name,
                                //Path = item1.Path,
                                //Name = null,
                                PoolName = item1.Properties["AppPoolId"].Value.ToStringEx()
                            });

                            foreach (DirectoryEntry item2 in item1.Children)
                            {
                                apps.Add(new SiteApplicationInfo()
                                {
                                    //Path = item2.Path,
                                    Path = item2.Name,
                                    PoolName = item2.Properties["AppPoolId"].Value.ToStringEx()
                                });
                            }
                        }
                        site.Applications = apps;
                        list.Add(site);
                    }
                }
                data.Data = list;
                rootEntity.Close();
            }
            catch (Exception e)
            {
                data.SetError(e);
            }
            return data;
        }

        public override IISResultInfo StartSite(IISStartInfo info)
        {
            var ret = new IISResultInfo();
            try
            {
                var siteEntry = GetByPath(info);
                if (siteEntry == null)
                {
                    return ret.SetError("Web site is not found.", 404);
                }
                siteEntry.Invoke("Start");
                siteEntry.Dispose();
            }
            catch (Exception e)
            {
                ret.SetError(e.Message, 500);
            }
            return ret;
        }

        public override IISResultInfo StopSite(IISStartInfo info)
        {
            var ret = new IISResultInfo();
            try
            {
                var siteEntry = GetByPath(info);
                if (siteEntry == null)
                {
                    return ret.SetError("站点不存在", 404);
                }
                siteEntry.Invoke("Stop", null);
                siteEntry.Dispose();
            }
            catch (Exception e)
            {
                ret.SetError(e.Message, 500);
            }
            return ret;
        }

        private DirectoryEntry GetByPath(IISStartInfo info)
        {
            if (info.ParentID < 1)
            {
                return null;
            }
            var webPath = IISWebPath + "/" + info.ParentID.ToString();
            var root = new DirectoryEntry(webPath);
            return root;
        }

        private DirectoryEntry GetWebRootEntry()
        {
            return new DirectoryEntry(IISWebPath);
        }
        private DirectoryEntry GetAppPoolRootEntry()
        {
            return new DirectoryEntry(IISPoolPath);
        }

        public override IISResultInfo StartAppPool(string appPoolName)
        {
            var ret = new IISResultInfo();
            try
            {
                HandleAppPool(appPoolName, "Start");
            }
            catch (Exception e)
            {
                ret.SetError(e.Message, 500);
            }
            return ret;
        }
        protected void HandleAppPool(string appPoolName, string method)
        {
            var root = GetAppPoolRootEntry();
            var find = root.Children.Find(appPoolName, IIS_APPLICATIONPOOL);
            find.Invoke(method);
            find.CommitChanges();
            find.Close();
        }

        public override IISResultInfo StopAppPool(string appPoolName)
        {
            var ret = new IISResultInfo();
            try
            {
                HandleAppPool(appPoolName, "Stop");
            }
            catch (Exception e)
            {
                ret.SetError(e.Message, 500);
            }
            return ret;
        }

        public override IISResultInfo RecycleAppPool(string appPoolName)
        {
            var ret = new IISResultInfo();
            if (string.IsNullOrWhiteSpace(appPoolName))
            {
                return ret.SetError("appPoolName is null", 404);
            }
            try
            {
                HandleAppPool(appPoolName, "Recycle");
            }
            catch (Exception e)
            {
                ret.SetError(e.Message, 500);
            }
            return ret;
        }
    }
}
