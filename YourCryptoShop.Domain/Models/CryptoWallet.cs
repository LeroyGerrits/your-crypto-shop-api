namespace YourCryptoShop.Domain.Models
{
    public class CryptoWallet
    {
        public Guid? Id { get; set; }
        public required Guid MerchantId { get; set; }
        public required Guid CurrencyId { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
    }
}