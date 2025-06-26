using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Domain.Interfaces;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services.Services
{
    /// <summary>
    /// Service class for handling vehicle photo uploads and retrieval.
    /// </summary>
    public class VehiclePhotosService : IVehiclePhotosService
    {
        private readonly ILogger<VehiclePhotosService> _logger;
        private readonly IVehiclePhotosRepository _vehiclePhotosRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehiclePhotosService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">Logger instance for logging service activities.</param>
        public VehiclePhotosService (ILogger<VehiclePhotosService> logger, IVehiclePhotosRepository vehiclePhotosRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
            _vehiclePhotosRepository = vehiclePhotosRepository;
        }

        /// <summary>
        /// Retrieves the vehicle photos as base64 encoded strings.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle whose photos are being requested.</param>
        /// <returns>A list of base64 encoded strings representing the vehicle's photos, or a <see cref="NotFoundResult"/> if no photos are found.</returns>
        public async Task<ActionResult<List<string>>> GetVehiclePhotosAsBase64(long vehicleId)
        {
            try
            {
                // Retrieve vehicle photos from the database
                var vehiclePhotos = await _vehiclePhotosRepository.GetPhotosByVehicleId(vehicleId);

                if (vehiclePhotos == null || !vehiclePhotos.Any())
                {
                    _logger.LogWarning($"No photos found for vehicle ID {vehicleId}");
                    return new NotFoundResult();
                }

                // Convert image data to base64 format
                List<string> base64Images = vehiclePhotos.Select(photo => $"data:image/jpeg;base64,{Convert.ToBase64String(photo.ImageData)}").ToList();

                _logger.LogInformation($"Successfully retrieved {vehiclePhotos.Count} photo(s) for vehicle ID {vehicleId}");
                return base64Images;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving photos for vehicle ID {vehicleId}: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Uploads a new photo for a vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to which the photo is being uploaded.</param>
        /// <param name="base64Image">The base64 encoded image string representing the photo.</param>
        /// <returns>The <see cref="VehiclePhotos"/> entity if the upload is successful, or a <see cref="BadRequestObjectResult"/> if the image data is invalid.</returns>
        public async Task<ActionResult<VehiclePhotos>> UploadVehiclePhoto(long vehicleId, string base64Image)
        {
            // Validate base64 image string
            if (string.IsNullOrWhiteSpace(base64Image))
            {
                _logger.LogWarning("Base64 image string is empty.");
                return new BadRequestObjectResult("Invalid image data.");
            }

            try
            {
                // Create a new VehiclePhotos entity and save it to the database
                VehiclePhotos vehiclePhoto = new VehiclePhotos
                {
                    VehicleId = vehicleId,
                    ImageData = Convert.FromBase64String(base64Image),
                    Active = true,
                    Deleted = false
                };

                var result = await _vehiclePhotosRepository.AddAsync(vehiclePhoto);

                _logger.LogInformation($"Photo uploaded for vehicle ID {vehicleId}");
                return result;
            }
            catch (FormatException)
            {
                _logger.LogWarning("Failed to convert image from base64.");
                return new BadRequestObjectResult("Image data is not in a valid base64 format.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading photo for vehicle ID {vehicleId}: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
