namespace IISManager.Core.Domain
{
    public enum ServerState
    {
        Starting = 1,
        Started,
        Stopping,
        Stopped,
        Pausing,
        Paused,
        Continuing
    }
}
