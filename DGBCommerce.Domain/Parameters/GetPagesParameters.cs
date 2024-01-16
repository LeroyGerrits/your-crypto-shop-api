namespace DGBCommerce.Domain.Parameters
{
    public class GetPagesParameters : GetParameters
    {
        public Guid? MerchantId { get; set; }
        public Guid? Id { get; set; }        
        public Guid? ShopId { get; set; }
        public string? Title { get; set; }
        public bool? Visible { get; set; }
    }
}