using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Enums;
using YourCryptoShop.Domain.Interfaces;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using System.Data;

namespace YourCryptoShop.Data.Repositories
{
    public class FinancialStatementTransactionRepository(IDataAccessLayer dataAccessLayer) : IFinancialStatementTransactionRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<FinancialStatementTransaction>> Get(GetFinancialStatementTransactionsParameters parameters)
            => await GetRaw(parameters);

        public async Task<FinancialStatementTransaction?> GetById(Guid id)
        {
            var faqs = await GetRaw(new GetFinancialStatementTransactionsParameters() { Id = id });
            return faqs.ToList().SingleOrDefault();
        }

        private async Task<IEnumerable<FinancialStatementTransaction>> GetRaw(GetFinancialStatementTransactionsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetFinancialStatementTransactions(parameters);
            List<FinancialStatementTransaction> financialStatementTransactions = [];

            foreach (DataRow row in table.Rows)
            {
                financialStatementTransactions.Add(new FinancialStatementTransaction()
                {
                    Id = new Guid(row["trx_id"].ToString()!),
                    Type = (FinancialStatementTransactionType)Convert.ToInt16(row["trx_type"]),
                    Date = Convert.ToDateTime(row["trx_date"]),
                    Currency = new Currency()
                    {
                        Id = new Guid(row["trx_currency"].ToString()!),
                        Type = (CurrencyType)Convert.ToInt32(row["trx_currency_type"]),
                        Symbol = Utilities.DbNullableString(row["trx_currency_symbol"]),
                        Code = Utilities.DbNullableString(row["trx_currency_code"]),
                        Name = Utilities.DbNullableString(row["trx_currency_name"])
                    },
                    Amount = Convert.ToDecimal(row["trx_amount"]),
                    Recurrance = (Recurrance)Convert.ToInt16(row["trx_recurrance"]),
                    Description = Utilities.DbNullableString(row["trx_description"])
                });
            }

            return financialStatementTransactions;
        }
    }
}