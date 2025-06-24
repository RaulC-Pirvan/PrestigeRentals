using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Exceptions;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Presentation.Controllers
{
    [Route("api/vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IVehicleFilterService _filterService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailService _emailService;

        public VehicleController(IVehicleService vehicleService, IVehicleFilterService filterService, IWebHostEnvironment webHostEnvironment, IOrderRepository orderRepository, IEmailService emailService)
        {
            _vehicleService = vehicleService;
            _filterService = filterService;
            _webHostEnvironment = webHostEnvironment;
            _orderRepository = orderRepository;
            _emailService = emailService;
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
                await _emailService.SendVehicleChangeNotificationToAdminAsync(vehicleDto.Id, "added");
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
                {
                    // Ștergere folder imagini vehicul
                    var vehicleFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "vehicle", vehicleId.ToString());

                    if (Directory.Exists(vehicleFolderPath))
                    {
                        try
                        {
                            Directory.Delete(vehicleFolderPath, recursive: true);
                            await _emailService.SendVehicleChangeNotificationToAdminAsync(vehicleId, "deleted");
                        }
                        catch (Exception folderEx)
                        {
                            // Folderul n-a putut fi șters, dar vehiculul a fost șters din DB
                            Console.WriteLine($"Eroare la ștergerea folderului: {folderEx.Message}");
                        }
                    }

                    return Ok(new { message = "Vehicle deleted successfully." });
                }

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
                await _emailService.SendVehicleChangeNotificationToAdminAsync(vehicleId, "updated");
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

        /// <summary>
        /// Retrieves the list of vehicles with their availability status.
        /// </summary>
        /// <param name="onlyActive">Optional flag to filter only active vehicles. Default is false.</param>
        /// <returns>A list of vehicles including their availability info.</returns>
        [HttpGet("availability")]
        public async Task<IActionResult> GetVehiclesWithAvailability([FromQuery] bool? onlyActive = false)
        {
            var now = DateTime.UtcNow; // Or DateTime.Now depending on your use case
            var vehicles = await _vehicleService.GetVehiclesWithAvailability(now, onlyActive);
            return Ok(vehicles);
        }

        /// <summary>
        /// Retrieves available filter options for vehicles (makes, models, fuel types, etc.).
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A <see cref="VehicleFilterOptionsDto"/> containing filter option lists.</returns>
        [HttpGet("filter-options")]
        public async Task<ActionResult<VehicleFilterOptionsDto>> GetFilterOptions(CancellationToken cancellationToken)
        {
            var result = await _filterService.GetFilterOptionsAsync(cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves IDs of vehicles similar to a specified vehicle, based on chassis and transmission.
        /// </summary>
        /// <param name="excludeId">The vehicle ID to exclude from results.</param>
        /// <param name="chassis">The chassis type to match.</param>
        /// <param name="transmission">The transmission type to match.</param>
        /// <returns>A list of similar vehicle IDs.</returns>
        [HttpGet("similar")]
        public async Task<IActionResult> GetSimilarVehicleIds([FromQuery] long excludeId, [FromQuery] string chassis, [FromQuery] string transmission)
        {
            var ids = await _vehicleService.GetSimilarVehicleIdsAsync(excludeId, chassis, transmission);
            return Ok(ids);
        }

        /// <summary>
        /// Retrieves date ranges for bookings of a specified vehicle.
        /// </summary>
        /// <param name="vehicleId">The vehicle ID for which to get booking date ranges.</param>
        /// <returns>A list of start and end dates for active bookings.</returns>
        [HttpGet("{vehicleId}/booked-dates")]
        public async Task<IActionResult> GetBookedDateRanges(int vehicleId)
        {
            var bookings = await _orderRepository.GetActiveOrdersByVehicle(vehicleId);

            var ranges = bookings.Where(o => !o.IsCancelled).Select(o => new
            {
                StartDate = o.StartTime.Date,
                EndDate = o.EndTime.Date
            });

            return Ok(ranges);
        }
    }
}
