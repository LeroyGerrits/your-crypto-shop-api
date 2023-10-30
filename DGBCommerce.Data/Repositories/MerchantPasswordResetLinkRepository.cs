using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class MerchantPasswordResetLinkRepository : IMerchantPasswordResetLinkRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public MerchantPasswordResetLinkRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public Task<MerchantPasswordResetLink?> GetById(Guid merchantId, Guid id)
            => throw new InvalidOperationException();

        public Task<IEnumerable<MerchantPasswordResetLink>> Get(object parameters)
            => throw new InvalidOperationException();

        public async Task<MerchantPasswordResetLink?> GetByIdAndKey(Guid id, string key)
            => await GetRawByIdAndKey(id, key);

        public Task<MutationResult> Create(MerchantPasswordResetLink item, Guid mutationId)
            => _dataAccessLayer.CreateMerchantPasswordResetLink(item);

        public Task<MutationResult> Update(MerchantPasswordResetLink item, Guid mutationId)
            => throw new InvalidOperationException();

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => throw new InvalidOperationException();

        private async Task<MerchantPasswordResetLink?> GetRawByIdAndKey(Guid id, string key)
        {
            DataTable table = await _dataAccessLayer.GetMerchantPasswordResetLinkByIdAndKey(id, key);

            if (table.Rows.Count != 1)
                return null;

            DataRow row = table.Rows[0];
            return new MerchantPasswordResetLink()
            {
                Id = new Guid(row["prl_id"].ToString()!),
                Merchant = new Merchant()
                {
                    Id = new Guid(row["prl_merchant"].ToString()!),                    
                    EmailAddress = Utilities.DbNullableString(row["prl_merchant_email_address"]),
                    Username = Utilities.DbNullableString(row["prl_merchant_username"]),
                    Gender = (Gender)Convert.ToInt32(row["prl_merchant_gender"]),
                    LastName = Utilities.DbNullableString(row["prl_merchant_last_name"]),
                },
                Date = Convert.ToDateTime(row["prl_date"]),
                IpAddress = Utilities.DbNullableString(row["prl_ip_address"]),
                Key = Utilities.DbNullableString(row["prl_key"]),
                Used = Utilities.DBNullableDateTime(row["prl_used"])
            };
        }
    }
}