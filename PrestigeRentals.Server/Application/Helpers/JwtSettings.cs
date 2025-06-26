using System;

namespace PrestigeRentals.Application.Helpers
{
    /// <summary>
    /// Represents the settings required for configuring JWT (JSON Web Token) authentication.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Gets or sets the secret key used for signing JWT tokens.
        /// This key should be kept confidential and secure.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Gets or sets the issuer of the JWT token.
        /// This typically refers to the entity that is generating the token.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the audience of the JWT token.
        /// This typically refers to the intended recipients or consumers of the token.
        /// </summary>
        public string Audience { get; set; }
    }
}
