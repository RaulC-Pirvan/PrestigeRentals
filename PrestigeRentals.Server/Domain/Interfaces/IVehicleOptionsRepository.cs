using System.Threading.Tasks;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Interfaces.Repositories
{
    /// <summary>
    /// Defines the contract for interacting with the vehicle options repository.
    /// </summary>
    public interface IVehicleOptionsRepository
    {
        /// <summary>
        /// Asynchronously adds vehicle options to the repository.
        /// </summary>
        /// <param name="vehicleOptions">The vehicle options to be added to the repository.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddVehicleOptions(VehicleOptions vehicleOptions);

        /// <summary>
        /// Retrieves vehicle options by the vehicle's unique identifier.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle whose options are requested.</param>
        /// <returns>The <see cref="VehicleOptions"/> associated with the specified vehicle.</returns>
        Task<VehicleOptions> GetVehicleOptionsById(long vehicleId);

        /// <summary>
        /// Updates existing vehicle options in the repository.
        /// </summary>
        /// <param name="vehicleOptions">The vehicle options entity with updated data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(VehicleOptions vehicleOptions);

        /// <summary>
        /// Deletes vehicle options from the repository.
        /// </summary>
        /// <param name="vehicleOptions">The vehicle options entity to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(VehicleOptions vehicleOptions);

        /// <summary>
        /// Determines whether the vehicle options for the specified vehicle ID are active.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <returns>True if the vehicle options are active; otherwise, false.</returns>
        Task<bool> IsAliveAsync(long vehicleId);

        /// <summary>
        /// Determines whether the vehicle options for the specified vehicle ID are inactive or deleted.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <returns>True if the vehicle options are inactive or deleted; otherwise, false.</returns>
        Task<bool> IsDeadAsync(long vehicleId);
    }
}
