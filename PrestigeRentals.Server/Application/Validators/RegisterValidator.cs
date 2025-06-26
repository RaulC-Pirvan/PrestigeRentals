using System;
using System.Text.RegularExpressions;

namespace PrestigeRentals.Application.Validators
{
    /// <summary>
    /// Provides static methods for validating user registration input such as email, password, and name.
    /// </summary>
    public class RegisterValidator
    {
        /// <summary>
        /// Validates whether the provided email string has a valid email format.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns>True if the email format is valid; otherwise, false.</returns>
        public static bool ValidateEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        /// <summary>
        /// Validates whether the provided password meets security requirements.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <returns>
        /// True if the password is at least 6 characters long and contains at least:
        /// one uppercase letter, one lowercase letter, one digit, and one special character (!, @, or #).
        /// </returns>
        public static bool ValidatePassword(string password)
        {
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#])[A-Za-z\d!@#]{6,}$");
            return passwordRegex.IsMatch(password);
        }

        /// <summary>
        /// Validates that a name is not null, empty, or composed only of whitespace.
        /// </summary>
        /// <param name="name">The name string to validate.</param>
        /// <returns>True if the name is not null or whitespace; otherwise, false.</returns>
        public static bool ValidateName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }
    }
}
