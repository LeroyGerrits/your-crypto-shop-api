namespace DGBCommerce.Domain.Parameters
{
    public class GetProductCategoriesParameters : GetParameters
    {
        public required Guid MerchantId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? CategoryId { get; set; }
    }
}