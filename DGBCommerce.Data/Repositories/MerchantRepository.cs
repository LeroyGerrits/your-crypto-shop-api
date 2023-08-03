using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;
using System.Linq;

namespace DGBCommerce.Data.Repositories
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public MerchantRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public Task<MutationResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Merchant>> Get()
            => await GetRaw(new GetMerchantsParameters());

        public async Task<Merchant> GetById(Guid id)
        {
            var merchants = await GetRaw(new GetMerchantsParameters() { Id = id });
            return merchants.ToList().Single();
        }

        public async Task<Merchant> GetByEmailAddressAndPassword(string emailAddress, string password)
        {
            var merchants = await GetRaw(new GetMerchantsParameters() { EmailAddress = emailAddress, Password = password });
            return merchants.ToList().Single();
        }

        public Task<MutationResult> Insert(Merchant item)
        {
            throw new NotImplementedException();
        }

        public Task<MutationResult> Update(Merchant item)
        {
            throw new NotImplementedException();
        }

        private async Task<IEnumerable<Merchant>> GetRaw(GetMerchantsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetMerchants(parameters);
            List<Merchant> merchants = new();

            foreach (DataRow row in table.Rows)
            {
                merchants.Add(new Merchant()
                {
                    Id = new Guid(row["mer_id"].ToString()!),
                    EmailAddress = Utilities.DbNullableString(row["mer_email_address"]),
                    PasswordSalt = Utilities.DbNullableString(row["mer_password_salt"]),
                    Password = Utilities.DbNullableString(row["mer_password"]),
                    Gender = (Gender)Convert.ToInt32(row["mer_gender"]),
                    FirstName = Utilities.DbNullableString(row["mer_last_name"]),
                    LastName = Utilities.DbNullableString(row["mer_last_name"])
                });
            }

            return merchants;
        }
    }
}
