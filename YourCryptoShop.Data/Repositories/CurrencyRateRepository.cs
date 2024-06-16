using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Interfaces;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using System.Data;

namespace YourCryptoShop.Data.Repositories
{
    public class CurrencyRateRepository(IDataAccessLayer dataAccessLayer) : ICurrencyRateRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<CurrencyRate>> Get(GetCurrencyRatesParameters parameters)
            => await this.GetRaw(parameters);

        public async Task<CurrencyRate?> GetById(Guid id)
        {
            var CurrencyRates = await this.GetRaw(new GetCurrencyRatesParameters() { Id = id });
            return CurrencyRates.ToList().SingleOrDefault();
        }

        public Task<MutationResult> Create(CurrencyRate item, Guid mutationId)
            => throw new InvalidOperationException($"{nameof(CurrencyRate)} objects can not be created.");

        public Task<MutationResult> Update(CurrencyRate item, Guid mutationId)
            => _dataAccessLayer.UpdateCurrencyRate(item, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => throw new InvalidOperationException($"{nameof(CurrencyRate)} objects can not be deleted.");

        private async Task<IEnumerable<CurrencyRate>> GetRaw(GetCurrencyRatesParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetCurrencyRates(parameters);
            List<CurrencyRate> CurrencyRates = [];

            foreach (DataRow row in table.Rows)
            {
                CurrencyRates.Add(new()
                {
                    Id = new Guid(row["rat_id"].ToString()!),
                    CurrencyFromId = new Guid(row["rat_currency_from"].ToString()!),
                    CurrencyToId = new Guid(row["rat_currency_to"].ToString()!),
                    Rate = Convert.ToDecimal(row["rat_rate"]),
                    Date = Convert.ToDateTime(row["rat_date"])
                });
            }

            return CurrencyRates;
        }
    }
}