using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IOrderItemRepository : IMutableRepository<OrderItem, GetOrderItemsParameters>
    {
        Task<IEnumerable<OrderItem>> GetByOrderId(Guid orderId);
        Task<OrderItem?> GetById(Guid id);
    }
}