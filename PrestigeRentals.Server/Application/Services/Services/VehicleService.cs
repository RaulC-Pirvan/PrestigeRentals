using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Exceptions;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Services
{
    /// <summary>
    /// Service that handles operations related to vehicles.
    /// </summary>
    public class VehicleService : IVehicleService
    {
        private readonly ILogger<VehicleService> _logger;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleOptionsRepository _vehicleOptionsRepository;
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleService"/> class.
        /// </summary>
        /// <param name="mapper">AutoMapper instance for mapping between DTOs and entities.</param>
        /// <param name="logger">Logger instance for logging service operations.</param>
        /// <param name="vehicleRepository">Vehicle repository for accessing vehicle data.</param>
        /// <param name="vehicleOptionsRepository">Repository for accessing vehicle options data.</param>
        public VehicleService(ILogger<VehicleService> logger, IVehicleRepository vehicleRepository, IVehicleOptionsRepository vehicleOptionsRepository, IOrderRepository orderRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _vehicleOptionsRepository = vehicleOptionsRepository ?? throw new ArgumentNullException(nameof(vehicleOptionsRepository));
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Retrieves a vehicle by its ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the vehicle entity.</returns>
        public async Task<Vehicle> GetVehicleByID(long vehicleId)
        {
            Vehicle? vehicle = await _vehicleRepository.GetVehicleById(vehicleId);

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
        public async Task<VehicleOptions> GetVehicleOptions(long vehicleId)
        {
            VehicleOptions? vehicleOptions = await _vehicleRepository.GetVehicleOptionsByVehicleId(vehicleId);

            if (vehicleOptions == null)
            {
                _logger.LogWarning($"Options for vehicle with ID {vehicleId} not found.");
                throw new VehicleOptionsNotFoundException(vehicleId);
            }

            return vehicleOptions;
        }

        /// <summary>
        /// Checks if the vehicle options for a given vehicle are active and not deleted.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the vehicle options are active.</returns>
        private async Task<bool> IsVehicleOptionsAlive(long vehicleId)
        {
            return await _vehicleOptionsRepository.IsAliveAsync(vehicleId);
        }

        /// <summary>
        /// Deactivates and marks a vehicle as deleted.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to deactivate.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        public async Task<bool> DeactivateVehicle(long vehicleId)
        {
            bool isVehicleAlive = await _vehicleRepository.IsAliveAsync(vehicleId);
            bool isVehicleOptionsAlive = await _vehicleOptionsRepository.IsAliveAsync(vehicleId);

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

            await _vehicleRepository.UpdateAsync(vehicle);

            _logger.LogInformation($"Vehicle with ID {vehicleId} has been deactivated.");
            return true;
        }

        /// <summary>
        /// Activates a previously deactivated vehicle and its options.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to activate.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        public async Task<bool> ActivateVehicle(long vehicleId)
        {
            bool isVehicleDead = await _vehicleRepository.IsDeadAsync(vehicleId);
            bool isVehicleOptionsDead = await _vehicleOptionsRepository.IsDeadAsync(vehicleId);

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

            await _vehicleRepository.UpdateAsync(vehicle);
            _logger.LogInformation($"Vehicle with ID {vehicleId} has been activated.");
            return true;
        }

        /// <summary>
        /// Deletes a vehicle and its options from the database.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        public async Task<bool> DeleteVehicle(long vehicleId)
        {
            Vehicle? vehicle = await _vehicleRepository.GetVehicleById(vehicleId);
            VehicleOptions? vehicleOptions = await _vehicleOptionsRepository.GetVehicleOptionsById(vehicleId);

            if (vehicle != null && vehicleOptions != null)
            {
                await _vehicleOptionsRepository.DeleteAsync(vehicleOptions);
                await _vehicleRepository.DeleteAsync(vehicle);

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
            List<Vehicle> vehicles = await _vehicleRepository.GetAllVehiclesAsync(onlyActive);

            if (onlyActive.HasValue && onlyActive.Value)
            {
                _logger.LogInformation("Retrieved all active vehicles.");
            }
            else
            {
                _logger.LogInformation("Retrieved all vehicles, including inactive ones.");
            }

            return vehicles;
        }

        /// <summary>
        /// Adds a new vehicle and its options to the database.
        /// </summary>
        /// <param name="vehicleRequest">Request containing the vehicle details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<VehicleDTO> AddVehicle(VehicleRequest vehicleRequest)
        {
            if (vehicleRequest == null)
            {
                _logger.LogError("Received null vehicle request.");
                throw new ArgumentNullException(nameof(vehicleRequest), "Vehicle request cannot be null.");
            }

            var vehicle = new Vehicle
            {
                Make = vehicleRequest.Make,
                Model = vehicleRequest.Model,
                Chassis = vehicleRequest.Chassis,
                Horsepower = vehicleRequest.Horsepower,
                EngineSize = vehicleRequest.EngineSize,
                FuelType = vehicleRequest.FuelType,
                Transmission = vehicleRequest.Transmission
            };

            await _vehicleRepository.AddAsync(vehicle);

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

            var vehicleDTO = new VehicleDTO
            {
                Id = vehicle.Id,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Chassis = vehicle.Chassis,
                Horsepower = vehicle.Horsepower,
                EngineSize = vehicle.EngineSize,
                FuelType = vehicle.FuelType,
                Transmission = vehicle.Transmission,
                Active = vehicle.Active,
                Deleted = vehicle.Deleted,
                Available = vehicle.Available,

                // Manually map the VehicleOptions to the VehicleDTO
                Navigation = vehicleOptions.Navigation,
                HeadsUpDisplay = vehicleOptions.HeadsUpDisplay,
                HillAssist = vehicleOptions.HillAssist,
                CruiseControl = vehicleOptions.CruiseControl
            };

            return vehicleDTO;
        }

        /// <summary>
        /// Updates an existing vehicle and its options.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to update.</param>
        /// <param name="vehicleUpdateRequest">Request containing the updated vehicle details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated vehicle DTO.</returns>
        public async Task<VehicleDTO> UpdateVehicle(long vehicleId, VehicleUpdateRequest vehicleUpdateRequest)
        {
            var vehicle = await _vehicleRepository.GetVehicleById(vehicleId);
            var vehicleOptions = await _vehicleOptionsRepository.GetVehicleOptionsById(vehicleId);

            if (vehicle == null)
            {
                _logger.LogWarning($"Vehicle with ID {vehicleId} not found for update.");
                throw new VehicleUpdateException(vehicleId);
            }

            if (vehicleOptions == null)
            {
                _logger.LogWarning($"Vehicle options for vehicle ID {vehicleId} not found. Creating new options.");
                vehicleOptions = new VehicleOptions { VehicleId = vehicleId };
                await _vehicleOptionsRepository.AddVehicleOptions(vehicleOptions);
            }

            UpdateVehicleProperties(vehicle, vehicleUpdateRequest);
            UpdateVehicleOptionsProperties(vehicleOptions, vehicleUpdateRequest);

            await _vehicleRepository.UpdateAsync(vehicle);
            await _vehicleOptionsRepository.UpdateAsync(vehicleOptions);


            _logger.LogInformation($"Vehicle with ID {vehicleId} has been updated.");

            var vehicleDTO = new VehicleDTO
            {
                Id = vehicle.Id,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Chassis = vehicle.Chassis,
                Horsepower = vehicle.Horsepower,
                EngineSize = vehicle.EngineSize,
                FuelType = vehicle.FuelType,
                Transmission = vehicle.Transmission,
                Active = vehicle.Active,
                Deleted = vehicle.Deleted,
                Available = vehicle.Available,

                // Manually map the VehicleOptions to VehicleDTO
                Navigation = vehicleOptions.Navigation,
                HeadsUpDisplay = vehicleOptions.HeadsUpDisplay,
                HillAssist = vehicleOptions.HillAssist,
                CruiseControl = vehicleOptions.CruiseControl
            };

            return vehicleDTO;
        }
        private void UpdateVehicleProperties(Vehicle vehicle, VehicleUpdateRequest request)
        {
            vehicle.Make = request.Make ?? vehicle.Make;
            vehicle.Model = request.Model ?? vehicle.Model;
            vehicle.Chassis = request.Chassis ?? vehicle.Chassis;
            vehicle.Horsepower = request.Horsepower ?? vehicle.Horsepower;
            vehicle.EngineSize = request.EngineSize ?? vehicle.EngineSize;
            vehicle.FuelType = request.FuelType ?? vehicle.FuelType;
            vehicle.Transmission = request.Transmission ?? vehicle.Transmission;
        }

        private void UpdateVehicleOptionsProperties(VehicleOptions vehicleOptions, VehicleUpdateRequest request)
        {
            vehicleOptions.Navigation = request.Navigation ?? vehicleOptions.Navigation;
            vehicleOptions.HeadsUpDisplay = request.HeadsUpDisplay ?? vehicleOptions.HeadsUpDisplay;
            vehicleOptions.HillAssist = request.HillAssist ?? vehicleOptions.HillAssist;
            vehicleOptions.CruiseControl = request.CruiseControl ?? vehicleOptions.CruiseControl;
        }

        public async Task<List<VehicleAvailabilityDTO>> GetVehiclesWithAvailability(DateTime now, bool? onlyActive = false)
        {
            var vehicles = await _vehicleRepository.GetAllVehiclesAsync(onlyActive);

            // For each vehicle, get the active orders and determine availability
            var vehicleAvailability = new List<VehicleAvailabilityDTO>();

            foreach (var vehicle in vehicles)
            {
                var activeOrders = await _orderRepository.GetActiveOrdersForVehicleAsync(vehicle.Id, now, now);

                var order = activeOrders.FirstOrDefault();
                var availabilityDTO = new VehicleAvailabilityDTO
                {
                    VehicleID = vehicle.Id,
                    Make = vehicle.Make,
                    Model = vehicle.Model,
                    IsAvailable = order == null,
                    AvailableAt = order?.EndTime
                };

                vehicleAvailability.Add(availabilityDTO);
            }

            return vehicleAvailability;
        }
    }
}
