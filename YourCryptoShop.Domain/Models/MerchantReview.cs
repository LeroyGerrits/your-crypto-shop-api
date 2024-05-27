namespace YourCryptoShop.Domain.Models
{
    public class MerchantReview
    {
        public Guid? Id { get; set; }
        public required Guid MerchantId { get; set; }
        public required Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public required int Score { get; set; }
        public string? Review { get; set; }
        public required bool Anonymous { get; set; }
    }
}