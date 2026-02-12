using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services
{
    /// <summary>
    /// Repository class responsible for managing <see cref="VehicleOptions"/> in the database.
    /// Implements <see cref="IVehicleOptionsRepository"/>.
    /// </summary>
    public class VehicleOptionsRepository : IVehicleOptionsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<VehicleOptionsRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleOptionsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The application database context.</param>
        /// <param name="logger">The logger for logging database operations.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dbContext"/> or <paramref name="logger"/> is null.</exception>
        public VehicleOptionsRepository(ApplicationDbContext dbContext, ILogger<VehicleOptionsRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Database context cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        }

        /// <summary>
        /// Adds the specified vehicle options to the database asynchronously.
        /// </summary>
        /// <param name="vehicleOptions">The vehicle options to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicleOptions"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs during the database operation.</exception>
        public async Task AddVehicleOptions(VehicleOptions vehicleOptions)
        {
            if (vehicleOptions == null)
            {
                _logger.LogError("Attempted to add null vehicle options.");
                throw new ArgumentNullException(nameof(vehicleOptions), "Vehicle options cannot be null.");
            }

            try
            {
                _logger.LogInformation("Attempting to add vehicle options to the database for VehicleId: {VehicleId}.", vehicleOptions.VehicleId);
                await _dbContext.VehicleOptions.AddAsync(vehicleOptions);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Vehicle options for VehicleId: {VehicleId} have been successfully added.", vehicleOptions.VehicleId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while adding vehicle options for VehicleId: {vehicleOptions.VehicleId}. Error: {ex.Message}", ex);
                throw new InvalidOperationException("An error occurred while adding the vehicle options.", ex);
            }
        }

        /// <summary>
        /// Retrieves vehicle options by the vehicle's unique identifier.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle whose options are requested.</param>
        /// <returns>The <see cref="VehicleOptions"/> associated with the specified vehicle.</returns>
        public async Task<VehicleOptions> GetVehicleOptionsById(long vehicleId)
        {
            return await _dbContext.VehicleOptions.FindAsync(vehicleId);
        }

        /// <summary>
        /// Updates existing vehicle options in the database asynchronously.
        /// </summary>
        /// <param name="vehicleOptions">The vehicle options entity with updated data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicleOptions"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs during the update.</exception>
        public async Task UpdateAsync(VehicleOptions vehicleOptions)
        {
            if (vehicleOptions == null)
            {
                _logger.LogError("Attempted to update a null list of vehicle options to the database.");
                throw new ArgumentNullException(nameof(vehicleOptions), "Vehicle options cannot be null.");
            }
            try
            {
                _logger.LogInformation("Attempting to update a list of vehicle options to the database.");
                _dbContext.VehicleOptions.Update(vehicleOptions);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Vehicle with ID {vehicleOptions.Id} has been successfully updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating vehicle options: {ex.Message}");
                throw new InvalidOperationException("An error occurred while updating the vehicle options.", ex);
            }
        }

        /// <summary>
        /// Deletes vehicle options from the database asynchronously.
        /// </summary>
        /// <param name="vehicleOptions">The vehicle options entity to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicleOptions"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs during the deletion.</exception>
        public async Task DeleteAsync(VehicleOptions vehicleOptions)
        {
            if (vehicleOptions == null)
            {
                _logger.LogError("Attempted to delete a null list of vehicle options from the database.");
                throw new ArgumentNullException(nameof(vehicleOptions), "Vehicle options cannot be null.");
            }
            try
            {
                _logger.LogInformation("Attempting to delete vehicle options from the database.");
                _dbContext.VehicleOptions.Remove(vehicleOptions);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Vehicle options with ID {vehicleOptions.Id} have been successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting vehicle options: {ex.Message}");
                throw new InvalidOperationException("An error occurred while deleting the vehicle options.", ex);
            }
        }

        /// <summary>
        /// Checks if vehicle options for the specified vehicle ID are active and not deleted.
        /// </summary>
        /// <param name="vehicleId">The vehicle ID to check.</param>
        /// <returns>True if active and not deleted; otherwise, false.</returns>
        public async Task<bool> IsAliveAsync(long vehicleId)
        {
            return await _dbContext.VehicleOptions.AnyAsync(vo => vo.Id == vehicleId && vo.Active && !vo.Deleted);
        }

        /// <summary>
        /// Checks if vehicle options for the specified vehicle ID are marked as inactive or deleted.
        /// </summary>
        /// <param name="vehicleId">The vehicle ID to check.</param>
        /// <returns>True if inactive or deleted; otherwise, false.</returns>
        public async Task<bool> IsDeadAsync(long vehicleId)
        {
            return await _dbContext.VehicleOptions.AnyAsync(vo => vo.Id == vehicleId && !vo.Active && vo.Deleted);
        }
    }
}
