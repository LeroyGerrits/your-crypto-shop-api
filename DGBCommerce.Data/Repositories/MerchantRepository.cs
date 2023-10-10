using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public MerchantRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

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
                    Username = Utilities.DbNullableString(row["mer_username"]),
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
                Username = Utilities.DbNullableString(row["mer_username"]),
                EmailAddress = Utilities.DbNullableString(row["mer_email_address"]),
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

        private async Task<IEnumerable<PublicMerchant>> GetRawPublic(GetMerchantsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetMerchants(parameters);
            List<PublicMerchant> merchants = new();

            foreach (DataRow row in table.Rows)
            {
                merchants.Add(new PublicMerchant()
                {
                    Id = new Guid(row["mer_id"].ToString()!),
                    Username = Utilities.DbNullableString(row["mer_username"]),
                    Gender = (Gender)Convert.ToInt32(row["mer_gender"]),
                    FirstName = Utilities.DbNullableString(row["mer_first_name"]),
                    LastName = Utilities.DbNullableString(row["mer_last_name"]),
                    Score = Utilities.DbNullableDecimal(row["mer_score"])
                });
            }

            return merchants;
        }
    }
}