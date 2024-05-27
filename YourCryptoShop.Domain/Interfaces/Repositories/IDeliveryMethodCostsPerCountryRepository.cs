using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IDeliveryMethodCostsPerCountryRepository : IMutableRepository<DeliveryMethodCostsPerCountry, GetDeliveryMethodCostsPerCountryParameters>
    {
        Task<MutationResult> Delete(Guid deliveryMethodId, Guid countryId, Guid mutationId);
    }
}