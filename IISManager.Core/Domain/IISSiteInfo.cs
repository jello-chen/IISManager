using System.Collections.Generic;

namespace IISManager.Core.Domain
{
    public class IISSiteInfo
    {
        public string Name { get; set; }
        public long ID { get; set; }
        public ServerState State { get; set; }
        public bool ServerAutoStart { get; set; }
        public string ServerBindings { get; set; }
        public string DefaultDoc { get; set; }
        public string AnonymousUserName { get; set; }
        public bool EnableDirBrowsing { get; set; }
        public string ServerComment { get; set; }
        public string DefaultApplicationPoolName { get; set; }
        public List<SiteApplicationInfo> Applications { get; set; }
    }
}
