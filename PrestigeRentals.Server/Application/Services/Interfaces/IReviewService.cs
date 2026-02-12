using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Defines operations for managing reviews in the system.
    /// </summary>
    public interface IReviewService
    {
        /// <summary>
        /// Creates a new review based on the provided request data.
        /// </summary>
        /// <param name="request">The request containing review information.</param>
        /// <returns>The created review as a DTO.</returns>
        Task<ReviewDTO> CreateReview(CreateReviewRequest request);

        /// <summary>
        /// Retrieves all reviews in the system.
        /// </summary>
        /// <returns>A collection of all reviews.</returns>
        Task<IEnumerable<ReviewDTO>> GetAllReviews();

        /// <summary>
        /// Retrieves a specific review by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the review.</param>
        /// <returns>The corresponding review DTO, or null if not found.</returns>
        Task<ReviewDTO> GetReviewById(long id);

        /// <summary>
        /// Retrieves all reviews submitted by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A collection of reviews submitted by the specified user.</returns>
        Task<IEnumerable<ReviewDTO>> GetReviewsByUserIdAsync(long userId);

        /// <summary>
        /// Retrieves all reviews associated with a specific vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <returns>A list of reviews for the specified vehicle.</returns>
        Task<List<ReviewDTO>> GetReviewsByVehicleIdAsync(long vehicleId);

        /// <summary>
        /// Checks whether a review already exists for the given order.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>True if a review exists for the order; otherwise, false.</returns>
        Task<bool> ReviewExistsForOrder(long orderId);
    }
}
