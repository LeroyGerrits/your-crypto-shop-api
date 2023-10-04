using DGBCommerce.Domain.Models.Response;

namespace DGBCommerce.Domain.Interfaces.Services
{
    public interface IRpcService
    {
        Task<uint> GetBlockCount();
        Task<GetDifficultyResponse> GetDifficulty();
        Task<GetMiningInfoResponse> GetMiningInfo();        
    }
}
