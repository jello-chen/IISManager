using IISManager.Core.Domain;
using IISManager.Core.Extensions;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IISManager.Core
{
    internal class IIS7Controller : BaseController
    {
        public override IISResultInfo<List<IISAppPoolInfo>> GetAllPoolInfos()
        {
            var data = new IISResultInfo<List<IISAppPoolInfo>>();
            var list = new List<IISAppPoolInfo>();
            try
            {
                using (var iisManager = new ServerManager())
                {
                    foreach (var item in iisManager.ApplicationPools)
                    {
                        list.Add(new IISAppPoolInfo()
                        {
                            Name = item.Name,
                            Enable32BitAppOnWin64 = item.Enable32BitAppOnWin64,
                            ManagedRuntimeVersion = item.ManagedRuntimeVersion,
                            State = (AppPoolState)((int)item.State),
                            AppPoolAutoStart = item.AutoStart,
                            //WorkerProcessesCount = item.WorkerProcesses.LongCount(),
                            //MaxProcessesCount = item.ProcessModel.MaxProcesses,
                            IdentityType = item.ProcessModel.IdentityType.ToString().ToEnum<AppPoolIdentityType>(),
                            AnonymousUserName = item.ProcessModel.UserName
                        });
                    }
                }
                data.Data = list;
            }
            catch (Exception e)
            {
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
                using (ServerManager iisManager = new ServerManager())
                {
                    foreach (var item in iisManager.Sites)
                    {
                        list.Add(new IISSiteInfo()
                        {
                            Name = item.Name,
                            ID = item.Id,
                            ServerAutoStart = item.ServerAutoStart,
                            ServerBindings = string.Join(",", item.Bindings.Cast<Binding>().Where(p => p.Protocol.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)).Select(p => ":{0}:{1}".FormatWith(p.EndPoint.Port, p.Host))),
                            DefaultDoc = string.Join(",", item.GetWebConfiguration().GetSection("system.webServer/defaultDocument").GetCollection("files").Select(p => p["value"])),
                            State = item.State.ToServerState(),
                            Applications = new List<SiteApplicationInfo>(
                                item.Applications.Select(p =>
                                    new SiteApplicationInfo()
                                    {
                                        Path = p.Path.TrimStart('/'),
                                        PhysicalPath = p.VirtualDirectories["/"].PhysicalPath,
                                        PoolName = p.ApplicationPoolName
                                    })),
                        });
                    }
                }
                data.Data = list;
            }
            catch (Exception e)
            {
                data.SetError(e);
            }
            return data;
        }



        public override IISResultInfo StartSite(IISStartInfo info)
        {
            var data = new IISResultInfo();
            try
            {
                using (var iisManager = new ServerManager())
                {
                    var site = iisManager.Sites.FirstOrDefault(p => p.Id == info.ParentID);
                    if (site == null)
                    {
                        return data.SetError("site not found", 404);
                    }
                    switch (site.State)
                    {
                        case ObjectState.Starting:
                            break;
                        case ObjectState.Started:
                            break;
                        case ObjectState.Stopping:
                        case ObjectState.Stopped:
                        case ObjectState.Unknown:
                            site.Start();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (Exception e)
            {
                data.SetError(e);
            }
            return data;
        }

        public override IISResultInfo StopSite(IISStartInfo info)
        {
            var data = new IISResultInfo();
            try
            {
                using (var iisManager = new ServerManager())
                {
                    var site = iisManager.Sites.FirstOrDefault(p => p.Id == info.ParentID);
                    if (site == null)
                    {
                        return data.SetError("site not found", 404);
                    }
                    switch (site.State)
                    {
                        case ObjectState.Starting:
                        case ObjectState.Started:
                            site.Stop();
                            break;
                        case ObjectState.Stopping:
                        case ObjectState.Stopped:
                        case ObjectState.Unknown:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (Exception e)
            {
                data.SetError(e);
            }
            return data;
        }

        public override IISResultInfo StartAppPool(string appPoolName)
        {
            var data = new IISResultInfo();
            try
            {
                using (var iisManager = new ServerManager())
                {
                    var appPool = iisManager.ApplicationPools.FirstOrDefault(p => p.Name == appPoolName);
                    if (appPool == null)
                    {
                        return data.SetError("Application pool is not found", 404);
                    }
                    switch (appPool.State)
                    {
                        case ObjectState.Starting:
                            break;
                        case ObjectState.Started:
                            break;
                        case ObjectState.Stopping:
                        case ObjectState.Stopped:
                        case ObjectState.Unknown:
                            appPool.Start();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (Exception e)
            {
                data.SetError(e);
            }
            return data;
        }

        public override IISResultInfo StopAppPool(string appPoolName)
        {
            var data = new IISResultInfo();
            try
            {
                using (var iisManager = new ServerManager())
                {
                    var appPool = iisManager.ApplicationPools.FirstOrDefault(p => p.Name == appPoolName);
                    if (appPool == null)
                    {
                        return data.SetError("appPool not found", 404);
                    }
                    switch (appPool.State)
                    {
                        case ObjectState.Starting:
                        case ObjectState.Started:
                            appPool.Stop();
                            break;
                        case ObjectState.Stopping:
                        case ObjectState.Stopped:
                        case ObjectState.Unknown:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (Exception e)
            {
                data.SetError(e);
            }
            return data;
        }

        public override IISResultInfo RecycleAppPool(string appPoolName)
        {

            var data = new IISResultInfo();
            try
            {
                using (var iisManager = new ServerManager())
                {
                    var appPool = iisManager.ApplicationPools.FirstOrDefault(p => p.Name == appPoolName);
                    if (appPool == null)
                    {
                        return data.SetError("appPool not found", 404);
                    }
                    switch (appPool.State)
                    {
                        case ObjectState.Starting:
                            break;
                        case ObjectState.Started:
                            appPool.Recycle();
                            break;
                        case ObjectState.Stopping:
                        case ObjectState.Stopped:
                        case ObjectState.Unknown:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (Exception e)
            {
                data.SetError(e);
            }
            return data;
        }
    }
}
