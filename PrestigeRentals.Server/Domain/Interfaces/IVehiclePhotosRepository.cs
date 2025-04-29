using PrestigeRentals.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Domain.Interfaces
{
    public interface IVehiclePhotosRepository
    {
        Task<VehiclePhotos> GetPhotoByIdAsync(long photoId);
        Task<List<VehiclePhotos>> GetPhotosByVehicleId(long vehicleId);
        Task<VehiclePhotos> AddAsync(VehiclePhotos vehiclePhoto);
        Task UpdateAsync(VehiclePhotos vehiclePhoto);
        Task DeleteAsync(long photoId);
    }
}
