using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using System.Data;
using System.Globalization;

namespace DGBCommerce.Data.Repositories
{
    public class GeneralRepository(IDataAccessLayer dataAccessLayer) : IGeneralRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<Dictionary<string, decimal>> GetDashboardSales(Guid merchantId, string mode)
        {
            Dictionary<string, decimal> sales = [];
            DataTable table = await _dataAccessLayer.GetDashboardSales(merchantId, mode);

            foreach (DataRow row in table.Rows)
            {
                int year = Convert.ToInt32(row["year"]);
                int month = Convert.ToInt32(row["month"]);
                DateTime fictionalDate = new(year, month, 1);
                sales.Add(fictionalDate.ToString("MMM yyyy", new CultureInfo("en-US")), Convert.ToDecimal(row["value"]));
            }

            return sales;
        }

        public async Task<IEnumerable<Product>> GetDashboardMostPopularProducts(Guid merchantId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Customer>> GetDashboardNewestCustomers(Guid merchantId)
        {
            throw new NotImplementedException();
        }

        public async Task<Stats> GetStats()
        {
            Stats stats = new();
            DataTable table = await _dataAccessLayer.GetStats();
            
            foreach (DataRow row in table.Rows)
            {
                stats = new()
                {
                    Merchants = Convert.ToInt32(row["Merchants"]),
                    Shops = Convert.ToInt32(row["Shops"]),
                    Products = Convert.ToInt32(row["Products"]),
                    Orders = Convert.ToInt32(row["Orders"])
                };
            }

            return stats;
        }
    }
}