using System;

namespace PrestigeRentals.Application.Requests
{
    /// <summary>
    /// Represents a request to log in a user by providing email and password.
    /// This class is used to capture login credentials from the client.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Gets or sets the email address of the user attempting to log in.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user attempting to log in.
        /// </summary>
        public string Password { get; set; }
    }
}
