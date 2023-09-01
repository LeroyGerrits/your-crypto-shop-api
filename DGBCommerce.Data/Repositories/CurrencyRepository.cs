using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public CurrencyRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public async Task<IEnumerable<Currency>> Get(GetCurrenciesParameters parameters)
            => await GetRaw(parameters);

        public async Task<Currency?> GetById(Guid id)
        {
            var currencies = await GetRaw(new GetCurrenciesParameters() { Id = id });
            return currencies.ToList().SingleOrDefault();
        }

        private async Task<IEnumerable<Currency>> GetRaw(GetCurrenciesParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetCurrencies(parameters);
            List<Currency> currencies = new();

            foreach (DataRow row in table.Rows)
            {
                currencies.Add(new()
                {
                    Id = new Guid(row["cur_id"].ToString()!),
                    Symbol = Utilities.DbNullableString(row["cur_symbol"]),
                    Name = Utilities.DbNullableString(row["cur_name"])
                });
            }

            return currencies;
        }
    }
}
