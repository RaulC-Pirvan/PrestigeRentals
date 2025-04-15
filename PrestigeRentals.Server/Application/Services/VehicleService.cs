using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace PrestigeRentals.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<VehicleService> _logger;
        private readonly IMapper _mapper;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleOptionsRepository _vehicleOptionsRepository;

        public VehicleService(ApplicationDbContext dbContext, IMapper mapper, ILogger<VehicleService> logger, IVehicleRepository vehicleRepository, IVehicleOptionsRepository VehicleOptionsRepository)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _vehicleRepository = vehicleRepository;
            _vehicleOptionsRepository = VehicleOptionsRepository;
        }

        public async Task<Vehicle> GetVehicleByID(int vehicleId)
        {
            Vehicle vehicle = await _dbContext.Vehicles.Where(v => v.Id == vehicleId).FirstOrDefaultAsync();

            if (vehicle == null)
            {
                _logger.LogWarning($"Vehicle with ID {vehicleId} not found.");
                throw new VehicleNotFoundException(vehicleId);
            }
                
            return vehicle;
        }

        public async Task<VehicleOptions> GetVehicleOptions(int vehicleId)
        {
            VehicleOptions vehicleOptions = await _dbContext.VehicleOptions.Where(vo => vo.VehicleId == vehicleId).FirstOrDefaultAsync();

            if (vehicleOptions == null)
            {
                _logger.LogWarning($"Options for vehicle with ID {vehicleId} not found.");
                throw new VehicleOptionsNotFoundException(vehicleId);
            }
                
            return vehicleOptions;
        }

        private async Task<bool> IsVehicleAlive(int vehicleId)
        {
            bool isVehicleAlive = await _dbContext.Vehicles.AnyAsync(v => v.Id == vehicleId && v.Active && !v.Deleted);

            return isVehicleAlive;
        }

        private async Task<bool> IsVehicleOptionsAlive(int vehicleId)
        {
            bool isVehicleOptionsAlive = await _dbContext.VehicleOptions.AnyAsync(vo => vo.VehicleId == vehicleId && vo.Active && !vo.Deleted);

            return isVehicleOptionsAlive;
        }

        public async Task<bool> DeactivateVehicle(int vehicleId)
        {
            bool isVehicleAlive = await IsVehicleAlive(vehicleId);
            bool isVehicleOptionsAlive = await IsVehicleOptionsAlive(vehicleId);

            if (!isVehicleAlive && !isVehicleOptionsAlive)
            {
                _logger.LogWarning($"Vehicle with ID {vehicleId} was not found or is already dead.");
                throw new VehicleAlreadyDeactivatedException(vehicleId);
            }

            Vehicle vehicle = await GetVehicleByID(vehicleId);
            vehicle.Active = false;
            vehicle.Deleted = true;

            VehicleOptions vehicleOptions = await GetVehicleOptions(vehicleId);
            vehicleOptions.Active = false;
            vehicleOptions.Deleted = true;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Vehicle with ID {vehicleId} has been deactivated.");
            return true;
        }

        public async Task<bool> ActivateVehicle(int vehicleId)
        {
            bool isVehicleDead = await _dbContext.Vehicles.AnyAsync(v => v.Id == vehicleId && !v.Active && v.Deleted);
            bool isVehicleOptionsDead = await _dbContext.VehicleOptions.AnyAsync(vo => vo.VehicleId == vehicleId && !vo.Active && vo.Deleted);

            if (!isVehicleDead && !isVehicleOptionsDead)
            {
                _logger.LogWarning($"Vehicle with ID {vehicleId} was not found or is already active.");
                throw new VehicleAlreadyActiveException(vehicleId);
            }
           
            Vehicle vehicle = await GetVehicleByID(vehicleId);
            vehicle.Active = true;
            vehicle.Deleted = false;
               
            VehicleOptions vehicleOptions = await GetVehicleOptions(vehicleId);
            vehicleOptions.Active = true;
            vehicleOptions.Deleted = false;
               
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Vehicle with ID {vehicleId} has been activated.");
            return true;
        }

        public async Task<bool> DeleteVehicle(int vehicleId)
        {
            Vehicle vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId);
            VehicleOptions vehicleOptions = await _dbContext.VehicleOptions.FirstOrDefaultAsync(vo => vo.VehicleId == vehicleId);

            if(vehicle != null && vehicleOptions != null)
            {
                _dbContext.VehicleOptions.Remove(vehicleOptions);
                await _dbContext.SaveChangesAsync();

                _dbContext.Vehicles.Remove(vehicle);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Vehicle with ID {vehicleId} has been deleted.");
                return true;
            }

            _logger.LogWarning($"Vehicle with ID {vehicleId} not found or has been already deleted.");
            return false;
        }

        public async Task<List<Vehicle>> GetAllVehicles(bool? onlyActive = false)
        {
            if (onlyActive.HasValue && onlyActive.Value)
            {
                List<Vehicle> activeVehicles = await _dbContext.Vehicles.Where(v => v.Active && !v.Deleted).ToListAsync();

                _logger.LogInformation($"Retrieved all alive vehicles.");
                return activeVehicles;
            }

            List<Vehicle> allVehicles = await _dbContext.Vehicles.ToListAsync();

            _logger.LogInformation($"Retrieved all vehicles (including inactive).");
            return allVehicles;

        }

        public async Task<ActionResult?> AddVehicle(VehicleRequest vehicleRequest)
        {
            var vehicle = new Vehicle
            {
                Make = vehicleRequest.Make,
                Model = vehicleRequest.Model,
                EngineSize = vehicleRequest.EngineSize,
                FuelType = vehicleRequest.FuelType,
                Transmission = vehicleRequest.Transmission
            };

            await _vehicleRepository.AddVehicle(vehicle);
            await _dbContext.SaveChangesAsync();

            var vehicleOptions = new VehicleOptions
            {
                VehicleId = vehicle.Id,
                Navigation = vehicleRequest.Navigation,
                HeadsUpDisplay = vehicleRequest.HeadsUpDisplay,
                HillAssist = vehicleRequest.HillAssist,
                CruiseControl = vehicleRequest.CruiseControl
            };

            await _vehicleOptionsRepository.AddVehicleOptions(vehicleOptions);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Vehicle with ID {vehicle.Id} has been added.");
            return null;
        }

        public async Task<VehicleDTO> UpdateVehicle(int vehicleId, VehicleUpdateRequest vehicleUpdateRequest)
        {
            Vehicle vehicle = await GetVehicleByID(vehicleId);
            if (vehicle == null)
            {
                _logger.LogWarning($"Vehicle with ID {vehicleId} not found for update.");
                throw new VehicleUpdateException(vehicleId);
            }

            VehicleOptions vehicleOptions = await GetVehicleOptions(vehicleId);
            if (vehicleOptions == null)
            {
                _logger.LogWarning($"Options for vehicle with ID {vehicleId} not found. Creating new options.");
                vehicleOptions = new VehicleOptions { VehicleId = vehicleId };
                _dbContext.VehicleOptions.Add(vehicleOptions);
                await _dbContext.SaveChangesAsync(); 
            }
           
            vehicle.Make = string.IsNullOrWhiteSpace(vehicleUpdateRequest.Make) ? vehicle.Make : vehicleUpdateRequest.Make;
            vehicle.Model = string.IsNullOrWhiteSpace(vehicleUpdateRequest.Model) ? vehicle.Model : vehicleUpdateRequest.Model;
            vehicle.EngineSize = vehicleUpdateRequest.EngineSize.HasValue && vehicleUpdateRequest.EngineSize.Value != 0 ? vehicleUpdateRequest.EngineSize.Value : vehicle.EngineSize;
            vehicle.FuelType = string.IsNullOrWhiteSpace(vehicleUpdateRequest.FuelType) ? vehicle.FuelType : vehicleUpdateRequest.FuelType;
            vehicle.Transmission = string.IsNullOrWhiteSpace(vehicleUpdateRequest.Transmission) ? vehicle.Transmission : vehicleUpdateRequest.Transmission;

            if (vehicleUpdateRequest.Navigation.HasValue)
            {
                vehicleOptions.Navigation = vehicleUpdateRequest.Navigation.Value;
            }
            if (vehicleUpdateRequest.HeadsUpDisplay.HasValue)
            {
                vehicleOptions.HeadsUpDisplay = vehicleUpdateRequest.HeadsUpDisplay.Value;
            }
            if (vehicleUpdateRequest.HillAssist.HasValue)
            {
                vehicleOptions.HillAssist = vehicleUpdateRequest.HillAssist.Value;
            }
            if (vehicleUpdateRequest.CruiseControl.HasValue)
            {
                vehicleOptions.CruiseControl = vehicleUpdateRequest.CruiseControl.Value;
            }

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Vehicle with ID {vehicleId} has been updated.");

            return _mapper.Map<VehicleDTO>(vehicle);
        }
    }
}
