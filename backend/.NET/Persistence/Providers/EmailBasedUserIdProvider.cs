using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Providers
{
    public class EmailBasedUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var user = connection.User;
            var email = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            return email;
        }
    }
}
