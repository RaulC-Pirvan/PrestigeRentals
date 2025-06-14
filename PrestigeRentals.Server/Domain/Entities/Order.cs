using System;
using System.ComponentModel.DataAnnotations;

namespace PrestigeRentals.Domain.Entities
{
    /// <summary>
    /// Represents an order in the system for renting a vehicle.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Gets or sets the unique identifier for the order.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who created the order.
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the vehicle being rented in this order.
        /// </summary>
        [Required]
        public long VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the start time of the rental period.
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the rental period.
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public decimal PricePerDay { get; set; }

        [Required]
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether the order has been cancelled.
        /// Default value is false.
        /// </summary>
        [Required]
        public bool IsCancelled { get; set; } = false;

        [Required]
        public string BookingReference { get; set; }

        [Required]
        public string QrCodeData { get; set; }

        [Required]
        public string QrCodeBase64Image { get; set; }

        [Required]
        public bool ReviewReminderSet { get; set; } = false;

    }
}
