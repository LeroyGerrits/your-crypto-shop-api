using DGBCommerce.Domain.Models;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IShopRepository : IRepository<Shop>
    {
        Task<IEnumerable<Shop>> GetByMerchant(Guid merchantId);
    }
}