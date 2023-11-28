using DGBCommerce.Domain.Models;

namespace DGBCommerce.Domain.Interfaces.Services
{
    public interface IAddressService
    {
        Task<Address?> GetAddress(string addressLine1, string? addressLine2, string postalCode, string city, string? province, Guid countryId);
    }
}