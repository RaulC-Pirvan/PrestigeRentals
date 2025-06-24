using PrestigeRentals.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrestigeRentals.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for CRUD and query operations related to <see cref="Review"/> entities.
    /// </summary>
    public interface IReviewRepository
    {
        /// <summary>
        /// Retrieves a review by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the review to retrieve.</param>
        /// <returns>The review if found; otherwise, null.</returns>
        Task<Review> GetByIdAsync(long id);

        /// <summary>
        /// Retrieves all reviews in the system.
        /// </summary>
        /// <returns>A collection of all reviews.</returns>
        Task<IEnumerable<Review>> GetAllAsync();

        /// <summary>
        /// Adds a new review to the data store.
        /// </summary>
        /// <param name="review">The review to add.</param>
        Task AddAsync(Review review);

        /// <summary>
        /// Updates an existing review in the data store.
        /// </summary>
        /// <param name="review">The review to update.</param>
        Task UpdateAsync(Review review);

        /// <summary>
        /// Deletes a review from the data store.
        /// </summary>
        /// <param name="review">The review to delete.</param>
        Task DeleteAsync(Review review);

        /// <summary>
        /// Retrieves all reviews submitted by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A collection of reviews submitted by the user.</returns>
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(long userId);

        /// <summary>
        /// Retrieves all reviews for a specific vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <returns>A collection of reviews for the specified vehicle.</returns>
        Task<IEnumerable<Review>> GetReviewsByVehicleIdAsync(long vehicleId);

        /// <summary>
        /// Retrieves the review associated with a specific order.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>The review if found; otherwise, null.</returns>
        Task<Review?> GetReviewByOrderIdAsync(long orderId);

        /// <summary>
        /// Checks whether a review exists for the specified order.
        /// </summary>
        /// <param name="orderId">The ID of the order to check.</param>
        /// <returns>True if a review exists; otherwise, false.</returns>
        Task<bool> ExistsByOrderIdAsync(long orderId);
    }
}
