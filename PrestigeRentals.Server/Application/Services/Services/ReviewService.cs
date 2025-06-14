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
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IOrderRepository _orderRepository;

        public ReviewService(IReviewRepository reviewRepository, IOrderRepository orderRepository)
        {
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
        }

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

        public async Task<bool> ReviewExistsForOrder(long orderId)
        {
            return await _reviewRepository.ExistsByOrderIdAsync(orderId);
        }
    }
}
