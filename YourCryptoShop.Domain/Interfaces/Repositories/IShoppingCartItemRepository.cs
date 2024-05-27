using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IShoppingCartItemRepository : IMutableRepository<ShoppingCartItem, GetShoppingCartItemsParameters>
    {
        Task<ShoppingCartItem?> GetById(Guid id);
        Task<IEnumerable<ShoppingCartItem>> GetByShoppingCartId(Guid shoppingCartId);
    }
}