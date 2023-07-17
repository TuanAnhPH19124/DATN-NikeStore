using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Webapi.Hubs
{
   
    public class CustomerHub : Hub
    {
        private static Dictionary<string, List<string>> userConnections = new Dictionary<string, List<string>>();

        public override async Task OnConnectedAsync()
        {

            string userEmail = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (!string.IsNullOrEmpty(userEmail))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userEmail);
            }
            await base.OnConnectedAsync();

        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userClaimId = Context.User?.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userClaimId != null)
            {
                var userId = userClaimId.Value;
                lock (userConnections)
                {
                    if (userConnections.ContainsKey(userId))
                    {
                        userConnections[userId].Remove(Context.ConnectionId);

                        if (userConnections[userId].Count == 0)
                        {
                            userConnections.Remove(userId);
                        }
                    }

                }
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string email, string message)
        {
            await Clients.Group(email).SendAsync("ReceiveMessage", message);
        }
    }
}