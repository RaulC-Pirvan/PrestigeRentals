using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services
{
    public class VehicleOptionsService : IVehicleOptionsService
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<VehiclePhotosService> _logger;
        public VehicleOptionsService(ApplicationDbContext dbContext, ILogger<VehiclePhotosService> logger) 
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<VehicleOptions> AddVehicleOptions(int vehicleId,VehicleOptionsRequest vehicleOptionsRequest)
        {
            VehicleOptions vehicleOptions = new VehicleOptions()

            {
                VehicleId = vehicleId,
                Navigation = vehicleOptionsRequest.Navigation,
                HeadsUpDisplay = vehicleOptionsRequest.HeadsUpDisplay,
                HillAssist = vehicleOptionsRequest.HillAssist,
                CruiseControl = vehicleOptionsRequest.CruiseControl,
            };

            _dbContext.VehicleOptions.Add(vehicleOptions);
            await _dbContext.SaveChangesAsync();
            
            return vehicleOptions;
        }

        public async Task<VehicleOptions> GetOptionsByVehicleId(int vehicleId)
        {

            return await _dbContext.VehicleOptions.FirstOrDefaultAsync(vo => vo.VehicleId == vehicleId);
        }
        
        public async Task<VehicleOptions> UpdateVehicleOptions(int vehicleId, VehicleOptionsRequest vehicleOptionsRequest)
        {
            VehicleOptions existingOptions = await _dbContext.VehicleOptions.FirstOrDefaultAsync(vo => vo.VehicleId == vehicleId);

            if (existingOptions == null)
            {
                return null;
            }

            existingOptions.Navigation = vehicleOptionsRequest.Navigation;
            existingOptions.HeadsUpDisplay = vehicleOptionsRequest.HeadsUpDisplay;
            existingOptions.CruiseControl = vehicleOptionsRequest.CruiseControl;    
            existingOptions.HillAssist = vehicleOptionsRequest.HillAssist;

            _dbContext.Entry(existingOptions).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return existingOptions;

        }

        private async Task<bool> IsVehicleOptionsAlive(int vehicleId)
        {
            bool isVehicleOptionsAlive = await _dbContext.VehicleOptions.AnyAsync(vo => vo.VehicleId == vehicleId && vo.Active && !vo.Deleted);

            return isVehicleOptionsAlive;
        }

        public async Task<bool> DeactivateVehicleOptions(int vehicleId)
        {
            bool isVehicleOptionsAlive = await IsVehicleOptionsAlive(vehicleId);

            if(isVehicleOptionsAlive)
            {
                VehicleOptions vehicleOptions = await GetOptionsByVehicleId(vehicleId);
                vehicleOptions.Active = false;
                vehicleOptions.Deleted = true;

                var vehicleOptionsEntry = _dbContext.Entry(vehicleOptions);
                vehicleOptionsEntry.State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<bool> ActivateVehicleOptions(int vehicleId)
        {
            bool isVehicleOptionsDead = await _dbContext.VehicleOptions.AnyAsync(v => v.VehicleId == vehicleId && !v.Active && v.Deleted);

            if (isVehicleOptionsDead)
            {
                VehicleOptions vehicleOptions = await GetOptionsByVehicleId(vehicleId);
                vehicleOptions.Active = true;
                vehicleOptions.Deleted = false;

                var vehicleEntry = _dbContext.Entry(vehicleOptions);
                vehicleEntry.State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteVehicleOptions(int vehicleId)
        {
            VehicleOptions vehicleOptions = await _dbContext.VehicleOptions.FirstOrDefaultAsync(v => v.Id == vehicleId);

            if (vehicleOptions != null)
            {
                _dbContext.VehicleOptions.Remove(vehicleOptions);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

    }
}
