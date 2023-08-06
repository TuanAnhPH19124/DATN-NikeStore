using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace Webapi.Hubs
{
    public class ManagerHub : Hub
    {
        public static string managerGroup = "manager-group";
        private static Dictionary<string, List<string>> employees = new Dictionary<string, List<string>>();
        private static Dictionary<string, List<string>> personConnection = new Dictionary<string, List<string>>();
        public override async Task OnConnectedAsync()
        {
            try
            {
                string userName = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(userName))
                {
                    throw new Exception("Can not conntected, Wrong credential!");
                }
                if (personConnection.ContainsKey(userName))
                {
                    personConnection[userName].Add(Context.ConnectionId);
                }
                else
                {
                    var newList = new List<string>();
                    newList.Add(Context.ConnectionId);
                    personConnection.Add(userName, newList);
                }
                employees.Add(userName, new List<string>());
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                string userName = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(userName))
                {
                    throw new Exception("Can not disconnected, Wrong credential!");
                }
                if (personConnection.ContainsKey(userName))
                {
                    personConnection[userName].Remove(Context.ConnectionId);
                    
                    if (personConnection[userName].Count == 0)
                    {
                        personConnection.Remove(userName);
                        employees.Remove(userName);
                    }
                  
                }   
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SendMessageToManager(string message)
        {
            await Clients.Group(managerGroup).SendAsync("ReceiveMessage", message);
        }
    }
}
