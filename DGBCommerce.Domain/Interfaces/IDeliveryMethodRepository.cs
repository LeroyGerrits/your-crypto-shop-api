using DGBCommerce.Domain.Models;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IDeliveryMethodRepository : IMutableRepository<DeliveryMethod>
    {
        Task<IEnumerable<DeliveryMethod>> GetByMerchantId(Guid merchantId);
        Task<IEnumerable<DeliveryMethod>> GetByShopId(Guid shopId);
    }
}