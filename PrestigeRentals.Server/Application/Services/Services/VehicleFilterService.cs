using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services.Services
{
    /// <summary>
    /// Provides vehicle filtering options based on distinct values found in the database.
    /// Implements <see cref="IVehicleFilterService"/>.
    /// </summary>
    public class VehicleFilterService : IVehicleFilterService
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleFilterService"/> class.
        /// </summary>
        /// <param name="dbContext">The application's database context.</param>
        public VehicleFilterService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves distinct filter values from the vehicle table, such as makes, models,
        /// fuel types, transmissions, and chassis types.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token for the asynchronous operation.</param>
        /// <returns>A <see cref="VehicleFilterOptionsDto"/> containing lists of unique filter values.</returns>
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
