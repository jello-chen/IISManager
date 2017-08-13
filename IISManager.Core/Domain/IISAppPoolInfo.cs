namespace IISManager.Core.Domain
{
    public class IISAppPoolInfo
    {
        public string Name { get; set; }

        public bool Enable32BitAppOnWin64 { get; set; }

        public string ManagedRuntimeVersion { get; set; }

        public AppPoolState State { get; set; }

        public long WorkerProcessesCount { get; set; }

        public long MaxProcessesCount { get; set; }

        public bool AppPoolAutoStart { get; set; }

        public string AnonymousUserName { get; set; }

        public AppPoolIdentityType IdentityType { get; set; }
    }
}
