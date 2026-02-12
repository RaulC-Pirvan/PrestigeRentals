namespace PrestigeRentals.Application.DTO
{
    /// <summary>
    /// Represents the response returned after a successful authentication.
    /// </summary>
    public class AuthenticatedResponseDTO
    {
        /// <summary>
        /// The JWT token issued to the authenticated user.
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
