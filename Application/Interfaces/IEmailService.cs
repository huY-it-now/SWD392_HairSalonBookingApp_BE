using Domain.Contracts.Abstracts.Account;

namespace Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(MailRequest mailRequest);
        Task SendOtpMail(string name, string otpText, string email);
        string GenerateRandomNumber();
    }
}
