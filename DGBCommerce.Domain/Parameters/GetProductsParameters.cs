namespace DGBCommerce.Domain.Parameters
{
    public class GetProductsParameters : GetParameters
    {
        public Guid? MerchantId { get; set; }
        public Guid? Id { get; set; }
        public Guid? ShopId { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool? Visible { get; set; }
        public bool? ShowOnHome { get; set; }
    }
}