using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Domain.Interfaces;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing vehicle photo entities in the database.
    /// Implements <see cref="IVehiclePhotosRepository"/>.
    /// </summary>
    public class VehiclePhotosRepository : IVehiclePhotosRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehiclePhotosRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context used for data access.</param>
        public VehiclePhotosRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves a vehicle photo by its unique identifier, excluding deleted photos.
        /// </summary>
        /// <param name="photoId">The ID of the photo to retrieve.</param>
        /// <returns>The <see cref="VehiclePhotos"/> entity if found and not deleted; otherwise, null.</returns>
        public async Task<VehiclePhotos> GetPhotoByIdAsync(long photoId)
        {
            return await _dbContext.VehiclePhotos.FirstOrDefaultAsync(photo => photo.Id == photoId && !photo.Deleted);
        }

        /// <summary>
        /// Retrieves all non-deleted photos associated with a specific vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <returns>A list of <see cref="VehiclePhotos"/> entities.</returns>
        public async Task<List<VehiclePhotos>> GetPhotosByVehicleId(long vehicleId)
        {
            return await _dbContext.VehiclePhotos.Where(photo => photo.VehicleId == vehicleId && !photo.Deleted).ToListAsync();
        }

        /// <summary>
        /// Adds a new vehicle photo to the database.
        /// </summary>
        /// <param name="vehiclePhoto">The photo entity to add.</param>
        /// <returns>The added <see cref="VehiclePhotos"/> entity.</returns>
        public async Task<VehiclePhotos> AddAsync(VehiclePhotos vehiclePhoto)
        {
            await _dbContext.VehiclePhotos.AddAsync(vehiclePhoto);
            await _dbContext.SaveChangesAsync();

            return vehiclePhoto;
        }

        /// <summary>
        /// Updates an existing vehicle photo's data if it exists and is not deleted.
        /// </summary>
        /// <param name="vehiclePhotos">The vehicle photo entity with updated data.</param>
        public async Task UpdateAsync(VehiclePhotos vehiclePhotos)
        {
            var existingPhoto = await _dbContext.VehiclePhotos.FirstOrDefaultAsync(photo => photo.Id == vehiclePhotos.Id);

            if (existingPhoto != null && !existingPhoto.Deleted)
            {
                existingPhoto.ImageData = vehiclePhotos.ImageData;
                existingPhoto.Active = vehiclePhotos.Active;
                existingPhoto.Deleted = vehiclePhotos.Deleted;

                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Soft deletes a vehicle photo by marking it as deleted.
        /// </summary>
        /// <param name="photoId">The ID of the photo to delete.</param>
        public async Task DeleteAsync(long photoId)
        {
            var existingPhoto = await _dbContext.VehiclePhotos.FirstOrDefaultAsync(photo => photo.Id == photoId);

            if (existingPhoto != null && !existingPhoto.Deleted)
            {
                existingPhoto.Deleted = true;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
