using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Enums;
using YourCryptoShop.Domain.Interfaces;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Parameters;
using System.Data;

namespace YourCryptoShop.Data.Repositories
{
    public class ShopRepository(IDataAccessLayer dataAccessLayer) : IShopRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<Shop>> Get(GetShopsParameters parameters)
            => await GetRaw(parameters);

        public async Task<PublicShop?> GetByIdAndSubDomainPublic(Guid? id, string subDomain)
        {
            var shops = await GetRawByIdAndSubDomainPublic(id, subDomain);
            return shops.SingleOrDefault();
        }

        public async Task<PublicShop?> GetByIdPublic(Guid id)
        {
            var shops = await GetRawPublic(new GetShopsParameters() { Id = id });
            return shops.SingleOrDefault();
        }

        public async Task<PublicShop?> GetBySubDomainPublic(string subDomain)
        {
            var shops = await GetRawByIdAndSubDomainPublic(null, subDomain);
            return shops.SingleOrDefault();
        }

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
            List<Shop> shops = [];

            foreach (DataRow row in table.Rows)
            {
                Shop shop = new()
                {
                    Id = new Guid(row["shp_id"].ToString()!),
                    Name = Utilities.DbNullableString(row["shp_name"]),
                    MerchantId = new Guid(row["shp_merchant"].ToString()!),
                    SubDomain = Utilities.DbNullableString(row["shp_subdomain"]),
                    OrderMethod = (ShopOrderMethod)Convert.ToInt32(row["shp_order_method"]),
                    RequireAddresses = Convert.ToBoolean(row["shp_require_addresses"]),
                    Featured = Convert.ToBoolean(row["shp_featured"])
                };

                if (row["shp_country"] != DBNull.Value)
                {
                    shop.Country = new Country()
                    {
                        Id = Utilities.DbNullableGuid(row["shp_country"]),
                        Code = Utilities.DbNullableString(row["shp_country_code"]),
                        Name = Utilities.DbNullableString(row["shp_country_name"])
                    };
                }

                if (row["shp_category"] != DBNull.Value)
                {
                    shop.Category = new ShopCategory()
                    {
                        Id = Utilities.DbNullableGuid(row["shp_category"]),
                        Name = Utilities.DbNullableString(row["shp_category_name"])
                    };
                }

                if (row["shp_wallet"] != DBNull.Value)
                {
                    shop.Wallet = new CryptoWallet()
                    {
                        Id = Utilities.DbNullableGuid(row["shp_wallet"]),
                        MerchantId = new Guid(row["shp_merchant"].ToString()!),
                        CurrencyId = new Guid(row["shp_wallet_currency"].ToString()!),
                        Name = Utilities.DbNullableString(row["shp_wallet_name"]),
                        Address = Utilities.DbNullableString(row["shp_wallet_address"])
                    };
                }

                shops.Add(shop);
            }

            return shops;
        }

        private async Task<IEnumerable<PublicShop>> GetRawPublic(GetShopsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetShops(parameters);
            List<PublicShop> shops = [];

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
                    CountryId = Utilities.DbNullableGuid(row["shp_country"]),
                    CountryCode = Utilities.DbNullableString(row["shp_country_code"]),
                    CountryName = Utilities.DbNullableString(row["shp_country_name"]),
                    CategoryId = Utilities.DbNullableGuid(row["shp_category"]),
                    CategoryName = Utilities.DbNullableString(row["shp_category_name"]),
                    HasWallet = (row["shp_wallet"] != DBNull.Value),
                    OrderMethod = (ShopOrderMethod)Convert.ToInt32(row["shp_order_method"]),
                    RequireAddresses = Convert.ToBoolean(row["shp_require_addresses"]),
                    Featured = Convert.ToBoolean(row["shp_featured"])
                });
            }

            return shops;
        }

        private async Task<IEnumerable<PublicShop>> GetRawByIdAndSubDomainPublic(Guid? id, string subDomain)
        {
            DataTable table = await _dataAccessLayer.GetShopByIdAndSubDomain(id, subDomain);
            List<PublicShop> shops = [];

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
                    CountryId = Utilities.DbNullableGuid(row["shp_country"]),
                    CountryCode = Utilities.DbNullableString(row["shp_country_code"]),
                    CountryName = Utilities.DbNullableString(row["shp_country_name"]),
                    CategoryId = Utilities.DbNullableGuid(row["shp_category"]),
                    CategoryName = Utilities.DbNullableString(row["shp_category_name"]),                    
                    HasWallet = (row["shp_wallet"] != DBNull.Value),
                    OrderMethod = (ShopOrderMethod)Convert.ToInt32(row["shp_order_method"]),
                    RequireAddresses = Convert.ToBoolean(row["shp_require_addresses"]),
                    Featured = Convert.ToBoolean(row["shp_featured"])
                });
            }

            return shops;
        }
    }
}
