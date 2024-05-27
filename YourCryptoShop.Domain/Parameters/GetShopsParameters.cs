namespace YourCryptoShop.Domain.Parameters
{
    public class GetShopsParameters : GetParameters
    {
        public Guid? MerchantId { get; set; }
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? SubDomain { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? CategoryId { get; set; }
        public bool? Featured { get; set; }
        public bool? Usable { get; set; }
    }
}