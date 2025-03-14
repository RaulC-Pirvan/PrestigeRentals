using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services
{
    public class VehicleRepository : IVehicleRepository
    {
        ApplicationDbContext _dbContext;
        ILogger<VehicleRepository> _logger;

        public VehicleRepository(ApplicationDbContext dbContext, ILogger<VehicleRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddVehicle(Vehicle vehicle)
        {
            try
            {
                _dbContext.Vehicles.Add(vehicle);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error saving vehicle: {Message}", ex.Message);
                throw;
            }
        }
    }
}
