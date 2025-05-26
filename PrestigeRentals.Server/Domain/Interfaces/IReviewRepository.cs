using PrestigeRentals.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> GetByIdAsync(long id);

        Task<IEnumerable<Review>> GetAllAsync();

        Task AddAsync(Review review);

        Task UpdateAsync(Review review);

        Task DeleteAsync(Review review);

        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(long userId);
        Task<IEnumerable<Review>> GetReviewsByVehicleIdAsync(long vehicleId);
    }
}
