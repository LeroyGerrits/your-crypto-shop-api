using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IShoppingCartRepository : IMutableRepository<ShoppingCart, GetShoppingCartsParameters>
    {
        Task<ShoppingCart?> GetById(Guid id);
        Task<ShoppingCart?> GetBySession(Guid session);
        Task<IEnumerable<ShoppingCart>> GetByCustomerId(Guid customerId);
    }
}