using DGBCommerce.Domain.Models.Response;

namespace DGBCommerce.Domain.Interfaces.Services
{
    public interface IRpcService
    {
        Task<uint> GetBlockCount();
        Task<GetDifficultyResponse> GetDifficulty();
        Task<GetMiningInfoResponse> GetMiningInfo();
        Task<string> GetNewAddress();
        Task<string> GetNewAddress(string? label);
        Task<string> GetNewAddress(string? label, string? addressType);
    }
}