using System;
using System.ComponentModel.DataAnnotations;

namespace PrestigeRentals.Application.Requests
{
    /// <summary>
    /// Request model for submitting a review for a completed rental.
    /// </summary>
    public class CreateReviewRequest
    {
        /// <summary>
        /// Gets or sets the ID of the user submitting the review.
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the vehicle being reviewed.
        /// </summary>
        [Required]
        public long VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the numeric rating given by the user (e.g., 1 to 5).
        /// </summary>
        [Required]
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the description or feedback provided by the user.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ID of the order associated with the review.
        /// Used to validate that the user has rented the vehicle.
        /// </summary>
        [Required]
        public long OrderId { get; set; }
    }
}
