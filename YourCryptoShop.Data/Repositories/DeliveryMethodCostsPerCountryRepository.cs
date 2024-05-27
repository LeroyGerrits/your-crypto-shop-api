using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Interfaces;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using System.Data;

namespace YourCryptoShop.Data.Repositories
{
    public class DeliveryMethodCostsPerCountryRepository(IDataAccessLayer dataAccessLayer) : IDeliveryMethodCostsPerCountryRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<DeliveryMethodCostsPerCountry>> Get(GetDeliveryMethodCostsPerCountryParameters parameters)
            => await GetRaw(parameters);

        public Task<DeliveryMethodCostsPerCountry?> GetById(Guid merchantId, Guid id)
            => throw new InvalidOperationException("DeliveryMethodCostsPerCountry objects can not be retrieved by Id.");

        public Task<MutationResult> Create(DeliveryMethodCostsPerCountry item, Guid mutationId)
            => _dataAccessLayer.CreateDeliveryMethodCostsPerCountry(item, mutationId);

        public Task<MutationResult> Update(DeliveryMethodCostsPerCountry item, Guid mutationId)
            => throw new InvalidOperationException("DeliveryMethodCostsPerCountry objects can not be updated.");

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => throw new InvalidOperationException("DeliveryMethodCostsPerCountry objects can not be deleted by Id.");

        public Task<MutationResult> Delete(Guid productId, Guid categoryId, Guid mutationId)
            => _dataAccessLayer.DeleteDeliveryMethodCostsPerCountry(productId, categoryId, mutationId);

        private async Task<IEnumerable<DeliveryMethodCostsPerCountry>> GetRaw(GetDeliveryMethodCostsPerCountryParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetDeliveryMethodCostsPerCountry(parameters);
            List<DeliveryMethodCostsPerCountry> productCategories = [];

            foreach (DataRow row in table.Rows)
            {
                productCategories.Add(new DeliveryMethodCostsPerCountry()
                {
                    DeliveryMethodId = new Guid(row["cpc_delivery_method"].ToString()!),
                    CountryId = new Guid(row["cpc_country"].ToString()!),
                    Costs = Convert.ToDecimal(row["cpc_costs"])
                });
            }

            return productCategories;
        }
    }
}