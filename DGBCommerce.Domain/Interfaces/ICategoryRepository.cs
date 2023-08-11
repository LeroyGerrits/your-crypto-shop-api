using DGBCommerce.Domain.Models;

namespace DGBCommerce.Domain.Interfaces
{
    public interface ICategoryRepository : IMutableRepository<Category>
    {
        Task<IEnumerable<Category>> GetByMerchantId(Guid merchantId);
        Task<IEnumerable<Category>> GetByParentId(Guid merchantId);
        Task<IEnumerable<Category>> GetByShopId(Guid merchantId);
    }
}