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
        /// Asynchronously gets a vehicle based on id.
        /// </summary>
        /// <returns></returns>
        Task<Vehicle> GetVehicleById(long vehicleId);
        Task<List<Vehicle>> GetAllVehiclesAsync(bool? onlyActive = false);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(Vehicle vehicle);
        Task<VehicleOptions?> GetVehicleOptionsByVehicleId(long vehicleId);
        Task<bool> IsAliveAsync(long vehicleId);
        Task<bool> IsDeadAsync(long vehicleId);
    }
}
