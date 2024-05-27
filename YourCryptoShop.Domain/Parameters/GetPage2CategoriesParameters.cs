namespace YourCryptoShop.Domain.Parameters
{
    public class GetPage2CategoriesParameters : GetParameters
    {
        public required Guid MerchantId { get; set; }
        public Guid? PageId { get; set; }
        public Guid? CategoryId { get; set; }
    }
}