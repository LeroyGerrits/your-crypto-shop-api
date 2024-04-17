using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IDeliveryMethodCostsPerCountryRepository : IMutableRepository<DeliveryMethodCostsPerCountry, GetDeliveryMethodCostsPerCountryParameters>
    {
        Task<MutationResult> Delete(Guid deliveryMethodId, Guid countryId, Guid mutationId);
    }
}