using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{vehicleId}/photos")]
        public async Task<IActionResult> GetVehiclePhotos(int vehicleId)
        {
           var result = await _vehiclePhotosService.GetVehiclePhotosAsBase64(vehicleId);

            if (result.Result is NotFoundResult)
                return NotFound("No images found for this vehicle.");

            return Ok(result.Value);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPhoto([FromQuery] int vehicleId, [FromBody] VehiclePhotoRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.ImageData))
                return BadRequest("Invalid request. Image data is required.");

            ActionResult<VehiclePhotos> result = await _vehiclePhotosService.UploadVehiclePhoto(vehicleId, request.ImageData);

            if (result.Result is BadRequestObjectResult)
                return BadRequest(result.Result);

            return Ok(result.Value);
        }
    }
}
