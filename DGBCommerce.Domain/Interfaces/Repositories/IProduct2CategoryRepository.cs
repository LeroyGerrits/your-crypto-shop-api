using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IProduct2CategoryRepository : IMutableRepository<Product2Category, GetProduct2CategoriesParameters> {
        Task<MutationResult> Delete(Guid productId, Guid categoryId, Guid mutationId);
    }
}