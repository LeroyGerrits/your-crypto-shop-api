using DGBCommerce.Domain.Models;

namespace DGBCommerce.Domain.Interfaces.Repositories
{
    public interface IGeneralRepository
    {
        Task<Dictionary<string, decimal>> GetDashboardSales(Guid merchantId, string mode);
        Task<IEnumerable<Product>> GetDashboardMostPopularProducts(Guid merchantId);
        Task<IEnumerable<Customer>> GetDashboardNewestCustomers(Guid merchantId);
        Task<Stats> GetStats();
    }
}