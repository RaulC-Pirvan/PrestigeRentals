using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrestigeRentals.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Infrastructure.Workers
{
    public class OrderBackgroundWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public OrderBackgroundWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IOrderExpirationService>();
                await service.UpdateExpiredOrderAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
