using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository : IMutableRepository<Category, GetCategoriesParameters>
    {
        Task<MutationResult> ChangeParent(Guid id, Guid parentId, Guid mutationId);
        Task<IEnumerable<PublicCategory>> GetByShopIdPublic(Guid shopId);
        Task<MutationResult> MoveDown(Guid id, Guid? parentId, Guid mutationId);
        Task<MutationResult> MoveUp(Guid id, Guid? parentId, Guid mutationId);
    }
}