using Application.Interfaces;
using Domain.Contracts.Abstracts.Account;
using Domain.Contracts.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendEmail(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(mailRequest.Email));
            email.Subject = mailRequest.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.EmailBody;
            email.Body = builder.ToMessageBody();

            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
                await smtp.SendAsync(email);

                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log or handle the error appropriately
                throw new Exception($"Failed to send email: {ex.Message}", ex);
            }
        }


        public async Task SendOtpMail(string name, string otpText, string email)
        {
            var mailRequest = new MailRequest();
            mailRequest.Email = email;
            mailRequest.Subject = "Thank for registering : OTP";
            mailRequest.EmailBody = GenerateEmailBody(name, otpText);
            await SendEmail(mailRequest);
        }

        // Create OTP Text
        public string GenerateRandomNumber()
        {
            Random random = new Random();
            string randomo = random.Next(0, 1000000).ToString("D6");
            return randomo;
        }

        // Create Email Body
        private string GenerateEmailBody(string name, string otpText)
        {
            string email = string.Empty;
            email = "<div>";
            email += "<h1> Hi " + name + ", Thanks for registering</h1>";
            email += "<h2>This is your OTP: " + otpText + "</h2>";
            email += "</div>";

            return email;
        }
    }
}
