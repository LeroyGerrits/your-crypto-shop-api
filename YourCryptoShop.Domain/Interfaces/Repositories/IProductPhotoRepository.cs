using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IProductPhotoRepository : IMutableRepository<ProductPhoto, GetProductPhotosParameters>
    {
        Task<MutationResult> ChangeDescription(Guid id, string description, Guid mutationId);
        Task<MutationResult> ChangeMain(Guid id, Guid productId, Guid mutationId);
        Task<MutationResult> ChangeVisible(Guid id, bool visible, Guid mutationId);
        Task<MutationResult> MoveDown(Guid id, Guid productId, Guid mutationId);
        Task<MutationResult> MoveUp(Guid id, Guid productId, Guid mutationId);
        Task<IEnumerable<PublicProductPhoto>> GetByProductIdPublic(Guid productId);
    }
}