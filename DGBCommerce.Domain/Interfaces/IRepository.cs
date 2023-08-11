using DGBCommerce.Domain.Models;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> Get();
        Task<T?> GetById(Guid id);
    }
}
