using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IDeliveryMethodRepository : IMutableRepository<DeliveryMethod, GetDeliveryMethodsParameters> { }
}