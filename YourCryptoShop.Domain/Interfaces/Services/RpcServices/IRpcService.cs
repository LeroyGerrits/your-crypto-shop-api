using YourCryptoShop.Domain.Models.Response;

namespace YourCryptoShop.Domain.Interfaces.Services.RpcServices
{
    public interface IRpcService
    {
        Task<uint> GetBlockCount();
        Task<string> GetNewAddress();
        Task<string> GetNewAddress(string? label);
        Task<string> GetNewAddress(string? label, string? addressType);
        Task<List<ListReceivedByAddressResponse>> ListReceivedByAddress();
        Task<ValidateAddressResponse> ValidateAddress(string address);
    }
}