using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces
{
    public interface ICategoryRepository : IMutableRepository<Category, GetCategoriesParameters>
    {
        Task<MutationResult> ChangeParent(Guid id, Guid parentId, Guid mutationId);
        Task<MutationResult> MoveDown(Guid id, Guid? parentId, Guid mutationId);
        Task<MutationResult> MoveUp(Guid id, Guid? parentId, Guid mutationId);
    }
}