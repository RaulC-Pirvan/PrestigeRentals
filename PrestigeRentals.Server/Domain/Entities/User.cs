using System;
using System.ComponentModel.DataAnnotations;

namespace PrestigeRentals.Domain.Entities
{
    /// <summary>
    /// Represents a user account in the Prestige Rentals system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Primary key for the User entity.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Email address of the user. This should be unique.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Hashed password of the user.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Role assigned to the user (e.g., "Admin", "User"). Defaults to "User".
        /// </summary>
        [Required]
        public string Role { get; set; } = "User";

        /// <summary>
        /// Whether the user account is active.
        /// </summary>
        [Required]
        public bool Active { get; set; } = true;

        /// <summary>
        /// Indicates if the user is soft-deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; } = false;

        /// <summary>
        /// Code sent to the user's email for verification.
        /// </summary>
        public string? EmailVerificationCode { get; set; }

        /// <summary>
        /// Expiry date/time for the verification code.
        /// </summary>
        public DateTime? VerificationCodeExpiry { get; set; }

        /// <summary>
        /// Indicates whether the user's email has been confirmed.
        /// </summary>
        public bool EmailConfirmed { get; set; } = false;

        public bool Banned { get; set; } = false;
    }
}
