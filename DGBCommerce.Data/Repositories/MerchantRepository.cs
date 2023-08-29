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

        public async Task<IEnumerable<Merchant>> Get()
            => await GetRaw(new GetMerchantsParameters());

        public async Task<Merchant?> GetById(Guid id)
        {
            var merchants = await GetRaw(new GetMerchantsParameters() { Id = id });
            return merchants.ToList().SingleOrDefault();
        }

        public async Task<Merchant?> GetByEmailAddress(string emailAddress)
            => await GetRawByEmailAddress(emailAddress);

        public async Task<Merchant?> GetByEmailAddressAndPassword(string emailAddress, string password)
            => await GetRawByEmailAddressAndPassword(emailAddress, password);

        public Task<MutationResult> Create(Merchant item, Guid mutationId)
            => _dataAccessLayer.CreateMerchant(item, mutationId);

        public Task<MutationResult> Update(Merchant item, Guid mutationId)
            => _dataAccessLayer.UpdateMerchant(item, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => throw new NotImplementedException();

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
                    FirstName = Utilities.DbNullableString(row["mer_first_name"]),
                    LastName = Utilities.DbNullableString(row["mer_last_name"])
                });
            }

            return merchants;
        }

        private async Task<Merchant?> GetRawByEmailAddress(string emailAddress)
        {
            DataTable table = await _dataAccessLayer.GetMerchantByEmailAddress(emailAddress);

            if (table.Rows.Count != 1)
                return null;

            DataRow row = table.Rows[0];
            return new Merchant()
            {
                Id = new Guid(row["mer_id"].ToString()!),
                EmailAddress = Utilities.DbNullableString(row["mer_email_address"]),
                PasswordSalt = Utilities.DbNullableString(row["mer_password_salt"]),
                Password = Utilities.DbNullableString(row["mer_password"]),
                Gender = (Gender)Convert.ToInt32(row["mer_gender"]),
                FirstName = Utilities.DbNullableString(row["mer_first_name"]),
                LastName = Utilities.DbNullableString(row["mer_last_name"])
            };
        }

        private async Task<Merchant?> GetRawByEmailAddressAndPassword(string emailAddress, string password)
        {
            DataTable table = await _dataAccessLayer.GetMerchantByEmailAddressAndPassword(emailAddress, password);

            if (table.Rows.Count != 1)
                return null;

            DataRow row = table.Rows[0];
            return new Merchant()
            {
                Id = new Guid(row["mer_id"].ToString()!),
                EmailAddress = Utilities.DbNullableString(row["mer_email_address"]),
                PasswordSalt = Utilities.DbNullableString(row["mer_password_salt"]),
                Password = Utilities.DbNullableString(row["mer_password"]),
                Gender = (Gender)Convert.ToInt32(row["mer_gender"]),
                FirstName = Utilities.DbNullableString(row["mer_first_name"]),
                LastName = Utilities.DbNullableString(row["mer_last_name"])
            };
        }
    }
}
