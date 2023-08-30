using DGBCommerce.Domain.Models;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace DGBCommerce.API.Services
{
    public interface IMailService
    {
        void SendMail(string recipient, string subject, string body);
        void SendMail(List<string> recipients, string subject, string body);
    }

    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;

            if (string.IsNullOrEmpty(_mailSettings.Password))
                throw new Exception("MailSettings Password not configured.");

            if (!_mailSettings.Port.HasValue)
                throw new Exception("MailSettings Port not configured.");

            if (string.IsNullOrEmpty(_mailSettings.SenderEmailAddress))
                throw new Exception("MailSettings SenderEmailAddress not configured.");

            if (string.IsNullOrEmpty(_mailSettings.SmtpServer))
                throw new Exception("MailSettings SmtpServer not configured.");

            if (string.IsNullOrEmpty(_mailSettings.Username))
                throw new Exception("MailSettings Username not configured.");
        }

        public void SendMail(string recipient, string subject, string body)
            => SendMail(new List<string>() { recipient }, subject, body);

        public void SendMail(List<string> recipients, string subject, string body)
        {
            SmtpClient smtpClient = new()
            {
                Credentials = new NetworkCredential(_mailSettings.Username, _mailSettings.Password),
                EnableSsl = true,
                Host = _mailSettings.SmtpServer!,
                Port = _mailSettings.Port!.Value                
            };

            MailMessage mailMessage = new()
            {
                From = new MailAddress(_mailSettings.SenderEmailAddress!),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            foreach (string recipient in recipients)
                mailMessage.To.Add(recipient);

            smtpClient.Send(mailMessage);
        }
    }
}