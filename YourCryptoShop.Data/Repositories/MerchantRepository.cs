using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Enums;
using YourCryptoShop.Domain.Interfaces;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Parameters;
using System.Data;

namespace YourCryptoShop.Data.Repositories
{
    public class MerchantRepository(IDataAccessLayer dataAccessLayer) : IMerchantRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<Merchant>> Get(GetMerchantsParameters parameters)
            => await GetRaw(parameters);

        public async Task<Merchant?> GetById(Guid merchantId, Guid id)
        {
            var merchants = await GetRaw(new GetMerchantsParameters() { Id = id });
            return merchants.ToList().SingleOrDefault();
        }

        public async Task<IEnumerable<PublicMerchant>> GetPublic(GetMerchantsParameters parameters)
            => await GetRawPublic(parameters);

        public async Task<PublicMerchant?> GetByIdPublic(Guid id)
        {
            var merchants = await GetRawPublic(new GetMerchantsParameters() { Id = id });
            return merchants.ToList().SingleOrDefault();
        }

        public async Task<Merchant?> GetByEmailAddress(string emailAddress)
            => await GetRawByEmailAddress(emailAddress);

        public async Task<Merchant?> GetByEmailAddressAndPassword(string emailAddress, string password, string? ipAddress)
            => await GetRawByEmailAddressAndPassword(emailAddress, password, ipAddress);

        public async Task<Merchant?> GetByIdAndPassword(Guid id, string password)
            => await GetRawByIdAndPassword(id, password);

        public Task<MutationResult> Create(Merchant item, Guid mutationId)
            => _dataAccessLayer.CreateMerchant(item, mutationId);

        public Task<MutationResult> Update(Merchant item, Guid mutationId)
            => _dataAccessLayer.UpdateMerchant(item, mutationId);

        public Task<MutationResult> UpdatePasswordAndSalt(Merchant item, string password, string passwordSalt, Guid mutationId)
            => _dataAccessLayer.UpdateMerchantPasswordAndSalt(item, password, passwordSalt, mutationId);

        public Task<MutationResult> UpdatePasswordAndActivate(Merchant item, string password, Guid mutationId)
            => _dataAccessLayer.UpdateMerchantPasswordAndActivate(item, password, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => throw new NotImplementedException();

        private async Task<IEnumerable<Merchant>> GetRaw(GetMerchantsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetMerchants(parameters);
            List<Merchant> merchants = [];

            foreach (DataRow row in table.Rows)
            {
                merchants.Add(new Merchant()
                {
                    Id = new Guid(row["mer_id"].ToString()!),
                    Activated = Utilities.DBNullableDateTime(row["mer_activated"]),
                    EmailAddress = Utilities.DbNullableString(row["mer_email_address"]),
                    Username = Utilities.DbNullableString(row["mer_username"]),
                    PasswordSalt = Utilities.DbNullableString(row["mer_password_salt"]),
                    Gender = (Gender)Convert.ToInt32(row["mer_gender"]),
                    FirstName = Utilities.DbNullableString(row["mer_first_name"]),
                    LastName = Utilities.DbNullableString(row["mer_last_name"]),
                    LastLogin = Utilities.DBNullableDateTime(row["mer_last_login"]),
                    LastIpAddress = Utilities.DbNullableString(row["mer_last_ip_address"]),
                    SecondLastLogin = Utilities.DBNullableDateTime(row["mer_second_last_login"]),
                    SecondLastIpAddress = Utilities.DbNullableString(row["mer_second_last_ip_address"])
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
                Activated = Utilities.DBNullableDateTime(row["mer_activated"]),
                EmailAddress = Utilities.DbNullableString(row["mer_email_address"]),
                Username = Utilities.DbNullableString(row["mer_username"]),
                PasswordSalt = Utilities.DbNullableString(row["mer_password_salt"]),
                Gender = (Gender)Convert.ToInt32(row["mer_gender"]),
                FirstName = Utilities.DbNullableString(row["mer_first_name"]),
                LastName = Utilities.DbNullableString(row["mer_last_name"])
            };
        }

        private async Task<Merchant?> GetRawByEmailAddressAndPassword(string emailAddress, string password, string? ipAddress)
        {
            DataTable table = await _dataAccessLayer.GetMerchantByEmailAddressAndPassword(emailAddress, password, ipAddress);

            if (table.Rows.Count != 1)
                return null;

            DataRow row = table.Rows[0];
            return new Merchant()
            {
                Id = new Guid(row["mer_id"].ToString()!),
                Activated = Utilities.DBNullableDateTime(row["mer_activated"]),
                EmailAddress = Utilities.DbNullableString(row["mer_email_address"]),
                Username = Utilities.DbNullableString(row["mer_username"]),
                Gender = (Gender)Convert.ToInt32(row["mer_gender"]),
                FirstName = Utilities.DbNullableString(row["mer_first_name"]),
                LastName = Utilities.DbNullableString(row["mer_last_name"]),
                LastLogin = Utilities.DBNullableDateTime(row["mer_last_login"]),
                LastIpAddress = Utilities.DbNullableString(row["mer_last_ip_address"]),
                SecondLastLogin = Utilities.DBNullableDateTime(row["mer_second_last_login"]),
                SecondLastIpAddress = Utilities.DbNullableString(row["mer_second_last_ip_address"])
            };
        }

        private async Task<Merchant?> GetRawByIdAndPassword(Guid id, string password)
        {
            DataTable table = await _dataAccessLayer.GetMerchantByIdAndPassword(id, password);

            if (table.Rows.Count != 1)
                return null;

            DataRow row = table.Rows[0];
            return new Merchant()
            {
                Id = new Guid(row["mer_id"].ToString()!),
                Activated = Utilities.DBNullableDateTime(row["mer_activated"]),
                EmailAddress = Utilities.DbNullableString(row["mer_email_address"]),
                Username = Utilities.DbNullableString(row["mer_username"]),
                Password = Utilities.DbNullableString(row["mer_password"]),
                PasswordSalt = Utilities.DbNullableString(row["mer_password_salt"]),
                Gender = (Gender)Convert.ToInt32(row["mer_gender"]),
                FirstName = Utilities.DbNullableString(row["mer_first_name"]),
                LastName = Utilities.DbNullableString(row["mer_last_name"])
            };
        }

        private async Task<IEnumerable<PublicMerchant>> GetRawPublic(GetMerchantsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetMerchants(parameters);
            List<PublicMerchant> merchants = [];

            foreach (DataRow row in table.Rows)
            {
                merchants.Add(new PublicMerchant()
                {
                    Id = new Guid(row["mer_id"].ToString()!),
                    Username = Utilities.DbNullableString(row["mer_username"]),
                    Score = Utilities.DbNullableDecimal(row["mer_score"])
                });
            }

            return merchants;
        }
    }
}