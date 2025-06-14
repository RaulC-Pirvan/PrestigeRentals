using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Exceptions;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Application.Services.Services;
using PrestigeRentals.Domain.Exceptions;
using System.Security.Claims;

namespace PrestigeRentals.Presentation.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public ReviewController(IReviewService reviewService, IEmailService emailService, IUserRepository userRepository)
        {
            _reviewService = reviewService;
            _emailService = emailService;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ReviewDTO>> CreateReview([FromBody] CreateReviewRequest createReviewRequest)
        {
            if (createReviewRequest == null)
                return BadRequest("Invalid data.");

            try
            {
                var createdReview = await _reviewService.CreateReview(createReviewRequest);

                var user = await _userRepository.GetUserById(createReviewRequest.UserId);


                if (user != null)
                {
                    await _emailService.SendReviewNotificationToAdminAsync(
                  userEmail: user.Email,
                  vehicleId: createReviewRequest.VehicleId,
                  rating: createReviewRequest.Rating,
                  review: createReviewRequest.Description
              );
                }
                return Ok(createdReview);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

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
    }
}
