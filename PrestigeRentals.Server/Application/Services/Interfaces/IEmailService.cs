using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Defines methods for sending emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends a verification email containing a verification code to the specified email address.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="verificationCode">The verification code to include in the email.</param>
        Task SendVerificationEmailAsync(string toEmail, string verificationCode);
        Task SendNewPasswordEmailAsync(string toEmal, string newPassword);
    }
}
