using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Validators
{
    public class RegisterValidator
    {
        public static bool ValidateEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        public static bool ValidatePassword(string password)
        {
            // Password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter,
            // one number, and one special character (!@#).
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#])[A-Za-z\d!@#]{6,}$");
            return passwordRegex.IsMatch(password);
        }

        public static bool ValidateName(string name)
        {
            // Check if the name is not null or empty
            return !string.IsNullOrWhiteSpace(name);
        }
    }
}
