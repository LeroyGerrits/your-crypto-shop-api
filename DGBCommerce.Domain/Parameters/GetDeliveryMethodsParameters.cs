namespace DGBCommerce.Domain.Parameters
{
    public class GetDeliveryMethodsParameters
    {
        public Guid? Id { get; set; }
        public Guid? ShopId { get; set; }
        public string? Name { get; set; }
    }
}