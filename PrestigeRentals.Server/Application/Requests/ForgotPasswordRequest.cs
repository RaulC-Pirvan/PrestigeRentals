using System;

namespace PrestigeRentals.Application.Requests
{
    /// <summary>
    /// Request model used when a user initiates a "forgot password" action.
    /// </summary>
    public class ForgotPasswordRequest
    {
        /// <summary>
        /// Gets or sets the email address associated with the user account requesting password reset.
        /// </summary>
        public string Email { get; set; }
    }
}
