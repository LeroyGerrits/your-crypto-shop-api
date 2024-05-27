using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IPageCategoryRepository : IPublicRepository<PageCategory, GetPageCategoriesParameters> { }
}