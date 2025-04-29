using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Exceptions;
using PrestigeRentals.Application.Requests;
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

        /// <summary>
        /// Retrieves all vehicles, optionally filtering by active status.
        /// </summary>
        /// <param name="onlyActive">Filter vehicles by their active status. Defaults to false.</param>
        /// <returns>A list of vehicles.</returns>
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

        /// <summary>
        /// Retrieves a specific vehicle by its ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to retrieve.</param>
        /// <returns>The vehicle with the specified ID.</returns>
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

        /// <summary>
        /// Retrieves options for a specific vehicle by its ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to retrieve options for.</param>
        /// <returns>The available options for the specified vehicle.</returns>
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

        /// <summary>
        /// Adds a new vehicle to the system.
        /// </summary>
        /// <param name="vehicleRequest">The request body containing the vehicle details.</param>
        /// <returns>The details of the added vehicle.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("")]
        public async Task<ActionResult<VehicleDTO>> AddVehicle([FromBody, Required] VehicleRequest vehicleRequest)
        {
            try
            {
                var vehicleDto = await _vehicleService.AddVehicle(vehicleRequest);
                return Ok(vehicleDto); // returns 200 OK with the DTO
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// Deactivates a vehicle by its ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to deactivate.</param>
        /// <returns>A message indicating whether the vehicle was successfully deactivated.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{vehicleId}/set-inactive")]
        public async Task<ActionResult> DeactivateVehicle(int vehicleId)
        {
            try
            {
                bool isVehicleDeactivated = await _vehicleService.DeactivateVehicle(vehicleId);
                if (isVehicleDeactivated)
                    return Ok("Vehicle deactivated successfully.");
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

        /// <summary>
        /// Activates a vehicle by its ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to activate.</param>
        /// <returns>A message indicating whether the vehicle was successfully activated.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{vehicleId}/set-active")]
        public async Task<ActionResult> ActivateVehicle(int vehicleId)
        {
            try
            {
                bool isVehicleActivated = await _vehicleService.ActivateVehicle(vehicleId);
                if (isVehicleActivated)
                    return Ok("Vehicle activated successfully.");
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

        /// <summary>
        /// Deletes a vehicle by its ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to delete.</param>
        /// <returns>A message indicating whether the vehicle was successfully deleted.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{vehicleId}")]
        public async Task<ActionResult> DeleteVehicle(int vehicleId)
        {
            try
            {
                bool isVehicleDeleted = await _vehicleService.DeleteVehicle(vehicleId);
                if (isVehicleDeleted)
                    return Ok("Vehicle deleted successfully.");
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

        /// <summary>
        /// Updates a vehicle's details.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to update.</param>
        /// <param name="vehicleUpdateRequest">The updated vehicle details.</param>
        /// <returns>The updated vehicle details.</returns>
        [Authorize(Roles = "Admin")]
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
