﻿namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IMutableRepository<T, U> : IRepository<T, U>
    {
        Task<MutationResult> Create(T item, Guid mutationId);
        Task<MutationResult> Update(T item, Guid mutationId);
        Task<MutationResult> Delete(Guid id, Guid mutationId);
    }
}