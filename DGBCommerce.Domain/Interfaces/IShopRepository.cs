using DGBCommerce.Domain.Models;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IShopRepository : IMutableRepository<Shop>
    {
        Task<IEnumerable<Shop>> GetByMerchantId(Guid merchantId);
    }
}