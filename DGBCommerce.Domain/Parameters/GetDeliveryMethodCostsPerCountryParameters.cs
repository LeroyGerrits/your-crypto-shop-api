namespace DGBCommerce.Domain.Parameters
{
    public class GetDeliveryMethodCostsPerCountryParameters : GetParameters
    {
        public Guid? DeliveryMethodId { get; set; }
        public Guid? CountryId { get; set; }
    }
}