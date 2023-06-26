using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Webapi
{
    public class AuthBackgroundService : BackgroundService
    {
        private readonly ILogger<AuthBackgroundService> _logger;
        private readonly UserActivityTracker _userActivityTracker;

        public AuthBackgroundService(ILogger<AuthBackgroundService> logger, UserActivityTracker userActivityTracker)
        {
            _logger = logger;
            _userActivityTracker = userActivityTracker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _userActivityTracker.UserLoggedIn += UserLoggedInHandler;
            _userActivityTracker.UserLoggedOut += UserLoggedOutHandler;
           

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }

            _userActivityTracker.UserLoggedIn -= UserLoggedInHandler;
            _userActivityTracker.UserLoggedOut -= UserLoggedOutHandler;
           
        }

        private void UserLoggedInHandler(object sender, string username)
        {
            _logger.LogInformation("User logged in: {Username} at {time}", username, DateTimeOffset.Now);
        }

        private void UserLoggedOutHandler(object sender, string username)
        {
            _logger.LogInformation("User logged out: {Username}", username);
        }
    }
}
