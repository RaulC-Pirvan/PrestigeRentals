using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services.Services
{
    public class OrderExpirationService : IOrderExpirationService
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<OrderExpirationService> _logger;

        public OrderExpirationService(ApplicationDbContext dbContext, ILogger<OrderExpirationService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task UpdateExpiredOrderAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var expiredOrders = await _dbContext.Orders
               .Where(o => o.EndTime <= now && !o.IsCancelled)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("[OrderExpirationService] Found {Count} expired orders at {Time}.", expiredOrders.Count, now);

            foreach (var order in expiredOrders)
            {
                var vehicle = await _dbContext.Vehicles.FindAsync(new object[] { order.VehicleId }, cancellationToken);

                if (vehicle != null)
                {
                    _logger.LogInformation("[OrderExpirationService] Vehicle {VehicleId} available: {Available}", vehicle.Id, vehicle.Available);

                    if (!vehicle.Available)
                    {
                        vehicle.Available = true;
                        _dbContext.Entry(vehicle).State = EntityState.Modified;

                        _logger.LogInformation("[OrderExpirationService] Vehicle {VehicleId} marked as available.", vehicle.Id);
                    }
                }
            }

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("[OrderExpirationService] Changes saved.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OrderExpirationService] Save failed: {Message}", ex.Message);
            }
        }
    }
}
