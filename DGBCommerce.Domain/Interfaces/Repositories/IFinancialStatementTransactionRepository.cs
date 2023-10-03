using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IFinancialStatementTransactionRepository : IPublicRepository<FinancialStatementTransaction, GetFinancialStatementTransactionsParameters> { }
}