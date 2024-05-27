using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository : IMutableRepository<Transaction, GetTransactionsParameters>
    {
        Task<IEnumerable<Transaction>> GetByShopId(Guid ordeshopId);
        Task<IEnumerable<Transaction>> GetUnpaidAndYoungerThan3Days();
        Task<MutationResult> UpdateAmountPaid(Transaction item, decimal amountPaid, Guid mutationId);
    }
}