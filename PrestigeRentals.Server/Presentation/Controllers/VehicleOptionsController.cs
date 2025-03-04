using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Presentation.Controllers
{

    [Route("api/vehicle")]
    [ApiController]
    public class VehicleOptionsController : ControllerBase
    {
        private readonly IVehicleOptionsService _vehicleOptionsService;

        public VehicleOptionsController(IVehicleOptionsService vehicleOptionsService)
        {
            _vehicleOptionsService = vehicleOptionsService;
        }

        [HttpGet("{vehicleId}/options")]
        public async Task<IActionResult> GetVehicleOptions(int vehicleId)
        {
            try
            {
                VehicleOptions vehicleOptions = await _vehicleOptionsService.GetOptionsByVehicleId(vehicleId);

                if (vehicleOptions == null)
                    return NotFound("Vehicle options not found.");
                return Ok(vehicleOptions);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("{vehicleId}/options")]
        public async Task<IActionResult> AddVehicleOptions(int vehicleId, [FromBody] VehicleOptionsRequest vehicleOptionsRequest)
        {
            try
            {
                VehicleOptions? addVehicleOptionsOperation = await _vehicleOptionsService.AddVehicleOptions(vehicleId, vehicleOptionsRequest);

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

        [HttpPatch("{vehicleId}/options/set-inactive")]
        public async Task<ActionResult> DeactivateVehicleOptions(int vehicleId)
        {
            try
            {
                bool isVehicleOptionsDeactivated = await _vehicleOptionsService.DeactivateVehicleOptions(vehicleId);

                if (isVehicleOptionsDeactivated)
                    return Ok("Vehicle options deactivated successfully.");
                return BadRequest("Error: Vehicle options could not be deactivated.");
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }

        [HttpPatch("{vehicleId}/options/set-active")]
        public async Task<ActionResult> ActivateVehicleOptions(int vehicleId)
        {
            try
            {
                bool isVehicleOptionsActivated = await _vehicleOptionsService.ActivateVehicleOptions(vehicleId);

                if (isVehicleOptionsActivated)
                    return Ok("Vehicle options activated successfully.");
                return BadRequest("Error: Vehicle options could not be found.");
            }

            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{vehicleId}/options")]
        public async Task<ActionResult> DeleteVehicleOptions(int vehicleId)
        {
            try
            {
                bool isVehicleOptionsDeleted = await _vehicleOptionsService.DeleteVehicleOptions(vehicleId);

                if (isVehicleOptionsDeleted)
                    return Ok("Vehicle options deleted successfully.");
                return BadRequest("Error: Vehicle could not be deleted");
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("{vehicleId}/options")]
        public async Task<IActionResult> UpdateVehicleOptions(int vehicleId, [FromBody] VehicleOptionsRequest vehicleOptionsRequest)
        {
            VehicleOptions? updatedOptions = await _vehicleOptionsService.UpdateVehicleOptions(vehicleId, vehicleOptionsRequest);

            if (updatedOptions == null)
                return NotFound("Vehicle options not found.");

            return Ok(updatedOptions);
        }


    }
}
