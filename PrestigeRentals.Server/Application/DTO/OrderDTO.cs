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
        /// Gets or sets the price per day for renting the vehicle.
        /// </summary>
        public decimal PricePerDay { get; set; }

        /// <summary>
        /// Gets or sets the total cost for the entire rental period.
        /// This is usually calculated as the number of days multiplied by PricePerDay.
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the order has been cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Gets or sets the unique booking reference code associated with the rental.
        /// This is used to generate and validate the QR code.
        /// </summary>
        public string BookingReference { get; set; }

        /// <summary>
        /// Gets or sets the raw data encoded in the QR code for this booking.
        /// </summary>
        public string QrCodeData { get; set; }

        /// <summary>
        /// Gets or sets the base64-encoded image of the QR code, used for display in the frontend.
        /// </summary>
        public string QrCodeBase64Image { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the QR code has been used (e.g., at vehicle pickup).
        /// </summary>
        public bool IsUsed { get; set; }
    }
}
