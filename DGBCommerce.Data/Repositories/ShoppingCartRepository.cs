using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class ShoppingCartRepository(IDataAccessLayer dataAccessLayer) : IShoppingCartRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<ShoppingCart>> Get(GetShoppingCartsParameters parameters)
            => await GetRaw(parameters);

        public Task<ShoppingCart?> GetById(Guid merchantId, Guid id)
            => throw new InvalidOperationException($"{nameof(ShoppingCart)} objects can not be retrieved by merchant ID.");

        public async Task<ShoppingCart?> GetById(Guid id)
        {
            var shoppingCarts = await GetRaw(new GetShoppingCartsParameters() { Id = id });
            return shoppingCarts.ToList().SingleOrDefault();
        }

        public async Task<ShoppingCart?> GetBySession(Guid session)
        {
            var shoppingCarts = await GetRaw(new GetShoppingCartsParameters() { Session = session });
            return shoppingCarts.ToList().SingleOrDefault();
        }

        public async Task<IEnumerable<ShoppingCart>> GetByCustomerId(Guid customerId)
            => await GetRaw(new GetShoppingCartsParameters() { CustomerId = customerId });

        public Task<MutationResult> Create(ShoppingCart item, Guid mutationId)
            => _dataAccessLayer.CreateShoppingCart(item, mutationId);

        public Task<MutationResult> Update(ShoppingCart item, Guid mutationId)
            => throw new InvalidOperationException($"{nameof(ShoppingCart)} objects can not be updated.");

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => throw new InvalidOperationException($"{nameof(ShoppingCart)} objects can not be deleted.");

        private async Task<IEnumerable<ShoppingCart>> GetRaw(GetShoppingCartsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetShoppingCarts(parameters);
            List<ShoppingCart> shoppingCarts = [];

            foreach (DataRow row in table.Rows)
            {
                ShoppingCart shoppingCart = new()
                {
                    Id = new Guid(row["shc_id"].ToString()!),
                    Session = new Guid(row["shc_session"].ToString()!),
                    CustomerId = Utilities.DbNullableGuid(row["shc_customer"]),
                    LastIpAddress = Utilities.DbNullableString(row["shc_last_ip_address"]),
                    Created = Convert.ToDateTime(row["shc_created"]),
                    Edited = Convert.ToDateTime(row["shc_edited"])
                };

                shoppingCarts.Add(shoppingCart);
            }

            return shoppingCarts;
        }
    }
}
