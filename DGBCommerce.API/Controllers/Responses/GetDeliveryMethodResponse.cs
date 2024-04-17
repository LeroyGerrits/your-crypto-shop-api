using DGBCommerce.Domain.Models;

namespace DGBCommerce.API.Controllers.Responses
{
    public class GetDeliveryMethodResponse(DeliveryMethod deliveryMethod, Dictionary<Guid, decimal> costsPerCountry)
    {
        public DeliveryMethod DeliveryMethod { get; set; } = deliveryMethod;
        public Dictionary<Guid, decimal> CostsPerCountry { get; set; } = costsPerCountry;
    }
}