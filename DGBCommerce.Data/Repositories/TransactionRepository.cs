using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class TransactionRepository(IDataAccessLayer dataAccessLayer) : ITransactionRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<Transaction>> Get(GetTransactionsParameters parameters)
            => await GetRaw(parameters);

        public async Task<IEnumerable<Transaction>> GetByShopId(Guid shopId)
            => await GetRaw(new GetTransactionsParameters() { ShopId = shopId });

        public async Task<IEnumerable<Transaction>> GetUnpaid()
            => await GetRaw(new GetTransactionsParameters() { Unpaid = true });

        public Task<Transaction?> GetById(Guid merchantId, Guid id)
            => throw new InvalidOperationException($"{nameof(Transaction)} objects can not be retrieved by merchant ID.");

        public async Task<Transaction?> GetById(Guid id)
        {
            var transactions = await GetRaw(new GetTransactionsParameters() { Id = id });
            return transactions.ToList().SingleOrDefault();
        }

        public Task<MutationResult> Create(Transaction item, Guid mutationId)
            => _dataAccessLayer.CreateTransaction(item, mutationId);

        public Task<MutationResult> Update(Transaction item, Guid mutationId)
            => throw new InvalidOperationException($"{nameof(Transaction)} objects can not be updated.");

        public Task<MutationResult> UpdateAmountPaid(Transaction item, decimal amountPaid, Guid mutationId)
            => _dataAccessLayer.UpdateTransactionAmountPaid(item.Id!.Value, amountPaid, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => throw new InvalidOperationException($"{nameof(Transaction)} objects can not be deleted.");

        private async Task<IEnumerable<Transaction>> GetRaw(GetTransactionsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetTransactions(parameters);
            List<Transaction> transactions = [];

            foreach (DataRow row in table.Rows)
            {
                transactions.Add(new Transaction()
                {
                    Id = new Guid(row["trx_id"].ToString()!),
                    ShopId = new Guid(row["trx_shop"].ToString()!),
                    Date = Convert.ToDateTime(row["trx_date"]),
                    Recipient = Utilities.DbNullableString(row["trx_recipient"]),
                    AmountDue = Convert.ToDecimal(row["trx_amount_due"]),                    
                    AmountPaid = Convert.ToDecimal(row["trx_amount_paid"]),
                    PaidInFull = Utilities.DBNullableDateTime(row["trx_paid_in_full"])
                });
            }

            return transactions;
        }
    }
}