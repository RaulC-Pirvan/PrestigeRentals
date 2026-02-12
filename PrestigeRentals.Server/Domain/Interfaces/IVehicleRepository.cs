using System.Collections.Generic;
using System.Threading.Tasks;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Interfaces.Repositories
{
    /// <summary>
    /// Defines the contract for interacting with the vehicle repository.
    /// </summary>
    public interface IVehicleRepository
    {
        /// <summary>
        /// Asynchronously adds a new vehicle to the repository.
        /// </summary>
        /// <param name="vehicle">The vehicle to be added to the repository.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(Vehicle vehicle);

        /// <summary>
        /// Asynchronously retrieves a vehicle by its unique identifier.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to retrieve.</param>
        /// <returns>The <see cref="Vehicle"/> entity if found; otherwise, null.</returns>
        Task<Vehicle> GetVehicleById(long vehicleId);

        /// <summary>
        /// Asynchronously retrieves all vehicles, optionally filtering by active status.
        /// </summary>
        /// <param name="onlyActive">If true, only active vehicles are returned; otherwise, all vehicles.</param>
        /// <returns>A list of vehicles.</returns>
        Task<List<Vehicle>> GetAllVehiclesAsync(bool? onlyActive = false);

        /// <summary>
        /// Asynchronously updates an existing vehicle in the repository.
        /// </summary>
        /// <param name="vehicle">The vehicle entity with updated data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(Vehicle vehicle);

        /// <summary>
        /// Asynchronously deletes a vehicle from the repository.
        /// </summary>
        /// <param name="vehicle">The vehicle entity to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(Vehicle vehicle);

        /// <summary>
        /// Retrieves vehicle options associated with the specified vehicle ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <returns>The <see cref="VehicleOptions"/> entity if found; otherwise, null.</returns>
        Task<VehicleOptions?> GetVehicleOptionsByVehicleId(long vehicleId);

        /// <summary>
        /// Checks whether a vehicle with the specified ID is active.
        /// </summary>
        /// <param name="vehicleId">The vehicle ID to check.</param>
        /// <returns>True if the vehicle is active; otherwise, false.</returns>
        Task<bool> IsAliveAsync(long vehicleId);

        /// <summary>
        /// Checks whether a vehicle with the specified ID is inactive or deleted.
        /// </summary>
        /// <param name="vehicleId">The vehicle ID to check.</param>
        /// <returns>True if the vehicle is inactive or deleted; otherwise, false.</returns>
        Task<bool> IsDeadAsync(long vehicleId);

        /// <summary>
        /// Retrieves IDs of vehicles similar to a given vehicle, excluding a specified vehicle ID,
        /// based on chassis and transmission type.
        /// </summary>
        /// <param name="excludeId">The ID of the vehicle to exclude from the results.</param>
        /// <param name="chassis">The chassis type to match.</param>
        /// <param name="transmission">The transmission type to match.</param>
        /// <returns>A list of vehicle IDs that match the criteria.</returns>
        Task<List<long>> GetSimilarVehicleIdsAsync(long excludeId, string chassis, string transmission);
    }
}
