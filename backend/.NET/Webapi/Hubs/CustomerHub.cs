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
            string userEmail = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;


            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string email, string message)
        {
            await Clients.Group(email).SendAsync("ReceiveMessage", message);
            
        }
    }
}