using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
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
                        Name = Utilities.DbNullableString(row["trx_currency_name"]),
                        Symbol = Utilities.DbNullableString(row["trx_currency_symbol"])
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