using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IShoppingCartItemFieldDataRepository : IMutableRepository<ShoppingCartItemFieldData, GetShoppingCartItemFieldDataParameters>
    {
        Task<MutationResult> Delete(Guid shoppingCartItemId, Guid fieldId);
    }
}