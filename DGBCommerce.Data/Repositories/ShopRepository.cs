using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class ShopRepository : IShopRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public ShopRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public async Task<IEnumerable<Shop>> Get(GetShopsParameters parameters)
            => await GetRaw(parameters);

        public async Task<IEnumerable<PublicShop>> GetPublic(GetShopsParameters parameters)
            => await GetRawPublic(parameters);

        public async Task<Shop?> GetById(Guid merchantId, Guid id)
        {
            var shops = await GetRaw(new GetShopsParameters() { MerchantId = merchantId, Id = id });
            return shops.ToList().SingleOrDefault();
        }

        public Task<MutationResult> Create(Shop item, Guid mutationId)
            => _dataAccessLayer.CreateShop(item, mutationId);

        public Task<MutationResult> Update(Shop item, Guid mutationId)
            => _dataAccessLayer.UpdateShop(item, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => _dataAccessLayer.DeleteShop(id, mutationId);

        private async Task<IEnumerable<Shop>> GetRaw(GetShopsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetShops(parameters);
            List<Shop> shops = new();

            foreach (DataRow row in table.Rows)
            {
                shops.Add(new Shop()
                {
                    Id = new Guid(row["shp_id"].ToString()!),
                    Name = Utilities.DbNullableString(row["shp_name"]),
                    MerchantId = new Guid(row["shp_merchant"].ToString()!),
                    SubDomain = Utilities.DbNullableString(row["shp_subdomain"]),
                    Featured = Convert.ToBoolean(row["shp_featured"])
                });
            }

            return shops;
        }

        private async Task<IEnumerable<PublicShop>> GetRawPublic(GetShopsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetShops(parameters);
            List<PublicShop> shops = new();

            foreach (DataRow row in table.Rows)
            {                
                shops.Add(new PublicShop()
                {
                    Id = new Guid(row["shp_id"].ToString()!),
                    Name = Utilities.DbNullableString(row["shp_name"]),
                    MerchantId = new Guid(row["shp_merchant"].ToString()!),
                    MerchantUsername = Utilities.DbNullableString(row["shp_merchant_username"]),
                    MerchantScore = Utilities.DbNullableDecimal(row["shp_merchant_score"]),
                    SubDomain = Utilities.DbNullableString(row["shp_subdomain"]),
                    Featured = Convert.ToBoolean(row["shp_featured"])
                });
            }

            return shops;
        }
    }
}
