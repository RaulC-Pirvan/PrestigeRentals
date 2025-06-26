using System;

namespace PrestigeRentals.Application.Requests
{
    /// <summary>
    /// Represents a request to update the user's personal details such as first and last name.
    /// </summary>
    public class UpdateUserDetailsRequest
    {
        /// <summary>
        /// Gets or sets the first name of the user to be updated.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user to be updated.
        /// </summary>
        public string? LastName { get; set; }
    }
}
