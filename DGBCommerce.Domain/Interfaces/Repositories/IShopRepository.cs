using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IShopRepository : IMutableRepository<Shop, GetShopsParameters>
    {
        Task<PublicShop?> GetByIdAndSubDomainPublic(Guid? id, string subDomain);
        Task<IEnumerable<PublicShop>> GetPublic(GetShopsParameters parameters);
    }
}