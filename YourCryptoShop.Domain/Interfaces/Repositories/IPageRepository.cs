using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IPageRepository : IMutableRepository<Page, GetPagesParameters>
    {
        Task<IEnumerable<PublicPage>> GetByShopIdPublic(Guid shopId);
    }
}