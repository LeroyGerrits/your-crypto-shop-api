namespace DGBCommerce.Domain.Interfaces
{
    public interface IPublicRepository<T, U>
    {
        Task<IEnumerable<T>> Get(U parameters);
        Task<T?> GetById(Guid id);
    }
}