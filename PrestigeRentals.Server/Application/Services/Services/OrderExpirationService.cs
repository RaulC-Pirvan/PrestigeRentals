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
        private readonly IEmailService _emailService;

        public OrderExpirationService(ApplicationDbContext dbContext, ILogger<OrderExpirationService> logger, IEmailService emailService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task UpdateExpiredOrderAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var activeOrders = await _dbContext.Orders.Where(o => o.StartTime <= now && o.EndTime > now && !o.IsCancelled).ToListAsync(cancellationToken);

            _logger.LogInformation("[OrderExpirationService] Found {Count} active orders at {Time}.", activeOrders.Count, now);

            foreach(var order in activeOrders)
            {
                var vehicle = await _dbContext.Vehicles.FindAsync(new object[] { order.VehicleId }, cancellationToken);
                if (vehicle != null && vehicle.Available)
                {
                    vehicle.Available = false;
                    _dbContext.Entry(vehicle).State = EntityState.Modified;
                    _logger.LogInformation("[OrderExpirationService] Vehicle {VehicleId} marked as unavailable.", vehicle.Id);
                }
            }
            
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

                    if(!order.ReviewReminderSet)
                    {
                        var user = await _dbContext.Users.FindAsync(new object[] { order.UserId }, cancellationToken);

                        if(user != null)
                        {
                            var vechileName = $"{vehicle.Make} {vehicle.Model}";
                            await _emailService.SendReviewRequestEmailAsync(user.Email, vechileName, order.Id);

                            order.ReviewReminderSet = true;
                            _logger.LogInformation("[OrderExpirationService] Sent review request email to user.");
                        }
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
