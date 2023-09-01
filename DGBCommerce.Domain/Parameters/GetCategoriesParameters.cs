namespace DGBCommerce.Domain.Parameters
{
    public class GetCategoriesParameters : GetParameters
    {
        public required Guid MerchantId { get; set; }
        public Guid? Id { get; set; }
        public Guid? ShopId { get; set; }        
        public Guid? ParentId { get; set; }
        public string? Name { get; set; }
    }
}