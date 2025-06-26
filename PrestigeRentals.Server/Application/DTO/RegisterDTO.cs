using System;

namespace PrestigeRentals.Application.DTO
{
    /// <summary>
    /// Represents the data transfer object (DTO) for user registration.
    /// This class contains the email and password that a user would submit to create a new account.
    /// </summary>
    public class RegisterDTO
    {
        /// <summary>
        /// Gets or sets the email address for registration.
        /// This should be a valid email format to ensure proper account creation.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password associated with the registration.
        /// The password should be securely stored and should meet security standards such as length and complexity.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterDTO"/> class.
        /// </summary>
        public RegisterDTO() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterDTO"/> class with specified email and password.
        /// </summary>
        /// <param name="email">The email address for registration.</param>
        /// <param name="password">The password associated with the registration.</param>
        public RegisterDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
