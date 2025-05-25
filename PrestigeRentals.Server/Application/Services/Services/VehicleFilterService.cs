using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services.Services
{
    public class VehicleFilterService : IVehicleFilterService
    {
        private readonly ApplicationDbContext _dbContext;

        public VehicleFilterService(ApplicationDbContext dbContext) { _dbContext = dbContext; }

        public async Task<VehicleFilterOptionsDto> GetFilterOptionsAsync(CancellationToken cancellationToken)
        {
            return new VehicleFilterOptionsDto
            {
                Makes = await _dbContext.Vehicles.Select(v => v.Make).Distinct().ToListAsync(cancellationToken),
                Models = await _dbContext.Vehicles.Select(v => v.Model).Distinct().ToListAsync(cancellationToken),
                FuelTypes = await _dbContext.Vehicles.Select(v => v.FuelType).Distinct().ToListAsync(cancellationToken),
                Transmissions = await _dbContext.Vehicles.Select(v => v.Transmission).Distinct().ToListAsync(cancellationToken),
                Chassis = await _dbContext.Vehicles.Select(v => v.Chassis).Distinct().ToListAsync(cancellationToken)
            };
        }
    }
}
