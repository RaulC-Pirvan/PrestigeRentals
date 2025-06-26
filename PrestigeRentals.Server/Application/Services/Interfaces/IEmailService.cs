using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Defines methods for sending various types of emails used in the platform.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends a verification email containing a verification code to the specified email address.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="verificationCode">The verification code to include in the email.</param>
        Task SendVerificationEmailAsync(string toEmail, string verificationCode);

        /// <summary>
        /// Sends a new password email to a user who requested password reset.
        /// </summary>
        /// <param name="toEmal">The recipient's email address.</param>
        /// <param name="newPassword">The new password generated for the user.</param>
        Task SendNewPasswordEmailAsync(string toEmal, string newPassword);

        /// <summary>
        /// Sends an email to the user containing the QR code for a confirmed booking.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="bookingReference">The booking reference associated with the reservation.</param>
        /// <param name="qrData">The raw QR data (usually a unique string).</param>
        Task SendQrCodeEmailAsync(string toEmail, string bookingReference, string qrData);

        /// <summary>
        /// Sends a message from the contact form to the admin's inbox.
        /// </summary>
        /// <param name="userEmail">The email address of the user who submitted the message.</param>
        /// <param name="message">The message content provided by the user.</param>
        Task SendContactFormTicketToAdminAsync(string userEmail, string message);

        /// <summary>
        /// Sends a notification to the admin when a user submits a new review.
        /// </summary>
        /// <param name="userEmail">The email address of the user who submitted the review.</param>
        /// <param name="vehicleId">The ID of the vehicle being reviewed.</param>
        /// <param name="rating">The rating given by the user.</param>
        /// <param name="review">The review content submitted by the user.</param>
        Task SendReviewNotificationToAdminAsync(string userEmail, long vehicleId, int rating, string review);

        /// <summary>
        /// Sends a notification to the admin about a vehicle being added, updated, or deleted.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle that was changed.</param>
        /// <param name="action">The action performed on the vehicle (e.g., "created", "updated", "deleted").</param>
        Task SendVehicleChangeNotificationToAdminAsync(long vehicleId, string action);

        /// <summary>
        /// Sends a notification to the admin about a change in a user's status (e.g., banned/unbanned).
        /// </summary>
        /// <param name="affectedUserEmail">The email address of the user whose status changed.</param>
        /// <param name="newStatus">The new status assigned to the user.</param>
        Task SendUserStatusChangeNotificationToAdminAsync(string affectedUserEmail, string newStatus);

        /// <summary>
        /// Sends an email requesting a user to submit a review after a completed order.
        /// </summary>
        /// <param name="userEmail">The email address of the user.</param>
        /// <param name="vehicleName">The name of the vehicle rented.</param>
        /// <param name="orderId">The ID of the completed order.</param>
        Task SendReviewRequestEmailAsync(string userEmail, string vehicleName, long orderId);

        /// <summary>
        /// Sends a notification email to a user informing them that their account has been banned.
        /// </summary>
        /// <param name="userEmail">The email address of the banned user.</param>
        /// <returns>A task representing the asynchronous email sending operation.</returns>
        Task SendUserBannedEmailAsync(string userEmail);
    }
}
