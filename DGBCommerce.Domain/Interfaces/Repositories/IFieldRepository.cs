using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IFieldRepository : IMutableRepository<Field, GetFieldsParameters>
    {
        Task<IEnumerable<PublicField>> GetByShopIdPublic(Guid shopId, FieldEntity? entity, FieldType? type);
    }
}