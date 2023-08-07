using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class DeliveryMethodRepository : IDeliveryMethodRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public DeliveryMethodRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public Task<MutationResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DeliveryMethod>> Get()
        {
            DataTable table = await _dataAccessLayer.GetDeliveryMethods(new GetDeliveryMethodsParameters());
            List<DeliveryMethod> faqcategories = new();

            foreach (DataRow row in table.Rows)
            {
                faqcategories.Add(new()
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

            return faqcategories;
        }

        public async Task<DeliveryMethod?> GetById(Guid id)
        {
            var deliveryMethods = await GetRaw(new GetDeliveryMethodsParameters() { Id = id });
            return deliveryMethods.ToList().SingleOrDefault();
        }

        public Task<IEnumerable<DeliveryMethod>> GetByMerchant(Guid merchantId)
        {
            throw new NotImplementedException();
        }

        public Task<MutationResult> Insert(DeliveryMethod item)
        {
            throw new NotImplementedException();
        }

        public Task<MutationResult> Update(DeliveryMethod item)
        {
            throw new NotImplementedException();
        }

        private async Task<IEnumerable<DeliveryMethod>> GetRaw(GetDeliveryMethodsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetDeliveryMethods(parameters);
            List<DeliveryMethod> deliveryMethods = new();

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
        }
    }
}
