using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IProductFieldDataRepository : IMutableRepository<ProductFieldData, GetProductFieldDataParameters>
    {
        Task<MutationResult> Delete(Guid productId, Guid fieldId, Guid mutationId);
    }
}