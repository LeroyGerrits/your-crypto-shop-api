namespace YourCryptoShop.API
{
    public class MailSettings
    {
        public string? Password { get; set; }
        public int? Port { get; set; }
        public string? SenderName { get; set; }
        public string? SenderEmailAddress { get; set; }
        public string? SmtpServer { get; set; }
        public string? Username { get; set; }
    }
}
