namespace YourCryptoShop.Domain.Parameters
{
    public class GetMerchantsParameters : GetParameters
    {
        public Guid? Id { get; set; }
        public string? EmailAddress { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}