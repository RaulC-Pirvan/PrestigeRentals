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
        public int Id { get; set; }

        /// <summary>
        /// Email address of the user. This serves as the unique identifier.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Hashed password of the user.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Role assigned to the user (e.g., Admin, User).
        /// Defaults to "User".
        /// </summary>
        [Required]
        public string Role { get; set; } = "User";

        /// <summary>
        /// Indicates whether the user is currently active.
        /// </summary>
        [Required]
        public bool Active { get; set; } = true;

        /// <summary>
        /// Indicates whether the user account is marked as deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; } = false;
    }
}
