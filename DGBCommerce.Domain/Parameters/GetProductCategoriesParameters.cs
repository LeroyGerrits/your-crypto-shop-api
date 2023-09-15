namespace DGBCommerce.Domain.Parameters
{
    public class GetProductCategoriessParameters : GetParameters
    {
        public required Guid MerchantId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? CategoryId { get; set; }
    }
}