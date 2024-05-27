using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IDeliveryMethodRepository : IMutableRepository<DeliveryMethod, GetDeliveryMethodsParameters> 
    {
        Task<PublicDeliveryMethod?> GetByIdPublic(Guid id);
        Task<IEnumerable<PublicDeliveryMethod>> GetByShopIdPublic(Guid shopId);
    }
}