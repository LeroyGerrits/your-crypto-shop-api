using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class ShoppingCartItemFieldDataRepository(IDataAccessLayer dataAccessLayer) : IShoppingCartItemFieldDataRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<ShoppingCartItemFieldData>> Get(GetShoppingCartItemFieldDataParameters parameters)
            => await GetRaw(parameters);

        public Task<ShoppingCartItemFieldData?> GetById(Guid merchantId, Guid id)
            => throw new InvalidOperationException("ShoppingCartItemFieldData objects can not be retrieved by Id.");

        public Task<MutationResult> Create(ShoppingCartItemFieldData item, Guid mutationId)
            => _dataAccessLayer.CreateShoppingCartItemFieldData(item, mutationId);

        public Task<MutationResult> Update(ShoppingCartItemFieldData item, Guid mutationId)
            => throw new InvalidOperationException("ShoppingCartItemFieldData objects can not be updated.");

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => throw new InvalidOperationException("ShoppingCartItemFieldData objects can not be deleted by Id.");

        public Task<MutationResult> Delete(Guid shoppingCartItemId, Guid fieldId, Guid mutationId)
            => _dataAccessLayer.DeleteShoppingCartItemFieldData(shoppingCartItemId, fieldId, mutationId);

        private async Task<IEnumerable<ShoppingCartItemFieldData>> GetRaw(GetShoppingCartItemFieldDataParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetShoppingCartItemFieldData(parameters);
            List<ShoppingCartItemFieldData> shoppingCartItemFieldData = [];

            foreach (DataRow row in table.Rows)
            {
                shoppingCartItemFieldData.Add(new ShoppingCartItemFieldData()
                {
                    ShoppingCartItemId = new Guid(row["pfd_shoppingCartItem"].ToString()!),
                    FieldId = new Guid(row["pfd_field"].ToString()!),
                    Data = Utilities.DbNullableString(row["pfd_data"])
                });
            }

            return shoppingCartItemFieldData;
        }
    }
}