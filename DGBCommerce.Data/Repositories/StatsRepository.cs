using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class StatsRepository(IDataAccessLayer dataAccessLayer) : IStatsRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<Stats>> Get(GetParameters parameters)
            => await GetRaw();

        public Task<Stats?> GetById(Guid id)
            => throw new InvalidOperationException("Stats objects can not be retrieved by Id.");

        private async Task<IEnumerable<Stats>> GetRaw()
        {
            DataTable table = await _dataAccessLayer.GetStats();
            List<Stats> stats = [];

            foreach (DataRow row in table.Rows)
            {
                stats.Add(new()
                {
                    Merchants = Convert.ToInt32(row["Merchants"]),
                    Shops = Convert.ToInt32(row["Shops"]),
                    Products = Convert.ToInt32(row["Products"]),
                    Orders = Convert.ToInt32(row["Orders"])
                });
            }

            return stats;
        }
    }
}