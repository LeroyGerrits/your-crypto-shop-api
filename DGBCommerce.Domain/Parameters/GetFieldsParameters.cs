using DGBCommerce.Domain.Enums;

namespace DGBCommerce.Domain.Parameters
{
    public class GetFieldsParameters : GetParameters
    {
        public Guid? MerchantId { get; set; }
        public Guid? Id { get; set; }        
        public Guid? ShopId { get; set; }
        public string? Name { get; set; }
        public FieldEntity? Entity { get; set; }
        public FieldType? Type { get; set; }
        public FieldDataType? DataType { get; set; }
        public bool? Visible { get; set; }
    }
}