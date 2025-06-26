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
            string subject = "Verify Your Email - Prestige Rentals";

            string body = $@"
<div style='
  background-color: #1e1e1e;
  padding: 30px;
  font-family: Arial, sans-serif;
  color: #f1f1f1;
  max-width: 600px;
  margin: auto;
  border-radius: 10px;'>

  <div style='text-align: center; margin-bottom: 30px;'>
    <img src='https://i.imgur.com/F3bdvC8.png' alt='Prestige Rentals Logo' style='max-width: 150px; height: auto;' />
  </div>

  <h2 style='text-align: center; font-size: 24px;'>Email Verification</h2>

  <p style='font-size: 16px; line-height: 1.6; text-align: center;'>
    Your one-time verification code is:
  </p>

  <p style='
    text-align: center;
    font-size: 24px;
    font-weight: bold;
    color: #f7c566;
    background-color: #2c2c2c;
    padding: 12px 24px;
    border-radius: 8px;
    display: inline-block;
    margin: 20px auto;'>
    {verificationCode}
  </p>

  <p style='font-size: 14px; color: #B0BEC5; text-align: center;'>
    Please enter this code to verify your email address. It will expire soon.
  </p>

  <hr style='margin: 30px 0; border-color: #f7c566;' />

  <p style='font-size: 12px; color: #f1f1f1; text-align: center;'>
    If you didn’t request this, you can safely ignore this email.
  </p>
