using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Domain.Interfaces;
using PrestigeRentals.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Infrastructure.Repositories
{
    public class VehiclePhotosRepository : IVehiclePhotosRepository
    {
        private readonly ApplicationDbContext _dbContext;
        
        public VehiclePhotosRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<VehiclePhotos> GetPhotoByIdAsync(long photoId)
        {
            return await _dbContext.VehiclePhotos.FirstOrDefaultAsync(photo => photo.Id == photoId && !photo.Deleted);
        }

        public async Task<List<VehiclePhotos>> GetPhotosByVehicleId(long vehicleId)
        {
            return await _dbContext.VehiclePhotos.Where(photo => photo.VehicleId == vehicleId && !photo.Deleted).ToListAsync();
        }

        public async Task<VehiclePhotos> AddAsync(VehiclePhotos vehiclePhoto)
        {
            await _dbContext.VehiclePhotos.AddAsync(vehiclePhoto);
            await _dbContext.SaveChangesAsync();

            return vehiclePhoto;
        }

        public async Task UpdateAsync(VehiclePhotos vehiclePhotos)
        {
            var existingPhoto = await _dbContext.VehiclePhotos.FirstOrDefaultAsync(photo => photo.Id == vehiclePhotos.Id);

            if(existingPhoto != null && !existingPhoto.Deleted)
            {
                existingPhoto.ImageData = vehiclePhotos.ImageData;
                existingPhoto.Active = vehiclePhotos.Active;
                existingPhoto.Deleted = vehiclePhotos.Deleted;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(long photoId)
        {
            var existingPhoto = await _dbContext.VehiclePhotos.FirstOrDefaultAsync(photo => photo.Id == photoId);

            if(existingPhoto != null && !existingPhoto.Deleted)
            {
                existingPhoto.Deleted = true;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
