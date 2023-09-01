namespace DGBCommerce.Domain.Parameters
{
    public class GetShopsParameters : GetParameters
    {
        public required Guid MerchantId { get; set; }
        public Guid? Id { get; set; }        
        public string? Name { get; set; }
        public string? SubDomain { get; set; }
    }
}