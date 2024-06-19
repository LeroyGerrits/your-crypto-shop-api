using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Interfaces;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using System.Data;

namespace YourCryptoShop.Data.Repositories
{
    public class CryptoWalletRepository(IDataAccessLayer dataAccessLayer) : ICryptoWalletRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<CryptoWallet>> Get(GetCryptoWalletsParameters parameters)
            => await GetRaw(parameters);

        public async Task<CryptoWallet?> GetById(Guid merchantId, Guid id)
        {
            var deliveryMethods = await GetRaw(new GetCryptoWalletsParameters() { MerchantId = merchantId, Id = id });
            return deliveryMethods.ToList().SingleOrDefault();
        }

        public Task<MutationResult> Create(CryptoWallet item, Guid mutationId)
            => _dataAccessLayer.CreateCryptoWallet(item, mutationId);

        public Task<MutationResult> Update(CryptoWallet item, Guid mutationId)
            => _dataAccessLayer.UpdateCryptoWallet(item, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => _dataAccessLayer.DeleteCryptoWallet(id, mutationId);

        private async Task<IEnumerable<CryptoWallet>> GetRaw(GetCryptoWalletsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetCryptoWallets(parameters);
            List<CryptoWallet> deliveryMethods = [];

            foreach (DataRow row in table.Rows)
            {
                deliveryMethods.Add(new()
                {
                    Id = new Guid(row["crw_id"].ToString()!),
                    MerchantId = new Guid(row["crw_merchant"].ToString()!),
                    CurrencyId = new Guid(row["crw_currency"].ToString()!),
                    Name = Utilities.DbNullableString(row["crw_name"]),
                    Address = Utilities.DbNullableString(row["crw_address"])
                });
            }

            return deliveryMethods;
        }
    }
}