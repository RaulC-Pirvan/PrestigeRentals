using System;

namespace PrestigeRentals.Application.DTO
{
    /// <summary>
    /// Data Transfer Object representing a simplified view of a user's profile,
    /// typically used for display or authorization purposes.
    /// </summary>
    public class UserProfileDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's role in the system (e.g., "User", "Admin").
        /// </summary>
        public string Role { get; set; }
    }
}
