using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Exceptions;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Application.Services.Services;
using PrestigeRentals.Domain.Exceptions;
using PrestigeRentals.Domain.Interfaces;
using System.Security.Claims;

namespace PrestigeRentals.Presentation.Controllers
{
    /// <summary>
    /// Controller responsible for handling review-related endpoints including creation, retrieval,
    /// and existence checks of vehicle reviews.
    /// </summary>
    [Route("api/review")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IReviewRepository _reviewRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewController"/> class.
        /// </summary>
        public ReviewController(IReviewService reviewService, IEmailService emailService, IUserRepository userRepository, IReviewRepository reviewRepository)
        {
            _reviewService = reviewService;
            _emailService = emailService;
            _userRepository = userRepository;
            _reviewRepository = reviewRepository;
        }

        /// <summary>
        /// Creates a new review.
        /// Requires authentication.
        /// </summary>
        /// <param name="createReviewRequest">The review data submitted by the user.</param>
        /// <returns>The created review on success; appropriate error otherwise.</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest createReviewRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdReview = await _reviewService.CreateReview(createReviewRequest);
                return Ok(createdReview);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all reviews.
        /// Requires Admin role.
        /// </summary>
        /// <returns>A list of all reviews or internal server error.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetAllReviews()
        {
            try
            {
                var reviews = await _reviewService.GetAllReviews();
                return Ok(reviews);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a review by its unique ID.
        /// Requires authentication.
        /// </summary>
        /// <param name="id">The ID of the review.</param>
        /// <returns>The review if found; 404 otherwise.</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ReviewDTO>> GetReviewById(long id)
        {
            try
            {
                var review = await _reviewService.GetReviewById(id);
                if (review == null)
                    return NotFound();

                return Ok(review);
            }

            catch (ReviewNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// Retrieves reviews submitted by the currently authenticated user.
        /// </summary>
        /// <returns>A list of the user's reviews or unauthorized if no user found.</returns>
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetReviewsForUser()
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !long.TryParse(userIdString, out var userId))
                {
                    return Unauthorized();
                }

                var reviews = await _reviewService.GetReviewsByUserIdAsync(userId);
                return Ok(reviews);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);

            }
        }

        /// <summary>
        /// Retrieves all reviews for a specific vehicle.
        /// </summary>
        /// <param name="vehicleId">The vehicle's ID.</param>
        /// <returns>A list of reviews or error status.</returns>
        [HttpGet("vehicle/{vehicleId}")]
        public async Task<ActionResult> GetReviewsForVehicle(long vehicleId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByVehicleIdAsync(vehicleId);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving reviews: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if a review exists for a specific order.
        /// </summary>
        /// <param name="orderId">The order ID to check.</param>
        /// <returns>True if a review exists; false otherwise.</returns>
        [HttpGet("exists/{orderId}")]
        public async Task<IActionResult> ReviewExists(long orderId)
        {
            bool exists = await _reviewRepository.ExistsByOrderIdAsync(orderId);
            return Ok(exists);
        }
    }
}
