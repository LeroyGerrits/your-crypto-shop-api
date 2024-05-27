using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IProduct2CategoryRepository : IMutableRepository<Product2Category, GetProduct2CategoriesParameters> {
        Task<MutationResult> Delete(Guid productId, Guid categoryId, Guid mutationId);
    }
}