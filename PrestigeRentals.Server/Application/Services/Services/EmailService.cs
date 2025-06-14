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
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost = "smtp.ethereal.email";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "garret.hauck36@ethereal.email";
        private readonly string _smtpPassword = "83dKdH1D92st8MVMEM";
        private readonly string _adminEmail = "PrestigeRentalsRO@gmail.com";

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

        private byte[] GenerateQrCodeBytes(string data)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

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

        public async Task SendUserStatusChangeNotificationToAdminAsync(long adminId, string affectedUserEmail, string newStatus)
        {
            string body = $@"
        <div style='font-family: Arial, sans-serif; padding: 20px;'>
            <h2>User Status Changed</h2>
            <p><strong>Admin ID:</strong> {adminId}</p>
            <p><strong>Affected User:</strong> {affectedUserEmail}</p>
            <p><strong>New Status:</strong> {newStatus}</p>
        </div>";

            await SendEmailAsync(_adminEmail, $"[User Status Update] {affectedUserEmail} → {newStatus}", body);
        }
    }
}
