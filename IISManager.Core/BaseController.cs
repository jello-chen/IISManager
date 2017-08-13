using System.Collections.Generic;
using IISManager.Core.Domain;

namespace IISManager.Core
{
    public abstract class BaseController : IController
    {
        public abstract IISResultInfo<List<IISAppPoolInfo>> GetAllPoolInfos();
        public abstract IISResultInfo<List<IISSiteInfo>> GetAllSites();
        public abstract IISResultInfo RecycleAppPool(string appPoolName);
        public virtual IISResultInfo ReStartSite(IISStartInfo info)
        {
            var result = StopSite(info);
            if (result.Success)
            {
                result = StartSite(info);
            }
            return result;
        }
        public abstract IISResultInfo StartAppPool(string appPoolName);
        public abstract IISResultInfo StartSite(IISStartInfo info);
        public abstract IISResultInfo StopAppPool(string appPoolName);
        public abstract IISResultInfo StopSite(IISStartInfo info);
    }
}
