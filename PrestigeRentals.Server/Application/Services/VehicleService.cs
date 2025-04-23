using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Exceptions;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services
{
    /// <summary>
    /// Service that handles operations related to vehicles.
    /// </summary>
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<VehicleService> _logger;
        private readonly IMapper _mapper;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleOptionsRepository _vehicleOptionsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleService"/> class.
        /// </summary>
        /// <param name="dbContext">The application database context.</param>
        /// <param name="mapper">AutoMapper instance for mapping between DTOs and entities.</param>
        /// <param name="logger">Logger instance for logging service operations.</param>
        /// <param name="vehicleRepository">Vehicle repository for accessing vehicle data.</param>
        /// <param name="vehicleOptionsRepository">Repository for accessing vehicle options data.</param>
        public VehicleService(ApplicationDbContext dbContext, IMapper mapper, ILogger<VehicleService> logger, IVehicleRepository vehicleRepository, IVehicleOptionsRepository vehicleOptionsRepository)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _vehicleOptionsRepository = vehicleOptionsRepository ?? throw new ArgumentNullException(nameof(vehicleOptionsRepository));
        }

        /// <summary>
        /// Retrieves a vehicle by its ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the vehicle entity.</returns>
        public async Task<Vehicle> GetVehicleByID(int vehicleId)
        {
            var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId);
            if (vehicle == null)
            {
                _logger.LogWarning($"Vehicle with ID {vehicleId} not found.");
                throw new VehicleNotFoundException(vehicleId);
            }

            return vehicle;
        }

        /// <summary>
        /// Retrieves the options for a specific vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to retrieve options for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the vehicle options entity.</returns>
        public async Task<VehicleOptions> GetVehicleOptions(int vehicleId)
        {
            var vehicleOptions = await _dbContext.VehicleOptions.FirstOrDefaultAsync(vo => vo.VehicleId == vehicleId);
            if (vehicleOptions == null)
            {
                _logger.LogWarning($"Options for vehicle with ID {vehicleId} not found.");
                throw new VehicleOptionsNotFoundException(vehicleId);
            }

            return vehicleOptions;
        }

        /// <summary>
        /// Checks if a vehicle is active and not deleted.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the vehicle is active.</returns>
        private async Task<bool> IsVehicleAlive(int vehicleId)
        {
            return await _dbContext.Vehicles.AnyAsync(v => v.Id == vehicleId && v.Active && !v.Deleted);
        }

        /// <summary>
        /// Checks if the vehicle options for a given vehicle are active and not deleted.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the vehicle options are active.</returns>
        private async Task<bool> IsVehicleOptionsAlive(int vehicleId)
        {
            return await _dbContext.VehicleOptions.AnyAsync(vo => vo.VehicleId == vehicleId && vo.Active && !vo.Deleted);
        }

        /// <summary>
        /// Deactivates and marks a vehicle as deleted.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to deactivate.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        public async Task<bool> DeactivateVehicle(int vehicleId)
        {
            var isVehicleAlive = await IsVehicleAlive(vehicleId);
            var isVehicleOptionsAlive = await IsVehicleOptionsAlive(vehicleId);

            if (!isVehicleAlive && !isVehicleOptionsAlive)
            {
                _logger.LogWarning($"Vehicle with ID {vehicleId} is already deactivated or not found.");
                throw new VehicleAlreadyDeactivatedException(vehicleId);
            }

            var vehicle = await GetVehicleByID(vehicleId);
            vehicle.Active = false;
            vehicle.Deleted = true;

            var vehicleOptions = await GetVehicleOptions(vehicleId);
            vehicleOptions.Active = false;
            vehicleOptions.Deleted = true;

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Vehicle with ID {vehicleId} has been deactivated.");
            return true;
        }

        /// <summary>
        /// Activates a previously deactivated vehicle and its options.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to activate.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        public async Task<bool> ActivateVehicle(int vehicleId)
        {
            var isVehicleDead = await _dbContext.Vehicles.AnyAsync(v => v.Id == vehicleId && !v.Active && v.Deleted);
            var isVehicleOptionsDead = await _dbContext.VehicleOptions.AnyAsync(vo => vo.VehicleId == vehicleId && !vo.Active && vo.Deleted);

            if (!isVehicleDead && !isVehicleOptionsDead)
            {
                _logger.LogWarning($"Vehicle with ID {vehicleId} is already active or not found.");
                throw new VehicleAlreadyActiveException(vehicleId);
            }

            var vehicle = await GetVehicleByID(vehicleId);
            vehicle.Active = true;
            vehicle.Deleted = false;

            var vehicleOptions = await GetVehicleOptions(vehicleId);
            vehicleOptions.Active = true;
            vehicleOptions.Deleted = false;

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Vehicle with ID {vehicleId} has been activated.");
            return true;
        }

        /// <summary>
        /// Deletes a vehicle and its options from the database.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        public async Task<bool> DeleteVehicle(int vehicleId)
        {
            var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId);
            var vehicleOptions = await _dbContext.VehicleOptions.FirstOrDefaultAsync(vo => vo.VehicleId == vehicleId);

            if (vehicle != null && vehicleOptions != null)
            {
                _dbContext.VehicleOptions.Remove(vehicleOptions);
                _dbContext.Vehicles.Remove(vehicle);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Vehicle with ID {vehicleId} has been deleted.");
                return true;
            }

            _logger.LogWarning($"Vehicle with ID {vehicleId} not found or already deleted.");
            return false;
        }

        /// <summary>
        /// Retrieves all vehicles from the database, optionally filtering by active status.
        /// </summary>
        /// <param name="onlyActive">Optional flag to return only active vehicles.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of vehicles.</returns>
        public async Task<List<Vehicle>> GetAllVehicles(bool? onlyActive = false)
        {
            List<Vehicle> vehicles;

            if (onlyActive.HasValue && onlyActive.Value)
            {
                vehicles = await _dbContext.Vehicles.Where(v => v.Active && !v.Deleted).ToListAsync();
                _logger.LogInformation("Retrieved all active vehicles.");
            }
            else
            {
                vehicles = await _dbContext.Vehicles.ToListAsync();
                _logger.LogInformation("Retrieved all vehicles, including inactive ones.");
            }

            return vehicles;
        }

        /// <summary>
        /// Adds a new vehicle and its options to the database.
        /// </summary>
        /// <param name="vehicleRequest">Request containing the vehicle details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<ActionResult?> AddVehicle(VehicleRequest vehicleRequest)
        {
            if (vehicleRequest == null)
            {
                _logger.LogError("Received null vehicle request.");
                return new BadRequestObjectResult("Vehicle request cannot be null.");
            }

            var vehicle = new Vehicle
            {
                Make = vehicleRequest.Make,
                Model = vehicleRequest.Model,
                EngineSize = vehicleRequest.EngineSize,
                FuelType = vehicleRequest.FuelType,
                Transmission = vehicleRequest.Transmission
            };

            await _vehicleRepository.AddVehicle(vehicle);

            var vehicleOptions = new VehicleOptions
            {
                VehicleId = vehicle.Id,
                Navigation = vehicleRequest.Navigation,
                HeadsUpDisplay = vehicleRequest.HeadsUpDisplay,
                HillAssist = vehicleRequest.HillAssist,
                CruiseControl = vehicleRequest.CruiseControl
            };

            await _vehicleOptionsRepository.AddVehicleOptions(vehicleOptions);

            _logger.LogInformation($"Vehicle with ID {vehicle.Id} and its options have been added.");
            return new OkResult();
        }

        /// <summary>
        /// Updates an existing vehicle and its options.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to update.</param>
        /// <param name="vehicleUpdateRequest">Request containing the updated vehicle details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated vehicle DTO.</returns>
        public async Task<VehicleDTO> UpdateVehicle(int vehicleId, VehicleUpdateRequest vehicleUpdateRequest)
        {
            var vehicle = await GetVehicleByID(vehicleId);

            if (vehicle == null)
            {
                _logger.LogWarning($"Vehicle with ID {vehicleId} not found for update.");
                throw new VehicleUpdateException(vehicleId);
            }

            var vehicleOptions = await GetVehicleOptions(vehicleId);
            if (vehicleOptions == null)
            {
                _logger.LogWarning($"Vehicle options for vehicle ID {vehicleId} not found. Creating new options.");
                vehicleOptions = new VehicleOptions { VehicleId = vehicleId };
                _dbContext.VehicleOptions.Add(vehicleOptions);
                await _dbContext.SaveChangesAsync();
            }

            vehicle.Make = vehicleUpdateRequest.Make ?? vehicle.Make;
            vehicle.Model = vehicleUpdateRequest.Model ?? vehicle.Model;
            vehicle.EngineSize = vehicleUpdateRequest.EngineSize ?? vehicle.EngineSize;
            vehicle.FuelType = vehicleUpdateRequest.FuelType ?? vehicle.FuelType;
            vehicle.Transmission = vehicleUpdateRequest.Transmission ?? vehicle.Transmission;

            vehicleOptions.Navigation = vehicleUpdateRequest.Navigation ?? vehicleOptions.Navigation;
            vehicleOptions.HeadsUpDisplay = vehicleUpdateRequest.HeadsUpDisplay ?? vehicleOptions.HeadsUpDisplay;
            vehicleOptions.HillAssist = vehicleUpdateRequest.HillAssist ?? vehicleOptions.HillAssist;
            vehicleOptions.CruiseControl = vehicleUpdateRequest.CruiseControl ?? vehicleOptions.CruiseControl;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Vehicle with ID {vehicleId} has been updated.");
            return _mapper.Map<VehicleDTO>(vehicle);
        }
    }
}
