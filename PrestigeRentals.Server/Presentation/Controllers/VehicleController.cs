using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.DTO;
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
        public async Task<IActionResult> GetAllVehicles()
        {
            try
            {
                List<Vehicle> vehicles = await _vehicleService.GetAllVehicles();

                return Ok(vehicles);
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


        [HttpGet("{vehicleId}")]
        public async Task<ActionResult<Vehicle>> GetVehicleByID(int vehicleId)
        {
            try
            {
                Vehicle vehicle = await _vehicleService.GetVehicleByID(vehicleId);

                if (vehicle == null)
                    return NotFound("Vehicle not found.");
                return Ok(vehicle);
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

                if(isVehicleDeleted == true)
                    return Ok("Success");
                return BadRequest("Error");
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("{vehicleId}")]
        public async Task<ActionResult<VehicleDTO>> UpdateVehicle(int vehicleId, [FromBody, Required] VehicleRequest vehicleRequest)
        {
            try
            {
                VehicleDTO vehicleDTO = await _vehicleService.UpdateVehicle(vehicleId, vehicleRequest);

                if(vehicleDTO == null)
                    return BadRequest("Error");
                return Ok(vehicleDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
