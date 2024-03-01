namespace DGBCommerce.Domain.Parameters
{
    public class GetProductPhotosParameters : GetParameters
    {
        public Guid? MerchantId { get; set; }
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public bool? Visible { get; set; }
    }
}