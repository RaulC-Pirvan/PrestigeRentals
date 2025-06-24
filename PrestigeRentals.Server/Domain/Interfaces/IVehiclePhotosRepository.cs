using System.Collections.Generic;
using System.Threading.Tasks;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for managing vehicle photo entities in the data store.
    /// </summary>
    public interface IVehiclePhotosRepository
    {
        /// <summary>
        /// Retrieves a vehicle photo by its unique identifier.
        /// </summary>
        /// <param name="photoId">The ID of the photo to retrieve.</param>
        /// <returns>The <see cref="VehiclePhotos"/> entity if found; otherwise, null.</returns>
        Task<VehiclePhotos> GetPhotoByIdAsync(long photoId);

        /// <summary>
        /// Retrieves all photos associated with a specific vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <returns>A list of <see cref="VehiclePhotos"/> entities associated with the vehicle.</returns>
        Task<List<VehiclePhotos>> GetPhotosByVehicleId(long vehicleId);

        /// <summary>
        /// Adds a new vehicle photo entity to the data store.
        /// </summary>
        /// <param name="vehiclePhoto">The vehicle photo entity to add.</param>
        /// <returns>The added <see cref="VehiclePhotos"/> entity.</returns>
        Task<VehiclePhotos> AddAsync(VehiclePhotos vehiclePhoto);

        /// <summary>
        /// Updates an existing vehicle photo entity.
        /// </summary>
        /// <param name="vehiclePhoto">The vehicle photo entity with updated data.</param>
        Task UpdateAsync(VehiclePhotos vehiclePhoto);

        /// <summary>
        /// Deletes a vehicle photo entity by its unique identifier.
        /// </summary>
        /// <param name="photoId">The ID of the photo to delete.</param>
        Task DeleteAsync(long photoId);
    }
}
