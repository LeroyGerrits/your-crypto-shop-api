using DGBCommerce.Domain.Models;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IMerchantPasswordResetLinkRepository : IMutableRepository<MerchantPasswordResetLink, object> 
    {
        Task<MerchantPasswordResetLink?> GetByIdAndKey(Guid id, string key);
    }
}