</div>";

            await SendEmailAsync(toEmail, subject, body);
        }


        /// <summary>
        /// Sends an email to the user containing their newly generated password.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="newPassword">The new password to include in the email.</param>
        public async Task SendNewPasswordEmailAsync(string toEmail, string newPassword)
        {
            string subject = "Your New Password - Prestige Rentals";

            string body = $@"
<div style='
  background-color: #1e1e1e;
  padding: 30px;
  font-family: Arial, sans-serif;
  color: #f1f1f1;
  max-width: 600px;
  margin: auto;
  border-radius: 10px;'>

  <div style='text-align: center; margin-bottom: 30px;'>
    <img src='https://i.imgur.com/F3bdvC8.png' alt='Prestige Rentals Logo' style='max-width: 150px; height: auto;' />
  </div>

  <h2 style='text-align: center; font-size: 24px;'>Password Reset</h2>

  <p style='font-size: 16px; line-height: 1.6; text-align: center;'>
    Your new temporary password is:
  </p>

  <p style='
    text-align: center;
    font-size: 20px;
    font-weight: bold;
    color: #f7c566;
    background-color: #2c2c2c;
    padding: 10px 20px;
    border-radius: 8px;
    display: inline-block;
    margin: 20px auto;'>
    {newPassword}
  </p>

  <p style='font-size: 16px; text-align: center;'>
    Please log in using this password and make sure to change it after your first login.
  </p>

  <p style='font-size: 14px; color: #B0BEC5; text-align: center;'>
    If you didn’t request this, you can safely ignore this email.
  </p>

  <hr style='margin: 30px 0; border-color: #f7c566;' />

  <p style='font-size: 12px; color: #f1f1f1; text-align: center;'>
    For your security, never share your password with anyone.
  </p>
</div>";

            await SendEmailAsync(toEmail, subject, body);
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

            string subject = "Your Booking Confirmation - Prestige Rentals";

            string body = $@"
<div style='
  background-color: #1e1e1e;
  padding: 30px;
  font-family: Arial, sans-serif;
  color: #f1f1f1;
  max-width: 600px;
  margin: auto;
  border-radius: 10px;'>

  <div style='text-align: center; margin-bottom: 30px;'>
    <img src='https://i.imgur.com/F3bdvC8.png' alt='Prestige Rentals Logo' style='max-width: 150px; height: auto;' />
  </div>

  <h2 style='text-align: center; font-size: 24px;'>Booking Confirmed</h2>

  <p style='font-size: 16px; line-height: 1.6; text-align: center;'>
    Your booking reference is:<br/>
    <strong style='font-size: 20px; color: #f7c566;'>{bookingReference}</strong>
  </p>

  <p style='text-align: center; font-size: 16px;'>Please scan the QR code below at the vehicle pickup location:</p>

  <div style='text-align: center; margin: 20px 0;'>
    <img src='{base64ImageSrc}' alt='QR Code' style='margin-top: 10px; width: 200px;' />
  </div>

  <p style='color: #B0BEC5; font-size: 12px; text-align: center;'>
    Do not share this code with anyone. It grants access to your booking.
  </p>

  <hr style='margin: 30px 0; border-color: #f7c566;' />

  <p style='font-size: 12px; color: #f1f1f1; text-align: center;'>
    Thank you for choosing PrestigeRentals!
  </p>
</div>";

            await SendEmailAsync(toEmail, subject, body);
        }


        /// <summary>
        /// Sends a contact form submission to the admin email.
        /// </summary>
        /// <param name="userEmail">The email of the user who submitted the form.</param>
        /// <param name="message">The message content.</param>
        public async Task SendContactFormTicketToAdminAsync(string userEmail, string message)
        {
            string subject = $"[Contact Form] New Submission";

            string body = $@"
<div style='
  background-color: #1e1e1e;
  padding: 30px;
  font-family: Arial, sans-serif;
  color: #f1f1f1;
  max-width: 600px;
  margin: auto;
  border-radius: 10px;'>

  <div style='text-align: center; margin-bottom: 30px;'>
    <img src='https://i.imgur.com/F3bdvC8.png' alt='Prestige Rentals Logo' style='max-width: 150px; height: auto;' />
  </div>

  <h2 style='text-align: center; font-size: 24px;'>New Contact Form Submission</h2>

  <p style='font-size: 16px;'>
    <strong>From:</strong> {userEmail}
  </p>

  <div style='margin: 20px 0; background: #2c2c2c; padding: 15px; border-left: 4px solid #f7c566; border-radius: 6px;'>
    <p style='margin: 0;'>{message}</p>
  </div>

  <hr style='margin: 30px 0; border-color: #f7c566;' />

  <p style='font-size: 12px; color: #f1f1f1; text-align: center;'>
    This message was submitted through the contact form on the platform.
  </p>
</div>";

            await SendEmailAsync(_adminEmail, subject, body);
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
            string subject = $"[Review] Vehicle #{vehicleId} rated {rating}/5";

            string body = $@"
<div style='
  background-color: #1e1e1e;
  padding: 30px;
  font-family: Arial, sans-serif;
  color: #f1f1f1;
  max-width: 600px;
  margin: auto;
  border-radius: 10px;'>

  <div style='text-align: center; margin-bottom: 30px;'>
    <img src='https://i.imgur.com/F3bdvC8.png' alt='Prestige Rentals Logo' style='max-width: 150px; height: auto;' />
  </div>

  <h2 style='text-align: center; font-size: 24px;'>New Review Submitted</h2>

  <p style='font-size: 16px;'>
    <strong>User:</strong> {userEmail}<br/>
    <strong>Vehicle ID:</strong> #{vehicleId}<br/>
    <strong>Rating:</strong> {rating}/5
  </p>

  <div style='margin: 20px 0; background: #2c2c2c; padding: 15px; border-left: 4px solid #f7c566; border-radius: 6px;'>
    <p style='font-style: italic; margin: 0;'>{review}</p>
  </div>

  <hr style='margin: 30px 0; border-color: #f7c566;' />

  <p style='font-size: 12px; color: #f1f1f1; text-align: center;'>
    This is an automated message from the PrestigeRentals review system.
  </p>
</div>";

            await SendEmailAsync(_adminEmail, subject, body);
        }


        /// <summary>
        /// Notifies the admin of changes to a vehicle's status (e.g., added, updated, deleted).
        /// </summary>
        /// <param name="vehicleId">The vehicle's ID.</param>
        /// <param name="action">The type of action (e.g., "added", "updated", "deleted").</param>
        public async Task SendVehicleChangeNotificationToAdminAsync(long vehicleId, string action)
        {
            string subject = $"[Vehicle {action}] Vehicle ID #{vehicleId}";

            string body = $@"
<div style='
  background-color: #1e1e1e;
  padding: 30px;
  font-family: Arial, sans-serif;
  color: #f1f1f1;
  max-width: 600px;
  margin: auto;
  border-radius: 10px;'>

  <div style='text-align: center; margin-bottom: 30px;'>
    <img src='https://i.imgur.com/F3bdvC8.png' alt='Prestige Rentals Logo' style='max-width: 150px; height: auto;' />
  </div>

  <h2 style='text-align: center; font-size: 24px;'>Vehicle {action.ToUpper()} Notification</h2>

  <p style='font-size: 16px; line-height: 1.6;'>
    A vehicle has been <strong>{action.ToLower()}</strong> in the system.
  </p>

  <p style='font-size: 16px;'>
    <strong>Vehicle ID:</strong> #{vehicleId}
  </p>

  <hr style='margin: 30px 0; border-color: #f7c566;' />

  <p style='font-size: 12px; color: #f1f1f1; text-align: center;'>
    This is an automated update for administrative purposes.
  </p>
</div>";

            await SendEmailAsync(_adminEmail, subject, body);
        }


        /// <summary>
        /// Notifies the admin when a user's account status changes (e.g., banned, reactivated).
        /// </summary>
        /// <param name="affectedUserEmail">The email of the user whose status changed.</param>
        /// <param name="newStatus">The new status (e.g., "banned", "active").</param>
        public async Task SendUserStatusChangeNotificationToAdminAsync(string affectedUserEmail, string newStatus)
        {
            string subject = $"[User Status Update] {affectedUserEmail} → {newStatus}";

            string body = $@"
<div style='
  background-color: #1e1e1e;
  padding: 30px;
  font-family: Arial, sans-serif;
  color: #f1f1f1;
  max-width: 600px;
  margin: auto;
  border-radius: 10px;'>

  <div style='text-align: center; margin-bottom: 30px;'>
    <img src='https://i.imgur.com/F3bdvC8.png' alt='Prestige Rentals Logo' style='max-width: 150px; height: auto;' />
  </div>

  <h2 style='text-align: center; font-size: 24px;'>User Status Changed</h2>

  <p style='font-size: 16px; line-height: 1.6;'>
    An account status change has been recorded in the system.
  </p>

  <p style='font-size: 16px;'>
    <strong>Affected User:</strong> {affectedUserEmail}<br/>
    <strong>New Status:</strong> <span style='color: #f7c566;'>{newStatus.ToUpperInvariant()}</span>
  </p>

  <hr style='margin: 30px 0; border-color: #f7c566;' />

  <p style='font-size: 12px; color: #f1f1f1; text-align: center;'>
    This is an automated notification sent to the administrative team.
  </p>
</div>";

            await SendEmailAsync(_adminEmail, subject, body);
        }


        /// <summary>
        /// Sends an email asking the user to leave a review for a recently completed rental.
        /// </summary>
        /// <param name="userEmail">The email address of the user.</param>
        /// <param name="vehicleName">The name of the rented vehicle.</param>
        /// <param name="orderId">The ID of the completed order.</param>
        public async Task SendReviewRequestEmailAsync(string userEmail, string vehicleName, long orderId)
        {
            var reviewLink = $"https://localhost:4200/review?orderId={orderId}";

            string body = $@"
<div style='
  background-color: #1e1e1e;
  padding: 30px;
  font-family: Arial, sans-serif;
  color: #f1f1f1;
  max-width: 600px;
  margin: auto;
  border-radius: 10px;'>

  <div style='text-align: center; margin-bottom: 30px;'>
    <img src='https://i.imgur.com/F3bdvC8.png' alt='Prestige Rentals Logo' style='max-width: 150px; height: auto;' />
  </div>

  <h2 style='text-align: center; font-size: 24px;'>How was your ride with the {vehicleName}?</h2>

  <p style='font-size: 16px; line-height: 1.6;'>
    Your recent rental has ended and we'd love to hear about your experience. Your feedback helps us improve and helps other users choose the best ride!
  </p>

  <div style='text-align: center; margin-top: 30px;'>
    <a href='{reviewLink}' style='
      display: inline-block;
      padding: 12px 24px;
      background-color: #f7c566;
      color: #1e1e1e;
      font-weight: bold;
      text-decoration: none;
      border-radius: 6px;'>Leave a Review</a>
  </div>

  <hr style='margin: 40px 0; border-color: #f7c566;' />

  <p style='font-size: 12px; color: #f1f1f1; text-align: center;'>
    Thank you for using PrestigeRentals. We appreciate your feedback!
  </p>
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
<div style='
  background-color: #1e1e1e;
  padding: 30px;
  font-family: Arial, sans-serif;
  color: #f1f1f1;
  max-width: 600px;
  margin: auto;
  border-radius: 10px;'>

  <div style='text-align: center; margin-bottom: 30px;'>
    <img src='https://i.imgur.com/F3bdvC8.png' alt='Prestige Rentals Logo' style='max-width: 150px; height: auto;' />
  </div>

  <h2 style='text-align: center; font-size: 24px;'>Account Suspended</h2>

  <p style='font-size: 16px; line-height: 1.6;'>
    We regret to inform you that your account on <strong>PrestigeRentals</strong> has been 
    <span style='color: #FFCDD2; font-weight: bold;'>permanently suspended</span> due to a violation of our Terms of Service.
  </p>

  <p style='font-size: 16px; line-height: 1.6;'>
    If you believe this was done in error or wish to appeal, please contact our support team as soon as possible.
  </p>

  <hr style='margin: 30px 0; border-color: #f7c566;' />

  <p style='font-size: 12px; color: #f1f1f1; text-align: center;'>
    Please do not reply to this email. This mailbox is not monitored.
  </p>
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
<div style='
  background-color: #1e1e1e;
  padding: 30px;
  font-family: Arial, sans-serif;
  color: #f1f1f1;
  max-width: 600px;
  margin: auto;
  border-radius: 10px;'>

  <div style='text-align: center; margin-bottom: 30px;'>
    <img src='https://i.imgur.com/F3bdvC8.png' alt='Prestige Rentals Logo' style='max-width: 150px; height: auto;' />
  </div>

  <h2 style='text-align: center; font-size: 24px;'>Account Reactivated</h2>

  <p style='font-size: 16px; line-height: 1.6;'>
    Good news! Your account on <strong>PrestigeRentals</strong> has been 
    <span style='color: #A5D6A7; font-weight: bold;'>successfully reinstated</span> and is now active again.
  </p>

  <p style='font-size: 16px; line-height: 1.6;'>
    You can now log in and continue using our platform as usual. If you experience any issues, feel free to contact our support team.
  </p>

  <hr style='margin: 30px 0; border-color: #f7c566;' />

  <p style='font-size: 12px; color: #f1f1f1; text-align: center;'>
    Please do not reply to this email. This mailbox is not monitored.
  </p>
</div>";

            await SendEmailAsync(userEmail, subject, body);
        }
    }
}
