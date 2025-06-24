using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services.Services
{
    /// <summary>
    /// Service responsible for managing reviews, including creation, retrieval, and validation.
    /// Implements <see cref="IReviewService"/>.
    /// </summary>
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewService"/> class.
        /// </summary>
        /// <param name="reviewRepository">Repository for review data access.</param>
        /// <param name="orderRepository">Repository for validating order ownership and review status.</param>
        public ReviewService(IReviewRepository reviewRepository, IOrderRepository orderRepository)
        {
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Creates a new review for a specific order, if the user is authorized and no review exists yet.
        /// </summary>
        /// <param name="request">The request containing review data.</param>
        /// <returns>A DTO of the created review.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the order does not belong to the user.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a review for the order already exists.</exception>
        public async Task<ReviewDTO> CreateReview(CreateReviewRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);

            if (order == null || order.UserId != request.UserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to review this order.");
            }

            var existingReview = await _reviewRepository.GetReviewByOrderIdAsync(request.OrderId);
            if (existingReview != null)
            {
                throw new InvalidOperationException("A review for this order already exists.");
            }


            Review review = new Review
            {
                UserId = request.UserId,
                VehicleId = request.VehicleId,
                Rating = request.Rating,
                Description = request.Description,
                OrderId = request.OrderId,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.AddAsync(review);

            return new ReviewDTO
            {
                Id = review.Id,
                UserId = review.UserId,
                VehicleId = review.VehicleId,
                Rating = review.Rating,
                Description = review.Description,
                CreatedAt = review.CreatedAt
            };
        }


        /// <summary>
        /// Retrieves all reviews from the system.
        /// </summary>
        /// <returns>A collection of all reviews represented as DTOs.</returns>
        public async Task<IEnumerable<ReviewDTO>> GetAllReviews()
        {
            IEnumerable<Review> reviews = await _reviewRepository.GetAllAsync();
            List<ReviewDTO> reviewDTOs = new List<ReviewDTO>();

            foreach (var review in reviews)
            {
                reviewDTOs.Add(new ReviewDTO
                {
                    Id = review.Id,
                    UserId = review.UserId,
                    VehicleId = review.VehicleId,
                    Rating = review.Rating,
                    Description = review.Description
                });
            }

            return reviewDTOs;
        }

        /// <summary>
        /// Retrieves a review by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the review.</param>
        /// <returns>The review DTO if found; otherwise, null.</returns>
        public async Task<ReviewDTO> GetReviewById(long id)
        {
            Review review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
                return null;

            return new ReviewDTO
            {
                Id = review.Id,
                UserId = review.UserId,
                VehicleId = review.VehicleId,
                Rating = review.Rating,
                Description = review.Description
            };
        }


        /// <summary>
        /// Retrieves all reviews submitted by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A collection of review DTOs submitted by the specified user.</returns>
        public async Task<IEnumerable<ReviewDTO>> GetReviewsByUserIdAsync(long userId)
        {
            var reviews = await _reviewRepository.GetReviewsByUserIdAsync(userId);

            var reviewDTOs = reviews.Select(r => new ReviewDTO
            {
                Id = r.Id,
                UserId = r.UserId,
                VehicleId = r.VehicleId,
                Rating = r.Rating,
                Description = r.Description,
                CreatedAt = r.CreatedAt
            });

            return reviewDTOs;
        }


        /// <summary>
        /// Retrieves all reviews associated with a specific vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <returns>A list of review DTOs for the specified vehicle.</returns>
        public async Task<List<ReviewDTO>> GetReviewsByVehicleIdAsync(long vehicleId)
        {
            var reviews = await _reviewRepository.GetReviewsByVehicleIdAsync(vehicleId);

            var reviewDTOs = reviews.Select(r => new ReviewDTO
            {
                Id = r.Id,
                UserId = r.UserId,
                VehicleId = r.VehicleId,
                Rating = r.Rating,
                Description = r.Description,
                CreatedAt = r.CreatedAt
            }).ToList();

            return reviewDTOs;
        }

        /// <summary>
        /// Checks whether a review already exists for a specific order.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>True if a review exists; otherwise, false.</returns>
        public async Task<bool> ReviewExistsForOrder(long orderId)
        {
            return await _reviewRepository.ExistsByOrderIdAsync(orderId);
        }
    }
}
