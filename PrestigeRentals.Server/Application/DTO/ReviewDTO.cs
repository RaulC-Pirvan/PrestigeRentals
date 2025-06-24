using System;

namespace PrestigeRentals.Application.DTO
{
    /// <summary>
    /// Data Transfer Object representing a user review for a rented vehicle.
    /// </summary>
    public class ReviewDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the review.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who submitted the review.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the vehicle that was reviewed.
        /// </summary>
        public long VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the rating given by the user, typically on a scale (e.g., 1 to 5).
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the optional textual description provided by the user in the review.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the review was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
