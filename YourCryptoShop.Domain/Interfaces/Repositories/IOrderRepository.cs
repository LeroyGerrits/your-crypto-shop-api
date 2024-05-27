using YourCryptoShop.Domain.Enums;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IOrderRepository : IMutableRepository<Order, GetOrdersParameters>
    {
        Task<IEnumerable<Order>> GetByStatus(OrderStatus status);
        Task<IEnumerable<PublicOrder>> GetPublic(GetOrdersParameters parameters);
        Task<PublicOrder?> GetByIdPublic(Guid shopId, Guid id);
        Task<MutationResult> UpdateStatus(Order item, OrderStatus status, Guid mutationId);
        Task<MutationResult> UpdateTransaction(Order item, Guid transactionId, Guid mutationId);
    }
}