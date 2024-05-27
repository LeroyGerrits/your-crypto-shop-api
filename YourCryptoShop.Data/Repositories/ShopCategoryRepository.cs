using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Interfaces;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using System.Data;

namespace YourCryptoShop.Data.Repositories
{
    public class ShopCategoryRepository(IDataAccessLayer dataAccessLayer) : IShopCategoryRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<ShopCategory>> Get(GetShopCategoriesParameters parameters)
            => await GetRaw(parameters);

        public async Task<ShopCategory?> GetById(Guid id)
        {
            var shopCategories = await GetRaw(new GetShopCategoriesParameters() { Id = id });
            return shopCategories.ToList().SingleOrDefault();
        }

        private async Task<IEnumerable<ShopCategory>> GetRaw(GetShopCategoriesParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetShopCategories(parameters);
            List<ShopCategory> shopCategories = [];

            foreach (DataRow row in table.Rows)
            {
                shopCategories.Add(new()
                {
                    Id = new Guid(row["cat_id"].ToString()!),
                    Name = Utilities.DbNullableString(row["cat_name"])
                });
            }

            return shopCategories;
        }
    }
}