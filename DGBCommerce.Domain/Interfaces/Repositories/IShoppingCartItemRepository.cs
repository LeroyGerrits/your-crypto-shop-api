using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IShoppingCartItemRepository : IMutableRepository<ShoppingCartItem, GetShoppingCartItemsParameters>
    {
        Task<IEnumerable<ShoppingCartItem>> GetByShoppingCartId(Guid shoppingCartId);
    }
}