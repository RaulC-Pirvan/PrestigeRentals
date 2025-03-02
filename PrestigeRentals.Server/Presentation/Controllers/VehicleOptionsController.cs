using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Presentation.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VehicleOptionsController : ControllerBase
    {
        private readonly IVehicleOptionsService _vehicleOptionsService;

        public VehicleOptionsController(IVehicleOptionsService vehicleOptionsService)
        {
            _vehicleOptionsService = vehicleOptionsService;
        }

        [HttpPost("{vehicleId}")]
        public async Task<IActionResult> CreateVehicleOptions(int vehicleId, [FromBody] VehicleOptionsRequest vehicleOptionsRequest)
        {
            try
            {
                var addVehicleOptionsOperation = await _vehicleOptionsService.CreateVehicleOptions(vehicleId, vehicleOptionsRequest);
                if (addVehicleOptionsOperation == null)
                {
                    return BadRequest("Error");
                }
                return Ok(vehicleOptionsRequest);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{vehicleId}")]
        public async Task<IActionResult> GetVehicleOptions(int vehicleId)
        {
            try
            {
                VehicleOptions vehicleOptions = await _vehicleOptionsService.GetOptionsByVehicleId(vehicleId);
                if (vehicleOptions == null)
                    return NotFound("Vehicle not found.");
                return Ok(vehicleOptions);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("{vehicleId}")]
        public async Task<IActionResult> UpdateVehicleOptions(int vehicleId, [FromBody] VehicleOptionsRequest vehicleOptionsRequest)
        {
            var updatedOptions = await _vehicleOptionsService.UpdateVehicleOptions(vehicleId, vehicleOptionsRequest);

            if (updatedOptions == null)
                return NotFound("Vehicle options not found.");

            return Ok(updatedOptions);
        }

        [HttpDelete("{vehicleId}")]
        public async Task<IActionResult> DeleteVehicleOptions(int vehicleId)
        {
            var success = await _vehicleOptionsService.DeleteVehicleOptions(vehicleId);

            if (!success)
                return NotFound("Vehicle options not found.");

            return Ok(success);
        }
    }
}
