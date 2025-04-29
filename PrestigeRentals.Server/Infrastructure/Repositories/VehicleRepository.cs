using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;
using PrestigeRentals.Application.Services.Interfaces.Repositories;

namespace PrestigeRentals.Application.Services
{
    /// <summary>
    /// Repository for handling vehicle-related database operations.
    /// </summary>
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<VehicleRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The application database context.</param>
        /// <param name="logger">Logger instance to log repository activities.</param>
        public VehicleRepository(ApplicationDbContext dbContext, ILogger<VehicleRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Database context cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        }

        /// <summary>
        /// Adds a new vehicle to the database.
        /// </summary>
        /// <param name="vehicle">The vehicle to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                _logger.LogError("Attempted to add a null vehicle to the database.");
                throw new ArgumentNullException(nameof(vehicle), "Vehicle cannot be null.");
            }

            try
            {
                _logger.LogInformation("Attempting to add a new vehicle to the database.");
                _dbContext.Vehicles.Add(vehicle);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Vehicle with ID {vehicle.Id} has been successfully added.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving vehicle: {ex.Message}");
                throw new InvalidOperationException("An error occurred while saving the vehicle.", ex);
            }
        }

        public async Task<Vehicle> GetVehicleById(long vehicleId)
        {
            return await _dbContext.Vehicles.FindAsync(vehicleId);
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            var existingVehicle = await _dbContext.Vehicles.FindAsync(vehicle.Id);
            if(existingVehicle != null)
            {
                existingVehicle.Available = vehicle.Available;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
