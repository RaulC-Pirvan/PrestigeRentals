using Microsoft.Extensions.Logging;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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
        public async Task AddAsync(Vehicle vehicle)
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

        public async Task<List<Vehicle>> GetAllVehiclesAsync(bool? onlyActive = false)
        {
            if (onlyActive.HasValue && onlyActive.Value)
            {
                return await _dbContext.Vehicles
                    .Where(v => v.Active && !v.Deleted)
                    .ToListAsync();
            }
            else
            {
                return await _dbContext.Vehicles
                    .ToListAsync();
            }
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                _logger.LogError("Attempted to update a null vehicle to the database.");
                throw new ArgumentNullException(nameof(vehicle), "Vehicle cannot be null.");
            }
            try
            {
                _logger.LogInformation("Attempting to update a vehicle to the database.");
                _dbContext.Vehicles.Update(vehicle);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Vehicle with ID {vehicle.Id} has been successfully updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating vehicle: {ex.Message}");
                throw new InvalidOperationException("An error occurred while updating the vehicle.", ex);
            }
        }

        public async Task DeleteAsync(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                _logger.LogError("Attempted to delete a null vehicle from the database.");
                throw new ArgumentNullException(nameof(vehicle), "Vehicle cannot be null.");
            }
            try
            {
                _logger.LogInformation("Attempting to delete an vehicle from the database.");
                _dbContext.Vehicles.Remove(vehicle);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Vehicle with ID {vehicle.Id} has been successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting vehicle: {ex.Message}");
                throw new InvalidOperationException("An error occurred while deleting the vehicle.", ex);
            }
        }

        public async Task<VehicleOptions?> GetVehicleOptionsByVehicleId(long vehicleId)
        {
            return await _dbContext.VehicleOptions.FirstOrDefaultAsync(vo => vo.VehicleId == vehicleId);
        }

        public async Task<bool> IsAliveAsync(long vehicleId)
        {
            return await _dbContext.Vehicles.AnyAsync(v => v.Id == vehicleId && v.Active && !v.Deleted);
        }
        public async Task<bool> IsDeadAsync(long vehicleId)
        {
            return await _dbContext.Vehicles.AnyAsync(v => v.Id == vehicleId && !v.Active && v.Deleted);
        }

        public async Task<List<long>> GetSimilarVehicleIdsAsync(long excludeId, string chassis, string transmission)
        {
            return await _dbContext.Vehicles.Where(v => v.Id != excludeId && v.Chassis == chassis && v.Transmission == transmission).Select(v => v.Id).ToListAsync();
        }
    }
}
