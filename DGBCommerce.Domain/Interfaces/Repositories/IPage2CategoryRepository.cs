using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IPage2CategoryRepository : IMutableRepository<Page2Category, GetPage2CategoriesParameters> {
        Task<MutationResult> Delete(Guid pageId, Guid categoryId, Guid mutationId);
    }
}