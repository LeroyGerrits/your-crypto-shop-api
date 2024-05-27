using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Interfaces;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Parameters;
using System.Data;

namespace YourCryptoShop.Data.Repositories
{
    public class DeliveryMethodRepository(IDataAccessLayer dataAccessLayer) : IDeliveryMethodRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<DeliveryMethod>> Get(GetDeliveryMethodsParameters parameters)
            => await GetRaw(parameters);

        public async Task<DeliveryMethod?> GetById(Guid merchantId, Guid id)
        {
            var deliveryMethods = await GetRaw(new GetDeliveryMethodsParameters() { MerchantId = merchantId, Id = id });
            return deliveryMethods.ToList().SingleOrDefault();
        }

        public async Task<PublicDeliveryMethod?> GetByIdPublic(Guid id)
        {
            var categories = await GetRawPublic(new GetDeliveryMethodsParameters() { Id = id});
            return categories.ToList().SingleOrDefault();
        }

        public async Task<IEnumerable<PublicDeliveryMethod>> GetByShopIdPublic(Guid shopId)
        {
            var categories = await GetRawPublic(new GetDeliveryMethodsParameters() { ShopId = shopId });
            return categories.ToList();
        }

        public Task<MutationResult> Create(DeliveryMethod item, Guid mutationId)
            => _dataAccessLayer.CreateDeliveryMethod(item, mutationId);

        public Task<MutationResult> Update(DeliveryMethod item, Guid mutationId)
            => _dataAccessLayer.UpdateDeliveryMethod(item, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => _dataAccessLayer.DeleteDeliveryMethod(id, mutationId);

        private async Task<IEnumerable<DeliveryMethod>> GetRaw(GetDeliveryMethodsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetDeliveryMethods(parameters);
            List<DeliveryMethod> deliveryMethods = [];

            foreach (DataRow row in table.Rows)
            {
                deliveryMethods.Add(new()
                {
                    Id = new Guid(row["dlm_id"].ToString()!),
                    Shop = new Shop()
                    {
                        Id = new Guid(row["dlm_shop"].ToString()!),
                        Name = Utilities.DbNullableString(row["dlm_shop_name"]),
                        MerchantId = new Guid(row["dlm_shop_merchant"].ToString()!)
                    },
                    Name = Utilities.DbNullableString(row["dlm_name"]),
                    Costs = Utilities.DbNullableDecimal(row["dlm_costs"])
                });
            }

            return deliveryMethods;
        }

        private async Task<IEnumerable<PublicDeliveryMethod>> GetRawPublic(GetDeliveryMethodsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetDeliveryMethods(parameters);
            List<PublicDeliveryMethod> deliveryMethods = [];

            foreach (DataRow row in table.Rows)
            {
                deliveryMethods.Add(new()
                {
                    Id = new Guid(row["dlm_id"].ToString()!),
                    Name = Utilities.DbNullableString(row["dlm_name"]),
                    Costs = Utilities.DbNullableDecimal(row["dlm_costs"])
                });
            }

            return deliveryMethods;
        }
    }
}