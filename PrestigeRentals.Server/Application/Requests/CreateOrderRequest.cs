using System;
using System.ComponentModel.DataAnnotations;

namespace PrestigeRentals.Application.Requests
{
    /// <summary>
    /// Request model for creating a new order (vehicle rental).
    /// </summary>
    public class CreateOrderRequest
    {
        /// <summary>
        /// Gets or sets the ID of the user making the order.
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the vehicle being rented.
        /// </summary>
        [Required]
        public long VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the start date and time of the rental period.
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end date and time of the rental period.
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }
    }
}
