using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public CountryRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public async Task<IEnumerable<Country>> Get(GetCountriesParameters parameters)
            => await GetRaw(parameters);

        public async Task<Country?> GetById(Guid id)
        {
            var ctrrencies = await GetRaw(new GetCountriesParameters() { Id = id });
            return ctrrencies.ToList().SingleOrDefault();
        }

        private async Task<IEnumerable<Country>> GetRaw(GetCountriesParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetCountries(parameters);
            List<Country> ctrrencies = new();

            foreach (DataRow row in table.Rows)
            {
                ctrrencies.Add(new()
                {
                    Id = new Guid(row["ctr_id"].ToString()!),
                    Code = Utilities.DbNullableString(row["ctr_code"]),
                    Name = Utilities.DbNullableString(row["ctr_name"])
                });
            }

            return ctrrencies;
        }
    }
}