using System.ComponentModel.DataAnnotations;

namespace PrestigeRentals.Application.Requests
{
    /// <summary>
    /// Represents the data required to verify a user's email address.
    /// </summary>
    public class VerifyEmailRequest
    {
        /// <summary>
        /// The email address to verify.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The verification code sent to the user's email.
        /// </summary>
        public string Code { get; set; }
    }
}
