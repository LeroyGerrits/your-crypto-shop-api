using DGBCommerce.Domain.Models;
using Microsoft.Extensions.Options;

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;

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
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmailAddress));            
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            foreach (string recipient in recipients)
                email.To.Add(MailboxAddress.Parse(recipient));

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.SmtpServer, _mailSettings.Port!.Value, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Username, _mailSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}