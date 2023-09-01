﻿namespace DGBCommerce.Domain.Interfaces
{
    public interface IRepository<T, U>
    {
        Task<IEnumerable<T>> Get(U parameters);
        Task<T?> GetById(Guid merchantId, Guid id);
    }
}