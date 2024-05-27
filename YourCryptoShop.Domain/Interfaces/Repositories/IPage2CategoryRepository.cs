using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IPage2CategoryRepository : IMutableRepository<Page2Category, GetPage2CategoriesParameters> {
        Task<MutationResult> Delete(Guid pageId, Guid categoryId, Guid mutationId);
    }
}