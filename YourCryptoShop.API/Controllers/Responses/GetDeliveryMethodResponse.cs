using YourCryptoShop.Domain.Models;

namespace YourCryptoShop.API.Controllers.Responses
{
    public class GetDeliveryMethodResponse(DeliveryMethod deliveryMethod, Dictionary<Guid, decimal> costsPerCountry)
    {
        public DeliveryMethod DeliveryMethod { get; set; } = deliveryMethod;
        public Dictionary<Guid, decimal> CostsPerCountry { get; set; } = costsPerCountry;
    }
}