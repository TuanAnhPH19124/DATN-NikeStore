using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Webapi.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, List<string>> Employees = new Dictionary<string, List<string>>();

        public override async Task OnConnectedAsync()
        {
            //var userName = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            // this condition make sure that user has loged in or not
            //if (!string.IsNullOrEmpty(userName))
            //{
            //    await Groups.AddToGroupAsync(Context.ConnectionId, userName);
            //}   
            var userRole = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!string.IsNullOrEmpty(userRole) && userRole == "Admin")
            {
                if (!Employees.ContainsKey(Context.ConnectionId))
                {
                    Employees.Add(Context.ConnectionId, new List<string>());
                }
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userRole = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!string.IsNullOrEmpty(userRole) && userRole == "Admin")
            {
                if (!Employees.ContainsKey(Context.ConnectionId))
                {
                    Employees.Remove(Context.ConnectionId);
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string Id, string message)
        {
            await Clients.Client(Id).SendAsync("receiveMessage", message, Context.ConnectionId);
        }

        public IEnumerable<string> GetSupportUser()
        {
            var supports = Employees.Keys.ToList();
            return supports;
        }
    }
}
