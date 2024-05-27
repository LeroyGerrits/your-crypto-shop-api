using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IShopRepository : IMutableRepository<Shop, GetShopsParameters>
    {
        Task<PublicShop?> GetByIdAndSubDomainPublic(Guid? id, string subDomain);
        Task<PublicShop?> GetByIdPublic(Guid id);
        Task<PublicShop?> GetBySubDomainPublic(string subdomain);
        Task<IEnumerable<PublicShop>> GetPublic(GetShopsParameters parameters);
    }
}