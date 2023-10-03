using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IShopRepository : IMutableRepository<Shop, GetShopsParameters>
    {
        Task<IEnumerable<PublicShop>> GetPublic(GetShopsParameters parameters);
    }
}