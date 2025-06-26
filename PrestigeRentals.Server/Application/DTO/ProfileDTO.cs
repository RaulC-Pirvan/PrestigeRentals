using System;

namespace PrestigeRentals.Application.DTO
{
    /// <summary>
    /// Data Transfer Object representing a user's profile information.
    /// </summary>
    public class ProfileDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the profile.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        public string LastName { get; set; }
    }
}
