using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace IISManager.Workbench.Hubs
{
    [HubName("publishHub")]
    public class PublishHub : Hub
    {
        public void Send(string message, bool success)
        {
            Clients.All.Send();
        }
    }
}
