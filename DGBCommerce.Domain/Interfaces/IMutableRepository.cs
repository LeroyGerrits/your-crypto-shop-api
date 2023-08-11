using DGBCommerce.Domain.Models;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IMutableRepository<T> : IRepository<T>
    {
        Task<MutationResult> Create(T item, Guid merchantId);
        Task<MutationResult> Update(T item);
        Task<MutationResult> Delete(Guid id);
    }
}
