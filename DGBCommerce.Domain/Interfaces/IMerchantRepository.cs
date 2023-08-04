using DGBCommerce.Domain.Models;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IMerchantRepository : IRepository<Merchant>
    {
        Task<Merchant?> GetByEmailAddressAndPassword(string emailAddress, string password);
    }
}
