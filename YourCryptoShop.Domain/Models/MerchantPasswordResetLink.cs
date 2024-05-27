namespace YourCryptoShop.Domain.Models
{
    public class MerchantPasswordResetLink
    {
        public Guid? Id { get; set; }
        public required Merchant Merchant { get; set; }
        public required DateTime Date { get; set; } = DateTime.UtcNow;
        public string? IpAddress { get; set; }
        public required string Key { get; set; }
        public DateTime? Used { get; set; }
    }
}