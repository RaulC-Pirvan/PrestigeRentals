using System;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for generating JWT tokens for users.
    /// </summary>
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token will be generated.</param>
        /// <returns>A string representing the generated JWT token.</returns>
        string GenerateToken(User user);
    }
}
