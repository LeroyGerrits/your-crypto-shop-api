using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGBCommerce.Data.Repositories
{
    public class ShopRepository : IShopRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public ShopRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public Task<MutationResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Shop>> Get()
        {
            DataTable table = await _dataAccessLayer.GetShops(null, null, null);
            List<Shop> shops = new();

            foreach (DataRow row in table.Rows)
            {
                shops.Add(new Shop()
                {
                    Name = DbNullables.NullableString(row["shp_name"]),
                    Merchant = new Merchant()
                    {
                        Id = new Guid(row["shp_merchant"].ToString()!),
                        EmailAddress = DbNullables.NullableString(row["shp_merchant_email_address"]),
                        Password = DbNullables.NullableString(row["shp_merchant_password"]),
                        Gender = (Gender)Convert.ToInt32(row["shp_merchant_gender"]),
                        LastName = DbNullables.NullableString(row["shp_merchant_lastname"]),
                    }
                });
            }

            return shops;
        }

        public Task<Merchant> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Shop>> GetByMerchant(Guid merchantId)
        {
            throw new NotImplementedException();
        }

        public Task<MutationResult> Insert(Shop item)
        {
            throw new NotImplementedException();
        }

        public Task<MutationResult> Update(Shop item)
        {
            throw new NotImplementedException();
        }
    }
}
