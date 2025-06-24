using System;
using System.ComponentModel.DataAnnotations;

namespace PrestigeRentals.Domain.Entities
{
    /// <summary>
    /// Represents a review left by a user for a vehicle they have rented.
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Gets or sets the unique identifier for the review.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who submitted the review.
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the vehicle being reviewed.
        /// </summary>
        [Required]
        public long VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the order associated with the review.
        /// This ensures that only users who have completed a rental can review.
        /// </summary>
        [Required]
        public long OrderId { get; set; }

        /// <summary>
        /// Gets or sets the rating given in the review, typically from 1 to 5.
        /// </summary>
        [Required]
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the text description provided by the user in the review.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the timestamp indicating when the review was created.
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
