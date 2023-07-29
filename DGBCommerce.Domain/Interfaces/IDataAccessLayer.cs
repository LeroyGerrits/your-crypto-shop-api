using System.Data;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IDataAccessLayer
    {
        Task<DataTable> GetDeliveryMethods(Guid? id, string? name);
        Task<DataTable> GetShops(Guid? id, string? name, string? subDomain);
    }
}