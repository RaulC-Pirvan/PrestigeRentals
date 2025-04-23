using System.Threading.Tasks;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Interfaces
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
    }
}
