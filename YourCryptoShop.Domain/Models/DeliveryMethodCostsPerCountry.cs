namespace YourCryptoShop.Domain.Models
{
    public class DeliveryMethodCostsPerCountry
    {
        public required Guid DeliveryMethodId { get; set; }
        public required Guid CountryId { get; set; }
        public decimal Costs { get; set; }
    }
}