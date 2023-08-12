using DGBCommerce.Domain.Models;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IDigiByteWalletRepository : IMutableRepository<DigiByteWallet>
    {
        Task<IEnumerable<DigiByteWallet>> GetByMerchantId(Guid merchantId);
    }
}