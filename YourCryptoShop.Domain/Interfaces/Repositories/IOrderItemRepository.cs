using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IOrderItemRepository : IMutableRepository<OrderItem, GetOrderItemsParameters>
    {
        Task<IEnumerable<OrderItem>> GetByOrderId(Guid orderId);
        Task<OrderItem?> GetById(Guid id);
    }
}