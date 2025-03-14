using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services
{
    public class VehicleOptionsRepository : IVehicleOptionsRepository
    {
        ApplicationDbContext _dbContext;

        public VehicleOptionsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddVehicleOptions(VehicleOptions vehicleOptions)
        {
            await _dbContext.VehicleOptions.AddAsync(vehicleOptions);
            await _dbContext.SaveChangesAsync();
        }
    }
}
