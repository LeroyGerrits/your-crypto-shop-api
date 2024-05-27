namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IPublicRepository<T, U>
    {
        Task<IEnumerable<T>> Get(U parameters);
        Task<T?> GetById(Guid id);
    }
}