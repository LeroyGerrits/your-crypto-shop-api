namespace YourCryptoShop.Domain.Parameters
{
    public class GetCountriesParameters : GetParameters
    {
        public Guid? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}