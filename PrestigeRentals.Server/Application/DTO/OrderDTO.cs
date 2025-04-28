using System;

namespace PrestigeRentals.Application.DTO
{
    /// <summary>
    /// Data Transfer Object representing an order (rental) made by a user.
    /// </summary>
    public class OrderDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the order.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who made the order.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the vehicle that was rented.
        /// </summary>
        public long VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the start date and time of the rental period.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end date and time of the rental period.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the order has been cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }
    }
}
