using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<List<VehiclePhotos>>> GetVehiclePhotos(int vehicleId)
        {
            try
            {
                var vehiclePhotos = await _vehiclePhotosService.GetVehiclePhotos(vehicleId);

                if(vehiclePhotos == null)
                    return NotFound($"No photos found for vehicle ID {vehicleId}.");
                return Ok(vehiclePhotos);
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
