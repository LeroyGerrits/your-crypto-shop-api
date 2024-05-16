namespace DGBCommerce.Domain.Parameters
{
    public class GetProductFieldDataParameters : GetParameters
    {
        public required Guid MerchantId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? FieldId { get; set; }
    }
}