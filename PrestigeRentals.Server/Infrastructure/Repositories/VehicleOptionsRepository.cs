using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services
{
    /// <summary>
    /// Repository class responsible for managing VehicleOptions in the database.
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
        public VehicleOptionsRepository(ApplicationDbContext dbContext, ILogger<VehicleOptionsRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Database context cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        }

        /// <summary>
        /// Adds the specified vehicle options to the database.
        /// </summary>
        /// <param name="vehicleOptions">The vehicle options to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="vehicleOptions"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs during the database operation.</exception>
        public async Task AddVehicleOptions(VehicleOptions vehicleOptions)
        {
            // Validate the input to ensure vehicleOptions is not null
            if (vehicleOptions == null)
            {
                _logger.LogError("Attempted to add null vehicle options.");
                throw new ArgumentNullException(nameof(vehicleOptions), "Vehicle options cannot be null.");
            }

            try
            {
                // Log the beginning of the database operation
                _logger.LogInformation("Attempting to add vehicle options to the database for VehicleId: {VehicleId}.", vehicleOptions.VehicleId);

                // Add the vehicle options to the database
                await _dbContext.VehicleOptions.AddAsync(vehicleOptions);

                // Save changes asynchronously
                await _dbContext.SaveChangesAsync();

                // Log success if the operation is successful
                _logger.LogInformation("Vehicle options for VehicleId: {VehicleId} have been successfully added.", vehicleOptions.VehicleId);
            }
            catch (Exception ex)
            {
                // Log the exception if any error occurs
                _logger.LogError($"An error occurred while adding vehicle options for VehicleId: {vehicleOptions.VehicleId}. Error: {ex.Message}", ex);
                throw new InvalidOperationException("An error occurred while adding the vehicle options.", ex);
            }
        }
    }
}
