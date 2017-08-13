using IISManager.Core.Domain;
using System.Collections.Generic;

namespace IISManager.Core
{
    public interface IController
    {
        IISResultInfo<List<IISAppPoolInfo>> GetAllPoolInfos();

        IISResultInfo<List<IISSiteInfo>> GetAllSites();

        IISResultInfo ReStartSite(IISStartInfo info);

        IISResultInfo StartSite(IISStartInfo info);

        IISResultInfo StopSite(IISStartInfo info);

        IISResultInfo RecycleAppPool(string appPoolName);

        IISResultInfo StartAppPool(string appPoolName);

        IISResultInfo StopAppPool(string appPoolName);
    }
}
