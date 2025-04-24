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
                From = new MailAddress(_smtpUsername),
                Subject = "Email Verification",
                Body = $"Your verification code is: {verificationCode}",
                IsBodyHtml = false
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);

        }
    }
}
