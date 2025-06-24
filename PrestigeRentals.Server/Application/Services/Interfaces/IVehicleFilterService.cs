using System.Threading;
using System.Threading.Tasks;
using PrestigeRentals.Application.DTO;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Defines operations for retrieving dynamic filter options for vehicles,
    /// such as available makes, models, fuel types, and transmissions.
    /// </summary>
    public interface IVehicleFilterService
    {
        /// <summary>
        /// Retrieves available filter options for vehicles (e.g., makes, models, fuel types).
        /// This is typically used to populate search filters in the frontend.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
        /// <returns>A <see cref="VehicleFilterOptionsDto"/> containing lists of available filter values.</returns>
        Task<VehicleFilterOptionsDto> GetFilterOptionsAsync(CancellationToken cancellationToken);
    }
}
