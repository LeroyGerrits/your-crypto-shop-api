using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IProductFieldDataRepository : IMutableRepository<ProductFieldData, GetProductFieldDataParameters>
    {
        Task<MutationResult> Delete(Guid productId, Guid fieldId, Guid mutationId);
    }
}