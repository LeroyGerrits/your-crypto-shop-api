using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository : IMutableRepository<Customer, GetCustomersParameters>
    {
        Task<Customer?> GetById(Guid id);
        Task<Customer?> GetByEmailAddressAndPassword(Guid shopId, string emailAddress, string password, string? ipAddress);
        Task<Customer?> GetByEmailAddress(Guid shopId, string emailAddress);
        Task<Customer?> GetByIdAndPassword(Guid id, string password);
        Task<IEnumerable<PublicCustomer>> GetPublic(GetCustomersParameters parameters);        
        Task<MutationResult> UpdatePasswordAndSalt(Customer item, string password, string passwordSalt, Guid mutationId);
        Task<MutationResult> UpdatePasswordAndActivate(Customer item, string password, Guid mutationId);
    }
}