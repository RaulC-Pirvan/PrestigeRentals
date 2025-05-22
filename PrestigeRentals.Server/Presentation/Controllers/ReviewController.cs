using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Exceptions;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Application.Services.Services;
using System.Security.Claims;

namespace PrestigeRentals.Presentation.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
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

            catch (OrderNotFoundException ex) // De adaugat exceptii custom pentru reviews
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
    }
}
