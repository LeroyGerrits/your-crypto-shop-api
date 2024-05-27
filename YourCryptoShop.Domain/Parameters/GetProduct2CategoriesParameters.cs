namespace YourCryptoShop.Domain.Parameters
{
    public class GetProduct2CategoriesParameters : GetParameters
    {
        public required Guid MerchantId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? CategoryId { get; set; }
    }
}