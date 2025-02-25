using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Services;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;


        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService; 
        }

        [HttpGet]
        [Route("Vehicles")]
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

        [HttpPost]
        [Route("Vehicle")]
        public async Task<IActionResult> AddVehicle([FromBody, Required] CreateVehicleDTO createVehicleDTO)
        {
            try
            {
                var addVehicleOperation = await _vehicleService.AddVehicle(createVehicleDTO);
                if (addVehicleOperation != null)
                {
                    return BadRequest("Error");
                }
                return Ok(createVehicleDTO);
            }

            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
