using System;

namespace PrestigeRentals.Application.Requests
{
    /// <summary>
    /// Request model for initiating a payment for an order.
    /// </summary>
    public class CreatePaymentRequest
    {
        /// <summary>
        /// Gets or sets the ID of the order for which the payment is made.
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Gets or sets the total cost of the order to be paid.
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user making the payment.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the vehicle associated with the order.
        /// </summary>
        public long VehicleId { get; set; }
    }
}
