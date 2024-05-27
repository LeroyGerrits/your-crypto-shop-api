using YourCryptoShop.Domain.Models.Response;

namespace YourCryptoShop.Domain.Interfaces.Services
{
    public interface IRpcService
    {
        Task<uint> GetBlockCount();
        Task<GetDifficultyResponse> GetDifficulty();
        Task<GetMiningInfoResponse> GetMiningInfo();
        Task<string> GetNewAddress();
        Task<string> GetNewAddress(string? label);
        Task<string> GetNewAddress(string? label, string? addressType);
        Task<List<ListReceivedByAddressResponse>> ListReceivedByAddress();
        Task<ValidateAddressResponse> ValidateAddress(string address);
    }
}