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

        public async Task<ActionResult<List<String>>> GetVehiclePhotosAsBase64(int vehicleId)
        {
          List<VehiclePhotos>? vehiclePhotos = await _dbContext.VehiclePhotos.Where(v => v.VehicleId == vehicleId).ToListAsync();

            if (vehiclePhotos == null || !vehiclePhotos.Any()) 
            {
                _logger.LogWarning($"No photos found for vehicle ID {vehicleId}");
                return new NotFoundResult();
            }

            List<string>? base64Images = vehiclePhotos.Select(photo => $"data:image/jpeg;base64,{Convert.ToBase64String(photo.ImageData)}").ToList();

            return base64Images;
        }

        public async Task<ActionResult<VehiclePhotos>> UploadVehiclePhoto(int vehicleId, string base64Image)
        {
            if(string.IsNullOrWhiteSpace(base64Image))
            {
                _logger.LogWarning("Base64 image string is empty.");
                return new BadRequestObjectResult("Invalid image data.");
            }

            try
            {
                VehiclePhotos vehiclePhoto = new VehiclePhotos
                {
                    VehicleId = vehicleId,
                    ImageData = Convert.FromBase64String(base64Image)
                };

                _dbContext.VehiclePhotos.Add(vehiclePhoto);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Photo uploaded for vehicle ID {vehicleId}");

                return vehiclePhoto;
            }

            catch(Exception ex)
            {
                _logger.LogError($"Error uploading photo: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
