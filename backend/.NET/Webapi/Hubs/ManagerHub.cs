using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Webapi.Hubs
{
    public class ManagerHub : Hub
    {
        public static string managerGroup = "manager-group";
        public async Task AddToGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, managerGroup);
        }

        public async Task RemoveFromGroup()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, managerGroup);
        }

        public async Task SendMessageToManager(string message)
        {
            await Clients.Group(managerGroup).SendAsync("ReceiveMessage", message);
        }
    }
}
