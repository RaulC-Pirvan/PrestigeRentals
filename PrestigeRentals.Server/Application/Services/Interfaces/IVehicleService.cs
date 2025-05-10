using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for vehicle-related services, including adding, retrieving, updating, and deleting vehicles.
    /// </summary>
    public interface IVehicleService
    {
        /// <summary>
        /// Asynchronously retrieves all vehicles, optionally filtered by active status.
        /// </summary>
        /// <param name="onlyActive">If true, only active vehicles are returned; otherwise, all vehicles are retrieved. Default is false.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of vehicles.</returns>
        Task<List<Vehicle>> GetAllVehicles(bool? onlyActive = false);

        /// <summary>
        /// Asynchronously adds a new vehicle.
        /// </summary>
        /// <param name="vehicleRequest">The request object containing the details of the vehicle to be added.</param>
        /// <returns>A task representing the asynchronous operation, with an action result indicating success or failure.</returns>
        Task<VehicleDTO> AddVehicle(VehicleRequest vehicleRequest);

        /// <summary>
        /// Asynchronously retrieves a vehicle by its ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to be retrieved.</param>
        /// <returns>A task representing the asynchronous operation, containing the vehicle data if found, otherwise null.</returns>
        Task<Vehicle> GetVehicleByID(long vehicleId);

        /// <summary>
        /// Asynchronously retrieves the options associated with a vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle whose options are to be retrieved.</param>
        /// <returns>A task representing the asynchronous operation, containing the vehicle options if found, otherwise null.</returns>
        Task<VehicleOptions> GetVehicleOptions(long vehicleId);

        /// <summary>
        /// Asynchronously deactivates a vehicle by its ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to be deactivated.</param>
        /// <returns>A task representing the asynchronous operation, indicating whether the operation was successful.</returns>
        Task<bool> DeactivateVehicle(long vehicleId);

        /// <summary>
        /// Asynchronously activates a vehicle by its ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to be activated.</param>
        /// <returns>A task representing the asynchronous operation, indicating whether the operation was successful.</returns>
        Task<bool> ActivateVehicle(long vehicleId);

        /// <summary>
        /// Asynchronously deletes a vehicle by its ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to be deleted.</param>
        /// <returns>A task representing the asynchronous operation, indicating whether the operation was successful.</returns>
        Task<bool> DeleteVehicle(long vehicleId);

        /// <summary>
        /// Asynchronously updates a vehicle's details by its ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to be updated.</param>
        /// <param name="vehicleUpdateRequest">The request object containing the updated vehicle details.</param>
        /// <returns>A task representing the asynchronous operation, containing the updated vehicle DTO.</returns>
        Task<VehicleDTO> UpdateVehicle(long vehicleId, VehicleUpdateRequest vehicleUpdateRequest);

        Task<List<VehicleAvailabilityDTO>> GetVehiclesWithAvailability(DateTime now, bool? onlyActive);
    }
}
