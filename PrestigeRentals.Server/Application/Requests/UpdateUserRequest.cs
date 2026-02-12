using System;

namespace PrestigeRentals.Application.Requests
{
    /// <summary>
    /// Represents a request to update a user's details, including their personal information and account status.
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user to be updated.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user to be updated.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user to be updated.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the role of the user. Default is "User".
        /// </summary>
        public string Role { get; set; } = "User";

        /// <summary>
        /// Gets or sets a value indicating whether the user is active. Default is true.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the user is deleted. Default is false.
        /// </summary>
        public bool Deleted { get; set; } = false;

        /// <summary>
        /// Gets or sets the first name of the user to be updated.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user to be updated.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the user to be updated.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the image data for the user's profile picture. This is a byte array representing the image.
        /// </summary>
        public byte[] ImageData { get; set; }
    }
}
