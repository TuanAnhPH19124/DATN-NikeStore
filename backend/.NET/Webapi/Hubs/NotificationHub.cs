using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Webapi.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task OrderNotification()
        {
            await Clients.All.SendAsync("updateNotification", "Có đơn hàng mới cần bạn xác nhận");
           
        }
    }
}
