﻿using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository : IMutableRepository<Transaction, GetTransactionsParameters>
    {
        Task<IEnumerable<Transaction>> GetByShopId(Guid ordeshopId);
        Task<IEnumerable<Transaction>> GetUnpaid();
    }
}