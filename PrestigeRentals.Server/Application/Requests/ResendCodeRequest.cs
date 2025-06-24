using System;

namespace PrestigeRentals.Application.Requests
{
    /// <summary>
    /// Request model used to trigger a resend of a verification or authentication code to a user's email.
    /// </summary>
    public class ResendCodeRequest
    {
        /// <summary>
        /// Gets or sets the email address to which the verification code should be resent.
        /// </summary>
        public string Email { get; set; }
    }
}
