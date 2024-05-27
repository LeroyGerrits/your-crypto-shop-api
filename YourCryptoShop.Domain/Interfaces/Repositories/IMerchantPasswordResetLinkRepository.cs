using YourCryptoShop.Domain.Models;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IMerchantPasswordResetLinkRepository : IMutableRepository<MerchantPasswordResetLink, object>
    {
        Task<MerchantPasswordResetLink?> GetByIdAndKey(Guid id, string key);
        Task<MutationResult> UpdateUsed(MerchantPasswordResetLink item, Guid mutationId);
    }
}