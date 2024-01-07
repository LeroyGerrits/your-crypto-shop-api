using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IPageRepository : IMutableRepository<Page, GetPagesParameters> { }
}