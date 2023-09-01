namespace DGBCommerce.Domain.Parameters
{
    public class GetDeliveryMethodsParameters : GetParameters
    {
        public required Guid MerchantId { get; set; }
        public Guid? Id { get; set; }        
        public Guid? ShopId { get; set; }
        public string? Name { get; set; }
    }
}