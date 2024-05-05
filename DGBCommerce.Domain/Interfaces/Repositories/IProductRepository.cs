using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IProductRepository : IMutableRepository<Product, GetProductsParameters> 
    {
        Task<IEnumerable<PublicProduct>> GetPublic(GetProductsParameters parameters);
        Task<PublicProduct?> GetByIdPublic(Guid shopId, Guid id);
        Task<MutationResult> Duplicate(Guid id, Guid mutationId);
    }
}