using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
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

        public async Task<MerchantPasswordResetLink?> GetById(Guid id)
            => throw new InvalidOperationException();

        public async Task<IEnumerable<MerchantPasswordResetLink>> Get()
            => throw new InvalidOperationException();

        public async Task<IEnumerable<MerchantPasswordResetLink>> GetByIdAndKey(Guid id, string Key)
            => throw new InvalidOperationException();

        public Task<MutationResult> Create(MerchantPasswordResetLink item, Guid mutationId)
            => _dataAccessLayer.CreateMerchantPasswordResetLink(item);

        public Task<MutationResult> Update(MerchantPasswordResetLink item, Guid mutationId)
            => throw new InvalidOperationException();

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => throw new InvalidOperationException();

        /*private async Task<IEnumerable<MerchantPasswordResetLink>> GetRaw(GetMerchantPasswordResetLinksParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetMerchantPasswordResetLinks(parameters);
            List<MerchantPasswordResetLink> deliveryMethods = new();

            foreach (DataRow row in table.Rows)
            {
                deliveryMethods.Add(new()
                {
                    Id = new Guid(row["dlm_id"].ToString()!),
                    Shop = new Shop()
                    {
                        Id = new Guid(row["dlm_shop"].ToString()!),
                        Name = Utilities.DbNullableString(row["dlm_shop_name"]),
                        Merchant = new Merchant()
                        {
                            Id = new Guid(row["dlm_shop_merchant"].ToString()!),
                            EmailAddress = Utilities.DbNullableString(row["dlm_shop_merchant_email_address"]),
                            Gender = (Gender)Convert.ToInt32(row["dlm_shop_merchant_gender"]),
                            LastName = Utilities.DbNullableString(row["dlm_shop_merchant_last_name"]),
                        }
                    },
                    Name = Utilities.DbNullableString(row["dlm_name"]),
                    Costs = Utilities.DbNullableDecimal(row["dlm_costs"])
                });
            }

            return deliveryMethods;
        }*/
    }
}