using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class Product2CategoryRepository(IDataAccessLayer dataAccessLayer) : IProduct2CategoryRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<Product2Category>> Get(GetProduct2CategoriesParameters parameters)
            => await GetRaw(parameters);

        public Task<Product2Category?> GetById(Guid merchantId, Guid id)
            => throw new InvalidOperationException("ProductCategory objects can not be retrieved by Id.");

        public Task<MutationResult> Create(Product2Category item, Guid mutationId)
            => _dataAccessLayer.CreateProduct2Category(item, mutationId);

        public Task<MutationResult> Update(Product2Category item, Guid mutationId)
            => throw new InvalidOperationException("ProductCategory objects can not be updated.");

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => throw new InvalidOperationException("ProductCategory objects can not be deleted by Id.");

        public Task<MutationResult> Delete(Guid productId, Guid categoryId, Guid mutationId)
            => _dataAccessLayer.DeleteProduct2Category(productId, categoryId, mutationId);

        private async Task<IEnumerable<Product2Category>> GetRaw(GetProduct2CategoriesParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetProduct2Categories(parameters);
            List<Product2Category> productCategories = [];

            foreach (DataRow row in table.Rows)
            {
                productCategories.Add(new Product2Category()
                {
                    ProductId = new Guid(row["p2c_product"].ToString()!),
                    CategoryId = new Guid(row["p2c_category"].ToString()!),
                });
            }

            return productCategories;
        }
    }
}