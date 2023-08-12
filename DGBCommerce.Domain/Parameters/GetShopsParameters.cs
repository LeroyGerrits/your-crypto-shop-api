namespace DGBCommerce.Domain.Parameters
{
    public class GetShopsParameters
    {
        public Guid? Id { get; set; }
        public Guid? MerchantId { get; set; }
        public string? Name { get; set; }
        public string? SubDomain { get; set; }
    }
}