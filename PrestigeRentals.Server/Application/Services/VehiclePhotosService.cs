using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services
{
    public class VehiclePhotosService : IVehiclePhotosService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<VehiclePhotosService> _logger;

        public VehiclePhotosService(ApplicationDbContext dbContext, ILogger<VehiclePhotosService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ActionResult<List<VehiclePhotos>>> GetVehiclePhotos(int vehicleId)
        {
            List<VehiclePhotos> vehiclePhotos = await _dbContext.VehiclePhotos.Where(v => v.VehicleId == vehicleId).ToListAsync();

            if (vehiclePhotos == null || !vehiclePhotos.Any())
            {
                _logger.LogWarning($"No photos were found for vehicle ID {vehicleId}.");
                return null;
            }

            _logger.LogInformation($"Successfully retrieved photos for vehicle ID {vehicleId}");
            return vehiclePhotos;
        }
    }
}
