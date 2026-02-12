using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrestigeRentals.Application.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrestigeRentals.Infrastructure.Workers
{
    /// <summary>
    /// A background worker that periodically checks and updates expired vehicle rental orders.
    /// </summary>
    public class OrderBackgroundWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderBackgroundWorker"/> class.
        /// </summary>
        /// <param name="serviceProvider">The application's service provider for scoped dependency resolution.</param>
        public OrderBackgroundWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Periodically executes the order expiration service in a background loop.
        /// </summary>
        /// <param name="stoppingToken">Token used to cancel the background task.</param>
        /// <returns>A task that represents the background execution operation.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IOrderExpirationService>();
                await service.UpdateExpiredOrderAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
