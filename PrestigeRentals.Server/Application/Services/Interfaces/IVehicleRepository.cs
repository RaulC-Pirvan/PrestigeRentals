using System.Threading.Tasks;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Interfaces
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
        Task AddVehicle(Vehicle vehicle);

        /// <summary>
        /// Asynchronously gets a vehicle based on id.
        /// </summary>
        /// <returns></returns>
        Task<Vehicle> GetVehicleById(long vehicleId);
        Task UpdateAsync(Vehicle vehicle);
    }
}
