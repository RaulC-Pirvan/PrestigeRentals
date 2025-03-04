using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services
{
    public class VehicleService(ApplicationDbContext dbContext, IMapper mapper) : IVehicleService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<Vehicle> GetVehicleByID(int vehicleId)
        {
            Vehicle vehicle = await _dbContext.Vehicles.Where(v => v.Id == vehicleId).FirstOrDefaultAsync();

            return vehicle;
        }

        private async Task<bool> IsVehicleAlive(int vehicleId)
        {
            bool isVehicleAlive = await _dbContext.Vehicles.AnyAsync(v => v.Id == vehicleId && v.Active && !v.Deleted);

            return isVehicleAlive;
        }

        public async Task<bool> DeactivateVehicle(int vehicleId)
        {
            bool isVehicleAlive = await IsVehicleAlive(vehicleId);

            if (isVehicleAlive)
            {
                Vehicle vehicle = await GetVehicleByID(vehicleId);
                vehicle.Active = false;
                vehicle.Deleted = true;

                var vehicleEntry = _dbContext.Entry(vehicle);
                vehicleEntry.State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> ActivateVehicle(int vehicleId)
        {
            bool isVehicleDead = await _dbContext.Vehicles.AnyAsync(v => v.Id == vehicleId && !v.Active && v.Deleted);

            if (isVehicleDead)
            {
                Vehicle vehicle = await GetVehicleByID(vehicleId);
                vehicle.Active = true;
                vehicle.Deleted = false;

                var vehicleEntry = _dbContext.Entry(vehicle);
                vehicleEntry.State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteVehicle(int vehicleId)
        {
            Vehicle vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId);

            if(vehicle != null)
            {
                _dbContext.Vehicles.Remove(vehicle);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<List<Vehicle>> GetAllVehicles(bool? onlyActive = false)
        {
            if(onlyActive.HasValue && onlyActive.Value)
                return await _dbContext.Vehicles.Where(v => v.Active && !v.Deleted).ToListAsync();
            return await _dbContext.Vehicles.ToListAsync();
        }

        public async Task<ActionResult?> AddVehicle(VehicleRequest vehicleRequest)
        {
            Vehicle mappedVehicle = _mapper.Map<Vehicle>(vehicleRequest);

            _dbContext.Vehicles.Add(mappedVehicle);
            await _dbContext.SaveChangesAsync();

            return null;
        }

        public async Task<VehicleDTO> UpdateVehicle(int vehicleId, VehicleRequest vehicleRequest)
        {
            Vehicle vehicle = await GetVehicleByID(vehicleId);

            if (vehicle != null)
            {
                vehicle.Make = vehicleRequest.Make ?? vehicle.Make;
                vehicle.Model = vehicleRequest.Model ?? vehicle.Model;
                vehicle.EngineSize = vehicleRequest.EngineSize != 0 ? vehicleRequest.EngineSize : vehicle.EngineSize;
                vehicle.FuelType = vehicleRequest.FuelType ?? vehicle.FuelType;
                vehicle.Transmission = vehicleRequest.Transmission ?? vehicle.Transmission;
            }

            _dbContext.Entry(vehicle).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            VehicleDTO vehicleDTO = _mapper.Map<VehicleDTO>(vehicle);

            return vehicleDTO;
        }


    }
}
