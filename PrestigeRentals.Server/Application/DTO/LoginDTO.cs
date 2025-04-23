using System;

namespace PrestigeRentals.Application.DTO
{
    /// <summary>
    /// Represents the data transfer object (DTO) for a user login.
    /// This class contains the email and password that a user would submit for authentication.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Gets or sets the email address for login.
        /// This should be a valid email format to ensure proper authentication.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password associated with the login.
        /// The password should be securely stored and never exposed in plain text.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginDTO"/> class.
        /// </summary>
        public LoginDTO() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginDTO"/> class with specified email and password.
        /// </summary>
        /// <param name="email">The email address for login.</param>
        /// <param name="password">The password associated with the login.</param>
        public LoginDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
