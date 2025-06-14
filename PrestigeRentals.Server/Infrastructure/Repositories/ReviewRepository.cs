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
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ReviewRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Review> GetByIdAsync(long id)
        {
            return await _dbContext.Reviews.FindAsync(id);
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _dbContext.Reviews.ToListAsync();
        }

        public async Task AddAsync(Review review)
        {
            if (review == null)
                throw new ArgumentNullException(nameof(review), "Review cannot be null.");

            try
            {
                _dbContext.Reviews.Add(review);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("An error occured while saving the review.", ex);
            }
        }

        public async Task UpdateAsync(Review review)
        {
            if (review == null)
                throw new ArgumentNullException(nameof(review), "Review cannot be null.");

            try
            {
                _dbContext.Reviews.Update(review);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occured while updating the review.", ex);
            }
        }

        public async Task DeleteAsync(Review review)
        {
            if (review == null)
                throw new ArgumentNullException(nameof(review), "Review cannot be null.");

            try
            {
                _dbContext.Reviews.Remove(review);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occured while updating the review.", ex);
            }
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(long userId)
        {
            return await _dbContext.Reviews.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByVehicleIdAsync(long vehicleId)
        {
            return await _dbContext.Reviews.Where(r => r.VehicleId == vehicleId).ToListAsync();
        }

        public async Task<Review?> GetReviewByOrderIdAsync(long orderId)
        {
            return await _dbContext.Reviews
                .FirstOrDefaultAsync(r => r.OrderId == orderId);
        }

        public async Task<bool> ExistsByOrderIdAsync(long orderId)
        {
            return await _dbContext.Reviews.AnyAsync(r => r.OrderId == orderId);
        }

    }
}
