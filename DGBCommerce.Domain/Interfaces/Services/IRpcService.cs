using DGBCommerce.Domain.Models.Response;

namespace DGBCommerce.Domain.Interfaces.Services
{
    public interface IRpcService
    {
        Task<double> GetCurrentBlock();
        Task<double> GetHashrate();
        Task<GetDifficultyResponse> GetDifficulty();
    }
}
