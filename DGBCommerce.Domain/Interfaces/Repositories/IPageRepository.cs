using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IPageRepository : IMutableRepository<Page, GetPagesParameters>
    {
        Task<IEnumerable<PublicPage>> GetByShopIdPublic(Guid shopId);
    }
}