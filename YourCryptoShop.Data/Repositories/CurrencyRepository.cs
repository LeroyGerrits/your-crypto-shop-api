using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Interfaces;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using System.Data;
using YourCryptoShop.Domain.Enums;

namespace YourCryptoShop.Data.Repositories
{
    public class CurrencyRepository(IDataAccessLayer dataAccessLayer) : ICurrencyRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

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
            List<Currency> currencies = [];

            foreach (DataRow row in table.Rows)
            {
                currencies.Add(new()
                {
                    Id = new Guid(row["cur_id"].ToString()!),
                    Type = (CurrencyType)Convert.ToInt32(row["cur_type"]),
                    Symbol = Utilities.DbNullableString(row["cur_symbol"]),
                    Code = Utilities.DbNullableString(row["cur_code"]),
                    Name = Utilities.DbNullableString(row["cur_name"]),
                    Supported = Convert.ToBoolean(row["cur_supported"])
                });
            }

            return currencies;
        }
    }
}