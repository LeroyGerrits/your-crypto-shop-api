using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class ShoppingCartItemRepository(IDataAccessLayer dataAccessLayer) : IShoppingCartItemRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<ShoppingCartItem>> Get(GetShoppingCartItemsParameters parameters)
            => await GetRaw(parameters);

        public Task<ShoppingCartItem?> GetById(Guid merchantId, Guid id)
            => throw new InvalidOperationException($"{nameof(ShoppingCartItem)} objects can not be retrieved by merchant ID.");

        public async Task<ShoppingCartItem?> GetById(Guid id)
        {
            var shoppingCartItems = await GetRaw(new GetShoppingCartItemsParameters() { Id = id });
            return shoppingCartItems.ToList().SingleOrDefault();
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetByShoppingCartId(Guid shoppingCartId)
            => await GetRaw(new GetShoppingCartItemsParameters() { ShoppingCartId = shoppingCartId });

        public Task<MutationResult> Create(ShoppingCartItem item, Guid mutationId)
            => _dataAccessLayer.CreateShoppingCartItem(item);

        public Task<MutationResult> Update(ShoppingCartItem item, Guid mutationId)
            => _dataAccessLayer.UpdateShoppingCartItem(item);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => _dataAccessLayer.DeleteShoppingCartItem(id);

        private async Task<IEnumerable<ShoppingCartItem>> GetRaw(GetShoppingCartItemsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetShoppingCartItems(parameters);
            List<ShoppingCartItem> shoppingCartItems = [];

            foreach (DataRow row in table.Rows)
            {
                ShoppingCartItem shoppingCartItem = new()
                {
                    Id = new Guid(row["sci_id"].ToString()!),
                    ShoppingCartId = new Guid(row["sci_shopping_cart"].ToString()!),
                    ProductId = new Guid(row["sci_product"].ToString()!),
                    ProductName = row["sci_product_name"].ToString()!,
                    ProductPrice = Convert.ToDecimal(row["sci_product_price"]),
                    ProductStock = Utilities.DbNullableInt(row["sci_product_stock"]),
                    ProductMainPhotoId = Utilities.DbNullableGuid(row["sci_product_main_photo_id"]),
                    ProductMainPhotoExtension = Utilities.DbNullableString(row["sci_product_main_photo_extension"]),
                    Amount = Convert.ToUInt32(row["sci_amount"])
                };

                shoppingCartItems.Add(shoppingCartItem);
            }

            return shoppingCartItems;
        }
    }
}
