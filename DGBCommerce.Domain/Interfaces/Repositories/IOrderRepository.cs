using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IOrderRepository : IMutableRepository<Order, GetOrdersParameters>
    {
        Task<IEnumerable<Order>> GetByStatus(OrderStatus status);        
        Task<MutationResult> UpdateStatus(Order item, OrderStatus status, Guid mutationId);
        Task<MutationResult> UpdateTransaction(Order item, Guid transactionId, Guid mutationId);
    }
}