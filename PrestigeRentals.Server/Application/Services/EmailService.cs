using PrestigeRentals.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost = "smtp.mailersend.net";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "MS_SjKDQR@test-2p0347zzkd7lzdrn.mlsender.net";
        private readonly string _smtpPassword = "mssp.cNmLtF8.pq3enl60r30l2vwr.blQM3cj";

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
    }
}
