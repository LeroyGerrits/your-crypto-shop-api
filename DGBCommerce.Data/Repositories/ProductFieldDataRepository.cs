using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class ProductFieldDataRepository(IDataAccessLayer dataAccessLayer) : IProductFieldDataRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<ProductFieldData>> Get(GetProductFieldDataParameters parameters)
            => await GetRaw(parameters);

        public Task<ProductFieldData?> GetById(Guid merchantId, Guid id)
            => throw new InvalidOperationException("ProductFieldData objects can not be retrieved by Id.");

        public Task<MutationResult> Create(ProductFieldData item, Guid mutationId)
            => _dataAccessLayer.CreateProductFieldData(item, mutationId);

        public Task<MutationResult> Update(ProductFieldData item, Guid mutationId)
            => throw new InvalidOperationException("ProductFieldData objects can not be updated.");

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => throw new InvalidOperationException("ProductFieldData objects can not be deleted by Id.");

        public Task<MutationResult> Delete(Guid productId, Guid fieldId, Guid mutationId)
            => _dataAccessLayer.DeleteProductFieldData(productId, fieldId, mutationId);

        private async Task<IEnumerable<ProductFieldData>> GetRaw(GetProductFieldDataParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetProductFieldData(parameters);
            List<ProductFieldData> productFieldData = [];

            foreach (DataRow row in table.Rows)
            {
                productFieldData.Add(new ProductFieldData()
                {
                    ProductId = new Guid(row["pfd_product"].ToString()!),
                    FieldId = new Guid(row["pfd_field"].ToString()!),
                    Data = Utilities.DbNullableString(row["pfd_data"])
                });
            }

            return productFieldData;
        }
    }
}