using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository : IMutableRepository<Customer, GetCustomersParameters>
    {
        Task<Customer?> GetByEmailAddressAndPassword(string emailAddress, string password, string? ipAddress);
        Task<Customer?> GetByEmailAddress(string emailAddress);
        Task<Customer?> GetByIdAndPassword(Guid id, string password);
        Task<PublicCustomer?> GetByIdPublic(Guid merchantId, Guid id);
        Task<IEnumerable<PublicCustomer>> GetPublic(GetCustomersParameters parameters);        
        Task<MutationResult> UpdatePasswordAndSalt(Customer item, string password, string passwordSalt, Guid mutationId);
        Task<MutationResult> UpdatePasswordAndActivate(Customer item, string password, Guid mutationId);
    }
}