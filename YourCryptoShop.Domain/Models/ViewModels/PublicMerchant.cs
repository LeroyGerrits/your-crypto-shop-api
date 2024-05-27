namespace YourCryptoShop.Domain.Models.ViewModels
{
    public class PublicMerchant
    {
        public Guid? Id { get; set; }
        public required string Username { get; set; }
        public decimal? Score { get; set; }
    }
}