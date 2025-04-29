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
        Task<VehicleOptions> GetVehicleOptionsById(long vehicleId);
        Task UpdateAsync(VehicleOptions vehicleOptions);
        Task DeleteAsync(VehicleOptions vehicleOptions);
        Task<bool> IsAliveAsync(long vehicleId);
        Task<bool> IsDeadAsync(long vehicleId);
    }
}