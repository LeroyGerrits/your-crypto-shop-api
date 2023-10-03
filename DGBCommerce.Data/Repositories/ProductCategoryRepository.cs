using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public ProductCategoryRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public async Task<IEnumerable<ProductCategory>> Get(GetProductCategoriesParameters parameters)
            => await GetRaw(parameters);

        public Task<ProductCategory?> GetById(Guid merchantId, Guid id)
            => throw new InvalidOperationException("ProductCategory objects can not be retrieved by Id.");

        public Task<MutationResult> Create(ProductCategory item, Guid mutationId)
            => _dataAccessLayer.CreateProductCategory(item, mutationId);

        public Task<MutationResult> Update(ProductCategory item, Guid mutationId)
            => throw new InvalidOperationException("ProductCategory objects can not be updated.");

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => throw new InvalidOperationException("ProductCategory objects can not be deleted by Id.");

        public Task<MutationResult> Delete(Guid productId, Guid categoryId, Guid mutationId)
            => _dataAccessLayer.DeleteProductCategory(productId, categoryId, mutationId);

        private async Task<IEnumerable<ProductCategory>> GetRaw(GetProductCategoriesParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetProductCategories(parameters);
            List<ProductCategory> shops = new();

            foreach (DataRow row in table.Rows)
            {
                shops.Add(new ProductCategory()
                {
                    ProductId = new Guid(row["p2c_product"].ToString()!),
                    CategoryId = new Guid(row["p2c_category"].ToString()!),
                });
            }

            return shops;
        }
    }
}