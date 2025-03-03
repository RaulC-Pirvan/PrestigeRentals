using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public VehicleOptionsService(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
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

            return await _dbContext.VehicleOptions.FirstOrDefaultAsync(vo => vo.VehicleId == vehicleId && vo.Active && !vo.Deleted);
        }
        
        public async Task<VehicleOptions> UpdateVehicleOptions(int vehicleId, VehicleOptionsRequest vehicleOptionsRequest)
        {
            VehicleOptions existingOptions = await _dbContext.VehicleOptions.FirstOrDefaultAsync(vo => vo.Id == vehicleId);

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

        private async Task<bool> IsVehicleOptionsExists(int vehicleId)
        {
            bool isVehicleOptionsExists = await _dbContext.VehicleOptions.AnyAsync(vo => vo.VehicleId == vehicleId && vo.Active && !vo.Deleted);

            return isVehicleOptionsExists;
        }

        public async Task<bool> DeleteVehicleOptions(int vehicleId)
        {
            bool isVehicleOptionsExists = await IsVehicleOptionsExists(vehicleId);

            if(isVehicleOptionsExists)
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

    }
}
