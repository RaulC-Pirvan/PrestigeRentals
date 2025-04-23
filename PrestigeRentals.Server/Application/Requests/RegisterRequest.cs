using System;

namespace PrestigeRentals.Application.Requests
{
    /// <summary>
    /// Represents a request to register a new user, containing their personal information and credentials.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Gets or sets the email address of the user registering for the application.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user registering for the application.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user registering for the application.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user registering for the application.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the user registering for the application.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the optional image data of the user registering for the application.
        /// This could be a base64 encoded string representing the user's profile picture.
        /// </summary>
        public string? ImageData { get; set; }
    }
}
