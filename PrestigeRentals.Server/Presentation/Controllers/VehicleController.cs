using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Exceptions;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Presentation.Controllers
{
    [Route("api/vehicle")]
    [ApiController]

    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;


        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllVehicles(bool? onlyActive = false)
        {
            try
            {
                List<Vehicle> vehicles = await _vehicleService.GetAllVehicles(onlyActive);

                return Ok(vehicles);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{vehicleId}")]
        public async Task<ActionResult<Vehicle>> GetVehicleByID(int vehicleId)
        {
            try
            {
                Vehicle vehicle = await _vehicleService.GetVehicleByID(vehicleId);
                return Ok(vehicle);
            }

            catch (VehicleNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{vehicleId}/options")]
        public async Task<ActionResult<VehicleOptions>> GetVehicleOptions(int vehicleId)
        {
            try
            {
                VehicleOptions vehicleOptions = await _vehicleService.GetVehicleOptions(vehicleId);
                return Ok(vehicleOptions);
            }

            catch (VehicleOptionsNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AddVehicle([FromBody, Required] VehicleRequest vehicleRequest)
        {
            try
            {
                ActionResult? addVehicleOperation = await _vehicleService.AddVehicle(vehicleRequest);

                if (addVehicleOperation != null)
                {
                    return BadRequest("Error");
                }
                return Ok(vehicleRequest);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPatch("{vehicleId}/set-inactive")]
        public async Task<ActionResult> DeactivateVehicle(int vehicleId)
        {
            try
            {
                bool isVehicleDeactivated = await _vehicleService.DeactivateVehicle(vehicleId);
                return BadRequest("Error: Vehicle could not be deactivated.");
            }

            catch (VehicleAlreadyDeactivatedException ex)
            {
                return Conflict(ex.Message);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPatch("{vehicleId}/set-active")]
        public async Task<ActionResult> ActivateVehicle(int vehicleId)
        {
            try
            {
                bool isVehicleActivated = await _vehicleService.ActivateVehicle(vehicleId);
                return BadRequest("Error: Vehicle could not be activated.");
            }

            catch (VehicleAlreadyActiveException ex)
            {
                return Conflict(ex.Message);
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{vehicleId}")]
        public async Task<ActionResult> DeleteVehicle(int vehicleId)
        {
            try
            {
                bool isVehicleDeleted = await _vehicleService.DeleteVehicle(vehicleId);
                return BadRequest("Error: Vehicle could not be deleted.");
            }

            catch (VehicleNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPatch("{vehicleId}")]
        public async Task<ActionResult<VehicleDTO>> UpdateVehicle(int vehicleId, [FromBody] VehicleUpdateRequest vehicleUpdateRequest)
        {
            try
            {
                VehicleDTO vehicleDTO = await _vehicleService.UpdateVehicle(vehicleId, vehicleUpdateRequest);
                return Ok(vehicleDTO);
            }

            catch (VehicleUpdateException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
