using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services
{
    /// <summary>
    /// Repository for handling vehicle-related database operations.
    /// Implements <see cref="IVehicleRepository"/>.
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dbContext"/> or <paramref name="logger"/> is null.</exception>
        public VehicleRepository(ApplicationDbContext dbContext, ILogger<VehicleRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Database context cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        }

        /// <summary>
        /// Adds a new vehicle to the database asynchronously.
        /// </summary>
        /// <param name="vehicle">The vehicle to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs while saving.</exception>
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

        /// <summary>
        /// Retrieves a vehicle by its unique identifier asynchronously.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to retrieve.</param>
        /// <returns>The <see cref="Vehicle"/> entity if found; otherwise, null.</returns>
        public async Task<Vehicle> GetVehicleById(long vehicleId)
        {
            return await _dbContext.Vehicles.FindAsync(vehicleId);
        }

        /// <summary>
        /// Retrieves all vehicles, optionally filtered by active status.
        /// </summary>
        /// <param name="onlyActive">If true, returns only active and not deleted vehicles; otherwise, returns all vehicles.</param>
        /// <returns>A list of <see cref="Vehicle"/> entities.</returns>
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

        /// <summary>
        /// Updates an existing vehicle in the database asynchronously.
        /// </summary>
        /// <param name="vehicle">The vehicle entity with updated data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs while updating.</exception>
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

        /// <summary>
        /// Deletes a vehicle from the database asynchronously.
        /// </summary>
        /// <param name="vehicle">The vehicle entity to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs while deleting.</exception>
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

        /// <summary>
        /// Retrieves the vehicle options associated with a specific vehicle ID asynchronously.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <returns>The <see cref="VehicleOptions"/> entity if found; otherwise, null.</returns>
        public async Task<VehicleOptions?> GetVehicleOptionsByVehicleId(long vehicleId)
        {
            return await _dbContext.VehicleOptions.FirstOrDefaultAsync(vo => vo.VehicleId == vehicleId);
        }

        /// <summary>
        /// Checks whether a vehicle with the specified ID is active and not deleted.
        /// </summary>
        /// <param name="vehicleId">The vehicle ID to check.</param>
        /// <returns>True if the vehicle is active; otherwise, false.</returns>
        public async Task<bool> IsAliveAsync(long vehicleId)
        {
            return await _dbContext.Vehicles.AnyAsync(v => v.Id == vehicleId && v.Active && !v.Deleted);
        }

        /// <summary>
        /// Checks whether a vehicle with the specified ID is inactive or deleted.
        /// </summary>
        /// <param name="vehicleId">The vehicle ID to check.</param>
        /// <returns>True if the vehicle is inactive or deleted; otherwise, false.</returns>
        public async Task<bool> IsDeadAsync(long vehicleId)
        {
            return await _dbContext.Vehicles.AnyAsync(v => v.Id == vehicleId && !v.Active && v.Deleted);
        }

        /// <summary>
        /// Retrieves a list of vehicle IDs that have similar characteristics, excluding the specified vehicle ID.
        /// </summary>
        /// <param name="excludeId">The ID of the vehicle to exclude.</param>
        /// <param name="chassis">The chassis type for similarity matching.</param>
        /// <param name="transmission">The transmission type for similarity matching.</param>
        /// <returns>A list of vehicle IDs matching the criteria.</returns>
        public async Task<List<long>> GetSimilarVehicleIdsAsync(long excludeId, string chassis, string transmission)
        {
            return await _dbContext.Vehicles
                .Where(v => v.Id != excludeId && v.Chassis == chassis && v.Transmission == transmission)
                .Select(v => v.Id)
                .ToListAsync();
        }
    }
}
