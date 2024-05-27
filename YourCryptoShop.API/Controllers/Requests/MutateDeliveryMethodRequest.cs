using YourCryptoShop.Domain.Models;

namespace YourCryptoShop.API.Controllers.Requests
{
    public class MutateDeliveryMethodRequest
    {
        public required DeliveryMethod DeliveryMethod { get; set; }
        public required Dictionary<Guid, decimal?> CostsPerCountry { get; set; }
    }
}