using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface ICryptoWalletRepository : IMutableRepository<CryptoWallet, GetCryptoWalletsParameters> { }
}