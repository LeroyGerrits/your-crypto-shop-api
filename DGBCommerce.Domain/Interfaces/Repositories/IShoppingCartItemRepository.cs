using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IShoppingCartItemRepository : IMutableRepository<ShoppingCartItem, GetShoppingCartItemsParameters>
    {
        Task<ShoppingCartItem?> GetById(Guid id);
        Task<IEnumerable<ShoppingCartItem>> GetByShoppingCartId(Guid shoppingCartId);
    }
}