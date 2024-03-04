using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IShoppingCartRepository : IMutableRepository<ShoppingCart, GetShoppingCartsParameters>
    {
        Task<ShoppingCart?> GetBySession(Guid session);
        Task<IEnumerable<ShoppingCart>> GetByCustomerId(Guid customerId);
    }
}