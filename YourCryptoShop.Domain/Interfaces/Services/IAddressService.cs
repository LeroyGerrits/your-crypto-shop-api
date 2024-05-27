using YourCryptoShop.Domain.Models;

namespace YourCryptoShop.Domain.Interfaces.Services
{
    public interface IAddressService
    {
        Task<Address?> GetAddress(string addressLine1, string? addressLine2, string postalCode, string city, string? province, Guid countryId);
    }
}