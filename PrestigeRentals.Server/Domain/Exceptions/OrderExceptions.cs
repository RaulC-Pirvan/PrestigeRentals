using System;

namespace PrestigeRentals.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when an order with the specified ID cannot be found.
    /// </summary>
    public class OrderNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderNotFoundException"/> class with the specified order ID.
        /// </summary>
        /// <param name="orderId">The ID of the order that was not found.</param>
        public OrderNotFoundException(long orderId)
            : base($"The order with Id {orderId} was not found.") { }
    }

    /// <summary>
    /// Exception thrown when an order has already been cancelled.
    /// </summary>
    public class OrderAlreadyCancelledException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderAlreadyCancelledException"/> class with the specified order ID.
        /// </summary>
        /// <param name="orderId">The ID of the order that is already cancelled.</param>
        public OrderAlreadyCancelledException(long orderId)
            : base($"The order with Id {orderId} is already cancelled.") { }
    }
}
