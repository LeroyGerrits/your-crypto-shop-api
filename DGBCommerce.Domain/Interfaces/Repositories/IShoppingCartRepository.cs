using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IShoppingCartRepository : IMutableRepository<ShoppingCart, GetShoppingCartsParameters>
    {
        Task<ShoppingCart?> GetById(Guid id);
        Task<ShoppingCart?> GetBySession(Guid session);
        Task<IEnumerable<ShoppingCart>> GetByCustomerId(Guid customerId);
    }
}