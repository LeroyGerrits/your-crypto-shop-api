namespace DGBCommerce.Domain.Parameters
{
    public class GetProductPhotosParameters : GetParameters
    {
        public required Guid MerchantId { get; set; }
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
    }
}