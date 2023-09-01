using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IMerchantRepository : IMutableRepository<Merchant, GetMerchantsParameters>
    {
        Task<Merchant?> GetByEmailAddressAndPassword(string emailAddress, string password);
        Task<Merchant?> GetByEmailAddress(string emailAddress);
    }
}