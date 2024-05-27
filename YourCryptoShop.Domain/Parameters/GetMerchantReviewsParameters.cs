namespace YourCryptoShop.Domain.Parameters
{
    public class GetMerchantReviewsParameters : GetParameters
    {
        public required Guid MerchantId { get; set; }
        public Guid? Id { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateUntil { get; set; }
    }
}