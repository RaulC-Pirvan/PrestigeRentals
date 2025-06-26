using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Domain.Interfaces;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing review entities in the database.
    /// Implements <see cref="IReviewRepository"/>.
    /// </summary>
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The application's database context.</param>
        public ReviewRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves a review by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the review.</param>
        /// <returns>The review entity if found; otherwise, null.</returns>
        public async Task<Review> GetByIdAsync(long id)
        {
            return await _dbContext.Reviews.FindAsync(id);
        }

        /// <summary>
        /// Retrieves all reviews from the database.
        /// </summary>
        /// <returns>A collection of all review entities.</returns>
        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _dbContext.Reviews.ToListAsync();
        }

        /// <summary>
        /// Adds a new review to the database.
        /// </summary>
        /// <param name="review">The review entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the review is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs during saving.</exception>
        public async Task AddAsync(Review review)
        {
            if (review == null)
                throw new ArgumentNullException(nameof(review), "Review cannot be null.");

            try
            {
                _dbContext.Reviews.Add(review);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while saving the review.", ex);
            }
        }

        /// <summary>
        /// Updates an existing review in the database.
        /// </summary>
        /// <param name="review">The review entity with updated data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the review is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs during update.</exception>
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
                throw new InvalidOperationException("An error occurred while updating the review.", ex);
            }
        }

        /// <summary>
        /// Deletes a review from the database.
        /// </summary>
        /// <param name="review">The review entity to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the review is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs during deletion.</exception>
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
                throw new InvalidOperationException("An error occurred while deleting the review.", ex);
            }
        }

        /// <summary>
        /// Retrieves all reviews submitted by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A collection of reviews submitted by the user.</returns>
        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(long userId)
        {
            return await _dbContext.Reviews.Where(r => r.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Retrieves all reviews for a specific vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <returns>A collection of reviews for the specified vehicle.</returns>
        public async Task<IEnumerable<Review>> GetReviewsByVehicleIdAsync(long vehicleId)
        {
            return await _dbContext.Reviews.Where(r => r.VehicleId == vehicleId).ToListAsync();
        }

        /// <summary>
        /// Retrieves a review associated with a specific order.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>The review if found; otherwise, null.</returns>
        public async Task<Review?> GetReviewByOrderIdAsync(long orderId)
        {
            return await _dbContext.Reviews.FirstOrDefaultAsync(r => r.OrderId == orderId);
        }

        /// <summary>
        /// Determines whether a review exists for a specific order.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>True if a review exists; otherwise, false.</returns>
        public async Task<bool> ExistsByOrderIdAsync(long orderId)
        {
            return await _dbContext.Reviews.AnyAsync(r => r.OrderId == orderId);
        }
    }
}
