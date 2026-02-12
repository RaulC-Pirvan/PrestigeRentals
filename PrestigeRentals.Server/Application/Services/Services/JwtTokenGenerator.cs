using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PrestigeRentals.Application.Helpers;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Services
{
    /// <summary>
    /// Generates JSON Web Tokens (JWT) for user authentication.
    /// </summary>
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtTokenGenerator"/> class.
        /// </summary>
        /// <param name="jwtSettings">The settings used for configuring JWT token generation, including the secret key, issuer, and audience.</param>
        public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Generates a JWT token for the given user.
        /// </summary>
        /// <param name="user">The user for whom the JWT token will be generated.</param>
        /// <returns>A string representing the JWT token.</returns>
        public string GenerateToken(User user)
        {
            // Create a symmetric security key using the secret key from the JWT settings
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            // Create the signing credentials using the security key and HMAC-SHA256 algorithm
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Prepare the claims
            var claims = new[]
            {
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email, user.Email),
                new System.Security.Claims.Claim("banned", user.Banned.ToString().ToLower()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.Role),
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Create the JWT token with issuer, audience, expiration time, and signing credentials
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Set expiration to 30 minutes
                signingCredentials: credentials);

            // Write and return the generated token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
