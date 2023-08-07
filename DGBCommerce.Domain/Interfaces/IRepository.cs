using DGBCommerce.Domain.Models;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> Get();
        Task<T?> GetById(Guid id);
        Task<MutationResult> Insert(T item);
        Task<MutationResult> Update(T item);
        Task<MutationResult> Delete(Guid id);
    }
}
