using PrestigeRentals.Application.Services.Interfaces;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services.Services
{
    /// <summary>
    /// Implementation of the <see cref="IEmailService"/> that uses SMTP to send various transactional emails 
    /// (verification codes, booking confirmations, admin notifications, etc.).
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost = "smtp.ethereal.email";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "garret.hauck36@ethereal.email";
        private readonly string _smtpPassword = "83dKdH1D92st8MVMEM";
        private readonly string _adminEmail = "PrestigeRentalsRO@gmail.com";

        /// <summary>
        /// Sends a basic HTML email using SMTP.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="htmlBody">The HTML body of the email.</param>
        private async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var smtpClient = new SmtpClient(_smtpHost)
            {
                Port = _smtpPort,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUsername, "Prestige Rentals"),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }

        /// <summary>
        /// Sends a verification email with a one-time verification code.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="verificationCode">The verification code to be sent.</param>
        public async Task SendVerificationEmailAsync(string toEmail, string verificationCode)
        {
            var smtpClient = new SmtpClient(_smtpHost)
            {
                Port = _smtpPort,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUsername, "Prestige Rentals"),
                Subject = "Welcome to Prestige Rentals",
                Body = $@"
                        <div style='font-family: Arial, sans-serif; padding: 20px;'>
                            <h2>Email Verification</h2>
                            <p>Your verification code is: <strong style='font-size: 18px;'>{verificationCode}</strong></p>
                            <p style='color: gray;'>If you didn’t request this, you can ignore the email.</p>
                        </div>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);

        }

        /// <summary>
        /// Sends an email to the user containing their newly generated password.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="newPassword">The new password to include in the email.</param>
        public async Task SendNewPasswordEmailAsync(string toEmail, string newPassword)
        {
            var smtpClient = new SmtpClient(_smtpHost)
            {
                Port = _smtpPort,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUsername, "Prestige Rentals"),
                Subject = "Your New Password - Prestige Rentals",
                Body = $@"
                        <div style='font-family: Arial, sans-serif; padding: 20px;'>
                            <h2>Password Reset</h2>
                            <p>Your new temporary password is: <strong style='font-size: 18px;'>{newPassword}</strong></p>
                            <p style='color: gray;'>Please log in using this password and change it as soon as possible.</p>
                            <p style='color: gray;'>If you didn’t request this, you can ignore the email.</p>
                        </div>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);

        }

        /// <summary>
        /// Generates a QR code as a PNG byte array from the specified string data.
        /// </summary>
        /// <param name="data">The data to encode into the QR code.</param>
        /// <returns>A byte array representing the PNG image of the QR code.</returns>
        private byte[] GenerateQrCodeBytes(string data)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

        /// <summary>
        /// Sends a booking confirmation email to the user, including a QR code image for vehicle pickup.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="bookingReference">The booking reference code.</param>
        /// <param name="qrData">The string encoded in the QR code.</param>
        public async Task SendQrCodeEmailAsync(string toEmail, string bookingReference, string qrData)
        {
            var qrBytes = GenerateQrCodeBytes(qrData);
            var base64Qr = Convert.ToBase64String(qrBytes);
            var base64ImageSrc = $"data:image/png;base64,{base64Qr}";

            var smtpClient = new SmtpClient(_smtpHost)
            {
                Port = _smtpPort,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUsername, "Prestige Rentals"),
                Subject = "Your Booking Confirmation - Prestige Rentals",
                Body = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px;'>
                <h2>Booking Confirmed</h2>
                <p>Your booking reference is: <strong>{bookingReference}</strong></p>
                <p>Please scan the QR code below at the vehicle pickup location:</p>
                <img src='{base64ImageSrc}' alt='QR Code' style='margin-top: 10px;' width='200'/>
                <p style='color: gray; font-size: 12px;'>Do not share this code with anyone.</p>
            </div>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }

        /// <summary>
        /// Sends a contact form submission to the admin email.
        /// </summary>
        /// <param name="userEmail">The email of the user who submitted the form.</param>
        /// <param name="message">The message content.</param>
        public async Task SendContactFormTicketToAdminAsync(string userEmail, string message)
        {
            string body = $@"
        <div style='font-family: Arial, sans-serif; padding: 20px;'>
            <h2>New Contact Form Submission</h2>
            <p><strong>From:</strong> {userEmail}</p>
            <p><strong>Message:</strong></p>
            <blockquote style='background:#f9f9f9; padding:10px; border-left: 4px solid #ccc;'>{message}</blockquote>
        </div>";

            await SendEmailAsync(_adminEmail, $"[Contact Form] New Submission", body);
        }

        /// <summary>
        /// Notifies the admin when a new review is submitted for a vehicle.
        /// </summary>
        /// <param name="userEmail">The email of the user who submitted the review.</param>
        /// <param name="vehicleId">The ID of the reviewed vehicle.</param>
        /// <param name="rating">The rating provided by the user.</param>
        /// <param name="review">The textual review content.</param>
        public async Task SendReviewNotificationToAdminAsync(string userEmail, long vehicleId, int rating, string review)
        {
            string body = $@"
        <div style='font-family: Arial, sans-serif; padding: 20px;'>
            <h2>New Review Submitted</h2>
            <p><strong>User:</strong> ({userEmail})</p>
            <p><strong>Vehicle ID:</strong> {vehicleId}</p>
            <p><strong>Rating:</strong> {rating}/5</p>
            <p><strong>Review:</strong></p>
            <blockquote style='background:#f9f9f9; padding:10px; border-left: 4px solid #ccc;'>{review}</blockquote>
        </div>";

            await SendEmailAsync(_adminEmail, $"[Review] Vehicle #{vehicleId} rated {rating}/5", body);
        }

        /// <summary>
        /// Notifies the admin of changes to a vehicle's status (e.g., added, updated, deleted).
        /// </summary>
        /// <param name="vehicleId">The vehicle's ID.</param>
        /// <param name="action">The type of action (e.g., "added", "updated", "deleted").</param>
        public async Task SendVehicleChangeNotificationToAdminAsync(long vehicleId, string action)
        {
            string body = $@"
        <div style='font-family: Arial, sans-serif; padding: 20px;'>
            <h2>Vehicle {action.ToUpper()} Notification</h2>
            <p>A vehicle was <strong>{action}</strong>.</p>
            <p><strong>Vehicle ID:</strong> {vehicleId}</p>
        </div>";

            await SendEmailAsync(_adminEmail, $"[Vehicle {action}] Vehicle ID #{vehicleId}", body);
        }

        /// <summary>
        /// Notifies the admin when a user's account status changes (e.g., banned, reactivated).
        /// </summary>
        /// <param name="affectedUserEmail">The email of the user whose status changed.</param>
        /// <param name="newStatus">The new status (e.g., "banned", "active").</param>
        public async Task SendUserStatusChangeNotificationToAdminAsync(string affectedUserEmail, string newStatus)
        {
            string body = $@"
        <div style='font-family: Arial, sans-serif; padding: 20px;'>
            <h2>User Status Changed</h2>
            <p><strong>Affected User:</strong> {affectedUserEmail}</p>
            <p><strong>New Status:</strong> {newStatus}</p>
        </div>";

            await SendEmailAsync(_adminEmail, $"[User Status Update] {affectedUserEmail} → {newStatus}", body);
        }

        /// <summary>
        /// Sends an email asking the user to leave a review for a recently completed rental.
        /// </summary>
        /// <param name="userEmail">The email address of the user.</param>
        /// <param name="vehicleName">The name of the rented vehicle.</param>
        /// <param name="orderId">The ID of the completed order.</param>
        public async Task SendReviewRequestEmailAsync(string userEmail, string vehicleName, long orderId)
        {
            var reviewLink = $"localhost:4200/review?orderId={orderId}";

            string body = $@"
        <div style='font-family: Arial, sans-serif; padding: 20px;'>
            <h2>How was your ride with the {vehicleName}?</h2>
            <p>Your trip has ended. We'd love to hear your feedback.</p>
            <a href='{reviewLink}' style='
                display: inline-block;
                margin-top: 10px;
                padding: 10px 20px;
                background-color: #007bff;
                color: #fff;
                text-decoration: none;
                border-radius: 5px;'>Leave a Review</a>
        </div>";

            await SendEmailAsync(userEmail, "We'd love your feedback!", body);
        }

        /// <summary>
        /// Sends an HTML email to a user notifying them that their account has been banned.
        /// </summary>
        /// <param name="userEmail">The recipient's email address.</param>
        public async Task SendUserBannedEmailAsync(string userEmail)
        {
            var subject = "Your PrestigeRentals Account Has Been Suspended";

            string body = $@"
    <div style='font-family: Arial, sans-serif; padding: 20px;'>
        <h2>Account Suspended</h2>
        <p>We regret to inform you that your account on <strong>PrestigeRentals</strong> has been <span style='color: red; font-weight: bold;'>permanently suspended</span> due to a violation of our Terms of Service.</p>
        <p>If you believe this action was made in error or wish to appeal the suspension, please contact our support team as soon as possible.</p>
        <p style='color: gray; font-size: 12px;'>Do not reply directly to this email. This mailbox is not monitored.</p>
    </div>";

            await SendEmailAsync(userEmail, subject, body);
        }


        /// <summary>
        /// Sends an HTML email to a user notifying them that their account has been reactivated (unbanned).
        /// </summary>
        /// <param name="userEmail">The recipient's email address.</param>
        public async Task SendUserUnbannedEmailAsync(string userEmail)
        {
            var subject = "Your PrestigeRentals Account Has Been Reactivated";

            string body = $@"
    <div style='font-family: Arial, sans-serif; padding: 20px;'>
        <h2>Account Reactivated</h2>
        <p>Good news! Your account on <strong>PrestigeRentals</strong> has been <span style='color: green; font-weight: bold;'>reinstated</span> and is now active again.</p>
        <p>You can now log in and continue using our platform as usual. If you experience any issues, feel free to reach out to our support team.</p>
        <p style='color: gray; font-size: 12px;'>Please do not reply to this email. This mailbox is not monitored.</p>
    </div>";

            await SendEmailAsync(userEmail, subject, body);
        }
    }
}
