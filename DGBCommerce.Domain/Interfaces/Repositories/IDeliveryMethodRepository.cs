using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IDeliveryMethodRepository : IMutableRepository<DeliveryMethod, GetDeliveryMethodsParameters> 
    {
        Task<PublicDeliveryMethod?> GetByIdPublic(Guid id);
        Task<IEnumerable<PublicDeliveryMethod>> GetByShopIdPublic(Guid shopId);
    }
}