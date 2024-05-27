using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IShoppingCartItemFieldDataRepository : IMutableRepository<ShoppingCartItemFieldData, GetShoppingCartItemFieldDataParameters>
    {
        new Task<MutationResult> Delete(Guid shoppingCartItemId, Guid fieldId);
    }
}