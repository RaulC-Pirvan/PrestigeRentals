using System;

namespace PrestigeRentals.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when a review with the specified ID cannot be found.
    /// </summary>
    public class ReviewNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewNotFoundException"/> class.
        /// </summary>
        /// <param name="Id">The ID of the review that was not found.</param>
        public ReviewNotFoundException(long Id)
            : base($"The review with Id {Id} was not found.") { }
    }
}
