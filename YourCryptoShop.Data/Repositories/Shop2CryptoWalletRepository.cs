using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Interfaces;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using System.Data;

namespace YourCryptoShop.Data.Repositories
{
    public class Shop2CryptoWalletRepository(IDataAccessLayer dataAccessLayer) : IShop2CryptoWalletRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<Shop2CryptoWallet>> Get(GetShop2CryptoWalletsParameters parameters)
            => await GetRaw(parameters);

        public Task<Shop2CryptoWallet?> GetById(Guid merchantId, Guid id)
            => throw new InvalidOperationException($"{nameof(Shop2CryptoWallet)} objects can not be retrieved by Id.");

        public Task<MutationResult> Create(Shop2CryptoWallet item, Guid mutationId)
            => _dataAccessLayer.CreateShop2CryptoWallet(item, mutationId);

        public Task<MutationResult> Update(Shop2CryptoWallet item, Guid mutationId)
            => throw new InvalidOperationException($"{nameof(Shop2CryptoWallet)} objects can not be updated.");

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => throw new InvalidOperationException($"{nameof(Shop2CryptoWallet)} objects can not be deleted by Id.");

        public Task<MutationResult> Delete(Guid pageId, Guid categoryId, Guid mutationId)
            => _dataAccessLayer.DeleteShop2CryptoWallet(pageId, categoryId, mutationId);

        private async Task<IEnumerable<Shop2CryptoWallet>> GetRaw(GetShop2CryptoWalletsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetShop2CryptoWallets(parameters);
            List<Shop2CryptoWallet> pageCryptoWallets = [];

            foreach (DataRow row in table.Rows)
            {
                pageCryptoWallets.Add(new Shop2CryptoWallet()
                {
                    ShopId = new Guid(row["p2c_page"].ToString()!),
                    CryptoWalletId = new Guid(row["p2c_category"].ToString()!),
                });
            }

            return pageCryptoWallets;
        }
    }
}