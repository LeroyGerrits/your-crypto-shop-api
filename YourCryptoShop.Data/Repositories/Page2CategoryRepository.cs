using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Interfaces;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using System.Data;

namespace YourCryptoShop.Data.Repositories
{
    public class Page2CategoryRepository(IDataAccessLayer dataAccessLayer) : IPage2CategoryRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<Page2Category>> Get(GetPage2CategoriesParameters parameters)
            => await GetRaw(parameters);

        public Task<Page2Category?> GetById(Guid merchantId, Guid id)
            => throw new InvalidOperationException("PageCategory objects can not be retrieved by Id.");

        public Task<MutationResult> Create(Page2Category item, Guid mutationId)
            => _dataAccessLayer.CreatePage2Category(item, mutationId);

        public Task<MutationResult> Update(Page2Category item, Guid mutationId)
            => throw new InvalidOperationException("PageCategory objects can not be updated.");

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => throw new InvalidOperationException("PageCategory objects can not be deleted by Id.");

        public Task<MutationResult> Delete(Guid pageId, Guid categoryId, Guid mutationId)
            => _dataAccessLayer.DeletePage2Category(pageId, categoryId, mutationId);

        private async Task<IEnumerable<Page2Category>> GetRaw(GetPage2CategoriesParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetPage2Categories(parameters);
            List<Page2Category> pageCategories = [];

            foreach (DataRow row in table.Rows)
            {
                pageCategories.Add(new Page2Category()
                {
                    PageId = new Guid(row["p2c_page"].ToString()!),
                    CategoryId = new Guid(row["p2c_category"].ToString()!),
                });
            }

            return pageCategories;
        }
    }
}