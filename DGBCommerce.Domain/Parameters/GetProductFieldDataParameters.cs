namespace DGBCommerce.Domain.Parameters
{
    public class GetProductFieldDataParameters : GetParameters
    {
        public Guid? MerchantId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? FieldId { get; set; }
        public bool? FieldVisible { get; set; }
    }
}