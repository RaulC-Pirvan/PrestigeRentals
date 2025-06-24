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

        /// <summary>
        /// Gets or sets the price per day for the rented vehicle.
        /// </summary>
        [Required]
        public decimal PricePerDay { get; set; }

        /// <summary>
        /// Gets or sets the total cost for the rental period.
        /// </summary>
        [Required]
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether the order has been cancelled.
        /// </summary>
        [Required]
        public bool IsCancelled { get; set; } = false;

        /// <summary>
        /// Gets or sets the unique booking reference string associated with this order.
        /// </summary>
        [Required]
        public string BookingReference { get; set; }

        /// <summary>
        /// Gets or sets the data encoded in the QR code for this order.
        /// </summary>
        [Required]
        public string QrCodeData { get; set; }

        /// <summary>
        /// Gets or sets the base64-encoded PNG image of the QR code.
        /// </summary>
        [Required]
        public string QrCodeBase64Image { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a review reminder has been sent for this order.
        /// </summary>
        [Required]
        public bool ReviewReminderSet { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the order has been used (e.g., vehicle picked up).
        /// </summary>
        [Required]
        public bool IsUsed { get; set; } = false;
    }
}
