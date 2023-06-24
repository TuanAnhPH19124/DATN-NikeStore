using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Webapi
{
    public class BackgroundWorkerService : BackgroundService
    {
        readonly ILogger<BackgroundWorkerService> _logger;

        public BackgroundWorkerService(ILogger<BackgroundWorkerService> logger)
        {
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker runnin ad {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}