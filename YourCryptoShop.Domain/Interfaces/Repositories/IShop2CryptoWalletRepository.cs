using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IShop2CryptoWalletRepository : IMutableRepository<Shop2CryptoWallet, GetShop2CryptoWalletsParameters> {
        Task<MutationResult> Delete(Guid pageId, Guid shopId, Guid cryptoWalletId);
    }
}