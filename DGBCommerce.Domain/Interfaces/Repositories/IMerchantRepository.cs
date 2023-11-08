using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IMerchantRepository : IMutableRepository<Merchant, GetMerchantsParameters>
    {
        Task<Merchant?> GetByEmailAddressAndPassword(string emailAddress, string password, string? ipAddress);
        Task<Merchant?> GetByEmailAddress(string emailAddress);
        Task<Merchant?> GetByIdAndPassword(Guid id, string password);
        Task<PublicMerchant?> GetByIdPublic(Guid id);
        Task<IEnumerable<PublicMerchant>> GetPublic(GetMerchantsParameters parameters);        
        Task<MutationResult> UpdatePasswordAndSalt(Merchant item, string password, string passwordSalt, Guid mutationId);
        Task<MutationResult> UpdatePasswordAndActivate(Merchant item, string password, Guid mutationId);
    }
}