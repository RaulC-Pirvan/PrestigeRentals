using System;
using System.Threading.Tasks;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for authentication services, including user registration, login, password hashing, and verification.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user asynchronously.
        /// </summary>
        /// <param name="registerRequest">The request containing the user details for registration.</param>
        /// <returns>A task representing the asynchronous operation, with a string result indicating the registration status or a token.</returns>
        Task<string> RegisterAsync(RegisterRequest registerRequest);

        /// <summary>
        /// Authenticates a user asynchronously based on their login credentials.
        /// </summary>
        /// <param name="loginRequest">The request containing the user's email and password for authentication.</param>
        /// <returns>A task representing the asynchronous operation, with a string result containing a JWT or an authentication token.</returns>
        Task<string> AuthenticateAsync(LoginRequest loginRequest);

        /// <summary>
        /// Hashes a plain-text password using a secure algorithm.
        /// </summary>
        /// <param name="password">The plain-text password to be hashed.</param>
        /// <returns>The hashed password as a string.</returns>
        string HashPassword(string password);

        /// <summary>
        /// Verifies whether the provided plain-text password matches the hashed password.
        /// </summary>
        /// <param name="oldPassword">The plain-text password to verify.</param>
        /// <param name="hashedPassword">The hashed password to compare against.</param>
        /// <returns>True if the passwords match, otherwise false.</returns>
        bool VerifyPassword(string oldPassword, string hashedPassword);
    }
}
