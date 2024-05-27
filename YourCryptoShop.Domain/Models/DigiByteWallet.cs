namespace YourCryptoShop.Domain.Models
{
    public class DigiByteWallet
    {
        public Guid? Id { get; set; }
        public required Guid MerchantId { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
    }
}