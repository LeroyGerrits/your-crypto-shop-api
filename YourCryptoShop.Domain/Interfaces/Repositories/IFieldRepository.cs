using YourCryptoShop.Domain.Enums;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IFieldRepository : IMutableRepository<Field, GetFieldsParameters>
    {
        Task<IEnumerable<PublicField>> GetByShopIdPublic(Guid shopId, FieldEntity? entity, FieldType? type);
    }
}