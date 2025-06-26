using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.Exceptions;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Presentation.Controllers
{
    [Route("api/vehicle")]
    [ApiController]
    public class VehiclePhotosController : ControllerBase
    {
        private readonly IVehiclePhotosService _vehiclePhotosService;

        public VehiclePhotosController(IVehiclePhotosService vehiclePhotosService)
        {
            _vehiclePhotosService = vehiclePhotosService;
        }

        /// <summary>
        /// Retrieves photos of a specific vehicle by its ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to retrieve photos for.</param>
        /// <returns>A list of vehicle photos in base64 format.</returns>
        [HttpGet("{vehicleId}/photos")]
        public async Task<IActionResult> GetVehiclePhotos(int vehicleId)
        {
            try
            {
                var result = await _vehiclePhotosService.GetVehiclePhotosAsBase64(vehicleId);
                return Ok(result.Value);  // Assuming result.Value is the actual photo data.
            }
            catch (VehiclePhotoNotFoundException ex)
            {
                return NotFound(ex.Message);  // If no photos found, return 404.
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);  // Return a generic 500 error for unexpected issues.
            }
        }

        /// <summary>
        /// Uploads a photo for a specific vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to upload a photo for.</param>
        /// <param name="request">The request body containing the base64-encoded image data.</param>
        /// <returns>A status indicating success or failure of the upload operation.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadPhoto([FromQuery] int vehicleId, [FromBody] VehiclePhotoRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.ImageData))
                {
                    return BadRequest("Invalid request. Image data is required.");  // Ensures the image data is not empty or null.
                }

                var result = await _vehiclePhotosService.UploadVehiclePhoto(vehicleId, request.ImageData);

                if (result.Result is BadRequestObjectResult badRequest)
                {
                    // Return the result of the BadRequest if the upload fails
                    return BadRequest(badRequest.Value);  // Return specific error message if upload failed.
                }

                return Ok(result.Value);  // Assuming result.Value holds the uploaded photo information.
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);  // Catch unexpected errors
            }
        }
    }
}
