using DGBCommerce.Domain.Models;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IMutableRepository<T> : IRepository<T>
    {
        Task<MutationResult> Create(T item, Guid mutationId);
        Task<MutationResult> Update(T item, Guid mutationId);
        Task<MutationResult> Delete(Guid id, Guid mutationId);
    }
}
